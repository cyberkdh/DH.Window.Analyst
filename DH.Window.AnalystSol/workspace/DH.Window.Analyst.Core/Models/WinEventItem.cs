//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.Models {
	public class WinEventItem {
		public DateTime Timestamp { get; }

		public string EventName { get; }

		public IntPtr Handle { get; }

		public int ObjectId { get; }

		public int ChildId { get; }

		public WinEventItem(DateTime timestamp, string streventname, IntPtr handle, int nobjectid, int nchildid) {
			Timestamp = timestamp;
			EventName = streventname;
			Handle = handle;
			ObjectId = nobjectid;
			ChildId = nchildid;
		}
	}
}
