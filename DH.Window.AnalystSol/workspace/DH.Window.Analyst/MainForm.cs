//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.Settings;
using DH.Window.Analyst.UI.Dialogs;
using DH.Window.Analyst.UI.Dialogs.Options;

namespace DH.Window.Analyst {
	// WinForm shell: window list (left) + property preview (right), both reused from DH.Window.Analyst.UI. Designer-editable (see .Designer.cs)
	public partial class MainForm : Form {
		private readonly IWindowTreeService m_treeService = new WindowTreeService();
		private TopLevelWindowItem m_selectedWindowItem;
		private bool m_bGroupByProcess;
		private MousePickerCoordinator m_pickerCoordinator;
		private SystemDiagnosticsForm m_dlgDiagnostics;
		// Property Compare feature: session-only, never persisted — same lifetime as Event/Message logs
		private readonly List<PropertySnapshot> m_listSnapshots = new List<PropertySnapshot>();
		private SnapshotCompareForm m_dlgSnapshotCompare;

		public MainForm() {
			InitializeComponent();

			m_bGroupByProcess = AppSettings.Get().GroupByProcessDefault;
			m_toolBtnGroupByProcess.Text = m_bGroupByProcess == true ? "Group: Process" : "Group: None";

			if (AppSettings.Get().RememberMainWindowBounds == true) {
				System.Drawing.Rectangle? rectbounds = AppSettings.Get().GetMainWindowBounds();
				if (rectbounds.HasValue == true) {
					StartPosition = FormStartPosition.Manual;
					Bounds = rectbounds.Value;
				}
			}

			FormClosing += (sender, e) => {
				if (AppSettings.Get().RememberMainWindowBounds == true && WindowState == FormWindowState.Normal) {
					AppSettings.Get().SetMainWindowBounds(Bounds);
				}
			};

			m_ctrlWindowList.Initialize(m_treeService);
			m_ctrlWindowList.GroupByProcess = m_bGroupByProcess;
			m_ctrlPropertyView.Initialize(m_treeService);
			m_ctrlWorkspace.Initialize(m_treeService);
			m_ctrlWindowList.SelectedWindowChanged += async (sender, item) => {
				m_selectedWindowItem = item;
				UpdateToolbarState();
				await m_ctrlPropertyView.ShowWindowAsync(item);
			};
			m_ctrlPropertyView.WindowReferenceActivated += async (sender, handle) => await OpenWindowByHandleAsync(handle);
			m_ctrlWorkspace.WindowReferenceActivated += async (sender, handle) => await OpenWindowByHandleAsync(handle);
			m_ctrlWindowList.WindowActivated += async (sender, item) => {
				AppLog.i($"Workspace opened: {item.ClassName} \"{item.Title}\" (PID {item.ProcessId}, handle 0x{item.Handle.ToInt64():X})");
				await m_ctrlWorkspace.OpenWindowAsync(item);
			};
			m_ctrlWindowList.WindowPicked += async (sender, item) => {
				AppLog.i($"Finder picked: {item.ClassName} \"{item.Title}\" (PID {item.ProcessId}, handle 0x{item.Handle.ToInt64():X})");
				await SelectTopLevelWindowAsync(item);
			};

			m_menuWindow_TabList.DropDownOpening += (sender, e) => m_ctrlWorkspace.PopulateTabListSubmenu(m_menuWindow_TabList.DropDownItems);
			m_menuTools_TakeSnapshot.DropDownOpening += (sender, e) => m_ctrlWorkspace.PopulateSnapshotTargetSubmenu(m_menuTools_TakeSnapshot.DropDownItems, CaptureSnapshotFromTabAsync);

			m_pickerCoordinator = new MousePickerCoordinator(m_treeService, m_ctrlWorkspace, m_toolBtnShowInfoOnMouse, SelectTopLevelWindowAsync);
			FormClosing += (sender, e) => m_pickerCoordinator.Dispose();

			Load += async (sender, e) => await m_ctrlWindowList.RefreshAsync();
		}

		// shared by the Finder Tool and the Show Info on Mouse global picker: selects the top-level window in the list, refreshing once if it's not loaded yet, then shows it in Property View and opens/focuses its workspace tab
		private async Task SelectTopLevelWindowAsync(TopLevelWindowItem item) {
			m_selectedWindowItem = item;
			if (m_ctrlWindowList.SelectWindowByHandle(item.Handle) == false) {
				await m_ctrlWindowList.RefreshAsync();
				m_ctrlWindowList.SelectWindowByHandle(item.Handle);
			}
			await m_ctrlPropertyView.ShowWindowAsync(item);
			await m_ctrlWorkspace.OpenWindowAsync(item);
		}

		// single place for every "does the current selection allow this toolbar action" rule, called anywhere the selection may have
		// changed (list selection, Refresh/filter/group-toggle clearing it) so new selection-dependent buttons only need one line here
		private void UpdateToolbarState() {
			bool bhasselection = m_selectedWindowItem != null;
			m_toolBtnHighlight.Enabled = bhasselection;
			m_toolBtnForeground.Enabled = bhasselection;
			m_toolBtnRefreshProperty.Enabled = bhasselection;
		}

		// FindWindows uses blocking SendMessage with no timeout, so it must run off the UI thread or a hung window's handle freezes the app
		private async Task OpenWindowByHandleAsync(IntPtr handle) {
			TopLevelWindowItem itemfound = await Task.Run(() => {
				foreach (TopLevelWindowItem item in m_treeService.FindWindows(null, null, handle)) {
					return item;
				}
				return null;
			});

			if (itemfound == null) {
				return;
			}

			m_selectedWindowItem = itemfound;
			if (m_ctrlWindowList.SelectWindowByHandle(handle) == false) {
				await m_ctrlWindowList.RefreshAsync();
				m_ctrlWindowList.SelectWindowByHandle(handle);
			}

			await m_ctrlPropertyView.ShowWindowAsync(itemfound);
			await m_ctrlWorkspace.OpenWindowAsync(itemfound);
		}

		// invoked by the "Take Property Snapshot" submenu's "Current" entry and its per-tab entries alike, so whichever Inspector
		// tab/node the developer means (top-level window or a UIA child element) is captured through the same single path
		private async Task CaptureSnapshotFromTabAsync(DH.Window.Analyst.UI.Controls.WindowWorkspaceTabControl ctrltab) {
			PropertySnapshot snapshot = await ctrltab.CaptureSnapshotAsync();
			if (snapshot == null) {
				MessageBox.Show(this, "Select a window or element first.", "Take Property Snapshot", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else {
				m_listSnapshots.Add(snapshot);
				AppLog.i($"Property snapshot captured: {snapshot.ClassName} \"{snapshot.WindowTitle}\" (handle 0x{snapshot.Handle.ToInt64():X}), {snapshot.Items.Count} properties");
			}
		}

		private async void OnProcControls(object sender, System.EventArgs e) {
			if (sender == m_toolBtnRefreshProperty) {
				await m_ctrlPropertyView.ShowWindowAsync(m_selectedWindowItem);
			}
			else if (sender == m_menuView_ExportWindowList) {
				m_ctrlWindowList.ExportWindowList();
			}
			else if (sender == m_menuTools_CompareSnapshots) {
				if (m_dlgSnapshotCompare == null || m_dlgSnapshotCompare.IsDisposed == true) {
					m_dlgSnapshotCompare = new SnapshotCompareForm(m_listSnapshots);
					m_dlgSnapshotCompare.Show(this);
				}
				else {
					m_dlgSnapshotCompare.Activate();
				}
			}
			else if (sender == m_toolBtnGroupByProcess) {
				m_bGroupByProcess = !m_bGroupByProcess;
				m_toolBtnGroupByProcess.Text = m_bGroupByProcess == true ? "Group: Process" : "Group: None";
				m_ctrlWindowList.GroupByProcess = m_bGroupByProcess;
			}
			else if (sender == m_toolBtnHighlight) {
				AppLog.i($"Highlight: {m_selectedWindowItem?.ClassName} \"{m_selectedWindowItem?.Title}\"");
				m_ctrlWindowList.HighlightSelected();
			}
			else if (sender == m_toolBtnForeground) {
				AppLog.i($"Foreground: {m_selectedWindowItem?.ClassName} \"{m_selectedWindowItem?.Title}\"");
				m_ctrlWindowList.ActivateSelected();
			}
			else if (sender == m_toolBtnShowInfoOnMouse) {
				m_pickerCoordinator.Toggle();
			}
			else if (sender == m_menuFile_Options) {
				using (OptionsForm dlgoptions = new OptionsForm()) {
					dlgoptions.ShowDialog(this);
				}
			}
			else if (sender == m_menuFile_Exit) {
				AppLog.i("Exit requested from File menu");
				Close();
			}
			else if (sender == m_menuView_Refresh || sender == m_toolBtnRefresh) {
				AppLog.i("Window list refresh requested");
				await m_ctrlWindowList.RefreshAsync();
			}
			else if (sender == m_menuView_Find || sender == m_toolBtnFind) {
				using (FindWindowForm dlgfind = new FindWindowForm(m_treeService)) {
					if (dlgfind.ShowDialog(this) == DialogResult.OK && dlgfind.FoundWindow != null) {
						AppLog.i($"Find Window matched: {dlgfind.FoundWindow.ClassName} \"{dlgfind.FoundWindow.Title}\" (handle 0x{dlgfind.FoundWindow.Handle.ToInt64():X})");
						IntPtr handletoplevel = m_treeService.GetTopLevelAncestorHandle(dlgfind.FoundWindow.Handle);
						if (m_ctrlWindowList.SelectWindowByHandle(handletoplevel) == false) {
							await m_ctrlWindowList.RefreshAsync();
							m_ctrlWindowList.SelectWindowByHandle(handletoplevel);
						}
					}
				}
			}
			else if (sender == m_menuTools_Diagnostics || sender == m_toolBtnDiagnostics) {
				if (m_dlgDiagnostics == null || m_dlgDiagnostics.IsDisposed == true) {
					m_dlgDiagnostics = new SystemDiagnosticsForm();
					m_dlgDiagnostics.Show(this);
				}
				else {
					m_dlgDiagnostics.Activate();
				}
			}
			else if (sender == m_menuWindow_CloseTab) {
				m_ctrlWorkspace.CloseSelectedTab();
			}
			else if (sender == m_menuWindow_DetachTab) {
				m_ctrlWorkspace.DetachSelectedTab();
			}
			else if (sender == m_toolBtnTabList) {
				m_ctrlWorkspace.ShowTabListPopup(m_toolMain.PointToScreen(new System.Drawing.Point(m_toolBtnTabList.Bounds.Left, m_toolMain.Height)));
			}
			else if (sender == m_menuHelp_About) {
				MessageBox.Show(this, "DH.Window.Analyst\r\nWindows UI structure analyzer", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
