//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.Models {
	public class WindowChildSummaryItem {
		public string ClassName { get; }
		public int Count { get; }

		public WindowChildSummaryItem(string strclassname, int ncount) {
			ClassName = strclassname;
			Count = ncount;
		}
	}
}
