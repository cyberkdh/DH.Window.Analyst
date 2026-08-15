//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Dialogs.Options {
	partial class InspectorOptionPage {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent() {
			this.m_lblHighlightHoldMs = new System.Windows.Forms.Label();
			this.m_numHighlightHoldMs = new System.Windows.Forms.NumericUpDown();
			this.m_lblHighlightFadeMs = new System.Windows.Forms.Label();
			this.m_numHighlightFadeMs = new System.Windows.Forms.NumericUpDown();
			this.m_lblHighlightColor = new System.Windows.Forms.Label();
			this.m_panelHighlightColor = new System.Windows.Forms.Panel();
			this.m_btnHighlightColor = new System.Windows.Forms.Button();
			this.m_lblAccessibilityMaxElements = new System.Windows.Forms.Label();
			this.m_numAccessibilityMaxElements = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize) (this.m_numHighlightHoldMs)).BeginInit();
			((System.ComponentModel.ISupportInitialize) (this.m_numHighlightFadeMs)).BeginInit();
			((System.ComponentModel.ISupportInitialize) (this.m_numAccessibilityMaxElements)).BeginInit();
			this.SuspendLayout();
			//
			// m_lblHighlightHoldMs
			//
			this.m_lblHighlightHoldMs.AutoSize = true;
			this.m_lblHighlightHoldMs.Location = new System.Drawing.Point(3, 5);
			this.m_lblHighlightHoldMs.Name = "m_lblHighlightHoldMs";
			this.m_lblHighlightHoldMs.Size = new System.Drawing.Size(130, 13);
			this.m_lblHighlightHoldMs.TabIndex = 0;
			this.m_lblHighlightHoldMs.Text = "Highlight Hold Time (ms):";
			//
			// m_numHighlightHoldMs
			//
			this.m_numHighlightHoldMs.Location = new System.Drawing.Point(160, 3);
			this.m_numHighlightHoldMs.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			this.m_numHighlightHoldMs.Name = "m_numHighlightHoldMs";
			this.m_numHighlightHoldMs.Size = new System.Drawing.Size(70, 20);
			this.m_numHighlightHoldMs.TabIndex = 1;
			this.m_numHighlightHoldMs.ValueChanged += new System.EventHandler(this.OnControlChanged);
			//
			// m_lblHighlightFadeMs
			//
			this.m_lblHighlightFadeMs.AutoSize = true;
			this.m_lblHighlightFadeMs.Location = new System.Drawing.Point(3, 32);
			this.m_lblHighlightFadeMs.Name = "m_lblHighlightFadeMs";
			this.m_lblHighlightFadeMs.Size = new System.Drawing.Size(128, 13);
			this.m_lblHighlightFadeMs.TabIndex = 2;
			this.m_lblHighlightFadeMs.Text = "Highlight Fade Time (ms):";
			//
			// m_numHighlightFadeMs
			//
			this.m_numHighlightFadeMs.Location = new System.Drawing.Point(160, 30);
			this.m_numHighlightFadeMs.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
			this.m_numHighlightFadeMs.Name = "m_numHighlightFadeMs";
			this.m_numHighlightFadeMs.Size = new System.Drawing.Size(70, 20);
			this.m_numHighlightFadeMs.TabIndex = 3;
			this.m_numHighlightFadeMs.ValueChanged += new System.EventHandler(this.OnControlChanged);
			//
			// m_lblHighlightColor
			//
			this.m_lblHighlightColor.AutoSize = true;
			this.m_lblHighlightColor.Location = new System.Drawing.Point(3, 62);
			this.m_lblHighlightColor.Name = "m_lblHighlightColor";
			this.m_lblHighlightColor.Size = new System.Drawing.Size(83, 13);
			this.m_lblHighlightColor.TabIndex = 4;
			this.m_lblHighlightColor.Text = "Highlight Color:";
			//
			// m_panelHighlightColor
			//
			this.m_panelHighlightColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_panelHighlightColor.Location = new System.Drawing.Point(160, 59);
			this.m_panelHighlightColor.Name = "m_panelHighlightColor";
			this.m_panelHighlightColor.Size = new System.Drawing.Size(40, 20);
			this.m_panelHighlightColor.TabIndex = 5;
			//
			// m_btnHighlightColor
			//
			this.m_btnHighlightColor.Location = new System.Drawing.Point(206, 58);
			this.m_btnHighlightColor.Name = "m_btnHighlightColor";
			this.m_btnHighlightColor.Size = new System.Drawing.Size(75, 23);
			this.m_btnHighlightColor.TabIndex = 6;
			this.m_btnHighlightColor.Text = "Change...";
			this.m_btnHighlightColor.UseVisualStyleBackColor = true;
			this.m_btnHighlightColor.Click += new System.EventHandler(this.OnHighlightColorClick);
			//
			// m_lblAccessibilityMaxElements
			//
			this.m_lblAccessibilityMaxElements.AutoSize = true;
			this.m_lblAccessibilityMaxElements.Location = new System.Drawing.Point(3, 92);
			this.m_lblAccessibilityMaxElements.Name = "m_lblAccessibilityMaxElements";
			this.m_lblAccessibilityMaxElements.Size = new System.Drawing.Size(151, 13);
			this.m_lblAccessibilityMaxElements.TabIndex = 7;
			this.m_lblAccessibilityMaxElements.Text = "Accessibility Scan Max Elements:";
			//
			// m_numAccessibilityMaxElements
			//
			this.m_numAccessibilityMaxElements.Increment = new decimal(new int[] { 100, 0, 0, 0 });
			this.m_numAccessibilityMaxElements.Location = new System.Drawing.Point(160, 116);
			this.m_numAccessibilityMaxElements.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
			this.m_numAccessibilityMaxElements.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
			this.m_numAccessibilityMaxElements.Name = "m_numAccessibilityMaxElements";
			this.m_numAccessibilityMaxElements.Size = new System.Drawing.Size(90, 20);
			this.m_numAccessibilityMaxElements.TabIndex = 8;
			this.m_numAccessibilityMaxElements.Value = new decimal(new int[] { 5000, 0, 0, 0 });
			this.m_numAccessibilityMaxElements.ValueChanged += new System.EventHandler(this.OnControlChanged);
			//
			// InspectorOptionPage
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_numAccessibilityMaxElements);
			this.Controls.Add(this.m_lblAccessibilityMaxElements);
			this.Controls.Add(this.m_btnHighlightColor);
			this.Controls.Add(this.m_panelHighlightColor);
			this.Controls.Add(this.m_lblHighlightColor);
			this.Controls.Add(this.m_numHighlightFadeMs);
			this.Controls.Add(this.m_lblHighlightFadeMs);
			this.Controls.Add(this.m_numHighlightHoldMs);
			this.Controls.Add(this.m_lblHighlightHoldMs);
			this.Name = "InspectorOptionPage";
			this.Size = new System.Drawing.Size(320, 300);
			((System.ComponentModel.ISupportInitialize) (this.m_numHighlightHoldMs)).EndInit();
			((System.ComponentModel.ISupportInitialize) (this.m_numHighlightFadeMs)).EndInit();
			((System.ComponentModel.ISupportInitialize) (this.m_numAccessibilityMaxElements)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label m_lblHighlightHoldMs;
		private System.Windows.Forms.NumericUpDown m_numHighlightHoldMs;
		private System.Windows.Forms.Label m_lblHighlightFadeMs;
		private System.Windows.Forms.NumericUpDown m_numHighlightFadeMs;
		private System.Windows.Forms.Label m_lblHighlightColor;
		private System.Windows.Forms.Panel m_panelHighlightColor;
		private System.Windows.Forms.Button m_btnHighlightColor;
		private System.Windows.Forms.Label m_lblAccessibilityMaxElements;
		private System.Windows.Forms.NumericUpDown m_numAccessibilityMaxElements;
	}
}
