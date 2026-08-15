//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DH.Window.Analyst.UI.Controls {
	partial class WindowWorkspaceTabControl {
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
			this.m_tabPageInspector = new System.Windows.Forms.TabPage();
			this.m_ctrlBreadcrumb = new DH.Window.Analyst.UI.Controls.BreadcrumbControl();
			this.m_splitContent = new System.Windows.Forms.SplitContainer();
			this.m_ctrlHierarchy = new DH.Window.Analyst.UI.Controls.WindowHierarchyControl();
			this.m_ctrlPropertyView = new DH.Window.Analyst.UI.Controls.PropertyViewControl();
			this.m_ctrlUiaPropertyView = new DH.Window.Analyst.UI.Controls.UiaPropertyViewControl();
			this.m_tabPageEvents = new System.Windows.Forms.TabPage();
			this.m_ctrlEventLog = new DH.Window.Analyst.UI.Controls.EventLogControl();
			this.m_tabPageMessages = new System.Windows.Forms.TabPage();
			this.m_ctrlMessageLog = new DH.Window.Analyst.UI.Controls.MessageLogControl();
			this.m_tabPageAccessibility = new System.Windows.Forms.TabPage();
			this.m_ctrlAccessibilityCheck = new DH.Window.Analyst.UI.Controls.AccessibilityCheckControl();
			this.m_panelClosedBanner = new System.Windows.Forms.Panel();
			this.m_lblClosedBanner = new System.Windows.Forms.Label();
			this.m_timerWindowCheck = new System.Windows.Forms.Timer(this.components);
			this.m_panelClosedBanner.SuspendLayout();
			this.m_tabMain.SuspendLayout();
			this.m_tabPageInspector.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_splitContent)).BeginInit();
			this.m_splitContent.Panel1.SuspendLayout();
			this.m_splitContent.Panel2.SuspendLayout();
			this.m_splitContent.SuspendLayout();
			this.m_tabPageEvents.SuspendLayout();
			this.m_tabPageMessages.SuspendLayout();
			this.m_tabPageAccessibility.SuspendLayout();
			this.SuspendLayout();
			//
			// m_tabMain
			//
			this.m_tabMain.Controls.Add(this.m_tabPageInspector);
			this.m_tabMain.Controls.Add(this.m_tabPageEvents);
			this.m_tabMain.Controls.Add(this.m_tabPageMessages);
			this.m_tabMain.Controls.Add(this.m_tabPageAccessibility);
			this.m_tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_tabMain.Location = new System.Drawing.Point(0, 0);
			this.m_tabMain.Name = "m_tabMain";
			this.m_tabMain.SelectedIndex = 0;
			this.m_tabMain.Size = new System.Drawing.Size(700, 500);
			this.m_tabMain.TabIndex = 0;
			//
			// m_tabPageInspector
			//
			this.m_tabPageInspector.Controls.Add(this.m_splitContent);
			this.m_tabPageInspector.Controls.Add(this.m_ctrlBreadcrumb);
			this.m_tabPageInspector.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageInspector.Name = "m_tabPageInspector";
			this.m_tabPageInspector.Size = new System.Drawing.Size(692, 474);
			this.m_tabPageInspector.TabIndex = 0;
			this.m_tabPageInspector.Text = "Inspector";
			this.m_tabPageInspector.UseVisualStyleBackColor = true;
			//
			// m_ctrlBreadcrumb
			//
			this.m_ctrlBreadcrumb.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_ctrlBreadcrumb.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlBreadcrumb.Name = "m_ctrlBreadcrumb";
			this.m_ctrlBreadcrumb.Size = new System.Drawing.Size(692, 28);
			this.m_ctrlBreadcrumb.TabIndex = 0;
			//
			// m_splitContent
			//
			this.m_splitContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_splitContent.Location = new System.Drawing.Point(0, 28);
			this.m_splitContent.Name = "m_splitContent";
			//
			// m_splitContent.Panel1
			//
			this.m_splitContent.Panel1.Controls.Add(this.m_ctrlHierarchy);
			//
			// m_splitContent.Panel2
			//
			this.m_splitContent.Panel2.Controls.Add(this.m_ctrlUiaPropertyView);
			this.m_splitContent.Panel2.Controls.Add(this.m_ctrlPropertyView);
			this.m_splitContent.Size = new System.Drawing.Size(692, 446);
			this.m_splitContent.SplitterDistance = 300;
			this.m_splitContent.TabIndex = 1;
			//
			// m_ctrlHierarchy
			//
			this.m_ctrlHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlHierarchy.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlHierarchy.Name = "m_ctrlHierarchy";
			this.m_ctrlHierarchy.Size = new System.Drawing.Size(300, 446);
			this.m_ctrlHierarchy.TabIndex = 0;
			//
			// m_ctrlPropertyView
			//
			this.m_ctrlPropertyView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPropertyView.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlPropertyView.Name = "m_ctrlPropertyView";
			this.m_ctrlPropertyView.Size = new System.Drawing.Size(388, 446);
			this.m_ctrlPropertyView.TabIndex = 0;
			//
			// m_ctrlUiaPropertyView
			//
			this.m_ctrlUiaPropertyView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlUiaPropertyView.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlUiaPropertyView.Name = "m_ctrlUiaPropertyView";
			this.m_ctrlUiaPropertyView.Size = new System.Drawing.Size(388, 446);
			this.m_ctrlUiaPropertyView.TabIndex = 1;
			this.m_ctrlUiaPropertyView.Visible = false;
			//
			// m_tabPageEvents
			//
			this.m_tabPageEvents.Controls.Add(this.m_ctrlEventLog);
			this.m_tabPageEvents.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageEvents.Name = "m_tabPageEvents";
			this.m_tabPageEvents.Size = new System.Drawing.Size(692, 474);
			this.m_tabPageEvents.TabIndex = 1;
			this.m_tabPageEvents.Text = "Events";
			this.m_tabPageEvents.UseVisualStyleBackColor = true;
			//
			// m_ctrlEventLog
			//
			this.m_ctrlEventLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlEventLog.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlEventLog.Name = "m_ctrlEventLog";
			this.m_ctrlEventLog.Size = new System.Drawing.Size(692, 474);
			this.m_ctrlEventLog.TabIndex = 0;
			//
			// m_tabPageMessages
			//
			this.m_tabPageMessages.Controls.Add(this.m_ctrlMessageLog);
			this.m_tabPageMessages.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageMessages.Name = "m_tabPageMessages";
			this.m_tabPageMessages.Size = new System.Drawing.Size(692, 474);
			this.m_tabPageMessages.TabIndex = 2;
			this.m_tabPageMessages.Text = "Messages";
			this.m_tabPageMessages.UseVisualStyleBackColor = true;
			//
			// m_ctrlMessageLog
			//
			this.m_ctrlMessageLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlMessageLog.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlMessageLog.Name = "m_ctrlMessageLog";
			this.m_ctrlMessageLog.Size = new System.Drawing.Size(692, 474);
			this.m_ctrlMessageLog.TabIndex = 0;
			//
			// m_tabPageAccessibility
			//
			this.m_tabPageAccessibility.Controls.Add(this.m_ctrlAccessibilityCheck);
			this.m_tabPageAccessibility.Location = new System.Drawing.Point(4, 22);
			this.m_tabPageAccessibility.Name = "m_tabPageAccessibility";
			this.m_tabPageAccessibility.Size = new System.Drawing.Size(692, 474);
			this.m_tabPageAccessibility.TabIndex = 3;
			this.m_tabPageAccessibility.Text = "Accessibility";
			this.m_tabPageAccessibility.UseVisualStyleBackColor = true;
			//
			// m_ctrlAccessibilityCheck
			//
			this.m_ctrlAccessibilityCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlAccessibilityCheck.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlAccessibilityCheck.Name = "m_ctrlAccessibilityCheck";
			this.m_ctrlAccessibilityCheck.Size = new System.Drawing.Size(692, 474);
			this.m_ctrlAccessibilityCheck.TabIndex = 0;
			//
			// m_panelClosedBanner
			//
			this.m_panelClosedBanner.BackColor = System.Drawing.Color.MistyRose;
			this.m_panelClosedBanner.Controls.Add(this.m_lblClosedBanner);
			this.m_panelClosedBanner.Dock = System.Windows.Forms.DockStyle.Top;
			this.m_panelClosedBanner.Location = new System.Drawing.Point(0, 0);
			this.m_panelClosedBanner.Name = "m_panelClosedBanner";
			this.m_panelClosedBanner.Size = new System.Drawing.Size(700, 24);
			this.m_panelClosedBanner.TabIndex = 1;
			this.m_panelClosedBanner.Visible = false;
			//
			// m_lblClosedBanner
			//
			this.m_lblClosedBanner.AutoSize = true;
			this.m_lblClosedBanner.ForeColor = System.Drawing.Color.DarkRed;
			this.m_lblClosedBanner.Location = new System.Drawing.Point(6, 5);
			this.m_lblClosedBanner.Name = "m_lblClosedBanner";
			this.m_lblClosedBanner.Size = new System.Drawing.Size(240, 13);
			this.m_lblClosedBanner.TabIndex = 0;
			this.m_lblClosedBanner.Text = "This window has been closed.";
			//
			// m_timerWindowCheck
			//
			this.m_timerWindowCheck.Interval = 1000;
			//
			// WindowWorkspaceTabControl
			//
			this.Controls.Add(this.m_tabMain);
			this.Controls.Add(this.m_panelClosedBanner);
			this.Name = "WindowWorkspaceTabControl";
			this.Size = new System.Drawing.Size(700, 500);
			this.m_panelClosedBanner.ResumeLayout(false);
			this.m_panelClosedBanner.PerformLayout();
			this.m_tabMain.ResumeLayout(false);
			this.m_tabPageInspector.ResumeLayout(false);
			this.m_splitContent.Panel1.ResumeLayout(false);
			this.m_splitContent.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_splitContent)).EndInit();
			this.m_splitContent.ResumeLayout(false);
			this.m_tabPageEvents.ResumeLayout(false);
			this.m_tabPageMessages.ResumeLayout(false);
			this.m_tabPageAccessibility.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl m_tabMain;
		private System.Windows.Forms.TabPage m_tabPageInspector;
		private DH.Window.Analyst.UI.Controls.BreadcrumbControl m_ctrlBreadcrumb;
		private System.Windows.Forms.SplitContainer m_splitContent;
		private DH.Window.Analyst.UI.Controls.WindowHierarchyControl m_ctrlHierarchy;
		private DH.Window.Analyst.UI.Controls.PropertyViewControl m_ctrlPropertyView;
		private DH.Window.Analyst.UI.Controls.UiaPropertyViewControl m_ctrlUiaPropertyView;
		private System.Windows.Forms.TabPage m_tabPageEvents;
		private DH.Window.Analyst.UI.Controls.EventLogControl m_ctrlEventLog;
		private System.Windows.Forms.TabPage m_tabPageMessages;
		private DH.Window.Analyst.UI.Controls.MessageLogControl m_ctrlMessageLog;
		private System.Windows.Forms.TabPage m_tabPageAccessibility;
		private DH.Window.Analyst.UI.Controls.AccessibilityCheckControl m_ctrlAccessibilityCheck;
		private System.Windows.Forms.Panel m_panelClosedBanner;
		private System.Windows.Forms.Label m_lblClosedBanner;
		private System.Windows.Forms.Timer m_timerWindowCheck;
	}
}
