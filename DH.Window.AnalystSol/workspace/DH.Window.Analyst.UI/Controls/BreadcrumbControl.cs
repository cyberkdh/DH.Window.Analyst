//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DH.Window.Analyst.UI.Models;

namespace DH.Window.Analyst.UI.Controls {
	// generic clickable path bar with no knowledge of Win32/UIA — caller supplies segments and gets a click-back with whatever Tag it attached
	public partial class BreadcrumbControl : UserControl {
		private const string ELLIPSIS_TEXT = "...";

		private List<BreadcrumbSegment> m_listSegments = new List<BreadcrumbSegment>();

		public event EventHandler<object> SegmentClicked;

		public BreadcrumbControl() {
			InitializeComponent();
			// path bar width changes with the window/tab, so segments must be re-collapsed on resize, not just once at SetPath time
			Resize += (sender, e) => Rebuild();
		}

		public void SetPath(IEnumerable<BreadcrumbSegment> listsegments) {
			m_listSegments = new List<BreadcrumbSegment>(listsegments);
			Rebuild();
		}

		// drops leading segments (oldest/most distant from the current node) behind a clickable "..." when the full path does not fit,
		// so the bar never needs a scrollbar (which was squeezing the fixed-height strip and hiding the text entirely)
		private void Rebuild() {
			m_flowPath.SuspendLayout();

			// Controls.Clear() only unparents controls, it does not Dispose them — omitting this would leak a Label/LinkLabel per click
			while (m_flowPath.Controls.Count > 0) {
				Control controlold = m_flowPath.Controls[m_flowPath.Controls.Count - 1];
				m_flowPath.Controls.RemoveAt(m_flowPath.Controls.Count - 1);
				controlold.Dispose();
			}

			if (m_listSegments.Count > 0) {
				int navailablewidth = ClientSize.Width - m_flowPath.Padding.Horizontal;
				if (navailablewidth <= 0) {
					navailablewidth = int.MaxValue;
				}

				List<BreadcrumbSegment> listtail = new List<BreadcrumbSegment>(m_listSegments);
				List<BreadcrumbSegment> listhidden = new List<BreadcrumbSegment>();

				while (listtail.Count > 1 && MeasureRowWidth(listtail, listhidden.Count > 0) > navailablewidth) {
					listhidden.Add(listtail[0]);
					listtail.RemoveAt(0);
				}

				bool bfirst = true;

				if (listhidden.Count > 0) {
					LinkLabel linkellipsis = new LinkLabel {
						Text = ELLIPSIS_TEXT,
						AutoSize = true,
						Margin = new Padding(0, 6, 0, 0),
						LinkBehavior = LinkBehavior.HoverUnderline
					};
					linkellipsis.LinkClicked += (sender, e) => ShowHiddenSegmentsMenu(listhidden, (Control) sender);
					m_flowPath.Controls.Add(linkellipsis);
					bfirst = false;
				}

				foreach (BreadcrumbSegment segment in listtail) {
					if (bfirst == false) {
						m_flowPath.Controls.Add(new Label {
							Text = ">",
							AutoSize = true,
							Margin = new Padding(4, 6, 4, 0),
							ForeColor = System.Drawing.SystemColors.GrayText
						});
					}
					bfirst = false;

					LinkLabel linksegment = new LinkLabel {
						Text = segment.Text,
						Tag = segment.Tag,
						AutoSize = true,
						Margin = new Padding(0, 6, 0, 0),
						LinkBehavior = LinkBehavior.HoverUnderline
					};
					linksegment.LinkClicked += (sender, e) => SegmentClicked?.Invoke(this, ((LinkLabel) sender).Tag);
					m_flowPath.Controls.Add(linksegment);
				}
			}

			m_flowPath.ResumeLayout();
		}

		private int MeasureRowWidth(List<BreadcrumbSegment> listtail, bool bwithellipsis) {
			int ntotal = 0;
			bool bfirst = true;

			if (bwithellipsis) {
				ntotal += TextRenderer.MeasureText(ELLIPSIS_TEXT, Font).Width + 4;
				bfirst = false;
			}

			foreach (BreadcrumbSegment segment in listtail) {
				if (bfirst == false) {
					ntotal += TextRenderer.MeasureText(">", Font).Width + 8;
				}
				bfirst = false;
				ntotal += TextRenderer.MeasureText(segment.Text, Font).Width + 4;
			}

			return ntotal;
		}

		// mirrors clicking a visible segment: any hidden segment picked here fires the same SegmentClicked event with its Tag
		private void ShowHiddenSegmentsMenu(List<BreadcrumbSegment> listhidden, Control controlanchor) {
			ContextMenuStrip menu = new ContextMenuStrip();
			// disposing inside Closed itself throws ObjectDisposedException — ToolStripDropDown keeps touching the strip after Closed returns, within the same call stack, so the actual Dispose must be deferred a tick
			menu.Closed += (sender, e) => menu.BeginInvoke((MethodInvoker) (() => menu.Dispose()));

			foreach (BreadcrumbSegment segment in listhidden) {
				ToolStripMenuItem item = new ToolStripMenuItem(segment.Text) { Tag = segment.Tag };
				item.Click += (sender, e) => SegmentClicked?.Invoke(this, ((ToolStripMenuItem) sender).Tag);
				menu.Items.Add(item);
			}

			menu.Show(controlanchor, new System.Drawing.Point(0, controlanchor.Height));
		}
	}
}
