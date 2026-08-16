//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class PropertyViewControl {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.m_tabMain = new System.Windows.Forms.TabControl();
			this.m_tabPageBasic = new System.Windows.Forms.TabPage();
			this.m_lsvBasic = new System.Windows.Forms.ListView();
			this.m_colBasicProperty = new System.Windows.Forms.ColumnHeader();
			this.m_colBasicValue = new System.Windows.Forms.ColumnHeader();
			this.m_tabPageExtended = new System.Windows.Forms.TabPage();
			this.m_lsvExtended = new System.Windows.Forms.ListView();
			this.m_colExtendedProperty = new System.Windows.Forms.ColumnHeader();
			this.m_colExtendedValue = new System.Windows.Forms.ColumnHeader();
			this.m_tabPageChildren = new System.Windows.Forms.TabPage();
			this.m_lsvChildSummary = new System.Windows.Forms.ListView();
			this.m_colChildClass = new System.Windows.Forms.ColumnHeader();
			this.m_colChildCount = new System.Windows.Forms.ColumnHeader();
			this.m_tabPageControl = new System.Windows.Forms.TabPage();
			this.m_panelControl = new System.Windows.Forms.Panel();
			this.m_lblControlWarning = new System.Windows.Forms.Label();
			this.m_lblVisibility = new System.Windows.Forms.Label();
			this.m_btnShow = new System.Windows.Forms.Button();
			this.m_btnHide = new System.Windows.Forms.Button();
			this.m_lblEnabled = new System.Windows.Forms.Label();
			this.m_btnEnable = new System.Windows.Forms.Button();
			this.m_btnDisable = new System.Windows.Forms.Button();
			this.m_chkTopmost = new System.Windows.Forms.CheckBox();
			this.m_lblPositionSize = new System.Windows.Forms.Label();
			this.m_lblX = new System.Windows.Forms.Label();
			this.m_txtX = new System.Windows.Forms.TextBox();
			this.m_lblY = new System.Windows.Forms.Label();
			this.m_txtY = new System.Windows.Forms.TextBox();
			this.m_lblWidth = new System.Windows.Forms.Label();
			this.m_txtWidth = new System.Windows.Forms.TextBox();
			this.m_lblHeight = new System.Windows.Forms.Label();
			this.m_txtHeight = new System.Windows.Forms.TextBox();
			this.m_btnApplyBounds = new System.Windows.Forms.Button();
			this.m_btnRefreshBounds = new System.Windows.Forms.Button();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.m_tabMain.SuspendLayout();
			this.m_tabPageBasic.SuspendLayout();
			this.m_tabPageExtended.SuspendLayout();
			this.m_tabPageChildren.SuspendLayout();
			this.m_tabPageControl.SuspendLayout();
			this.m_panelControl.SuspendLayout();
			this.SuspendLayout();
			//
			// m_tabMain
			//
			this.m_tabMain.Controls.Add(this.m_tabPageBasic);
			this.m_tabMain.Controls.Add(this.m_tabPageExtended);
			this.m_tabMain.Controls.Add(this.m_tabPageChildren);
			this.m_tabMain.Controls.Add(this.m_tabPageControl);
			this.m_tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabMain.Location = new System.Drawing.Point(0, 0);
			this.m_tabMain.Name = "m_tabMain";
			this.m_tabMain.SelectedIndex = 0;
			this.m_tabMain.Size = new System.Drawing.Size(320, 600);
			this.m_tabMain.TabIndex = 0;
			//
			// m_tabPageBasic
			//
			this.m_tabPageBasic.Controls.Add(this.m_lsvBasic);
			this.m_tabPageBasic.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageBasic.Name = "m_tabPageBasic";
			this.m_tabPageBasic.Padding = new System.Windows.Forms.Padding(3);
			this.m_tabPageBasic.Size = new System.Drawing.Size(312, 574);
			this.m_tabPageBasic.TabIndex = 0;
			this.m_tabPageBasic.Text = "Basic";
			this.m_tabPageBasic.UseVisualStyleBackColor = true;
			//
			// m_lsvBasic
			//
			this.m_lsvBasic.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colBasicProperty,
			this.m_colBasicValue});
			this.m_lsvBasic.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvBasic.FullRowSelect = true;
			this.m_lsvBasic.Location = new System.Drawing.Point(3, 3);
			this.m_lsvBasic.Name = "m_lsvBasic";
			this.m_lsvBasic.Size = new System.Drawing.Size(306, 568);
			this.m_lsvBasic.TabIndex = 0;
			this.m_lsvBasic.UseCompatibleStateImageBehavior = false;
			this.m_lsvBasic.View = System.Windows.Forms.View.Details;
			//
			// m_colBasicProperty
			//
			this.m_colBasicProperty.Text = "Property";
			this.m_colBasicProperty.Width = 130;
			//
			// m_colBasicValue
			//
			this.m_colBasicValue.Text = "Value";
			this.m_colBasicValue.Width = 160;
			//
			// m_tabPageExtended
			//
			this.m_tabPageExtended.Controls.Add(this.m_lsvExtended);
			this.m_tabPageExtended.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageExtended.Name = "m_tabPageExtended";
			this.m_tabPageExtended.Padding = new System.Windows.Forms.Padding(3);
			this.m_tabPageExtended.Size = new System.Drawing.Size(312, 574);
			this.m_tabPageExtended.TabIndex = 1;
			this.m_tabPageExtended.Text = "Native Details";
			this.m_tabPageExtended.UseVisualStyleBackColor = true;
			//
			// m_lsvExtended
			//
			this.m_lsvExtended.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colExtendedProperty,
			this.m_colExtendedValue});
			this.m_lsvExtended.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvExtended.FullRowSelect = true;
			this.m_lsvExtended.Location = new System.Drawing.Point(3, 3);
			this.m_lsvExtended.Name = "m_lsvExtended";
			this.m_lsvExtended.Size = new System.Drawing.Size(306, 568);
			this.m_lsvExtended.TabIndex = 0;
			this.m_lsvExtended.UseCompatibleStateImageBehavior = false;
			this.m_lsvExtended.View = System.Windows.Forms.View.Details;
			//
			// m_colExtendedProperty
			//
			this.m_colExtendedProperty.Text = "Property";
			this.m_colExtendedProperty.Width = 130;
			//
			// m_colExtendedValue
			//
			this.m_colExtendedValue.Text = "Value";
			this.m_colExtendedValue.Width = 160;
			//
			// m_tabPageChildren
			//
			this.m_tabPageChildren.Controls.Add(this.m_lsvChildSummary);
			this.m_tabPageChildren.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageChildren.Name = "m_tabPageChildren";
			this.m_tabPageChildren.Padding = new System.Windows.Forms.Padding(3);
			this.m_tabPageChildren.Size = new System.Drawing.Size(312, 574);
			this.m_tabPageChildren.TabIndex = 2;
			this.m_tabPageChildren.Text = "Child Windows";
			this.m_tabPageChildren.UseVisualStyleBackColor = true;
			//
			// m_lsvChildSummary
			//
			this.m_lsvChildSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colChildClass,
			this.m_colChildCount});
			this.m_lsvChildSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_lsvChildSummary.FullRowSelect = true;
			this.m_lsvChildSummary.Location = new System.Drawing.Point(3, 3);
			this.m_lsvChildSummary.Name = "m_lsvChildSummary";
			this.m_lsvChildSummary.Size = new System.Drawing.Size(306, 568);
			this.m_lsvChildSummary.TabIndex = 0;
			this.m_lsvChildSummary.UseCompatibleStateImageBehavior = false;
			this.m_lsvChildSummary.View = System.Windows.Forms.View.Details;
			//
			// m_colChildClass
			//
			this.m_colChildClass.Text = "Child Class";
			this.m_colChildClass.Width = 130;
			//
			// m_colChildCount
			//
			this.m_colChildCount.Text = "Count";
			this.m_colChildCount.Width = 160;
			//
			// m_tabPageControl
			//
			this.m_tabPageControl.Controls.Add(this.m_panelControl);
			this.m_tabPageControl.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageControl.Name = "m_tabPageControl";
			this.m_tabPageControl.Padding = new System.Windows.Forms.Padding(3);
			this.m_tabPageControl.Size = new System.Drawing.Size(312, 574);
			this.m_tabPageControl.TabIndex = 3;
			this.m_tabPageControl.Text = "Window Control";
			this.m_tabPageControl.UseVisualStyleBackColor = true;
			//
			// m_panelControl
			//
			this.m_panelControl.Controls.Add(this.m_lblControlWarning);
			this.m_panelControl.Controls.Add(this.m_lblVisibility);
			this.m_panelControl.Controls.Add(this.m_btnShow);
			this.m_panelControl.Controls.Add(this.m_btnHide);
			this.m_panelControl.Controls.Add(this.m_lblEnabled);
			this.m_panelControl.Controls.Add(this.m_btnEnable);
			this.m_panelControl.Controls.Add(this.m_btnDisable);
			this.m_panelControl.Controls.Add(this.m_chkTopmost);
			this.m_panelControl.Controls.Add(this.m_lblPositionSize);
			this.m_panelControl.Controls.Add(this.m_lblX);
			this.m_panelControl.Controls.Add(this.m_txtX);
			this.m_panelControl.Controls.Add(this.m_lblY);
			this.m_panelControl.Controls.Add(this.m_txtY);
			this.m_panelControl.Controls.Add(this.m_lblWidth);
			this.m_panelControl.Controls.Add(this.m_txtWidth);
			this.m_panelControl.Controls.Add(this.m_lblHeight);
			this.m_panelControl.Controls.Add(this.m_txtHeight);
			this.m_panelControl.Controls.Add(this.m_btnApplyBounds);
			this.m_panelControl.Controls.Add(this.m_btnRefreshBounds);
			this.m_panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_panelControl.Location = new System.Drawing.Point(3, 3);
			this.m_panelControl.Name = "m_panelControl";
			this.m_panelControl.Size = new System.Drawing.Size(306, 568);
			this.m_panelControl.TabIndex = 0;
			//
			// m_lblControlWarning
			//
			this.m_lblControlWarning.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_lblControlWarning.ForeColor = System.Drawing.Color.DarkRed;
			this.m_lblControlWarning.Location = new System.Drawing.Point(8, 8);
			this.m_lblControlWarning.Name = "m_lblControlWarning";
			this.m_lblControlWarning.Size = new System.Drawing.Size(290, 36);
			this.m_lblControlWarning.TabIndex = 0;
			this.m_lblControlWarning.Text = "These actions directly change the target window\'s real state (Show/Hide/Enable/Move/etc.).";
			//
			// m_lblVisibility
			//
			this.m_lblVisibility.AutoSize = true;
			this.m_lblVisibility.Location = new System.Drawing.Point(8, 55);
			this.m_lblVisibility.Name = "m_lblVisibility";
			this.m_lblVisibility.Size = new System.Drawing.Size(53, 13);
			this.m_lblVisibility.TabIndex = 1;
			this.m_lblVisibility.Text = "Visibility:";
			//
			// m_btnShow
			//
			this.m_btnShow.BackColor = System.Drawing.SystemColors.Control;
			this.m_btnShow.Location = new System.Drawing.Point(100, 50);
			this.m_btnShow.Name = "m_btnShow";
			this.m_btnShow.Size = new System.Drawing.Size(80, 23);
			this.m_btnShow.TabIndex = 2;
			this.m_btnShow.Text = "Show";
			this.m_btnShow.UseVisualStyleBackColor = true;
			//
			// m_btnHide
			//
			this.m_btnHide.BackColor = System.Drawing.SystemColors.Control;
			this.m_btnHide.Location = new System.Drawing.Point(186, 50);
			this.m_btnHide.Name = "m_btnHide";
			this.m_btnHide.Size = new System.Drawing.Size(80, 23);
			this.m_btnHide.TabIndex = 3;
			this.m_btnHide.Text = "Hide";
			this.m_btnHide.UseVisualStyleBackColor = true;
			//
			// m_lblEnabled
			//
			this.m_lblEnabled.AutoSize = true;
			this.m_lblEnabled.Location = new System.Drawing.Point(8, 85);
			this.m_lblEnabled.Name = "m_lblEnabled";
			this.m_lblEnabled.Size = new System.Drawing.Size(50, 13);
			this.m_lblEnabled.TabIndex = 4;
			this.m_lblEnabled.Text = "Enabled:";
			//
			// m_btnEnable
			//
			this.m_btnEnable.BackColor = System.Drawing.SystemColors.Control;
			this.m_btnEnable.Location = new System.Drawing.Point(100, 80);
			this.m_btnEnable.Name = "m_btnEnable";
			this.m_btnEnable.Size = new System.Drawing.Size(80, 23);
			this.m_btnEnable.TabIndex = 5;
			this.m_btnEnable.Text = "Enable";
			this.m_btnEnable.UseVisualStyleBackColor = true;
			//
			// m_btnDisable
			//
			this.m_btnDisable.BackColor = System.Drawing.SystemColors.Control;
			this.m_btnDisable.Location = new System.Drawing.Point(186, 80);
			this.m_btnDisable.Name = "m_btnDisable";
			this.m_btnDisable.Size = new System.Drawing.Size(80, 23);
			this.m_btnDisable.TabIndex = 6;
			this.m_btnDisable.Text = "Disable";
			this.m_btnDisable.UseVisualStyleBackColor = true;
			//
			// m_chkTopmost
			//
			this.m_chkTopmost.AutoSize = true;
			this.m_chkTopmost.Location = new System.Drawing.Point(8, 115);
			this.m_chkTopmost.Name = "m_chkTopmost";
			this.m_chkTopmost.Size = new System.Drawing.Size(96, 17);
			this.m_chkTopmost.TabIndex = 7;
			this.m_chkTopmost.Text = "Always On Top";
			this.m_chkTopmost.UseVisualStyleBackColor = true;
			//
			// m_lblPositionSize
			//
			this.m_lblPositionSize.AutoSize = true;
			this.m_lblPositionSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.m_lblPositionSize.Location = new System.Drawing.Point(8, 150);
			this.m_lblPositionSize.Name = "m_lblPositionSize";
			this.m_lblPositionSize.Size = new System.Drawing.Size(89, 15);
			this.m_lblPositionSize.TabIndex = 8;
			this.m_lblPositionSize.Text = "Position && Size";
			//
			// m_lblX
			//
			this.m_lblX.AutoSize = true;
			this.m_lblX.Location = new System.Drawing.Point(8, 178);
			this.m_lblX.Name = "m_lblX";
			this.m_lblX.Size = new System.Drawing.Size(17, 13);
			this.m_lblX.TabIndex = 9;
			this.m_lblX.Text = "X:";
			//
			// m_txtX
			//
			this.m_txtX.Location = new System.Drawing.Point(28, 175);
			this.m_txtX.Name = "m_txtX";
			this.m_txtX.Size = new System.Drawing.Size(64, 20);
			this.m_txtX.TabIndex = 10;
			//
			// m_lblY
			//
			this.m_lblY.AutoSize = true;
			this.m_lblY.Location = new System.Drawing.Point(104, 178);
			this.m_lblY.Name = "m_lblY";
			this.m_lblY.Size = new System.Drawing.Size(17, 13);
			this.m_lblY.TabIndex = 11;
			this.m_lblY.Text = "Y:";
			//
			// m_txtY
			//
			this.m_txtY.Location = new System.Drawing.Point(124, 175);
			this.m_txtY.Name = "m_txtY";
			this.m_txtY.Size = new System.Drawing.Size(64, 20);
			this.m_txtY.TabIndex = 12;
			//
			// m_lblWidth
			//
			this.m_lblWidth.AutoSize = true;
			this.m_lblWidth.Location = new System.Drawing.Point(8, 208);
			this.m_lblWidth.Name = "m_lblWidth";
			this.m_lblWidth.Size = new System.Drawing.Size(20, 13);
			this.m_lblWidth.TabIndex = 13;
			this.m_lblWidth.Text = "W:";
			//
			// m_txtWidth
			//
			this.m_txtWidth.Location = new System.Drawing.Point(28, 205);
			this.m_txtWidth.Name = "m_txtWidth";
			this.m_txtWidth.Size = new System.Drawing.Size(64, 20);
			this.m_txtWidth.TabIndex = 14;
			//
			// m_lblHeight
			//
			this.m_lblHeight.AutoSize = true;
			this.m_lblHeight.Location = new System.Drawing.Point(104, 208);
			this.m_lblHeight.Name = "m_lblHeight";
			this.m_lblHeight.Size = new System.Drawing.Size(18, 13);
			this.m_lblHeight.TabIndex = 15;
			this.m_lblHeight.Text = "H:";
			//
			// m_txtHeight
			//
			this.m_txtHeight.Location = new System.Drawing.Point(124, 205);
			this.m_txtHeight.Name = "m_txtHeight";
			this.m_txtHeight.Size = new System.Drawing.Size(64, 20);
			this.m_txtHeight.TabIndex = 16;
			//
			// m_btnApplyBounds
			//
			this.m_btnApplyBounds.BackColor = System.Drawing.SystemColors.Control;
			this.m_btnApplyBounds.Location = new System.Drawing.Point(8, 235);
			this.m_btnApplyBounds.Name = "m_btnApplyBounds";
			this.m_btnApplyBounds.Size = new System.Drawing.Size(90, 23);
			this.m_btnApplyBounds.TabIndex = 17;
			this.m_btnApplyBounds.Text = "Apply";
			this.m_btnApplyBounds.UseVisualStyleBackColor = true;
			//
			// m_btnRefreshBounds
			//
			this.m_btnRefreshBounds.BackColor = System.Drawing.SystemColors.Control;
			this.m_btnRefreshBounds.Location = new System.Drawing.Point(104, 235);
			this.m_btnRefreshBounds.Name = "m_btnRefreshBounds";
			this.m_btnRefreshBounds.Size = new System.Drawing.Size(120, 23);
			this.m_btnRefreshBounds.TabIndex = 18;
			this.m_btnRefreshBounds.Text = "Refresh Current";
			this.m_btnRefreshBounds.UseVisualStyleBackColor = true;
			//
			// PropertyViewControl
			//
			this.Controls.Add(this.m_tabMain);
			this.Name = "PropertyViewControl";
			this.Size = new System.Drawing.Size(320, 600);
			this.m_tabMain.ResumeLayout(false);
			this.m_tabPageBasic.ResumeLayout(false);
			this.m_tabPageExtended.ResumeLayout(false);
			this.m_tabPageChildren.ResumeLayout(false);
			this.m_tabPageControl.ResumeLayout(false);
			this.m_panelControl.ResumeLayout(false);
			this.m_panelControl.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TabControl m_tabMain;
		private System.Windows.Forms.TabPage m_tabPageBasic;
		private System.Windows.Forms.ListView m_lsvBasic;
		private System.Windows.Forms.ColumnHeader m_colBasicProperty;
		private System.Windows.Forms.ColumnHeader m_colBasicValue;
		private System.Windows.Forms.TabPage m_tabPageExtended;
		private System.Windows.Forms.ListView m_lsvExtended;
		private System.Windows.Forms.ColumnHeader m_colExtendedProperty;
		private System.Windows.Forms.ColumnHeader m_colExtendedValue;
		private System.Windows.Forms.TabPage m_tabPageChildren;
		private System.Windows.Forms.ListView m_lsvChildSummary;
		private System.Windows.Forms.ColumnHeader m_colChildClass;
		private System.Windows.Forms.ColumnHeader m_colChildCount;
		private System.Windows.Forms.TabPage m_tabPageControl;
		private System.Windows.Forms.Panel m_panelControl;
		private System.Windows.Forms.Label m_lblControlWarning;
		private System.Windows.Forms.Label m_lblVisibility;
		private System.Windows.Forms.Button m_btnShow;
		private System.Windows.Forms.Button m_btnHide;
		private System.Windows.Forms.Label m_lblEnabled;
		private System.Windows.Forms.Button m_btnEnable;
		private System.Windows.Forms.Button m_btnDisable;
		private System.Windows.Forms.CheckBox m_chkTopmost;
		private System.Windows.Forms.Label m_lblPositionSize;
		private System.Windows.Forms.Label m_lblX;
		private System.Windows.Forms.TextBox m_txtX;
		private System.Windows.Forms.Label m_lblY;
		private System.Windows.Forms.TextBox m_txtY;
		private System.Windows.Forms.Label m_lblWidth;
		private System.Windows.Forms.TextBox m_txtWidth;
		private System.Windows.Forms.Label m_lblHeight;
		private System.Windows.Forms.TextBox m_txtHeight;
		private System.Windows.Forms.Button m_btnApplyBounds;
		private System.Windows.Forms.Button m_btnRefreshBounds;
		private System.Windows.Forms.ToolTip m_toolTip;
	}
}
