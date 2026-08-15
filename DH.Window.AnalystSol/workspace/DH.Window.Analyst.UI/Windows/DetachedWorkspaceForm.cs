//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using DH.Window.Analyst.UI.Controls;

namespace DH.Window.Analyst.UI.Windows {
	// Hosts a tab "torn off" from WorkspaceControl; "Dock Back" re-attaches it via the detach-time callback.
	public partial class DetachedWorkspaceForm : Form {
		public DetachedWorkspaceForm(WindowWorkspaceTabControl ctrlworkspacetab, string strtitle, Action actionDockBack) {
			InitializeComponent();

			// intentionally an independent top-level window (no Owner) with its own taskbar entry — the user may move it to
			// another monitor or switch to it via Alt+Tab/taskbar directly, same as a detached VS Code panel or browser tab
			ShowInTaskbar = true;

			ctrlworkspacetab.Dock = DockStyle.Fill;
			Controls.Add(ctrlworkspacetab);
			Text = string.IsNullOrEmpty(strtitle) == false ? strtitle : "DH.Window.Analyst";

			m_btnDockBack.Click += (sender, e) => {
				Controls.Remove(ctrlworkspacetab);
				actionDockBack();
				Close();
			};
		}
	}
}
