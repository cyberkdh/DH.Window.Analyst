//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Media;

namespace DH.Window.Analyst.Models {
	// obtained via Win32 EnumWindows, not UIA, so enumeration completes immediately
	public class TopLevelWindowItem {
		public IntPtr Handle { get; }

		public string Title { get; }

		public string ClassName { get; }

		public string ProcessName { get; }

		public int ProcessId { get; }

		public ImageSource Icon { get; }

		public string HandleText { get { return "0x" + Handle.ToString("X"); } }

		public TopLevelWindowItem(IntPtr handle, string strtitle, string strclassname, string strprocessname, int nprocessid, ImageSource icon) {
			Handle = handle;
			Title = strtitle;
			ClassName = strclassname;
			ProcessName = strprocessname;
			ProcessId = nprocessid;
			Icon = icon;
		}
	}
}
