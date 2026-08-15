//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Dialogs.Options {
	// Ported from DHToolSol's FrmOptionMain; OK stays disabled until any page reports itself dirty.
	public partial class OptionsForm : Form {
		private readonly List<IOptionPage> m_listPages = new List<IOptionPage>();

		public OptionsForm() {
			InitializeComponent();

			// modal dialog owned by MainForm — must not get its own taskbar button (unlike DetachedWorkspaceForm, which is a real independent window)
			ShowInTaskbar = false;

			AddPage(new GeneralOptionPage());
			AddPage(new LoggingOptionPage());
			AddPage(new InspectorOptionPage());

			if (m_lstCategories.Items.Count > 0) {
				m_lstCategories.SelectedIndex = 0;
			}
		}

		private void AddPage(IOptionPage page) {
			m_listPages.Add(page);
			page.DirtyChanged += OnPageDirtyChanged;
			page.LoadSettings();
			m_lstCategories.Items.Add(page.Title);
		}

		private void OnPageDirtyChanged(object sender, EventArgs e) {
			m_btnOK.Enabled = m_listPages.Exists(page => page.IsDirty() == true);
		}

		private void OnCategorySelectedIndexChanged(object sender, EventArgs e) {
			m_panelPage.Controls.Clear();
			if (m_lstCategories.SelectedIndex < 0) {
				return;
			}

			UserControl ctrlpage = m_listPages[m_lstCategories.SelectedIndex].GetControl();
			ctrlpage.Dock = DockStyle.Fill;
			m_panelPage.Controls.Add(ctrlpage);
		}

		private void OnOKClick(object sender, EventArgs e) {
			foreach (IOptionPage page in m_listPages) {
				if (page.IsDirty() == true) {
					page.ApplySettings();
				}
			}

			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
