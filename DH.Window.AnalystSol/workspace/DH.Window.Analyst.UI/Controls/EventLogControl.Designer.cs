//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class EventLogControl {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			if (disposing) {
				m_monitorService.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_panelTop = new System.Windows.Forms.Panel();
			this.m_lblStatus = new System.Windows.Forms.Label();
			this.m_btnCopy = new System.Windows.Forms.Button();
			this.m_btnExport = new System.Windows.Forms.Button();
			this.m_btnClear = new System.Windows.Forms.Button();
			this.m_btnStop = new System.Windows.Forms.Button();
			this.m_btnStart = new System.Windows.Forms.Button();
			this.m_splitEvents = new System.Windows.Forms.SplitContainer();
			this.m_clbFilter = new System.Windows.Forms.CheckedListBox();
			this.m_lsvEvents = new System.Windows.Forms.ListView();
			this.m_colTime = new System.Windows.Forms.ColumnHeader();
			this.m_colEvent = new System.Windows.Forms.ColumnHeader();
			this.m_colHandle = new System.Windows.Forms.ColumnHeader();
			this.m_colObjectId = new System.Windows.Forms.ColumnHeader();
			this.m_colChildId = new System.Windows.Forms.ColumnHeader();
			this.m_panelTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.m_splitEvents)).BeginInit();
			this.m_splitEvents.Panel1.SuspendLayout();
			this.m_splitEvents.Panel2.SuspendLayout();
			this.m_splitEvents.SuspendLayout();
			this.SuspendLayout();
			//
			// m_panelTop
			//
			this.m_panelTop.Controls.Add(this.m_lblStatus);
			this.m_panelTop.Controls.Add(this.m_btnCopy);
			this.m_panelTop.Controls.Add(this.m_btnExport);
			this.m_panelTop.Controls.Add(this.m_btnClear);
			this.m_panelTop.Controls.Add(this.m_btnStop);
			this.m_panelTop.Controls.Add(this.m_btnStart);
			this.m_panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelTop.Location = new System.Drawing.Point(0, 0);
			this.m_panelTop.Name = "m_panelTop";
			this.m_panelTop.Size = new System.Drawing.Size(700, 36);
			this.m_panelTop.TabIndex = 0;
			//
			// m_btnStart
			//
			this.m_btnStart.Location = new System.Drawing.Point(8, 6);
			this.m_btnStart.Name = "m_btnStart";
			this.m_btnStart.Size = new System.Drawing.Size(75, 23);
			this.m_btnStart.TabIndex = 0;
			this.m_btnStart.Text = "Start";
			this.m_btnStart.UseVisualStyleBackColor = true;
			//
			// m_btnStop
			//
			this.m_btnStop.Location = new System.Drawing.Point(89, 6);
			this.m_btnStop.Name = "m_btnStop";
			this.m_btnStop.Size = new System.Drawing.Size(75, 23);
			this.m_btnStop.TabIndex = 1;
			this.m_btnStop.Text = "Stop";
			this.m_btnStop.UseVisualStyleBackColor = true;
			//
			// m_btnClear
			//
			this.m_btnClear.Location = new System.Drawing.Point(170, 6);
			this.m_btnClear.Name = "m_btnClear";
			this.m_btnClear.Size = new System.Drawing.Size(75, 23);
			this.m_btnClear.TabIndex = 2;
			this.m_btnClear.Text = "Clear";
			this.m_btnClear.UseVisualStyleBackColor = true;
			//
			// m_btnExport
			//
			this.m_btnExport.Location = new System.Drawing.Point(251, 6);
			this.m_btnExport.Name = "m_btnExport";
			this.m_btnExport.Size = new System.Drawing.Size(75, 23);
			this.m_btnExport.TabIndex = 4;
			this.m_btnExport.Text = "Export...";
			this.m_btnExport.UseVisualStyleBackColor = true;
			//
			// m_btnCopy
			//
			this.m_btnCopy.Location = new System.Drawing.Point(332, 6);
			this.m_btnCopy.Name = "m_btnCopy";
			this.m_btnCopy.Size = new System.Drawing.Size(75, 23);
			this.m_btnCopy.TabIndex = 5;
			this.m_btnCopy.Text = "Copy";
			this.m_btnCopy.UseVisualStyleBackColor = true;
			//
			// m_lblStatus
			//
			this.m_lblStatus.AutoSize = true;
			this.m_lblStatus.Location = new System.Drawing.Point(419, 11);
			this.m_lblStatus.Name = "m_lblStatus";
			this.m_lblStatus.Size = new System.Drawing.Size(103, 13);
			this.m_lblStatus.TabIndex = 3;
			this.m_lblStatus.Text = "(No target window)";
			//
			// m_splitEvents
			//
			this.m_splitEvents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_splitEvents.Location = new System.Drawing.Point(0, 36);
			this.m_splitEvents.Name = "m_splitEvents";
			//
			// m_splitEvents.Panel1
			//
			this.m_splitEvents.Panel1.Controls.Add(this.m_clbFilter);
			//
			// m_splitEvents.Panel2
			//
			this.m_splitEvents.Panel2.Controls.Add(this.m_lsvEvents);
			this.m_splitEvents.Size = new System.Drawing.Size(700, 464);
			this.m_splitEvents.SplitterDistance = 190;
			this.m_splitEvents.TabIndex = 1;
			//
			// m_clbFilter
			//
			this.m_clbFilter.CheckOnClick = true;
			this.m_clbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_clbFilter.FormattingEnabled = true;
			this.m_clbFilter.IntegralHeight = false;
			this.m_clbFilter.Location = new System.Drawing.Point(0, 0);
			this.m_clbFilter.Name = "m_clbFilter";
			this.m_clbFilter.Size = new System.Drawing.Size(190, 464);
			this.m_clbFilter.TabIndex = 0;
			//
			// m_lsvEvents
			//
			this.m_lsvEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colTime,
			this.m_colEvent,
			this.m_colHandle,
			this.m_colObjectId,
			this.m_colChildId});
			this.m_lsvEvents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvEvents.FullRowSelect = true;
			this.m_lsvEvents.Location = new System.Drawing.Point(0, 0);
			this.m_lsvEvents.Name = "m_lsvEvents";
			this.m_lsvEvents.Size = new System.Drawing.Size(506, 464);
			this.m_lsvEvents.TabIndex = 0;
			this.m_lsvEvents.UseCompatibleStateImageBehavior = false;
			this.m_lsvEvents.View = System.Windows.Forms.View.Details;
			//
			// m_colTime
			//
			this.m_colTime.Text = "Time";
			this.m_colTime.Width = 90;
			//
			// m_colEvent
			//
			this.m_colEvent.Text = "Event";
			this.m_colEvent.Width = 190;
			//
			// m_colHandle
			//
			this.m_colHandle.Text = "Handle";
			this.m_colHandle.Width = 100;
			//
			// m_colObjectId
			//
			this.m_colObjectId.Text = "ObjectId";
			this.m_colObjectId.Width = 70;
			//
			// m_colChildId
			//
			this.m_colChildId.Text = "ChildId";
			this.m_colChildId.Width = 70;
			//
			// EventLogControl
			//
			this.Controls.Add(this.m_splitEvents);
			this.Controls.Add(this.m_panelTop);
			this.Name = "EventLogControl";
			this.Size = new System.Drawing.Size(700, 500);
			this.m_panelTop.ResumeLayout(false);
			this.m_panelTop.PerformLayout();
			this.m_splitEvents.Panel1.ResumeLayout(false);
			this.m_splitEvents.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize) (this.m_splitEvents)).EndInit();
			this.m_splitEvents.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_panelTop;
		private System.Windows.Forms.Button m_btnStart;
		private System.Windows.Forms.Button m_btnStop;
		private System.Windows.Forms.Button m_btnClear;
		private System.Windows.Forms.Button m_btnCopy;
		private System.Windows.Forms.Button m_btnExport;
		private System.Windows.Forms.Label m_lblStatus;
		private System.Windows.Forms.SplitContainer m_splitEvents;
		private System.Windows.Forms.CheckedListBox m_clbFilter;
		private System.Windows.Forms.ListView m_lsvEvents;
		private System.Windows.Forms.ColumnHeader m_colTime;
		private System.Windows.Forms.ColumnHeader m_colEvent;
		private System.Windows.Forms.ColumnHeader m_colHandle;
		private System.Windows.Forms.ColumnHeader m_colObjectId;
		private System.Windows.Forms.ColumnHeader m_colChildId;
	}
}
