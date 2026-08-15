//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs {
	partial class WindowListExportForm {
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
			this.m_grpScope = new System.Windows.Forms.GroupBox();
			this.m_rbScopeSelected = new System.Windows.Forms.RadioButton();
			this.m_rbScopeAll = new System.Windows.Forms.RadioButton();
			this.m_chkIncludeChildren = new System.Windows.Forms.CheckBox();
			this.m_grpStructure = new System.Windows.Forms.GroupBox();
			this.m_rbStructureTree = new System.Windows.Forms.RadioButton();
			this.m_rbStructureFlat = new System.Windows.Forms.RadioButton();
			this.m_btnOk = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_grpFormat.SuspendLayout();
			this.m_grpScope.SuspendLayout();
			this.m_grpStructure.SuspendLayout();
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
			this.m_rbFormatCsv.CheckedChanged += new System.EventHandler(this.OnFormatChanged);
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
			this.m_rbFormatJson.CheckedChanged += new System.EventHandler(this.OnFormatChanged);
			//
			// m_grpScope
			//
			this.m_grpScope.Controls.Add(this.m_rbScopeSelected);
			this.m_grpScope.Controls.Add(this.m_rbScopeAll);
			this.m_grpScope.Location = new System.Drawing.Point(12, 74);
			this.m_grpScope.Name = "m_grpScope";
			this.m_grpScope.Size = new System.Drawing.Size(236, 56);
			this.m_grpScope.TabIndex = 1;
			this.m_grpScope.TabStop = false;
			this.m_grpScope.Text = "Scope";
			//
			// m_rbScopeAll
			//
			this.m_rbScopeAll.AutoSize = true;
			this.m_rbScopeAll.Checked = true;
			this.m_rbScopeAll.Location = new System.Drawing.Point(12, 22);
			this.m_rbScopeAll.Name = "m_rbScopeAll";
			this.m_rbScopeAll.Size = new System.Drawing.Size(101, 17);
			this.m_rbScopeAll.TabIndex = 0;
			this.m_rbScopeAll.TabStop = true;
			this.m_rbScopeAll.Text = "All windows";
			this.m_rbScopeAll.UseVisualStyleBackColor = true;
			//
			// m_rbScopeSelected
			//
			this.m_rbScopeSelected.AutoSize = true;
			this.m_rbScopeSelected.Location = new System.Drawing.Point(120, 22);
			this.m_rbScopeSelected.Name = "m_rbScopeSelected";
			this.m_rbScopeSelected.Size = new System.Drawing.Size(104, 17);
			this.m_rbScopeSelected.TabIndex = 1;
			this.m_rbScopeSelected.Text = "Selected only";
			this.m_rbScopeSelected.UseVisualStyleBackColor = true;
			//
			// m_chkIncludeChildren
			//
			this.m_chkIncludeChildren.AutoSize = true;
			this.m_chkIncludeChildren.Location = new System.Drawing.Point(12, 136);
			this.m_chkIncludeChildren.Name = "m_chkIncludeChildren";
			this.m_chkIncludeChildren.Size = new System.Drawing.Size(133, 17);
			this.m_chkIncludeChildren.TabIndex = 2;
			this.m_chkIncludeChildren.Text = "Include child windows";
			this.m_chkIncludeChildren.UseVisualStyleBackColor = true;
			//
			// m_grpStructure
			//
			this.m_grpStructure.Controls.Add(this.m_rbStructureTree);
			this.m_grpStructure.Controls.Add(this.m_rbStructureFlat);
			this.m_grpStructure.Location = new System.Drawing.Point(12, 159);
			this.m_grpStructure.Name = "m_grpStructure";
			this.m_grpStructure.Size = new System.Drawing.Size(236, 56);
			this.m_grpStructure.TabIndex = 3;
			this.m_grpStructure.TabStop = false;
			this.m_grpStructure.Text = "Structure (JSON only)";
			//
			// m_rbStructureFlat
			//
			this.m_rbStructureFlat.AutoSize = true;
			this.m_rbStructureFlat.Checked = true;
			this.m_rbStructureFlat.Location = new System.Drawing.Point(12, 22);
			this.m_rbStructureFlat.Name = "m_rbStructureFlat";
			this.m_rbStructureFlat.Size = new System.Drawing.Size(78, 17);
			this.m_rbStructureFlat.TabIndex = 0;
			this.m_rbStructureFlat.TabStop = true;
			this.m_rbStructureFlat.Text = "Flat list";
			this.m_rbStructureFlat.UseVisualStyleBackColor = true;
			//
			// m_rbStructureTree
			//
			this.m_rbStructureTree.AutoSize = true;
			this.m_rbStructureTree.Location = new System.Drawing.Point(120, 22);
			this.m_rbStructureTree.Name = "m_rbStructureTree";
			this.m_rbStructureTree.Size = new System.Drawing.Size(50, 17);
			this.m_rbStructureTree.TabIndex = 1;
			this.m_rbStructureTree.Text = "Tree";
			this.m_rbStructureTree.UseVisualStyleBackColor = true;
			//
			// m_btnOk
			//
			this.m_btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_btnOk.Location = new System.Drawing.Point(92, 224);
			this.m_btnOk.Name = "m_btnOk";
			this.m_btnOk.Size = new System.Drawing.Size(75, 23);
			this.m_btnOk.TabIndex = 4;
			this.m_btnOk.Text = "OK";
			this.m_btnOk.UseVisualStyleBackColor = true;
			//
			// m_btnCancel
			//
			this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_btnCancel.Location = new System.Drawing.Point(173, 224);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
			this.m_btnCancel.TabIndex = 5;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.UseVisualStyleBackColor = true;
			//
			// WindowListExportForm
			//
			this.AcceptButton = this.m_btnOk;
			this.CancelButton = this.m_btnCancel;
			this.ClientSize = new System.Drawing.Size(260, 259);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_btnOk);
			this.Controls.Add(this.m_grpStructure);
			this.Controls.Add(this.m_chkIncludeChildren);
			this.Controls.Add(this.m_grpScope);
			this.Controls.Add(this.m_grpFormat);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WindowListExportForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Export Window List";
			this.m_grpFormat.ResumeLayout(false);
			this.m_grpFormat.PerformLayout();
			this.m_grpScope.ResumeLayout(false);
			this.m_grpScope.PerformLayout();
			this.m_grpStructure.ResumeLayout(false);
			this.m_grpStructure.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox m_grpFormat;
		private System.Windows.Forms.RadioButton m_rbFormatJson;
		private System.Windows.Forms.RadioButton m_rbFormatCsv;
		private System.Windows.Forms.GroupBox m_grpScope;
		private System.Windows.Forms.RadioButton m_rbScopeSelected;
		private System.Windows.Forms.RadioButton m_rbScopeAll;
		private System.Windows.Forms.CheckBox m_chkIncludeChildren;
		private System.Windows.Forms.GroupBox m_grpStructure;
		private System.Windows.Forms.RadioButton m_rbStructureTree;
		private System.Windows.Forms.RadioButton m_rbStructureFlat;
		private System.Windows.Forms.Button m_btnOk;
		private System.Windows.Forms.Button m_btnCancel;
	}
}
