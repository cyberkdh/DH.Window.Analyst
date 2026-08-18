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
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// ListViewItem has no supported per-row show/hide, so all received entries are kept in m_entries (capped) and the ListView is rebuilt from it whenever the Debug filter toggles
	public partial class AppLogViewerControl : UserControl {
		private const int MAX_LOG_ROWS = 1000;

		private readonly List<LogEntry> m_entries = new List<LogEntry>();

		public AppLogViewerControl() {
			InitializeComponent();

			EnableDoubleBuffering(m_lsvLog);

			m_btnCopy.Click += (sender, e) => ListViewExportHelper.CopyToClipboard(m_lsvLog);
			m_btnExport.Click += (sender, e) => ListViewExportHelper.Export(m_lsvLog, "AppLog");
			m_btnClear.Click += (sender, e) => { m_entries.Clear(); m_lsvLog.Items.Clear(); UpdateButtonState(); };
			m_chkShowDebug.CheckedChanged += (sender, e) => ApplyDebugFilter();
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvLog, "AppLog");

			AppLog.EntryLogged += OnEntryLogged;

			UpdateButtonState();
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				AppLog.EntryLogged -= OnEntryLogged;

				if (components != null) {
					components.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		// AppLog.EntryLogged may fire from background/hook threads (e.g. the message-hook monitor), so marshal to the UI thread before touching the ListView
		private void OnEntryLogged(LogEntry entry) {
			if (IsHandleCreated == false) {
				return;
			}

			if (InvokeRequired == true) {
				BeginInvoke((Action)(() => AddEntry(entry)));
				return;
			}

			AddEntry(entry);
		}

		private void AddEntry(LogEntry entry) {
			m_entries.Add(entry);
			while (m_entries.Count > MAX_LOG_ROWS) {
				m_entries.RemoveAt(0);
			}

			if (entry.Level == LogLevel.Debug && m_chkShowDebug.Checked == false) {
				return;
			}

			m_lsvLog.BeginUpdate();
			try {
				m_lsvLog.Items.Add(BuildRow(entry));

				while (m_lsvLog.Items.Count > MAX_LOG_ROWS) {
					m_lsvLog.Items.RemoveAt(0);
				}
			}
			finally {
				m_lsvLog.EndUpdate();
			}

			if (m_lsvLog.Items.Count > 0) {
				m_lsvLog.Items[m_lsvLog.Items.Count - 1].EnsureVisible();
			}

			UpdateButtonState();
		}

		private static ListViewItem BuildRow(LogEntry entry) {
			ListViewItem lvitem = new ListViewItem(entry.Timestamp.ToString("HH:mm:ss.fff"));
			lvitem.SubItems.Add(LevelToText(entry.Level));
			lvitem.SubItems.Add(entry.Message);
			lvitem.ForeColor = LevelToColor(entry.Level);
			return lvitem;
		}

		// rebuilds the ListView from m_entries, since ListViewItem has no supported per-row show/hide
		private void ApplyDebugFilter() {
			m_lsvLog.BeginUpdate();
			try {
				m_lsvLog.Items.Clear();

				foreach (LogEntry entry in m_entries) {
					if (entry.Level == LogLevel.Debug && m_chkShowDebug.Checked == false) {
						continue;
					}

					m_lsvLog.Items.Add(BuildRow(entry));
				}
			}
			finally {
				m_lsvLog.EndUpdate();
			}

			if (m_lsvLog.Items.Count > 0) {
				m_lsvLog.Items[m_lsvLog.Items.Count - 1].EnsureVisible();
			}

			UpdateButtonState();
		}

		private static string LevelToText(LogLevel level) {
			switch (level) {
				case LogLevel.Error: return "Error";
				case LogLevel.Warn: return "Warn";
				case LogLevel.Info: return "Info";
				case LogLevel.Debug: return "Debug";
				default: return level.ToString();
			}
		}

		private static System.Drawing.Color LevelToColor(LogLevel level) {
			switch (level) {
				case LogLevel.Error: return System.Drawing.Color.Firebrick;
				case LogLevel.Warn: return System.Drawing.Color.DarkGoldenrod;
				case LogLevel.Debug: return System.Drawing.Color.Gray;
				default: return System.Drawing.SystemColors.ControlText;
			}
		}

		private static void EnableDoubleBuffering(Control ctrl) {
			typeof(Control).InvokeMember(
				"DoubleBuffered",
				BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				ctrl,
				new object[] { true });
		}

		private void UpdateButtonState() {
			m_btnExport.Enabled = m_lsvLog.Items.Count > 0;
			m_btnCopy.Enabled = m_lsvLog.Items.Count > 0;
		}
	}
}
