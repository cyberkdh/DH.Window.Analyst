//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace DH.Window.Analyst.Models {
	public class PropertyItem {
		public string Name { get; }
		public string Value { get; }

		// set only for rows referencing another window (Parent/Owner/First Child/Next/Previous), for "navigate to it"
		public IntPtr? NavigationHandle { get; }

		// set only for bit-flag rows (Style/ExStyle/Class Style)
		public IReadOnlyList<string> FlagDetails { get; }

		public PropertyItem(string strname, string strvalue, IntPtr? hwndnavigation = null, IReadOnlyList<string> listflagdetails = null) {
			Name = strname;
			Value = strvalue;
			NavigationHandle = hwndnavigation;
			FlagDetails = listflagdetails;
		}
	}
}
