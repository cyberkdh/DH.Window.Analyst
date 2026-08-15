//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.Models {
	// Caption/ClassName/Handle/ProcessId/ProcessName/ControlId are cheap Win32-level filters (fast EnumWindows walk);
	// AutomationId/UiaName/ControlType require a cross-process UIA subtree walk and are far more expensive, so callers
	// must branch on HasUiaCriteria to route the search through the background/cancellable path instead of the synchronous one
	public class WindowSearchCriteria {
		public string Caption { get; set; }
		public string ClassName { get; set; }
		public IntPtr Handle { get; set; }
		public int? ProcessId { get; set; }
		public string ProcessName { get; set; }
		public int? ControlId { get; set; }
		public string AutomationId { get; set; }
		public string UiaName { get; set; }
		public string ControlType { get; set; }

		public bool HasUiaCriteria {
			get {
				return string.IsNullOrEmpty(AutomationId) == false
					|| string.IsNullOrEmpty(UiaName) == false
					|| string.IsNullOrEmpty(ControlType) == false;
			}
		}
	}
}
