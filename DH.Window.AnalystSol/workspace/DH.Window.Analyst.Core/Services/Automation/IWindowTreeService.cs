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
		// Win32-based, no UIA, returns immediately
		IEnumerable<TopLevelWindowItem> GetTopLevelWindowInfos();

		// resolves the top-level window under the given screen point (Finder Tool); null if none found
		TopLevelWindowItem GetTopLevelWindowAtScreenPoint(System.Drawing.Point point);

		// raw WindowFromPoint result under the given screen point, NOT resolved up to its top-level ancestor (Show Info on Mouse); null if none found
		TopLevelWindowItem GetWindowAtScreenPoint(System.Drawing.Point point);

		// walks the parent chain from handle up to (but not including) roothandle, returning it top-down (root's direct child first, handle last);
		// empty if handle is not actually a descendant of roothandle
		IReadOnlyList<IntPtr> GetAncestorChainToRoot(IntPtr handle, IntPtr roothandle);

		// screen-space bounds of the given window, for drag-feedback highlighting
		System.Drawing.Rectangle GetWindowScreenRect(IntPtr handle);

		// restores (if minimized) and brings the given top-level window to the foreground
		void ActivateWindow(IntPtr handle);

		// resolves the top-level ancestor HWND of any window (top-level or descendant); returns the handle itself if already top-level
		IntPtr GetTopLevelAncestorHandle(IntPtr handle);

		// if handlefilter is non-zero it takes priority (exact match, no tree walk); otherwise searches every window for a caption/classname substring match, case-insensitive
		IEnumerable<TopLevelWindowItem> FindWindows(string strcaption, string strclassname, IntPtr handlefilter);

		// Win32-level criteria only (Caption/ClassName/Handle/ProcessId/ProcessName/ControlId) — fast, synchronous, no UIA involved
		IEnumerable<TopLevelWindowItem> FindWindows(WindowSearchCriteria criteria);

		// builds a TopLevelWindowItem for any single handle (top-level or descendant); null if the handle is no longer valid
		TopLevelWindowItem GetWindowItem(IntPtr handle);

		// the real Win32 parent of every top-level window (GetDesktopWindow) — has its own handle/class/rect like any other window
		TopLevelWindowItem GetDesktopWindowInfo();

		// immediate child windows only (Win32 native hierarchy), for the Native Window tree
		IEnumerable<TopLevelWindowItem> GetChildWindowInfos(IntPtr parenthandle);

		// converts a single selected window only; null on failure
		AutomationElement CreateElementFromHandle(IntPtr handle);

		IEnumerable<AutomationElement> GetChildren(AutomationElement parent);

		// low-level Win32 details (rect/proc/instance/menu/...) for the single selected window only
		IEnumerable<PropertyItem> GetNativeWindowDetails(IntPtr handle);

		// cheap existence check (IsWindow) — used to poll whether an inspected window has been closed
		bool IsWindowValid(IntPtr handle);

		// currently active top-level window handle (GetForegroundWindow); used to detect a click that leaked through a UIPI-blocked low-level hook
		IntPtr GetForegroundWindowHandle();

		// rect/style/exstyle/thread-id only — cheap enough to call every hover-poll tick, unlike GetNativeWindowDetails
		WindowQuickInfo GetWindowQuickInfo(IntPtr handle);
	}
}
