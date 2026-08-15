//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs {
	partial class FindWindowForm {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_lblCaption = new System.Windows.Forms.Label();
			this.m_txtCaption = new System.Windows.Forms.TextBox();
			this.m_lblClassName = new System.Windows.Forms.Label();
			this.m_txtClassName = new System.Windows.Forms.TextBox();
			this.m_lblHandle = new System.Windows.Forms.Label();
			this.m_txtHandle = new System.Windows.Forms.TextBox();
			this.m_lblHint = new System.Windows.Forms.Label();
			this.m_btnFinder = new DH.Window.Analyst.UI.Controls.FinderToolButton();
			this.m_linkAdvanced = new System.Windows.Forms.LinkLabel();
			this.m_pnlAdvanced = new System.Windows.Forms.Panel();
			this.m_lblPid = new System.Windows.Forms.Label();
			this.m_txtPid = new System.Windows.Forms.TextBox();
			this.m_lblProcessName = new System.Windows.Forms.Label();
			this.m_txtProcessName = new System.Windows.Forms.TextBox();
			this.m_lblControlId = new System.Windows.Forms.Label();
			this.m_txtControlId = new System.Windows.Forms.TextBox();
			this.m_lblAutomationId = new System.Windows.Forms.Label();
			this.m_txtAutomationId = new System.Windows.Forms.TextBox();
			this.m_lblUiaName = new System.Windows.Forms.Label();
			this.m_txtUiaName = new System.Windows.Forms.TextBox();
			this.m_lblControlType = new System.Windows.Forms.Label();
			this.m_txtControlType = new System.Windows.Forms.TextBox();
			this.m_progressSearch = new System.Windows.Forms.ProgressBar();
			this.m_btnCancelSearch = new System.Windows.Forms.Button();
			this.m_treeResults = new System.Windows.Forms.TreeView();
			this.m_btnSelectResult = new System.Windows.Forms.Button();
			this.m_btnFind = new System.Windows.Forms.Button();
			this.m_btnCancel = new System.Windows.Forms.Button();
			this.m_pnlAdvanced.SuspendLayout();
			this.SuspendLayout();
			//
			// m_lblCaption
			//
			this.m_lblCaption.Location = new System.Drawing.Point(12, 15);
			this.m_lblCaption.Name = "m_lblCaption";
			this.m_lblCaption.Size = new System.Drawing.Size(70, 23);
			this.m_lblCaption.TabIndex = 0;
			this.m_lblCaption.Text = "Caption:";
			this.m_lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtCaption
			//
			this.m_txtCaption.Location = new System.Drawing.Point(88, 12);
			this.m_txtCaption.Name = "m_txtCaption";
			this.m_txtCaption.Size = new System.Drawing.Size(280, 20);
			this.m_txtCaption.TabIndex = 1;
			this.m_txtCaption.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblClassName
			//
			this.m_lblClassName.Location = new System.Drawing.Point(12, 41);
			this.m_lblClassName.Name = "m_lblClassName";
			this.m_lblClassName.Size = new System.Drawing.Size(70, 23);
			this.m_lblClassName.TabIndex = 2;
			this.m_lblClassName.Text = "Class Name:";
			this.m_lblClassName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtClassName
			//
			this.m_txtClassName.Location = new System.Drawing.Point(88, 38);
			this.m_txtClassName.Name = "m_txtClassName";
			this.m_txtClassName.Size = new System.Drawing.Size(280, 20);
			this.m_txtClassName.TabIndex = 3;
			this.m_txtClassName.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblHandle
			//
			this.m_lblHandle.Location = new System.Drawing.Point(12, 67);
			this.m_lblHandle.Name = "m_lblHandle";
			this.m_lblHandle.Size = new System.Drawing.Size(70, 23);
			this.m_lblHandle.TabIndex = 4;
			this.m_lblHandle.Text = "Handle:";
			this.m_lblHandle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtHandle
			//
			this.m_txtHandle.Location = new System.Drawing.Point(88, 64);
			this.m_txtHandle.Name = "m_txtHandle";
			this.m_txtHandle.Size = new System.Drawing.Size(238, 20);
			this.m_txtHandle.TabIndex = 5;
			this.m_txtHandle.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_btnFinder
			//
			this.m_btnFinder.Location = new System.Drawing.Point(332, 63);
			this.m_btnFinder.Name = "m_btnFinder";
			this.m_btnFinder.Size = new System.Drawing.Size(36, 23);
			this.m_btnFinder.TabIndex = 9;
			this.m_btnFinder.Text = "⌖";
			this.m_btnFinder.UseVisualStyleBackColor = true;
			//
			// m_linkAdvanced
			//
			this.m_linkAdvanced.AutoSize = true;
			this.m_linkAdvanced.Location = new System.Drawing.Point(12, 92);
			this.m_linkAdvanced.Name = "m_linkAdvanced";
			this.m_linkAdvanced.Size = new System.Drawing.Size(100, 13);
			this.m_linkAdvanced.TabIndex = 10;
			this.m_linkAdvanced.TabStop = true;
			this.m_linkAdvanced.Text = "▸ Advanced";
			this.m_linkAdvanced.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnAdvancedToggleClicked);
			//
			// m_pnlAdvanced
			//
			this.m_pnlAdvanced.Controls.Add(this.m_lblPid);
			this.m_pnlAdvanced.Controls.Add(this.m_txtPid);
			this.m_pnlAdvanced.Controls.Add(this.m_lblProcessName);
			this.m_pnlAdvanced.Controls.Add(this.m_txtProcessName);
			this.m_pnlAdvanced.Controls.Add(this.m_lblControlId);
			this.m_pnlAdvanced.Controls.Add(this.m_txtControlId);
			this.m_pnlAdvanced.Controls.Add(this.m_lblAutomationId);
			this.m_pnlAdvanced.Controls.Add(this.m_txtAutomationId);
			this.m_pnlAdvanced.Controls.Add(this.m_lblUiaName);
			this.m_pnlAdvanced.Controls.Add(this.m_txtUiaName);
			this.m_pnlAdvanced.Controls.Add(this.m_lblControlType);
			this.m_pnlAdvanced.Controls.Add(this.m_txtControlType);
			this.m_pnlAdvanced.Location = new System.Drawing.Point(12, 111);
			this.m_pnlAdvanced.Name = "m_pnlAdvanced";
			this.m_pnlAdvanced.Size = new System.Drawing.Size(356, 156);
			this.m_pnlAdvanced.TabIndex = 11;
			this.m_pnlAdvanced.Visible = false;
			//
			// m_lblPid
			//
			this.m_lblPid.Location = new System.Drawing.Point(0, 3);
			this.m_lblPid.Name = "m_lblPid";
			this.m_lblPid.Size = new System.Drawing.Size(90, 17);
			this.m_lblPid.TabIndex = 0;
			this.m_lblPid.Text = "PID:";
			this.m_lblPid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtPid
			//
			this.m_txtPid.Location = new System.Drawing.Point(94, 0);
			this.m_txtPid.Name = "m_txtPid";
			this.m_txtPid.Size = new System.Drawing.Size(262, 20);
			this.m_txtPid.TabIndex = 1;
			this.m_txtPid.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblProcessName
			//
			this.m_lblProcessName.Location = new System.Drawing.Point(0, 29);
			this.m_lblProcessName.Name = "m_lblProcessName";
			this.m_lblProcessName.Size = new System.Drawing.Size(90, 17);
			this.m_lblProcessName.TabIndex = 2;
			this.m_lblProcessName.Text = "Process Name:";
			this.m_lblProcessName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtProcessName
			//
			this.m_txtProcessName.Location = new System.Drawing.Point(94, 26);
			this.m_txtProcessName.Name = "m_txtProcessName";
			this.m_txtProcessName.Size = new System.Drawing.Size(262, 20);
			this.m_txtProcessName.TabIndex = 3;
			this.m_txtProcessName.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblControlId
			//
			this.m_lblControlId.Location = new System.Drawing.Point(0, 55);
			this.m_lblControlId.Name = "m_lblControlId";
			this.m_lblControlId.Size = new System.Drawing.Size(90, 17);
			this.m_lblControlId.TabIndex = 4;
			this.m_lblControlId.Text = "Control ID:";
			this.m_lblControlId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtControlId
			//
			this.m_txtControlId.Location = new System.Drawing.Point(94, 52);
			this.m_txtControlId.Name = "m_txtControlId";
			this.m_txtControlId.Size = new System.Drawing.Size(262, 20);
			this.m_txtControlId.TabIndex = 5;
			this.m_txtControlId.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblAutomationId
			//
			this.m_lblAutomationId.Location = new System.Drawing.Point(0, 81);
			this.m_lblAutomationId.Name = "m_lblAutomationId";
			this.m_lblAutomationId.Size = new System.Drawing.Size(90, 17);
			this.m_lblAutomationId.TabIndex = 6;
			this.m_lblAutomationId.Text = "AutomationId:";
			this.m_lblAutomationId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtAutomationId
			//
			this.m_txtAutomationId.Location = new System.Drawing.Point(94, 78);
			this.m_txtAutomationId.Name = "m_txtAutomationId";
			this.m_txtAutomationId.Size = new System.Drawing.Size(262, 20);
			this.m_txtAutomationId.TabIndex = 7;
			this.m_txtAutomationId.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblUiaName
			//
			this.m_lblUiaName.Location = new System.Drawing.Point(0, 107);
			this.m_lblUiaName.Name = "m_lblUiaName";
			this.m_lblUiaName.Size = new System.Drawing.Size(90, 17);
			this.m_lblUiaName.TabIndex = 8;
			this.m_lblUiaName.Text = "UIA Name:";
			this.m_lblUiaName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtUiaName
			//
			this.m_txtUiaName.Location = new System.Drawing.Point(94, 104);
			this.m_txtUiaName.Name = "m_txtUiaName";
			this.m_txtUiaName.Size = new System.Drawing.Size(262, 20);
			this.m_txtUiaName.TabIndex = 9;
			this.m_txtUiaName.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblControlType
			//
			this.m_lblControlType.Location = new System.Drawing.Point(0, 133);
			this.m_lblControlType.Name = "m_lblControlType";
			this.m_lblControlType.Size = new System.Drawing.Size(90, 17);
			this.m_lblControlType.TabIndex = 10;
			this.m_lblControlType.Text = "Control Type:";
			this.m_lblControlType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// m_txtControlType
			//
			this.m_txtControlType.Location = new System.Drawing.Point(94, 130);
			this.m_txtControlType.Name = "m_txtControlType";
			this.m_txtControlType.Size = new System.Drawing.Size(262, 20);
			this.m_txtControlType.TabIndex = 11;
			this.m_txtControlType.TextChanged += new System.EventHandler(this.OnCriteriaTextChanged);
			//
			// m_lblHint
			//
			this.m_lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
			this.m_lblHint.Location = new System.Drawing.Point(12, 111);
			this.m_lblHint.Name = "m_lblHint";
			this.m_lblHint.Size = new System.Drawing.Size(356, 32);
			this.m_lblHint.TabIndex = 6;
			this.m_lblHint.Text = "Drag the Finder Tool (target icon) onto any window to pick it, or type Caption/" +
	"Class Name to search by substring across every window on the desktop.";
			//
			// m_progressSearch
			//
			this.m_progressSearch.Location = new System.Drawing.Point(12, 146);
			this.m_progressSearch.MarqueeAnimationSpeed = 30;
			this.m_progressSearch.Name = "m_progressSearch";
			this.m_progressSearch.Size = new System.Drawing.Size(266, 15);
			this.m_progressSearch.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.m_progressSearch.TabIndex = 12;
			this.m_progressSearch.Visible = false;
			//
			// m_btnCancelSearch
			//
			this.m_btnCancelSearch.Location = new System.Drawing.Point(284, 143);
			this.m_btnCancelSearch.Name = "m_btnCancelSearch";
			this.m_btnCancelSearch.Size = new System.Drawing.Size(84, 22);
			this.m_btnCancelSearch.TabIndex = 13;
			this.m_btnCancelSearch.Text = "Cancel Search";
			this.m_btnCancelSearch.UseVisualStyleBackColor = true;
			this.m_btnCancelSearch.Visible = false;
			this.m_btnCancelSearch.Click += new System.EventHandler(this.OnCancelSearchClick);
			//
			// m_treeResults
			//
			this.m_treeResults.HideSelection = false;
			this.m_treeResults.Location = new System.Drawing.Point(12, 111);
			this.m_treeResults.Name = "m_treeResults";
			this.m_treeResults.Size = new System.Drawing.Size(356, 150);
			this.m_treeResults.TabIndex = 14;
			this.m_treeResults.Visible = false;
			this.m_treeResults.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnResultAfterSelect);
			this.m_treeResults.DoubleClick += new System.EventHandler(this.OnResultDoubleClick);
			//
			// m_btnSelectResult
			//
			this.m_btnSelectResult.Enabled = false;
			this.m_btnSelectResult.Location = new System.Drawing.Point(212, 128);
			this.m_btnSelectResult.Name = "m_btnSelectResult";
			this.m_btnSelectResult.Size = new System.Drawing.Size(75, 23);
			this.m_btnSelectResult.TabIndex = 15;
			this.m_btnSelectResult.Text = "Select";
			this.m_btnSelectResult.UseVisualStyleBackColor = true;
			this.m_btnSelectResult.Visible = false;
			this.m_btnSelectResult.Click += new System.EventHandler(this.OnSelectResultClick);
			//
			// m_btnFind
			//
			this.m_btnFind.Enabled = false;
			this.m_btnFind.Location = new System.Drawing.Point(212, 128);
			this.m_btnFind.Name = "m_btnFind";
			this.m_btnFind.Size = new System.Drawing.Size(75, 23);
			this.m_btnFind.TabIndex = 7;
			this.m_btnFind.Text = "Find";
			this.m_btnFind.UseVisualStyleBackColor = true;
			this.m_btnFind.Click += new System.EventHandler(this.OnFindClick);
			//
			// m_btnCancel
			//
			this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_btnCancel.Location = new System.Drawing.Point(293, 128);
			this.m_btnCancel.Name = "m_btnCancel";
			this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
			this.m_btnCancel.TabIndex = 8;
			this.m_btnCancel.Text = "Cancel";
			this.m_btnCancel.UseVisualStyleBackColor = true;
			//
			// FindWindowForm
			//
			this.AcceptButton = this.m_btnFind;
			this.CancelButton = this.m_btnCancel;
			this.ClientSize = new System.Drawing.Size(380, 163);
			this.Controls.Add(this.m_btnSelectResult);
			this.Controls.Add(this.m_treeResults);
			this.Controls.Add(this.m_btnCancelSearch);
			this.Controls.Add(this.m_progressSearch);
			this.Controls.Add(this.m_pnlAdvanced);
			this.Controls.Add(this.m_linkAdvanced);
			this.Controls.Add(this.m_btnCancel);
			this.Controls.Add(this.m_btnFind);
			this.Controls.Add(this.m_lblHint);
			this.Controls.Add(this.m_btnFinder);
			this.Controls.Add(this.m_txtHandle);
			this.Controls.Add(this.m_lblHandle);
			this.Controls.Add(this.m_txtClassName);
			this.Controls.Add(this.m_lblClassName);
			this.Controls.Add(this.m_txtCaption);
			this.Controls.Add(this.m_lblCaption);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindWindowForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Find Window";
			this.m_pnlAdvanced.ResumeLayout(false);
			this.m_pnlAdvanced.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label m_lblCaption;
		private System.Windows.Forms.TextBox m_txtCaption;
		private System.Windows.Forms.Label m_lblClassName;
		private System.Windows.Forms.TextBox m_txtClassName;
		private System.Windows.Forms.Label m_lblHandle;
		private System.Windows.Forms.TextBox m_txtHandle;
		private System.Windows.Forms.Label m_lblHint;
		private DH.Window.Analyst.UI.Controls.FinderToolButton m_btnFinder;
		private System.Windows.Forms.LinkLabel m_linkAdvanced;
		private System.Windows.Forms.Panel m_pnlAdvanced;
		private System.Windows.Forms.Label m_lblPid;
		private System.Windows.Forms.TextBox m_txtPid;
		private System.Windows.Forms.Label m_lblProcessName;
		private System.Windows.Forms.TextBox m_txtProcessName;
		private System.Windows.Forms.Label m_lblControlId;
		private System.Windows.Forms.TextBox m_txtControlId;
		private System.Windows.Forms.Label m_lblAutomationId;
		private System.Windows.Forms.TextBox m_txtAutomationId;
		private System.Windows.Forms.Label m_lblUiaName;
		private System.Windows.Forms.TextBox m_txtUiaName;
		private System.Windows.Forms.Label m_lblControlType;
		private System.Windows.Forms.TextBox m_txtControlType;
		private System.Windows.Forms.ProgressBar m_progressSearch;
		private System.Windows.Forms.Button m_btnCancelSearch;
		private System.Windows.Forms.TreeView m_treeResults;
		private System.Windows.Forms.Button m_btnSelectResult;
		private System.Windows.Forms.Button m_btnFind;
		private System.Windows.Forms.Button m_btnCancel;
	}
}
