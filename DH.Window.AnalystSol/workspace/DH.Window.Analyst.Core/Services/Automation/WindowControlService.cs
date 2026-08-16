//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Drawing;
using DH.Window.Analyst.Logging;

namespace DH.Window.Analyst.Services.Automation {
	public class WindowControlService : IWindowControlService {
		public bool TryShow(IntPtr handle) {
			return TryCall(() => NativeMethods.ShowWindow(handle, WindowControlNativeMethods.SW_SHOW), "TryShow");
		}

		public bool TryHide(IntPtr handle) {
			return TryCall(() => NativeMethods.ShowWindow(handle, WindowControlNativeMethods.SW_HIDE), "TryHide");
		}

		public bool TryEnable(IntPtr handle) {
			return TryCall(() => WindowControlNativeMethods.EnableWindow(handle, true), "TryEnable");
		}

		public bool TryDisable(IntPtr handle) {
			return TryCall(() => WindowControlNativeMethods.EnableWindow(handle, false), "TryDisable");
		}

		public bool IsEnabled(IntPtr handle) {
			try {
				return WindowControlNativeMethods.IsWindowEnabled(handle);
			}
			catch (Exception ex) {
				AppLog.d($"WindowControlService.IsEnabled failed: {ex.Message}");
				return true;
			}
		}

		public bool IsTopmost(IntPtr handle) {
			try {
				int nexstyle = (int) NativeMethods.GetWindowLongPtr(handle, NativeMethods.GWL_EXSTYLE).ToInt64();
				return (nexstyle & WindowControlNativeMethods.WS_EX_TOPMOST) == WindowControlNativeMethods.WS_EX_TOPMOST;
			}
			catch (Exception ex) {
				AppLog.d($"WindowControlService.IsTopmost failed: {ex.Message}");
				return false;
			}
		}

		public bool TrySetTopmost(IntPtr handle, bool btopmost) {
			IntPtr hwndinsertafter = btopmost == true ? WindowControlNativeMethods.HWND_TOPMOST : WindowControlNativeMethods.HWND_NOTOPMOST;
			return TryCall(() => WindowControlNativeMethods.SetWindowPos(handle, hwndinsertafter, 0, 0, 0, 0, WindowControlNativeMethods.SWP_NOMOVE | WindowControlNativeMethods.SWP_NOSIZE | WindowControlNativeMethods.SWP_NOACTIVATE), "TrySetTopmost");
		}

		public bool TryGetBounds(IntPtr handle, out Rectangle rect) {
			rect = Rectangle.Empty;
			try {
				if (NativeMethods.GetWindowRect(handle, out NativeMethods.RECT nativerect) == false) {
					return false;
				}

				rect = new Rectangle(nativerect.Left, nativerect.Top, nativerect.Right - nativerect.Left, nativerect.Bottom - nativerect.Top);
				return true;
			}
			catch (Exception ex) {
				AppLog.d($"WindowControlService.TryGetBounds failed: {ex.Message}");
				return false;
			}
		}

		public bool TryMoveResize(IntPtr handle, int x, int y, int width, int height) {
			return TryCall(() => WindowControlNativeMethods.SetWindowPos(handle, IntPtr.Zero, x, y, width, height, WindowControlNativeMethods.SWP_NOACTIVATE | WindowControlNativeMethods.SWP_NOZORDER), "TryMoveResize");
		}

		public bool IsOwnProcessWindow(IntPtr handle) {
			try {
				NativeMethods.GetWindowThreadProcessId(handle, out uint nprocessid);
				return nprocessid == (uint) Process.GetCurrentProcess().Id;
			}
			catch (Exception ex) {
				AppLog.d($"WindowControlService.IsOwnProcessWindow failed: {ex.Message}");
				return false;
			}
		}

		private static bool TryCall(Func<bool> action, string strcaller) {
			try {
				return action();
			}
			catch (Exception ex) {
				AppLog.d($"WindowControlService.{strcaller} failed: {ex.Message}");
				return false;
			}
		}
	}
}
