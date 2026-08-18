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

		// true when connected via GWL_HWNDPARENT owner (e.g. modal dialog/tooltip/IME) rather than true WS_CHILD; tree still nests it under its owner
		public bool IsOwnedWindow { get; }

		public string HandleText { get { return "0x" + Handle.ToString("X"); } }

		public TopLevelWindowItem(IntPtr handle, string strtitle, string strclassname, string strprocessname, int nprocessid, ImageSource icon, bool bisownedwindow = false) {
			Handle = handle;
			Title = strtitle;
			ClassName = strclassname;
			ProcessName = strprocessname;
			ProcessId = nprocessid;
			Icon = icon;
			IsOwnedWindow = bisownedwindow;
		}
	}
}
