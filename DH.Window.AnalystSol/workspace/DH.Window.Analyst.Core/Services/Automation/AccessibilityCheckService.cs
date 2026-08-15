//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Settings;

namespace DH.Window.Analyst.Services.Automation {
	// unlike IWindowTreeService.GetChildren's immediate-children lazy loading, this walks the entire subtree eagerly since a diagnostic report must cover the whole window at once
	public class AccessibilityCheckService : IAccessibilityCheckService {
		// re-read from AppSettings at the start of every call (not cached at construction) so an Options change takes effect on the next "Run Check" without restarting the app
		private int m_nMaxElementsVisited;

		// only controls a user directly interacts with; missing Name/AutomationId elsewhere is not a defect
		private static readonly HashSet<int> s_setInteractiveControlTypeIds = new HashSet<int> {
			ControlType.Button.Id,
			ControlType.CheckBox.Id,
			ControlType.RadioButton.Id,
			ControlType.ComboBox.Id,
			ControlType.Edit.Id,
			ControlType.Hyperlink.Id,
			ControlType.MenuItem.Id,
			ControlType.Slider.Id,
			ControlType.Spinner.Id,
			ControlType.SplitButton.Id,
			ControlType.TabItem.Id,
		};

		public List<AccessibilityIssueItem> CheckSubtree(AutomationElement root) {
			List<AccessibilityIssueItem> listissues = new List<AccessibilityIssueItem>();
			int nvisited = 0;

			m_nMaxElementsVisited = AppSettings.Get().AccessibilityMaxElements;

			AppLog.i("AccessibilityCheckService.CheckSubtree: scan started");

			WalkElement(root, listissues, ref nvisited);

			if (nvisited >= m_nMaxElementsVisited) {
				AppLog.w($"AccessibilityCheckService.CheckSubtree: hit MAX_ELEMENTS_VISITED cap ({m_nMaxElementsVisited}), scan truncated");
			}

			AppLog.i($"AccessibilityCheckService.CheckSubtree: scan finished, visited={nvisited}, issues={listissues.Count}");

			return listissues;
		}

		private void WalkElement(AutomationElement element, List<AccessibilityIssueItem> listissues, ref int nvisited) {
			if (element == null || nvisited >= m_nMaxElementsVisited) {
				return;
			}
			nvisited++;

			try {
				CheckElement(element, listissues);

				AutomationElementCollection colchildren = element.FindAll(TreeScope.Children, Condition.TrueCondition);
				foreach (AutomationElement child in colchildren) {
					WalkElement(child, listissues, ref nvisited);
				}
			}
			catch (Exception ex) {
				// element went stale or is unresponsive mid-walk; skip its subtree, keep issues already collected
				AppLog.d($"AccessibilityCheckService.WalkElement: skipped a stale/unresponsive element: {ex.Message}");
			}
		}

		private void CheckElement(AutomationElement element, List<AccessibilityIssueItem> listissues) {
			ControlType controltype = element.Current.ControlType;
			if (s_setInteractiveControlTypeIds.Contains(controltype.Id) == false) {
				return;
			}

			string strname = element.Current.Name;
			string strautomationid = element.Current.AutomationId;
			string strcontroltypename = controltype.LocalizedControlType;
			string strdisplayname = string.IsNullOrEmpty(strname) == false ? strname : "(No Name)";

			if (string.IsNullOrWhiteSpace(strname) == true) {
				listissues.Add(new AccessibilityIssueItem(AccessibilitySeverity.Error, "Missing Name", strdisplayname, strcontroltypename, strautomationid, element));
			}

			if (element.Current.IsEnabled == true && element.Current.IsKeyboardFocusable == false) {
				listissues.Add(new AccessibilityIssueItem(AccessibilitySeverity.Warning, "Not Keyboard Focusable", strdisplayname, strcontroltypename, strautomationid, element));
			}

			if (string.IsNullOrEmpty(strautomationid) == true) {
				listissues.Add(new AccessibilityIssueItem(AccessibilitySeverity.Info, "Missing AutomationId", strdisplayname, strcontroltypename, strautomationid, element));
			}
		}
	}
}
