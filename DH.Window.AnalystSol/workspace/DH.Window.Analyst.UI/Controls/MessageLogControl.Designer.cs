//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class MessageLogControl {
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
			this.m_splitMessages = new System.Windows.Forms.SplitContainer();
			this.m_clbFilter = new System.Windows.Forms.CheckedListBox();
			this.m_lsvMessages = new System.Windows.Forms.ListView();
			this.m_colTime = new System.Windows.Forms.ColumnHeader();
			this.m_colMessage = new System.Windows.Forms.ColumnHeader();
			this.m_colHandle = new System.Windows.Forms.ColumnHeader();
			this.m_colWParam = new System.Windows.Forms.ColumnHeader();
			this.m_colLParam = new System.Windows.Forms.ColumnHeader();
			this.m_colDetails = new System.Windows.Forms.ColumnHeader();
			this.m_btnLayoutFilter = new System.Windows.Forms.Button();
			this.m_btnSelectAll = new System.Windows.Forms.Button();
			this.m_btnDeselectAll = new System.Windows.Forms.Button();
			this.m_btnDefaultFilter = new System.Windows.Forms.Button();
			this.m_panelFilterOptions = new System.Windows.Forms.Panel();
			this.m_panelTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) (this.m_splitMessages)).BeginInit();
			this.m_splitMessages.Panel1.SuspendLayout();
			this.m_splitMessages.Panel2.SuspendLayout();
			this.m_splitMessages.SuspendLayout();
			this.m_panelFilterOptions.SuspendLayout();
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
			// m_splitMessages
			//
			this.m_splitMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_splitMessages.Location = new System.Drawing.Point(0, 36);
			this.m_splitMessages.Name = "m_splitMessages";
			//
			// m_splitMessages.Panel1
			//
			this.m_splitMessages.Panel1.Controls.Add(this.m_clbFilter);
			this.m_splitMessages.Panel1.Controls.Add(this.m_panelFilterOptions);
			//
			// m_splitMessages.Panel2
			//
			this.m_splitMessages.Panel2.Controls.Add(this.m_lsvMessages);
			this.m_splitMessages.Size = new System.Drawing.Size(700, 464);
			this.m_splitMessages.SplitterDistance = 190;
			this.m_splitMessages.TabIndex = 1;
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
			// m_lsvMessages
			//
			this.m_lsvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colTime,
			this.m_colMessage,
			this.m_colHandle,
			this.m_colWParam,
			this.m_colLParam,
			this.m_colDetails});
			this.m_lsvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvMessages.FullRowSelect = true;
			this.m_lsvMessages.Location = new System.Drawing.Point(0, 0);
			this.m_lsvMessages.Name = "m_lsvMessages";
			this.m_lsvMessages.Size = new System.Drawing.Size(506, 464);
			this.m_lsvMessages.TabIndex = 0;
			this.m_lsvMessages.UseCompatibleStateImageBehavior = false;
			this.m_lsvMessages.View = System.Windows.Forms.View.Details;
			//
			// m_colTime
			//
			this.m_colTime.Text = "Time";
			this.m_colTime.Width = 90;
			//
			// m_colMessage
			//
			this.m_colMessage.Text = "Message";
			this.m_colMessage.Width = 190;
			//
			// m_colHandle
			//
			this.m_colHandle.Text = "HWND";
			this.m_colHandle.Width = 100;
			//
			// m_colWParam
			//
			this.m_colWParam.Text = "WParam";
			this.m_colWParam.Width = 110;
			//
			// m_colLParam
			//
			this.m_colLParam.Text = "LParam";
			this.m_colLParam.Width = 110;
			//
			// m_colDetails
			//
			this.m_colDetails.Text = "Details";
			this.m_colDetails.Width = 220;
			//
			// m_btnSelectAll
			//
			this.m_btnSelectAll.Location = new System.Drawing.Point(3, 3);
			this.m_btnSelectAll.Name = "m_btnSelectAll";
			this.m_btnSelectAll.Size = new System.Drawing.Size(180, 23);
			this.m_btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
			this.m_btnSelectAll.TabIndex = 0;
			this.m_btnSelectAll.Text = "Select All";
			this.m_btnSelectAll.UseVisualStyleBackColor = true;
			//
			// m_btnDeselectAll
			//
			this.m_btnDeselectAll.Location = new System.Drawing.Point(3, 29);
			this.m_btnDeselectAll.Name = "m_btnDeselectAll";
			this.m_btnDeselectAll.Size = new System.Drawing.Size(180, 23);
			this.m_btnDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
			this.m_btnDeselectAll.TabIndex = 1;
			this.m_btnDeselectAll.Text = "Deselect All";
			this.m_btnDeselectAll.UseVisualStyleBackColor = true;
			//
			// m_btnDefaultFilter
			//
			this.m_btnDefaultFilter.Location = new System.Drawing.Point(3, 55);
			this.m_btnDefaultFilter.Name = "m_btnDefaultFilter";
			this.m_btnDefaultFilter.Size = new System.Drawing.Size(180, 23);
			this.m_btnDefaultFilter.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
			this.m_btnDefaultFilter.TabIndex = 2;
			this.m_btnDefaultFilter.Text = "Default";
			this.m_btnDefaultFilter.UseVisualStyleBackColor = true;
			//
			// m_btnLayoutFilter
			//
			this.m_btnLayoutFilter.Location = new System.Drawing.Point(3, 81);
			this.m_btnLayoutFilter.Name = "m_btnLayoutFilter";
			this.m_btnLayoutFilter.Size = new System.Drawing.Size(180, 23);
			this.m_btnLayoutFilter.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
			this.m_btnLayoutFilter.TabIndex = 3;
			this.m_btnLayoutFilter.Text = "Layout/DPI Only";
			this.m_btnLayoutFilter.UseVisualStyleBackColor = true;
			//
			// m_panelFilterOptions
			//
			// sits below the filter checklist (same Panel1) rather than next to the message list,
			// since these buttons act on the checklist selection; more presets can be added later without crowding it
			this.m_panelFilterOptions.Controls.Add(this.m_btnLayoutFilter);
			this.m_panelFilterOptions.Controls.Add(this.m_btnDefaultFilter);
			this.m_panelFilterOptions.Controls.Add(this.m_btnDeselectAll);
			this.m_panelFilterOptions.Controls.Add(this.m_btnSelectAll);
			this.m_panelFilterOptions.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_panelFilterOptions.Location = new System.Drawing.Point(0, 357);
			this.m_panelFilterOptions.Name = "m_panelFilterOptions";
			this.m_panelFilterOptions.Size = new System.Drawing.Size(190, 107);
			this.m_panelFilterOptions.TabIndex = 1;
			//
			// MessageLogControl
			//
			this.Controls.Add(this.m_splitMessages);
			this.Controls.Add(this.m_panelTop);
			this.Name = "MessageLogControl";
			this.Size = new System.Drawing.Size(700, 500);
			this.m_panelTop.ResumeLayout(false);
			this.m_panelTop.PerformLayout();
			this.m_splitMessages.Panel1.ResumeLayout(false);
			this.m_splitMessages.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize) (this.m_splitMessages)).EndInit();
			this.m_splitMessages.ResumeLayout(false);
			this.m_panelFilterOptions.ResumeLayout(false);
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
		private System.Windows.Forms.SplitContainer m_splitMessages;
		private System.Windows.Forms.CheckedListBox m_clbFilter;
		private System.Windows.Forms.ListView m_lsvMessages;
		private System.Windows.Forms.ColumnHeader m_colTime;
		private System.Windows.Forms.ColumnHeader m_colMessage;
		private System.Windows.Forms.ColumnHeader m_colHandle;
		private System.Windows.Forms.ColumnHeader m_colWParam;
		private System.Windows.Forms.ColumnHeader m_colLParam;
		private System.Windows.Forms.ColumnHeader m_colDetails;
		private System.Windows.Forms.Button m_btnLayoutFilter;
		private System.Windows.Forms.Panel m_panelFilterOptions;
		private System.Windows.Forms.Button m_btnSelectAll;
		private System.Windows.Forms.Button m_btnDeselectAll;
		private System.Windows.Forms.Button m_btnDefaultFilter;
	}
}
