//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Utils {
	public static class ElevationRestartHelper {
		// asks the user before elevating, since UAC itself also prompts; a silent auto-relaunch would surprise the user with a second, unexplained prompt
		public static bool ConfirmAndRestartElevated(IWin32Window owner, string strtargetdescription) {
			string strmessage = $"{strtargetdescription} is running elevated (as Administrator).\n\nMessage hooking requires this application to run elevated too.\n\nRestart as Administrator now?";
			DialogResult result = MessageBoxEx.Show(owner, strmessage, "Elevation Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (result != DialogResult.Yes) {
				return false;
			}

			var startinfo = new ProcessStartInfo {
				FileName = Application.ExecutablePath,
				UseShellExecute = true,
				Verb = "runas",
			};

			try {
				Process.Start(startinfo);
			}
			catch (Win32Exception) {
				// ERROR_CANCELLED: user declined the UAC prompt itself — stay running unelevated rather than exiting
				return false;
			}

			Application.Exit();
			return true;
		}
	}
}
