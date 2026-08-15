//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Windows.Automation;

namespace DH.Window.Analyst.Models {
	// Element is kept for possible future "select in tree" navigation from the diagnostic report
	public class AccessibilityIssueItem {
		public AccessibilitySeverity Severity { get; }
		public string RuleName { get; }
		public string ElementName { get; }
		public string ControlType { get; }
		public string AutomationId { get; }
		public AutomationElement Element { get; }

		public AccessibilityIssueItem(AccessibilitySeverity severity, string strrulename, string strelementname, string strcontroltype, string strautomationid, AutomationElement element) {
			Severity = severity;
			RuleName = strrulename;
			ElementName = strelementname;
			ControlType = strcontroltype;
			AutomationId = strautomationid;
			Element = element;
		}
	}
}
