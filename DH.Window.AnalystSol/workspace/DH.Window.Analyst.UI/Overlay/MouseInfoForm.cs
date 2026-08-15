//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Overlay {
	// small floating panel that follows the cursor during "Show Info on Mouse" mode; same WS_EX_NOACTIVATE/TOOLWINDOW skeleton as HighlightOverlayForm so it never steals focus/clicks from the window under the cursor
	public class MouseInfoForm : Form {
		private const int WS_EX_NOACTIVATE = 0x08000000;
		private const int WS_EX_TOOLWINDOW = 0x00000080;

		private const int CURSOR_OFFSET_X = 16;
		private const int CURSOR_OFFSET_Y = 16;

		private static readonly Color COLOR_WARNING = Color.FromArgb(0xC0, 0x00, 0x00);

		private readonly FlowLayoutPanel m_panelStack;
		private readonly TableLayoutPanel m_tableFields;
		private readonly Label m_lblWarning;

		// reused across ticks instead of recreated — ShowInfo is called every 50ms while hovering, and destroying/recreating a Label's underlying
		// HWND that often (rather than just updating Text) is what caused the visible flicker
		private readonly List<Label> m_listLabelKeys = new List<Label>();
		private readonly List<Label> m_listLabelValues = new List<Label>();

		public MouseInfoForm() {
			FormBorderStyle = FormBorderStyle.None;
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.Manual;
			TopMost = true;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			BackColor = SystemColors.Info;
			DoubleBuffered = true;

			// TopDown flow instead of a single long Label: the field table and the elevation warning (added separately, red)
			// only takes a row when it actually applies
			m_panelStack = new FlowLayoutPanel {
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				FlowDirection = FlowDirection.TopDown,
				WrapContents = false,
				Padding = new Padding(4),
				BackColor = SystemColors.Info
			};

			m_tableFields = new TableLayoutPanel {
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				ColumnCount = 2,
				CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
				BackColor = SystemColors.Info
			};
			m_tableFields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			m_tableFields.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			m_lblWarning = new Label {
				AutoSize = true,
				ForeColor = COLOR_WARNING,
				BackColor = SystemColors.Info,
				Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold),
				Margin = new Padding(4, 4, 4, 0),
				Visible = false
			};

			m_panelStack.Controls.Add(m_tableFields);
			m_panelStack.Controls.Add(m_lblWarning);
			Controls.Add(m_panelStack);
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
				return cp;
			}
		}

		protected override bool ShowWithoutActivation {
			get { return true; }
		}

		// fields is applied onto a fixed pool of reused Label controls (grown on demand, never shrunk/recreated) — ShowInfo runs every 50ms while
		// hovering, so recreating controls here every tick is what caused the flicker; updating .Text on existing controls does not.
		// Callers should only invoke this when the content actually changed (e.g. the hovered window changed) — call MoveTo separately every
		// tick to just follow the cursor, since re-assigning unchanged Label.Text still triggers an avoidable invalidate/layout pass.
		public void ShowInfo(Point ptscreen, IReadOnlyList<KeyValuePair<string, string>> fields, string strwarning = null) {
			m_tableFields.SuspendLayout();

			while (m_listLabelKeys.Count < fields.Count) {
				int nrow = m_listLabelKeys.Count;
				m_tableFields.RowCount = nrow + 1;
				m_tableFields.RowStyles.Add(new RowStyle(SizeType.AutoSize));

				Label lbllabel = new Label {
					AutoSize = true,
					Font = new Font(SystemFonts.MessageBoxFont, FontStyle.Bold),
					ForeColor = SystemColors.InfoText,
					BackColor = SystemColors.Info,
					Margin = new Padding(4, 2, 10, 2),
					TextAlign = ContentAlignment.MiddleLeft
				};
				Label lblvalue = new Label {
					AutoSize = true,
					Font = SystemFonts.MessageBoxFont,
					ForeColor = SystemColors.InfoText,
					BackColor = SystemColors.Info,
					Margin = new Padding(4, 2, 4, 2),
					TextAlign = ContentAlignment.MiddleLeft
				};

				m_tableFields.Controls.Add(lbllabel, 0, nrow);
				m_tableFields.Controls.Add(lblvalue, 1, nrow);
				m_listLabelKeys.Add(lbllabel);
				m_listLabelValues.Add(lblvalue);
			}

			for (int nrow = 0; nrow < m_listLabelKeys.Count; nrow++) {
				bool bisrowused = nrow < fields.Count;
				m_listLabelKeys[nrow].Visible = bisrowused;
				m_listLabelValues[nrow].Visible = bisrowused;
				m_tableFields.RowStyles[nrow].SizeType = bisrowused == true ? SizeType.AutoSize : SizeType.Absolute;
				if (bisrowused == true) {
					m_listLabelKeys[nrow].Text = fields[nrow].Key;
					m_listLabelValues[nrow].Text = fields[nrow].Value;
				}
			}

			m_tableFields.ResumeLayout(true);

			m_lblWarning.Visible = string.IsNullOrEmpty(strwarning) == false;
			if (m_lblWarning.Visible == true) {
				m_lblWarning.Text = strwarning;
			}

			PerformLayout();

			MoveTo(ptscreen);

			if (Visible == false) {
				Show();
			}
		}

		// cursor-follow only, no content/layout work — safe to call every hover-poll tick even when ShowInfo is skipped because the hovered window didn't change
		public void MoveTo(Point ptscreen) {
			int nx = ptscreen.X + CURSOR_OFFSET_X;
			int ny = ptscreen.Y + CURSOR_OFFSET_Y;

			Rectangle rectvirtualscreen = SystemInformation.VirtualScreen;
			nx = Math.Min(nx, rectvirtualscreen.Right - Width);
			ny = Math.Min(ny, rectvirtualscreen.Bottom - Height);
			nx = Math.Max(nx, rectvirtualscreen.Left);
			ny = Math.Max(ny, rectvirtualscreen.Top);

			Location = new Point(nx, ny);
		}

		public void HideInfo() {
			Hide();
		}
	}
}
