//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs.Options {
	partial class GeneralOptionPage {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent() {
			this.m_chkGroupByProcessDefault = new System.Windows.Forms.CheckBox();
			this.m_chkRememberWindowBounds = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			//
			// m_chkGroupByProcessDefault
			//
			this.m_chkGroupByProcessDefault.AutoSize = true;
			this.m_chkGroupByProcessDefault.Location = new System.Drawing.Point(3, 3);
			this.m_chkGroupByProcessDefault.Name = "m_chkGroupByProcessDefault";
			this.m_chkGroupByProcessDefault.Size = new System.Drawing.Size(230, 17);
			this.m_chkGroupByProcessDefault.TabIndex = 0;
			this.m_chkGroupByProcessDefault.Text = "Group window list by process by default";
			this.m_chkGroupByProcessDefault.UseVisualStyleBackColor = true;
			this.m_chkGroupByProcessDefault.CheckedChanged += new System.EventHandler(this.OnControlChanged);
			//
			// m_chkRememberWindowBounds
			//
			this.m_chkRememberWindowBounds.AutoSize = true;
			this.m_chkRememberWindowBounds.Location = new System.Drawing.Point(3, 30);
			this.m_chkRememberWindowBounds.Name = "m_chkRememberWindowBounds";
			this.m_chkRememberWindowBounds.Size = new System.Drawing.Size(230, 17);
			this.m_chkRememberWindowBounds.TabIndex = 1;
			this.m_chkRememberWindowBounds.Text = "Remember main window position/size";
			this.m_chkRememberWindowBounds.UseVisualStyleBackColor = true;
			this.m_chkRememberWindowBounds.CheckedChanged += new System.EventHandler(this.OnControlChanged);
			//
			// GeneralOptionPage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_chkRememberWindowBounds);
			this.Controls.Add(this.m_chkGroupByProcessDefault);
			this.Name = "GeneralOptionPage";
			this.Size = new System.Drawing.Size(320, 300);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox m_chkGroupByProcessDefault;
		private System.Windows.Forms.CheckBox m_chkRememberWindowBounds;
	}
}
