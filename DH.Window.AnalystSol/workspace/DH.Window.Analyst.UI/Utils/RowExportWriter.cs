//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;

namespace DH.Window.Analyst.UI.Utils {
	// row-data-in, text-out formatter shared by every exporter (ListView-backed logs, the TreeView-backed window list) so
	// CSV/JSON formatting isn't duplicated per control; no NuGet JSON library is referenced project-wide, so this writes
	// the minimal escaping (\, ", \r, \n) by hand rather than pulling in Newtonsoft.Json for flat key/value rows
	public static class RowExportWriter {
		public static string BuildDelimitedText(string[] arrheaders, IEnumerable<string[]> rows, string strdelimiter, bool bquotefields) {
			var sb = new StringBuilder();

			for (int ncol = 0; ncol < arrheaders.Length; ncol++) {
				if (ncol > 0) {
					sb.Append(strdelimiter);
				}
				sb.Append(FormatDelimitedField(arrheaders[ncol], bquotefields));
			}
			sb.AppendLine();

			foreach (string[] row in rows) {
				for (int ncol = 0; ncol < row.Length; ncol++) {
					if (ncol > 0) {
						sb.Append(strdelimiter);
					}
					sb.Append(FormatDelimitedField(row[ncol], bquotefields));
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		public static string BuildJsonFlat(string[] arrheaders, IEnumerable<string[]> rows) {
			var sb = new StringBuilder();
			sb.Append("[");

			bool bfirstrow = true;
			foreach (string[] row in rows) {
				if (bfirstrow == false) {
					sb.Append(",");
				}
				bfirstrow = false;

				sb.Append("\n  ");
				AppendJsonObjectFields(sb, arrheaders, row);
			}

			sb.Append(bfirstrow == false ? "\n]" : "]");
			return sb.ToString();
		}

		// nodes may carry children (e.g. a top-level window's child windows) — each gets a "Children" array only when non-empty, so a flat source (no children anywhere) renders identically to BuildJsonFlat
		public static string BuildJsonTree(string[] arrheaders, IEnumerable<ExportTreeNode> roots) {
			var sb = new StringBuilder();
			sb.Append("[");

			bool bfirstroot = true;
			foreach (ExportTreeNode noderoot in roots) {
				if (bfirstroot == false) {
					sb.Append(",");
				}
				bfirstroot = false;

				sb.Append("\n  ");
				AppendJsonNode(sb, arrheaders, noderoot, 1);
			}

			sb.Append(bfirstroot == false ? "\n]" : "]");
			return sb.ToString();
		}

		private static void AppendJsonNode(StringBuilder sb, string[] arrheaders, ExportTreeNode node, int nindent) {
			sb.Append("{ ");
			for (int ncol = 0; ncol < arrheaders.Length; ncol++) {
				if (ncol > 0) {
					sb.Append(", ");
				}
				string strvalue = ncol < node.Values.Length ? node.Values[ncol] : "";
				sb.Append("\"").Append(EscapeJson(arrheaders[ncol])).Append("\": \"").Append(EscapeJson(strvalue)).Append("\"");
			}

			if (node.Children.Count > 0) {
				sb.Append(", \"Children\": [");
				string strindent = new string(' ', (nindent + 1) * 2);
				for (int nchild = 0; nchild < node.Children.Count; nchild++) {
					if (nchild > 0) {
						sb.Append(",");
					}
					sb.Append("\n").Append(strindent);
					AppendJsonNode(sb, arrheaders, node.Children[nchild], nindent + 1);
				}
				sb.Append("\n").Append(new string(' ', nindent * 2)).Append("]");
			}

			sb.Append(" }");
		}

		private static void AppendJsonObjectFields(StringBuilder sb, string[] arrheaders, string[] row) {
			sb.Append("{ ");
			for (int ncol = 0; ncol < arrheaders.Length; ncol++) {
				if (ncol > 0) {
					sb.Append(", ");
				}
				string strvalue = ncol < row.Length ? row[ncol] : "";
				sb.Append("\"").Append(EscapeJson(arrheaders[ncol])).Append("\": \"").Append(EscapeJson(strvalue)).Append("\"");
			}
			sb.Append(" }");
		}

		private static string FormatDelimitedField(string strvalue, bool bquotefields) {
			if (bquotefields == false) {
				return strvalue;
			}

			return "\"" + strvalue.Replace("\"", "\"\"") + "\"";
		}

		private static string EscapeJson(string strvalue) {
			if (string.IsNullOrEmpty(strvalue) == true) {
				return "";
			}

			return strvalue.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
		}
	}
}
