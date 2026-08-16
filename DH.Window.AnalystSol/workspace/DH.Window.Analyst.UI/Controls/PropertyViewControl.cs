//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// reusable window-property preview: basic info + Win32 native details + child class summary. Designer-editable (see .Designer.cs)
	public partial class PropertyViewControl : UserControl {
		private IWindowTreeService m_treeService;
		// IPatternActionService precedent: a stateless action service is owned locally by the control that uses it, not injected
		private readonly IWindowControlService m_controlService = new WindowControlService();

		private TopLevelWindowItem m_currentItem;
		// guards against TrySetTopmost firing while ShowWindowAsync/RefreshWindowControlState is programmatically setting m_chkTopmost.Checked
		private bool m_bupdatingWindowControlUi;

		// raised when the user double-clicks a Parent/Owner/First Child/Next/Previous row that carries a NavigationHandle
		public event EventHandler<IntPtr> WindowReferenceActivated;

		public PropertyViewControl() {
			InitializeComponent();

			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvBasic, "PropertyBasic", () => CopySelectedRows(m_lsvBasic));
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvExtended, "PropertyNativeDetails", () => CopySelectedRows(m_lsvExtended));
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvChildSummary, "PropertyChildWindows", () => CopySelectedRows(m_lsvChildSummary));

			m_lsvExtended.DoubleClick += OnExtendedRowDoubleClick;

			m_btnShow.Click += (s, e) => ExecuteControlAction(() => m_controlService.TryShow(m_currentItem.Handle), "Show");
			m_btnHide.Click += (s, e) => ExecuteControlAction(() => m_controlService.TryHide(m_currentItem.Handle), "Hide");
			m_btnEnable.Click += (s, e) => ExecuteControlAction(() => m_controlService.TryEnable(m_currentItem.Handle), "Enable");
			m_btnDisable.Click += (s, e) => ExecuteControlAction(() => m_controlService.TryDisable(m_currentItem.Handle), "Disable");
			m_chkTopmost.CheckedChanged += OnTopmostCheckedChanged;
			m_btnApplyBounds.Click += OnApplyBoundsClick;
			m_btnRefreshBounds.Click += (s, e) => RefreshWindowControlState();
		}

		private void OnExtendedRowDoubleClick(object sender, EventArgs e) {
			if (m_lsvExtended.SelectedItems.Count == 0) {
				return;
			}

			if (m_lsvExtended.SelectedItems[0].Tag is PropertyItem prop) {
				if (prop.NavigationHandle.HasValue == true) {
					WindowReferenceActivated?.Invoke(this, prop.NavigationHandle.Value);
				}
				else if (prop.FlagDetails != null) {
					ShowFlagDetailsPopup(prop);
				}
			}
		}

		// Spy++ "Styles" tab style: each individually named flag on its own line, since the packed
		// "0x... A | B | C" value in the grid row is too long to read at a glance
		private static void ShowFlagDetailsPopup(PropertyItem prop) {
			string strmessage = string.Join(Environment.NewLine, prop.FlagDetails);
			MessageBox.Show(strmessage, prop.Name, MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		// "Copy" puts "Name: Value" lines on the clipboard, Spy++ style — passed as the custom copy action to
		// ListViewRowInteractionHelper.AttachRowContextMenu since these are name/value lists, unlike the
		// tab-delimited full-row copy the helper defaults to for multi-column lists
		private static void CopySelectedRows(ListView lsv) {
			if (lsv.SelectedItems.Count == 0) {
				return;
			}

			StringBuilder sbtext = new StringBuilder();
			foreach (ListViewItem lvitem in lsv.SelectedItems) {
				string strvalue = lvitem.SubItems.Count > 1 ? lvitem.SubItems[1].Text : string.Empty;
				sbtext.AppendLine($"{lvitem.Text}: {strvalue}");
			}

			Clipboard.SetText(sbtext.ToString());
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
		}

		// per-selection enrichment only; never called for the bulk window list
		public async Task ShowWindowAsync(TopLevelWindowItem item) {
			m_lsvBasic.Items.Clear();
			m_lsvExtended.Items.Clear();
			m_lsvChildSummary.Items.Clear();
			m_currentItem = item;
			if (item == null || m_treeService == null) {
				RefreshWindowControlState();
				return;
			}

			AddRow(m_lsvBasic, "Title", item.Title);
			AddRow(m_lsvBasic, "Class", item.ClassName);
			AddRow(m_lsvBasic, "Handle", item.HandleText);
			AddRow(m_lsvBasic, "Process", item.ProcessName);
			AddRow(m_lsvBasic, "PID", item.ProcessId.ToString());

			List<PropertyItem> listextended = await Task.Run(() => new List<PropertyItem>(m_treeService.GetNativeWindowDetails(item.Handle)));
			foreach (PropertyItem prop in listextended) {
				ListViewItem lvitem = AddRow(m_lsvExtended, prop.Name, prop.Value);
				if (prop.NavigationHandle.HasValue == true) {
					lvitem.Tag = prop;
					lvitem.Font = new System.Drawing.Font(m_lsvExtended.Font, System.Drawing.FontStyle.Underline);
					lvitem.ForeColor = System.Drawing.Color.RoyalBlue;
				}
				else if (prop.FlagDetails != null) {
					lvitem.Tag = prop;
					lvitem.Font = new System.Drawing.Font(m_lsvExtended.Font, System.Drawing.FontStyle.Underline);
					lvitem.ForeColor = System.Drawing.Color.DarkGreen;
				}
			}

			List<TopLevelWindowItem> listchildren = await Task.Run(() => new List<TopLevelWindowItem>(m_treeService.GetChildWindowInfos(item.Handle)));
			Dictionary<string, int> diccounts = new Dictionary<string, int>();
			foreach (TopLevelWindowItem child in listchildren) {
				string strkey = string.IsNullOrEmpty(child.ClassName) == true ? "(Unknown)" : child.ClassName;
				diccounts[strkey] = diccounts.TryGetValue(strkey, out int ncurrent) == true ? ncurrent + 1 : 1;
			}
			foreach (KeyValuePair<string, int> pair in diccounts) {
				AddRow(m_lsvChildSummary, pair.Key, pair.Value.ToString());
			}

			RefreshWindowControlState();
		}

		// Property Compare feature: re-queries live Win32 Native Details at the moment of capture (not whatever is currently
		// rendered) so a Snapshot always reflects the window's actual state, even if it moved/resized since the last
		// Refresh Property — explicit trigger only, called from MainForm's "Take Property Snapshot" menu item
		public async Task<PropertySnapshot> CaptureSnapshotAsync() {
			if (m_currentItem == null || m_treeService == null) {
				return null;
			}

			List<PropertyItem> listitems = new List<PropertyItem> {
				new PropertyItem("Title", m_currentItem.Title),
				new PropertyItem("Class", m_currentItem.ClassName),
				new PropertyItem("Handle", m_currentItem.HandleText),
				new PropertyItem("Process", m_currentItem.ProcessName),
				new PropertyItem("PID", m_currentItem.ProcessId.ToString())
			};

			List<PropertyItem> listextended = await Task.Run(() => new List<PropertyItem>(m_treeService.GetNativeWindowDetails(m_currentItem.Handle)));
			listitems.AddRange(listextended);

			return new PropertySnapshot(m_currentItem.Handle, m_currentItem.Title, m_currentItem.ClassName, m_currentItem.ProcessName, DateTime.Now, listitems);
		}

		private static ListViewItem AddRow(ListView lsv, string strname, string strvalue) {
			ListViewItem lvitem = new ListViewItem(strname);
			lvitem.SubItems.Add(strvalue);
			lsv.Items.Add(lvitem);
			return lvitem;
		}

		// runs a Window Control action, logs it (AppLog user-action convention), then re-queries current state so the tab reflects the result immediately
		private void ExecuteControlAction(Func<bool> action, string stractionname) {
			if (m_currentItem == null) {
				return;
			}

			bool bsuccess = action();
			AppLog.i($"Window Control {stractionname}: {m_currentItem.ClassName} \"{m_currentItem.Title}\" (handle 0x{m_currentItem.Handle.ToInt64():X}) — {(bsuccess == true ? "OK" : "FAILED")}");
			RefreshWindowControlState();
		}

		private void OnTopmostCheckedChanged(object sender, EventArgs e) {
			// re-entrant guard: RefreshWindowControlState() also sets m_chkTopmost.Checked to reflect actual state, which must not itself trigger TrySetTopmost
			if (m_bupdatingWindowControlUi == true || m_currentItem == null) {
				return;
			}

			ExecuteControlAction(() => m_controlService.TrySetTopmost(m_currentItem.Handle, m_chkTopmost.Checked), m_chkTopmost.Checked == true ? "Always On Top ON" : "Always On Top OFF");
		}

		private void OnApplyBoundsClick(object sender, EventArgs e) {
			if (m_currentItem == null) {
				return;
			}

			if (int.TryParse(m_txtX.Text, out int nx) == false || int.TryParse(m_txtY.Text, out int ny) == false ||
				int.TryParse(m_txtWidth.Text, out int nwidth) == false || int.TryParse(m_txtHeight.Text, out int nheight) == false) {
				MessageBox.Show("X/Y/Width/Height must all be whole numbers.", "Window Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (nwidth <= 0 || nheight <= 0) {
				MessageBox.Show("Width and Height must be greater than zero.", "Window Control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			ExecuteControlAction(() => m_controlService.TryMoveResize(m_currentItem.Handle, nx, ny, nwidth, nheight), $"Move/Resize to ({nx}, {ny}, {nwidth}x{nheight})");
		}

		// re-queries live Win32 state for the Window Control tab (Enabled/Topmost/bounds) and applies the self-process Hide/Disable guard;
		// called on selection change, after every action, and from the "Refresh Current" button (explicit trigger, no auto-polling)
		private void RefreshWindowControlState() {
			m_bupdatingWindowControlUi = true;
			try {
				bool bhasitem = m_currentItem != null;

				m_btnShow.Enabled = bhasitem;
				m_btnEnable.Enabled = bhasitem;
				m_chkTopmost.Enabled = bhasitem;
				m_btnApplyBounds.Enabled = bhasitem;
				m_btnRefreshBounds.Enabled = bhasitem;

				bool bisownprocess = bhasitem == true && m_controlService.IsOwnProcessWindow(m_currentItem.Handle);
				m_btnHide.Enabled = bhasitem && bisownprocess == false;
				m_btnDisable.Enabled = bhasitem && bisownprocess == false;
				m_toolTip.SetToolTip(m_btnHide, bisownprocess == true ? "Cannot hide this application's own window." : string.Empty);
				m_toolTip.SetToolTip(m_btnDisable, bisownprocess == true ? "Cannot disable this application's own window." : string.Empty);

				if (bhasitem == false) {
					m_chkTopmost.Checked = false;
					m_txtX.Text = m_txtY.Text = m_txtWidth.Text = m_txtHeight.Text = string.Empty;
					return;
				}

				m_chkTopmost.Checked = m_controlService.IsTopmost(m_currentItem.Handle);

				if (m_controlService.TryGetBounds(m_currentItem.Handle, out System.Drawing.Rectangle rect) == true) {
					m_txtX.Text = rect.X.ToString();
					m_txtY.Text = rect.Y.ToString();
					m_txtWidth.Text = rect.Width.ToString();
					m_txtHeight.Text = rect.Height.ToString();
				}
			}
			finally {
				m_bupdatingWindowControlUi = false;
			}
		}
	}
}
