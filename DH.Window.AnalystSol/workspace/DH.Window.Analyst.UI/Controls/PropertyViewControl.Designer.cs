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
			this.m_tabMain.SuspendLayout();
			this.m_tabPageBasic.SuspendLayout();
			this.m_tabPageExtended.SuspendLayout();
			this.m_tabPageChildren.SuspendLayout();
			this.SuspendLayout();
			//
			// m_tabMain
			//
			this.m_tabMain.Controls.Add(this.m_tabPageBasic);
			this.m_tabMain.Controls.Add(this.m_tabPageExtended);
			this.m_tabMain.Controls.Add(this.m_tabPageChildren);
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
			// PropertyViewControl
			//
			this.Controls.Add(this.m_tabMain);
			this.Name = "PropertyViewControl";
			this.Size = new System.Drawing.Size(320, 600);
			this.m_tabMain.ResumeLayout(false);
			this.m_tabPageBasic.ResumeLayout(false);
			this.m_tabPageExtended.ResumeLayout(false);
			this.m_tabPageChildren.ResumeLayout(false);
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
	}
}
