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
	public class ElementInspectionService : IElementInspectionService {
		// element can go stale via UIA COM interop at any point; degrade to an empty/partial result rather than propagate the exception into the UI layer
		public List<string> GetParentChainNames(AutomationElement element) {
			List<string> listnames = new List<string>();

			try {
				AutomationElement current = TreeWalker.RawViewWalker.GetParent(element);

				while (current != null) {
					string strname = string.IsNullOrEmpty(current.Current.Name) == true ? current.Current.ClassName : current.Current.Name;
					listnames.Insert(0, string.IsNullOrEmpty(strname) == true ? "(Unnamed)" : strname);
					current = TreeWalker.RawViewWalker.GetParent(current);
				}
			}
			catch (ElementNotAvailableException) {
				// stop walking; return whatever chain was resolved so far
			}

			return listnames;
		}

		public List<ChildSummaryItem> GetChildSummary(AutomationElement element) {
			Dictionary<string, int> diccounts = new Dictionary<string, int>();

			try {
				AutomationElementCollection colchildren = element.FindAll(TreeScope.Children, Condition.TrueCondition);

				foreach (AutomationElement child in colchildren) {
					string strtypename = child.Current.ControlType.LocalizedControlType;

					if (diccounts.ContainsKey(strtypename) == true) {
						diccounts[strtypename] = diccounts[strtypename] + 1;
					}
					else {
						diccounts[strtypename] = 1;
					}
				}
			}
			catch (ElementNotAvailableException) {
				// element gone; report whatever was counted so far (usually empty)
			}

			List<ChildSummaryItem> listsummary = new List<ChildSummaryItem>();
			foreach (KeyValuePair<string, int> pair in diccounts) {
				listsummary.Add(new ChildSummaryItem(pair.Key, pair.Value));
			}

			return listsummary;
		}

		public List<AutomationPattern> GetSupportedPatterns(AutomationElement element) {
			try {
				return new List<AutomationPattern>(element.GetSupportedPatterns());
			}
			catch (ElementNotAvailableException) {
				return new List<AutomationPattern>();
			}
		}
	}
}
