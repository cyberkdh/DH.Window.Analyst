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
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// UIA-element property preview: AutomationElement.Current fields + child ControlType summary + pattern actions
	public partial class UiaPropertyViewControl : UserControl {
		// stateless, so instantiated locally rather than threaded through Initialize() like the shared IWindowTreeService
		private readonly IPatternActionService m_patternService = new PatternActionService();
		// Enable/Disable has no UIA pattern equivalent, so the Win32-side control service is reused whenever NativeWindowHandle != 0
		private readonly IWindowControlService m_controlService = new WindowControlService();

		private IWindowTreeService m_treeService;
		private AutomationElement m_currentElement;
		private IntPtr m_currentNativeHandle;

		// selector strings recomputed by BuildSuggestedSelector() every time ShowElementAsync runs; copied verbatim by the
		// Basic tab's "Copy Selector" context-menu items, independent of which row happens to be selected
		private string m_strSuggestedSelectorKv = string.Empty;
		private string m_strSuggestedSelectorXPath = string.Empty;

		public UiaPropertyViewControl() {
			InitializeComponent();

			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvBasic, "UiaBasic");
			ToolStripMenuItem itemcopyselectorkv = new ToolStripMenuItem("Copy Selector (key=value)");
			itemcopyselectorkv.Click += (s, e) => CopySuggestedSelector(false);
			ToolStripMenuItem itemcopyselectorxpath = new ToolStripMenuItem("Copy Selector (XPath)");
			itemcopyselectorxpath.Click += (s, e) => CopySuggestedSelector(true);
			m_lsvBasic.ContextMenuStrip.Items.Insert(1, itemcopyselectorkv);
			m_lsvBasic.ContextMenuStrip.Items.Insert(2, itemcopyselectorxpath);
			m_lsvBasic.ContextMenuStrip.Opening += (s, e) => {
				itemcopyselectorkv.Visible = m_currentElement != null;
				itemcopyselectorxpath.Visible = m_currentElement != null;
			};

			m_lsvPatterns.SelectedIndexChanged += OnPatternSelectedIndexChanged;
			m_btnInvoke.Click += OnInvokeClick;
			m_btnToggle.Click += OnToggleClick;
			m_btnExpand.Click += OnExpandClick;
			m_btnCollapse.Click += OnCollapseClick;
			m_btnSelect.Click += OnSelectClick;
			m_btnSetValue.Click += OnSetValueClick;
			m_btnMinimize.Click += OnMinimizeClick;
			m_btnMaximize.Click += OnMaximizeClick;
			m_btnRestore.Click += OnRestoreClick;
			m_btnClose.Click += OnCloseClick;
			m_btnMove.Click += OnMoveClick;
			m_btnResize.Click += OnResizeClick;
			m_btnRotate.Click += OnRotateClick;
			m_btnNativeEnable.Click += (s, e) => ExecuteNativeAction(() => m_controlService.TryEnable(m_currentNativeHandle));
			m_btnNativeDisable.Click += (s, e) => ExecuteNativeAction(() => m_controlService.TryDisable(m_currentNativeHandle));
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
			m_currentNativeHandle = IntPtr.Zero;
			m_strSuggestedSelectorKv = string.Empty;
			m_strSuggestedSelectorXPath = string.Empty;
			if (element == null || m_treeService == null) {
				return;
			}

			// element may have been destroyed/invalidated since being captured, so every AutomationElement.Current access here is a fallible cross-process UIA call
			try {
				AutomationElement.AutomationElementInformation info = element.Current;
				m_currentNativeHandle = new IntPtr(info.NativeWindowHandle);
				RefreshNativeWindowActionsVisibility();
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

				string strreliability = BuildSuggestedSelector(info);
				ListViewItem lvitemselector = AddRow(m_lsvBasic, "Suggested Selector", $"{m_strSuggestedSelectorKv}  [{strreliability}]");
				lvitemselector.Font = new System.Drawing.Font(m_lsvBasic.Font, System.Drawing.FontStyle.Bold);
				lvitemselector.BackColor = System.Drawing.Color.LightYellow;

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
				m_currentNativeHandle = IntPtr.Zero;
				m_btnNativeEnable.Visible = false;
				m_btnNativeDisable.Visible = false;
				m_lblNativeActions.Visible = false;
			}
		}

		// live re-query of AutomationElement.Current at capture time (never reads m_lsvBasic's already-rendered rows, which can be
		// stale relative to the live element) — single node only, deliberately does not walk the child subtree (performance)
		public async Task<PropertySnapshot> CaptureSnapshotAsync() {
			if (m_currentElement == null) {
				return null;
			}

			try {
				AutomationElement.AutomationElementInformation info = await Task.Run(() => m_currentElement.Current);

				List<PropertyItem> listitems = new List<PropertyItem> {
					new PropertyItem("Name", info.Name),
					new PropertyItem("ControlType", info.ControlType.LocalizedControlType),
					new PropertyItem("AutomationId", info.AutomationId),
					new PropertyItem("ClassName", info.ClassName),
					new PropertyItem("FrameworkId", info.FrameworkId),
					new PropertyItem("ProcessId", info.ProcessId.ToString()),
					new PropertyItem("NativeWindowHandle", info.NativeWindowHandle.ToString()),
					new PropertyItem("BoundingRectangle", info.BoundingRectangle.ToString()),
					new PropertyItem("IsEnabled", info.IsEnabled.ToString()),
					new PropertyItem("IsOffscreen", info.IsOffscreen.ToString()),
					new PropertyItem("IsKeyboardFocusable", info.IsKeyboardFocusable.ToString()),
					new PropertyItem("HasKeyboardFocus", info.HasKeyboardFocus.ToString()),
					new PropertyItem("AcceleratorKey", info.AcceleratorKey),
					new PropertyItem("AccessKey", info.AccessKey),
					new PropertyItem("HelpText", info.HelpText)
				};

				return new PropertySnapshot(new IntPtr(info.NativeWindowHandle), info.Name, info.ClassName, GetProcessNameSafe(info.ProcessId), DateTime.Now, listitems);
			}
			catch (ElementNotAvailableException) {
				return null;
			}
		}

		private static string GetProcessNameSafe(int nprocessid) {
			try {
				using (System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(nprocessid)) {
					return process.ProcessName;
				}
			}
			catch (Exception) {
				return $"PID {nprocessid}";
			}
		}

		// AutomationId is preferred (survives localization/UI text changes); Name+ControlType is the fallback most
		// automation authors reach for next; ClassName+ControlType is the weakest signal (ambiguous with repeated controls).
		// Returns the reliability label ("Good"/"Fair"/"Poor") shown next to the value in the Basic tab row
		private string BuildSuggestedSelector(AutomationElement.AutomationElementInformation info) {
			string strcontroltypetoken = GetControlTypeToken(info.ControlType);

			if (string.IsNullOrEmpty(info.AutomationId) == false) {
				m_strSuggestedSelectorKv = $"AutomationId={info.AutomationId}";
				m_strSuggestedSelectorXPath = $"//{strcontroltypetoken}[@AutomationId='{info.AutomationId}']";
				return "Good";
			}

			if (string.IsNullOrEmpty(info.Name) == false) {
				m_strSuggestedSelectorKv = $"Name='{info.Name}' && ControlType={strcontroltypetoken}";
				m_strSuggestedSelectorXPath = $"//{strcontroltypetoken}[@Name='{info.Name}']";
				return "Fair";
			}

			if (string.IsNullOrEmpty(info.ClassName) == false) {
				m_strSuggestedSelectorKv = $"ClassName={info.ClassName} && ControlType={strcontroltypetoken}";
				m_strSuggestedSelectorXPath = $"//{strcontroltypetoken}[@ClassName='{info.ClassName}']";
				return "Poor";
			}

			m_strSuggestedSelectorKv = "(no reliable identifier)";
			m_strSuggestedSelectorXPath = string.Empty;
			return "Poor";
		}

		// ProgrammaticName looks like "ControlType.Button"; the XPath convention only wants the "Button" part
		private static string GetControlTypeToken(ControlType controltype) {
			string strprogrammatic = controltype.ProgrammaticName;
			int nindex = strprogrammatic.IndexOf('.');
			return nindex >= 0 ? strprogrammatic.Substring(nindex + 1) : strprogrammatic;
		}

		private void CopySuggestedSelector(bool busexpath) {
			if (m_currentElement == null) {
				return;
			}

			string strvalue = busexpath == true ? m_strSuggestedSelectorXPath : m_strSuggestedSelectorKv;
			if (string.IsNullOrEmpty(strvalue) == false) {
				Clipboard.SetText(strvalue);
			}
		}

		private static ListViewItem AddRow(ListView lsv, string strname, string strvalue) {
			ListViewItem lvitem = new ListViewItem(strname);
			lvitem.SubItems.Add(strvalue);
			lsv.Items.Add(lvitem);
			return lvitem;
		}

		private string GetPatternStateText(string strpatternname, AutomationElement element) {
			switch (strpatternname) {
				case "Toggle":
					return m_patternService.TryGetToggleState(element, out string strtogglestate) == true ? strtogglestate : string.Empty;
				case "ExpandCollapse":
					return m_patternService.TryGetExpandCollapseState(element, out string strexpandstate) == true ? strexpandstate : string.Empty;
				case "Value":
					return m_patternService.TryGetValue(element, out string strvalue, out bool breadonly) == true ? strvalue : string.Empty;
				case "Window":
					return m_patternService.TryGetWindowVisualState(element, out string strwindowstate) == true ? strwindowstate : string.Empty;
				case "Transform":
					if (m_patternService.TryGetTransformCapabilities(element, out bool bcanmove, out bool bcanresize, out bool bcanrotate) == false) {
						return string.Empty;
					}
					List<string> listcaps = new List<string>();
					if (bcanmove == true) { listcaps.Add("Move"); }
					if (bcanresize == true) { listcaps.Add("Resize"); }
					if (bcanrotate == true) { listcaps.Add("Rotate"); }
					return string.Join("/", listcaps);
				default:
					return string.Empty;
			}
		}

		// pattern-specific controls only — Native Window Actions (Enable/Disable) are independent of pattern selection, driven instead by RefreshNativeWindowActionsVisibility()
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
			m_btnMinimize.Visible = false;
			m_btnMaximize.Visible = false;
			m_btnRestore.Visible = false;
			m_btnClose.Visible = false;
			m_lblMoveX.Visible = false;
			m_txtMoveX.Visible = false;
			m_lblMoveY.Visible = false;
			m_txtMoveY.Visible = false;
			m_btnMove.Visible = false;
			m_lblResizeW.Visible = false;
			m_txtResizeW.Visible = false;
			m_lblResizeH.Visible = false;
			m_txtResizeH.Visible = false;
			m_btnResize.Visible = false;
			m_lblRotate.Visible = false;
			m_txtRotate.Visible = false;
			m_btnRotate.Visible = false;
		}

		// Enable/Disable has no UIA-pattern equivalent, so this section is shown whenever the element carries a real HWND, independent of which pattern row is selected
		private void RefreshNativeWindowActionsVisibility() {
			bool bhashandle = m_currentNativeHandle != IntPtr.Zero;
			m_lblNativeActions.Visible = bhashandle;
			m_btnNativeEnable.Visible = bhashandle;
			m_btnNativeDisable.Visible = bhashandle;
			if (bhashandle == false) {
				return;
			}

			bool bisownprocess = m_controlService.IsOwnProcessWindow(m_currentNativeHandle);
			m_btnNativeDisable.Enabled = bisownprocess == false;
		}

		private void ExecuteNativeAction(Func<bool> action) {
			if (m_currentNativeHandle == IntPtr.Zero) {
				return;
			}

			if (action() == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
			RefreshNativeWindowActionsVisibility();
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
				case "Window":
					m_patternService.TryGetWindowVisualState(m_currentElement, out string strwindowstate);
					m_btnMinimize.Visible = true;
					m_btnMaximize.Visible = true;
					m_btnRestore.Visible = true;
					m_btnClose.Visible = true;
					m_btnMinimize.Enabled = strwindowstate != "Minimized";
					m_btnMaximize.Enabled = strwindowstate != "Maximized";
					m_btnRestore.Enabled = strwindowstate != "Normal";
					break;
				case "Transform":
					m_patternService.TryGetTransformCapabilities(m_currentElement, out bool bcanmove, out bool bcanresize, out bool bcanrotate);
					m_lblMoveX.Visible = true;
					m_txtMoveX.Visible = true;
					m_lblMoveY.Visible = true;
					m_txtMoveY.Visible = true;
					m_btnMove.Visible = true;
					m_btnMove.Enabled = bcanmove;
					m_lblResizeW.Visible = true;
					m_txtResizeW.Visible = true;
					m_lblResizeH.Visible = true;
					m_txtResizeH.Visible = true;
					m_btnResize.Visible = true;
					m_btnResize.Enabled = bcanresize;
					m_lblRotate.Visible = true;
					m_txtRotate.Visible = true;
					m_btnRotate.Visible = true;
					m_btnRotate.Enabled = bcanrotate;
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

		private void OnMinimizeClick(object sender, EventArgs e) {
			ExecuteWindowAction(() => m_patternService.TryMinimize(m_currentElement));
		}

		private void OnMaximizeClick(object sender, EventArgs e) {
			ExecuteWindowAction(() => m_patternService.TryMaximize(m_currentElement));
		}

		private void OnRestoreClick(object sender, EventArgs e) {
			ExecuteWindowAction(() => m_patternService.TryRestore(m_currentElement));
		}

		private void OnCloseClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (m_patternService.TryClose(m_currentElement) == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
		}

		private void ExecuteWindowAction(Func<bool> action) {
			if (m_currentElement == null) {
				return;
			}

			if (action() == true) {
				m_lblActionStatus.Visible = false;
				RefreshSelectedPatternState();
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnMoveClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (double.TryParse(m_txtMoveX.Text, out double dx) == false || double.TryParse(m_txtMoveY.Text, out double dy) == false) {
				MessageBox.Show("X/Y must be numbers.", "Transform", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (m_patternService.TryMove(m_currentElement, dx, dy) == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnResizeClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (double.TryParse(m_txtResizeW.Text, out double dwidth) == false || double.TryParse(m_txtResizeH.Text, out double dheight) == false || dwidth <= 0 || dheight <= 0) {
				MessageBox.Show("Width/Height must be positive numbers.", "Transform", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (m_patternService.TryResize(m_currentElement, dwidth, dheight) == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
		}

		private void OnRotateClick(object sender, EventArgs e) {
			if (m_currentElement == null) {
				return;
			}

			if (double.TryParse(m_txtRotate.Text, out double ddegrees) == false) {
				MessageBox.Show("Degrees must be a number.", "Transform", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (m_patternService.TryRotate(m_currentElement, ddegrees) == true) {
				m_lblActionStatus.Visible = false;
			}
			else {
				ShowActionFailed();
			}
		}
	}
}
