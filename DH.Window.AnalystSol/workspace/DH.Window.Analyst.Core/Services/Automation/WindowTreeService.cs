//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public class WindowTreeService : IWindowTreeService {
		// icon extraction cached per exe path (many windows share the same process)
		private readonly Dictionary<string, ImageSource> m_dicIconCache = new Dictionary<string, ImageSource>();

		// WS_* bit flags, checked high-to-low so combo flags (e.g. WS_CAPTION) print before their component bits
		private static readonly List<KeyValuePair<uint, string>> s_listStyleFlags = new List<KeyValuePair<uint, string>> {
			new KeyValuePair<uint, string>(0x80000000, "WS_POPUP"),
			new KeyValuePair<uint, string>(0x40000000, "WS_CHILD"),
			new KeyValuePair<uint, string>(0x20000000, "WS_MINIMIZE"),
			new KeyValuePair<uint, string>(0x10000000, "WS_VISIBLE"),
			new KeyValuePair<uint, string>(0x08000000, "WS_DISABLED"),
			new KeyValuePair<uint, string>(0x04000000, "WS_CLIPSIBLINGS"),
			new KeyValuePair<uint, string>(0x02000000, "WS_CLIPCHILDREN"),
			new KeyValuePair<uint, string>(0x01000000, "WS_MAXIMIZE"),
			new KeyValuePair<uint, string>(0x00C00000, "WS_CAPTION"),
			new KeyValuePair<uint, string>(0x00800000, "WS_BORDER"),
			new KeyValuePair<uint, string>(0x00400000, "WS_DLGFRAME"),
			new KeyValuePair<uint, string>(0x00200000, "WS_VSCROLL"),
			new KeyValuePair<uint, string>(0x00100000, "WS_HSCROLL"),
			new KeyValuePair<uint, string>(0x00080000, "WS_SYSMENU"),
			new KeyValuePair<uint, string>(0x00040000, "WS_THICKFRAME"),
			new KeyValuePair<uint, string>(0x00020000, "WS_GROUP / WS_MINIMIZEBOX"),
			new KeyValuePair<uint, string>(0x00010000, "WS_TABSTOP / WS_MAXIMIZEBOX"),
		};

		private static readonly List<KeyValuePair<uint, string>> s_listExStyleFlags = new List<KeyValuePair<uint, string>> {
			new KeyValuePair<uint, string>(0x08000000, "WS_EX_NOACTIVATE"),
			new KeyValuePair<uint, string>(0x02000000, "WS_EX_COMPOSITED"),
			new KeyValuePair<uint, string>(0x00400000, "WS_EX_LAYOUTRTL"),
			new KeyValuePair<uint, string>(0x00200000, "WS_EX_NOREDIRECTIONBITMAP"),
			new KeyValuePair<uint, string>(0x00100000, "WS_EX_NOINHERITLAYOUT"),
			new KeyValuePair<uint, string>(0x00080000, "WS_EX_LAYERED"),
			new KeyValuePair<uint, string>(0x00040000, "WS_EX_APPWINDOW"),
			new KeyValuePair<uint, string>(0x00020000, "WS_EX_STATICEDGE"),
			new KeyValuePair<uint, string>(0x00010000, "WS_EX_CONTROLPARENT"),
			new KeyValuePair<uint, string>(0x00004000, "WS_EX_LEFTSCROLLBAR"),
			new KeyValuePair<uint, string>(0x00002000, "WS_EX_RTLREADING"),
			new KeyValuePair<uint, string>(0x00001000, "WS_EX_RIGHT"),
			new KeyValuePair<uint, string>(0x00000400, "WS_EX_CONTEXTHELP"),
			new KeyValuePair<uint, string>(0x00000200, "WS_EX_CLIENTEDGE"),
			new KeyValuePair<uint, string>(0x00000100, "WS_EX_WINDOWEDGE"),
			new KeyValuePair<uint, string>(0x00000080, "WS_EX_TOOLWINDOW"),
			new KeyValuePair<uint, string>(0x00000040, "WS_EX_MDICHILD"),
			new KeyValuePair<uint, string>(0x00000020, "WS_EX_TRANSPARENT"),
			new KeyValuePair<uint, string>(0x00000010, "WS_EX_ACCEPTFILES"),
			new KeyValuePair<uint, string>(0x00000008, "WS_EX_TOPMOST"),
			new KeyValuePair<uint, string>(0x00000004, "WS_EX_NOPARENTNOTIFY"),
			new KeyValuePair<uint, string>(0x00000001, "WS_EX_DLGMODALFRAME"),
		};

		// CS_* class style bits (WNDCLASSEX.style), checked high-to-low
		private static readonly List<KeyValuePair<uint, string>> s_listClassStyleFlags = new List<KeyValuePair<uint, string>> {
			new KeyValuePair<uint, string>(0x00020000, "CS_DROPSHADOW"),
			new KeyValuePair<uint, string>(0x00004000, "CS_GLOBALCLASS"),
			new KeyValuePair<uint, string>(0x00002000, "CS_BYTEALIGNWINDOW"),
			new KeyValuePair<uint, string>(0x00001000, "CS_BYTEALIGNCLIENT"),
			new KeyValuePair<uint, string>(0x00000800, "CS_SAVEBITS"),
			new KeyValuePair<uint, string>(0x00000200, "CS_NOCLOSE"),
			new KeyValuePair<uint, string>(0x00000080, "CS_PARENTDC"),
			new KeyValuePair<uint, string>(0x00000040, "CS_CLASSDC"),
			new KeyValuePair<uint, string>(0x00000020, "CS_OWNDC"),
			new KeyValuePair<uint, string>(0x00000008, "CS_DBLCLKS"),
			new KeyValuePair<uint, string>(0x00000002, "CS_HREDRAW"),
			new KeyValuePair<uint, string>(0x00000001, "CS_VREDRAW"),
		};

		// Win32 EnumWindows only, no UIA, returns immediately
		public IEnumerable<TopLevelWindowItem> GetTopLevelWindowInfos() {
			List<TopLevelWindowItem> listitems = new List<TopLevelWindowItem>();

			NativeMethods.EnumWindows((hwnd, lparam) => {
				if (NativeMethods.IsWindowVisible(hwnd) == true && NativeMethods.GetWindow(hwnd, NativeMethods.GW_OWNER) == IntPtr.Zero) {
					if (NativeMethods.GetWindowTextLength(hwnd) > 0) {
						listitems.Add(BuildWindowItem(hwnd));
					}
				}
				return true;
			}, IntPtr.Zero);

			return listitems;
		}

		// the real Win32 parent of every top-level window — a genuine HWND with its own handle/class/rect
		public TopLevelWindowItem GetDesktopWindowInfo() {
			return BuildWindowItem(NativeMethods.GetDesktopWindow());
		}

		public bool IsWindowValid(IntPtr handle) {
			return NativeMethods.IsWindow(handle) == true;
		}

		public IntPtr GetForegroundWindowHandle() {
			return NativeMethods.GetForegroundWindow();
		}

		// immediate children only (GW_CHILD + GW_HWNDNEXT siblings), matches lazy tree loading
		public IEnumerable<TopLevelWindowItem> GetChildWindowInfos(IntPtr parenthandle) {
			List<TopLevelWindowItem> listitems = new List<TopLevelWindowItem>();

			IntPtr hchild = NativeMethods.GetWindow(parenthandle, NativeMethods.GW_CHILD);
			while (hchild != IntPtr.Zero) {
				listitems.Add(BuildWindowItem(hchild));
				hchild = NativeMethods.GetWindow(hchild, NativeMethods.GW_HWNDNEXT);
			}

			return listitems;
		}

		// resolves the top-level window under the given screen point (Finder Tool), skipping this process's own windows
		public TopLevelWindowItem GetTopLevelWindowAtScreenPoint(System.Drawing.Point point) {
			IntPtr hwnd = NativeMethods.WindowFromPoint(new NativeMethods.POINT { X = point.X, Y = point.Y });
			if (hwnd == IntPtr.Zero) {
				return null;
			}

			IntPtr hroot = NativeMethods.GetAncestor(hwnd, NativeMethods.GA_ROOT);
			if (hroot == IntPtr.Zero) {
				hroot = hwnd;
			}

			NativeMethods.GetWindowThreadProcessId(hroot, out uint nprocessid);
			if (nprocessid == (uint) Process.GetCurrentProcess().Id) {
				return null;
			}

			return BuildWindowItem(hroot);
		}

		// raw WindowFromPoint result under the given screen point (Show Info on Mouse), NOT resolved up to GA_ROOT like GetTopLevelWindowAtScreenPoint — the caller needs the exact descendant that was hit
		public TopLevelWindowItem GetWindowAtScreenPoint(System.Drawing.Point point) {
			IntPtr hwnd = NativeMethods.WindowFromPoint(new NativeMethods.POINT { X = point.X, Y = point.Y });
			if (hwnd == IntPtr.Zero) {
				return null;
			}

			NativeMethods.GetWindowThreadProcessId(hwnd, out uint nprocessid);
			if (nprocessid == (uint) Process.GetCurrentProcess().Id) {
				return null;
			}

			return BuildWindowItem(hwnd);
		}

		// top-down parent chain from roothandle's direct child down to handle; empty if handle never reaches roothandle (256-hop safety cap against parent-chain cycles/corruption)
		public IReadOnlyList<IntPtr> GetAncestorChainToRoot(IntPtr handle, IntPtr roothandle) {
			List<IntPtr> listchain = new List<IntPtr>();

			IntPtr hcurrent = handle;
			int nhopcount = 0;
			while (hcurrent != IntPtr.Zero && hcurrent != roothandle && nhopcount < 256) {
				listchain.Insert(0, hcurrent);
				hcurrent = NativeMethods.GetParent(hcurrent);
				nhopcount++;
			}

			return hcurrent == roothandle ? listchain : new List<IntPtr>();
		}

		// screen-space bounds of the given window, for drag-feedback highlighting
		public Rectangle GetWindowScreenRect(IntPtr handle) {
			if (NativeMethods.GetWindowRect(handle, out NativeMethods.RECT rect) == false) {
				return Rectangle.Empty;
			}

			return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
		}

		// rect/style/exstyle/thread-id only — cheap enough to call every hover-poll tick, unlike GetNativeWindowDetails
		public WindowQuickInfo GetWindowQuickInfo(IntPtr handle) {
			NativeMethods.GetWindowRect(handle, out NativeMethods.RECT rect);
			uint nstyle = (uint) NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWL_STYLE).ToInt64();
			uint nexstyle = (uint) NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWL_EXSTYLE).ToInt64();
			uint nthreadid = NativeMethods.GetWindowThreadProcessId(handle, out uint _);

			return new WindowQuickInfo(new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top), nstyle, nexstyle, (int) nthreadid);
		}

		// restores (if minimized) and brings the given top-level window to the foreground
		public void ActivateWindow(IntPtr handle) {
			if (NativeMethods.IsIconic(handle) == true) {
				NativeMethods.ShowWindow(handle, NativeMethods.SW_RESTORE);
			}
			NativeMethods.SetForegroundWindow(handle);
		}

		// resolves the top-level ancestor HWND of any window (top-level or descendant); follows both the parent AND owner chain
		// (GA_ROOTOWNER, not just GA_ROOT) so owned popups with no real parent — tooltips, MSCTFIME UI/Default IME windows — resolve
		// up to the actual application window that owns them, instead of being treated as unrelated top-level windows of their own
		public IntPtr GetTopLevelAncestorHandle(IntPtr handle) {
			IntPtr hroot = NativeMethods.GetAncestor(handle, NativeMethods.GA_ROOTOWNER);
			return hroot != IntPtr.Zero ? hroot : handle;
		}

		// exact-handle search short-circuits to a single IsWindow check; otherwise walks every window (top-level + descendants) matching caption/classname substrings, case-insensitive
		public IEnumerable<TopLevelWindowItem> FindWindows(string strcaption, string strclassname, IntPtr handlefilter) {
			return FindWindows(new WindowSearchCriteria { Caption = strcaption, ClassName = strclassname, Handle = handlefilter });
		}

		// Win32-level criteria only (Caption/ClassName/Handle/ProcessId/ProcessName/ControlId) — fast, synchronous, no UIA involved;
		// AutomationId/UiaName/ControlType on the criteria are ignored here, callers needing those must layer IUiaSearchService on top of this result
		public IEnumerable<TopLevelWindowItem> FindWindows(WindowSearchCriteria criteria) {
			List<TopLevelWindowItem> listmatches = new List<TopLevelWindowItem>();

			if (criteria.Handle != IntPtr.Zero) {
				if (NativeMethods.IsWindow(criteria.Handle) == true) {
					listmatches.Add(BuildWindowItem(criteria.Handle));
				}
				return listmatches;
			}

			NativeMethods.EnumWindows((hwnd, lparam) => {
				CollectMatchingWindows(hwnd, criteria, listmatches);
				return true;
			}, IntPtr.Zero);

			return listmatches;
		}

		private void CollectMatchingWindows(IntPtr hwnd, WindowSearchCriteria criteria, List<TopLevelWindowItem> listmatches) {
			if (IsWindowMatch(hwnd, criteria) == true) {
				listmatches.Add(BuildWindowItem(hwnd));
			}

			IntPtr hchild = NativeMethods.GetWindow(hwnd, NativeMethods.GW_CHILD);
			while (hchild != IntPtr.Zero) {
				CollectMatchingWindows(hchild, criteria, listmatches);
				hchild = NativeMethods.GetWindow(hchild, NativeMethods.GW_HWNDNEXT);
			}
		}

		private static bool IsWindowMatch(IntPtr hwnd, WindowSearchCriteria criteria) {
			if (string.IsNullOrEmpty(criteria.Caption) == false) {
				int nlength = NativeMethods.GetWindowTextLength(hwnd);
				StringBuilder sbtitle = new StringBuilder(nlength + 1);
				if (nlength > 0) {
					NativeMethods.GetWindowText(hwnd, sbtitle, sbtitle.Capacity);
				}
				if (sbtitle.ToString().IndexOf(criteria.Caption, StringComparison.OrdinalIgnoreCase) < 0) {
					return false;
				}
			}

			if (string.IsNullOrEmpty(criteria.ClassName) == false) {
				StringBuilder sbclass = new StringBuilder(256);
				NativeMethods.GetClassName(hwnd, sbclass, sbclass.Capacity);
				if (sbclass.ToString().IndexOf(criteria.ClassName, StringComparison.OrdinalIgnoreCase) < 0) {
					return false;
				}
			}

			if (criteria.ControlId.HasValue == true) {
				int ncontrolid = NativeMethods.GetWindowLongPtr(hwnd, NativeMethods.GWL_ID).ToInt32();
				if (ncontrolid != criteria.ControlId.Value) {
					return false;
				}
			}

			if (criteria.ProcessId.HasValue == true || string.IsNullOrEmpty(criteria.ProcessName) == false) {
				NativeMethods.GetWindowThreadProcessId(hwnd, out uint nprocessid);

				if (criteria.ProcessId.HasValue == true && (int) nprocessid != criteria.ProcessId.Value) {
					return false;
				}

				if (string.IsNullOrEmpty(criteria.ProcessName) == false && IsProcessNameMatch((int) nprocessid, criteria.ProcessName) == false) {
					return false;
				}
			}

			return true;
		}

		private static bool IsProcessNameMatch(int nprocessid, string strprocessname) {
			try {
				using (Process process = Process.GetProcessById(nprocessid)) {
					return process.ProcessName.IndexOf(strprocessname, StringComparison.OrdinalIgnoreCase) >= 0;
				}
			}
			catch (Exception ex) {
				AppLog.d($"IsProcessNameMatch failed for PID {nprocessid}: {ex.Message}");
				return false;
			}
		}

		// builds a TopLevelWindowItem for any single handle (top-level or descendant); null if the handle is no longer valid
		public TopLevelWindowItem GetWindowItem(IntPtr handle) {
			return NativeMethods.IsWindow(handle) == true ? BuildWindowItem(handle) : null;
		}

		private TopLevelWindowItem BuildWindowItem(IntPtr hwnd) {
			int nlength = NativeMethods.GetWindowTextLength(hwnd);
			StringBuilder sbtitle = new StringBuilder(nlength + 1);
			if (nlength > 0) {
				NativeMethods.GetWindowText(hwnd, sbtitle, sbtitle.Capacity);
			}

			StringBuilder sbclassname = new StringBuilder(256);
			NativeMethods.GetClassName(hwnd, sbclassname, sbclassname.Capacity);

			NativeMethods.GetWindowThreadProcessId(hwnd, out uint nprocessid);
			string strprocessname = GetProcessName((int) nprocessid, out ImageSource icon);

			return new TopLevelWindowItem(hwnd, sbtitle.ToString(), sbclassname.ToString(), strprocessname, (int) nprocessid, icon);
		}

		private string GetProcessName(int nprocessid, out ImageSource icon) {
			icon = null;

			try {
				using (Process process = Process.GetProcessById(nprocessid)) {
					// process.ProcessName resolves via a toolhelp snapshot and needs no handle access, so it still works against an elevated target; MainModule (icon only) is best-effort separately since it needs broader access
					string strprocessname = process.ProcessName;

					try {
						string strexepath = process.MainModule?.FileName;
						if (string.IsNullOrEmpty(strexepath) == false) {
							icon = GetCachedIcon(strexepath);
						}
					}
					catch (Exception ex) {
						AppLog.d($"GetProcessName: MainModule access denied for PID {nprocessid}: {ex.Message}");
					}

					return strprocessname;
				}
			}
			catch (Exception ex) {
				AppLog.d($"GetProcessName failed for PID {nprocessid}: {ex.Message}");
				return "(Unknown)";
			}
		}

		private ImageSource GetCachedIcon(string strexepath) {
			if (m_dicIconCache.TryGetValue(strexepath, out ImageSource cachedicon) == true) {
				return cachedicon;
			}

			ImageSource icon = null;

			try {
				using (Icon rawicon = Icon.ExtractAssociatedIcon(strexepath)) {
					if (rawicon != null) {
						icon = Imaging.CreateBitmapSourceFromHIcon(rawicon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
						icon.Freeze();
					}
				}
			}
			catch (Exception ex) {
				// icon extraction failure is non-fatal
				AppLog.d($"GetCachedIcon failed for {strexepath}: {ex.Message}");
			}

			m_dicIconCache[strexepath] = icon;
			return icon;
		}

		// UIA conversion for one user-selected window only
		public AutomationElement CreateElementFromHandle(IntPtr handle) {
			try {
				return AutomationElement.FromHandle(handle);
			}
			catch (Exception ex) {
				AppLog.w($"CreateElementFromHandle failed for handle 0x{handle.ToInt64():X}: {ex.Message}");
				return null;
			}
		}

		// low-level Win32 details for the single selected window only (Spy++ "General" tab style)
		public IEnumerable<PropertyItem> GetNativeWindowDetails(IntPtr handle) {
			List<PropertyItem> listitems = new List<PropertyItem>();

			NativeMethods.GetWindowRect(handle, out NativeMethods.RECT windowrect);
			listitems.Add(new PropertyItem("Window Rect", FormatRect(windowrect)));

			NativeMethods.GetClientRect(handle, out NativeMethods.RECT clientrect);
			listitems.Add(new PropertyItem("Client Rect", FormatRect(clientrect)));

			NativeMethods.WINDOWPLACEMENT placement = new NativeMethods.WINDOWPLACEMENT();
			placement.Length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
			string strrestorerect = NativeMethods.GetWindowPlacement(handle, ref placement) == true
				? FormatRect(placement.NormalPosition)
				: "(Unavailable)";
			listitems.Add(new PropertyItem("Restore Rect", strrestorerect));

			listitems.Add(new PropertyItem("Window State", FormatWindowState(placement.ShowCmd, NativeMethods.IsWindowVisible(handle))));

			uint nstyle = (uint) NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWL_STYLE).ToInt64();
			listitems.Add(AddFlagsItem("Style", nstyle, s_listStyleFlags));

			uint nexstyle = (uint) NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWL_EXSTYLE).ToInt64();
			listitems.Add(AddFlagsItem("ExStyle", nexstyle, s_listExStyleFlags));

			listitems.Add(new PropertyItem("Window Proc", FormatPointer(NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWL_WNDPROC))));
			listitems.Add(new PropertyItem("Instance", FormatPointer(NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWLP_HINSTANCE))));

			IntPtr hmenu = NativeMethods.GetMenu(handle);
			listitems.Add(new PropertyItem("Menu", hmenu == IntPtr.Zero ? "(None)" : FormatPointer(hmenu)));

			listitems.Add(new PropertyItem("User Data", FormatPointer(NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWLP_USERDATA))));

			IntPtr nwndextra = NativeMethods.GetClassLongPtr(handle, NativeMethods.GCL_CBWNDEXTRA);
			listitems.Add(new PropertyItem("Window Bytes", nwndextra.ToInt64() + " bytes"));

			// window relationship (Spy++ "Windows" tab style) — NavigationHandle lets the UI jump to the referenced window
			listitems.Add(AddWindowRefItem("Parent", NativeMethods.GetParent(handle)));
			listitems.Add(AddWindowRefItem("Owner", NativeMethods.GetWindow(handle, NativeMethods.GW_OWNER)));
			listitems.Add(AddWindowRefItem("First Child", NativeMethods.GetWindow(handle, NativeMethods.GW_CHILD)));
			listitems.Add(AddWindowRefItem("Next Window (Z-Order)", NativeMethods.GetWindow(handle, NativeMethods.GW_HWNDNEXT)));
			listitems.Add(AddWindowRefItem("Previous Window (Z-Order)", NativeMethods.GetWindow(handle, NativeMethods.GW_HWNDPREV)));

			// via GetClassInfoEx, not GetClassLongPtr, since GCL_MENUNAME/GCL_WNDPROC/GCL_HMODULE are unsupported on 64-bit through GetClassLongPtr
			StringBuilder sbclassname = new StringBuilder(256);
			NativeMethods.GetClassName(handle, sbclassname, sbclassname.Capacity);

			NativeMethods.WNDCLASSEX wndclass = new NativeMethods.WNDCLASSEX();
			wndclass.cbSize = (uint) Marshal.SizeOf(typeof(NativeMethods.WNDCLASSEX));
			IntPtr hinstance = NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWLP_HINSTANCE);

			if (NativeMethods.GetClassInfoEx(hinstance, sbclassname.ToString(), ref wndclass) == true) {
				listitems.Add(new PropertyItem("Class Name", sbclassname.ToString()));
				listitems.Add(AddFlagsItem("Class Style", wndclass.style, s_listClassStyleFlags));
				listitems.Add(new PropertyItem("Class Extra Bytes", wndclass.cbClsExtra + " bytes"));
				listitems.Add(new PropertyItem("hIcon", wndclass.hIcon == IntPtr.Zero ? "(None)" : FormatPointer(wndclass.hIcon)));
				listitems.Add(new PropertyItem("hIconSm", wndclass.hIconSm == IntPtr.Zero ? "(None)" : FormatPointer(wndclass.hIconSm)));
				listitems.Add(new PropertyItem("hCursor", wndclass.hCursor == IntPtr.Zero ? "(None)" : FormatPointer(wndclass.hCursor)));
				listitems.Add(new PropertyItem("hbrBackground", wndclass.hbrBackground == IntPtr.Zero ? "(None)" : FormatPointer(wndclass.hbrBackground)));
				listitems.Add(new PropertyItem("Menu Name", FormatClassNamePointer(wndclass.lpszMenuName)));
			}

			return listitems;
		}

		// resolves the given related-window handle to a short "handle  class  \"title\"" description, or "(None)"
		private static PropertyItem AddWindowRefItem(string strname, IntPtr hwndref) {
			return new PropertyItem(strname, FormatWindowRef(hwndref), hwndref == IntPtr.Zero ? (IntPtr?) null : hwndref);
		}

		private static string FormatWindowRef(IntPtr hwndref) {
			if (hwndref == IntPtr.Zero) {
				return "(None)";
			}

			StringBuilder sbclassname = new StringBuilder(256);
			NativeMethods.GetClassName(hwndref, sbclassname, sbclassname.Capacity);

			int nlength = NativeMethods.GetWindowTextLength(hwndref);
			StringBuilder sbtitle = new StringBuilder(nlength + 1);
			if (nlength > 0) {
				NativeMethods.GetWindowText(hwndref, sbtitle, sbtitle.Capacity);
			}

			string strtitle = sbtitle.Length > 0 ? $"  \"{sbtitle}\"" : string.Empty;
			return $"{FormatPointer(hwndref)}  {sbclassname}{strtitle}";
		}

		// Hidden takes precedence over ShowCmd (a minimized/maximized window can still be !IsWindowVisible) since users care most about "will I actually see this" first
		private static string FormatWindowState(int nshowcmd, bool bisvisible) {
			if (bisvisible == false) {
				return "Hidden";
			}

			switch (nshowcmd) {
				case NativeMethods.SW_SHOWMAXIMIZED:
					return "Maximized";
				case NativeMethods.SW_SHOWMINIMIZED:
					return "Minimized";
				default:
					return "Normal";
			}
		}

		private static string FormatRect(NativeMethods.RECT rect) {
			return $"({rect.Left}, {rect.Top}) - ({rect.Right}, {rect.Bottom})  {rect.Right - rect.Left}x{rect.Bottom - rect.Top}";
		}

		private static string FormatPointer(IntPtr pointer) {
			return "0x" + pointer.ToInt64().ToString(IntPtr.Size == 8 ? "X16" : "X8");
		}

		// a class can register this as a MAKEINTRESOURCE integer (low word only) instead of a real string pointer, so treat any value that isn't a plausible pointer as absent rather than dereferencing it
		private static string FormatClassNamePointer(IntPtr pointer) {
			if (pointer.ToInt64() <= 0xFFFF) {
				return "(None)";
			}

			string strvalue = Marshal.PtrToStringAuto(pointer);
			return string.IsNullOrEmpty(strvalue) == false ? strvalue : "(None)";
		}

		// Spy++ "Styles" tab style: hex value followed by the symbolic names of every set bit, high-to-low
		private static string FormatFlags(uint nvalue, List<KeyValuePair<uint, string>> listflags) {
			List<string> listnames = ResolveFlagNames(nvalue, listflags);
			string strnames = listnames.Count > 0 ? string.Join(" | ", listnames) : "(none)";
			return $"0x{nvalue:X8}  {strnames}";
		}

		// one "0x00000008  WS_VISIBLE" line per individually named set bit, for the Style/ExStyle detail popup
		private static PropertyItem AddFlagsItem(string strname, uint nvalue, List<KeyValuePair<uint, string>> listflags) {
			List<string> listnames = ResolveFlagNames(nvalue, listflags);

			List<string> listdetails = new List<string>();
			foreach (KeyValuePair<uint, string> flag in listflags) {
				if (listnames.Contains(flag.Value) == true) {
					listdetails.Add($"0x{flag.Key:X8}  {flag.Value}");
				}
			}

			string strnames = listnames.Count > 0 ? string.Join(" | ", listnames) : "(none)";
			return new PropertyItem(strname, $"0x{nvalue:X8}  {strnames}", null, listdetails.Count > 0 ? listdetails : null);
		}

		private static List<string> ResolveFlagNames(uint nvalue, List<KeyValuePair<uint, string>> listflags) {
			List<string> listnames = new List<string>();
			uint nremaining = nvalue;

			foreach (KeyValuePair<uint, string> flag in listflags) {
				if ((nremaining & flag.Key) == flag.Key) {
					listnames.Add(flag.Value);
					nremaining &= ~flag.Key;
				}
			}

			return listnames;
		}

		public IEnumerable<AutomationElement> GetChildren(AutomationElement parent) {
			List<AutomationElement> listchildren = new List<AutomationElement>();

			try {
				AutomationElementCollection colchildren = parent.FindAll(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);

				foreach (AutomationElement element in colchildren) {
					listchildren.Add(element);
				}
			}
			catch (Exception ex) {
				// unresponsive/restricted elements yield an empty list
				AppLog.w($"GetChildren failed: {ex.Message}");
			}

			return listchildren;
		}
	}
}
