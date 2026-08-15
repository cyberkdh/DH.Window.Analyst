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
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;

namespace DH.Window.Analyst.UI.Controls {
	// uses Control.Capture instead of a global mouse hook so no cross-process injection is needed; the hover frame is drawn via XOR (ControlPaint.DrawReversibleFrame) instead of a real overlay window, since showing/activating any window while Capture is held would cancel the capture and abort the drag
	public class FinderToolButton : Button {
		private const int BORDER_THICKNESS = 3;

		private IWindowTreeService m_treeService;
		private bool m_bDragging;
		private Rectangle m_rectHighlight = Rectangle.Empty;
		private TopLevelWindowItem m_lastHoveredItem;

		public event EventHandler<TopLevelWindowItem> WindowPicked;

		public FinderToolButton() {
			Text = "Find";
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left && m_treeService != null) {
				m_bDragging = true;
				Capture = true;
				Cursor.Current = Cursors.Cross;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			if (m_bDragging == false) {
				return;
			}

			// re-asserted on every move: Windows resets the cursor to whatever the window under the pointer owns
			// (WM_SETCURSOR), so a one-time set in OnMouseDown would not survive moving over another window
			Cursor.Current = Cursors.Cross;

			Point ptscreen = PointToScreen(e.Location);
			TopLevelWindowItem item = m_treeService.GetTopLevelWindowAtScreenPoint(ptscreen);

			if (item?.Handle != m_lastHoveredItem?.Handle) {
				ClearHighlight();

				if (item != null) {
					Rectangle rectwindow = m_treeService.GetWindowScreenRect(item.Handle);
					if (rectwindow.IsEmpty == false) {
						DrawHighlightFrame(rectwindow);
						m_rectHighlight = rectwindow;
					}
				}

				m_lastHoveredItem = item;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);

			if (m_bDragging == false) {
				return;
			}

			m_bDragging = false;
			Capture = false;
			Cursor.Current = Cursors.Default;
			ClearHighlight();

			if (m_lastHoveredItem != null) {
				WindowPicked?.Invoke(this, m_lastHoveredItem);
				m_lastHoveredItem = null;
			}
		}

		// nested reversible frames to fake a thicker border than a single DrawReversibleFrame call allows
		private static void DrawHighlightFrame(Rectangle rectwindow) {
			for (int noffset = 0; noffset < BORDER_THICKNESS; noffset++) {
				Rectangle rectinset = Rectangle.Inflate(rectwindow, -noffset, -noffset);
				if (rectinset.Width > 0 && rectinset.Height > 0) {
					ControlPaint.DrawReversibleFrame(rectinset, Color.Red, FrameStyle.Thick);
				}
			}
		}

		private void ClearHighlight() {
			if (m_rectHighlight.IsEmpty == false) {
				DrawHighlightFrame(m_rectHighlight);
				m_rectHighlight = Rectangle.Empty;
			}
		}
	}
}
