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
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Dialogs;
using DH.Window.Analyst.UI.Overlay;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// reusable Win32 top-level window picker: search box + TreeView rooted at "Desktop"; deep hierarchy/UI Element drill-down happens in a separate workspace tab, not here
	public partial class WindowListControl : UserControl {
		private const string DESKTOP_ICON_KEY = "__desktop";

		private static readonly string[] EXPORT_HEADERS = { "Handle", "ClassName", "Title", "ProcessName", "ProcessId" };

		private IWindowTreeService m_treeService;
		private List<TopLevelWindowItem> m_listAllWindows = new List<TopLevelWindowItem>();
		private List<TopLevelWindowItem> m_listFilteredWindows = new List<TopLevelWindowItem>();
		private TopLevelWindowItem m_desktopItem;
		private bool m_bGroupByProcess = true;
		private readonly HighlightOverlayForm m_overlayHighlight = new HighlightOverlayForm();

		public bool GroupByProcess {
			get { return m_bGroupByProcess; }
			set {
				m_bGroupByProcess = value;
				ApplyFilter();
			}
		}

		public event EventHandler<TopLevelWindowItem> SelectedWindowChanged;

		public event EventHandler<TopLevelWindowItem> WindowActivated;

		public event EventHandler<TopLevelWindowItem> WindowPicked;

		public WindowListControl() {
			InitializeComponent();

			m_ilIcons.Images.Add(DESKTOP_ICON_KEY, SystemIcons.WinLogo);

			m_txtFilter.TextChanged += (sender, e) => ApplyFilter();
			m_trvWindows.AfterSelect += (sender, e) => {
				TopLevelWindowItem item = GetWindowItem(e.Node);
				SelectedWindowChanged?.Invoke(this, item);
			};
			m_trvWindows.NodeMouseDoubleClick += (sender, e) => {
				TopLevelWindowItem item = GetWindowItem(e.Node);
				if (item != null) {
					WindowActivated?.Invoke(this, item);
				}
			};

			// context-menu item and the sidebar's File/View menu are two triggers for the exact same action — both call ExportWindowList, never duplicated logic
			m_menuCtxExport.Click += OnExportClick;
			m_ctrlFinder.WindowPicked += (sender, item) => WindowPicked?.Invoke(this, item);
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
			m_ctrlFinder.Initialize(treeservice);
		}

		// flashes the highlight overlay over the currently selected window, triggered explicitly (Highlight button)
		public void HighlightSelected() {
			TopLevelWindowItem item = GetWindowItem(m_trvWindows.SelectedNode);
			if (item == null || m_treeService == null) {
				return;
			}

			m_overlayHighlight.FlashAt(m_treeService.GetWindowScreenRect(item.Handle));
		}

		// brings the currently selected window to the foreground, triggered explicitly (Foreground button)
		public void ActivateSelected() {
			TopLevelWindowItem item = GetWindowItem(m_trvWindows.SelectedNode);
			if (item == null || m_treeService == null) {
				return;
			}

			m_treeService.ActivateWindow(item.Handle);
		}

		// Win32-based, cheap, still off the UI thread to keep the message loop responsive
		public async Task RefreshAsync() {
			if (m_treeService == null) {
				return;
			}

			m_listAllWindows = await Task.Run(() => new List<TopLevelWindowItem>(m_treeService.GetTopLevelWindowInfos()));
			m_desktopItem = await Task.Run(() => m_treeService.GetDesktopWindowInfo());

			Dictionary<int, Icon> dicprocessicons = await Task.Run(() => ExtractProcessIcons(m_listAllWindows));
			foreach (KeyValuePair<int, Icon> pair in dicprocessicons) {
				string strkey = GetProcessIconKey(pair.Key);
				if (m_ilIcons.Images.ContainsKey(strkey) == false) {
					m_ilIcons.Images.Add(strkey, pair.Value);
				}
				pair.Value.Dispose();
			}

			ApplyFilter();
		}

		// selects the tree node matching the given handle (Finder Tool drop target), expanding ancestors as needed
		public bool SelectWindowByHandle(IntPtr handle) {
			TreeNode nodefound = FindNodeByHandle(m_trvWindows.Nodes, handle);
			if (nodefound == null) {
				return false;
			}

			nodefound.EnsureVisible();
			m_trvWindows.SelectedNode = nodefound;
			return true;
		}

		private static TreeNode FindNodeByHandle(TreeNodeCollection nodes, IntPtr handle) {
			foreach (TreeNode node in nodes) {
				if (node.Tag is TopLevelWindowItem item && item.Handle == handle) {
					return node;
				}

				TreeNode nodefoundinchildren = FindNodeByHandle(node.Nodes, handle);
				if (nodefoundinchildren != null) {
					return nodefoundinchildren;
				}
			}

			return null;
		}

		// single shared handler for both export triggers (main menu, TreeView context menu) — see class remarks on the "one action, one handler" rule
		public void ExportWindowList() {
			OnExportClick(this, EventArgs.Empty);
		}

		private async void OnExportClick(object sender, EventArgs e) {
			TopLevelWindowItem itemselected = GetWindowItem(m_trvWindows.SelectedNode);
			bool bhasselection = itemselected != null;

			using (WindowListExportForm dlg = new WindowListExportForm(bhasselection)) {
				if (dlg.ShowDialog(FindForm()) != DialogResult.OK) {
					return;
				}

				List<TopLevelWindowItem> listscope = dlg.ScopeIsSelectedOnly == true && itemselected != null
					? new List<TopLevelWindowItem> { itemselected }
					: m_listFilteredWindows;

				using (SaveFileDialog dlgsave = new SaveFileDialog()) {
					dlgsave.Filter = dlg.FormatIsJson == true
						? "JSON files (*.json)|*.json|All files (*.*)|*.*"
						: "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
					dlgsave.FileName = ExportFileNameHelper.BuildTimestampedFileName("WindowList", dlg.FormatIsJson == true ? "json" : "csv");

					if (dlgsave.ShowDialog(FindForm()) != DialogResult.OK) {
						return;
					}

					string strcontent = await BuildExportContentAsync(listscope, dlg.IncludeChildWindows, dlg.FormatIsJson, dlg.StructureIsTree);
					File.WriteAllText(dlgsave.FileName, strcontent, Encoding.UTF8);
				}
			}
		}

		private async Task<string> BuildExportContentAsync(List<TopLevelWindowItem> listitems, bool bincludechildren, bool bjson, bool btree) {
			if (bjson == true && btree == true) {
				List<ExportTreeNode> listroots = bincludechildren == true
					? await Task.Run(() => BuildExportTree(listitems, true))
					: BuildExportTree(listitems, false);
				return RowExportWriter.BuildJsonTree(EXPORT_HEADERS, listroots);
			}

			List<string[]> listrows = bincludechildren == true
				? await Task.Run(() => BuildFlatRowsWithChildren(listitems))
				: BuildFlatRows(listitems);

			return bjson == true
				? RowExportWriter.BuildJsonFlat(EXPORT_HEADERS, listrows)
				: RowExportWriter.BuildDelimitedText(EXPORT_HEADERS, listrows, ",", true);
		}

		private List<ExportTreeNode> BuildExportTree(List<TopLevelWindowItem> listitems, bool bincludechildren) {
			List<ExportTreeNode> listroots = new List<ExportTreeNode>();
			foreach (TopLevelWindowItem item in listitems) {
				listroots.Add(BuildNode(item, bincludechildren));
			}
			return listroots;
		}

		private ExportTreeNode BuildNode(TopLevelWindowItem item, bool bincludechildren) {
			ExportTreeNode node = new ExportTreeNode { Values = ToRow(item) };
			if (bincludechildren == true) {
				foreach (TopLevelWindowItem child in m_treeService.GetChildWindowInfos(item.Handle)) {
					node.Children.Add(BuildNode(child, true));
				}
			}
			return node;
		}

		private List<string[]> BuildFlatRows(List<TopLevelWindowItem> listitems) {
			List<string[]> listrows = new List<string[]>();
			foreach (TopLevelWindowItem item in listitems) {
				listrows.Add(ToRow(item));
			}
			return listrows;
		}

		private List<string[]> BuildFlatRowsWithChildren(List<TopLevelWindowItem> listitems) {
			List<string[]> listrows = new List<string[]>();
			foreach (TopLevelWindowItem item in listitems) {
				listrows.Add(ToRow(item));
				AppendChildRowsRecursive(item.Handle, listrows);
			}
			return listrows;
		}

		private void AppendChildRowsRecursive(IntPtr handleparent, List<string[]> listrows) {
			foreach (TopLevelWindowItem child in m_treeService.GetChildWindowInfos(handleparent)) {
				listrows.Add(ToRow(child));
				AppendChildRowsRecursive(child.Handle, listrows);
			}
		}

		private static string[] ToRow(TopLevelWindowItem item) {
			return new[] { item.HandleText, item.ClassName, item.Title, item.ProcessName, item.ProcessId.ToString() };
		}

		private void ApplyFilter() {
			string strfilter = m_txtFilter.Text;

			m_trvWindows.BeginUpdate();
			m_trvWindows.Nodes.Clear();

			TreeNode noderoot = new TreeNode {
				Tag = m_desktopItem,
				ImageKey = DESKTOP_ICON_KEY,
				SelectedImageKey = DESKTOP_ICON_KEY,
				ToolTipText = m_desktopItem == null
					? "The real Win32 parent of every top-level window"
					: $"Desktop\r\nClass: {m_desktopItem.ClassName}\r\nProcess: {m_desktopItem.ProcessName} (PID {m_desktopItem.ProcessId})\r\nHandle: {m_desktopItem.HandleText}"
			};

			// typing a handle ("0x1A2B" or a plain decimal) jumps straight to that window instead of doing a text search
			bool bhandlefilter = HandleTextParser.TryParse(strfilter, out IntPtr handlefilter);

			List<TopLevelWindowItem> listmatched = new List<TopLevelWindowItem>();
			foreach (TopLevelWindowItem item in m_listAllWindows) {
				if (bhandlefilter == true) {
					if (item.Handle != handlefilter) {
						continue;
					}
				}
				else if (string.IsNullOrEmpty(strfilter) == false
					&& item.Title.IndexOf(strfilter, StringComparison.OrdinalIgnoreCase) < 0
					&& item.ClassName.IndexOf(strfilter, StringComparison.OrdinalIgnoreCase) < 0
					&& item.ProcessName.IndexOf(strfilter, StringComparison.OrdinalIgnoreCase) < 0) {
					continue;
				}
				listmatched.Add(item);
			}

			if (m_bGroupByProcess == false) {
				foreach (TopLevelWindowItem item in listmatched) {
					string strimagekeyflat = GetProcessIconKey(item.ProcessId);
					if (m_ilIcons.Images.ContainsKey(strimagekeyflat) == false) {
						strimagekeyflat = DESKTOP_ICON_KEY;
					}
					noderoot.Nodes.Add(CreateWindowNode(item, strimagekeyflat));
				}
			}
			else {
				// group by process so an app with multiple windows collapses into one node; a lone window stays a direct child of Desktop instead of forming a pointless single-item group
				List<int> listprocessorder = new List<int>();
				Dictionary<int, List<TopLevelWindowItem>> dicgroups = new Dictionary<int, List<TopLevelWindowItem>>();
				foreach (TopLevelWindowItem item in listmatched) {
					if (dicgroups.TryGetValue(item.ProcessId, out List<TopLevelWindowItem> listgroup) == false) {
						listgroup = new List<TopLevelWindowItem>();
						dicgroups[item.ProcessId] = listgroup;
						listprocessorder.Add(item.ProcessId);
					}
					listgroup.Add(item);
				}

				foreach (int nprocessid in listprocessorder) {
					List<TopLevelWindowItem> listgroup = dicgroups[nprocessid];
					string strimagekey = GetProcessIconKey(nprocessid);
					if (m_ilIcons.Images.ContainsKey(strimagekey) == false) {
						strimagekey = DESKTOP_ICON_KEY;
					}

					if (listgroup.Count == 1) {
						noderoot.Nodes.Add(CreateWindowNode(listgroup[0], strimagekey));
						continue;
					}

					TreeNode nodegroup = new TreeNode($"{listgroup[0].ProcessName} ({listgroup.Count})") {
						ImageKey = strimagekey,
						SelectedImageKey = strimagekey,
						ToolTipText = $"Process: {listgroup[0].ProcessName} (PID {nprocessid})\r\n{listgroup.Count} windows"
					};
					foreach (TopLevelWindowItem item in listgroup) {
						nodegroup.Nodes.Add(CreateWindowNode(item, strimagekey));
					}
					nodegroup.Expand();
					noderoot.Nodes.Add(nodegroup);
				}
			}

			noderoot.Text = $"Desktop ({listmatched.Count})";
			m_trvWindows.Nodes.Add(noderoot);
			noderoot.Expand();

			m_trvWindows.EndUpdate();

			m_listFilteredWindows = listmatched;
			m_menuCtxExport.Enabled = listmatched.Count > 0;

			// rebuilding the tree always drops SelectedNode without raising AfterSelect, so notify explicitly — otherwise listeners
			// (e.g. MainForm's toolbar enable/disable) keep acting on a stale selection after Refresh/filter/group-toggle
			SelectedWindowChanged?.Invoke(this, null);
		}

		private static TreeNode CreateWindowNode(TopLevelWindowItem item, string strimagekey) {
			string strtext = string.IsNullOrEmpty(item.Title) == false ? item.Title : item.ClassName;
			return new TreeNode(strtext) {
				Tag = item,
				ImageKey = strimagekey,
				SelectedImageKey = strimagekey,
				// title is truncated by the tree width when long — ToolTipText fills in the rest on hover
				ToolTipText = $"{strtext}\r\nClass: {item.ClassName}\r\nProcess: {item.ProcessName} (PID {item.ProcessId})\r\nHandle: {item.HandleText}"
			};
		}

		// off the UI thread: one icon extraction per distinct process, never per window
		private static Dictionary<int, Icon> ExtractProcessIcons(List<TopLevelWindowItem> listwindows) {
			Dictionary<int, Icon> dicicons = new Dictionary<int, Icon>();

			foreach (TopLevelWindowItem item in listwindows) {
				if (dicicons.ContainsKey(item.ProcessId) == true) {
					continue;
				}

				try {
					using (Process process = Process.GetProcessById(item.ProcessId)) {
						string strexepath = process.MainModule?.FileName;
						if (string.IsNullOrEmpty(strexepath) == false) {
							Icon rawicon = Icon.ExtractAssociatedIcon(strexepath);
							if (rawicon != null) {
								dicicons[item.ProcessId] = rawicon;
							}
						}
					}
				}
				catch (Exception) {
					// access denied (e.g. elevated process) — falls back to the generic desktop icon
				}
			}

			return dicicons;
		}

		private static string GetProcessIconKey(int nprocessid) {
			return "pid_" + nprocessid;
		}

		private static TopLevelWindowItem GetWindowItem(TreeNode node) {
			return node?.Tag as TopLevelWindowItem;
		}
	}
}
