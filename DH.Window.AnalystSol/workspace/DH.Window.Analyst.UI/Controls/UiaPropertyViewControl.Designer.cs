//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class UiaPropertyViewControl {
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
			this.m_tabPageChildren = new System.Windows.Forms.TabPage();
			this.m_lsvChildSummary = new System.Windows.Forms.ListView();
			this.m_colChildType = new System.Windows.Forms.ColumnHeader();
			this.m_colChildCount = new System.Windows.Forms.ColumnHeader();
			this.m_tabPagePatterns = new System.Windows.Forms.TabPage();
			this.m_panelPatternActions = new System.Windows.Forms.Panel();
			this.m_lblNoAction = new System.Windows.Forms.Label();
			this.m_lblActionStatus = new System.Windows.Forms.Label();
			this.m_btnSetValue = new System.Windows.Forms.Button();
			this.m_txtValue = new System.Windows.Forms.TextBox();
			this.m_lblValue = new System.Windows.Forms.Label();
			this.m_btnSelect = new System.Windows.Forms.Button();
			this.m_btnCollapse = new System.Windows.Forms.Button();
			this.m_btnExpand = new System.Windows.Forms.Button();
			this.m_btnToggle = new System.Windows.Forms.Button();
			this.m_btnInvoke = new System.Windows.Forms.Button();
			this.m_lsvPatterns = new System.Windows.Forms.ListView();
			this.m_colPatternName = new System.Windows.Forms.ColumnHeader();
			this.m_colPatternState = new System.Windows.Forms.ColumnHeader();
			this.m_btnMinimize = new System.Windows.Forms.Button();
			this.m_btnMaximize = new System.Windows.Forms.Button();
			this.m_btnRestore = new System.Windows.Forms.Button();
			this.m_btnClose = new System.Windows.Forms.Button();
			this.m_lblMoveX = new System.Windows.Forms.Label();
			this.m_txtMoveX = new System.Windows.Forms.TextBox();
			this.m_lblMoveY = new System.Windows.Forms.Label();
			this.m_txtMoveY = new System.Windows.Forms.TextBox();
			this.m_btnMove = new System.Windows.Forms.Button();
			this.m_lblResizeW = new System.Windows.Forms.Label();
			this.m_txtResizeW = new System.Windows.Forms.TextBox();
			this.m_lblResizeH = new System.Windows.Forms.Label();
			this.m_txtResizeH = new System.Windows.Forms.TextBox();
			this.m_btnResize = new System.Windows.Forms.Button();
			this.m_lblRotate = new System.Windows.Forms.Label();
			this.m_txtRotate = new System.Windows.Forms.TextBox();
			this.m_btnRotate = new System.Windows.Forms.Button();
			this.m_lblNativeActions = new System.Windows.Forms.Label();
			this.m_btnNativeEnable = new System.Windows.Forms.Button();
			this.m_btnNativeDisable = new System.Windows.Forms.Button();
			this.m_tabMain.SuspendLayout();
			this.m_tabPageBasic.SuspendLayout();
			this.m_tabPageChildren.SuspendLayout();
			this.m_tabPagePatterns.SuspendLayout();
			this.m_panelPatternActions.SuspendLayout();
			this.SuspendLayout();
			//
			// m_tabMain
			//
			this.m_tabMain.Controls.Add(this.m_tabPageBasic);
			this.m_tabMain.Controls.Add(this.m_tabPageChildren);
			this.m_tabMain.Controls.Add(this.m_tabPagePatterns);
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
			this.m_colBasicValue.Width = 200;
			//
			// m_tabPageChildren
			//
			this.m_tabPageChildren.Controls.Add(this.m_lsvChildSummary);
			this.m_tabPageChildren.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageChildren.Name = "m_tabPageChildren";
			this.m_tabPageChildren.Padding = new System.Windows.Forms.Padding(3);
			this.m_tabPageChildren.Size = new System.Drawing.Size(312, 574);
			this.m_tabPageChildren.TabIndex = 1;
			this.m_tabPageChildren.Text = "Child Elements";
			this.m_tabPageChildren.UseVisualStyleBackColor = true;
			//
			// m_lsvChildSummary
			//
			this.m_lsvChildSummary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colChildType,
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
			// m_colChildType
			//
			this.m_colChildType.Text = "ControlType";
			this.m_colChildType.Width = 130;
			//
			// m_colChildCount
			//
			this.m_colChildCount.Text = "Count";
			this.m_colChildCount.Width = 200;
			//
			// m_tabPagePatterns
			//
			this.m_tabPagePatterns.Controls.Add(this.m_panelPatternActions);
			this.m_tabPagePatterns.Controls.Add(this.m_lsvPatterns);
			this.m_tabPagePatterns.Location = new System.Drawing.Point(4, 22);
			this.m_tabPagePatterns.Name = "m_tabPagePatterns";
			this.m_tabPagePatterns.Padding = new System.Windows.Forms.Padding(3);
			this.m_tabPagePatterns.Size = new System.Drawing.Size(312, 574);
			this.m_tabPagePatterns.TabIndex = 2;
			this.m_tabPagePatterns.Text = "Patterns";
			this.m_tabPagePatterns.UseVisualStyleBackColor = true;
			//
			// m_lsvPatterns
			//
			this.m_lsvPatterns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.m_colPatternName,
			this.m_colPatternState});
			this.m_lsvPatterns.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_lsvPatterns.FullRowSelect = true;
			this.m_lsvPatterns.HideSelection = false;
			this.m_lsvPatterns.Location = new System.Drawing.Point(3, 3);
			this.m_lsvPatterns.MultiSelect = false;
			this.m_lsvPatterns.Name = "m_lsvPatterns";
			this.m_lsvPatterns.Size = new System.Drawing.Size(306, 200);
			this.m_lsvPatterns.TabIndex = 0;
			this.m_lsvPatterns.UseCompatibleStateImageBehavior = false;
			this.m_lsvPatterns.View = System.Windows.Forms.View.Details;
			//
			// m_colPatternName
			//
			this.m_colPatternName.Text = "Pattern";
			this.m_colPatternName.Width = 150;
			//
			// m_colPatternState
			//
			this.m_colPatternState.Text = "State";
			this.m_colPatternState.Width = 130;
			//
			// m_panelPatternActions
			//
			this.m_panelPatternActions.Controls.Add(this.m_lblNoAction);
			this.m_panelPatternActions.Controls.Add(this.m_lblActionStatus);
			this.m_panelPatternActions.Controls.Add(this.m_btnSetValue);
			this.m_panelPatternActions.Controls.Add(this.m_txtValue);
			this.m_panelPatternActions.Controls.Add(this.m_lblValue);
			this.m_panelPatternActions.Controls.Add(this.m_btnSelect);
			this.m_panelPatternActions.Controls.Add(this.m_btnCollapse);
			this.m_panelPatternActions.Controls.Add(this.m_btnExpand);
			this.m_panelPatternActions.Controls.Add(this.m_btnToggle);
			this.m_panelPatternActions.Controls.Add(this.m_btnInvoke);
			this.m_panelPatternActions.Controls.Add(this.m_btnMinimize);
			this.m_panelPatternActions.Controls.Add(this.m_btnMaximize);
			this.m_panelPatternActions.Controls.Add(this.m_btnRestore);
			this.m_panelPatternActions.Controls.Add(this.m_btnClose);
			this.m_panelPatternActions.Controls.Add(this.m_lblMoveX);
			this.m_panelPatternActions.Controls.Add(this.m_txtMoveX);
			this.m_panelPatternActions.Controls.Add(this.m_lblMoveY);
			this.m_panelPatternActions.Controls.Add(this.m_txtMoveY);
			this.m_panelPatternActions.Controls.Add(this.m_btnMove);
			this.m_panelPatternActions.Controls.Add(this.m_lblResizeW);
			this.m_panelPatternActions.Controls.Add(this.m_txtResizeW);
			this.m_panelPatternActions.Controls.Add(this.m_lblResizeH);
			this.m_panelPatternActions.Controls.Add(this.m_txtResizeH);
			this.m_panelPatternActions.Controls.Add(this.m_btnResize);
			this.m_panelPatternActions.Controls.Add(this.m_lblRotate);
			this.m_panelPatternActions.Controls.Add(this.m_txtRotate);
			this.m_panelPatternActions.Controls.Add(this.m_btnRotate);
			this.m_panelPatternActions.Controls.Add(this.m_lblNativeActions);
			this.m_panelPatternActions.Controls.Add(this.m_btnNativeEnable);
			this.m_panelPatternActions.Controls.Add(this.m_btnNativeDisable);
			this.m_panelPatternActions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_panelPatternActions.Location = new System.Drawing.Point(3, 203);
			this.m_panelPatternActions.Name = "m_panelPatternActions";
			this.m_panelPatternActions.Size = new System.Drawing.Size(306, 368);
			this.m_panelPatternActions.TabIndex = 1;
			//
			// m_btnInvoke
			//
			this.m_btnInvoke.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnInvoke.Location = new System.Drawing.Point(8, 8);
			this.m_btnInvoke.Name = "m_btnInvoke";
			this.m_btnInvoke.Size = new System.Drawing.Size(90, 23);
			this.m_btnInvoke.TabIndex = 0;
			this.m_btnInvoke.Text = "Invoke";
			this.m_btnInvoke.UseVisualStyleBackColor = false;
			this.m_btnInvoke.Visible = false;
			//
			// m_btnToggle
			//
			this.m_btnToggle.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnToggle.Location = new System.Drawing.Point(8, 8);
			this.m_btnToggle.Name = "m_btnToggle";
			this.m_btnToggle.Size = new System.Drawing.Size(90, 23);
			this.m_btnToggle.TabIndex = 1;
			this.m_btnToggle.Text = "Toggle";
			this.m_btnToggle.UseVisualStyleBackColor = false;
			this.m_btnToggle.Visible = false;
			//
			// m_btnExpand
			//
			this.m_btnExpand.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnExpand.Location = new System.Drawing.Point(8, 8);
			this.m_btnExpand.Name = "m_btnExpand";
			this.m_btnExpand.Size = new System.Drawing.Size(90, 23);
			this.m_btnExpand.TabIndex = 2;
			this.m_btnExpand.Text = "Expand";
			this.m_btnExpand.UseVisualStyleBackColor = false;
			this.m_btnExpand.Visible = false;
			//
			// m_btnCollapse
			//
			this.m_btnCollapse.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnCollapse.Location = new System.Drawing.Point(104, 8);
			this.m_btnCollapse.Name = "m_btnCollapse";
			this.m_btnCollapse.Size = new System.Drawing.Size(90, 23);
			this.m_btnCollapse.TabIndex = 3;
			this.m_btnCollapse.Text = "Collapse";
			this.m_btnCollapse.UseVisualStyleBackColor = false;
			this.m_btnCollapse.Visible = false;
			//
			// m_btnSelect
			//
			this.m_btnSelect.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnSelect.Location = new System.Drawing.Point(8, 8);
			this.m_btnSelect.Name = "m_btnSelect";
			this.m_btnSelect.Size = new System.Drawing.Size(90, 23);
			this.m_btnSelect.TabIndex = 4;
			this.m_btnSelect.Text = "Select";
			this.m_btnSelect.UseVisualStyleBackColor = false;
			this.m_btnSelect.Visible = false;
			//
			// m_lblValue
			//
			this.m_lblValue.AutoSize = true;
			this.m_lblValue.Location = new System.Drawing.Point(8, 13);
			this.m_lblValue.Name = "m_lblValue";
			this.m_lblValue.Size = new System.Drawing.Size(36, 13);
			this.m_lblValue.TabIndex = 5;
			this.m_lblValue.Text = "Value:";
			this.m_lblValue.Visible = false;
			//
			// m_txtValue
			//
			this.m_txtValue.Location = new System.Drawing.Point(50, 9);
			this.m_txtValue.Name = "m_txtValue";
			this.m_txtValue.Size = new System.Drawing.Size(180, 20);
			this.m_txtValue.TabIndex = 6;
			this.m_txtValue.Visible = false;
			//
			// m_btnSetValue
			//
			this.m_btnSetValue.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnSetValue.Location = new System.Drawing.Point(8, 38);
			this.m_btnSetValue.Name = "m_btnSetValue";
			this.m_btnSetValue.Size = new System.Drawing.Size(90, 23);
			this.m_btnSetValue.TabIndex = 7;
			this.m_btnSetValue.Text = "Set Value";
			this.m_btnSetValue.UseVisualStyleBackColor = false;
			this.m_btnSetValue.Visible = false;
			//
			// m_lblActionStatus
			//
			this.m_lblActionStatus.ForeColor = System.Drawing.Color.DarkRed;
			this.m_lblActionStatus.Location = new System.Drawing.Point(8, 70);
			this.m_lblActionStatus.Name = "m_lblActionStatus";
			this.m_lblActionStatus.Size = new System.Drawing.Size(290, 32);
			this.m_lblActionStatus.TabIndex = 9;
			this.m_lblActionStatus.Visible = false;
			//
			// m_lblNoAction
			//
			this.m_lblNoAction.AutoSize = true;
			this.m_lblNoAction.Location = new System.Drawing.Point(8, 8);
			this.m_lblNoAction.Name = "m_lblNoAction";
			this.m_lblNoAction.Size = new System.Drawing.Size(220, 13);
			this.m_lblNoAction.TabIndex = 8;
			this.m_lblNoAction.Text = "No action available for this pattern.";
			this.m_lblNoAction.Visible = false;
			//
			// m_btnMinimize
			//
			this.m_btnMinimize.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnMinimize.Location = new System.Drawing.Point(8, 8);
			this.m_btnMinimize.Name = "m_btnMinimize";
			this.m_btnMinimize.Size = new System.Drawing.Size(90, 23);
			this.m_btnMinimize.TabIndex = 10;
			this.m_btnMinimize.Text = "Minimize";
			this.m_btnMinimize.UseVisualStyleBackColor = false;
			this.m_btnMinimize.Visible = false;
			//
			// m_btnMaximize
			//
			this.m_btnMaximize.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnMaximize.Location = new System.Drawing.Point(104, 8);
			this.m_btnMaximize.Name = "m_btnMaximize";
			this.m_btnMaximize.Size = new System.Drawing.Size(90, 23);
			this.m_btnMaximize.TabIndex = 11;
			this.m_btnMaximize.Text = "Maximize";
			this.m_btnMaximize.UseVisualStyleBackColor = false;
			this.m_btnMaximize.Visible = false;
			//
			// m_btnRestore
			//
			this.m_btnRestore.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnRestore.Location = new System.Drawing.Point(200, 8);
			this.m_btnRestore.Name = "m_btnRestore";
			this.m_btnRestore.Size = new System.Drawing.Size(90, 23);
			this.m_btnRestore.TabIndex = 12;
			this.m_btnRestore.Text = "Restore";
			this.m_btnRestore.UseVisualStyleBackColor = false;
			this.m_btnRestore.Visible = false;
			//
			// m_btnClose
			//
			this.m_btnClose.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnClose.Location = new System.Drawing.Point(8, 38);
			this.m_btnClose.Name = "m_btnClose";
			this.m_btnClose.Size = new System.Drawing.Size(90, 23);
			this.m_btnClose.TabIndex = 13;
			this.m_btnClose.Text = "Close";
			this.m_btnClose.UseVisualStyleBackColor = false;
			this.m_btnClose.Visible = false;
			//
			// m_lblMoveX
			//
			this.m_lblMoveX.AutoSize = true;
			this.m_lblMoveX.Location = new System.Drawing.Point(8, 13);
			this.m_lblMoveX.Name = "m_lblMoveX";
			this.m_lblMoveX.Size = new System.Drawing.Size(17, 13);
			this.m_lblMoveX.TabIndex = 14;
			this.m_lblMoveX.Text = "X:";
			this.m_lblMoveX.Visible = false;
			//
			// m_txtMoveX
			//
			this.m_txtMoveX.Location = new System.Drawing.Point(30, 9);
			this.m_txtMoveX.Name = "m_txtMoveX";
			this.m_txtMoveX.Size = new System.Drawing.Size(60, 20);
			this.m_txtMoveX.TabIndex = 15;
			this.m_txtMoveX.Visible = false;
			//
			// m_lblMoveY
			//
			this.m_lblMoveY.AutoSize = true;
			this.m_lblMoveY.Location = new System.Drawing.Point(98, 13);
			this.m_lblMoveY.Name = "m_lblMoveY";
			this.m_lblMoveY.Size = new System.Drawing.Size(17, 13);
			this.m_lblMoveY.TabIndex = 16;
			this.m_lblMoveY.Text = "Y:";
			this.m_lblMoveY.Visible = false;
			//
			// m_txtMoveY
			//
			this.m_txtMoveY.Location = new System.Drawing.Point(115, 9);
			this.m_txtMoveY.Name = "m_txtMoveY";
			this.m_txtMoveY.Size = new System.Drawing.Size(60, 20);
			this.m_txtMoveY.TabIndex = 17;
			this.m_txtMoveY.Visible = false;
			//
			// m_btnMove
			//
			this.m_btnMove.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnMove.Location = new System.Drawing.Point(185, 8);
			this.m_btnMove.Name = "m_btnMove";
			this.m_btnMove.Size = new System.Drawing.Size(90, 23);
			this.m_btnMove.TabIndex = 18;
			this.m_btnMove.Text = "Move";
			this.m_btnMove.UseVisualStyleBackColor = false;
			this.m_btnMove.Visible = false;
			//
			// m_lblResizeW
			//
			this.m_lblResizeW.AutoSize = true;
			this.m_lblResizeW.Location = new System.Drawing.Point(8, 43);
			this.m_lblResizeW.Name = "m_lblResizeW";
			this.m_lblResizeW.Size = new System.Drawing.Size(20, 13);
			this.m_lblResizeW.TabIndex = 19;
			this.m_lblResizeW.Text = "W:";
			this.m_lblResizeW.Visible = false;
			//
			// m_txtResizeW
			//
			this.m_txtResizeW.Location = new System.Drawing.Point(30, 39);
			this.m_txtResizeW.Name = "m_txtResizeW";
			this.m_txtResizeW.Size = new System.Drawing.Size(60, 20);
			this.m_txtResizeW.TabIndex = 20;
			this.m_txtResizeW.Visible = false;
			//
			// m_lblResizeH
			//
			this.m_lblResizeH.AutoSize = true;
			this.m_lblResizeH.Location = new System.Drawing.Point(98, 43);
			this.m_lblResizeH.Name = "m_lblResizeH";
			this.m_lblResizeH.Size = new System.Drawing.Size(20, 13);
			this.m_lblResizeH.TabIndex = 21;
			this.m_lblResizeH.Text = "H:";
			this.m_lblResizeH.Visible = false;
			//
			// m_txtResizeH
			//
			this.m_txtResizeH.Location = new System.Drawing.Point(115, 39);
			this.m_txtResizeH.Name = "m_txtResizeH";
			this.m_txtResizeH.Size = new System.Drawing.Size(60, 20);
			this.m_txtResizeH.TabIndex = 22;
			this.m_txtResizeH.Visible = false;
			//
			// m_btnResize
			//
			this.m_btnResize.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnResize.Location = new System.Drawing.Point(185, 38);
			this.m_btnResize.Name = "m_btnResize";
			this.m_btnResize.Size = new System.Drawing.Size(90, 23);
			this.m_btnResize.TabIndex = 23;
			this.m_btnResize.Text = "Resize";
			this.m_btnResize.UseVisualStyleBackColor = false;
			this.m_btnResize.Visible = false;
			//
			// m_lblRotate
			//
			this.m_lblRotate.AutoSize = true;
			this.m_lblRotate.Location = new System.Drawing.Point(8, 73);
			this.m_lblRotate.Name = "m_lblRotate";
			this.m_lblRotate.Size = new System.Drawing.Size(30, 13);
			this.m_lblRotate.TabIndex = 24;
			this.m_lblRotate.Text = "Deg:";
			this.m_lblRotate.Visible = false;
			//
			// m_txtRotate
			//
			this.m_txtRotate.Location = new System.Drawing.Point(40, 69);
			this.m_txtRotate.Name = "m_txtRotate";
			this.m_txtRotate.Size = new System.Drawing.Size(60, 20);
			this.m_txtRotate.TabIndex = 25;
			this.m_txtRotate.Visible = false;
			//
			// m_btnRotate
			//
			this.m_btnRotate.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnRotate.Location = new System.Drawing.Point(185, 68);
			this.m_btnRotate.Name = "m_btnRotate";
			this.m_btnRotate.Size = new System.Drawing.Size(90, 23);
			this.m_btnRotate.TabIndex = 26;
			this.m_btnRotate.Text = "Rotate";
			this.m_btnRotate.UseVisualStyleBackColor = false;
			this.m_btnRotate.Visible = false;
			//
			// m_lblNativeActions
			//
			this.m_lblNativeActions.AutoSize = true;
			this.m_lblNativeActions.Location = new System.Drawing.Point(8, 112);
			this.m_lblNativeActions.Name = "m_lblNativeActions";
			this.m_lblNativeActions.Size = new System.Drawing.Size(170, 13);
			this.m_lblNativeActions.TabIndex = 27;
			this.m_lblNativeActions.Text = "Native Window Actions (HWND):";
			this.m_lblNativeActions.Visible = false;
			//
			// m_btnNativeEnable
			//
			this.m_btnNativeEnable.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnNativeEnable.Location = new System.Drawing.Point(8, 132);
			this.m_btnNativeEnable.Name = "m_btnNativeEnable";
			this.m_btnNativeEnable.Size = new System.Drawing.Size(90, 23);
			this.m_btnNativeEnable.TabIndex = 28;
			this.m_btnNativeEnable.Text = "Enable";
			this.m_btnNativeEnable.UseVisualStyleBackColor = false;
			this.m_btnNativeEnable.Visible = false;
			//
			// m_btnNativeDisable
			//
			this.m_btnNativeDisable.BackColor = System.Drawing.Color.FromArgb(255, 224, 178);
			this.m_btnNativeDisable.Location = new System.Drawing.Point(104, 132);
			this.m_btnNativeDisable.Name = "m_btnNativeDisable";
			this.m_btnNativeDisable.Size = new System.Drawing.Size(90, 23);
			this.m_btnNativeDisable.TabIndex = 29;
			this.m_btnNativeDisable.Text = "Disable";
			this.m_btnNativeDisable.UseVisualStyleBackColor = false;
			this.m_btnNativeDisable.Visible = false;
			//
			// UiaPropertyViewControl
			//
			this.Controls.Add(this.m_tabMain);
			this.Name = "UiaPropertyViewControl";
			this.Size = new System.Drawing.Size(320, 600);
			this.m_tabMain.ResumeLayout(false);
			this.m_tabPageBasic.ResumeLayout(false);
			this.m_tabPageChildren.ResumeLayout(false);
			this.m_tabPagePatterns.ResumeLayout(false);
			this.m_panelPatternActions.ResumeLayout(false);
			this.m_panelPatternActions.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.TabControl m_tabMain;
		private System.Windows.Forms.TabPage m_tabPageBasic;
		private System.Windows.Forms.ListView m_lsvBasic;
		private System.Windows.Forms.ColumnHeader m_colBasicProperty;
		private System.Windows.Forms.ColumnHeader m_colBasicValue;
		private System.Windows.Forms.TabPage m_tabPageChildren;
		private System.Windows.Forms.ListView m_lsvChildSummary;
		private System.Windows.Forms.ColumnHeader m_colChildType;
		private System.Windows.Forms.ColumnHeader m_colChildCount;
		private System.Windows.Forms.TabPage m_tabPagePatterns;
		private System.Windows.Forms.ListView m_lsvPatterns;
		private System.Windows.Forms.ColumnHeader m_colPatternName;
		private System.Windows.Forms.ColumnHeader m_colPatternState;
		private System.Windows.Forms.Panel m_panelPatternActions;
		private System.Windows.Forms.Button m_btnInvoke;
		private System.Windows.Forms.Button m_btnToggle;
		private System.Windows.Forms.Button m_btnExpand;
		private System.Windows.Forms.Button m_btnCollapse;
		private System.Windows.Forms.Button m_btnSelect;
		private System.Windows.Forms.Label m_lblValue;
		private System.Windows.Forms.TextBox m_txtValue;
		private System.Windows.Forms.Button m_btnSetValue;
		private System.Windows.Forms.Label m_lblNoAction;
		private System.Windows.Forms.Label m_lblActionStatus;
		private System.Windows.Forms.Button m_btnMinimize;
		private System.Windows.Forms.Button m_btnMaximize;
		private System.Windows.Forms.Button m_btnRestore;
		private System.Windows.Forms.Button m_btnClose;
		private System.Windows.Forms.Label m_lblMoveX;
		private System.Windows.Forms.TextBox m_txtMoveX;
		private System.Windows.Forms.Label m_lblMoveY;
		private System.Windows.Forms.TextBox m_txtMoveY;
		private System.Windows.Forms.Button m_btnMove;
		private System.Windows.Forms.Label m_lblResizeW;
		private System.Windows.Forms.TextBox m_txtResizeW;
		private System.Windows.Forms.Label m_lblResizeH;
		private System.Windows.Forms.TextBox m_txtResizeH;
		private System.Windows.Forms.Button m_btnResize;
		private System.Windows.Forms.Label m_lblRotate;
		private System.Windows.Forms.TextBox m_txtRotate;
		private System.Windows.Forms.Button m_btnRotate;
		private System.Windows.Forms.Label m_lblNativeActions;
		private System.Windows.Forms.Button m_btnNativeEnable;
		private System.Windows.Forms.Button m_btnNativeDisable;
	}
}
