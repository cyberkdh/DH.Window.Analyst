//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class WorkspaceControl {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.m_tabWorkspace = new System.Windows.Forms.TabControl();
			this.m_menuTab = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.m_menuitemClose = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuitemDetach = new System.Windows.Forms.ToolStripMenuItem();
			this.m_lblEmptyState = new System.Windows.Forms.Label();
			this.m_toolTipTabs = new System.Windows.Forms.ToolTip(this.components);
			this.m_menuTabList = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.m_menuTab.SuspendLayout();
			this.SuspendLayout();
			//
			// m_tabWorkspace
			//
			this.m_tabWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabWorkspace.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.m_tabWorkspace.ItemSize = new System.Drawing.Size(140, 24);
			this.m_tabWorkspace.Location = new System.Drawing.Point(0, 0);
			this.m_tabWorkspace.Name = "m_tabWorkspace";
			this.m_tabWorkspace.Padding = new System.Drawing.Point(12, 4);
			this.m_tabWorkspace.SelectedIndex = 0;
			this.m_tabWorkspace.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.m_tabWorkspace.Size = new System.Drawing.Size(700, 500);
			this.m_tabWorkspace.TabIndex = 0;
			//
			// m_menuTab
			//
			this.m_menuTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.m_menuitemClose,
			this.m_menuitemDetach});
			this.m_menuTab.Name = "m_menuTab";
			this.m_menuTab.Size = new System.Drawing.Size(140, 48);
			//
			// m_menuitemClose
			//
			this.m_menuitemClose.Name = "m_menuitemClose";
			this.m_menuitemClose.Size = new System.Drawing.Size(139, 22);
			this.m_menuitemClose.Text = "닫기";
			//
			// m_menuitemDetach
			//
			this.m_menuitemDetach.Name = "m_menuitemDetach";
			this.m_menuitemDetach.Size = new System.Drawing.Size(139, 22);
			this.m_menuitemDetach.Text = "새 창으로 분리";
			//
			// m_lblEmptyState
			//
			this.m_lblEmptyState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.m_lblEmptyState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lblEmptyState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
			this.m_lblEmptyState.Location = new System.Drawing.Point(0, 0);
			this.m_lblEmptyState.Name = "m_lblEmptyState";
			this.m_lblEmptyState.Size = new System.Drawing.Size(700, 500);
			this.m_lblEmptyState.TabIndex = 1;
			this.m_lblEmptyState.Text = "더블클릭하여 창을 여세요";
			this.m_lblEmptyState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// m_menuTabList
			//
			this.m_menuTabList.Name = "m_menuTabList";
			this.m_menuTabList.Size = new System.Drawing.Size(140, 4);
			//
			// WorkspaceControl
			//
			this.Controls.Add(this.m_tabWorkspace);
			this.Controls.Add(this.m_lblEmptyState);
			this.Name = "WorkspaceControl";
			this.Size = new System.Drawing.Size(700, 500);
			this.m_menuTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl m_tabWorkspace;
		private System.Windows.Forms.ContextMenuStrip m_menuTab;
		private System.Windows.Forms.ToolStripMenuItem m_menuitemClose;
		private System.Windows.Forms.ToolStripMenuItem m_menuitemDetach;
		private System.Windows.Forms.Label m_lblEmptyState;
		private System.Windows.Forms.ToolTip m_toolTipTabs;
		private System.Windows.Forms.ContextMenuStrip m_menuTabList;
	}
}
