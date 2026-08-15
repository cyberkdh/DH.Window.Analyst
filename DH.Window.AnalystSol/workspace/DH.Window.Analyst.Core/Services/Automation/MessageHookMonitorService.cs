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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	// unlike WinEventMonitorService (callbacks land in our own process, no injection needed), WH_CALLWNDPROC/WH_GETMESSAGE require a DLL physically mapped into the target thread's process, which relays messages back here via WM_COPYDATA
	public class MessageHookMonitorService : IMessageHookMonitorService {
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool SetReportTargetDelegate(IntPtr hwndreporttarget, uint ndwdataid);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void StopAndJoinSenderThreadDelegate(uint ndwtimeoutms);

		// generous margin over the native sender thread's 50ms poll interval; it only needs to wake once and observe g_lRunning==0 to exit
		private const uint HOOKNATIVE_JOIN_TIMEOUT_MS = 2000;

		private static readonly Dictionary<uint, string> s_dicMessageNames = new Dictionary<uint, string> {
			{ 0x0001, "WM_CREATE" }, { 0x0002, "WM_DESTROY" }, { 0x0003, "WM_MOVE" }, { 0x0005, "WM_SIZE" },
			{ 0x0006, "WM_ACTIVATE" }, { 0x0007, "WM_SETFOCUS" }, { 0x0008, "WM_KILLFOCUS" }, { 0x000A, "WM_ENABLE" },
			{ 0x000B, "WM_SETREDRAW" }, { 0x000C, "WM_SETTEXT" }, { 0x000D, "WM_GETTEXT" }, { 0x000E, "WM_GETTEXTLENGTH" },
			{ 0x000F, "WM_PAINT" }, { 0x0010, "WM_CLOSE" }, { 0x0011, "WM_QUERYENDSESSION" }, { 0x0012, "WM_QUIT" },
			{ 0x0013, "WM_QUERYOPEN" }, { 0x0014, "WM_ERASEBKGND" }, { 0x0015, "WM_SYSCOLORCHANGE" }, { 0x0018, "WM_SHOWWINDOW" },
			{ 0x001A, "WM_WININICHANGE" }, { 0x001C, "WM_ACTIVATEAPP" }, { 0x001D, "WM_FONTCHANGE" }, { 0x001E, "WM_TIMECHANGE" },
			{ 0x001F, "WM_CANCELMODE" }, { 0x0020, "WM_SETCURSOR" }, { 0x0021, "WM_MOUSEACTIVATE" }, { 0x0022, "WM_CHILDACTIVATE" },
			{ 0x0023, "WM_QUEUESYNC" }, { 0x0024, "WM_GETMINMAXINFO" }, { 0x0026, "WM_PAINTICON" }, { 0x0027, "WM_ICONERASEBKGND" },
			{ 0x0028, "WM_NEXTDLGCTL" }, { 0x002B, "WM_DRAWITEM" }, { 0x002C, "WM_MEASUREITEM" }, { 0x002D, "WM_DELETEITEM" },
			{ 0x0030, "WM_SETFONT" }, { 0x0031, "WM_GETFONT" }, { 0x0032, "WM_SETHOTKEY" }, { 0x0033, "WM_GETHOTKEY" },
			{ 0x0037, "WM_QUERYDRAGICON" }, { 0x0039, "WM_COMPAREITEM" }, { 0x003D, "WM_GETOBJECT" }, { 0x0046, "WM_WINDOWPOSCHANGING" },
			{ 0x0047, "WM_WINDOWPOSCHANGED" }, { 0x004A, "WM_COPYDATA" }, { 0x004E, "WM_NOTIFY" }, { 0x0050, "WM_INPUTLANGCHANGEREQUEST" },
			{ 0x0051, "WM_INPUTLANGCHANGE" }, { 0x0053, "WM_HELP" }, { 0x0054, "WM_USERCHANGED" }, { 0x0055, "WM_NOTIFYFORMAT" },
			{ 0x007B, "WM_CONTEXTMENU" }, { 0x007C, "WM_STYLECHANGING" }, { 0x007D, "WM_STYLECHANGED" }, { 0x007E, "WM_DISPLAYCHANGE" },
			{ 0x007F, "WM_GETICON" }, { 0x0080, "WM_SETICON" }, { 0x0081, "WM_NCCREATE" }, { 0x0082, "WM_NCDESTROY" },
			{ 0x0083, "WM_NCCALCSIZE" }, { 0x0084, "WM_NCHITTEST" }, { 0x0085, "WM_NCPAINT" }, { 0x0086, "WM_NCACTIVATE" },
			{ 0x0087, "WM_GETDLGCODE" }, { 0x00A0, "WM_NCMOUSEMOVE" }, { 0x00A1, "WM_NCLBUTTONDOWN" }, { 0x00A2, "WM_NCLBUTTONUP" },
			{ 0x00A4, "WM_NCRBUTTONDOWN" }, { 0x00A5, "WM_NCRBUTTONUP" }, { 0x0100, "WM_KEYDOWN" }, { 0x0101, "WM_KEYUP" },
			{ 0x0102, "WM_CHAR" }, { 0x0103, "WM_DEADCHAR" }, { 0x0104, "WM_SYSKEYDOWN" }, { 0x0105, "WM_SYSKEYUP" },
			{ 0x0106, "WM_SYSCHAR" }, { 0x0111, "WM_COMMAND" }, { 0x0112, "WM_SYSCOMMAND" }, { 0x0113, "WM_TIMER" },
			{ 0x0114, "WM_HSCROLL" }, { 0x0115, "WM_VSCROLL" }, { 0x0116, "WM_INITMENU" }, { 0x0117, "WM_INITMENUPOPUP" },
			{ 0x011F, "WM_MENUSELECT" }, { 0x0120, "WM_MENUCHAR" }, { 0x0121, "WM_ENTERIDLE" }, { 0x0127, "WM_CHANGEUISTATE" },
			{ 0x0128, "WM_UPDATEUISTATE" }, { 0x0129, "WM_QUERYUISTATE" }, { 0x0132, "WM_CTLCOLORMSGBOX" }, { 0x0133, "WM_CTLCOLOREDIT" },
			{ 0x0134, "WM_CTLCOLORLISTBOX" }, { 0x0135, "WM_CTLCOLORBTN" }, { 0x0136, "WM_CTLCOLORDLG" }, { 0x0137, "WM_CTLCOLORSCROLLBAR" },
			{ 0x0138, "WM_CTLCOLORSTATIC" }, { 0x0200, "WM_MOUSEMOVE" }, { 0x0201, "WM_LBUTTONDOWN" }, { 0x0202, "WM_LBUTTONUP" },
			{ 0x0203, "WM_LBUTTONDBLCLK" }, { 0x0204, "WM_RBUTTONDOWN" }, { 0x0205, "WM_RBUTTONUP" }, { 0x0206, "WM_RBUTTONDBLCLK" },
			{ 0x0207, "WM_MBUTTONDOWN" }, { 0x0208, "WM_MBUTTONUP" }, { 0x0209, "WM_MBUTTONDBLCLK" }, { 0x020A, "WM_MOUSEWHEEL" },
			{ 0x020B, "WM_XBUTTONDOWN" }, { 0x020C, "WM_XBUTTONUP" }, { 0x0210, "WM_PARENTNOTIFY" }, { 0x0211, "WM_ENTERMENULOOP" },
			{ 0x0212, "WM_EXITMENULOOP" }, { 0x0213, "WM_NEXTMENU" }, { 0x0214, "WM_SIZING" }, { 0x0215, "WM_CAPTURECHANGED" },
			{ 0x0216, "WM_MOVING" }, { 0x0231, "WM_ENTERSIZEMOVE" }, { 0x0232, "WM_EXITSIZEMOVE" }, { 0x0281, "WM_IME_SETCONTEXT" },
			{ 0x0282, "WM_IME_NOTIFY" }, { 0x02A0, "WM_NCMOUSEHOVER" }, { 0x02A1, "WM_MOUSEHOVER" }, { 0x02A2, "WM_NCMOUSELEAVE" },
			{ 0x02A3, "WM_MOUSELEAVE" }, { 0x0317, "WM_PRINT" }, { 0x0318, "WM_PRINTCLIENT" }, { 0x0319, "WM_APPCOMMAND" },
			{ 0x031F, "WM_DWMNCRENDERINGCHANGED" }, { 0x0400, "WM_USER" },
		};

		// exposed so UI-layer filter defaults can leave these unchecked without referencing this dictionary directly
		public static readonly uint[] HighFrequencyMessageIds = {
			0x0200,
			0x00A0,
			0x02A1,
			0x0084,
			0x0020,
			0x000F,
			0x0085,
			0x0014,
			0x0113,
			0x007F,
			0x0046,
			0x0047,
			0x0083,
		};

		public static IEnumerable<uint> SupportedMessageIds { get { return s_dicMessageNames.Keys; } }

		public static string GetMessageName(uint nmessage) {
			return s_dicMessageNames.TryGetValue(nmessage, out string strname) == true ? strname : $"0x{nmessage:X4}";
		}

		private readonly MessageOnlyWindow m_windowMessageOnly;
		private IntPtr m_hmoduleHook = IntPtr.Zero;
		private IntPtr m_hookHandleCallWndProc = IntPtr.Zero;
		private IntPtr m_hookHandleGetMessage = IntPtr.Zero;
		private Action<IReadOnlyList<MessageLogItem>> m_callback;

		public bool IsMonitoring { get { return m_hookHandleCallWndProc != IntPtr.Zero || m_hookHandleGetMessage != IntPtr.Zero; } }

		public string LastError { get; private set; }

		public MessageHookMonitorService() {
			m_windowMessageOnly = new MessageOnlyWindow(this);
		}

		public bool Start(IntPtr htargetwindow, Action<IReadOnlyList<MessageLogItem>> callback) {
			Stop();

			LastError = null;
			m_callback = callback;

			uint ndwtargetprocessid;
			uint ndwtargetthreadid = MessageHookNativeMethods.GetWindowThreadProcessId(htargetwindow, out ndwtargetprocessid);
			if (ndwtargetthreadid == 0) {
				LastError = "Failed to get target window's thread info.";
				AppLog.w("MessageHookMonitorService.Start: GetWindowThreadProcessId failed");
				return false;
			}

			string strbitnessmismatch = CheckBitnessMismatch(ndwtargetprocessid);
			if (strbitnessmismatch != null) {
				LastError = strbitnessmismatch;
				AppLog.w($"MessageHookMonitorService.Start: {strbitnessmismatch}");
				return false;
			}

			string strdllpath = GetHookDllPath();
			m_hmoduleHook = MessageHookNativeMethods.LoadLibrary(strdllpath);
			if (m_hmoduleHook == IntPtr.Zero) {
				LastError = $"Failed to load native hook DLL: {strdllpath}";
				AppLog.w($"MessageHookMonitorService.Start: LoadLibrary failed for '{strdllpath}', error {Marshal.GetLastWin32Error()}");
				return false;
			}

			IntPtr pfnsetreporttarget = MessageHookNativeMethods.GetProcAddress(m_hmoduleHook, "HookNative_SetReportTarget");
			IntPtr pfncallwndproc = MessageHookNativeMethods.GetProcAddress(m_hmoduleHook, "HookNative_CallWndProc");
			IntPtr pfngetmsgproc = MessageHookNativeMethods.GetProcAddress(m_hmoduleHook, "HookNative_GetMsgProc");
			if (pfnsetreporttarget == IntPtr.Zero || pfncallwndproc == IntPtr.Zero || pfngetmsgproc == IntPtr.Zero) {
				LastError = "Failed to find required exports in native DLL.";
				AppLog.w("MessageHookMonitorService.Start: GetProcAddress failed for one or more exports");
				CleanupModule();
				return false;
			}

			var setreporttarget = (SetReportTargetDelegate) Marshal.GetDelegateForFunctionPointer(pfnsetreporttarget, typeof(SetReportTargetDelegate));
			if (setreporttarget(m_windowMessageOnly.Handle, MessageHookNativeMethods.HOOKNATIVE_DATA_ID) == false) {
				LastError = "Failed to set report target.";
				CleanupModule();
				return false;
			}

			m_hookHandleCallWndProc = MessageHookNativeMethods.SetWindowsHookEx(MessageHookNativeMethods.WH_CALLWNDPROC, pfncallwndproc, m_hmoduleHook, ndwtargetthreadid);
			if (m_hookHandleCallWndProc == IntPtr.Zero) {
				int nerror = Marshal.GetLastWin32Error();
				LastError = BuildHookFailureMessage(nerror);
				AppLog.w($"MessageHookMonitorService.Start: SetWindowsHookEx(WH_CALLWNDPROC) failed, error {nerror}");
				CleanupModule();
				return false;
			}

			m_hookHandleGetMessage = MessageHookNativeMethods.SetWindowsHookEx(MessageHookNativeMethods.WH_GETMESSAGE, pfngetmsgproc, m_hmoduleHook, ndwtargetthreadid);
			if (m_hookHandleGetMessage == IntPtr.Zero) {
				int nerror = Marshal.GetLastWin32Error();
				LastError = BuildHookFailureMessage(nerror);
				AppLog.w($"MessageHookMonitorService.Start: SetWindowsHookEx(WH_GETMESSAGE) failed, error {nerror}");
				Stop();
				return false;
			}

			AppLog.i($"MessageHookMonitorService.Start: hooked thread {ndwtargetthreadid} (PID {ndwtargetprocessid})");
			return true;
		}

		public void Stop() {
			if (m_hookHandleCallWndProc != IntPtr.Zero) {
				MessageHookNativeMethods.UnhookWindowsHookEx(m_hookHandleCallWndProc);
				m_hookHandleCallWndProc = IntPtr.Zero;
			}

			if (m_hookHandleGetMessage != IntPtr.Zero) {
				MessageHookNativeMethods.UnhookWindowsHookEx(m_hookHandleGetMessage);
				m_hookHandleGetMessage = IntPtr.Zero;
			}

			CleanupModule();
			m_callback = null;

			if (IsMonitoring == false) {
				AppLog.i("MessageHookMonitorService.Stop: hooks released");
			}
		}

		// DllMain's DETACH cleanup only signals the sender thread to stop without waiting (blocking in DllMain risks a loader-lock deadlock), so FreeLibrary must wait here or it can unmap code out from under a still-running thread and crash
		private void CleanupModule() {
			if (m_hmoduleHook == IntPtr.Zero) {
				return;
			}

			IntPtr pfnstopandjoin = MessageHookNativeMethods.GetProcAddress(m_hmoduleHook, "HookNative_StopAndJoinSenderThread");
			if (pfnstopandjoin != IntPtr.Zero) {
				var stopandjoin = (StopAndJoinSenderThreadDelegate) Marshal.GetDelegateForFunctionPointer(pfnstopandjoin, typeof(StopAndJoinSenderThreadDelegate));
				stopandjoin(HOOKNATIVE_JOIN_TIMEOUT_MS);
			}

			MessageHookNativeMethods.FreeLibrary(m_hmoduleHook);
			m_hmoduleHook = IntPtr.Zero;
		}

		// SetWindowsHookEx cannot inject a DLL across a bitness boundary, so this is checked up front rather than surfacing as a confusing hook-install failure
		private static string CheckBitnessMismatch(uint ndwtargetprocessid) {
			if (Environment.Is64BitOperatingSystem == false) {
				return null;
			}

			try {
				using (Process process = Process.GetProcessById((int) ndwtargetprocessid)) {
					if (MessageHookNativeMethods.IsWow64Process(process.Handle, out bool btargetiswow64) == false) {
						return null;
					}

					bool btargetis64bit = btargetiswow64 == false;
					if (btargetis64bit != Environment.Is64BitProcess) {
						return "Target process bitness (32/64-bit) differs from this application; hooking across bitness is not possible.";
					}
				}
			}
			catch (Exception ex) {
				AppLog.d($"MessageHookMonitorService.CheckBitnessMismatch failed: {ex.Message}");
			}

			return null;
		}

		private static string BuildHookFailureMessage(int nerror) {
			const int ERROR_ACCESS_DENIED = 5;
			if (nerror == ERROR_ACCESS_DENIED) {
				return "Hook install failed (Access Denied) — target may be running elevated.";
			}

			return $"Hook install failed (Win32 error code {nerror}).";
		}

		private static string GetHookDllPath() {
			string strbasedir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
			string strsuffix = Environment.Is64BitProcess == true ? "x64" : "x86";
			return Path.Combine(strbasedir, $"DH.Window.Analyst.HookNative_{strsuffix}.dll");
		}

		private void OnCopyData(IntPtr plparam) {
			var copydata = (MessageHookNativeMethods.COPYDATASTRUCT) Marshal.PtrToStructure(plparam, typeof(MessageHookNativeMethods.COPYDATASTRUCT));
			if ((uint) copydata.dwData.ToInt64() != MessageHookNativeMethods.HOOKNATIVE_DATA_ID) {
				return;
			}

			int nentrysize = Marshal.SizeOf(typeof(MessageHookNativeMethods.HookNativeMessageEntry));
			int ncount = copydata.cbData / nentrysize;

			// delivered as one batch call, not one call per entry, to avoid ListView flicker at WH_GETMESSAGE frequency
			var listitems = new List<MessageLogItem>(ncount);
			for (int i = 0; i < ncount; i++) {
				IntPtr pentry = IntPtr.Add(copydata.lpData, i * nentrysize);
				var entry = (MessageHookNativeMethods.HookNativeMessageEntry) Marshal.PtrToStructure(pentry, typeof(MessageHookNativeMethods.HookNativeMessageEntry));

				listitems.Add(new MessageLogItem(DateTime.Now, GetMessageName(entry.nmessage), entry.hwnd, entry.wparam, entry.lparam, entry.bposted));
			}

			if (listitems.Count > 0) {
				m_callback?.Invoke(listitems);
			}
		}

		public void Dispose() {
			Stop();
			m_windowMessageOnly.DestroyHandle();
		}

		// must be constructed on a thread that pumps Win32 messages (the WinForms UI thread), since WM_COPYDATA arrives as a queued message there
		private sealed class MessageOnlyWindow : NativeWindow {
			private readonly MessageHookMonitorService m_owner;

			public MessageOnlyWindow(MessageHookMonitorService owner) {
				m_owner = owner;

				var cp = new CreateParams();
				cp.Parent = new IntPtr(-3); // HWND_MESSAGE
				CreateHandle(cp);
			}

			protected override void WndProc(ref Message m) {
				if ((uint) m.Msg == MessageHookNativeMethods.WM_COPYDATA) {
					m_owner.OnCopyData(m.LParam);
				}

				base.WndProc(ref m);
			}
		}
	}
}
