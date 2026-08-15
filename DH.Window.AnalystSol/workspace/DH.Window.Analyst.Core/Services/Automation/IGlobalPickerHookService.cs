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
	// system-wide WH_MOUSE_LL/WH_KEYBOARD_LL hook, used only for "Show Info on Mouse" click/Esc detection — hover tracking itself is Timer-polled, not hooked
	public interface IGlobalPickerHookService : IDisposable {
		bool IsActive { get; }

		// set when Start returns false (SetWindowsHookEx failure); null/empty otherwise
		string LastError { get; }

		// fires on the UI thread with the screen point of a swallowed left-button-down, while active
		event EventHandler<Point> Picked;

		// fires on the UI thread when Esc is pressed while active
		event EventHandler Cancelled;

		bool Start();

		void Stop();
	}
}
