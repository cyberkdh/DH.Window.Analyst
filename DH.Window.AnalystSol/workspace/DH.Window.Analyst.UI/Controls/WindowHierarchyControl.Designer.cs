//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class WindowHierarchyControl {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			if (disposing) {
				m_overlayHighlight.Dispose();
				m_timerSync.Dispose();
				m_hookSyncClick.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_trvHierarchy = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			//
			// m_trvHierarchy
			//
			this.m_trvHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_trvHierarchy.HideSelection = false;
			this.m_trvHierarchy.Location = new System.Drawing.Point(0, 0);
			this.m_trvHierarchy.Name = "m_trvHierarchy";
			this.m_trvHierarchy.ShowNodeToolTips = true;
			this.m_trvHierarchy.Size = new System.Drawing.Size(416, 400);
			this.m_trvHierarchy.TabIndex = 1;
			//
			// WindowHierarchyControl
			//
			this.Controls.Add(this.m_trvHierarchy);
			this.Name = "WindowHierarchyControl";
			this.Size = new System.Drawing.Size(416, 400);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView m_trvHierarchy;
	}
}
