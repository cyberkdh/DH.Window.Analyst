//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// monitoring only starts/stops via the explicit Start/Stop buttons — never automatically on tab or tree selection; log capped at MAX_LOG_ROWS so a high-frequency message like WM_MOUSEMOVE cannot grow the ListView unbounded
	public partial class MessageLogControl : UserControl {
		private const int MAX_LOG_ROWS = 1000;

		// instantiated locally rather than threaded through Initialize(), same as EventLogControl — holds a hook handle tied 1:1 to this control's lifetime
		private readonly IMessageHookMonitorService m_monitorService = new MessageHookMonitorService();
			private readonly IProcessDiagnosticsService m_diagnosticsService = new ProcessDiagnosticsService();
		private IntPtr m_htargetWindow;
			private int m_ntargetProcessId;

		public MessageLogControl() {
			InitializeComponent();

			// forces the protected DoubleBuffered property via reflection — without it, ListView repaints each Add/Remove against the raw HDC, causing visible flicker at hook-driven update rates
			EnableDoubleBuffering(m_lsvMessages);

			foreach (uint nmessageid in MessageHookMonitorService.SupportedMessageIds) {
				int nindex = m_clbFilter.Items.Add(MessageHookMonitorService.GetMessageName(nmessageid));
				m_clbFilter.SetItemChecked(nindex, Array.IndexOf(MessageHookMonitorService.HighFrequencyMessageIds, nmessageid) < 0);
			}

			m_btnStart.Click += (sender, e) => StartMonitoring();
			m_btnStop.Click += (sender, e) => StopMonitoring();
			m_btnClear.Click += (sender, e) => { m_lsvMessages.Items.Clear(); UpdateButtonState(); };
			m_btnExport.Click += (sender, e) => ListViewExportHelper.Export(m_lsvMessages, "Messages");
			m_btnCopy.Click += (sender, e) => ListViewExportHelper.CopyToClipboard(m_lsvMessages);
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvMessages, "Messages");

			UpdateButtonState();
		}

		public void SetTargetWindow(TopLevelWindowItem item) {
			StopMonitoring();
			m_htargetWindow = item?.Handle ?? IntPtr.Zero;
			m_ntargetProcessId = item?.ProcessId ?? 0;
			m_lblStatus.Text = item != null ? $"Target: {item.ProcessName} (PID {item.ProcessId})" : "(No target window)";
			UpdateButtonState();
		}

		private void StartMonitoring() {
			if (m_htargetWindow == IntPtr.Zero || m_monitorService.IsMonitoring == true) {
				return;
			}

			// SetWindowsHookEx into an elevated target silently fails with ERROR_ACCESS_DENIED (UIPI), so this checks and offers a fix up front instead of just reporting the cryptic failure
			if (IsElevationMismatch()) {
				if (ElevationRestartHelper.ConfirmAndRestartElevated(FindForm(), "Target process") == true) {
					return;
				}
			}

			if (m_monitorService.Start(m_htargetWindow, OnMessagesReceived) == false) {
				m_lblStatus.Text = m_monitorService.LastError ?? "Hook install failed.";
			}

			UpdateButtonState();
		}

		private bool IsElevationMismatch() {
			bool bcurrentelevated = m_diagnosticsService.IsCurrentProcessElevated();
			if (bcurrentelevated == true) {
				return false;
			}

			bool btargetelevated = m_diagnosticsService.GetProcessDiagnostics(m_ntargetProcessId).IsElevated == true;
			return btargetelevated;
		}

		private void StopMonitoring() {
			m_monitorService.Stop();
			UpdateButtonState();
		}

		// batches all ListView mutations per WM_COPYDATA call under one BeginUpdate/EndUpdate instead of per-message, since per-message redraw at WH_GETMESSAGE frequency caused visible flicker/scroll-jitter
		private void OnMessagesReceived(IReadOnlyList<MessageLogItem> items) {
			List<ListViewItem> listrows = null;

			foreach (MessageLogItem item in items) {
				if (IsFilteredIn(item.MessageName) == false) {
					continue;
				}

				ListViewItem lvitem = new ListViewItem(item.Timestamp.ToString("HH:mm:ss.fff"));
				lvitem.SubItems.Add(item.MessageName);
				lvitem.SubItems.Add("0x" + item.Handle.ToString("X"));
				lvitem.SubItems.Add("0x" + item.WParam.ToString("X"));
				lvitem.SubItems.Add("0x" + item.LParam.ToString("X"));

				(listrows ?? (listrows = new List<ListViewItem>())).Add(lvitem);
			}

			if (listrows == null) {
				return;
			}

			m_lsvMessages.BeginUpdate();
			try {
				foreach (ListViewItem lvitem in listrows) {
					m_lsvMessages.Items.Add(lvitem);
				}

				while (m_lsvMessages.Items.Count > MAX_LOG_ROWS) {
					m_lsvMessages.Items.RemoveAt(0);
				}
			}
			finally {
				m_lsvMessages.EndUpdate();
			}

			listrows[listrows.Count - 1].EnsureVisible();
			UpdateButtonState();
		}

		private static void EnableDoubleBuffering(Control ctrl) {
			typeof(Control).InvokeMember(
				"DoubleBuffered",
				BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				ctrl,
				new object[] { true });
		}

		private bool IsFilteredIn(string strmessagename) {
			int nindex = m_clbFilter.Items.IndexOf(strmessagename);
			return nindex >= 0 && m_clbFilter.GetItemChecked(nindex) == true;
		}

		private void UpdateButtonState() {
			m_btnStart.Enabled = m_htargetWindow != IntPtr.Zero && m_monitorService.IsMonitoring == false;
			m_btnStop.Enabled = m_monitorService.IsMonitoring == true;
			m_btnExport.Enabled = m_lsvMessages.Items.Count > 0;
			m_btnCopy.Enabled = m_lsvMessages.Items.Count > 0;
		}
	}
}
