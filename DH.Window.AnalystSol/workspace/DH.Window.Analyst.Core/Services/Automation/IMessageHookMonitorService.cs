//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public interface IMessageHookMonitorService : IDisposable {
		bool IsMonitoring { get; }

		// set when Start returns false (SetWindowsHookEx failure, most commonly UIPI ERROR_ACCESS_DENIED against an elevated target); null/empty otherwise
		string LastError { get; }

		// takes a window handle (not a PID like IWinEventMonitorService) since WH_CALLWNDPROC/WH_GETMESSAGE are thread-scoped hooks; callback delivers a whole WM_COPYDATA batch per call, not per message, to avoid ListView flicker at WH_GETMESSAGE frequency
		bool Start(IntPtr htargetwindow, Action<IReadOnlyList<MessageLogItem>> callback);

		void Stop();
	}
}
