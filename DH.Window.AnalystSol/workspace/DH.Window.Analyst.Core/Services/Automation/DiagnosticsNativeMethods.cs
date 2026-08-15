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
	// kept apart from NativeMethods.cs since diagnostics is a distinct feature area
	internal static class DiagnosticsNativeMethods {
		[DllImport("user32.dll")]
		public static extern bool IsHungAppWindow(IntPtr hwnd);

		// DWMWA_EXTENDED_FRAME_BOUNDS (RECT) — actual visual bounds excluding the Aero drop shadow
		[DllImport("dwmapi.dll")]
		public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwattribute, out NativeMethods.RECT pvattribute, int cbattribute);

		// DWMWA_CLOAKED (DWORD) — nonzero if the window is cloaked (e.g. on another virtual desktop)
		[DllImport("dwmapi.dll")]
		public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwattribute, out int pvattribute, int cbattribute);

		public const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
		public const int DWMWA_CLOAKED = 14;

		[DllImport("user32.dll")]
		public static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out int crkey, out byte balpha, out uint dwflags);

		public const uint LWA_COLORKEY = 0x1;
		public const uint LWA_ALPHA = 0x2;

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDpiAwarenessContext(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern bool AreDpiAwarenessContextsEqual(IntPtr dpicontexta, IntPtr dpicontextb);

		public static readonly IntPtr DPI_AWARENESS_CONTEXT_UNAWARE = new IntPtr(-1);
		public static readonly IntPtr DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = new IntPtr(-2);
		public static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = new IntPtr(-3);
		public static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = new IntPtr(-4);
		public static readonly IntPtr DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED = new IntPtr(-5);

		[DllImport("user32.dll")]
		public static extern uint GetGuiResources(IntPtr hprocess, uint uiflags);

		public const uint GR_GDIOBJECTS = 0;
		public const uint GR_USEROBJECTS = 1;

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool OpenProcessToken(IntPtr hprocess, uint dwdesiredaccess, out IntPtr htokenhandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool GetTokenInformation(IntPtr htokenhandle, int tokeninformationclass, IntPtr ptokeninformation, int ntokeninformationlength, out int nreturnlength);

		[DllImport("kernel32.dll")]
		public static extern bool CloseHandle(IntPtr hobject);

		// unlike .NET's Process.Handle (which requests a broad access mask), PROCESS_QUERY_LIMITED_INFORMATION is grantable by MIC even from a non-elevated caller against an elevated target, and is sufficient for OpenProcessToken
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(uint dwdesiredaccess, bool binherithandle, int nprocessid);

		public const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

		public const uint TOKEN_QUERY = 0x0008;
		public const int TokenElevation = 20;
	}
}
