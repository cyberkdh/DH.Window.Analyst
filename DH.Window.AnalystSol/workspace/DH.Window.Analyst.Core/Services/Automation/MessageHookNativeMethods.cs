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
	// dynamic LoadLibrary since the DLL path (_x86 vs _x64) is chosen at runtime by target bitness, not known at compile time
	internal static class MessageHookNativeMethods {
		public const int WH_CALLWNDPROC = 4;
		public const int WH_GETMESSAGE = 3;

		public const uint WM_COPYDATA = 0x004A;

		// tag stamped by the native DLL so the receiver can confirm a WM_COPYDATA arrival is actually a HookNativeMessageEntry batch
		public const uint HOOKNATIVE_DATA_ID = 0x44485741; // "DHWA"

		[StructLayout(LayoutKind.Sequential)]
		public struct COPYDATASTRUCT {
			public IntPtr dwData;
			public int cbData;
			public IntPtr lpData;
		}

		// mirrors HookNativeMessageEntry in MessageQueue.h — field order/types must stay in sync
		[StructLayout(LayoutKind.Sequential)]
		public struct HookNativeMessageEntry {
			public IntPtr hwnd;
			public uint nmessage;
			public IntPtr wparam;
			public IntPtr lparam;
			public uint ndwtimestamp;
			public uint ndwthreadid;
			[MarshalAs(UnmanagedType.Bool)]
			public bool bposted;
			[MarshalAs(UnmanagedType.Bool)]
			public bool bhasdecodedrect;
			public int nrectleft;
			public int nrecttop;
			public int nrectright;
			public int nrectbottom;
			public uint ndpi;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string strlibpath);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hmodule);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hmodule, string strprocname);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process(IntPtr hprocess, [MarshalAs(UnmanagedType.Bool)] out bool bwow64process);

		// lpfn is the raw exported function pointer, not a managed delegate — the hook procedure must physically reside in a DLL mapped into the target process, which a C# delegate cannot satisfy
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int nidhook, IntPtr lpfn, IntPtr hmod, uint ndwthreadid);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll")]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int ncode, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint ndwprocessid);
	}
}
