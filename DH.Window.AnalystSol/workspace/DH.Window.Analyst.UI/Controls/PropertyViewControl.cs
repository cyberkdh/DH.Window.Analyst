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
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Controls {
	// reusable window-property preview: basic info + Win32 native details + child class summary. Designer-editable (see .Designer.cs)
	public partial class PropertyViewControl : UserControl {
		private IWindowTreeService m_treeService;

		// raised when the user double-clicks a Parent/Owner/First Child/Next/Previous row that carries a NavigationHandle
		public event EventHandler<IntPtr> WindowReferenceActivated;

		public PropertyViewControl() {
			InitializeComponent();

			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvBasic, "PropertyBasic", () => CopySelectedRows(m_lsvBasic));
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvExtended, "PropertyNativeDetails", () => CopySelectedRows(m_lsvExtended));
			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvChildSummary, "PropertyChildWindows", () => CopySelectedRows(m_lsvChildSummary));

			m_lsvExtended.DoubleClick += OnExtendedRowDoubleClick;
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
			if (item == null || m_treeService == null) {
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
		}

		private static ListViewItem AddRow(ListView lsv, string strname, string strvalue) {
			ListViewItem lvitem = new ListViewItem(strname);
			lvitem.SubItems.Add(strvalue);
			lsv.Items.Add(lvitem);
			return lvitem;
		}
	}
}
