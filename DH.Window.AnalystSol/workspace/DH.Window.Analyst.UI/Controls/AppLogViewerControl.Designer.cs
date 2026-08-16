//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class AppLogViewerControl {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.m_lsvLog = new System.Windows.Forms.ListView();
			this.m_colLogTime = new System.Windows.Forms.ColumnHeader();
			this.m_colLogLevel = new System.Windows.Forms.ColumnHeader();
			this.m_colLogMessage = new System.Windows.Forms.ColumnHeader();
			this.m_panelTop = new System.Windows.Forms.Panel();
			this.m_btnCopy = new System.Windows.Forms.Button();
			this.m_btnExport = new System.Windows.Forms.Button();
			this.m_btnClear = new System.Windows.Forms.Button();
			this.m_chkShowDebug = new System.Windows.Forms.CheckBox();
			this.m_lblTitle = new System.Windows.Forms.Label();
			this.m_panelTop.SuspendLayout();
			this.SuspendLayout();
			//
			// m_lsvLog
			//
			this.m_lsvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colLogTime,
			this.m_colLogLevel,
			this.m_colLogMessage});
			this.m_lsvLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_lsvLog.FullRowSelect = true;
			this.m_lsvLog.HideSelection = false;
			this.m_lsvLog.Location = new System.Drawing.Point(0, 28);
			this.m_lsvLog.Name = "m_lsvLog";
			this.m_lsvLog.Size = new System.Drawing.Size(800, 122);
			this.m_lsvLog.TabIndex = 1;
			this.m_lsvLog.UseCompatibleStateImageBehavior = false;
			this.m_lsvLog.View = System.Windows.Forms.View.Details;
			//
			// m_colLogTime
			//
			this.m_colLogTime.Text = "Time";
			this.m_colLogTime.Width = 90;
			//
			// m_colLogLevel
			//
			this.m_colLogLevel.Text = "Level";
			this.m_colLogLevel.Width = 60;
			//
			// m_colLogMessage
			//
			this.m_colLogMessage.Text = "Message";
			this.m_colLogMessage.Width = 620;
			//
			// m_panelTop
			//
			this.m_panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
			this.m_panelTop.Controls.Add(this.m_chkShowDebug);
			this.m_panelTop.Controls.Add(this.m_btnClear);
			this.m_panelTop.Controls.Add(this.m_btnExport);
			this.m_panelTop.Controls.Add(this.m_btnCopy);
			this.m_panelTop.Controls.Add(this.m_lblTitle);
			this.m_panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelTop.Location = new System.Drawing.Point(0, 0);
			this.m_panelTop.Name = "m_panelTop";
			this.m_panelTop.Size = new System.Drawing.Size(800, 28);
			this.m_panelTop.TabIndex = 0;
			//
			// m_lblTitle
			//
			this.m_lblTitle.AutoSize = true;
			this.m_lblTitle.Location = new System.Drawing.Point(8, 7);
			this.m_lblTitle.Name = "m_lblTitle";
			this.m_lblTitle.Size = new System.Drawing.Size(24, 13);
			this.m_lblTitle.TabIndex = 0;
			this.m_lblTitle.Text = "Log";
			this.m_lblTitle.Font = new System.Drawing.Font(this.m_lblTitle.Font, System.Drawing.FontStyle.Bold);
			//
			// m_chkShowDebug
			//
			this.m_chkShowDebug.AutoSize = true;
			this.m_chkShowDebug.Location = new System.Drawing.Point(80, 6);
			this.m_chkShowDebug.Name = "m_chkShowDebug";
			this.m_chkShowDebug.Size = new System.Drawing.Size(88, 17);
			this.m_chkShowDebug.TabIndex = 1;
			this.m_chkShowDebug.Text = "Show Debug";
			this.m_chkShowDebug.UseVisualStyleBackColor = true;
			//
			// m_btnCopy
			//
			this.m_btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnCopy.Location = new System.Drawing.Point(542, 2);
			this.m_btnCopy.Name = "m_btnCopy";
			this.m_btnCopy.Size = new System.Drawing.Size(75, 23);
			this.m_btnCopy.TabIndex = 2;
			this.m_btnCopy.Text = "Copy";
			this.m_btnCopy.UseVisualStyleBackColor = true;
			//
			// m_btnExport
			//
			this.m_btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnExport.Location = new System.Drawing.Point(623, 2);
			this.m_btnExport.Name = "m_btnExport";
			this.m_btnExport.Size = new System.Drawing.Size(75, 23);
			this.m_btnExport.TabIndex = 3;
			this.m_btnExport.Text = "Export...";
			this.m_btnExport.UseVisualStyleBackColor = true;
			//
			// m_btnClear
			//
			this.m_btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnClear.Location = new System.Drawing.Point(704, 2);
			this.m_btnClear.Name = "m_btnClear";
			this.m_btnClear.Size = new System.Drawing.Size(75, 23);
			this.m_btnClear.TabIndex = 4;
			this.m_btnClear.Text = "Clear";
			this.m_btnClear.UseVisualStyleBackColor = true;
			//
			// AppLogViewerControl
			//
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.Controls.Add(this.m_lsvLog);
			this.Controls.Add(this.m_panelTop);
			this.Name = "AppLogViewerControl";
			this.Size = new System.Drawing.Size(800, 150);
			this.m_panelTop.ResumeLayout(false);
			this.m_panelTop.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.ListView m_lsvLog;
		private System.Windows.Forms.ColumnHeader m_colLogTime;
		private System.Windows.Forms.ColumnHeader m_colLogLevel;
		private System.Windows.Forms.ColumnHeader m_colLogMessage;
		private System.Windows.Forms.Panel m_panelTop;
		private System.Windows.Forms.Label m_lblTitle;
		private System.Windows.Forms.CheckBox m_chkShowDebug;
		private System.Windows.Forms.Button m_btnCopy;
		private System.Windows.Forms.Button m_btnExport;
		private System.Windows.Forms.Button m_btnClear;
	}
}
