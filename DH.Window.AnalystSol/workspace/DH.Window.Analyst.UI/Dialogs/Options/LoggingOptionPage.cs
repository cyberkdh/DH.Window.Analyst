//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Settings;

namespace DH.Window.Analyst.UI.Dialogs.Options {
	public partial class LoggingOptionPage : UserControl, IOptionPage {
		private bool m_bDirty;
		private bool m_bInitializing;

		public event EventHandler DirtyChanged;

		public string Title {
			get { return "Logging"; }
		}

		public LoggingOptionPage() {
			InitializeComponent();
		}

		public bool IsDirty() {
			return m_bDirty;
		}

		public void LoadSettings() {
			m_bInitializing = true;

			LogLevel level = AppSettings.Get().LogLevel;
			m_chkLogDebug.Checked = (level & LogLevel.Debug) == LogLevel.Debug;
			m_chkLogInfo.Checked = (level & LogLevel.Info) == LogLevel.Info;
			m_chkLogWarn.Checked = (level & LogLevel.Warn) == LogLevel.Warn;
			m_chkLogError.Checked = (level & LogLevel.Error) == LogLevel.Error;
			m_numRetentionDays.Value = AppSettings.Get().LogRetentionDays;

			m_bInitializing = false;
			m_bDirty = false;
		}

		public void ApplySettings() {
			LogLevel level = 0;
			if (m_chkLogDebug.Checked == true) {
				level |= LogLevel.Debug;
			}
			if (m_chkLogInfo.Checked == true) {
				level |= LogLevel.Info;
			}
			if (m_chkLogWarn.Checked == true) {
				level |= LogLevel.Warn;
			}
			if (m_chkLogError.Checked == true) {
				level |= LogLevel.Error;
			}

			AppSettings.Get().LogLevel = level;
			AppSettings.Get().LogRetentionDays = (int) m_numRetentionDays.Value;

			m_bDirty = false;
		}

		public UserControl GetControl() {
			return this;
		}

		private void OnControlChanged(object sender, EventArgs e) {
			if (m_bInitializing == true) {
				return;
			}

			m_bDirty = true;
			DirtyChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
