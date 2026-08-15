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
	// kept separate from NativeMethods.cs/MessageHookNativeMethods.cs since IGlobalPickerHookService is the sole consumer and this is a distinct feature area (system-wide WH_MOUSE_LL/WH_KEYBOARD_LL, not the WH_CALLWNDPROC/WH_GETMESSAGE native-DLL injection used for Message Logging)
	internal static class GlobalHookNativeMethods {
		public const int WH_MOUSE_LL = 14;
		public const int WH_KEYBOARD_LL = 13;

		public const int WM_LBUTTONDOWN = 0x0201;
		public const int WM_LBUTTONUP = 0x0202;
		public const int WM_KEYDOWN = 0x0100;

		public const int VK_ESCAPE = 0x1B;

		[StructLayout(LayoutKind.Sequential)]
		public struct MSLLHOOKSTRUCT {
			public NativeMethods.POINT pt;
			public uint mouseData;
			public uint flags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct KBDLLHOOKSTRUCT {
			public uint vkCode;
			public uint scanCode;
			public uint flags;
			public uint time;
			public IntPtr dwExtraInfo;
		}

		public delegate IntPtr LowLevelMouseProc(int ncode, IntPtr wparam, IntPtr lparam);

		public delegate IntPtr LowLevelKeyboardProc(int ncode, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int nidhook, LowLevelMouseProc lpfn, IntPtr hmod, uint dwthreadid);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int nidhook, LowLevelKeyboardProc lpfn, IntPtr hmod, uint dwthreadid);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll")]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int ncode, IntPtr wparam, IntPtr lparam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpmodulename);
	}
}
