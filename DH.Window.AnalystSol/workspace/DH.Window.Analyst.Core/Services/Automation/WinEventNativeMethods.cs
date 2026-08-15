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
	// kept separate from NativeMethods.cs since WinEventMonitorService is the sole consumer and the two are unrelated feature areas
	internal static class WinEventNativeMethods {
		public delegate void WinEventProc(IntPtr hwineventhook, uint neventtype, IntPtr hwnd, int nidobject, int nidchild, uint neventthread, uint ndwmseventtime);

		[DllImport("user32.dll")]
		public static extern IntPtr SetWinEventHook(uint neventmin, uint neventmax, IntPtr hmodwineventproc, WinEventProc lpfnwineventproc, uint nidprocess, uint nidthread, uint ndwflags);

		[DllImport("user32.dll")]
		public static extern bool UnhookWinEvent(IntPtr hwineventhook);

		public const uint EVENT_MIN = 0x00000001;
		public const uint EVENT_MAX = 0x7FFFFFFF;

		// OUTOFCONTEXT delivers callbacks async via the hooking thread's message queue, which must pump messages (the WinForms UI thread does)
		public const uint WINEVENT_OUTOFCONTEXT = 0x0000;
		public const uint WINEVENT_SKIPOWNPROCESS = 0x0002;

		public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
		public const uint EVENT_OBJECT_CREATE = 0x8000;
		public const uint EVENT_OBJECT_DESTROY = 0x8001;
		public const uint EVENT_OBJECT_SHOW = 0x8002;
		public const uint EVENT_OBJECT_HIDE = 0x8003;
		public const uint EVENT_OBJECT_FOCUS = 0x8005;
		public const uint EVENT_OBJECT_SELECTION = 0x8006;
		public const uint EVENT_OBJECT_STATECHANGE = 0x800A;
		public const uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
		public const uint EVENT_OBJECT_NAMECHANGE = 0x800C;
		public const uint EVENT_OBJECT_VALUECHANGE = 0x800E;
	}
}
