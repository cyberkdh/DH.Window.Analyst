//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.UI.Utils {
	// shared default-filename builder for every SaveFileDialog opened by an export action, so repeated exports
	// of the same list never silently overwrite an earlier one
	public static class ExportFileNameHelper {
		public static string BuildTimestampedFileName(string strbasename, string strext) {
			return $"{strbasename}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{strext}";
		}
	}
}
