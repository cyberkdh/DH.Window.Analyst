//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Settings;

namespace DH.Window.Analyst.Services.Automation {
	// reuses the same visited-element budget as AccessibilityCheckService (AppSettings.AccessibilityMaxElements) since both are
	// unbounded-in-principle cross-process UIA subtree walks that must never be allowed to hang the Inspector on a huge/unresponsive tree
	public class UiaSearchService : IUiaSearchService {
		public List<TopLevelWindowItem> FindTopLevelWindows(IEnumerable<TopLevelWindowItem> listcandidates, WindowSearchCriteria criteria, IWindowTreeService treeservice, CancellationToken token) {
			List<TopLevelWindowItem> listmatches = new List<TopLevelWindowItem>();
			int nmaxelements = AppSettings.Get().AccessibilityMaxElements;
			int nvisited = 0;

			foreach (TopLevelWindowItem itemwindow in listcandidates) {
				token.ThrowIfCancellationRequested();

				if (nvisited >= nmaxelements) {
					AppLog.w($"UiaSearchService.FindTopLevelWindows: hit MAX_ELEMENTS_VISITED cap ({nmaxelements}), search truncated");
					break;
				}

				AutomationElement root = treeservice.CreateElementFromHandle(itemwindow.Handle);
				if (root == null) {
					continue;
				}

				if (WalkElement(root, criteria, treeservice, token, ref nvisited, nmaxelements) == true) {
					listmatches.Add(itemwindow);
				}
			}

			return listmatches;
		}

		// depth-first, returns true as soon as any element in this subtree matches — no need to keep walking once a window is confirmed a match
		private bool WalkElement(AutomationElement element, WindowSearchCriteria criteria, IWindowTreeService treeservice, CancellationToken token, ref int nvisited, int nmaxelements) {
			if (element == null || nvisited >= nmaxelements) {
				return false;
			}

			token.ThrowIfCancellationRequested();
			nvisited++;

			try {
				if (IsElementMatch(element, criteria) == true) {
					return true;
				}

				foreach (AutomationElement child in treeservice.GetChildren(element)) {
					if (WalkElement(child, criteria, treeservice, token, ref nvisited, nmaxelements) == true) {
						return true;
					}
				}
			}
			catch (Exception ex) {
				AppLog.d($"UiaSearchService.WalkElement: skipped a stale/unresponsive element: {ex.Message}");
			}

			return false;
		}

		private static bool IsElementMatch(AutomationElement element, WindowSearchCriteria criteria) {
			AutomationElement.AutomationElementInformation info = element.Current;

			if (string.IsNullOrEmpty(criteria.AutomationId) == false
				&& (info.AutomationId ?? string.Empty).IndexOf(criteria.AutomationId, StringComparison.OrdinalIgnoreCase) < 0) {
				return false;
			}

			if (string.IsNullOrEmpty(criteria.UiaName) == false
				&& (info.Name ?? string.Empty).IndexOf(criteria.UiaName, StringComparison.OrdinalIgnoreCase) < 0) {
				return false;
			}

			if (string.IsNullOrEmpty(criteria.ControlType) == false
				&& (info.ControlType.LocalizedControlType ?? string.Empty).IndexOf(criteria.ControlType, StringComparison.OrdinalIgnoreCase) < 0) {
				return false;
			}

			return true;
		}
	}
}
