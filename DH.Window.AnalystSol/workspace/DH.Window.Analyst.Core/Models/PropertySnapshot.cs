//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace DH.Window.Analyst.Models {
	// point-in-time capture of a window's Basic + Native Details property rows (Property Compare feature); session-only,
	// never persisted — held in a plain in-memory list by whoever captures it (MainForm), same lifetime as Event/Message logs
	public class PropertySnapshot {
		public IntPtr Handle { get; }

		public string WindowTitle { get; }

		public string ClassName { get; }

		public string ProcessName { get; }

		public DateTime CapturedAt { get; }

		public IReadOnlyList<PropertyItem> Items { get; }

		public string DisplayLabel { get { return $"{CapturedAt:HH:mm:ss}  {ProcessName} \"{WindowTitle}\" (0x{Handle.ToInt64():X})"; } }

		public PropertySnapshot(IntPtr handle, string strtitle, string strclassname, string strprocessname, DateTime dtcapturedat, IReadOnlyList<PropertyItem> listitems) {
			Handle = handle;
			WindowTitle = strtitle;
			ClassName = strclassname;
			ProcessName = strprocessname;
			CapturedAt = dtcapturedat;
			Items = listitems;
		}
	}
}
