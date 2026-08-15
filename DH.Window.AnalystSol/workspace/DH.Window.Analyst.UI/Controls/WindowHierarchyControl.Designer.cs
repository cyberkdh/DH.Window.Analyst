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
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_panelToolbar = new System.Windows.Forms.Panel();
			this.m_btnToggleMode = new System.Windows.Forms.Button();
			this.m_btnHighlight = new System.Windows.Forms.Button();
			this.m_btnForeground = new System.Windows.Forms.Button();
			this.m_trvHierarchy = new System.Windows.Forms.TreeView();
			this.m_panelToolbar.SuspendLayout();
			this.SuspendLayout();
			//
			// m_panelToolbar
			//
			this.m_panelToolbar.Controls.Add(this.m_btnToggleMode);
			this.m_panelToolbar.Controls.Add(this.m_btnHighlight);
			this.m_panelToolbar.Controls.Add(this.m_btnForeground);
			this.m_panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelToolbar.Location = new System.Drawing.Point(0, 0);
			this.m_panelToolbar.Name = "m_panelToolbar";
			this.m_panelToolbar.Size = new System.Drawing.Size(320, 30);
			this.m_panelToolbar.TabIndex = 0;
			//
			// m_btnToggleMode
			//
			this.m_btnToggleMode.Location = new System.Drawing.Point(3, 3);
			this.m_btnToggleMode.Name = "m_btnToggleMode";
			this.m_btnToggleMode.Size = new System.Drawing.Size(90, 23);
			this.m_btnToggleMode.TabIndex = 0;
			this.m_btnToggleMode.Text = "Win32";
			this.m_btnToggleMode.UseVisualStyleBackColor = true;
			//
			// m_btnHighlight
			//
			this.m_btnHighlight.Location = new System.Drawing.Point(99, 3);
			this.m_btnHighlight.Name = "m_btnHighlight";
			this.m_btnHighlight.Size = new System.Drawing.Size(90, 23);
			this.m_btnHighlight.TabIndex = 1;
			this.m_btnHighlight.Text = "Highlight";
			this.m_btnHighlight.UseVisualStyleBackColor = true;
			//
			// m_btnForeground
			//
			this.m_btnForeground.Location = new System.Drawing.Point(195, 3);
			this.m_btnForeground.Name = "m_btnForeground";
			this.m_btnForeground.Size = new System.Drawing.Size(90, 23);
			this.m_btnForeground.TabIndex = 2;
			this.m_btnForeground.Text = "Foreground";
			this.m_btnForeground.UseVisualStyleBackColor = true;
			//
			// m_trvHierarchy
			//
			this.m_trvHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_trvHierarchy.HideSelection = false;
			this.m_trvHierarchy.Location = new System.Drawing.Point(0, 30);
			this.m_trvHierarchy.Name = "m_trvHierarchy";
			this.m_trvHierarchy.ShowNodeToolTips = true;
			this.m_trvHierarchy.Size = new System.Drawing.Size(320, 370);
			this.m_trvHierarchy.TabIndex = 1;
			//
			// WindowHierarchyControl
			//
			this.Controls.Add(this.m_trvHierarchy);
			this.Controls.Add(this.m_panelToolbar);
			this.Name = "WindowHierarchyControl";
			this.Size = new System.Drawing.Size(320, 400);
			this.m_panelToolbar.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_panelToolbar;
		private System.Windows.Forms.Button m_btnToggleMode;
		private System.Windows.Forms.Button m_btnHighlight;
		private System.Windows.Forms.Button m_btnForeground;
		private System.Windows.Forms.TreeView m_trvHierarchy;
	}
}
