//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Dialogs {
	// minimal shared "pick a format" dialog; unlike WindowListExportForm, only needs CSV/JSON, no Scope/Structure options
	public partial class ExportFormatForm : Form {
		public bool FormatIsJson { get { return m_rbFormatJson.Checked; } }

		public ExportFormatForm() {
			InitializeComponent();
		}
	}
}
