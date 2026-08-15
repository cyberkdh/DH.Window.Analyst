//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs.Options {
	partial class OptionsForm {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_lstCategories = new System.Windows.Forms.ListBox();
			this.m_panelPage = new System.Windows.Forms.Panel();
			this.m_panelBottom = new System.Windows.Forms.Panel();
			this.m_btnOK = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_panelBottom.SuspendLayout();
			this.SuspendLayout();
			//
			// m_lstCategories
			//
			this.m_lstCategories.Dock = System.Windows.Forms.DockStyle.Left;
			this.m_lstCategories.IntegralHeight = false;
			this.m_lstCategories.Location = new System.Drawing.Point(0, 0);
			this.m_lstCategories.Name = "m_lstCategories";
			this.m_lstCategories.Size = new System.Drawing.Size(140, 320);
			this.m_lstCategories.TabIndex = 0;
			this.m_lstCategories.SelectedIndexChanged += new System.EventHandler(this.OnCategorySelectedIndexChanged);
			//
			// m_panelPage
			//
			this.m_panelPage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_panelPage.Location = new System.Drawing.Point(140, 0);
			this.m_panelPage.Name = "m_panelPage";
			this.m_panelPage.Padding = new System.Windows.Forms.Padding(10);
			this.m_panelPage.Size = new System.Drawing.Size(340, 320);
			this.m_panelPage.TabIndex = 1;
			//
			// m_panelBottom
			//
			this.m_panelBottom.Controls.Add(this.m_btnOK);
			this.m_panelBottom.Controls.Add(this.m_btnCancel);
			this.m_panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_panelBottom.Location = new System.Drawing.Point(0, 320);
			this.m_panelBottom.Name = "m_panelBottom";
			this.m_panelBottom.Size = new System.Drawing.Size(480, 44);
			this.m_panelBottom.TabIndex = 2;
			//
			// m_btnOK
			//
			this.m_btnOK.Enabled = false;
			this.m_btnOK.Location = new System.Drawing.Point(312, 10);
			this.m_btnOK.Name = "m_btnOK";
			this.m_btnOK.Size = new System.Drawing.Size(75, 23);
			this.m_btnOK.TabIndex = 0;
			this.m_btnOK.Text = "OK";
			this.m_btnOK.UseVisualStyleBackColor = true;
			this.m_btnOK.Click += new System.EventHandler(this.OnOKClick);
			//
			// m_btnCancel
			//
			this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_btnCancel.Location = new System.Drawing.Point(393, 10);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
			this.m_btnCancel.TabIndex = 1;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.UseVisualStyleBackColor = true;
			//
			// OptionsForm
			//
			this.AcceptButton = this.m_btnOK;
			this.CancelButton = this.m_btnCancel;
			this.ClientSize = new System.Drawing.Size(480, 364);
			this.Controls.Add(this.m_panelPage);
			this.Controls.Add(this.m_lstCategories);
			this.Controls.Add(this.m_panelBottom);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.m_panelBottom.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox m_lstCategories;
		private System.Windows.Forms.Panel m_panelPage;
		private System.Windows.Forms.Panel m_panelBottom;
		private System.Windows.Forms.Button m_btnOK;
		private System.Windows.Forms.Button m_btnCancel;
	}
}
