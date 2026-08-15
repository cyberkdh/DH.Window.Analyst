//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public interface IWinEventMonitorService : IDisposable {
		bool IsMonitoring { get; }

		// must be called on a thread that pumps Win32 messages (the WinForms UI thread), since callbacks arrive as queued messages there
		void Start(int nprocessid, Action<WinEventItem> callback);

		void Stop();
	}
}
