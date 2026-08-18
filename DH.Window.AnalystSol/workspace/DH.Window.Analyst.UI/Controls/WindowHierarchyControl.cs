//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Automation;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Overlay;

namespace DH.Window.Analyst.UI.Controls {
	// reusable child-hierarchy tree rooted at one window: Win32 native or UIA element children, toggled at runtime, lazily loaded per node so a deep tree never blocks the UI thread up front
	public partial class WindowHierarchyControl : UserControl {
		private const string DUMMY_NODE_KEY = "__dummy";

		private IWindowTreeService m_treeService;
		private TopLevelWindowItem m_rootItem;
		private bool m_bUiaMode;
		private readonly HighlightOverlayForm m_overlayHighlight = new HighlightOverlayForm();

		// Sync mode: hover-follow scoped to this tab's own root subtree only (never a desktop-wide poll), so cost stays bounded
		// regardless of how many other tabs/windows are open — see m_timerSync.Tick => OnSyncTick
		private readonly Timer m_timerSync = new Timer { Interval = 50 };
		private IntPtr m_handleLastSyncTarget;
		private AutomationElement m_elementLastSyncTarget;

		// last raw cursor position seen by OnSyncTick, used to detect "mouse still moving" and skip the expensive
		// resolution entirely while it is — see OnSyncTick
		private Point m_pointLastCursorTick;

		// while Sync is on, a left-click anywhere freezes the tree on whatever is currently synced and turns Sync back off —
		// same swallow-the-click "pick and stop" convention as MousePickerCoordinator's "Show Info on Mouse" mode, using a
		// separate hook instance scoped to this tab's own Sync session rather than the app-wide picker's shared one
		private readonly IGlobalPickerHookService m_hookSyncClick = new GlobalPickerHookService();

		// passes the TreeNode itself (not just Tag) so the caller can walk the ancestor chain for a breadcrumb
		public event EventHandler<TreeNode> NodeSelected;

		// raised after ToggleMode() flips the tree source, so an external toolbar (WindowWorkspaceTabControl) can refresh its button text
		public event EventHandler ModeChanged;

		// raised when a Sync click (or Esc) turns Sync off from inside this control, so the toolbar's Sync checkbox (owned by
		// WindowWorkspaceTabControl) can update to match without that control having to poll or reach into m_timerSync itself
		public event EventHandler SyncStoppedExternally;

		public TreeNode SelectedNode {
			get { return m_trvHierarchy.SelectedNode; }
		}

		public bool UiaMode {
			get { return m_bUiaMode; }
		}

		public WindowHierarchyControl() {
			InitializeComponent();

			m_trvHierarchy.BeforeExpand += OnBeforeExpand;
			m_trvHierarchy.AfterSelect += (sender, e) => NodeSelected?.Invoke(this, e.Node);

			m_timerSync.Tick += OnSyncTick;

			m_hookSyncClick.Picked += OnSyncClickPicked;
			m_hookSyncClick.Cancelled += OnSyncClickCancelled;
		}

		// flips between the Win32 and UIA tree sources for this tab, triggered explicitly (toolbar toggle button in WindowWorkspaceTabControl)
		public void ToggleMode() {
			m_bUiaMode = !m_bUiaMode;
			LoadRoot(m_rootItem);
			ModeChanged?.Invoke(this, EventArgs.Empty);
		}

		// starts/stops hover-follow sync, triggered explicitly (Sync toggle button in WindowWorkspaceTabControl)
		public void SetSyncEnabled(bool benabled) {
			if (benabled == true) {
				m_handleLastSyncTarget = IntPtr.Zero;
				m_elementLastSyncTarget = null;
				m_pointLastCursorTick = Cursor.Position;
				m_timerSync.Start();

				if (m_hookSyncClick.Start() == false) {
					AppLog.w($"WindowHierarchyControl: Sync click-hook failed to start ({m_hookSyncClick.LastError})");
				}
			}
			else {
				m_timerSync.Stop();
				m_hookSyncClick.Stop();
				ClearSyncHighlight();
			}
		}

		// left-click while Sync is on: resolve once more at the exact click point (not whichever position the last 50ms
		// tick happened to sample), freeze the tree/overlay there, then turn Sync off — mirrors "Show Info on Mouse"'s
		// pick-and-stop convention. The hook swallows the click, so the target app never sees it (consistent with that
		// existing mode; Sync is a developer-toggled inspection mode, not meant to also relay clicks through to the target)
		private void OnSyncClickPicked(object sender, Point ptscreen) {
			if (m_timerSync.Enabled == false) {
				return;
			}

			SyncAt(ptscreen);
			StopSyncFromExternalTrigger();
		}

		private void OnSyncClickCancelled(object sender, EventArgs e) {
			if (m_timerSync.Enabled == false) {
				return;
			}

			StopSyncFromExternalTrigger();
		}

		private void StopSyncFromExternalTrigger() {
			m_timerSync.Stop();
			m_hookSyncClick.Stop();
			SyncStoppedExternally?.Invoke(this, EventArgs.Empty);
		}

		// flashes the highlight overlay over the currently selected node, triggered explicitly (Highlight button)
		public void HighlightSelected() {
			m_overlayHighlight.FlashAt(GetNodeScreenRect(m_trvHierarchy.SelectedNode));
		}

		// brings the top-level window owning the currently selected node to the foreground, triggered explicitly (Foreground button)
		public void ActivateSelected() {
			if (m_treeService == null) {
				return;
			}

			TreeNode node = m_trvHierarchy.SelectedNode;
			if (node == null) {
				return;
			}

			// Win32 nodes carry their own handle — activate it directly rather than hit-testing a screen point, which picks up whatever window is on top when the target is occluded
			if (node.Tag is TopLevelWindowItem windowitem) {
				m_treeService.ActivateWindow(m_treeService.GetTopLevelAncestorHandle(windowitem.Handle));
				return;
			}

			Rectangle rectnode = GetNodeScreenRect(node);
			if (rectnode.IsEmpty == true) {
				return;
			}

			Point pointcenter = new Point(rectnode.X + rectnode.Width / 2, rectnode.Y + rectnode.Height / 2);
			TopLevelWindowItem topitem = m_treeService.GetTopLevelWindowAtScreenPoint(pointcenter);
			if (topitem != null) {
				m_treeService.ActivateWindow(topitem.Handle);
			}
		}

		private Rectangle GetNodeScreenRect(TreeNode node) {
			if (node == null) {
				return Rectangle.Empty;
			}

			if (node.Tag is TopLevelWindowItem windowitem && m_treeService != null) {
				return m_treeService.GetWindowScreenRect(windowitem.Handle);
			}

			if (node.Tag is AutomationElement element) {
				try {
					System.Windows.Rect rect = element.Current.BoundingRectangle;
					if (rect.IsEmpty == false) {
						return new Rectangle((int) rect.X, (int) rect.Y, (int) rect.Width, (int) rect.Height);
					}
				}
				catch (Exception) {
					// unresponsive/restricted elements yield no highlight
				}
			}

			return Rectangle.Empty;
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
		}

		// used by the breadcrumb to jump back up to an ancestor node
		public void FocusNode(TreeNode node) {
			m_trvHierarchy.SelectedNode = node;
		}

		public void LoadRoot(TopLevelWindowItem rootitem) {
			m_rootItem = rootitem;
			m_handleLastSyncTarget = IntPtr.Zero;
			m_elementLastSyncTarget = null;

			m_trvHierarchy.Nodes.Clear();
			if (rootitem == null || m_treeService == null) {
				return;
			}

			TreeNode noderoot = m_bUiaMode == false
				? CreateWin32Node(rootitem)
				: CreateElementNode(m_treeService.CreateElementFromHandle(rootitem.Handle));

			m_trvHierarchy.Nodes.Add(noderoot);
			noderoot.Expand();

			// select immediately so Highlight/Foreground have a target right after opening a tab, not only after the user clicks a node
			m_trvHierarchy.SelectedNode = noderoot;
		}

		// selects the tree node for the given descendant handle (Show Info on Mouse global click), expanding ancestors one hop at a time so lazy
		// child loading (OnBeforeExpand) populates each level along the way; Win32-mode tree only — the click target is always a raw HWND, which
		// matches the Win32 tree's TopLevelWindowItem nodes directly, so UIA mode (a different node/Tag shape) is out of scope here and returns false
		public bool SelectDescendantByHandle(IntPtr handle) {
			if (m_bUiaMode == true || m_treeService == null || m_rootItem == null || m_trvHierarchy.Nodes.Count == 0) {
				return false;
			}

			TreeNode noderoot = m_trvHierarchy.Nodes[0];

			// GetAncestorChainToRoot returns an empty chain both when handle is unreachable AND when handle IS the root
			// itself (0 hops) — the disabled-inclusive hit test in OnSyncTick can legitimately resolve straight to the
			// root (e.g. cursor over the root's own client area with no deeper child there), so that case must be
			// handled before treating an empty chain as a failure
			if (handle == m_rootItem.Handle) {
				m_trvHierarchy.SelectedNode = noderoot;
				noderoot.EnsureVisible();
				return true;
			}

			IReadOnlyList<IntPtr> listchain = m_treeService.GetAncestorChainToRoot(handle, m_rootItem.Handle);
			if (listchain.Count == 0) {
				return false;
			}

			TreeNode nodecurrent = noderoot;
			foreach (IntPtr handlehop in listchain) {
				nodecurrent.Expand();

				TreeNode nodenext = FindWin32ChildNode(nodecurrent, handlehop);
				if (nodenext == null && nodecurrent.Tag is TopLevelWindowItem itemparent) {
					// cached children reflect whatever existed when this node was first expanded — a window created afterward
					// (e.g. spotted live via Sync) won't be there yet, so re-enumerate once before giving up on this hop
					RefreshWin32ChildNodes(nodecurrent, itemparent);
					nodenext = FindWin32ChildNode(nodecurrent, handlehop);
				}

				if (nodenext == null) {
					return false;
				}

				nodecurrent = nodenext;
			}

			m_trvHierarchy.SelectedNode = nodecurrent;
			nodecurrent.EnsureVisible();
			return true;
		}

		private static TreeNode FindWin32ChildNode(TreeNode nodeparent, IntPtr handlehop) {
			foreach (TreeNode nodechild in nodeparent.Nodes) {
				if (nodechild.Tag is TopLevelWindowItem item && item.Handle == handlehop) {
					return nodechild;
				}

				// owned windows live one level deeper, under the synthetic "Owned Windows" group node
				if (nodechild.Tag == s_tagOwnedWindowsGroup) {
					TreeNode nodefound = FindWin32ChildNode(nodechild, handlehop);
					if (nodefound != null) {
						return nodefound;
					}
				}
			}
			return null;
		}

		private void RefreshWin32ChildNodes(TreeNode nodeparent, TopLevelWindowItem itemparent) {
			nodeparent.Nodes.Clear();
			PopulateWin32Children(nodeparent, itemparent.Handle);
		}

		// adds this handle's real WS_CHILD descendants directly, then — if any exist — a separate "Owned Windows" group
		// node holding the modal dialogs/tooltips/IME windows it owns, so the tree never implies a parent-child
		// relationship between a window and something it merely owns
		private void PopulateWin32Children(TreeNode nodeparent, IntPtr handle) {
			foreach (TopLevelWindowItem child in m_treeService.GetChildWindowInfos(handle)) {
				nodeparent.Nodes.Add(CreateWin32Node(child));
			}

			List<TopLevelWindowItem> listowned = new List<TopLevelWindowItem>(m_treeService.GetOwnedWindowInfos(handle));
			if (listowned.Count == 0) {
				return;
			}

			TreeNode nodegroup = new TreeNode($"Owned Windows ({listowned.Count})") {
				Tag = s_tagOwnedWindowsGroup,
				ForeColor = s_colorOwnedWindow,
				ToolTipText = "Separate top-level windows owned by this window (modal dialogs, tooltips, IME windows) — not real child controls"
			};
			foreach (TopLevelWindowItem owned in listowned) {
				nodegroup.Nodes.Add(CreateWin32Node(owned));
			}
			nodeparent.Nodes.Add(nodegroup);
		}

		private static TreeNode FindElementChildNode(TreeNode nodeparent, AutomationElement elementhop) {
			foreach (TreeNode nodechild in nodeparent.Nodes) {
				if (nodechild.Tag is AutomationElement elementchild && Automation.Compare(elementchild, elementhop) == true) {
					return nodechild;
				}
			}
			return null;
		}

		private void RefreshElementChildNodes(TreeNode nodeparent, AutomationElement elementparent) {
			nodeparent.Nodes.Clear();
			foreach (AutomationElement childelement in m_treeService.GetChildren(elementparent)) {
				nodeparent.Nodes.Add(CreateElementNode(childelement));
			}
		}

		// selects the tree node for the given descendant UIA element (Sync hover), expanding ancestors one hop at a time so lazy child
		// loading (OnBeforeExpand) populates each level along the way; UIA-mode tree only, the counterpart to SelectDescendantByHandle
		public bool SelectDescendantElement(AutomationElement element) {
			TreeNode node = FindElementChainNode(element);
			if (node == null) {
				return false;
			}

			m_trvHierarchy.SelectedNode = node;
			node.EnsureVisible();
			return true;
		}

		// ancestor-chain walk (RawView, from element up to root) resolved down to a tree node; null if element isn't
		// actually a descendant of the root element or the chain can't be matched to cached/refreshed tree nodes
		private TreeNode FindElementChainNode(AutomationElement element) {
			if (m_bUiaMode == false || m_treeService == null || m_rootItem == null || m_trvHierarchy.Nodes.Count == 0 || element == null) {
				return null;
			}

			TreeNode noderoot = m_trvHierarchy.Nodes[0];
			if (!(noderoot.Tag is AutomationElement elementroot)) {
				return null;
			}

			if (Automation.Compare(element, elementroot) == true) {
				return noderoot;
			}

			IReadOnlyList<AutomationElement> listchain = m_treeService.GetElementAncestorChainToRoot(element, elementroot);
			if (listchain.Count == 0) {
				return null;
			}

			TreeNode nodecurrent = noderoot;
			foreach (AutomationElement elementhop in listchain) {
				nodecurrent.Expand();

				TreeNode nodenext = FindElementChildNode(nodecurrent, elementhop);
				if (nodenext == null && nodecurrent.Tag is AutomationElement elementparent) {
					// same rationale as the Win32 path: an element created after this node's first expand (a new popup/control
					// that appeared while hovering) won't be in the cached children yet, so re-enumerate once before giving up
					RefreshElementChildNodes(nodecurrent, elementparent);
					nodenext = FindElementChildNode(nodecurrent, elementhop);
				}

				if (nodenext == null) {
					return null;
				}

				nodecurrent = nodenext;
			}

			return nodecurrent;
		}

		// fallback/complement to FindElementChainNode for when the ancestor-chain walk lands on the wrong branch entirely —
		// Chrome's GPU compositor exposes a plain "Intermediate D3D Window" surface pane as a sibling of the real content
		// pane, and that surface pane can share the same screen-space bounding rect as its sibling (both cover the visible
		// content area), so FromPoint/RawView-ancestor-walk can resolve to the dead-end surface pane instead of the real
		// content. Descending from the root through every candidate child whose rect contains the point (not just the first
		// match) and keeping whichever branch reaches the greatest tree depth routes around dead-end siblings like that
		private TreeNode FindDeepestPointNode(Point pointscreen) {
			if (m_bUiaMode == false || m_trvHierarchy.Nodes.Count == 0) {
				return null;
			}

			TreeNode noderoot = m_trvHierarchy.Nodes[0];
			return FindDeepestPointNode(noderoot, pointscreen) ?? noderoot;
		}

		private TreeNode FindDeepestPointNode(TreeNode nodeparent, Point pointscreen) {
			EnsureElementChildrenLoaded(nodeparent);

			TreeNode nodebest = null;

			foreach (TreeNode nodechild in nodeparent.Nodes) {
				if (!(nodechild.Tag is AutomationElement elementchild)) {
					continue;
				}

				try {
					if (elementchild.Current.BoundingRectangle.Contains(pointscreen.X, pointscreen.Y) == false) {
						continue;
					}
				}
				catch (ElementNotAvailableException) {
					// stale reference from a live UI update between enumeration and this read; skip it
					continue;
				}

				TreeNode nodecandidate = FindDeepestPointNode(nodechild, pointscreen) ?? nodechild;
				if (nodebest == null || nodecandidate.Level > nodebest.Level) {
					nodebest = nodecandidate;
				}
			}

			return nodebest;
		}

		private void EnsureElementChildrenLoaded(TreeNode node) {
			if (node.Nodes.Count == 1 && node.Nodes[0].Name == DUMMY_NODE_KEY && node.Tag is AutomationElement elementparent) {
				RefreshElementChildNodes(node, elementparent);
			}
		}

		// heuristic for Chrome's GPU-compositor surface pane and similar dead-end nodes: a disabled element can still be a
		// "valid" chain match, but real interactive content is never disabled, so this flags exactly the case that needs
		// the expensive point-based re-search in SyncAt
		private static bool IsLikelyDeadEndElement(AutomationElement element) {
			try {
				return element.Current.IsEnabled == false;
			}
			catch (ElementNotAvailableException) {
				return false;
			}
		}

		private static readonly int s_nOwnProcessId = Process.GetCurrentProcess().Id;

		// true if element belongs to this app's own process (e.g. the Sync overlay window) rather than the inspected target
		private static bool IsOwnProcessElement(AutomationElement element) {
			try {
				return element.Current.ProcessId == s_nOwnProcessId;
			}
			catch (ElementNotAvailableException) {
				return false;
			}
		}


		// scoped to this tab's own window family — its root window plus anything it owns (modal dialogs, tooltips), never a
		// desktop-wide poll; the target under the cursor not belonging to that family (or being unchanged since the last tick)
		// does no tree work at all, which is what keeps Sync mode cheap even though it polls at the same 50ms cadence as the
		// global picker. Deliberately hit-tests first and checks ownership via GetTopLevelAncestorHandle (GA_ROOTOWNER) rather
		// than pre-filtering by the root window's own screen rect — an owned dialog is frequently positioned outside that rect
		// (e.g. centered on the whole screen), so a rect-based gate would silently drop it before the ownership check ever ran
		private void OnSyncTick(object sender, EventArgs e) {
			Point pointcursor = Cursor.Position;

			// SyncAt's live UIA/Win32 hit-testing can take long enough (Chrome's cross-process UIA calls especially) to
			// delay this thread's WH_MOUSE_LL hook callback (m_hookSyncClick runs on this same UI thread), which Windows
			// perceives as system-wide mouse lag while the hook is installed — so only pay that cost once the cursor has
			// actually settled (unchanged since the previous 50ms tick), never while it's still moving between ticks
			if (pointcursor != m_pointLastCursorTick) {
				m_pointLastCursorTick = pointcursor;
				return;
			}

			SyncAt(pointcursor);
		}

		private void SyncAt(Point pointscreen) {
			if (m_treeService == null || m_rootItem == null) {
				return;
			}

			if (m_bUiaMode == false) {
				TopLevelWindowItem itemhover = m_treeService.GetWindowAtScreenPoint(pointscreen);
				if (itemhover == null || m_treeService.GetTopLevelAncestorHandle(itemhover.Handle) != m_rootItem.Handle) {
					// WindowFromPoint silently skips disabled windows, so it misses the root's own controls while a modal dialog
					// (owned by the root) has disabled it — retry with a hit test rooted at the root itself, which doesn't skip
					// disabled windows, but only when the cursor is actually within the root's bounds (this fallback has no
					// owner-chain check of its own, since it can only ever resolve to a descendant of roothandle by construction)
					Rectangle rectroot = m_treeService.GetWindowScreenRect(m_rootItem.Handle);
					itemhover = rectroot.Contains(pointscreen) == true
						? m_treeService.GetDescendantAtScreenPointIncludingDisabled(m_rootItem.Handle, pointscreen)
						: null;

					if (itemhover == null) {
						ClearSyncHighlight();
						return;
					}
				}

				if (itemhover.Handle == m_handleLastSyncTarget) {
					return;
				}

				m_handleLastSyncTarget = itemhover.Handle;

				if (SelectDescendantByHandle(itemhover.Handle) == true) {
					m_overlayHighlight.ShowSteadyAt(m_treeService.GetWindowScreenRect(itemhover.Handle));
				}
				else {
					m_overlayHighlight.HideSteady();
				}
			}
			else {
				AutomationElement elementhover = m_treeService.GetElementAtScreenPoint(pointscreen);

				// the overlay itself is a TopMost window that can sit directly under the cursor (it's still showing the
				// previous tick's target rect while this tick resolves the next one) — WS_EX_TRANSPARENT makes Win32's
				// WindowFromPoint skip it, but UI Automation's own point hit test does not reliably do the same, so FromPoint
				// can resolve to our own overlay's element instead of whatever is really under the cursor. Rather than hiding
				// the overlay each tick (visibly flickers), just discard a hit that belongs to our own process and fall
				// through to the disabled-inclusive Win32 fallback below, which walks descendants of m_rootItem only and so
				// can never resolve to the overlay (a separate top-level window, not a descendant of the inspected root)
				if (elementhover != null && IsOwnProcessElement(elementhover) == true) {
					elementhover = null;
				}

				if (elementhover == null) {
					// same disabled-window blind spot as the Win32 branch (UIA's point hit test is backed by WindowFromPoint for
					// HWND-based elements too) — fall back to the disabled-inclusive Win32 hit test, then wrap whatever HWND it
					// finds as a UIA element; this only reaches the nearest HWND-bearing container, not a deeper UIA-only leaf,
					// but that's still a correct (if coarser) sync target instead of nothing
					Rectangle rectroot = m_treeService.GetWindowScreenRect(m_rootItem.Handle);
					TopLevelWindowItem itemfallback = rectroot.Contains(pointscreen) == true
						? m_treeService.GetDescendantAtScreenPointIncludingDisabled(m_rootItem.Handle, pointscreen)
						: null;
					elementhover = itemfallback != null ? m_treeService.CreateElementFromHandle(itemfallback.Handle) : null;
				}

				if (elementhover == null) {
					ClearSyncHighlight();
					return;
				}

				if (m_elementLastSyncTarget != null && Automation.Compare(elementhover, m_elementLastSyncTarget) == true) {
					return;
				}

				m_elementLastSyncTarget = elementhover;

				TreeNode nodechain = FindElementChainNode(elementhover);

				// FindDeepestPointNode routes around a dead-end sibling branch (e.g. Chrome's GPU-compositor "Intermediate
				// D3D Window" pane) by force-loading every rect-matching sibling via live UIA calls — necessary for that one
				// case, but too expensive to pay on every tick just because the mouse moved (Chrome's UIA child enumeration
				// alone can take tens of ms). The dead-end pane is always disabled/non-interactive, so only re-run the
				// expensive point-based search when the chain result is missing or looks like exactly that case
				TreeNode nodetarget = nodechain;
				if (nodechain == null || (nodechain.Tag is AutomationElement elementchain && IsLikelyDeadEndElement(elementchain) == true)) {
					TreeNode nodepoint = FindDeepestPointNode(pointscreen);
					if (nodepoint != null && (nodetarget == null || nodepoint.Level > nodetarget.Level)) {
						nodetarget = nodepoint;
					}
				}

				if (nodetarget != null) {
					m_trvHierarchy.SelectedNode = nodetarget;
					nodetarget.EnsureVisible();
					m_overlayHighlight.ShowSteadyAt(GetNodeScreenRect(m_trvHierarchy.SelectedNode));
				}
				else {
					m_overlayHighlight.HideSteady();
				}
			}
		}

		private void ClearSyncHighlight() {
			m_handleLastSyncTarget = IntPtr.Zero;
			m_elementLastSyncTarget = null;
			m_overlayHighlight.HideSteady();
		}

		private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e) {
			TreeNode nodeparent = e.Node;
			if (nodeparent.Nodes.Count != 1 || nodeparent.Nodes[0].Name != DUMMY_NODE_KEY) {
				return;
			}

			nodeparent.Nodes.Clear();

			if (m_bUiaMode == false && nodeparent.Tag is TopLevelWindowItem windowitem) {
				PopulateWin32Children(nodeparent, windowitem.Handle);
			}
			else if (m_bUiaMode == true && nodeparent.Tag is AutomationElement element) {
				foreach (AutomationElement childelement in m_treeService.GetChildren(element)) {
					nodeparent.Nodes.Add(CreateElementNode(childelement));
				}
			}
		}

		// owned windows (modal dialogs, tooltips, IME windows — see IsOwnedWindow) are grouped under a synthetic "Owned
		// Windows" node (see PopulateWin32Children / s_tagOwnedWindowsGroup) rather than mixed in with real children;
		// they still get this color so they read as distinct even inside that group
		private static readonly Color s_colorOwnedWindow = Color.FromArgb(150, 90, 0);

		// marks the synthetic "Owned Windows" grouping node, which has no real HWND of its own — distinguishes it from
		// real TreeNode.Tag values (TopLevelWindowItem) when walking the tree
		private static readonly object s_tagOwnedWindowsGroup = new object();

		private TreeNode CreateWin32Node(TopLevelWindowItem item) {
			string strtext = string.IsNullOrEmpty(item.Title) == false ? item.Title : item.ClassName;
			TreeNode node = new TreeNode(strtext) {
				Tag = item,
				ToolTipText = item.IsOwnedWindow == false
					? $"{strtext}\r\nClass: {item.ClassName}\r\nHandle: {item.HandleText}"
					: $"{strtext}\r\nClass: {item.ClassName}\r\nHandle: {item.HandleText}\r\n(Owned window — a separate top-level window, not a child of its parent)"
			};
			if (item.IsOwnedWindow == true) {
				node.ForeColor = s_colorOwnedWindow;
			}
			node.Nodes.Add(CreateDummyNode());
			return node;
		}

		private static TreeNode CreateElementNode(AutomationElement element) {
			if (element == null) {
				return new TreeNode("(Unavailable)");
			}

			// element can go stale between enumeration and this read, since AutomationElement.Current crosses a UIA cross-process boundary
			try {
				string strname = string.IsNullOrEmpty(element.Current.Name) == false ? element.Current.Name : element.Current.ControlType.LocalizedControlType;
				TreeNode node = new TreeNode(strname) {
					Tag = element,
					ToolTipText = $"{strname}\r\nControlType: {element.Current.ControlType.LocalizedControlType}\r\nAutomationId: {element.Current.AutomationId}\r\nClass: {element.Current.ClassName}"
				};
				node.Nodes.Add(CreateDummyNode());
				return node;
			}
			catch (ElementNotAvailableException) {
				return new TreeNode("(Unavailable)");
			}
		}

		private static TreeNode CreateDummyNode() {
			return new TreeNode("Loading...") { Name = DUMMY_NODE_KEY };
		}
	}
}
