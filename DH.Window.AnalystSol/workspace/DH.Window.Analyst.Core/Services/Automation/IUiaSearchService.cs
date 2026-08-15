//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public interface IUiaSearchService {
		// walks each candidate window's UIA subtree looking for a descendant matching AutomationId/UiaName/ControlType on criteria;
		// a candidate window is reported as a match as soon as any single element inside it matches (first hit wins, no per-window duplicates).
		// must run off the UI thread (Task.Run) — this is a cross-process UIA walk, not the fast Win32 EnumWindows path
		List<TopLevelWindowItem> FindTopLevelWindows(IEnumerable<TopLevelWindowItem> listcandidates, WindowSearchCriteria criteria, IWindowTreeService treeservice, CancellationToken token);
	}
}
