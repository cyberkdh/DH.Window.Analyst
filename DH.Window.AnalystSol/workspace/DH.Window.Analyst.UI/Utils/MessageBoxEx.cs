//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Utils {
	// standard MessageBox.Show(owner, ...) still centers on the screen, not the owner; this uses a thread-local WH_CBT hook (the documented MSDN technique) to reposition the box once it activates
	public static class MessageBoxEx {
		private const int WH_CBT = 5;
		private const int HCBT_ACTIVATE = 5;
		private const uint SWP_NOSIZE = 0x0001;
		private const uint SWP_NOZORDER = 0x0004;
		private const uint SWP_NOACTIVATE = 0x0010;

		private delegate IntPtr HookProc(int ncode, IntPtr wparam, IntPtr lparam);

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowsHookEx(int nidhook, HookProc lpfn, IntPtr hmod, uint ndwthreadid);

		[DllImport("user32.dll")]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll")]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int ncode, IntPtr wparam, IntPtr lparam);

		[DllImport("kernel32.dll")]
		private static extern uint GetCurrentThreadId();

		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(IntPtr hwnd, out RECT lprect);

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndinsertafter, int nx, int ny, int ncx, int ncy, uint uflags);

		// hook state is thread-local: MessageBox.Show blocks the calling thread, so only that thread's hook needs to see the activation
		[ThreadStatic]
		private static IntPtr s_hhook;
		[ThreadStatic]
		private static IWin32Window s_owner;

		public static DialogResult Show(IWin32Window owner, string strtext) {
			return ShowCentered(owner, () => MessageBox.Show(owner, strtext));
		}

		public static DialogResult Show(IWin32Window owner, string strtext, string strcaption) {
			return ShowCentered(owner, () => MessageBox.Show(owner, strtext, strcaption));
		}

		public static DialogResult Show(IWin32Window owner, string strtext, string strcaption, MessageBoxButtons buttons) {
			return ShowCentered(owner, () => MessageBox.Show(owner, strtext, strcaption, buttons));
		}

		public static DialogResult Show(IWin32Window owner, string strtext, string strcaption, MessageBoxButtons buttons, MessageBoxIcon icon) {
			return ShowCentered(owner, () => MessageBox.Show(owner, strtext, strcaption, buttons, icon));
		}

		private static DialogResult ShowCentered(IWin32Window owner, Func<DialogResult> fnshow) {
			if (owner == null) {
				return fnshow();
			}

			s_owner = owner;
			s_hhook = SetWindowsHookEx(WH_CBT, HookProcImpl, IntPtr.Zero, GetCurrentThreadId());

			try {
				return fnshow();
			}
			finally {
				if (s_hhook != IntPtr.Zero) {
					UnhookWindowsHookEx(s_hhook);
					s_hhook = IntPtr.Zero;
				}

				s_owner = null;
			}
		}

		private static IntPtr HookProcImpl(int ncode, IntPtr wparam, IntPtr lparam) {
			IntPtr hhooklocal = s_hhook;

			if (ncode == HCBT_ACTIVATE) {
				CenterOnOwner(wparam);
				UnhookWindowsHookEx(hhooklocal);
				s_hhook = IntPtr.Zero;
			}

			return CallNextHookEx(hhooklocal, ncode, wparam, lparam);
		}

		private static void CenterOnOwner(IntPtr hwndmessagebox) {
			if (GetWindowRect(hwndmessagebox, out RECT rectbox) == false || GetWindowRect(s_owner.Handle, out RECT rectowner) == false) {
				return;
			}

			int nwidth = rectbox.Right - rectbox.Left;
			int nheight = rectbox.Bottom - rectbox.Top;
			int nx = rectowner.Left + ((rectowner.Right - rectowner.Left) - nwidth) / 2;
			int ny = rectowner.Top + ((rectowner.Bottom - rectowner.Top) - nheight) / 2;

			SetWindowPos(hwndmessagebox, IntPtr.Zero, nx, ny, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
		}
	}
}
