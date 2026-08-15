//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;

namespace DH.Window.Analyst.UI.Dialogs {
	// classic Spy++-style Find Window dialog: Caption/ClassName/Handle always visible; an "Advanced" section adds
	// PID/ProcessName/ControlId (cheap Win32-level filters) and AutomationId/UIA Name/ControlType (expensive UIA subtree walk)
	public partial class FindWindowForm : Form {
		private readonly IWindowTreeService m_treeService;
		// stateless, so instantiated locally rather than threaded through the constructor like the shared IWindowTreeService
		private readonly IUiaSearchService m_uiaSearchService = new UiaSearchService();

		private bool m_bAdvancedExpanded;
		private CancellationTokenSource m_ctsSearch;

		public TopLevelWindowItem FoundWindow { get; private set; }

		public FindWindowForm(IWindowTreeService treeservice) {
			InitializeComponent();
			m_treeService = treeservice;

			// modal dialog owned by MainForm — must not get its own taskbar button (unlike DetachedWorkspaceForm, which is a real independent window)
			ShowInTaskbar = false;

			m_btnFinder.Initialize(treeservice);
			m_btnFinder.WindowPicked += (sender, item) => {
				m_txtCaption.Text = item.Title;
				m_txtClassName.Text = item.ClassName;
				m_txtHandle.Text = item.HandleText;
			};

			RelayoutDialog();
		}

		private void OnAdvancedToggleClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			m_bAdvancedExpanded = !m_bAdvancedExpanded;
			m_linkAdvanced.Text = m_bAdvancedExpanded == true ? "▾ Advanced" : "▸ Advanced";
			RelayoutDialog();
		}

		// repositions everything below the Advanced toggle based on current state (Advanced expanded/collapsed, results shown/not, search in progress/not) —
		// simpler and less error-prone than maintaining several fixed Designer layouts for every state combination
		private void RelayoutDialog() {
			int ny = m_linkAdvanced.Bottom + 8;

			m_pnlAdvanced.Visible = m_bAdvancedExpanded;
			if (m_bAdvancedExpanded == true) {
				m_pnlAdvanced.Location = new System.Drawing.Point(m_pnlAdvanced.Left, ny);
				ny = m_pnlAdvanced.Bottom + 8;
			}

			bool bshowresults = m_treeResults.Nodes.Count > 0;

			m_lblHint.Visible = bshowresults == false;
			if (bshowresults == false) {
				m_lblHint.Location = new System.Drawing.Point(m_lblHint.Left, ny);
				ny = m_lblHint.Bottom + 6;
			}

			m_treeResults.Visible = bshowresults;
			if (bshowresults == true) {
				m_treeResults.Location = new System.Drawing.Point(m_treeResults.Left, ny);
				ny = m_treeResults.Bottom + 8;
			}

			m_progressSearch.Location = new System.Drawing.Point(m_progressSearch.Left, ny + 3);
			m_btnCancelSearch.Location = new System.Drawing.Point(m_btnCancelSearch.Left, ny);
			if (m_progressSearch.Visible == true || m_btnCancelSearch.Visible == true) {
				ny = m_btnCancelSearch.Bottom + 8;
			}

			int nbuttony = ny;
			m_btnFind.Location = new System.Drawing.Point(m_btnFind.Left, nbuttony);
			m_btnSelectResult.Location = new System.Drawing.Point(m_btnSelectResult.Left, nbuttony);
			m_btnCancel.Location = new System.Drawing.Point(m_btnCancel.Left, nbuttony);

			ClientSize = new System.Drawing.Size(ClientSize.Width, m_btnCancel.Bottom + 12);
		}

		private void OnCriteriaTextChanged(object sender, EventArgs e) {
			m_btnFind.Enabled = string.IsNullOrWhiteSpace(m_txtCaption.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtClassName.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtHandle.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtPid.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtProcessName.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtControlId.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtAutomationId.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtUiaName.Text) == false
				|| string.IsNullOrWhiteSpace(m_txtControlType.Text) == false;
		}

		private async void OnFindClick(object sender, EventArgs e) {
			IntPtr handlefilter = IntPtr.Zero;
			if (string.IsNullOrWhiteSpace(m_txtHandle.Text) == false
				&& HandleTextParser.TryParse(m_txtHandle.Text, out handlefilter) == false) {
				MessageBox.Show(this, "Handle must be a hex value (e.g. 0x000A1B2C) or a decimal number.", "Find Window",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			int? npid = null;
			if (string.IsNullOrWhiteSpace(m_txtPid.Text) == false) {
				if (int.TryParse(m_txtPid.Text.Trim(), out int nparsedpid) == false) {
					MessageBox.Show(this, "PID must be a decimal number.", "Find Window", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				npid = nparsedpid;
			}

			int? ncontrolid = null;
			if (string.IsNullOrWhiteSpace(m_txtControlId.Text) == false) {
				if (int.TryParse(m_txtControlId.Text.Trim(), out int nparsedcontrolid) == false) {
					MessageBox.Show(this, "Control ID must be a decimal number.", "Find Window", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				ncontrolid = nparsedcontrolid;
			}

			WindowSearchCriteria criteria = new WindowSearchCriteria {
				Caption = m_txtCaption.Text,
				ClassName = m_txtClassName.Text,
				Handle = handlefilter,
				ProcessId = npid,
				ProcessName = m_txtProcessName.Text,
				ControlId = ncontrolid,
				AutomationId = m_txtAutomationId.Text,
				UiaName = m_txtUiaName.Text,
				ControlType = m_txtControlType.Text
			};

			List<TopLevelWindowItem> listmatches;

			if (criteria.HasUiaCriteria == true) {
				listmatches = await RunUiaSearchAsync(criteria);
				if (listmatches == null) {
					// cancelled — leave the dialog open as-is
					return;
				}
			}
			else {
				// Win32-only search is fast enough to stay synchronous, matching the dialog's prior behavior
				listmatches = m_treeService.FindWindows(criteria).ToList();
			}

			HandleSearchResults(listmatches);
		}

		private async Task<List<TopLevelWindowItem>> RunUiaSearchAsync(WindowSearchCriteria criteria) {
			SetSearchingState(true);
			m_ctsSearch = new CancellationTokenSource();

			try {
				// narrow the candidate set with the cheap Win32-level fields first, then only UIA-walk what's left
				List<TopLevelWindowItem> listcandidates = m_treeService.FindWindows(criteria).ToList();
				CancellationToken token = m_ctsSearch.Token;

				return await Task.Run(() => m_uiaSearchService.FindTopLevelWindows(listcandidates, criteria, m_treeService, token), token);
			}
			catch (OperationCanceledException) {
				return null;
			}
			finally {
				SetSearchingState(false);
				m_ctsSearch?.Dispose();
				m_ctsSearch = null;
			}
		}

		private void SetSearchingState(bool bsearching) {
			m_progressSearch.Visible = bsearching;
			m_btnCancelSearch.Visible = bsearching;
			m_btnFind.Enabled = bsearching == false;
			m_linkAdvanced.Enabled = bsearching == false;
			m_pnlAdvanced.Enabled = bsearching == false;
			m_txtCaption.Enabled = bsearching == false;
			m_txtClassName.Enabled = bsearching == false;
			m_txtHandle.Enabled = bsearching == false;
			RelayoutDialog();
		}

		private void OnCancelSearchClick(object sender, EventArgs e) {
			m_ctsSearch?.Cancel();
		}

		private void HandleSearchResults(List<TopLevelWindowItem> listmatches) {
			if (listmatches.Count == 0) {
				MessageBox.Show(this, "No matching window was found.", "Find Window",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (listmatches.Count == 1) {
				FoundWindow = listmatches[0];
				DialogResult = DialogResult.OK;
				Close();
				return;
			}

			ShowResultList(listmatches);
		}

		// groups matches by top-level ancestor (a process/search can legitimately have several top-level windows), with any matched
		// descendant window nested underneath its owning top-level window for context — but only the top-level node is selectable,
		// so e.g. a PID search that technically matches dozens of child controls never lets the user pick a non-top-level result
		private void ShowResultList(List<TopLevelWindowItem> listmatches) {
			m_treeResults.Nodes.Clear();

			Dictionary<IntPtr, TreeNode> dicgroupnodes = new Dictionary<IntPtr, TreeNode>();

			foreach (TopLevelWindowItem item in listmatches) {
				IntPtr handletoplevel = m_treeService.GetTopLevelAncestorHandle(item.Handle);

				if (dicgroupnodes.TryGetValue(handletoplevel, out TreeNode nodegroup) == false) {
					TopLevelWindowItem itemtoplevel = handletoplevel == item.Handle ? item : m_treeService.GetWindowItem(handletoplevel);
					if (itemtoplevel == null) {
						continue;
					}

					nodegroup = new TreeNode(FormatResultNodeText(itemtoplevel));
					nodegroup.Tag = itemtoplevel;
					m_treeResults.Nodes.Add(nodegroup);
					dicgroupnodes[handletoplevel] = nodegroup;
				}

				if (item.Handle != handletoplevel) {
					TreeNode nodechild = new TreeNode(FormatResultNodeText(item));
					nodegroup.Nodes.Add(nodechild);
				}
			}

			foreach (TreeNode nodegroup in m_treeResults.Nodes) {
				nodegroup.Expand();
			}

			m_btnFind.Visible = false;
			m_btnSelectResult.Visible = true;

			RelayoutDialog();
		}

		private static string FormatResultNodeText(TopLevelWindowItem item) {
			return $"{item.HandleText}  \"{item.Title}\"  [{item.ClassName}]  — {item.ProcessName}";
		}

		private void OnSelectResultClick(object sender, EventArgs e) {
			ConfirmSelectedResult();
		}

		private void OnResultDoubleClick(object sender, EventArgs e) {
			ConfirmSelectedResult();
		}

		private void OnResultAfterSelect(object sender, TreeViewEventArgs e) {
			m_btnSelectResult.Enabled = e.Node?.Tag is TopLevelWindowItem;
		}

		private void ConfirmSelectedResult() {
			TopLevelWindowItem item = m_treeResults.SelectedNode?.Tag as TopLevelWindowItem;
			if (item == null) {
				return;
			}

			FoundWindow = item;
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
