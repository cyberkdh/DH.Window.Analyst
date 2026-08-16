//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.Models {
	public class MessageLogItem {
		public DateTime Timestamp { get; }

		public string MessageName { get; }

		public IntPtr Handle { get; }

		public IntPtr WParam { get; }

		public IntPtr LParam { get; }

		// FALSE = WH_CALLWNDPROC (sent, e.g. SendMessage), TRUE = WH_GETMESSAGE (posted/queued)
		public bool IsPosted { get; }

		// human-readable Rect/DPI decode for layout-related messages (WM_SIZE/WM_MOVE/WM_WINDOWPOSCHANGED/WM_DPICHANGED); null for everything else
		public string DecodedInfo { get; }

		public MessageLogItem(DateTime timestamp, string strmessagename, IntPtr handle, IntPtr wparam, IntPtr lparam, bool bisposted, string strdecodedinfo) {
			Timestamp = timestamp;
			MessageName = strmessagename;
			Handle = handle;
			WParam = wparam;
			LParam = lparam;
			IsPosted = bisposted;
			DecodedInfo = strdecodedinfo;
		}
	}
}
