//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;

namespace DH.Window.Analyst.Models {
	// IsTarget marks the window the stack was requested for, so the UI can highlight it
	public class ZOrderEntryItem {
		public IntPtr Handle { get; }
		public string Caption { get; }
		public string ClassName { get; }
		public string ProcessName { get; }
		public Rectangle Bounds { get; }
		public bool IsVisible { get; }
		public bool IsTarget { get; }

		public ZOrderEntryItem(IntPtr handle, string strcaption, string strclassname, string strprocessname, Rectangle bounds, bool bisvisible, bool bistarget) {
			Handle = handle;
			Caption = strcaption;
			ClassName = strclassname;
			ProcessName = strprocessname;
			Bounds = bounds;
			IsVisible = bisvisible;
			IsTarget = bistarget;
		}
	}
}
