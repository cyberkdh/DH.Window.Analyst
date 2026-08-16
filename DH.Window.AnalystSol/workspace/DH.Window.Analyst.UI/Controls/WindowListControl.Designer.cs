//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class WindowListControl {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			if (disposing) {
				m_overlayHighlight.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.m_panelFilter = new System.Windows.Forms.Panel();
			this.m_ctrlFinder = new DH.Window.Analyst.UI.Controls.FinderToolButton();
			this.m_txtFilter = new System.Windows.Forms.TextBox();
			this.m_lblFilter = new System.Windows.Forms.Label();
			this.m_trvWindows = new System.Windows.Forms.TreeView();
			this.m_ilIcons = new System.Windows.Forms.ImageList(this.components);
			this.m_ctxWindows = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.m_menuCtxExport = new System.Windows.Forms.ToolStripMenuItem();
			this.m_panelFilter.SuspendLayout();
			this.m_ctxWindows.SuspendLayout();
			this.SuspendLayout();
			//
			// m_panelFilter
			//
			this.m_panelFilter.Controls.Add(this.m_txtFilter);
			this.m_panelFilter.Controls.Add(this.m_ctrlFinder);
			this.m_panelFilter.Controls.Add(this.m_lblFilter);
			this.m_panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelFilter.Location = new System.Drawing.Point(0, 0);
			this.m_panelFilter.Name = "m_panelFilter";
			this.m_panelFilter.Size = new System.Drawing.Size(320, 24);
			this.m_panelFilter.TabIndex = 0;
			//
			// m_ctrlFinder
			//
			this.m_ctrlFinder.Dock = System.Windows.Forms.DockStyle.Right;
			this.m_ctrlFinder.Location = new System.Drawing.Point(230, 0);
			this.m_ctrlFinder.Name = "m_ctrlFinder";
			this.m_ctrlFinder.Size = new System.Drawing.Size(90, 24);
			this.m_ctrlFinder.TabIndex = 2;
			this.m_ctrlFinder.Text = "Instant Find";
			this.m_ctrlFinder.UseVisualStyleBackColor = true;
			//
			// m_txtFilter
			//
			this.m_txtFilter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_txtFilter.Location = new System.Drawing.Point(40, 0);
			this.m_txtFilter.Name = "m_txtFilter";
			this.m_txtFilter.Size = new System.Drawing.Size(200, 20);
			this.m_txtFilter.TabIndex = 1;
			//
			// m_lblFilter
			//
			this.m_lblFilter.Dock = System.Windows.Forms.DockStyle.Left;
			this.m_lblFilter.Location = new System.Drawing.Point(0, 0);
			this.m_lblFilter.Name = "m_lblFilter";
			this.m_lblFilter.Size = new System.Drawing.Size(40, 24);
			this.m_lblFilter.TabIndex = 0;
			this.m_lblFilter.Text = "Filter:";
			this.m_lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_trvWindows
			//
			this.m_trvWindows.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_trvWindows.ContextMenuStrip = this.m_ctxWindows;
			this.m_trvWindows.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_trvWindows.FullRowSelect = true;
			this.m_trvWindows.HideSelection = false;
			this.m_trvWindows.HotTracking = true;
			this.m_trvWindows.ImageIndex = 0;
			this.m_trvWindows.ImageList = this.m_ilIcons;
			this.m_trvWindows.ItemHeight = 20;
			this.m_trvWindows.Location = new System.Drawing.Point(0, 24);
			this.m_trvWindows.Name = "m_trvWindows";
			this.m_trvWindows.SelectedImageIndex = 0;
			this.m_trvWindows.ShowNodeToolTips = true;
			this.m_trvWindows.Size = new System.Drawing.Size(320, 376);
			this.m_trvWindows.TabIndex = 1;
			//
			// m_ilIcons
			//
			this.m_ilIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.m_ilIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ilIcons.TransparentColor = System.Drawing.Color.Transparent;
			//
			// m_ctxWindows
			//
			this.m_ctxWindows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.m_menuCtxExport});
			this.m_ctxWindows.Name = "m_ctxWindows";
			this.m_ctxWindows.Size = new System.Drawing.Size(129, 26);
			//
			// m_menuCtxExport
			//
			this.m_menuCtxExport.Name = "m_menuCtxExport";
			this.m_menuCtxExport.Size = new System.Drawing.Size(128, 22);
			this.m_menuCtxExport.Text = "Export...";
			//
			// WindowListControl
			//
			this.Controls.Add(this.m_trvWindows);
			this.Controls.Add(this.m_panelFilter);
			this.Name = "WindowListControl";
			this.Size = new System.Drawing.Size(320, 400);
			this.m_panelFilter.ResumeLayout(false);
			this.m_panelFilter.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_panelFilter;
		private System.Windows.Forms.Label m_lblFilter;
		private System.Windows.Forms.TextBox m_txtFilter;
		private DH.Window.Analyst.UI.Controls.FinderToolButton m_ctrlFinder;
		private System.Windows.Forms.TreeView m_trvWindows;
		private System.Windows.Forms.ImageList m_ilIcons;
		private System.Windows.Forms.ContextMenuStrip m_ctxWindows;
		private System.Windows.Forms.ToolStripMenuItem m_menuCtxExport;
	}
}
