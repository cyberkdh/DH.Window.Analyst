//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public interface IProcessDiagnosticsService {
		// snapshot of file version / resource usage / elevation state for the process owning a window
		ProcessDiagnosticsInfo GetProcessDiagnostics(int nprocessid);

		// checked via WindowsPrincipal, not OpenProcessToken, since it is always reliable for our own process
		bool IsCurrentProcessElevated();

		// lightweight standalone elevation check (OpenProcess+OpenProcessToken only) — unlike GetProcessDiagnostics, does not touch the file
		// system (FileVersionInfo) or GDI/USER resource counters, so it is cheap enough to call on every hover-poll tick
		bool? IsProcessElevated(int nprocessid);
	}
}
