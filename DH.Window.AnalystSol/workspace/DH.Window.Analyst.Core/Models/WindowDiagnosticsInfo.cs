//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Drawing;

namespace DH.Window.Analyst.Models {
	// all fields are queryable without DLL injection
	public class WindowDiagnosticsInfo {
		public bool IsHung { get; }

		public bool IsCloaked { get; }

		public bool HasExtendedFrameBounds { get; }
		public Rectangle ExtendedFrameBounds { get; }

		public bool IsLayered { get; }
		public byte? LayeredAlpha { get; }
		public int? LayeredColorKey { get; }

		public string DpiAwareness { get; }

		public WindowDiagnosticsInfo(bool bishung, bool biscloaked, bool bhasextendedframebounds, Rectangle extendedframebounds,
				bool bislayered, byte? nlayeredalpha, int? nlayeredcolorkey, string strdpiawareness) {
			IsHung = bishung;
			IsCloaked = biscloaked;
			HasExtendedFrameBounds = bhasextendedframebounds;
			ExtendedFrameBounds = extendedframebounds;
			IsLayered = bislayered;
			LayeredAlpha = nlayeredalpha;
			LayeredColorKey = nlayeredcolorkey;
			DpiAwareness = strdpiawareness;
		}
	}
}
