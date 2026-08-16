//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs {
	partial class SystemDiagnosticsForm {
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
			this.m_lblLastUpdated = new System.Windows.Forms.Label();
			this.m_btnRefresh = new System.Windows.Forms.Button();
			this.m_splitDiagnostics = new System.Windows.Forms.SplitContainer();
			this.m_lblProcessRanking = new System.Windows.Forms.Label();
			this.m_lsvProcessRanking = new System.Windows.Forms.ListView();
			this.m_colProcName = new System.Windows.Forms.ColumnHeader();
			this.m_colProcPid = new System.Windows.Forms.ColumnHeader();
			this.m_colProcGdi = new System.Windows.Forms.ColumnHeader();
			this.m_colProcUser = new System.Windows.Forms.ColumnHeader();
			this.m_colProcWorkingSet = new System.Windows.Forms.ColumnHeader();
			this.m_colProcElevated = new System.Windows.Forms.ColumnHeader();
			this.m_lblZOrder = new System.Windows.Forms.Label();
			this.m_lsvZOrder = new System.Windows.Forms.ListView();
			this.m_colZOrderHandle = new System.Windows.Forms.ColumnHeader();
			this.m_colZOrderCaption = new System.Windows.Forms.ColumnHeader();
			this.m_colZOrderClass = new System.Windows.Forms.ColumnHeader();
			this.m_colZOrderProcess = new System.Windows.Forms.ColumnHeader();
			this.m_colZOrderVisible = new System.Windows.Forms.ColumnHeader();
			this.m_panelTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_splitDiagnostics)).BeginInit();
			this.m_splitDiagnostics.Panel1.SuspendLayout();
			this.m_splitDiagnostics.Panel2.SuspendLayout();
			this.m_splitDiagnostics.SuspendLayout();
			this.SuspendLayout();
			//
			// m_panelTop
			//
			this.m_panelTop.Controls.Add(this.m_lblLastUpdated);
			this.m_panelTop.Controls.Add(this.m_btnRefresh);
			this.m_panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelTop.Location = new System.Drawing.Point(0, 0);
			this.m_panelTop.Name = "m_panelTop";
			this.m_panelTop.Size = new System.Drawing.Size(700, 34);
			this.m_panelTop.TabIndex = 0;
			//
			// m_lblLastUpdated
			//
			this.m_lblLastUpdated.AutoSize = true;
			this.m_lblLastUpdated.ForeColor = System.Drawing.SystemColors.GrayText;
			this.m_lblLastUpdated.Location = new System.Drawing.Point(12, 12);
			this.m_lblLastUpdated.Name = "m_lblLastUpdated";
			this.m_lblLastUpdated.Size = new System.Drawing.Size(100, 13);
			this.m_lblLastUpdated.TabIndex = 0;
			this.m_lblLastUpdated.Text = "Last updated: —";
			//
			// m_btnRefresh
			//
			this.m_btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnRefresh.Location = new System.Drawing.Point(613, 5);
			this.m_btnRefresh.Name = "m_btnRefresh";
			this.m_btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.m_btnRefresh.TabIndex = 1;
			this.m_btnRefresh.Text = "Refresh";
			this.m_btnRefresh.UseVisualStyleBackColor = true;
			this.m_btnRefresh.Click += new System.EventHandler(this.OnRefreshClick);
			//
			// m_splitDiagnostics
			//
			this.m_splitDiagnostics.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_splitDiagnostics.Location = new System.Drawing.Point(0, 34);
			this.m_splitDiagnostics.Name = "m_splitDiagnostics";
			this.m_splitDiagnostics.Orientation = System.Windows.Forms.Orientation.Horizontal;
			//
			// m_splitDiagnostics.Panel1
			//
			this.m_splitDiagnostics.Panel1.Controls.Add(this.m_lsvProcessRanking);
			this.m_splitDiagnostics.Panel1.Controls.Add(this.m_lblProcessRanking);
			//
			// m_splitDiagnostics.Panel2
			//
			this.m_splitDiagnostics.Panel2.Controls.Add(this.m_lsvZOrder);
			this.m_splitDiagnostics.Panel2.Controls.Add(this.m_lblZOrder);
			this.m_splitDiagnostics.Size = new System.Drawing.Size(700, 566);
			this.m_splitDiagnostics.SplitterDistance = 260;
			this.m_splitDiagnostics.TabIndex = 1;
			//
			// m_lblProcessRanking
			//
			this.m_lblProcessRanking.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_lblProcessRanking.Location = new System.Drawing.Point(0, 0);
			this.m_lblProcessRanking.Name = "m_lblProcessRanking";
			this.m_lblProcessRanking.Padding = new System.Windows.Forms.Padding(4, 4, 0, 4);
			this.m_lblProcessRanking.Size = new System.Drawing.Size(700, 21);
			this.m_lblProcessRanking.TabIndex = 1;
			this.m_lblProcessRanking.Text = "GDI/USER Object Ranking (by process, all running GUI processes)";
			//
			// m_lsvProcessRanking
			//
			this.m_lsvProcessRanking.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colProcName,
			this.m_colProcPid,
			this.m_colProcGdi,
			this.m_colProcUser,
			this.m_colProcWorkingSet,
			this.m_colProcElevated});
			this.m_lsvProcessRanking.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvProcessRanking.FullRowSelect = true;
			this.m_lsvProcessRanking.Location = new System.Drawing.Point(0, 21);
			this.m_lsvProcessRanking.Name = "m_lsvProcessRanking";
			this.m_lsvProcessRanking.Size = new System.Drawing.Size(700, 239);
			this.m_lsvProcessRanking.TabIndex = 0;
			this.m_lsvProcessRanking.UseCompatibleStateImageBehavior = false;
			this.m_lsvProcessRanking.View = System.Windows.Forms.View.Details;
			//
			// m_colProcName
			//
			this.m_colProcName.Text = "Process";
			this.m_colProcName.Width = 160;
			//
			// m_colProcPid
			//
			this.m_colProcPid.Text = "PID";
			this.m_colProcPid.Width = 70;
			//
			// m_colProcGdi
			//
			this.m_colProcGdi.Text = "GDI Objects";
			this.m_colProcGdi.Width = 90;
			//
			// m_colProcUser
			//
			this.m_colProcUser.Text = "USER Objects";
			this.m_colProcUser.Width = 90;
			//
			// m_colProcWorkingSet
			//
			this.m_colProcWorkingSet.Text = "Working Set";
			this.m_colProcWorkingSet.Width = 100;
			//
			// m_colProcElevated
			//
			this.m_colProcElevated.Text = "Elevated";
			this.m_colProcElevated.Width = 70;
			//
			// m_lblZOrder
			//
			this.m_lblZOrder.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_lblZOrder.Location = new System.Drawing.Point(0, 0);
			this.m_lblZOrder.Name = "m_lblZOrder";
			this.m_lblZOrder.Padding = new System.Windows.Forms.Padding(4, 4, 0, 4);
			this.m_lblZOrder.Size = new System.Drawing.Size(700, 21);
			this.m_lblZOrder.TabIndex = 1;
			this.m_lblZOrder.Text = "Top-Level Window Z-Order (front to back, all desktop windows)";
			//
			// m_lsvZOrder
			//
			this.m_lsvZOrder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colZOrderHandle,
			this.m_colZOrderCaption,
			this.m_colZOrderClass,
			this.m_colZOrderProcess,
			this.m_colZOrderVisible});
			this.m_lsvZOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvZOrder.FullRowSelect = true;
			this.m_lsvZOrder.Location = new System.Drawing.Point(0, 21);
			this.m_lsvZOrder.Name = "m_lsvZOrder";
			this.m_lsvZOrder.Size = new System.Drawing.Size(700, 281);
			this.m_lsvZOrder.TabIndex = 0;
			this.m_lsvZOrder.UseCompatibleStateImageBehavior = false;
			this.m_lsvZOrder.View = System.Windows.Forms.View.Details;
			//
			// m_colZOrderHandle
			//
			this.m_colZOrderHandle.Text = "Handle";
			this.m_colZOrderHandle.Width = 80;
			//
			// m_colZOrderCaption
			//
			this.m_colZOrderCaption.Text = "Caption";
			this.m_colZOrderCaption.Width = 200;
			//
			// m_colZOrderClass
			//
			this.m_colZOrderClass.Text = "Class";
			this.m_colZOrderClass.Width = 140;
			//
			// m_colZOrderProcess
			//
			this.m_colZOrderProcess.Text = "Process";
			this.m_colZOrderProcess.Width = 120;
			//
			// m_colZOrderVisible
			//
			this.m_colZOrderVisible.Text = "Visible";
			this.m_colZOrderVisible.Width = 60;
			//
			// SystemDiagnosticsForm
			//
			this.ClientSize = new System.Drawing.Size(700, 600);
			this.Controls.Add(this.m_splitDiagnostics);
			this.Controls.Add(this.m_panelTop);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.MinimumSize = new System.Drawing.Size(500, 350);
			this.Name = "SystemDiagnosticsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "System Diagnostics — GDI/USER Objects & Z-Order";
			this.m_panelTop.ResumeLayout(false);
			this.m_panelTop.PerformLayout();
			this.m_splitDiagnostics.Panel1.ResumeLayout(false);
			this.m_splitDiagnostics.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_splitDiagnostics)).EndInit();
			this.m_splitDiagnostics.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_panelTop;
		private System.Windows.Forms.Label m_lblLastUpdated;
		private System.Windows.Forms.Button m_btnRefresh;
		private System.Windows.Forms.SplitContainer m_splitDiagnostics;
		private System.Windows.Forms.Label m_lblProcessRanking;
		private System.Windows.Forms.ListView m_lsvProcessRanking;
		private System.Windows.Forms.ColumnHeader m_colProcName;
		private System.Windows.Forms.ColumnHeader m_colProcPid;
		private System.Windows.Forms.ColumnHeader m_colProcGdi;
		private System.Windows.Forms.ColumnHeader m_colProcUser;
		private System.Windows.Forms.ColumnHeader m_colProcWorkingSet;
		private System.Windows.Forms.ColumnHeader m_colProcElevated;
		private System.Windows.Forms.Label m_lblZOrder;
		private System.Windows.Forms.ListView m_lsvZOrder;
		private System.Windows.Forms.ColumnHeader m_colZOrderHandle;
		private System.Windows.Forms.ColumnHeader m_colZOrderCaption;
		private System.Windows.Forms.ColumnHeader m_colZOrderClass;
		private System.Windows.Forms.ColumnHeader m_colZOrderProcess;
		private System.Windows.Forms.ColumnHeader m_colZOrderVisible;
	}
}
