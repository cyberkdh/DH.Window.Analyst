//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Drawing;

namespace DH.Window.Analyst.Models {
	// cheap (GetWindowRect/GetWindowLongPtr/GetWindowThreadProcessId only, no file/UIA access) per-window snapshot, safe to call every hover-poll tick
	public class WindowQuickInfo {
		public Rectangle ScreenRect { get; }

		public uint Style { get; }

		public uint ExStyle { get; }

		public int ThreadId { get; }

		public WindowQuickInfo(Rectangle rectscreen, uint nstyle, uint nexstyle, int nthreadid) {
			ScreenRect = rectscreen;
			Style = nstyle;
			ExStyle = nexstyle;
			ThreadId = nthreadid;
		}
	}
}
