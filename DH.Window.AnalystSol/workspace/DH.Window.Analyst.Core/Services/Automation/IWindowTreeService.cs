//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public interface IWindowTreeService {
		// false (default) = RawView (unfiltered, includes Legacy IAccessible bridge peers); true = ControlView (IsControlElement-filtered, fewer duplicate nodes). Affects GetChildren and GetElementAncestorChainToRoot.
		bool UseControlView { get; set; }

		// Win32-based, no UIA, returns immediately
		IEnumerable<TopLevelWindowItem> GetTopLevelWindowInfos();

		TopLevelWindowItem GetTopLevelWindowAtScreenPoint(System.Drawing.Point point);

		// raw WindowFromPoint result under the given screen point, NOT resolved up to its top-level ancestor (Show Info on Mouse); null if none found
		TopLevelWindowItem GetWindowAtScreenPoint(System.Drawing.Point point);

		// returned top-down (root's direct child first, handle last); empty if handle is not actually a descendant of roothandle
		IReadOnlyList<IntPtr> GetAncestorChainToRoot(IntPtr handle, IntPtr roothandle);

		System.Drawing.Rectangle GetWindowScreenRect(IntPtr handle);

		void ActivateWindow(IntPtr handle);

		// resolves the top-level ancestor HWND of any window (top-level or descendant); returns the handle itself if already top-level
		IntPtr GetTopLevelAncestorHandle(IntPtr handle);

		// if handlefilter is non-zero it takes priority (exact match, no tree walk); otherwise searches every window for a caption/classname substring match, case-insensitive
		IEnumerable<TopLevelWindowItem> FindWindows(string strcaption, string strclassname, IntPtr handlefilter);

		// Win32-level criteria only (Caption/ClassName/Handle/ProcessId/ProcessName/ControlId) — fast, synchronous, no UIA involved
		IEnumerable<TopLevelWindowItem> FindWindows(WindowSearchCriteria criteria);

		TopLevelWindowItem GetWindowItem(IntPtr handle);

		// the real Win32 parent of every top-level window (GetDesktopWindow) — has its own handle/class/rect like any other window
		TopLevelWindowItem GetDesktopWindowInfo();

		IEnumerable<TopLevelWindowItem> GetChildWindowInfos(IntPtr parenthandle);

		// owned windows (modal dialogs, tooltips, IME windows) are not true WS_CHILD descendants, kept distinct so the tree can group them separately
		IEnumerable<TopLevelWindowItem> GetOwnedWindowInfos(IntPtr parenthandle);

		AutomationElement CreateElementFromHandle(IntPtr handle);

		IEnumerable<AutomationElement> GetChildren(AutomationElement parent);

		IEnumerable<PropertyItem> GetNativeWindowDetails(IntPtr handle);

		// cheap existence check (IsWindow) — used to poll whether an inspected window has been closed
		bool IsWindowValid(IntPtr handle);

		// currently active top-level window handle (GetForegroundWindow); used to detect a click that leaked through a UIPI-blocked low-level hook
		IntPtr GetForegroundWindowHandle();

		// rect/style/exstyle/thread-id only — cheap enough to call every hover-poll tick, unlike GetNativeWindowDetails
		WindowQuickInfo GetWindowQuickInfo(IntPtr handle);

		AutomationElement GetElementAtScreenPoint(System.Drawing.Point point);

		// walks the UIA parent chain (RawView, matching GetChildren's TrueCondition scope), returned top-down (root's direct child first, element last)
		IReadOnlyList<AutomationElement> GetElementAncestorChainToRoot(AutomationElement element, AutomationElement rootelement);

		// unlike GetWindowAtScreenPoint (WindowFromPoint), this does not skip disabled windows, so it still finds the target while it's showing its own modal dialog
		TopLevelWindowItem GetDescendantAtScreenPointIncludingDisabled(IntPtr roothandle, System.Drawing.Point point);
	}
}
