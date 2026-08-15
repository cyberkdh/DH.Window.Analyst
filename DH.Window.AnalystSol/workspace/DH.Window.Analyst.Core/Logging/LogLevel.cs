//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace DH.Window.Analyst.Logging {
	[Flags]
	public enum LogLevel {
		Debug = 0x08,
		Info = 0x04,
		Warn = 0x02,
		Error = 0x01,
		All = Debug | Info | Warn | Error
	}
}
