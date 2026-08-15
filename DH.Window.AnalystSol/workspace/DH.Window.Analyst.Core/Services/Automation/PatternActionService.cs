//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Automation;

namespace DH.Window.Analyst.Services.Automation {
	// each call crosses into another process via UIA COM interop, so failures are expected at this boundary and reported back as a bool rather than thrown
	public class PatternActionService : IPatternActionService {
		public bool TryInvoke(AutomationElement element) {
			try {
				InvokePattern pattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
				pattern?.Invoke();
				return pattern != null;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TryGetToggleState(AutomationElement element, out string strstate) {
			strstate = null;
			try {
				TogglePattern pattern = element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
				if (pattern == null) {
					return false;
				}
				strstate = pattern.Current.ToggleState.ToString();
				return true;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TryToggle(AutomationElement element) {
			try {
				TogglePattern pattern = element.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
				pattern?.Toggle();
				return pattern != null;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TryGetExpandCollapseState(AutomationElement element, out string strstate) {
			strstate = null;
			try {
				ExpandCollapsePattern pattern = element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
				if (pattern == null) {
					return false;
				}
				strstate = pattern.Current.ExpandCollapseState.ToString();
				return true;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TryExpand(AutomationElement element) {
			try {
				ExpandCollapsePattern pattern = element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
				pattern?.Expand();
				return pattern != null;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TryCollapse(AutomationElement element) {
			try {
				ExpandCollapsePattern pattern = element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
				pattern?.Collapse();
				return pattern != null;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TrySelect(AutomationElement element) {
			try {
				SelectionItemPattern pattern = element.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
				pattern?.Select();
				return pattern != null;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TryGetValue(AutomationElement element, out string strvalue, out bool breadonly) {
			strvalue = null;
			breadonly = true;
			try {
				ValuePattern pattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
				if (pattern == null) {
					return false;
				}
				strvalue = pattern.Current.Value;
				breadonly = pattern.Current.IsReadOnly;
				return true;
			}
			catch (Exception) {
				return false;
			}
		}

		public bool TrySetValue(AutomationElement element, string strvalue) {
			try {
				ValuePattern pattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
				if (pattern == null) {
					return false;
				}
				pattern.SetValue(strvalue);
				return true;
			}
			catch (Exception) {
				return false;
			}
		}
	}
}
