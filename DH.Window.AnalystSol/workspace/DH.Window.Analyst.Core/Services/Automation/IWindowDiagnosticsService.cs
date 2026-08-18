//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public interface IWindowDiagnosticsService {
		WindowDiagnosticsInfo GetWindowDiagnostics(IntPtr handle);

		// full top-level Z-order stack (front-to-back); the entry matching handle is flagged IsTarget
		IEnumerable<ZOrderEntryItem> GetZOrderStack(IntPtr handle);
	}
}
