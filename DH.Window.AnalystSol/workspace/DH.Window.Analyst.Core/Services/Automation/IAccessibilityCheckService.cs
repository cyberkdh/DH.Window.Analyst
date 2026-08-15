//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Windows.Automation;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public interface IAccessibilityCheckService {
		// walks the full subtree rooted at root and returns every rule violation found
		List<AccessibilityIssueItem> CheckSubtree(AutomationElement root);
	}
}
