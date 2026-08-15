//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.Models {
	// IsElevated/GdiObjectCount/UserObjectCount are null when the query itself failed (e.g. UIPI blocks a non-elevated caller), not a legitimate zero/unknown value
	public class ProcessDiagnosticsInfo {
		public string FileVersion { get; }
		public string ProductName { get; }
		public string CompanyName { get; }

		public long WorkingSetBytes { get; }
		public TimeSpan TotalProcessorTime { get; }
		public int ThreadCount { get; }

		public bool? IsElevated { get; }
		public int? GdiObjectCount { get; }
		public int? UserObjectCount { get; }

		public ProcessDiagnosticsInfo(string strfileversion, string strproductname, string strcompanyname,
				long nworkingsetbytes, TimeSpan totalprocessortime, int nthreadcount,
				bool? biselevated, int? ngdiobjectcount, int? nuserobjectcount) {
			FileVersion = strfileversion;
			ProductName = strproductname;
			CompanyName = strcompanyname;
			WorkingSetBytes = nworkingsetbytes;
			TotalProcessorTime = totalprocessortime;
			ThreadCount = nthreadcount;
			IsElevated = biselevated;
			GdiObjectCount = ngdiobjectcount;
			UserObjectCount = nuserobjectcount;
		}
	}
}
