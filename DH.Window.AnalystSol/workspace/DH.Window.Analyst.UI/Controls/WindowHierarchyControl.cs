//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Automation;
using System.Windows.Forms;
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

		// passes the TreeNode itself (not just Tag) so the caller can walk the ancestor chain for a breadcrumb
		public event EventHandler<TreeNode> NodeSelected;

		public TreeNode SelectedNode {
			get { return m_trvHierarchy.SelectedNode; }
		}

		public WindowHierarchyControl() {
			InitializeComponent();

			m_btnToggleMode.Click += (sender, e) => {
				m_bUiaMode = !m_bUiaMode;
				m_btnToggleMode.Text = m_bUiaMode == true ? "UI Element" : "Win32";
				LoadRoot(m_rootItem);
			};
			m_trvHierarchy.BeforeExpand += OnBeforeExpand;
			m_trvHierarchy.AfterSelect += (sender, e) => NodeSelected?.Invoke(this, e.Node);
			m_btnHighlight.Click += (sender, e) => HighlightSelected();
			m_btnForeground.Click += (sender, e) => ActivateSelected();
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

			IReadOnlyList<IntPtr> listchain = m_treeService.GetAncestorChainToRoot(handle, m_rootItem.Handle);
			if (listchain.Count == 0) {
				return false;
			}

			TreeNode nodecurrent = m_trvHierarchy.Nodes[0];
			foreach (IntPtr handlehop in listchain) {
				nodecurrent.Expand();

				TreeNode nodenext = null;
				foreach (TreeNode nodechild in nodecurrent.Nodes) {
					if (nodechild.Tag is TopLevelWindowItem item && item.Handle == handlehop) {
						nodenext = nodechild;
						break;
					}
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

		private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e) {
			TreeNode nodeparent = e.Node;
			if (nodeparent.Nodes.Count != 1 || nodeparent.Nodes[0].Name != DUMMY_NODE_KEY) {
				return;
			}

			nodeparent.Nodes.Clear();

			if (m_bUiaMode == false && nodeparent.Tag is TopLevelWindowItem windowitem) {
				foreach (TopLevelWindowItem child in m_treeService.GetChildWindowInfos(windowitem.Handle)) {
					nodeparent.Nodes.Add(CreateWin32Node(child));
				}
			}
			else if (m_bUiaMode == true && nodeparent.Tag is AutomationElement element) {
				foreach (AutomationElement childelement in m_treeService.GetChildren(element)) {
					nodeparent.Nodes.Add(CreateElementNode(childelement));
				}
			}
		}

		private TreeNode CreateWin32Node(TopLevelWindowItem item) {
			string strtext = string.IsNullOrEmpty(item.Title) == false ? item.Title : item.ClassName;
			TreeNode node = new TreeNode(strtext) {
				Tag = item,
				ToolTipText = $"{strtext}\r\nClass: {item.ClassName}\r\nHandle: {item.HandleText}"
			};
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
