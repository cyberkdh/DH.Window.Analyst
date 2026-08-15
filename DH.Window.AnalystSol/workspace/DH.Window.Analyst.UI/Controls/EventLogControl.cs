//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// monitoring only starts/stops via the explicit Start/Stop buttons — never automatically on tab or tree selection; log capped at MAX_LOG_ROWS so a high-frequency event like LOCATIONCHANGE cannot grow the ListView unbounded
	public partial class EventLogControl : UserControl {
		private const int MAX_LOG_ROWS = 1000;

		// instantiated locally rather than threaded through Initialize() — holds a hook handle tied 1:1 to this control's lifetime, not a shared instance
		private readonly IWinEventMonitorService m_monitorService = new WinEventMonitorService();
		private int m_nTargetProcessId;

		public EventLogControl() {
			InitializeComponent();

			foreach (uint neventid in WinEventMonitorService.SupportedEventIds) {
				int nindex = m_clbFilter.Items.Add(WinEventMonitorService.GetEventName(neventid));
				m_clbFilter.SetItemChecked(nindex, Array.IndexOf(WinEventMonitorService.HighFrequencyEventIds, neventid) < 0);
			}

			m_btnStart.Click += (sender, e) => StartMonitoring();
			m_btnStop.Click += (sender, e) => StopMonitoring();
			m_btnClear.Click += (sender, e) => { m_lsvEvents.Items.Clear(); UpdateButtonState(); };
			m_btnExport.Click += (sender, e) => ListViewExportHelper.Export(m_lsvEvents, "Events");
			m_btnCopy.Click += (sender, e) => ListViewExportHelper.CopyToClipboard(m_lsvEvents);
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvEvents, "Events");

			UpdateButtonState();
		}

		public void SetTargetWindow(TopLevelWindowItem item) {
			StopMonitoring();
			m_nTargetProcessId = item?.ProcessId ?? 0;
			m_lblStatus.Text = item != null ? $"Target: {item.ProcessName} (PID {item.ProcessId})" : "(No target window)";
			UpdateButtonState();
		}

		private void StartMonitoring() {
			if (m_nTargetProcessId == 0 || m_monitorService.IsMonitoring == true) {
				return;
			}

			m_monitorService.Start(m_nTargetProcessId, OnWinEventReceived);
			UpdateButtonState();
		}

		private void StopMonitoring() {
			m_monitorService.Stop();
			UpdateButtonState();
		}

		private void OnWinEventReceived(WinEventItem item) {
			if (IsFilteredIn(item.EventName) == false) {
				return;
			}

			ListViewItem lvitem = new ListViewItem(item.Timestamp.ToString("HH:mm:ss.fff"));
			lvitem.SubItems.Add(item.EventName);
			lvitem.SubItems.Add("0x" + item.Handle.ToString("X"));
			lvitem.SubItems.Add(item.ObjectId.ToString());
			lvitem.SubItems.Add(item.ChildId.ToString());

			m_lsvEvents.Items.Add(lvitem);

			while (m_lsvEvents.Items.Count > MAX_LOG_ROWS) {
				m_lsvEvents.Items.RemoveAt(0);
			}

			lvitem.EnsureVisible();
			UpdateButtonState();
		}

		private bool IsFilteredIn(string streventname) {
			int nindex = m_clbFilter.Items.IndexOf(streventname);
			return nindex >= 0 && m_clbFilter.GetItemChecked(nindex) == true;
		}

		private void UpdateButtonState() {
			m_btnStart.Enabled = m_nTargetProcessId != 0 && m_monitorService.IsMonitoring == false;
			m_btnStop.Enabled = m_monitorService.IsMonitoring == true;
			m_btnExport.Enabled = m_lsvEvents.Items.Count > 0;
			m_btnCopy.Enabled = m_lsvEvents.Items.Count > 0;
		}
	}
}
