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
using System.Threading.Tasks;
using System.Windows.Forms;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Windows;

namespace DH.Window.Analyst.UI.Controls {
	// tab host for WindowWorkspaceTabControl instances: one tab per window handle, reuses an already-open tab instead of duplicating it, closable via right-click
	public partial class WorkspaceControl : UserControl {
		private const int CLOSE_BUTTON_SIZE = 14;
		private const int CLOSE_BUTTON_MARGIN = 6;

		private IWindowTreeService m_treeService;
		private readonly Dictionary<IntPtr, TabPage> m_dicOpenTabs = new Dictionary<IntPtr, TabPage>();

		// tracks which tab the tooltip text currently reflects, so repeated MouseMove events over the same tab don't re-issue SetToolTip (which restarts the tooltip's show delay and causes flicker)
		private int m_nLastTooltipTabIndex = -1;

		// bubbled from a workspace tab's property view; the host (MainForm) resolves this into both a Windows-list selection and a workspace tab
		public event EventHandler<IntPtr> WindowReferenceActivated;

		public WorkspaceControl() {
			InitializeComponent();

			m_tabWorkspace.DrawItem += OnDrawTabItem;
			m_tabWorkspace.MouseDown += OnTabMouseDown;
			m_tabWorkspace.MouseUp += OnTabMouseUp;
			m_tabWorkspace.MouseMove += OnTabMouseMove;
			m_tabWorkspace.MouseLeave += (sender, e) => {
				m_nLastTooltipTabIndex = -1;
				m_toolTipTabs.SetToolTip(m_tabWorkspace, string.Empty);
			};
			m_menuTab.Opening += (sender, e) => e.Cancel = m_tabWorkspace.TabCount == 0;
			// use SelectedTab (already set by OnTabMouseUp before the menu showed) rather than re-deriving a tab from the cursor, which is stale by the time Click fires
			m_menuitemClose.Click += (sender, e) => CloseTab(m_tabWorkspace.SelectedTab);
			m_menuitemDetach.Click += (sender, e) => DetachTab(m_tabWorkspace.SelectedTab);

			UpdateEmptyState();
		}

		public void Initialize(IWindowTreeService treeservice) {
			m_treeService = treeservice;
		}

		// the currently selected tab's content, or null if no tab is open/selected — used by Show Info on Mouse to reach into the just-opened tab's tree
		public WindowWorkspaceTabControl ActiveTabControl {
			get { return m_tabWorkspace.SelectedTab?.Controls.Count > 0 ? m_tabWorkspace.SelectedTab.Controls[0] as WindowWorkspaceTabControl : null; }
		}

		public void CloseSelectedTab() {
			CloseTab(m_tabWorkspace.SelectedTab);
		}

		public void DetachSelectedTab() {
			DetachTab(m_tabWorkspace.SelectedTab);
		}

		// full tab titles, unlike the fixed-width ellipsized tab strip; screenlocation lets a non-Control caller (menu item, toolbar button) position the popup
		public void ShowTabListPopup(Point screenlocation) {
			if (m_tabWorkspace.TabCount == 0) {
				return;
			}

			m_menuTabList.Items.Clear();

			foreach (TabPage page in m_tabWorkspace.TabPages) {
				ToolStripMenuItem menuitem = new ToolStripMenuItem(page.Text) { Checked = page == m_tabWorkspace.SelectedTab };
				menuitem.Click += (sendermenu, emenu) => m_tabWorkspace.SelectedTab = page;
				m_menuTabList.Items.Add(menuitem);
			}

			m_menuTabList.Show(screenlocation);
		}

		// fills a submenu (e.g. the Window > Tab List menu item's DropDownItems) with one entry per open tab, for callers that want a hover-to-expand submenu rather than the click-triggered popup of ShowTabListPopup
		public void PopulateTabListSubmenu(ToolStripItemCollection items) {
			items.Clear();

			if (m_tabWorkspace.TabCount == 0) {
				items.Add(new ToolStripMenuItem("(No tabs open)") { Enabled = false });
				return;
			}

			foreach (TabPage page in m_tabWorkspace.TabPages) {
				ToolStripMenuItem menuitem = new ToolStripMenuItem(page.Text) { Checked = page == m_tabWorkspace.SelectedTab };
				menuitem.Click += (sendermenu, emenu) => m_tabWorkspace.SelectedTab = page;
				items.Add(menuitem);
			}
		}

		// fills the "Take Property Snapshot" submenu: a "Current" entry (whatever's selected in the active tab's Inspector) plus one entry per open tab, so capture isn't tied to a single fixed source
		public void PopulateSnapshotTargetSubmenu(ToolStripItemCollection items, Func<WindowWorkspaceTabControl, Task> onselect) {
			items.Clear();

			if (m_tabWorkspace.TabCount == 0) {
				items.Add(new ToolStripMenuItem("(No tabs open)") { Enabled = false });
				return;
			}

			if (ActiveTabControl != null) {
				WindowWorkspaceTabControl ctrlactive = ActiveTabControl;
				ToolStripMenuItem menuitemcurrent = new ToolStripMenuItem("Current");
				menuitemcurrent.Click += async (sendermenu, emenu) => await onselect(ctrlactive);
				items.Add(menuitemcurrent);
				items.Add(new ToolStripSeparator());
			}

			foreach (TabPage page in m_tabWorkspace.TabPages) {
				if (!(page.Controls.Count > 0 && page.Controls[0] is WindowWorkspaceTabControl ctrlworkspacetab)) {
					continue;
				}

				ToolStripMenuItem menuitem = new ToolStripMenuItem(page.Text) { Checked = page == m_tabWorkspace.SelectedTab };
				menuitem.Click += async (sendermenu, emenu) => await onselect(ctrlworkspacetab);
				items.Add(menuitem);
			}
		}

		public async Task OpenWindowAsync(TopLevelWindowItem item) {
			if (item == null || m_treeService == null) {
				return;
			}

			if (m_dicOpenTabs.TryGetValue(item.Handle, out TabPage pageexisting) == true) {
				m_tabWorkspace.SelectedTab = pageexisting;
				return;
			}

			WindowWorkspaceTabControl ctrlworkspacetab = new WindowWorkspaceTabControl { Dock = DockStyle.Fill };
			ctrlworkspacetab.Initialize(m_treeService);
			ctrlworkspacetab.WindowReferenceActivated += (sender, handle) => WindowReferenceActivated?.Invoke(this, handle);

			string strtitle = string.IsNullOrEmpty(item.Title) == false ? item.Title : item.ClassName;
			TabPage pagenew = new TabPage(strtitle) { Tag = item.Handle };
			pagenew.Controls.Add(ctrlworkspacetab);

			m_tabWorkspace.TabPages.Add(pagenew);
			m_dicOpenTabs[item.Handle] = pagenew;
			m_tabWorkspace.SelectedTab = pagenew;
			UpdateEmptyState();

			await ctrlworkspacetab.ShowWindowAsync(item);
		}

		private void OnDrawTabItem(object sender, DrawItemEventArgs e) {
			TabPage page = m_tabWorkspace.TabPages[e.Index];
			Rectangle recttab = m_tabWorkspace.GetTabRect(e.Index);
			bool bisselected = e.Index == m_tabWorkspace.SelectedIndex;

			using (SolidBrush brush = new SolidBrush(bisselected == true ? SystemColors.Window : SystemColors.Control)) {
				e.Graphics.FillRectangle(brush, recttab);
			}

			Rectangle rectclose = GetCloseButtonRect(recttab);
			Rectangle recttext = new Rectangle(recttab.Left + 4, recttab.Top, rectclose.Left - recttab.Left - 6, recttab.Height);
			TextRenderer.DrawText(e.Graphics, page.Text, m_tabWorkspace.Font, recttext, SystemColors.ControlText,
					TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

			TextRenderer.DrawText(e.Graphics, "x", m_tabWorkspace.Font, rectclose, SystemColors.ControlDarkDark,
					TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}

		private static Rectangle GetCloseButtonRect(Rectangle recttab) {
			return new Rectangle(recttab.Right - CLOSE_BUTTON_SIZE - CLOSE_BUTTON_MARGIN, recttab.Top + (recttab.Height - CLOSE_BUTTON_SIZE) / 2, CLOSE_BUTTON_SIZE, CLOSE_BUTTON_SIZE);
		}

		private void OnTabMouseDown(object sender, MouseEventArgs e) {
			if (e.Button != MouseButtons.Left) {
				return;
			}

			for (int nindex = 0; nindex < m_tabWorkspace.TabCount; nindex++) {
				if (GetCloseButtonRect(m_tabWorkspace.GetTabRect(nindex)).Contains(e.Location) == true) {
					CloseTab(m_tabWorkspace.TabPages[nindex]);
					return;
				}
			}
		}

		private void OnTabMouseUp(object sender, MouseEventArgs e) {
			if (e.Button != MouseButtons.Right) {
				return;
			}

			TabPage pageclicked = FindTabAtPoint(e.Location);
			if (pageclicked != null) {
				m_tabWorkspace.SelectedTab = pageclicked;
				m_menuTab.Show(m_tabWorkspace, e.Location);
			}
		}

		// fixed-width tabs ellipsize long titles (OnDrawTabItem), so the tooltip surfaces the untruncated TabPage.Text on hover
		private void OnTabMouseMove(object sender, MouseEventArgs e) {
			int nindex = -1;
			for (int ncandidate = 0; ncandidate < m_tabWorkspace.TabCount; ncandidate++) {
				if (m_tabWorkspace.GetTabRect(ncandidate).Contains(e.Location) == true) {
					nindex = ncandidate;
					break;
				}
			}

			if (nindex == m_nLastTooltipTabIndex) {
				return;
			}

			m_nLastTooltipTabIndex = nindex;
			m_toolTipTabs.SetToolTip(m_tabWorkspace, nindex >= 0 ? m_tabWorkspace.TabPages[nindex].Text : string.Empty);
		}

		private TabPage FindTabAtPoint(System.Drawing.Point point) {
			for (int nindex = 0; nindex < m_tabWorkspace.TabCount; nindex++) {
				if (m_tabWorkspace.GetTabRect(nindex).Contains(point) == true) {
					return m_tabWorkspace.TabPages[nindex];
				}
			}
			return null;
		}

		private void CloseTab(TabPage page) {
			if (page == null) {
				return;
			}

			m_dicOpenTabs.Remove((IntPtr) page.Tag);
			m_tabWorkspace.TabPages.Remove(page);
			page.Dispose();
			UpdateEmptyState();
		}

		// tears the tab's content out into an independent top-level window; the detached form's "Dock Back to Tabs" button calls AttachTab to reverse this
		private void DetachTab(TabPage page) {
			if (page == null || page.Controls.Count == 0) {
				return;
			}

			if (!(page.Controls[0] is WindowWorkspaceTabControl ctrlworkspacetab)) {
				return;
			}

			IntPtr handle = (IntPtr) page.Tag;
			string strtitle = page.Text;

			page.Controls.Remove(ctrlworkspacetab);
			m_dicOpenTabs.Remove(handle);
			m_tabWorkspace.TabPages.Remove(page);
			page.Dispose();
			UpdateEmptyState();

			DetachedWorkspaceForm formdetached = new DetachedWorkspaceForm(ctrlworkspacetab, strtitle, () => AttachTab(ctrlworkspacetab, strtitle, handle));
			formdetached.Show();
		}

		// re-attaches a control previously torn off via DetachTab, as a new tab in this strip
		private void AttachTab(WindowWorkspaceTabControl ctrlworkspacetab, string strtitle, IntPtr handle) {
			ctrlworkspacetab.Dock = DockStyle.Fill;

			TabPage pagenew = new TabPage(strtitle) { Tag = handle };
			pagenew.Controls.Add(ctrlworkspacetab);

			m_tabWorkspace.TabPages.Add(pagenew);
			m_dicOpenTabs[handle] = pagenew;
			m_tabWorkspace.SelectedTab = pagenew;
			UpdateEmptyState();
		}

		private void UpdateEmptyState() {
			m_lblEmptyState.Visible = m_tabWorkspace.TabCount == 0;
		}
	}
}
