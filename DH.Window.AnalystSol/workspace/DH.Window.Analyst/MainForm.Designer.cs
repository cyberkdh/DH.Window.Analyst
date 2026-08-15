//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst {
	partial class MainForm {
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
			this.m_menuMain = new System.Windows.Forms.MenuStrip();
			this.m_menuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuFile_Options = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuFile_OptionsSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.m_menuFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuView = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuView_Refresh = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuView_Find = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuWindow_CloseTab = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuWindow_DetachTab = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuWindow_TabListSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.m_menuWindow_TabList = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuHelp_About = new System.Windows.Forms.ToolStripMenuItem();
			this.m_toolMain = new System.Windows.Forms.ToolStrip();
			this.m_toolBtnRefresh = new System.Windows.Forms.ToolStripButton();
			this.m_toolSepFind = new System.Windows.Forms.ToolStripSeparator();
			this.m_toolBtnFind = new System.Windows.Forms.ToolStripButton();
			this.m_toolBtnShowInfoOnMouse = new System.Windows.Forms.ToolStripButton();
			this.m_toolSepWindowOps = new System.Windows.Forms.ToolStripSeparator();
			this.m_toolBtnHighlight = new System.Windows.Forms.ToolStripButton();
			this.m_toolBtnForeground = new System.Windows.Forms.ToolStripButton();
			this.m_toolBtnGroupByProcess = new System.Windows.Forms.ToolStripButton();
			this.m_toolSepTabList = new System.Windows.Forms.ToolStripSeparator();
			this.m_toolBtnTabList = new System.Windows.Forms.ToolStripButton();
			this.m_splitMain = new System.Windows.Forms.SplitContainer();
			this.panel3 = new System.Windows.Forms.Panel();
			this.m_ctrlWindowList = new DH.Window.Analyst.UI.Controls.WindowListControl();
			this.m_panelMenuInWinList = new System.Windows.Forms.Panel();
			this.m_ctrlFinder = new DH.Window.Analyst.UI.Controls.FinderToolButton();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.m_ctrlPropertyView = new DH.Window.Analyst.UI.Controls.PropertyViewControl();
			this.panel2 = new System.Windows.Forms.Panel();
			this.m_btn_refresh_property = new System.Windows.Forms.Button();
			this.m_ctrlWorkspace = new DH.Window.Analyst.UI.Controls.WorkspaceControl();
			this.m_menuMain.SuspendLayout();
			this.m_toolMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_splitMain)).BeginInit();
			this.m_splitMain.Panel1.SuspendLayout();
			this.m_splitMain.SuspendLayout();
			this.panel3.SuspendLayout();
			this.m_panelMenuInWinList.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			//
			// m_menuMain
			//
			this.m_menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.m_menuFile,
				this.m_menuView,
				this.m_menuWindow,
				this.m_menuHelp});
			this.m_menuMain.Location = new System.Drawing.Point(0, 0);
			this.m_menuMain.Name = "m_menuMain";
			this.m_menuMain.Size = new System.Drawing.Size(1000, 24);
			this.m_menuMain.TabIndex = 0;
			//
			// m_menuFile
			//
			this.m_menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.m_menuFile_Options,
				this.m_menuFile_OptionsSeparator,
				this.m_menuFile_Exit});
			this.m_menuFile.Name = "m_menuFile";
			this.m_menuFile.Text = "&File";
			//
			// m_menuFile_Options
			//
			this.m_menuFile_Options.Name = "m_menuFile_Options";
			this.m_menuFile_Options.Text = "Options...";
			this.m_menuFile_Options.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_menuFile_OptionsSeparator
			//
			this.m_menuFile_OptionsSeparator.Name = "m_menuFile_OptionsSeparator";
			//
			// m_menuFile_Exit
			//
			this.m_menuFile_Exit.Name = "m_menuFile_Exit";
			this.m_menuFile_Exit.Text = "Exit";
			this.m_menuFile_Exit.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_menuView
			//
			this.m_menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.m_menuView_Refresh,
				this.m_menuView_Find});
			this.m_menuView.Name = "m_menuView";
			this.m_menuView.Text = "&View";
			//
			// m_menuView_Refresh
			//
			this.m_menuView_Refresh.Name = "m_menuView_Refresh";
			this.m_menuView_Refresh.Text = "Refresh Window List";
			this.m_menuView_Refresh.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_menuView_Find
			//
			this.m_menuView_Find.Name = "m_menuView_Find";
			this.m_menuView_Find.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F;
			this.m_menuView_Find.Text = "Find Window...";
			this.m_menuView_Find.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_menuWindow
			//
			this.m_menuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.m_menuWindow_CloseTab,
				this.m_menuWindow_DetachTab,
				this.m_menuWindow_TabListSeparator,
				this.m_menuWindow_TabList});
			this.m_menuWindow.Name = "m_menuWindow";
			this.m_menuWindow.Text = "&Window";
			//
			// m_menuWindow_CloseTab
			//
			this.m_menuWindow_CloseTab.Name = "m_menuWindow_CloseTab";
			this.m_menuWindow_CloseTab.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W;
			this.m_menuWindow_CloseTab.Text = "Close Tab";
			this.m_menuWindow_CloseTab.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_menuWindow_DetachTab
			//
			this.m_menuWindow_DetachTab.Name = "m_menuWindow_DetachTab";
			this.m_menuWindow_DetachTab.Text = "Detach Tab to New Window";
			this.m_menuWindow_DetachTab.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_menuWindow_TabListSeparator
			//
			this.m_menuWindow_TabListSeparator.Name = "m_menuWindow_TabListSeparator";
			//
			// m_menuWindow_TabList
			//
			this.m_menuWindow_TabList.Name = "m_menuWindow_TabList";
			this.m_menuWindow_TabList.Text = "Tab List";
			//
			// m_menuHelp
			//
			this.m_menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.m_menuHelp_About});
			this.m_menuHelp.Name = "m_menuHelp";
			this.m_menuHelp.Text = "&Help";
			//
			// m_menuHelp_About
			//
			this.m_menuHelp_About.Name = "m_menuHelp_About";
			this.m_menuHelp_About.Text = "About";
			this.m_menuHelp_About.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_toolMain
			//
			this.m_toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.m_toolBtnRefresh,
				this.m_toolSepFind,
				this.m_toolBtnFind,
				this.m_toolBtnShowInfoOnMouse,
				this.m_toolSepWindowOps,
				this.m_toolBtnHighlight,
				this.m_toolBtnForeground,
				this.m_toolBtnGroupByProcess,
				this.m_toolSepTabList,
				this.m_toolBtnTabList});
			this.m_toolMain.Location = new System.Drawing.Point(0, 24);
			this.m_toolMain.Name = "m_toolMain";
			this.m_toolMain.Size = new System.Drawing.Size(1000, 25);
			this.m_toolMain.TabIndex = 1;
			//
			// m_toolBtnRefresh
			//
			this.m_toolBtnRefresh.Name = "m_toolBtnRefresh";
			this.m_toolBtnRefresh.Size = new System.Drawing.Size(56, 22);
			this.m_toolBtnRefresh.Text = "Refresh";
			this.m_toolBtnRefresh.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_toolSepFind
			//
			this.m_toolSepFind.Name = "m_toolSepFind";
			this.m_toolSepFind.Size = new System.Drawing.Size(6, 25);
			//
			// m_toolBtnFind
			//
			this.m_toolBtnFind.Name = "m_toolBtnFind";
			this.m_toolBtnFind.Size = new System.Drawing.Size(80, 22);
			this.m_toolBtnFind.Text = "Find Window...";
			this.m_toolBtnFind.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_toolBtnShowInfoOnMouse
			//
			this.m_toolBtnShowInfoOnMouse.CheckOnClick = true;
			this.m_toolBtnShowInfoOnMouse.Name = "m_toolBtnShowInfoOnMouse";
			this.m_toolBtnShowInfoOnMouse.Size = new System.Drawing.Size(115, 22);
			this.m_toolBtnShowInfoOnMouse.Text = "Show Info on Mouse";
			this.m_toolBtnShowInfoOnMouse.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_toolSepWindowOps
			//
			this.m_toolSepWindowOps.Name = "m_toolSepWindowOps";
			this.m_toolSepWindowOps.Size = new System.Drawing.Size(6, 25);
			//
			// m_toolBtnHighlight
			//
			this.m_toolBtnHighlight.Enabled = false;
			this.m_toolBtnHighlight.Name = "m_toolBtnHighlight";
			this.m_toolBtnHighlight.Size = new System.Drawing.Size(60, 22);
			this.m_toolBtnHighlight.Text = "Highlight";
			this.m_toolBtnHighlight.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_toolBtnForeground
			//
			this.m_toolBtnForeground.Enabled = false;
			this.m_toolBtnForeground.Name = "m_toolBtnForeground";
			this.m_toolBtnForeground.Size = new System.Drawing.Size(75, 22);
			this.m_toolBtnForeground.Text = "Foreground";
			this.m_toolBtnForeground.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_toolBtnGroupByProcess
			//
			this.m_toolBtnGroupByProcess.Name = "m_toolBtnGroupByProcess";
			this.m_toolBtnGroupByProcess.Size = new System.Drawing.Size(90, 22);
			this.m_toolBtnGroupByProcess.Text = "Group: Process";
			this.m_toolBtnGroupByProcess.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_toolSepTabList
			//
			this.m_toolSepTabList.Name = "m_toolSepTabList";
			this.m_toolSepTabList.Size = new System.Drawing.Size(6, 25);
			//
			// m_toolBtnTabList
			//
			this.m_toolBtnTabList.Name = "m_toolBtnTabList";
			this.m_toolBtnTabList.Size = new System.Drawing.Size(60, 22);
			this.m_toolBtnTabList.Text = "Tab List";
			this.m_toolBtnTabList.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_splitMain
			//
			this.m_splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_splitMain.Location = new System.Drawing.Point(0, 0);
			this.m_splitMain.Name = "m_splitMain";
			//
			// m_splitMain.Panel1
			//
			this.m_splitMain.Panel1.Controls.Add(this.panel3);
			this.m_splitMain.Panel1.Controls.Add(this.splitter1);
			this.m_splitMain.Panel1.Controls.Add(this.panel1);
			this.m_splitMain.Panel2.Controls.Add(this.m_ctrlWorkspace);
			this.m_splitMain.Size = new System.Drawing.Size(1000, 736);
			this.m_splitMain.SplitterDistance = 320;
			this.m_splitMain.SplitterWidth = 8;
			this.m_splitMain.TabIndex = 0;
			//
			// panel3
			//
			this.panel3.Controls.Add(this.m_ctrlWindowList);
			this.panel3.Controls.Add(this.m_panelMenuInWinList);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(320, 475);
			this.panel3.TabIndex = 3;
			//
			// m_ctrlWindowList
			//
			this.m_ctrlWindowList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlWindowList.GroupByProcess = true;
			this.m_ctrlWindowList.Location = new System.Drawing.Point(0, 32);
			this.m_ctrlWindowList.Name = "m_ctrlWindowList";
			this.m_ctrlWindowList.Size = new System.Drawing.Size(320, 443);
			this.m_ctrlWindowList.TabIndex = 0;
			//
			// m_panelMenuInWinList
			//
			this.m_panelMenuInWinList.Controls.Add(this.m_ctrlFinder);
			this.m_panelMenuInWinList.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelMenuInWinList.Location = new System.Drawing.Point(0, 0);
			this.m_panelMenuInWinList.Name = "m_panelMenuInWinList";
			this.m_panelMenuInWinList.Size = new System.Drawing.Size(320, 32);
			this.m_panelMenuInWinList.TabIndex = 1;
			//
			// m_ctrlFinder
			//
			this.m_ctrlFinder.Location = new System.Drawing.Point(3, 3);
			this.m_ctrlFinder.Name = "m_ctrlFinder";
			this.m_ctrlFinder.Size = new System.Drawing.Size(100, 23);
			this.m_ctrlFinder.TabIndex = 0;
			this.m_ctrlFinder.Text = "Instant Find";
			this.m_ctrlFinder.UseVisualStyleBackColor = true;
			//
			// splitter1
			//
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 475);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(320, 8);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			//
			// panel1
			//
			this.panel1.Controls.Add(this.m_ctrlPropertyView);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 483);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(320, 253);
			this.panel1.TabIndex = 1;
			//
			// m_ctrlPropertyView
			//
			this.m_ctrlPropertyView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPropertyView.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlPropertyView.Name = "m_ctrlPropertyView";
			this.m_ctrlPropertyView.Size = new System.Drawing.Size(320, 223);
			this.m_ctrlPropertyView.TabIndex = 0;
			//
			// panel2
			//
			this.panel2.Controls.Add(this.m_btn_refresh_property);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 223);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(320, 30);
			this.panel2.TabIndex = 1;
			//
			// m_btn_refresh_property
			//
			this.m_btn_refresh_property.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btn_refresh_property.Location = new System.Drawing.Point(242, 3);
			this.m_btn_refresh_property.Name = "m_btn_refresh_property";
			this.m_btn_refresh_property.Size = new System.Drawing.Size(75, 23);
			this.m_btn_refresh_property.TabIndex = 0;
			this.m_btn_refresh_property.Text = "Refresh";
			this.m_btn_refresh_property.UseVisualStyleBackColor = true;
			this.m_btn_refresh_property.Click += new System.EventHandler(this.OnProcControls);
			//
			// m_ctrlWorkspace
			//
			this.m_ctrlWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlWorkspace.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlWorkspace.Name = "m_ctrlWorkspace";
			this.m_ctrlWorkspace.Size = new System.Drawing.Size(672, 736);
			this.m_ctrlWorkspace.TabIndex = 0;
			//
			// MainForm
			//
			this.ClientSize = new System.Drawing.Size(1000, 736);
			this.Controls.Add(this.m_splitMain);
			this.Controls.Add(this.m_toolMain);
			this.Controls.Add(this.m_menuMain);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.MainMenuStrip = this.m_menuMain;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DH.Window.Analyst";
			this.m_menuMain.ResumeLayout(false);
			this.m_menuMain.PerformLayout();
			this.m_toolMain.ResumeLayout(false);
			this.m_toolMain.PerformLayout();
			this.m_splitMain.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_splitMain)).EndInit();
			this.m_splitMain.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.m_panelMenuInWinList.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip m_menuMain;
		private System.Windows.Forms.ToolStripMenuItem m_menuFile;
		private System.Windows.Forms.ToolStripMenuItem m_menuFile_Options;
		private System.Windows.Forms.ToolStripSeparator m_menuFile_OptionsSeparator;
		private System.Windows.Forms.ToolStripMenuItem m_menuFile_Exit;
		private System.Windows.Forms.ToolStripMenuItem m_menuView;
		private System.Windows.Forms.ToolStripMenuItem m_menuView_Refresh;
		private System.Windows.Forms.ToolStripMenuItem m_menuView_Find;
		private System.Windows.Forms.ToolStripMenuItem m_menuWindow;
		private System.Windows.Forms.ToolStripMenuItem m_menuWindow_CloseTab;
		private System.Windows.Forms.ToolStripMenuItem m_menuWindow_DetachTab;
		private System.Windows.Forms.ToolStripSeparator m_menuWindow_TabListSeparator;
		private System.Windows.Forms.ToolStripMenuItem m_menuWindow_TabList;
		private System.Windows.Forms.ToolStripMenuItem m_menuHelp;
		private System.Windows.Forms.ToolStripMenuItem m_menuHelp_About;
		private System.Windows.Forms.ToolStrip m_toolMain;
		private System.Windows.Forms.ToolStripButton m_toolBtnRefresh;
		private System.Windows.Forms.ToolStripSeparator m_toolSepFind;
		private System.Windows.Forms.ToolStripButton m_toolBtnFind;
		private System.Windows.Forms.ToolStripButton m_toolBtnShowInfoOnMouse;
		private System.Windows.Forms.ToolStripSeparator m_toolSepWindowOps;
		private System.Windows.Forms.ToolStripButton m_toolBtnHighlight;
		private System.Windows.Forms.ToolStripButton m_toolBtnForeground;
		private System.Windows.Forms.ToolStripButton m_toolBtnGroupByProcess;
		private System.Windows.Forms.ToolStripSeparator m_toolSepTabList;
		private System.Windows.Forms.ToolStripButton m_toolBtnTabList;
		private System.Windows.Forms.SplitContainer m_splitMain;
		private DH.Window.Analyst.UI.Controls.WindowListControl m_ctrlWindowList;
		private DH.Window.Analyst.UI.Controls.PropertyViewControl m_ctrlPropertyView;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button m_btn_refresh_property;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel m_panelMenuInWinList;
		private DH.Window.Analyst.UI.Controls.FinderToolButton m_ctrlFinder;
		private DH.Window.Analyst.UI.Controls.WorkspaceControl m_ctrlWorkspace;
	}
}
