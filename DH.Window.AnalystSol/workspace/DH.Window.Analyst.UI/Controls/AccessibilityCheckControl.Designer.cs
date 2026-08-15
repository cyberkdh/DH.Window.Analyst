//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class AccessibilityCheckControl {
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
			this.m_lblSummary = new System.Windows.Forms.Label();
			this.m_btnRunCheck = new System.Windows.Forms.Button();
			this.m_lsvIssues = new System.Windows.Forms.ListView();
			this.m_colSeverity = new System.Windows.Forms.ColumnHeader();
			this.m_colRule = new System.Windows.Forms.ColumnHeader();
			this.m_colElementName = new System.Windows.Forms.ColumnHeader();
			this.m_colControlType = new System.Windows.Forms.ColumnHeader();
			this.m_colAutomationId = new System.Windows.Forms.ColumnHeader();
			this.m_panelTop.SuspendLayout();
			this.SuspendLayout();
			//
			// m_panelTop
			//
			this.m_panelTop.Controls.Add(this.m_lblSummary);
			this.m_panelTop.Controls.Add(this.m_btnRunCheck);
			this.m_panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelTop.Location = new System.Drawing.Point(0, 0);
			this.m_panelTop.Name = "m_panelTop";
			this.m_panelTop.Size = new System.Drawing.Size(700, 36);
			this.m_panelTop.TabIndex = 0;
			//
			// m_btnRunCheck
			//
			this.m_btnRunCheck.Location = new System.Drawing.Point(8, 6);
			this.m_btnRunCheck.Name = "m_btnRunCheck";
			this.m_btnRunCheck.Size = new System.Drawing.Size(100, 23);
			this.m_btnRunCheck.TabIndex = 0;
			this.m_btnRunCheck.Text = "Run Check";
			this.m_btnRunCheck.UseVisualStyleBackColor = true;
			//
			// m_lblSummary
			//
			this.m_lblSummary.AutoSize = true;
			this.m_lblSummary.Location = new System.Drawing.Point(120, 11);
			this.m_lblSummary.Name = "m_lblSummary";
			this.m_lblSummary.Size = new System.Drawing.Size(103, 13);
			this.m_lblSummary.TabIndex = 1;
			this.m_lblSummary.Text = "(Not checked yet)";
			//
			// m_lsvIssues
			//
			this.m_lsvIssues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colSeverity,
			this.m_colRule,
			this.m_colElementName,
			this.m_colControlType,
			this.m_colAutomationId});
			this.m_lsvIssues.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvIssues.FullRowSelect = true;
			this.m_lsvIssues.Location = new System.Drawing.Point(0, 36);
			this.m_lsvIssues.Name = "m_lsvIssues";
			this.m_lsvIssues.Size = new System.Drawing.Size(700, 464);
			this.m_lsvIssues.TabIndex = 1;
			this.m_lsvIssues.UseCompatibleStateImageBehavior = false;
			this.m_lsvIssues.View = System.Windows.Forms.View.Details;
			//
			// m_colSeverity
			//
			this.m_colSeverity.Text = "Severity";
			this.m_colSeverity.Width = 70;
			//
			// m_colRule
			//
			this.m_colRule.Text = "Rule";
			this.m_colRule.Width = 160;
			//
			// m_colElementName
			//
			this.m_colElementName.Text = "Element Name";
			this.m_colElementName.Width = 180;
			//
			// m_colControlType
			//
			this.m_colControlType.Text = "ControlType";
			this.m_colControlType.Width = 110;
			//
			// m_colAutomationId
			//
			this.m_colAutomationId.Text = "AutomationId";
			this.m_colAutomationId.Width = 150;
			//
			// AccessibilityCheckControl
			//
			this.Controls.Add(this.m_lsvIssues);
			this.Controls.Add(this.m_panelTop);
			this.Name = "AccessibilityCheckControl";
			this.Size = new System.Drawing.Size(700, 500);
			this.m_panelTop.ResumeLayout(false);
			this.m_panelTop.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_panelTop;
		private System.Windows.Forms.Button m_btnRunCheck;
		private System.Windows.Forms.Label m_lblSummary;
		private System.Windows.Forms.ListView m_lsvIssues;
		private System.Windows.Forms.ColumnHeader m_colSeverity;
		private System.Windows.Forms.ColumnHeader m_colRule;
		private System.Windows.Forms.ColumnHeader m_colElementName;
		private System.Windows.Forms.ColumnHeader m_colControlType;
		private System.Windows.Forms.ColumnHeader m_colAutomationId;
	}
}
