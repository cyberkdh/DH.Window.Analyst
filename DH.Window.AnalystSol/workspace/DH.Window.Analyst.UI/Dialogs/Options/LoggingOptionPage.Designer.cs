//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs.Options {
	partial class LoggingOptionPage {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent() {
			this.m_lblLogLevel = new System.Windows.Forms.Label();
			this.m_chkLogDebug = new System.Windows.Forms.CheckBox();
			this.m_chkLogInfo = new System.Windows.Forms.CheckBox();
			this.m_chkLogWarn = new System.Windows.Forms.CheckBox();
			this.m_chkLogError = new System.Windows.Forms.CheckBox();
			this.m_lblRetentionDays = new System.Windows.Forms.Label();
			this.m_numRetentionDays = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize) (this.m_numRetentionDays)).BeginInit();
			this.SuspendLayout();
			//
			// m_lblLogLevel
			//
			this.m_lblLogLevel.AutoSize = true;
			this.m_lblLogLevel.Location = new System.Drawing.Point(3, 3);
			this.m_lblLogLevel.Name = "m_lblLogLevel";
			this.m_lblLogLevel.Size = new System.Drawing.Size(58, 13);
			this.m_lblLogLevel.TabIndex = 0;
			this.m_lblLogLevel.Text = "Log Level:";
			//
			// m_chkLogDebug
			//
			this.m_chkLogDebug.AutoSize = true;
			this.m_chkLogDebug.Location = new System.Drawing.Point(6, 24);
			this.m_chkLogDebug.Name = "m_chkLogDebug";
			this.m_chkLogDebug.Size = new System.Drawing.Size(60, 17);
			this.m_chkLogDebug.TabIndex = 1;
			this.m_chkLogDebug.Text = "Debug";
			this.m_chkLogDebug.UseVisualStyleBackColor = true;
			this.m_chkLogDebug.CheckedChanged += new System.EventHandler(this.OnControlChanged);
			//
			// m_chkLogInfo
			//
			this.m_chkLogInfo.AutoSize = true;
			this.m_chkLogInfo.Location = new System.Drawing.Point(6, 47);
			this.m_chkLogInfo.Name = "m_chkLogInfo";
			this.m_chkLogInfo.Size = new System.Drawing.Size(46, 17);
			this.m_chkLogInfo.TabIndex = 2;
			this.m_chkLogInfo.Text = "Info";
			this.m_chkLogInfo.UseVisualStyleBackColor = true;
			this.m_chkLogInfo.CheckedChanged += new System.EventHandler(this.OnControlChanged);
			//
			// m_chkLogWarn
			//
			this.m_chkLogWarn.AutoSize = true;
			this.m_chkLogWarn.Location = new System.Drawing.Point(6, 70);
			this.m_chkLogWarn.Name = "m_chkLogWarn";
			this.m_chkLogWarn.Size = new System.Drawing.Size(56, 17);
			this.m_chkLogWarn.TabIndex = 3;
			this.m_chkLogWarn.Text = "Warn";
			this.m_chkLogWarn.UseVisualStyleBackColor = true;
			this.m_chkLogWarn.CheckedChanged += new System.EventHandler(this.OnControlChanged);
			//
			// m_chkLogError
			//
			this.m_chkLogError.AutoSize = true;
			this.m_chkLogError.Location = new System.Drawing.Point(6, 93);
			this.m_chkLogError.Name = "m_chkLogError";
			this.m_chkLogError.Size = new System.Drawing.Size(54, 17);
			this.m_chkLogError.TabIndex = 4;
			this.m_chkLogError.Text = "Error";
			this.m_chkLogError.UseVisualStyleBackColor = true;
			this.m_chkLogError.CheckedChanged += new System.EventHandler(this.OnControlChanged);
			//
			// m_lblRetentionDays
			//
			this.m_lblRetentionDays.AutoSize = true;
			this.m_lblRetentionDays.Location = new System.Drawing.Point(3, 126);
			this.m_lblRetentionDays.Name = "m_lblRetentionDays";
			this.m_lblRetentionDays.Size = new System.Drawing.Size(115, 13);
			this.m_lblRetentionDays.TabIndex = 5;
			this.m_lblRetentionDays.Text = "Log Retention (days):";
			//
			// m_numRetentionDays
			//
			this.m_numRetentionDays.Location = new System.Drawing.Point(140, 124);
			this.m_numRetentionDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
			this.m_numRetentionDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.m_numRetentionDays.Name = "m_numRetentionDays";
			this.m_numRetentionDays.Size = new System.Drawing.Size(60, 20);
			this.m_numRetentionDays.TabIndex = 6;
			this.m_numRetentionDays.Value = new decimal(new int[] { 7, 0, 0, 0 });
			this.m_numRetentionDays.ValueChanged += new System.EventHandler(this.OnControlChanged);
			//
			// LoggingOptionPage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_numRetentionDays);
			this.Controls.Add(this.m_lblRetentionDays);
			this.Controls.Add(this.m_chkLogError);
			this.Controls.Add(this.m_chkLogWarn);
			this.Controls.Add(this.m_chkLogInfo);
			this.Controls.Add(this.m_chkLogDebug);
			this.Controls.Add(this.m_lblLogLevel);
			this.Name = "LoggingOptionPage";
			this.Size = new System.Drawing.Size(320, 300);
			((System.ComponentModel.ISupportInitialize) (this.m_numRetentionDays)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label m_lblLogLevel;
		private System.Windows.Forms.CheckBox m_chkLogDebug;
		private System.Windows.Forms.CheckBox m_chkLogInfo;
		private System.Windows.Forms.CheckBox m_chkLogWarn;
		private System.Windows.Forms.CheckBox m_chkLogError;
		private System.Windows.Forms.Label m_lblRetentionDays;
		private System.Windows.Forms.NumericUpDown m_numRetentionDays;
	}
}
