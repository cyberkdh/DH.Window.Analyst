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
using System.Threading.Tasks;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Dialogs {
	// modeless, system-wide diagnostics window: GDI/USER object ranking across every running GUI process,
	// plus the full top-level window Z-order stack — independent of whatever is selected in MainForm.
	// Explicit-trigger convention: no auto-refresh timer, only the initial load on Shown and the "Refresh" button.
	public partial class SystemDiagnosticsForm : Form {
		private readonly IProcessDiagnosticsService m_processDiagnosticsService = new ProcessDiagnosticsService();
		private readonly IWindowDiagnosticsService m_windowDiagnosticsService = new WindowDiagnosticsService();

		public SystemDiagnosticsForm() {
			InitializeComponent();

			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvProcessRanking, "SystemDiagnosticsProcessRanking");
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvZOrder, "SystemDiagnosticsZOrder");

			Shown += async (s, e) => await RefreshAsync();
		}

		private async void OnRefreshClick(object sender, EventArgs e) {
			await RefreshAsync();
		}

		private async Task RefreshAsync() {
			m_btnRefresh.Enabled = false;
			try {
				// any top-level HWND works as the Z-order walk seed (GetWindow(..., GW_HWNDFIRST) enumerates every
				// top-level sibling regardless of which one it starts from), so this dialog's own handle is enough
				IntPtr handleseed = Handle;

				DiagnosticsSnapshot snapshot = await Task.Run(() => BuildSnapshot(handleseed));

				PopulateProcessRanking(snapshot.ProcessRanking);
				PopulateZOrder(snapshot.ZOrder);
				m_lblLastUpdated.Text = $"Last updated: {DateTime.Now:HH:mm:ss}";
			}
			finally {
				m_btnRefresh.Enabled = true;
			}
		}

		private DiagnosticsSnapshot BuildSnapshot(IntPtr handleseed) {
			List<ProcessRankingRow> listranking = new List<ProcessRankingRow>();
			foreach (Process process in Process.GetProcesses()) {
				using (process) {
					// "GUI process" filter: only processes with a visible main window are relevant to a GDI/USER leak hunt
					if (process.MainWindowHandle == IntPtr.Zero) {
						continue;
					}

					ProcessDiagnosticsInfo info = m_processDiagnosticsService.GetProcessDiagnostics(process.Id);
					listranking.Add(new ProcessRankingRow(process.Id, process.ProcessName, info));
				}
			}
			listranking.Sort((a, b) => (b.Info.GdiObjectCount ?? 0).CompareTo(a.Info.GdiObjectCount ?? 0));

			List<ZOrderEntryItem> listzorder = new List<ZOrderEntryItem>(m_windowDiagnosticsService.GetZOrderStack(handleseed));

			return new DiagnosticsSnapshot(listranking, listzorder);
		}

		private void PopulateProcessRanking(List<ProcessRankingRow> listranking) {
			m_lsvProcessRanking.Items.Clear();
			foreach (ProcessRankingRow row in listranking) {
				ListViewItem lvitem = new ListViewItem(row.ProcessName);
				lvitem.SubItems.Add(row.ProcessId.ToString());
				lvitem.SubItems.Add(row.Info.GdiObjectCount.HasValue == true ? row.Info.GdiObjectCount.Value.ToString() : "N/A");
				lvitem.SubItems.Add(row.Info.UserObjectCount.HasValue == true ? row.Info.UserObjectCount.Value.ToString() : "N/A");
				lvitem.SubItems.Add(FormatBytes(row.Info.WorkingSetBytes));
				lvitem.SubItems.Add(row.Info.IsElevated == true ? "Yes" : row.Info.IsElevated == false ? "No" : "N/A");
				m_lsvProcessRanking.Items.Add(lvitem);
			}
		}

		private void PopulateZOrder(List<ZOrderEntryItem> listzorder) {
			m_lsvZOrder.Items.Clear();
			foreach (ZOrderEntryItem entry in listzorder) {
				ListViewItem lvitem = new ListViewItem($"0x{entry.Handle.ToInt64():X}");
				lvitem.SubItems.Add(entry.Caption);
				lvitem.SubItems.Add(entry.ClassName);
				lvitem.SubItems.Add(entry.ProcessName);
				lvitem.SubItems.Add(entry.IsVisible == true ? "Yes" : "No");
				if (entry.IsVisible == false) {
					lvitem.ForeColor = System.Drawing.Color.Gray;
				}
				m_lsvZOrder.Items.Add(lvitem);
			}
		}

		private static string FormatBytes(long nbytes) {
			return $"{nbytes / 1024.0 / 1024.0:0.0} MB";
		}

		private class DiagnosticsSnapshot {
			public List<ProcessRankingRow> ProcessRanking { get; }
			public List<ZOrderEntryItem> ZOrder { get; }

			public DiagnosticsSnapshot(List<ProcessRankingRow> listranking, List<ZOrderEntryItem> listzorder) {
				ProcessRanking = listranking;
				ZOrder = listzorder;
			}
		}

		private class ProcessRankingRow {
			public int ProcessId { get; }
			public string ProcessName { get; }
			public ProcessDiagnosticsInfo Info { get; }

			public ProcessRankingRow(int nprocessid, string strprocessname, ProcessDiagnosticsInfo info) {
				ProcessId = nprocessid;
				ProcessName = strprocessname;
				Info = info;
			}
		}
	}
}
