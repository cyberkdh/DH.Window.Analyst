//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Models {
	public class BreadcrumbSegment {
		public string Text { get; }
		public object Tag { get; }

		public BreadcrumbSegment(string strtext, object tag) {
			Text = strtext;
			Tag = tag;
		}
	}
}
