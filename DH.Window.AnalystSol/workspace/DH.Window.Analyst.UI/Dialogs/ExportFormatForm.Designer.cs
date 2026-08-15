//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs {
	partial class ExportFormatForm {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_grpFormat = new System.Windows.Forms.GroupBox();
			this.m_rbFormatJson = new System.Windows.Forms.RadioButton();
			this.m_rbFormatCsv = new System.Windows.Forms.RadioButton();
			this.m_btnOk = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_grpFormat.SuspendLayout();
			this.SuspendLayout();
			//
			// m_grpFormat
			//
			this.m_grpFormat.Controls.Add(this.m_rbFormatJson);
			this.m_grpFormat.Controls.Add(this.m_rbFormatCsv);
			this.m_grpFormat.Location = new System.Drawing.Point(12, 12);
			this.m_grpFormat.Name = "m_grpFormat";
			this.m_grpFormat.Size = new System.Drawing.Size(236, 56);
			this.m_grpFormat.TabIndex = 0;
			this.m_grpFormat.TabStop = false;
			this.m_grpFormat.Text = "Format";
			//
			// m_rbFormatCsv
			//
			this.m_rbFormatCsv.AutoSize = true;
			this.m_rbFormatCsv.Checked = true;
			this.m_rbFormatCsv.Location = new System.Drawing.Point(12, 22);
			this.m_rbFormatCsv.Name = "m_rbFormatCsv";
			this.m_rbFormatCsv.Size = new System.Drawing.Size(46, 17);
			this.m_rbFormatCsv.TabIndex = 0;
			this.m_rbFormatCsv.TabStop = true;
			this.m_rbFormatCsv.Text = "CSV";
			this.m_rbFormatCsv.UseVisualStyleBackColor = true;
			//
			// m_rbFormatJson
			//
			this.m_rbFormatJson.AutoSize = true;
			this.m_rbFormatJson.Location = new System.Drawing.Point(120, 22);
			this.m_rbFormatJson.Name = "m_rbFormatJson";
			this.m_rbFormatJson.Size = new System.Drawing.Size(50, 17);
			this.m_rbFormatJson.TabIndex = 1;
			this.m_rbFormatJson.Text = "JSON";
			this.m_rbFormatJson.UseVisualStyleBackColor = true;
			//
			// m_btnOk
			//
			this.m_btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_btnOk.Location = new System.Drawing.Point(92, 84);
			this.m_btnOk.Name = "m_btnOk";
			this.m_btnOk.Size = new System.Drawing.Size(75, 23);
			this.m_btnOk.TabIndex = 1;
			this.m_btnOk.Text = "OK";
			this.m_btnOk.UseVisualStyleBackColor = true;
			//
			// m_btnCancel
			//
			this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_btnCancel.Location = new System.Drawing.Point(173, 84);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
			this.m_btnCancel.TabIndex = 2;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.UseVisualStyleBackColor = true;
			//
			// ExportFormatForm
			//
			this.AcceptButton = this.m_btnOk;
			this.CancelButton = this.m_btnCancel;
			this.ClientSize = new System.Drawing.Size(260, 119);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_btnOk);
			this.Controls.Add(this.m_grpFormat);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportFormatForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Export";
			this.m_grpFormat.ResumeLayout(false);
			this.m_grpFormat.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox m_grpFormat;
		private System.Windows.Forms.RadioButton m_rbFormatJson;
		private System.Windows.Forms.RadioButton m_rbFormatCsv;
		private System.Windows.Forms.Button m_btnOk;
		private System.Windows.Forms.Button m_btnCancel;
	}
}
