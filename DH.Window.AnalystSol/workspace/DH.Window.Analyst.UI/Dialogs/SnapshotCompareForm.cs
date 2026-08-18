//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.UI.Utils;

namespace DH.Window.Analyst.UI.Dialogs {
	// modeless A/B diff viewer over MainForm's live snapshot list; explicit-trigger convention: no auto-refresh, diff rebuilds only on combo/checkbox change
	public partial class SnapshotCompareForm : Form {
		private static readonly string[] EXPORT_HEADERS = { "Property", "ValueA", "ValueB", "Changed" };
		private static readonly Color DIFF_HIGHLIGHT_COLOR = Color.FromArgb(0xFF, 0xCD, 0xD2);

		private readonly List<PropertySnapshot> m_listSnapshots;

		public SnapshotCompareForm(List<PropertySnapshot> listsnapshots) {
			InitializeComponent();

			m_listSnapshots = listsnapshots;

			ListViewRowInteractionHelper.AttachRowContextMenu(m_lsvDiff, "SnapshotDiff");

			m_cboSnapshotA.SelectedIndexChanged += (s, e) => RebuildDiff();
			m_cboSnapshotB.SelectedIndexChanged += (s, e) => RebuildDiff();
			m_chkOnlyDifferences.CheckedChanged += (s, e) => RebuildDiff();

			Shown += (s, e) => PopulateCombos();
		}

		private void OnRefreshListClick(object sender, System.EventArgs e) {
			PopulateCombos();
		}

		private void PopulateCombos() {
			PropertySnapshot snapselecteda = m_cboSnapshotA.SelectedItem as PropertySnapshot;
			PropertySnapshot snapselectedb = m_cboSnapshotB.SelectedItem as PropertySnapshot;

			m_cboSnapshotA.Items.Clear();
			m_cboSnapshotB.Items.Clear();
			foreach (PropertySnapshot snapshot in m_listSnapshots) {
				m_cboSnapshotA.Items.Add(snapshot);
				m_cboSnapshotB.Items.Add(snapshot);
			}

			m_lblEmpty.Visible = m_listSnapshots.Count < 2;

			int nindexa = snapselecteda != null ? m_listSnapshots.IndexOf(snapselecteda) : -1;
			int nindexb = snapselectedb != null ? m_listSnapshots.IndexOf(snapselectedb) : -1;
			if (m_listSnapshots.Count > 0) {
				m_cboSnapshotA.SelectedIndex = nindexa >= 0 ? nindexa : 0;
				m_cboSnapshotB.SelectedIndex = nindexb >= 0 ? nindexb : m_listSnapshots.Count - 1;
			}

			RebuildDiff();
		}

		private void RebuildDiff() {
			m_lsvDiff.Items.Clear();

			if (!(m_cboSnapshotA.SelectedItem is PropertySnapshot snapa) || !(m_cboSnapshotB.SelectedItem is PropertySnapshot snapb)) {
				return;
			}

			Dictionary<string, string> dica = ToValueMap(snapa.Items);
			Dictionary<string, string> dicb = ToValueMap(snapb.Items);

			HashSet<string> setseen = new HashSet<string>();
			List<string> listnames = new List<string>();
			foreach (PropertyItem prop in snapa.Items) {
				if (setseen.Add(prop.Name) == true) {
					listnames.Add(prop.Name);
				}
			}
			foreach (PropertyItem prop in snapb.Items) {
				if (setseen.Add(prop.Name) == true) {
					listnames.Add(prop.Name);
				}
			}

			foreach (string strname in listnames) {
				string strvaluea = dica.TryGetValue(strname, out string va) == true ? va : "N/A";
				string strvalueb = dicb.TryGetValue(strname, out string vb) == true ? vb : "N/A";
				bool bchanged = strvaluea != strvalueb;

				if (m_chkOnlyDifferences.Checked == true && bchanged == false) {
					continue;
				}

				ListViewItem lvitem = new ListViewItem(strname);
				lvitem.SubItems.Add(strvaluea);
				lvitem.SubItems.Add(strvalueb);
				if (bchanged == true) {
					lvitem.BackColor = DIFF_HIGHLIGHT_COLOR;
				}
				m_lsvDiff.Items.Add(lvitem);
			}
		}

		private static Dictionary<string, string> ToValueMap(IReadOnlyList<PropertyItem> listitems) {
			Dictionary<string, string> dic = new Dictionary<string, string>();
			foreach (PropertyItem prop in listitems) {
				dic[prop.Name] = prop.Value;
			}
			return dic;
		}

		private void OnExportDiffClick(object sender, System.EventArgs e) {
			if (m_lsvDiff.Items.Count == 0) {
				return;
			}

			using (ExportFormatForm dlgformat = new ExportFormatForm()) {
				if (dlgformat.ShowDialog(this) != DialogResult.OK) {
					return;
				}

				using (SaveFileDialog dlgsave = new SaveFileDialog()) {
					dlgsave.Filter = dlgformat.FormatIsJson == true
						? "JSON files (*.json)|*.json|All files (*.*)|*.*"
						: "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
					dlgsave.FileName = ExportFileNameHelper.BuildTimestampedFileName("SnapshotDiff", dlgformat.FormatIsJson == true ? "json" : "csv");

					if (dlgsave.ShowDialog(this) != DialogResult.OK) {
						return;
					}

					List<string[]> listrows = new List<string[]>();
					foreach (ListViewItem lvitem in m_lsvDiff.Items) {
						listrows.Add(new[] { lvitem.Text, lvitem.SubItems[1].Text, lvitem.SubItems[2].Text, lvitem.BackColor == DIFF_HIGHLIGHT_COLOR ? "Yes" : "No" });
					}

					string strcontent = dlgformat.FormatIsJson == true
						? RowExportWriter.BuildJsonFlat(EXPORT_HEADERS, listrows)
						: RowExportWriter.BuildDelimitedText(EXPORT_HEADERS, listrows, ",", true);
					File.WriteAllText(dlgsave.FileName, strcontent, Encoding.UTF8);
				}
			}
		}
	}
}
