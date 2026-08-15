//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Windows {
	partial class DetachedWorkspaceForm {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_panelTop = new System.Windows.Forms.Panel();
			this.m_btnDockBack = new System.Windows.Forms.Button();
			this.m_panelTop.SuspendLayout();
			this.SuspendLayout();
			//
			// m_panelTop
			//
			this.m_panelTop.Controls.Add(this.m_btnDockBack);
			this.m_panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelTop.Location = new System.Drawing.Point(0, 0);
			this.m_panelTop.Name = "m_panelTop";
			this.m_panelTop.Padding = new System.Windows.Forms.Padding(6);
			this.m_panelTop.Size = new System.Drawing.Size(900, 36);
			this.m_panelTop.TabIndex = 0;
			//
			// m_btnDockBack
			//
			this.m_btnDockBack.Dock = System.Windows.Forms.DockStyle.Left;
			this.m_btnDockBack.Name = "m_btnDockBack";
			this.m_btnDockBack.Size = new System.Drawing.Size(120, 24);
			this.m_btnDockBack.TabIndex = 0;
			this.m_btnDockBack.Text = "Dock Back to Tabs";
			this.m_btnDockBack.UseVisualStyleBackColor = true;
			//
			// DetachedWorkspaceForm
			//
			this.ClientSize = new System.Drawing.Size(900, 650);
			this.Controls.Add(this.m_panelTop);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.Name = "DetachedWorkspaceForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DH.Window.Analyst";
			this.m_panelTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_panelTop;
		private System.Windows.Forms.Button m_btnDockBack;
	}
}
