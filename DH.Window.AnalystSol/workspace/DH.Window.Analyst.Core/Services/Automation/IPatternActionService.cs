//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Windows.Automation;

namespace DH.Window.Analyst.Services.Automation {
	public interface IPatternActionService {
		bool TryInvoke(AutomationElement element);

		bool TryGetToggleState(AutomationElement element, out string strstate);
		bool TryToggle(AutomationElement element);

		bool TryGetExpandCollapseState(AutomationElement element, out string strstate);
		bool TryExpand(AutomationElement element);
		bool TryCollapse(AutomationElement element);

		bool TrySelect(AutomationElement element);

		bool TryGetValue(AutomationElement element, out string strvalue, out bool breadonly);
		bool TrySetValue(AutomationElement element, string strvalue);
	}
}
