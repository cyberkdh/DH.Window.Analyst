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
		// must run off the UI thread (Task.Run) — this is a cross-process UIA walk, not the fast Win32 EnumWindows path
		List<TopLevelWindowItem> FindTopLevelWindows(IEnumerable<TopLevelWindowItem> listcandidates, WindowSearchCriteria criteria, IWindowTreeService treeservice, CancellationToken token);
	}
}
