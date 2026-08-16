//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs {
	partial class SnapshotCompareForm {
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
			this.m_btnExportDiff = new System.Windows.Forms.Button();
			this.m_btnRefreshList = new System.Windows.Forms.Button();
			this.m_chkOnlyDifferences = new System.Windows.Forms.CheckBox();
			this.m_cboSnapshotB = new System.Windows.Forms.ComboBox();
			this.m_lblSnapshotB = new System.Windows.Forms.Label();
			this.m_cboSnapshotA = new System.Windows.Forms.ComboBox();
			this.m_lblSnapshotA = new System.Windows.Forms.Label();
			this.m_lblEmpty = new System.Windows.Forms.Label();
			this.m_lsvDiff = new System.Windows.Forms.ListView();
			this.m_colProperty = new System.Windows.Forms.ColumnHeader();
			this.m_colValueA = new System.Windows.Forms.ColumnHeader();
			this.m_colValueB = new System.Windows.Forms.ColumnHeader();
			this.m_panelTop.SuspendLayout();
			this.SuspendLayout();
			//
			// m_panelTop
			//
			this.m_panelTop.Controls.Add(this.m_btnExportDiff);
			this.m_panelTop.Controls.Add(this.m_btnRefreshList);
			this.m_panelTop.Controls.Add(this.m_chkOnlyDifferences);
			this.m_panelTop.Controls.Add(this.m_cboSnapshotB);
			this.m_panelTop.Controls.Add(this.m_lblSnapshotB);
			this.m_panelTop.Controls.Add(this.m_cboSnapshotA);
			this.m_panelTop.Controls.Add(this.m_lblSnapshotA);
			this.m_panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelTop.Location = new System.Drawing.Point(0, 0);
			this.m_panelTop.Name = "m_panelTop";
			this.m_panelTop.Size = new System.Drawing.Size(820, 66);
			this.m_panelTop.TabIndex = 0;
			//
			// m_lblSnapshotA
			//
			this.m_lblSnapshotA.AutoSize = true;
			this.m_lblSnapshotA.Location = new System.Drawing.Point(8, 12);
			this.m_lblSnapshotA.Name = "m_lblSnapshotA";
			this.m_lblSnapshotA.Size = new System.Drawing.Size(69, 13);
			this.m_lblSnapshotA.TabIndex = 0;
			this.m_lblSnapshotA.Text = "Snapshot A:";
			this.m_lblSnapshotA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_cboSnapshotA
			//
			this.m_cboSnapshotA.DisplayMember = "DisplayLabel";
			this.m_cboSnapshotA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cboSnapshotA.FormattingEnabled = true;
			this.m_cboSnapshotA.Location = new System.Drawing.Point(83, 8);
			this.m_cboSnapshotA.Name = "m_cboSnapshotA";
			this.m_cboSnapshotA.Size = new System.Drawing.Size(300, 21);
			this.m_cboSnapshotA.TabIndex = 1;
			//
			// m_lblSnapshotB
			//
			this.m_lblSnapshotB.AutoSize = true;
			this.m_lblSnapshotB.Location = new System.Drawing.Point(400, 12);
			this.m_lblSnapshotB.Name = "m_lblSnapshotB";
			this.m_lblSnapshotB.Size = new System.Drawing.Size(69, 13);
			this.m_lblSnapshotB.TabIndex = 2;
			this.m_lblSnapshotB.Text = "Snapshot B:";
			this.m_lblSnapshotB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_cboSnapshotB
			//
			this.m_cboSnapshotB.DisplayMember = "DisplayLabel";
			this.m_cboSnapshotB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_cboSnapshotB.FormattingEnabled = true;
			this.m_cboSnapshotB.Location = new System.Drawing.Point(475, 8);
			this.m_cboSnapshotB.Name = "m_cboSnapshotB";
			this.m_cboSnapshotB.Size = new System.Drawing.Size(300, 21);
			this.m_cboSnapshotB.TabIndex = 3;
			//
			// m_chkOnlyDifferences
			//
			this.m_chkOnlyDifferences.AutoSize = true;
			this.m_chkOnlyDifferences.Location = new System.Drawing.Point(8, 40);
			this.m_chkOnlyDifferences.Name = "m_chkOnlyDifferences";
			this.m_chkOnlyDifferences.Size = new System.Drawing.Size(127, 17);
			this.m_chkOnlyDifferences.TabIndex = 4;
			this.m_chkOnlyDifferences.Text = "Show only differences";
			this.m_chkOnlyDifferences.UseVisualStyleBackColor = true;
			//
			// m_btnRefreshList
			//
			this.m_btnRefreshList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnRefreshList.Location = new System.Drawing.Point(600, 36);
			this.m_btnRefreshList.Name = "m_btnRefreshList";
			this.m_btnRefreshList.Size = new System.Drawing.Size(95, 23);
			this.m_btnRefreshList.TabIndex = 5;
			this.m_btnRefreshList.Text = "Refresh List";
			this.m_btnRefreshList.UseVisualStyleBackColor = true;
			this.m_btnRefreshList.Click += new System.EventHandler(this.OnRefreshListClick);
			//
			// m_btnExportDiff
			//
			this.m_btnExportDiff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnExportDiff.Location = new System.Drawing.Point(701, 36);
			this.m_btnExportDiff.Name = "m_btnExportDiff";
			this.m_btnExportDiff.Size = new System.Drawing.Size(111, 23);
			this.m_btnExportDiff.TabIndex = 6;
			this.m_btnExportDiff.Text = "Export Diff...";
			this.m_btnExportDiff.UseVisualStyleBackColor = true;
			this.m_btnExportDiff.Click += new System.EventHandler(this.OnExportDiffClick);
			//
			// m_lblEmpty
			//
			this.m_lblEmpty.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_lblEmpty.ForeColor = System.Drawing.SystemColors.GrayText;
			this.m_lblEmpty.Location = new System.Drawing.Point(0, 66);
			this.m_lblEmpty.Name = "m_lblEmpty";
			this.m_lblEmpty.Padding = new System.Windows.Forms.Padding(8, 8, 0, 8);
			this.m_lblEmpty.Size = new System.Drawing.Size(820, 32);
			this.m_lblEmpty.TabIndex = 1;
			this.m_lblEmpty.Text = "Take at least 2 snapshots first (Snapshot button on the main toolbar), then Ref" +
    "resh List here.";
			//
			// m_lsvDiff
			//
			this.m_lsvDiff.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colProperty,
			this.m_colValueA,
			this.m_colValueB});
			this.m_lsvDiff.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvDiff.FullRowSelect = true;
			this.m_lsvDiff.Location = new System.Drawing.Point(0, 98);
			this.m_lsvDiff.Name = "m_lsvDiff";
			this.m_lsvDiff.Size = new System.Drawing.Size(820, 402);
			this.m_lsvDiff.TabIndex = 2;
			this.m_lsvDiff.UseCompatibleStateImageBehavior = false;
			this.m_lsvDiff.View = System.Windows.Forms.View.Details;
			//
			// m_colProperty
			//
			this.m_colProperty.Text = "Property";
			this.m_colProperty.Width = 200;
			//
			// m_colValueA
			//
			this.m_colValueA.Text = "Value A";
			this.m_colValueA.Width = 300;
			//
			// m_colValueB
			//
			this.m_colValueB.Text = "Value B";
			this.m_colValueB.Width = 300;
			//
			// SnapshotCompareForm
			//
			this.ClientSize = new System.Drawing.Size(820, 500);
			this.Controls.Add(this.m_lsvDiff);
			this.Controls.Add(this.m_lblEmpty);
			this.Controls.Add(this.m_panelTop);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.MinimumSize = new System.Drawing.Size(600, 350);
			this.Name = "SnapshotCompareForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Compare Snapshots";
			this.m_panelTop.ResumeLayout(false);
			this.m_panelTop.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_panelTop;
		private System.Windows.Forms.Label m_lblSnapshotA;
		private System.Windows.Forms.ComboBox m_cboSnapshotA;
		private System.Windows.Forms.Label m_lblSnapshotB;
		private System.Windows.Forms.ComboBox m_cboSnapshotB;
		private System.Windows.Forms.CheckBox m_chkOnlyDifferences;
		private System.Windows.Forms.Button m_btnRefreshList;
		private System.Windows.Forms.Button m_btnExportDiff;
		private System.Windows.Forms.Label m_lblEmpty;
		private System.Windows.Forms.ListView m_lsvDiff;
		private System.Windows.Forms.ColumnHeader m_colProperty;
		private System.Windows.Forms.ColumnHeader m_colValueA;
		private System.Windows.Forms.ColumnHeader m_colValueB;
	}
}
