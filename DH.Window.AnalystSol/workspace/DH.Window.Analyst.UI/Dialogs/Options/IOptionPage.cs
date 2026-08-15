//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Dialogs.Options {
	// Ported from DHToolSol's IUCOPBase: ApplySettings() runs only when IsDirty() is true on OK.
	public interface IOptionPage {
		event EventHandler DirtyChanged;

		string Title { get; }

		bool IsDirty();
		void LoadSettings();
		void ApplySettings();
		UserControl GetControl();
	}
}
