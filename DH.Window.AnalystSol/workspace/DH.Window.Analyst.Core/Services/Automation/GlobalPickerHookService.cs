//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;

namespace DH.Window.Analyst.Services.Automation {
	// hook callbacks must return fast and never let an exception escape into the OS hook chain, so real work is deferred to the UI thread via m_ctrlMarshal.BeginInvoke and every path is wrapped in try/catch
	public class GlobalPickerHookService : IGlobalPickerHookService {
		// kept as fields, not local/anonymous delegates, so the GC can't collect them while the hooks are installed
		private GlobalHookNativeMethods.LowLevelMouseProc m_procMouse;
		private GlobalHookNativeMethods.LowLevelKeyboardProc m_procKeyboard;
		private IntPtr m_hookMouse = IntPtr.Zero;
		private IntPtr m_hookKeyboard = IntPtr.Zero;

		// invisible marshaling target only — never shown, has no relation to MainForm — so the hook callback always has a UI-thread queue to post to
		private readonly Control m_ctrlMarshal = new Control();

		public bool IsActive { get { return m_hookMouse != IntPtr.Zero; } }

		public string LastError { get; private set; }

		public event EventHandler<Point> Picked;

		public event EventHandler Cancelled;

		public GlobalPickerHookService() {
			// force handle creation now so BeginInvoke is always valid once Start() installs the hooks
			IntPtr hforce = m_ctrlMarshal.Handle;
		}

		public bool Start() {
			Stop();
			LastError = null;

			m_procMouse = MouseHookProc;
			m_procKeyboard = KeyboardHookProc;

			using (System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess()) {
				using (System.Diagnostics.ProcessModule module = process.MainModule) {
					IntPtr hmodule = GlobalHookNativeMethods.GetModuleHandle(module.ModuleName);

					m_hookMouse = GlobalHookNativeMethods.SetWindowsHookEx(GlobalHookNativeMethods.WH_MOUSE_LL, m_procMouse, hmodule, 0);
					if (m_hookMouse == IntPtr.Zero) {
						LastError = $"SetWindowsHookEx(WH_MOUSE_LL) failed: Win32 error {Marshal.GetLastWin32Error()}";
						AppLog.w($"GlobalPickerHookService.Start: {LastError}");
						Stop();
						return false;
					}

					m_hookKeyboard = GlobalHookNativeMethods.SetWindowsHookEx(GlobalHookNativeMethods.WH_KEYBOARD_LL, m_procKeyboard, hmodule, 0);
					if (m_hookKeyboard == IntPtr.Zero) {
						LastError = $"SetWindowsHookEx(WH_KEYBOARD_LL) failed: Win32 error {Marshal.GetLastWin32Error()}";
						AppLog.w($"GlobalPickerHookService.Start: {LastError}");
						Stop();
						return false;
					}
				}
			}

			AppLog.i("GlobalPickerHookService.Start: mouse+keyboard low-level hooks installed");
			return true;
		}

		public void Stop() {
			if (m_hookMouse != IntPtr.Zero) {
				GlobalHookNativeMethods.UnhookWindowsHookEx(m_hookMouse);
				m_hookMouse = IntPtr.Zero;
			}

			if (m_hookKeyboard != IntPtr.Zero) {
				GlobalHookNativeMethods.UnhookWindowsHookEx(m_hookKeyboard);
				m_hookKeyboard = IntPtr.Zero;
			}

			m_procMouse = null;
			m_procKeyboard = null;
		}

		private IntPtr MouseHookProc(int ncode, IntPtr wparam, IntPtr lparam) {
			try {
				if (ncode >= 0) {
					int nmessage = wparam.ToInt32();
					if (nmessage == GlobalHookNativeMethods.WM_LBUTTONDOWN || nmessage == GlobalHookNativeMethods.WM_LBUTTONUP) {
						if (nmessage == GlobalHookNativeMethods.WM_LBUTTONDOWN) {
							GlobalHookNativeMethods.MSLLHOOKSTRUCT hookstruct = Marshal.PtrToStructure<GlobalHookNativeMethods.MSLLHOOKSTRUCT>(lparam);
							Point ptscreen = new Point(hookstruct.pt.X, hookstruct.pt.Y);
							m_ctrlMarshal.BeginInvoke((Action) (() => Picked?.Invoke(this, ptscreen)));
						}
						// swallow both DOWN and UP so the target app never sees a half-completed click while this mode is on (developer-toggled, so no side effect on the target app is acceptable)
						return (IntPtr) 1;
					}
				}
			}
			catch (Exception ex) {
				AppLog.e($"GlobalPickerHookService.MouseHookProc failed: {ex.Message}");
			}

			return GlobalHookNativeMethods.CallNextHookEx(m_hookMouse, ncode, wparam, lparam);
		}

		private IntPtr KeyboardHookProc(int ncode, IntPtr wparam, IntPtr lparam) {
			try {
				if (ncode >= 0 && wparam.ToInt32() == GlobalHookNativeMethods.WM_KEYDOWN) {
					GlobalHookNativeMethods.KBDLLHOOKSTRUCT hookstruct = Marshal.PtrToStructure<GlobalHookNativeMethods.KBDLLHOOKSTRUCT>(lparam);
					if (hookstruct.vkCode == GlobalHookNativeMethods.VK_ESCAPE) {
						m_ctrlMarshal.BeginInvoke((Action) (() => Cancelled?.Invoke(this, EventArgs.Empty)));
						return (IntPtr) 1;
					}
				}
			}
			catch (Exception ex) {
				AppLog.e($"GlobalPickerHookService.KeyboardHookProc failed: {ex.Message}");
			}

			return GlobalHookNativeMethods.CallNextHookEx(m_hookKeyboard, ncode, wparam, lparam);
		}

		public void Dispose() {
			Stop();
			m_ctrlMarshal.Dispose();
		}
	}
}
