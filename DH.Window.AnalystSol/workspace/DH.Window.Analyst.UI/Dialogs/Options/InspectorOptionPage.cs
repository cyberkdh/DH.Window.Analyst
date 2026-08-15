//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Windows.Forms;
using DH.Window.Analyst.Settings;

namespace DH.Window.Analyst.UI.Dialogs.Options {
	public partial class InspectorOptionPage : UserControl, IOptionPage {
		private bool m_bDirty;
		private bool m_bInitializing;
		private Color m_colorHighlight;

		public event EventHandler DirtyChanged;

		public string Title {
			get { return "Inspector"; }
		}

		public InspectorOptionPage() {
			InitializeComponent();
		}

		public bool IsDirty() {
			return m_bDirty;
		}

		public void LoadSettings() {
			m_bInitializing = true;

			m_numHighlightHoldMs.Value = AppSettings.Get().HighlightHoldMs;
			m_numHighlightFadeMs.Value = AppSettings.Get().HighlightFadeMs;
			m_numAccessibilityMaxElements.Value = AppSettings.Get().AccessibilityMaxElements;
			SetHighlightColor(AppSettings.Get().HighlightColor);

			m_bInitializing = false;
			m_bDirty = false;
		}

		public void ApplySettings() {
			AppSettings.Get().HighlightHoldMs = (int) m_numHighlightHoldMs.Value;
			AppSettings.Get().HighlightFadeMs = (int) m_numHighlightFadeMs.Value;
			AppSettings.Get().AccessibilityMaxElements = (int) m_numAccessibilityMaxElements.Value;
			AppSettings.Get().HighlightColor = m_colorHighlight;

			m_bDirty = false;
		}

		public UserControl GetControl() {
			return this;
		}

		private void SetHighlightColor(Color color) {
			m_colorHighlight = color;
			m_panelHighlightColor.BackColor = color;
		}

		private void OnControlChanged(object sender, EventArgs e) {
			if (m_bInitializing == true) {
				return;
			}

			m_bDirty = true;
			DirtyChanged?.Invoke(this, EventArgs.Empty);
		}

		private void OnHighlightColorClick(object sender, EventArgs e) {
			using (ColorDialog dlgcolor = new ColorDialog { Color = m_colorHighlight }) {
				if (dlgcolor.ShowDialog(this) == DialogResult.OK) {
					SetHighlightColor(dlgcolor.Color);
					OnControlChanged(sender, e);
				}
			}
		}
	}
}
