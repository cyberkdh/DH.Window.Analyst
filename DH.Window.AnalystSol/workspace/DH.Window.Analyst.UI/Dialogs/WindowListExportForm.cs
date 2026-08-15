//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Dialogs {
	// options dialog for WindowListControl's Export action; Structure only matters for JSON (CSV is always a flat table), so the group is disabled when CSV is selected
	public partial class WindowListExportForm : Form {
		public bool FormatIsJson { get { return m_rbFormatJson.Checked; } }

		public bool ScopeIsSelectedOnly { get { return m_rbScopeSelected.Checked; } }

		public bool IncludeChildWindows { get { return m_chkIncludeChildren.Checked; } }

		public bool StructureIsTree { get { return m_rbStructureTree.Checked; } }

		public WindowListExportForm(bool bhasselection) {
			InitializeComponent();

			m_rbScopeSelected.Enabled = bhasselection;
			UpdateStructureEnabled();
		}

		private void OnFormatChanged(object sender, EventArgs e) {
			UpdateStructureEnabled();
		}

		private void UpdateStructureEnabled() {
			m_grpStructure.Enabled = m_rbFormatJson.Checked;
		}
	}
}
