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
		private readonly Timer m_timerSync = new Timer { Interval = 50 };
		private IntPtr m_handleLastSyncTarget;
		private AutomationElement m_elementLastSyncTarget;

		// last raw cursor position seen by OnSyncTick, used to detect "mouse still moving" and skip the expensive resolution while it is
		private Point m_pointLastCursorTick;

		// while Sync is on, a left-click anywhere freezes the tree on the current target and turns Sync off, using a separate hook instance scoped to this tab
		private readonly IGlobalPickerHookService m_hookSyncClick = new GlobalPickerHookService();

		// passes the TreeNode itself (not just Tag) so the caller can walk the ancestor chain for a breadcrumb
		public event EventHandler<TreeNode> NodeSelected;

		// raised after ToggleMode() flips the tree source, so an external toolbar (WindowWorkspaceTabControl) can refresh its button text
		public event EventHandler ModeChanged;

		// raised when a Sync click (or Esc) turns Sync off from inside this control, so the toolbar's Sync checkbox can update to match
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

		// rebuilds the currently shown tree from scratch, triggered explicitly (Control View toggle in WindowWorkspaceTabControl) so a walker-mode change is reflected immediately
		public void ReloadTree() {
			LoadRoot(m_rootItem);
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

		// left-click while Sync is on: resolve once more at the exact click point, freeze the tree/overlay there, then turn Sync off; the hook swallows the click
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

		// selects the tree node for the given descendant handle, expanding ancestors one hop at a time so lazy loading (OnBeforeExpand) populates each level; Win32-mode tree only
		public bool SelectDescendantByHandle(IntPtr handle) {
			if (m_bUiaMode == true || m_treeService == null || m_rootItem == null || m_trvHierarchy.Nodes.Count == 0) {
				return false;
			}

			TreeNode noderoot = m_trvHierarchy.Nodes[0];

			// GetAncestorChainToRoot returns an empty chain both when handle is unreachable AND when handle IS the root itself, so that case is handled separately first
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
					// cached children reflect whatever existed when this node was first expanded, so re-enumerate once before giving up
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

		// adds real WS_CHILD descendants directly, then a separate "Owned Windows" group node, so the tree never implies parent-child for merely-owned windows
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

		// selects the tree node for the given descendant UIA element (Sync hover), expanding ancestors one hop at a time; UIA-mode counterpart to SelectDescendantByHandle
		public bool SelectDescendantElement(AutomationElement element) {
			TreeNode node = FindElementChainNode(element);
			if (node == null) {
				return false;
			}

			m_trvHierarchy.SelectedNode = node;
			node.EnsureVisible();
			return true;
		}

		// ancestor-chain walk (RawView, from element up to root) resolved down to a tree node; null if not a descendant or chain can't be matched
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
					// same rationale as the Win32 path: re-enumerate once before giving up, in case a new element appeared while hovering
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

		// fallback for when the ancestor-chain walk lands on the wrong branch (e.g. Chrome's GPU-compositor surface pane sharing a rect with real content): scans the whole subtree and keeps the smallest-area match
		private TreeNode FindDeepestPointNode(Point pointscreen) {
			if (m_bUiaMode == false || m_trvHierarchy.Nodes.Count == 0) {
				return null;
			}

			TreeNode noderoot = m_trvHierarchy.Nodes[0];
			TreeNode nodebest = null;
			double dblbestarea = double.MaxValue;
			FindDeepestPointNode(noderoot, pointscreen, ref nodebest, ref dblbestarea);
			return nodebest ?? noderoot;
		}

		// recurses into every child regardless of whether its own rect contains the point - a stale/clipped intermediate ancestor rect must not gate the recursion, or a genuinely-matching descendant further down gets missed entirely; among all matches found, keeps the smallest bounding-rectangle area (the most specific hit)
		private void FindDeepestPointNode(TreeNode nodeparent, Point pointscreen, ref TreeNode nodebest, ref double dblbestarea) {
			EnsureElementChildrenLoaded(nodeparent);

			foreach (TreeNode nodechild in nodeparent.Nodes) {
				if (!(nodechild.Tag is AutomationElement elementchild)) {
					continue;
				}

				System.Windows.Rect rect;
				try {
					rect = elementchild.Current.BoundingRectangle;
				}
				catch (ElementNotAvailableException) {
					// stale reference from a live UI update between enumeration and this read; skip it (and its subtree, since we can't read it either)
					continue;
				}

				if (rect.IsEmpty == false && rect.Contains(pointscreen.X, pointscreen.Y) == true) {
					double dblarea = rect.Width * rect.Height;
					if (dblarea < dblbestarea) {
						dblbestarea = dblarea;
						nodebest = nodechild;
					}
				}

				FindDeepestPointNode(nodechild, pointscreen, ref nodebest, ref dblbestarea);
			}
		}


		private void EnsureElementChildrenLoaded(TreeNode node) {
			if (node.Nodes.Count == 1 && node.Nodes[0].Name == DUMMY_NODE_KEY && node.Tag is AutomationElement elementparent) {
				RefreshElementChildNodes(node, elementparent);
			}
		}

		// heuristic for dead-end nodes like Chrome's GPU-compositor pane: real interactive content is never disabled, so this flags the case needing the expensive point-based re-search in SyncAt
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


		// scoped to this tab's own window family (root plus anything it owns), never a desktop-wide poll; hit-tests first and checks ownership via GetTopLevelAncestorHandle rather than pre-filtering by the root's screen rect, since an owned dialog is often positioned outside that rect
		private void OnSyncTick(object sender, EventArgs e) {
			Point pointcursor = Cursor.Position;

			// SyncAt's live hit-testing can take long enough to delay the WH_MOUSE_LL hook callback (perceived as system-wide mouse lag), so only pay that cost once the cursor has settled
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
					// WindowFromPoint silently skips disabled windows, so retry with a disabled-inclusive hit test rooted at the root itself, but only within the root's bounds
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

				// the TopMost overlay window can sit under the cursor; UIA's point hit test (unlike Win32's) doesn't reliably skip WS_EX_TRANSPARENT, so discard a hit belonging to our own process rather than hide the overlay each tick (flickers)
				if (elementhover != null && IsOwnProcessElement(elementhover) == true) {
					elementhover = null;
				}

				if (elementhover == null) {
					// same disabled-window blind spot as the Win32 branch; fall back to the disabled-inclusive Win32 hit test and wrap the resulting HWND as a UIA element (coarser, but a correct sync target)
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

				// FindDeepestPointNode is too expensive to run every tick (Chrome's UIA child enumeration can take tens of ms), so only re-run it when the chain result is missing or looks like a dead-end pane
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

		// owned windows are grouped under a synthetic "Owned Windows" node (see PopulateWin32Children) but still get this color to read as distinct even inside that group
		private static readonly Color s_colorOwnedWindow = Color.FromArgb(150, 90, 0);

		// marks the synthetic "Owned Windows" grouping node, distinguishing it from real TreeNode.Tag values (TopLevelWindowItem) when walking the tree
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
