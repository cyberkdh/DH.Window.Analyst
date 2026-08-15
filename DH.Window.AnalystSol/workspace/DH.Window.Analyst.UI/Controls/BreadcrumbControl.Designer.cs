//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class BreadcrumbControl {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_flowPath = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			//
			// m_flowPath
			//
			this.m_flowPath.AutoScroll = false;
			this.m_flowPath.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_flowPath.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
			this.m_flowPath.Location = new System.Drawing.Point(0, 0);
			this.m_flowPath.Name = "m_flowPath";
			this.m_flowPath.Padding = new System.Windows.Forms.Padding(6, 4, 6, 0);
			this.m_flowPath.Size = new System.Drawing.Size(400, 28);
			this.m_flowPath.TabIndex = 0;
			this.m_flowPath.WrapContents = false;
			//
			// BreadcrumbControl
			//
			this.Controls.Add(this.m_flowPath);
			this.Name = "BreadcrumbControl";
			this.Size = new System.Drawing.Size(400, 28);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel m_flowPath;
	}
}
