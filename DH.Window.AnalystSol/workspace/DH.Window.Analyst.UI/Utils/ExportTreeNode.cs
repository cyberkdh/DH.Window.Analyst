//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace DH.Window.Analyst.UI.Utils {
	// one row of export data plus its child rows; shared by any exporter (window list, future tree-shaped sources) that needs both a flat CSV/JSON view and a nested JSON view of the same data
	public class ExportTreeNode {
		public string[] Values { get; set; }
		public List<ExportTreeNode> Children { get; } = new List<ExportTreeNode>();
	}
}
