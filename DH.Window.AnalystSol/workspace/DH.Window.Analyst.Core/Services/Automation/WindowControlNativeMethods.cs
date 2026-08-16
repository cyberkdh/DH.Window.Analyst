//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;

namespace DH.Window.Analyst.Services.Automation {
	// kept apart from NativeMethods.cs since window-state mutation (Show/Hide/Enable/Move/Topmost) is a distinct feature area from the read-only lookups there
	internal static class WindowControlNativeMethods {
		[DllImport("user32.dll")]
		public static extern bool EnableWindow(IntPtr hwnd, bool benable);

		[DllImport("user32.dll")]
		public static extern bool IsWindowEnabled(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndinsertafter, int x, int y, int cx, int cy, uint uflags);

		public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
		public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

		public const uint SWP_NOSIZE = 0x0001;
		public const uint SWP_NOMOVE = 0x0002;
		public const uint SWP_NOZORDER = 0x0004;
		public const uint SWP_NOACTIVATE = 0x0010;

		public const int SW_HIDE = 0;
		public const int SW_SHOW = 5;

		public const int WS_EX_TOPMOST = 0x00000008;
	}
}
