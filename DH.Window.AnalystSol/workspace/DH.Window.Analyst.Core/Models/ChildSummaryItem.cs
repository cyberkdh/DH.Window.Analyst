//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.Models {
	public class ChildSummaryItem {
		public string ControlTypeName { get; }
		public int Count { get; }

		public ChildSummaryItem(string strcontroltypename, int ncount) {
			ControlTypeName = strcontroltypename;
			Count = ncount;
		}
	}
}
