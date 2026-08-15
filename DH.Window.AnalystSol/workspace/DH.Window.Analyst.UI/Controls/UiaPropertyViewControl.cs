//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using DH.Window.Analyst.Services.Automation;

namespace DH.Window.Analyst.UI.Controls {
	// UIA-element property preview: AutomationElement.Current fields + child ControlType summary + pattern actions
	public partial class UiaPropertyViewControl : UserControl {
		// stateless, so instantiated locally rather than threaded through Initialize() like the shared IWindowTreeService
		private readonly IPatternActionService m_patternService = new PatternActionService();

		private IWindowTreeService m_treeService;
		private AutomationElement m_currentElement;

		public UiaPropertyViewControl() {
			InitializeComponent();

			m_lsvPatterns.SelectedIndexChanged += OnPatternSelectedIndexChanged;
			m_btnInvoke.Click += OnInvokeClick;
			m_btnToggle.Click += OnToggleClick;
			m_btnExpand.Click += OnExpandClick;
			m_btnCollapse.Click += OnCollapseClick;
			m_btnSelect.Click += OnSelectClick;
			m_btnSetValue.Click += OnSetValueClick;
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
		}

		public async Task ShowElementAsync(AutomationElement element) {
			m_lsvBasic.Items.Clear();
			m_lsvChildSummary.Items.Clear();
			m_lsvPatterns.Items.Clear();
			HideAllActionControls();
			m_currentElement = element;
			if (element == null || m_treeService == null) {
				return;
			}

			// element may have been destroyed/invalidated since being captured, so every AutomationElement.Current access here is a fallible cross-process UIA call
			try {
				AutomationElement.AutomationElementInformation info = element.Current;
				AddRow(m_lsvBasic, "Name", info.Name);
				AddRow(m_lsvBasic, "ControlType", info.ControlType.LocalizedControlType);
				AddRow(m_lsvBasic, "AutomationId", info.AutomationId);
				AddRow(m_lsvBasic, "ClassName", info.ClassName);
				AddRow(m_lsvBasic, "FrameworkId", info.FrameworkId);
				AddRow(m_lsvBasic, "ProcessId", info.ProcessId.ToString());
				AddRow(m_lsvBasic, "NativeWindowHandle", info.NativeWindowHandle.ToString());
				AddRow(m_lsvBasic, "BoundingRectangle", info.BoundingRectangle.ToString());
				AddRow(m_lsvBasic, "IsEnabled", info.IsEnabled.ToString());
				AddRow(m_lsvBasic, "IsOffscreen", info.IsOffscreen.ToString());
				AddRow(m_lsvBasic, "IsKeyboardFocusable", info.IsKeyboardFocusable.ToString());
				AddRow(m_lsvBasic, "HasKeyboardFocus", info.HasKeyboardFocus.ToString());
				AddRow(m_lsvBasic, "AcceleratorKey", info.AcceleratorKey);
				AddRow(m_lsvBasic, "AccessKey", info.AccessKey);
				AddRow(m_lsvBasic, "HelpText", info.HelpText);

				List<AutomationElement> listchildren = await Task.Run(() => new List<AutomationElement>(m_treeService.GetChildren(element)));
				Dictionary<string, int> diccounts = new Dictionary<string, int>();
				foreach (AutomationElement child in listchildren) {
					string strkey = string.IsNullOrEmpty(child.Current.ControlType.LocalizedControlType) == false ? child.Current.ControlType.LocalizedControlType : "(Unknown)";
					diccounts[strkey] = diccounts.TryGetValue(strkey, out int ncurrent) == true ? ncurrent + 1 : 1;
				}
				foreach (KeyValuePair<string, int> pair in diccounts) {
					AddRow(m_lsvChildSummary, pair.Key, pair.Value.ToString());
				}

				foreach (AutomationPattern pattern in element.GetSupportedPatterns()) {
					string strpatternname = pattern.ProgrammaticName.Replace("PatternIdentifiers.Pattern", "");
					ListViewItem lvitem = new ListViewItem(strpatternname);
					lvitem.SubItems.Add(GetPatternStateText(strpatternname, element));
					lvitem.Tag = strpatternname;
					m_lsvPatterns.Items.Add(lvitem);
				}
			}
			catch (ElementNotAvailableException) {
				m_lsvBasic.Items.Clear();
				m_lsvChildSummary.Items.Clear();
				m_lsvPatterns.Items.Clear();
				AddRow(m_lsvBasic, "Name", "(Element is no longer available)");
				m_currentElement = null;
			}
		}

		private static void AddRow(ListView lsv, string strname, string strvalue) {
			ListViewItem lvitem = new ListViewItem(strname);
			lvitem.SubItems.Add(strvalue);
			lsv.Items.Add(lvitem);
		}

		private string GetPatternStateText(string strpatternname, AutomationElement element) {
			switch (strpatternname) {
				case "Toggle":
					return m_patternService.TryGetToggleState(element, out string strtogglestate) == true ? strtogglestate : string.Empty;
				case "ExpandCollapse":
					return m_patternService.TryGetExpandCollapseState(element, out string strexpandstate) == true ? strexpandstate : string.Empty;
				case "Value":
					return m_patternService.TryGetValue(element, out string strvalue, out bool breadonly) == true ? strvalue : string.Empty;
				default:
					return string.Empty;
			}
		}

		private void HideAllActionControls() {
			m_btnInvoke.Visible = false;
			m_btnToggle.Visible = false;
			m_btnExpand.Visible = false;
			m_btnCollapse.Visible = false;
			m_btnSelect.Visible = false;
			m_lblValue.Visible = false;
			m_txtValue.Visible = false;
			m_btnSetValue.Visible = false;
			m_lblNoAction.Visible = false;
			m_lblActionStatus.Visible = false;
		}

		// Try* returning false is silent by design at the service layer (cross-process UIA calls fail routinely), but a user clicking a button deserves to know why nothing happened — elevated targets (UIPI) are the most common real-world cause
		private void ShowActionFailed() {
			m_lblActionStatus.Text = "Action failed. The target may be running elevated (as Administrator), which blocks this from a non-elevated process.";
			m_lblActionStatus.Visible = true;
		}

		private void OnPatternSelectedIndexChanged(object sender, EventArgs e) {
			HideAllActionControls();
			if (m_lsvPatterns.SelectedItems.Count == 0 || m_currentElement == null) {
				return;
			}

			string strpatternname = (string)m_lsvPatterns.SelectedItems[0].Tag;
			switch (strpatternname) {
				case "Invoke":
					m_btnInvoke.Visible = true;
					break;
				case "Toggle":
					m_btnToggle.Visible = true;
					break;
				case "ExpandCollapse":
					m_patternService.TryGetExpandCollapseState(m_currentElement, out string strexpandstate);
					m_btnExpand.Visible = true;
					m_btnCollapse.Visible = true;
					m_btnExpand.Enabled = strexpandstate != "Expanded";
					m_btnCollapse.Enabled = strexpandstate != "Collapsed";
					break;
				case "SelectionItem":
					m_btnSelect.Visible = true;
					break;
				case "Value":
					m_patternService.TryGetValue(m_currentElement, out string strvalue, out bool breadonly);
					m_lblValue.Visible = true;
					m_txtValue.Visible = true;
					m_txtValue.Text = strvalue;
					m_txtValue.ReadOnly = breadonly;
					m_btnSetValue.Visible = true;
					m_btnSetValue.Enabled = breadonly == false;
					break;
				default:
					m_lblNoAction.Visible = true;
					break;
			}
		}

		private void RefreshSelectedPatternState() {
			if (m_lsvPatterns.SelectedItems.Count == 0 || m_currentElement == null) {
				return;
			}

			ListViewItem lvitem = m_lsvPatterns.SelectedItems[0];
			string strpatternname = (string)lvitem.Tag;
			lvitem.SubItems[1].Text = GetPatternStateText(strpatternname, m_currentElement);
			OnPatternSelectedIndexChanged(this, EventArgs.Empty);
		}

		private void OnInvokeClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (m_patternService.TryInvoke(m_currentElement) == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnToggleClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (m_patternService.TryToggle(m_currentElement) == true) {
				m_lblActionStatus.Visible = false;
				RefreshSelectedPatternState();
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnExpandClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (m_patternService.TryExpand(m_currentElement) == true) {
				m_lblActionStatus.Visible = false;
				RefreshSelectedPatternState();
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnCollapseClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (m_patternService.TryCollapse(m_currentElement) == true) {
				m_lblActionStatus.Visible = false;
				RefreshSelectedPatternState();
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnSelectClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (m_patternService.TrySelect(m_currentElement) == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnSetValueClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (m_patternService.TrySetValue(m_currentElement, m_txtValue.Text) == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
		}
	}
}
