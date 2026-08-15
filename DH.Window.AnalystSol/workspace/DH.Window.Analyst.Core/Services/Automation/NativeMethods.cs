//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DH.Window.Analyst.Services.Automation {
	internal static class NativeMethods {
		public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lparam);

		[DllImport("user32.dll")]
		public static extern bool EnumWindows(EnumWindowsProc lpenumfunc, IntPtr lparam);

		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern bool IsWindow(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern int GetWindowTextLength(IntPtr hwnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpstring, int nmaxcount);

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindow(IntPtr hwnd, uint ucmd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwprocessid);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetClassName(IntPtr hwnd, StringBuilder lpclassname, int nmaxcount);

		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hwnd, out RECT lprect);

		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hwnd, out RECT lprect);

		[DllImport("user32.dll")]
		public static extern bool GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll")]
		public static extern IntPtr GetMenu(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern IntPtr WindowFromPoint(POINT point);

		[DllImport("user32.dll")]
		public static extern IntPtr GetAncestor(IntPtr hwnd, uint gaflags);

		public const uint GA_ROOT = 2;
		public const uint GA_ROOTOWNER = 3;

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hwnd, int ncmdshow);

		[DllImport("user32.dll")]
		public static extern bool IsIconic(IntPtr hwnd);

		public const int SW_RESTORE = 9;
		public const int SW_SHOWNORMAL = 1;
		public const int SW_SHOWMINIMIZED = 2;
		public const int SW_SHOWMAXIMIZED = 3;

		[DllImport("user32.dll")]
		public static extern IntPtr GetParent(IntPtr hwnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetClassInfoEx(IntPtr hinstance, string lpszclass, ref WNDCLASSEX lpwcx);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hwnd, int nindex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
		private static extern int GetWindowLong32(IntPtr hwnd, int nindex);

		// GetWindowLongPtr is not exported on 32-bit Windows (it is a header macro there), so branch on pointer size
		public static IntPtr GetWindowLongPtr(IntPtr hwnd, int nindex) {
			return IntPtr.Size == 8 ? GetWindowLongPtr64(hwnd, nindex) : new IntPtr(GetWindowLong32(hwnd, nindex));
		}

		[DllImport("user32.dll", EntryPoint = "GetClassLongPtr", CharSet = CharSet.Auto)]
		private static extern IntPtr GetClassLongPtr64(IntPtr hwnd, int nindex);

		[DllImport("user32.dll", EntryPoint = "GetClassLong", CharSet = CharSet.Auto)]
		private static extern uint GetClassLong32(IntPtr hwnd, int nindex);

		public static IntPtr GetClassLongPtr(IntPtr hwnd, int nindex) {
			return IntPtr.Size == 8 ? GetClassLongPtr64(hwnd, nindex) : new IntPtr(GetClassLong32(hwnd, nindex));
		}

		public const uint GW_HWNDFIRST = 0;
		public const uint GW_HWNDLAST = 1;
		public const uint GW_HWNDNEXT = 2;
		public const uint GW_HWNDPREV = 3;
		public const uint GW_OWNER = 4;
		public const uint GW_CHILD = 5;

		public const int GWL_WNDPROC = -4;
		public const int GWL_STYLE = -16;
		public const int GWL_EXSTYLE = -20;
		public const int GWLP_HINSTANCE = -6;
		public const int GWLP_USERDATA = -21;
		public const int GCL_CBWNDEXTRA = -18;
		public const int GWL_ID = -12;

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT {
			public int X;
			public int Y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WINDOWPLACEMENT {
			public int Length;
			public int Flags;
			public int ShowCmd;
			public POINT MinPosition;
			public POINT MaxPosition;
			public RECT NormalPosition;
		}

		// lpszMenuName/lpszClassName are IntPtr, not marshaled strings: they point at the OS's own class-table memory, and a [MarshalAs(LPTStr)] field would make the marshaler free that pointer as if it owned it, corrupting the heap
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct WNDCLASSEX {
			public uint cbSize;
			public uint style;
			public IntPtr lpfnWndProc;
			public int cbClsExtra;
			public int cbWndExtra;
			public IntPtr hInstance;
			public IntPtr hIcon;
			public IntPtr hCursor;
			public IntPtr hbrBackground;
			public IntPtr lpszMenuName;
			public IntPtr lpszClassName;
			public IntPtr hIconSm;
		}
	}
}
