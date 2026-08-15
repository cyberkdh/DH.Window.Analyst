//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DH.Window.Analyst.UI.Dialogs;

namespace DH.Window.Analyst.UI.Utils {
	// ListView-to-rows adapter; the actual CSV/JSON text formatting lives in RowExportWriter so it's shared with non-ListView sources (e.g. WindowListControl's TreeView)
	public static class ListViewExportHelper {
		// Exports selected rows if any are selected, otherwise every row.
		public static void CopyToClipboard(ListView lsv) {
			if (HasAnyRow(lsv) == false) {
				return;
			}

			string strtext = RowExportWriter.BuildDelimitedText(ExtractHeaders(lsv), ExtractRows(lsv), "\t", false);
			Clipboard.SetText(strtext);
		}

		// single entry point behind the control's "Export..." button — shows the CSV/JSON format picker, then a SaveFileDialog.
		// Exports the current selection if any rows are selected, otherwise every row.
		public static void Export(ListView lsv, string strdefaultbasename) {
			if (HasAnyRow(lsv) == false) {
				return;
			}

			ExportRows(ExtractHeaders(lsv), ExtractRows(lsv), strdefaultbasename);
		}

		// behind a "Export All..." context menu item — always exports every row, ignoring the current selection
		// (paired with a selection-scoped "Copy", so the two menu items never do the same thing)
		public static void ExportAll(ListView lsv, string strdefaultbasename) {
			if (lsv.Items.Count == 0) {
				return;
			}

			List<string[]> listrows = new List<string[]>();
			foreach (ListViewItem lvitem in lsv.Items) {
				listrows.Add(ExtractRow(lvitem));
			}

			ExportRows(ExtractHeaders(lsv), listrows, strdefaultbasename);
		}

		private static void ExportRows(string[] arrheaders, List<string[]> listrows, string strdefaultbasename) {
			using (ExportFormatForm dlgformat = new ExportFormatForm()) {
				if (dlgformat.ShowDialog() != DialogResult.OK) {
					return;
				}

				using (var dlgsave = new SaveFileDialog()) {
					if (dlgformat.FormatIsJson == true) {
						dlgsave.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
						dlgsave.FileName = ExportFileNameHelper.BuildTimestampedFileName(strdefaultbasename, "json");
					}
					else {
						dlgsave.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
						dlgsave.FileName = ExportFileNameHelper.BuildTimestampedFileName(strdefaultbasename, "csv");
					}

					if (dlgsave.ShowDialog() != DialogResult.OK) {
						return;
					}

					string strcontent = dlgformat.FormatIsJson == true
						? RowExportWriter.BuildJsonFlat(arrheaders, listrows)
						: RowExportWriter.BuildDelimitedText(arrheaders, listrows, ",", true);
					File.WriteAllText(dlgsave.FileName, strcontent, Encoding.UTF8);
				}
			}
		}

		private static bool HasAnyRow(ListView lsv) {
			return (lsv.SelectedItems.Count > 0 ? lsv.SelectedItems.Count : lsv.Items.Count) > 0;
		}

		private static string[] ExtractHeaders(ListView lsv) {
			string[] arrheaders = new string[lsv.Columns.Count];
			for (int ncol = 0; ncol < lsv.Columns.Count; ncol++) {
				arrheaders[ncol] = lsv.Columns[ncol].Text;
			}
			return arrheaders;
		}

		private static List<string[]> ExtractRows(ListView lsv) {
			var items = lsv.SelectedItems.Count > 0 ? (System.Collections.IEnumerable) lsv.SelectedItems : lsv.Items;

			List<string[]> listrows = new List<string[]>();
			foreach (ListViewItem lvitem in items) {
				listrows.Add(ExtractRow(lvitem));
			}
			return listrows;
		}

		private static string[] ExtractRow(ListViewItem lvitem) {
			string[] row = new string[lvitem.SubItems.Count];
			for (int ncol = 0; ncol < lvitem.SubItems.Count; ncol++) {
				row[ncol] = lvitem.SubItems[ncol].Text;
			}
			return row;
		}
	}
}
