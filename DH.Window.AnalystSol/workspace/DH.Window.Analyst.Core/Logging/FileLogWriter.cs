//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace DH.Window.Analyst.Logging {
	// cleanup runs at startup and again whenever the calendar date rolls over during a long-running session
	public class FileLogWriter {
		private const int DEFAULT_RETENTION_DAYS = 7;

		private readonly LogTarget m_target;
		private readonly string m_strAppName;
		private readonly string m_strLogDirectory;
		private readonly int m_nRetentionDays;
		private readonly object m_objSync = new object();
		private string m_strLastCleanupDate = string.Empty;

		public FileLogWriter(LogTarget target, string strappname, string strlogdirectory, int nretentiondays = DEFAULT_RETENTION_DAYS) {
			m_strAppName = strappname;
			m_strLogDirectory = strlogdirectory;
			m_nRetentionDays = nretentiondays > 0 ? nretentiondays : DEFAULT_RETENTION_DAYS;

			if ((target & LogTarget.File) == LogTarget.File && string.IsNullOrEmpty(strlogdirectory) == false) {
				target = EnsureLogDirectory(strlogdirectory) == true ? target : target & ~LogTarget.File;
			}

			m_target = target;

			if ((m_target & LogTarget.File) == LogTarget.File) {
				CleanupOldLogFiles();
			}
		}

		public void WriteLine(string strline) {
			string strformatted = $"{DateTime.Now:yyyyMMdd HHmmss.fff} [{m_strAppName}] {strline}";

			lock (m_objSync) {
				if ((m_target & LogTarget.DebugView) == LogTarget.DebugView) {
					Trace.WriteLine(strformatted);
				}

				if ((m_target & LogTarget.File) == LogTarget.File) {
					WriteToFile(strformatted);
				}
			}
		}

		private static bool EnsureLogDirectory(string strlogdirectory) {
			try {
				Directory.CreateDirectory(strlogdirectory);
				return true;
			}
			catch (Exception ex) {
				Trace.WriteLine($"[FileLogWriter] failed to prepare log directory: {ex.Message}");
				return false;
			}
		}

		private void WriteToFile(string strformatted) {
			string strtoday = DateTime.Now.ToString("yyyyMMdd");

			try {
				string strpath = Path.Combine(m_strLogDirectory, $"{m_strAppName}_{strtoday}.log");
				File.AppendAllText(strpath, strformatted + Environment.NewLine);
			}
			catch (Exception ex) {
				Trace.WriteLine($"[FileLogWriter] write failed: {ex.Message}");
			}

			if (strtoday != m_strLastCleanupDate) {
				CleanupOldLogFiles();
			}
		}

		private void CleanupOldLogFiles() {
			m_strLastCleanupDate = DateTime.Now.ToString("yyyyMMdd");

			try {
				DateTime dtcutoff = DateTime.Now.Date.AddDays(-m_nRetentionDays);

				foreach (string strfile in Directory.GetFiles(m_strLogDirectory, $"{m_strAppName}_*.log")) {
					string strdatepart = Path.GetFileNameWithoutExtension(strfile).Substring(m_strAppName.Length + 1);
					if (DateTime.TryParseExact(strdatepart, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtfile) == true && dtfile < dtcutoff) {
						File.Delete(strfile);
					}
				}
			}
			catch (Exception ex) {
				Trace.WriteLine($"[FileLogWriter] cleanup failed: {ex.Message}");
			}
		}
	}
}
