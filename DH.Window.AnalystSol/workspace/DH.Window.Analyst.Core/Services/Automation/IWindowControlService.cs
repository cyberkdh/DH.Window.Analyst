//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;

namespace DH.Window.Analyst.Services.Automation {
	// direct Win32 state mutation on a target top-level window (Show/Hide/Enable/Disable/Topmost/Move/Resize) —
	// unlike IPatternActionService (UIA), this never crosses into another process's UIA server, but SetWindowPos/EnableWindow
	// can still fail silently against elevated targets (UIPI), so every action is a Try* returning bool
	public interface IWindowControlService {
		bool TryShow(IntPtr handle);
		bool TryHide(IntPtr handle);

		bool TryEnable(IntPtr handle);
		bool TryDisable(IntPtr handle);
		bool IsEnabled(IntPtr handle);

		bool IsTopmost(IntPtr handle);
		bool TrySetTopmost(IntPtr handle, bool btopmost);

		bool TryGetBounds(IntPtr handle, out Rectangle rect);
		bool TryMoveResize(IntPtr handle, int x, int y, int width, int height);

		// true if handle belongs to this application's own process (used to guard against self Hide/Disable lockout)
		bool IsOwnProcessWindow(IntPtr handle);
	}
}
