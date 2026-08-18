//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.Logging {
	// SetLogging() must run once at app startup (Program.cs); before that, every call below is a silent no-op rather than throwing
	public static class AppLog {
		private static FileLogWriter s_writer;
		private static LogLevel s_level = LogLevel.All;

		// raised for every level regardless of s_level/s_writer, so a viewer (AppLogViewerControl) can subscribe before SetLogging() and apply its own filter
		public static event Action<LogEntry> EntryLogged;

		// strlogdirectory is a folder, not a file path — FileLogWriter writes one dated file per day inside it
		public static void SetLogging(LogTarget target, string strappname, string strlogdirectory, LogLevel level = LogLevel.All, int nretentiondays = 7) {
			s_writer = new FileLogWriter(target, strappname, strlogdirectory, nretentiondays);
			s_level = level;
		}

		public static void d(string strlog) {
			Write(LogLevel.Debug, "D", strlog);
		}

		public static void i(string strlog) {
			Write(LogLevel.Info, "I", strlog);
		}

		public static void w(string strlog) {
			Write(LogLevel.Warn, "W", strlog);
		}

		public static void e(string strlog) {
			Write(LogLevel.Error, "E", strlog);
		}

		public static void e(string strlog, Exception ex) {
			string strdetail = ex != null ? $"{strlog} — {ex.GetType().Name}: {ex.Message}" : strlog;
			Write(LogLevel.Error, "E", strdetail);
		}

		private static void Write(LogLevel level, string strtag, string strlog) {
			EntryLogged?.Invoke(new LogEntry(DateTime.Now, level, strlog));

			if (s_writer == null || (s_level & level) != level) {
				return;
			}

			s_writer.WriteLine($"[{strtag}] {strlog}");
		}
	}
}
