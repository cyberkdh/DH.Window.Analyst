//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using DH.Window.Analyst.Settings;

namespace DH.Window.Analyst.UI.Dialogs.Options {
	public partial class GeneralOptionPage : UserControl, IOptionPage {
		private bool m_bDirty;
		private bool m_bInitializing;

		public event EventHandler DirtyChanged;

		public string Title {
			get { return "General"; }
		}

		public GeneralOptionPage() {
			InitializeComponent();
		}

		public bool IsDirty() {
			return m_bDirty;
		}

		public void LoadSettings() {
			m_bInitializing = true;

			m_chkGroupByProcessDefault.Checked = AppSettings.Get().GroupByProcessDefault;
			m_chkRememberWindowBounds.Checked = AppSettings.Get().RememberMainWindowBounds;

			m_bInitializing = false;
			m_bDirty = false;
		}

		public void ApplySettings() {
			AppSettings.Get().GroupByProcessDefault = m_chkGroupByProcessDefault.Checked;
			AppSettings.Get().RememberMainWindowBounds = m_chkRememberWindowBounds.Checked;

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
