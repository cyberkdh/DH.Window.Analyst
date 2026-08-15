//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// scanned only on the explicit "Run Check" click — never automatically on tab or tree selection
	public partial class AccessibilityCheckControl : UserControl {
		// stateless, so instantiated locally rather than threaded through Initialize()
		private readonly IAccessibilityCheckService m_checkService = new AccessibilityCheckService();
		private IWindowTreeService m_treeService;
		private TopLevelWindowItem m_targetWindow;

		public AccessibilityCheckControl() {
			InitializeComponent();

			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvIssues, "AccessibilityIssues");
			m_btnRunCheck.Click += async (sender, e) => await RunCheckAsync();

			UpdateButtonState();
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
		}

		public void SetTargetWindow(TopLevelWindowItem item) {
			m_targetWindow = item;
			m_lsvIssues.Items.Clear();
			m_lblSummary.Text = "(Not checked yet)";
			UpdateButtonState();
		}

		private async Task RunCheckAsync() {
			if (m_targetWindow == null || m_treeService == null) {
				return;
			}

			m_btnRunCheck.Enabled = false;
			m_lsvIssues.Items.Clear();
			m_lblSummary.Text = "Checking...";

			List<AccessibilityIssueItem> listissues = await Task.Run(() => {
				AutomationElement root = m_treeService.CreateElementFromHandle(m_targetWindow.Handle);
				return root != null ? m_checkService.CheckSubtree(root) : new List<AccessibilityIssueItem>();
			});

			foreach (AccessibilityIssueItem issue in listissues) {
				ListViewItem lvitem = new ListViewItem(issue.Severity.ToString());
				lvitem.SubItems.Add(issue.RuleName);
				lvitem.SubItems.Add(issue.ElementName);
				lvitem.SubItems.Add(issue.ControlType);
				lvitem.SubItems.Add(issue.AutomationId);
				m_lsvIssues.Items.Add(lvitem);
			}

			int nerrors = listissues.Count(item => item.Severity == AccessibilitySeverity.Error);
			int nwarnings = listissues.Count(item => item.Severity == AccessibilitySeverity.Warning);
			int ninfo = listissues.Count(item => item.Severity == AccessibilitySeverity.Info);
			m_lblSummary.Text = $"{nerrors} Errors, {nwarnings} Warnings, {ninfo} Info";

			UpdateButtonState();
		}

		private void UpdateButtonState() {
			m_btnRunCheck.Enabled = m_targetWindow != null;
		}
	}
}
