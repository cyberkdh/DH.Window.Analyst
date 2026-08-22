//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Models;

namespace DH.Window.Analyst.UI.Controls {
	// one workspace tab's content: breadcrumb on top, hierarchy tree (left) + property view (right) below — composes independent controls rather than owning the layout logic itself
	public partial class WindowWorkspaceTabControl : UserControl {
		public TopLevelWindowItem RootItem { get; private set; }

		// bubbled from the property view — a Parent/Owner/First Child/Next/Previous row was double-clicked
		public event EventHandler<IntPtr> WindowReferenceActivated;

		private IWindowTreeService m_treeService;

		public WindowWorkspaceTabControl() {
			InitializeComponent();

			m_ctrlHierarchy.NodeSelected += OnHierarchyNodeSelected;
			m_ctrlBreadcrumb.SegmentClicked += (sender, tag) => {
				if (tag is TreeNode node) {
					m_ctrlHierarchy.FocusNode(node);
				}
			};
			m_ctrlPropertyView.WindowReferenceActivated += (sender, handle) => WindowReferenceActivated?.Invoke(this, handle);
			m_timerWindowCheck.Tick += (sender, e) => CheckRootWindowStillExists();

			m_btnToggleMode.Click += (sender, e) => {
				m_ctrlHierarchy.ToggleMode();
				m_btnToggleMode.Text = m_ctrlHierarchy.UiaMode == true ? "UI Element" : "Win32";
			};
			m_btnHighlight.Click += (sender, e) => m_ctrlHierarchy.HighlightSelected();
			m_btnForeground.Click += (sender, e) => m_ctrlHierarchy.ActivateSelected();
			m_chkSync.CheckedChanged += (sender, e) => m_ctrlHierarchy.SetSyncEnabled(m_chkSync.Checked);
			// a Sync click (or Esc) stops Sync from inside m_ctrlHierarchy itself — reflect that back onto the checkbox so its visual state doesn't stay stuck ON
			m_ctrlHierarchy.SyncStoppedExternally += (sender, e) => m_chkSync.Checked = false;

			// RawView (default, off) shows every UIA peer including Legacy IAccessible bridge duplicates; ControlView (checked) filters to IsControlElement only, reducing duplicate nodes / Sync mis-highlight
			m_chkControlView.CheckedChanged += (sender, e) => {
				if (m_treeService != null) {
					m_treeService.UseControlView = m_chkControlView.Checked;
				}
				m_ctrlHierarchy.ReloadTree();
			};
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
			m_ctrlHierarchy.Initialize(treeservice);
			m_ctrlPropertyView.Initialize(treeservice);
			m_ctrlUiaPropertyView.Initialize(treeservice);
			m_ctrlAccessibilityCheck.Initialize(treeservice);
		}

		// don't ALSO populate the breadcrumb/property panel here — LoadRoot's auto-selection already fires NodeSelected for that, and doing both raced and produced duplicated property rows
		public Task ShowWindowAsync(TopLevelWindowItem rootitem) {
			RootItem = rootitem;

			m_ctrlEventLog.SetTargetWindow(rootitem);
			m_ctrlMessageLog.SetTargetWindow(rootitem);
			m_ctrlAccessibilityCheck.SetTargetWindow(rootitem);

			m_ctrlHierarchy.LoadRoot(rootitem);

			m_panelClosedBanner.Visible = false;
			m_timerWindowCheck.Enabled = rootitem != null;

			return Task.CompletedTask;
		}

		// selects the tree node for a specific descendant handle (Show Info on Mouse global click), delegating to the hierarchy tree without exposing m_ctrlHierarchy itself
		public bool SelectDescendantByHandle(IntPtr handle) {
			return m_ctrlHierarchy.SelectDescendantByHandle(handle);
		}

		// polls IsWindow() on the root handle since there's no per-window "destroyed" push notification available; a lightweight timer is the simplest way to notice the user closed the inspected window
		private void CheckRootWindowStillExists() {
			if (RootItem == null || m_treeService.IsWindowValid(RootItem.Handle) == true) {
				return;
			}

			m_timerWindowCheck.Enabled = false;
			m_panelClosedBanner.Visible = true;

			// release the native hooks/handlers still pointed at the now-dead window
			m_ctrlEventLog.SetTargetWindow(null);
			m_ctrlMessageLog.SetTargetWindow(null);
			m_ctrlAccessibilityCheck.SetTargetWindow(null);
		}

		private async void OnHierarchyNodeSelected(object sender, TreeNode node) {
			SetBreadcrumbPath(node);

			if (node.Tag is TopLevelWindowItem windowitem) {
				ShowWin32PropertyPanel();
				await m_ctrlPropertyView.ShowWindowAsync(windowitem);
			}
			else if (node.Tag is AutomationElement element) {
				ShowUiaPropertyPanel();
				await m_ctrlUiaPropertyView.ShowElementAsync(element, BuildIndexKeyPath(node), BuildTypePath(node));
			}
		}

		// tab's root child down to node, in-memory only (TreeNode.Parent/.Index) — deliberately reuses the already-loaded tree instead of re-querying UIA, since a fresh ancestor walk + per-level FindAll(Children) to resolve each sibling index was the main cause of a felt slowdown vs. DH.Window.UITool
		private static List<TreeNode> BuildChainFromRootChild(TreeNode node) {
			List<TreeNode> listchain = new List<TreeNode>();
			TreeNode nodecurrent = node;
			while (nodecurrent.Parent != null) {
				listchain.Insert(0, nodecurrent);
				nodecurrent = nodecurrent.Parent;
			}
			return listchain;
		}

		// sibling-index path ("0/2/1") from the tab's root to node; empty if node IS the root
		private static string BuildIndexKeyPath(TreeNode node) {
			List<TreeNode> listchain = BuildChainFromRootChild(node);
			List<string> listindices = new List<string>();
			foreach (TreeNode nodehop in listchain) {
				listindices.Add(nodehop.Index.ToString());
			}
			return string.Join("/", listindices);
		}

		// ControlType path (".\pane\pane\...\combo box") from the tab's root to node; each hop's ControlType is one cheap live property read (never a subtree enumeration), "?" for any hop that fails
		private static string BuildTypePath(TreeNode node) {
			List<TreeNode> listchain = BuildChainFromRootChild(node);
			List<string> listtypes = new List<string>();
			foreach (TreeNode nodehop in listchain) {
				string strtype = "?";
				if (nodehop.Tag is AutomationElement elementhop) {
					try {
						strtype = elementhop.Current.ControlType.LocalizedControlType;
					}
					catch (ElementNotAvailableException) {
						// stale reference from a live UI update since the tree was built; leave as "?"
					}
				}
				listtypes.Add(strtype);
			}
			return listtypes.Count > 0 ? "." + "\\" + string.Join("\\", listtypes) : ".";
		}

		// unifies snapshot capture for the "Current" submenu entry: dispatches on whatever node type is currently selected in this tab's hierarchy tree (Win32 window or UIA element), single node only — no subtree walk
		public async Task<PropertySnapshot> CaptureSnapshotAsync() {
			TreeNode node = m_ctrlHierarchy.SelectedNode;
			if (node == null) {
				return null;
			}

			if (node.Tag is TopLevelWindowItem) {
				return await m_ctrlPropertyView.CaptureSnapshotAsync();
			}
			if (node.Tag is AutomationElement) {
				return await m_ctrlUiaPropertyView.CaptureSnapshotAsync();
			}
			return null;
		}

		private void ShowWin32PropertyPanel() {
			m_ctrlUiaPropertyView.Visible = false;
			m_ctrlPropertyView.Visible = true;
		}

		private void ShowUiaPropertyPanel() {
			m_ctrlPropertyView.Visible = false;
			m_ctrlUiaPropertyView.Visible = true;
		}

		private void SetBreadcrumbPath(TreeNode nodeselected) {
			List<BreadcrumbSegment> listsegments = new List<BreadcrumbSegment>();

			TreeNode nodecurrent = nodeselected;
			while (nodecurrent != null) {
				listsegments.Insert(0, new BreadcrumbSegment(nodecurrent.Text, nodecurrent));
				nodecurrent = nodecurrent.Parent;
			}

			m_ctrlBreadcrumb.SetPath(listsegments);
		}
	}
}
