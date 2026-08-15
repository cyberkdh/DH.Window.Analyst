//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Settings;

namespace DH.Window.Analyst {
	internal static class Program {
		[STAThread]
		private static void Main() {
			InitializeLogging();

			Application.ThreadException += (sender, e) => AppLog.e("Unhandled UI thread exception", e.Exception);
			AppDomain.CurrentDomain.UnhandledException += (sender, e) => AppLog.e("Unhandled AppDomain exception", e.ExceptionObject as Exception);

			AppLog.i("Application starting");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());

			AppLog.i("Application exiting");
		}

		private static void InitializeLogging() {
			string strlogdirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DH.Window.Analyst", "logs");
			strlogdirectory = Path.Combine(@"c:\log", "DH.Window.Analyst");

			AppLog.SetLogging(LogTarget.All, "DH.Window.Analyst", strlogdirectory, AppSettings.Get().LogLevel, AppSettings.Get().LogRetentionDays);
		}
	}
}
