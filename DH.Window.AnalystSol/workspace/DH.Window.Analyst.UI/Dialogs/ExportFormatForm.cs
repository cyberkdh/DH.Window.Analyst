//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Dialogs {
	// minimal shared "pick a format" dialog for controls (EventLog, MessageLog) whose Export button needs only
	// CSV/JSON, unlike WindowListExportForm which also needs Scope/Include-children/Structure options
	public partial class ExportFormatForm : Form {
		public bool FormatIsJson { get { return m_rbFormatJson.Checked; } }

		public ExportFormatForm() {
			InitializeComponent();
		}
	}
}
