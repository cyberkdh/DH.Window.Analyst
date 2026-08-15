//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.Linq;

namespace DH.Window.Analyst.Services.Automation {
	// shared "0x..." hex / plain decimal window handle text parsing, used by both the sidebar filter box and the Find Window dialog
	public static class HandleTextParser {
		public static bool TryParse(string strtext, out IntPtr handle) {
			handle = IntPtr.Zero;

			if (string.IsNullOrWhiteSpace(strtext) == true) {
				return false;
			}

			string strtrimmed = strtext.Trim();

			if (strtrimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase) == true) {
				if (long.TryParse(strtrimmed.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long nhexvalue) == false) {
					return false;
				}
				handle = new IntPtr(nhexvalue);
				return true;
			}

			if (strtrimmed.All(char.IsDigit) == false) {
				return false;
			}

			if (long.TryParse(strtrimmed, out long ndecvalue) == false) {
				return false;
			}

			handle = new IntPtr(ndecvalue);
			return true;
		}
	}
}
