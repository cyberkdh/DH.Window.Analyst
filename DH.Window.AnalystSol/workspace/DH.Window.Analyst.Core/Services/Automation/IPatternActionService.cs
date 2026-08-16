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

		bool TryGetWindowVisualState(AutomationElement element, out string strstate);
		bool TryClose(AutomationElement element);
		bool TryMinimize(AutomationElement element);
		bool TryMaximize(AutomationElement element);
		bool TryRestore(AutomationElement element);

		bool TryGetTransformCapabilities(AutomationElement element, out bool bcanmove, out bool bcanresize, out bool bcanrotate);
		bool TryMove(AutomationElement element, double x, double y);
		bool TryResize(AutomationElement element, double width, double height);
		bool TryRotate(AutomationElement element, double degrees);
	}
}
