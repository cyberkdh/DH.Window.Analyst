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

namespace DH.Window.Analyst.UI.Overlay {
	// WS_EX_TRANSPARENT/NOACTIVATE via CreateParams ensures this overlay never steals clicks/focus.
	public class HighlightOverlayForm : Form {
		private const int WS_EX_TRANSPARENT = 0x00000020;
		private const int WS_EX_NOACTIVATE = 0x08000000;
		private const int WS_EX_TOOLWINDOW = 0x00000080;

		private const double INITIAL_OPACITY = 0.35;
		private const int TICK_MS = 30;

		private readonly Timer m_timer;
		private int m_nElapsedMs;

		// Re-read from AppSettings on each FlashAt() so Options changes apply without restart, yet stay fixed mid-fade.
		private int m_nHoldMs;
		private int m_nFadeMs;

		public HighlightOverlayForm() {
			FormBorderStyle = FormBorderStyle.None;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.Manual;
			TopMost = true;

			m_timer = new Timer { Interval = TICK_MS };
			m_timer.Tick += OnTick;
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS_EX_TRANSPARENT | WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
				return cp;
			}
		}

		protected override bool ShowWithoutActivation {
			get { return true; }
		}

		public void FlashAt(Rectangle rectscreen) {
			m_timer.Stop();

			if (rectscreen.IsEmpty == true) {
				Hide();
				return;
			}

			m_nHoldMs = AppSettings.Get().HighlightHoldMs;
			m_nFadeMs = AppSettings.Get().HighlightFadeMs;
			BackColor = AppSettings.Get().HighlightColor;

			m_nElapsedMs = 0;
			Bounds = rectscreen;
			Opacity = INITIAL_OPACITY;

			if (Visible == false) {
				Show();
			}

			m_timer.Start();
		}

		private void OnTick(object sender, EventArgs e) {
			m_nElapsedMs += TICK_MS;

			if (m_nElapsedMs < m_nHoldMs) {
				return;
			}

			double dfaderatio = 1.0 - (double) (m_nElapsedMs - m_nHoldMs) / m_nFadeMs;
			if (dfaderatio <= 0) {
				m_timer.Stop();
				Hide();
				return;
			}

			Opacity = INITIAL_OPACITY * dfaderatio;
		}

		protected override void Dispose(bool disposing) {
			if (disposing == true) {
				m_timer.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
