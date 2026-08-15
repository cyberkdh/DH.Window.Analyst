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
using System.Text;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public class WindowDiagnosticsService : IWindowDiagnosticsService {
		public WindowDiagnosticsInfo GetWindowDiagnostics(IntPtr handle) {
			bool bishung = DiagnosticsNativeMethods.IsHungAppWindow(handle);
			bool biscloaked = TryGetCloaked(handle);

			bool bhasextendedframebounds = TryGetExtendedFrameBounds(handle, out Rectangle extendedframebounds);

			bool bislayered = DiagnosticsNativeMethods.GetLayeredWindowAttributes(handle, out int ncolorkey, out byte nalpha, out uint dwflags);
			byte? nlayeredalpha = bislayered && (dwflags & DiagnosticsNativeMethods.LWA_ALPHA) == DiagnosticsNativeMethods.LWA_ALPHA ? (byte?) nalpha : null;
			int? nlayeredcolorkey = bislayered && (dwflags & DiagnosticsNativeMethods.LWA_COLORKEY) == DiagnosticsNativeMethods.LWA_COLORKEY ? (int?) ncolorkey : null;

			string strdpiawareness = GetDpiAwarenessLabel(handle);

			return new WindowDiagnosticsInfo(bishung, biscloaked, bhasextendedframebounds, extendedframebounds, bislayered, nlayeredalpha, nlayeredcolorkey, strdpiawareness);
		}

		// walks GW_HWNDFIRST/GW_HWNDNEXT front-to-back, including hidden/owned windows, so the stack is complete
		public IEnumerable<ZOrderEntryItem> GetZOrderStack(IntPtr handle) {
			List<ZOrderEntryItem> listentries = new List<ZOrderEntryItem>();

			IntPtr hwnd = NativeMethods.GetWindow(handle, NativeMethods.GW_HWNDFIRST);
			while (hwnd != IntPtr.Zero) {
				listentries.Add(BuildZOrderEntry(hwnd, handle));
				hwnd = NativeMethods.GetWindow(hwnd, NativeMethods.GW_HWNDNEXT);
			}

			return listentries;
		}

		private static ZOrderEntryItem BuildZOrderEntry(IntPtr hwnd, IntPtr handletarget) {
			int nlength = NativeMethods.GetWindowTextLength(hwnd);
			StringBuilder sbtitle = new StringBuilder(nlength + 1);
			if (nlength > 0) {
				NativeMethods.GetWindowText(hwnd, sbtitle, sbtitle.Capacity);
			}

			StringBuilder sbclassname = new StringBuilder(256);
			NativeMethods.GetClassName(hwnd, sbclassname, sbclassname.Capacity);

			NativeMethods.GetWindowRect(hwnd, out NativeMethods.RECT rect);
			Rectangle bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

			NativeMethods.GetWindowThreadProcessId(hwnd, out uint nprocessid);
			string strprocessname = TryGetProcessName((int) nprocessid);

			bool bisvisible = NativeMethods.IsWindowVisible(hwnd);

			return new ZOrderEntryItem(hwnd, sbtitle.ToString(), sbclassname.ToString(), strprocessname, bounds, bisvisible, hwnd == handletarget);
		}

		private static string TryGetProcessName(int nprocessid) {
			try {
				using (Process process = Process.GetProcessById(nprocessid)) {
					return process.ProcessName;
				}
			}
			catch (Exception ex) {
				AppLog.d($"WindowDiagnosticsService.TryGetProcessName failed for PID {nprocessid}: {ex.Message}");
				return "(Unknown)";
			}
		}

		private static bool TryGetCloaked(IntPtr handle) {
			try {
				return DiagnosticsNativeMethods.DwmGetWindowAttribute(handle, DiagnosticsNativeMethods.DWMWA_CLOAKED, out int ncloaked, sizeof(int)) == 0 && ncloaked != 0;
			}
			catch (Exception ex) {
				// dwmapi.dll unavailable (DWM off, or unsupported OS) — non-fatal
				AppLog.d($"WindowDiagnosticsService.TryGetCloaked failed: {ex.Message}");
				return false;
			}
		}

		private static bool TryGetExtendedFrameBounds(IntPtr handle, out Rectangle rect) {
			rect = Rectangle.Empty;

			try {
				int nresult = DiagnosticsNativeMethods.DwmGetWindowAttribute(handle, DiagnosticsNativeMethods.DWMWA_EXTENDED_FRAME_BOUNDS, out NativeMethods.RECT nativerect, System.Runtime.InteropServices.Marshal.SizeOf(typeof(NativeMethods.RECT)));
				if (nresult != 0) {
					return false;
				}

				rect = new Rectangle(nativerect.Left, nativerect.Top, nativerect.Right - nativerect.Left, nativerect.Bottom - nativerect.Top);
				return true;
			}
			catch (Exception ex) {
				AppLog.d($"WindowDiagnosticsService.TryGetExtendedFrameBounds failed: {ex.Message}");
				return false;
			}
		}

		// GetWindowDpiAwarenessContext/AreDpiAwarenessContextsEqual require Windows 10 1607+; missing entry points on older OS fall back to "(Unknown)" rather than throwing
		private static string GetDpiAwarenessLabel(IntPtr handle) {
			try {
				IntPtr dpicontext = DiagnosticsNativeMethods.GetWindowDpiAwarenessContext(handle);
				if (dpicontext == IntPtr.Zero) {
					return "(Unknown)";
				}

				if (DiagnosticsNativeMethods.AreDpiAwarenessContextsEqual(dpicontext, DiagnosticsNativeMethods.DPI_AWARENESS_CONTEXT_UNAWARE) == true) {
					return "Unaware";
				}
				if (DiagnosticsNativeMethods.AreDpiAwarenessContextsEqual(dpicontext, DiagnosticsNativeMethods.DPI_AWARENESS_CONTEXT_SYSTEM_AWARE) == true) {
					return "System Aware";
				}
				if (DiagnosticsNativeMethods.AreDpiAwarenessContextsEqual(dpicontext, DiagnosticsNativeMethods.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE) == true) {
					return "Per-Monitor Aware";
				}
				if (DiagnosticsNativeMethods.AreDpiAwarenessContextsEqual(dpicontext, DiagnosticsNativeMethods.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2) == true) {
					return "Per-Monitor Aware V2";
				}
				if (DiagnosticsNativeMethods.AreDpiAwarenessContextsEqual(dpicontext, DiagnosticsNativeMethods.DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED) == true) {
					return "Unaware (GDI-Scaled)";
				}

				return "(Unknown)";
			}
			catch (Exception ex) {
				AppLog.d($"WindowDiagnosticsService.GetDpiAwarenessLabel failed: {ex.Message}");
				return "(Unknown)";
			}
		}
	}
}
