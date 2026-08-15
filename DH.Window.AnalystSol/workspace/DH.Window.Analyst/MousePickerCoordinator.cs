//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;
using DH.Window.Analyst.Services.Automation;
using DH.Window.Analyst.UI.Controls;
using DH.Window.Analyst.UI.Overlay;

namespace DH.Window.Analyst {
	// "Show Info on Mouse" toggle mode: Timer-polled cursor tracking drives the hover info panel/highlight (no hooking involved),
	// while IGlobalPickerHookService (WH_MOUSE_LL/WH_KEYBOARD_LL, a separate module) only detects the confirming click and Esc-cancel.
	// Kept out of MainForm per the project's modularization convention.
	public class MousePickerCoordinator : IDisposable {
		private const int HOVER_POLL_INTERVAL_MS = 50;

		private readonly IWindowTreeService m_treeService;
		private readonly WorkspaceControl m_ctrlWorkspace;
		private readonly ToolStripButton m_toolButton;
		private readonly Func<TopLevelWindowItem, Task> m_funcSelectTopLevelAsync;

		private readonly IGlobalPickerHookService m_hookService = new GlobalPickerHookService();
		private readonly IProcessDiagnosticsService m_diagnosticsService = new ProcessDiagnosticsService();
		private readonly HighlightOverlayForm m_overlayHover = new HighlightOverlayForm();
		private readonly MouseInfoForm m_formInfo = new MouseInfoForm();
		private readonly Timer m_timerHover;

		// our own process's elevation never changes for the process lifetime, so this is checked once rather than per-tick; the UIPI warning
		// only applies when the hovered window's process outranks ours (elevated target, non-elevated us) — if we are already elevated too, a
		// click there works fine and the warning would be a false alarm
		private readonly bool m_bOwnProcessElevated;

		// per-toggle-session cache (cleared in StartInternal) — elevation cannot change for a process's lifetime, so one OpenProcess/OpenProcessToken
		// round-trip per distinct PID hovered is enough, avoiding repeat syscalls while the cursor lingers over the same window across many 50ms ticks
		private readonly Dictionary<int, bool?> m_dictElevatedCache = new Dictionary<int, bool?>();

		// top-level handle of the elevated window currently under the cursor, or IntPtr.Zero; WH_MOUSE_LL/WH_KEYBOARD_LL are UIPI-blocked while a
		// higher-integrity window is foreground, so a click there never reaches OnPicked/OnCancelled and the mode would otherwise stay stuck ON with
		// no way to cancel it — the next hover tick instead notices this exact window became the foreground window and treats that as the cancel
		private IntPtr m_handleLastHoveredElevated = IntPtr.Zero;

		// handle the info panel's content was last built for; while the cursor stays over the same window, BuildInfoFields/ShowInfo are skipped
		// entirely (only MouseInfoForm.MoveTo runs) since re-assigning identical Label.Text every 50ms tick is wasted work with no visible effect
		private IntPtr m_handleLastInfoContent = IntPtr.Zero;

		// funcselecttoplevelasync is the same "select in list + show in Property View + open workspace tab" logic MainForm's Finder handler
		// uses, shared here rather than duplicated so both entry points behave identically
		public MousePickerCoordinator(IWindowTreeService treeservice, WorkspaceControl ctrlworkspace, ToolStripButton toolbutton, Func<TopLevelWindowItem, Task> funcselecttoplevelasync) {
			m_treeService = treeservice;
			m_ctrlWorkspace = ctrlworkspace;
			m_toolButton = toolbutton;
			m_funcSelectTopLevelAsync = funcselecttoplevelasync;

			m_bOwnProcessElevated = m_diagnosticsService.IsCurrentProcessElevated();

			m_timerHover = new Timer { Interval = HOVER_POLL_INTERVAL_MS };
			m_timerHover.Tick += OnHoverTick;

			m_hookService.Picked += OnPicked;
			m_hookService.Cancelled += OnCancelled;
		}

		public void Toggle() {
			if (m_hookService.IsActive == true) {
				StopInternal();
			}
			else {
				StartInternal();
			}
		}

		private void StartInternal() {
			if (m_hookService.Start() == false) {
				AppLog.w($"MousePickerCoordinator: failed to start ({m_hookService.LastError})");
				m_toolButton.Checked = false;
				return;
			}

			AppLog.i("MousePickerCoordinator: Show Info on Mouse ON");
			m_toolButton.Checked = true;
			m_dictElevatedCache.Clear();
			m_handleLastHoveredElevated = IntPtr.Zero;
			m_handleLastInfoContent = IntPtr.Zero;
			m_timerHover.Start();
		}

		private void StopInternal() {
			m_hookService.Stop();
			m_timerHover.Stop();
			// FlashAt(Rectangle.Empty) hides immediately, matching HideInfo() below, rather than waiting for the fade timer
			m_overlayHover.FlashAt(Rectangle.Empty);
			m_formInfo.HideInfo();
			m_toolButton.Checked = false;
			m_handleLastHoveredElevated = IntPtr.Zero;
			m_handleLastInfoContent = IntPtr.Zero;
		}

		private void OnHoverTick(object sender, EventArgs e) {
			// safety net checked first, before anything else this tick: if the elevated window we were just hovering over has since become
			// the foreground window, a click/keypress leaked straight through the UIPI-blocked hooks instead of being swallowed/reported. Cancel
			// now rather than leaving the mode stuck ON with a hook that can no longer receive Esc either (same UIPI restriction applies to it)
			if (m_handleLastHoveredElevated != IntPtr.Zero && m_treeService.GetForegroundWindowHandle() == m_handleLastHoveredElevated) {
				AppLog.w($"MousePickerCoordinator: click leaked through to elevated foreground window 0x{m_handleLastHoveredElevated.ToInt64():X} (UIPI-blocked hook), auto-cancelling Show Info on Mouse");
				StopInternal();
				return;
			}

			Point ptcursor = Cursor.Position;
			TopLevelWindowItem item = m_treeService.GetWindowAtScreenPoint(ptcursor);

			if (item == null) {
				m_overlayHover.FlashAt(Rectangle.Empty);
				m_formInfo.HideInfo();
				m_handleLastHoveredElevated = IntPtr.Zero;
				m_handleLastInfoContent = IntPtr.Zero;
				return;
			}

			// repeated FlashAt calls on the same rect reset opacity to its initial value every tick, which reads as a continuously-lit border rather than a one-shot flash
			m_overlayHover.FlashAt(m_treeService.GetWindowScreenRect(item.Handle));

			if (item.Handle != m_handleLastInfoContent) {
				bool? biselevated = GetIsElevatedCached(item.ProcessId);
				// only an actual UIPI risk if the target outranks us; if we are elevated too, our hooks aren't blocked and the warning would be a false alarm
				bool bisuipirisk = biselevated == true && m_bOwnProcessElevated == false;
				m_handleLastHoveredElevated = bisuipirisk == true ? m_treeService.GetTopLevelAncestorHandle(item.Handle) : IntPtr.Zero;

				string strwarning = bisuipirisk == true
					? "⚠ Administrator window — click may not register (UIPI). If it doesn't, click the toolbar button again to cancel."
					: null;
				m_formInfo.ShowInfo(ptcursor, BuildInfoFields(item, biselevated), strwarning);
				m_handleLastInfoContent = item.Handle;
			}
			else {
				// same window as last tick — only follow the cursor, skip rebuilding/re-assigning the (unchanged) content
				m_formInfo.MoveTo(ptcursor);
			}
		}

		private bool? GetIsElevatedCached(int nprocessid) {
			if (m_dictElevatedCache.TryGetValue(nprocessid, out bool? biscached) == true) {
				return biscached;
			}

			bool? biselevated = m_diagnosticsService.IsProcessElevated(nprocessid);
			m_dictElevatedCache[nprocessid] = biselevated;
			return biselevated;
		}

		private async void OnPicked(object sender, Point ptscreen) {
			// stop the mode first so its side effects (overlay/hook teardown) are isolated from whatever the pick itself triggers
			StopInternal();

			TopLevelWindowItem itemhit = m_treeService.GetWindowAtScreenPoint(ptscreen);
			if (itemhit == null) {
				return;
			}

			IntPtr handletoplevel = m_treeService.GetTopLevelAncestorHandle(itemhit.Handle);
			TopLevelWindowItem itemtoplevel = m_treeService.GetWindowItem(handletoplevel);
			if (itemtoplevel == null) {
				return;
			}

			AppLog.i($"Show Info on Mouse picked: {itemhit.ClassName} \"{itemhit.Title}\" (handle 0x{itemhit.Handle.ToInt64():X}), top-level 0x{handletoplevel.ToInt64():X}");

			await m_funcSelectTopLevelAsync(itemtoplevel);

			if (itemhit.Handle != handletoplevel) {
				m_ctrlWorkspace.ActiveTabControl?.SelectDescendantByHandle(itemhit.Handle);
			}
		}

		private void OnCancelled(object sender, EventArgs e) {
			StopInternal();
		}

		// table rows for the info panel — everything here is cheap (already-fetched item fields plus one GetWindowQuickInfo call), safe for a 50ms poll tick
		private List<KeyValuePair<string, string>> BuildInfoFields(TopLevelWindowItem item, bool? biselevated) {
			WindowQuickInfo quickinfo = m_treeService.GetWindowQuickInfo(item.Handle);

			return new List<KeyValuePair<string, string>> {
				new KeyValuePair<string, string>("Handle", item.HandleText),
				new KeyValuePair<string, string>("Class Name", item.ClassName),
				new KeyValuePair<string, string>("Title", string.IsNullOrEmpty(item.Title) == false ? item.Title : "(None)"),
				new KeyValuePair<string, string>("Process", $"{item.ProcessName} (PID {item.ProcessId})"),
				new KeyValuePair<string, string>("Thread ID", quickinfo.ThreadId.ToString()),
				new KeyValuePair<string, string>("Rect", $"({quickinfo.ScreenRect.X}, {quickinfo.ScreenRect.Y})  {quickinfo.ScreenRect.Width}×{quickinfo.ScreenRect.Height}"),
				new KeyValuePair<string, string>("Style / ExStyle", $"0x{quickinfo.Style:X8} / 0x{quickinfo.ExStyle:X8}"),
				new KeyValuePair<string, string>("Elevated", biselevated.HasValue == false ? "(Unknown)" : (biselevated.Value == true ? "Yes" : "No"))
			};
		}

		public void Dispose() {
			StopInternal();
			m_hookService.Dispose();
			m_timerHover.Dispose();
			m_overlayHover.Dispose();
			m_formInfo.Dispose();
		}
	}
}
