//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.Logging {
	// one in-memory log line, raised via AppLog.EntryLogged for live viewers (e.g. AppLogViewerControl); independent of the file-writer path
	public struct LogEntry {
		public DateTime Timestamp;
		public LogLevel Level;
		public string Message;

		public LogEntry(DateTime timestamp, LogLevel level, string strmessage) {
			Timestamp = timestamp;
			Level = level;
			Message = strmessage;
		}
	}
}
