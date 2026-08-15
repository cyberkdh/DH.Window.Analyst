//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	// only the event IDs listed in s_dicEventNames are ever reported; everything else in EVENT_MIN..EVENT_MAX is dropped here rather than left for the UI to filter, since the full range fires far more event types than this tool needs
	public class WinEventMonitorService : IWinEventMonitorService {
		private static readonly Dictionary<uint, string> s_dicEventNames = new Dictionary<uint, string> {
			{ WinEventNativeMethods.EVENT_SYSTEM_FOREGROUND, "EVENT_SYSTEM_FOREGROUND" },
			{ WinEventNativeMethods.EVENT_OBJECT_CREATE, "EVENT_OBJECT_CREATE" },
			{ WinEventNativeMethods.EVENT_OBJECT_DESTROY, "EVENT_OBJECT_DESTROY" },
			{ WinEventNativeMethods.EVENT_OBJECT_SHOW, "EVENT_OBJECT_SHOW" },
			{ WinEventNativeMethods.EVENT_OBJECT_HIDE, "EVENT_OBJECT_HIDE" },
			{ WinEventNativeMethods.EVENT_OBJECT_FOCUS, "EVENT_OBJECT_FOCUS" },
			{ WinEventNativeMethods.EVENT_OBJECT_SELECTION, "EVENT_OBJECT_SELECTION" },
			{ WinEventNativeMethods.EVENT_OBJECT_STATECHANGE, "EVENT_OBJECT_STATECHANGE" },
			{ WinEventNativeMethods.EVENT_OBJECT_LOCATIONCHANGE, "EVENT_OBJECT_LOCATIONCHANGE" },
			{ WinEventNativeMethods.EVENT_OBJECT_NAMECHANGE, "EVENT_OBJECT_NAMECHANGE" },
			{ WinEventNativeMethods.EVENT_OBJECT_VALUECHANGE, "EVENT_OBJECT_VALUECHANGE" },
		};

		// kept as a field, not a local/anonymous delegate, so the GC can't collect it while the hook is still installed and crash the process on the next callback
		private WinEventNativeMethods.WinEventProc m_procCallback;
		private IntPtr m_hookHandle = IntPtr.Zero;
		private Action<WinEventItem> m_callback;

		public bool IsMonitoring { get { return m_hookHandle != IntPtr.Zero; } }

		// exposed so UI-layer filter defaults can leave these unchecked without referencing the internal native-methods class
		public static readonly uint[] HighFrequencyEventIds = {
			WinEventNativeMethods.EVENT_OBJECT_LOCATIONCHANGE,
			WinEventNativeMethods.EVENT_OBJECT_VALUECHANGE
		};

		public static IEnumerable<uint> SupportedEventIds { get { return s_dicEventNames.Keys; } }

		public static string GetEventName(uint neventtype) {
			return s_dicEventNames.TryGetValue(neventtype, out string strname) == true ? strname : $"0x{neventtype:X4}";
		}

		public void Start(int nprocessid, Action<WinEventItem> callback) {
			Stop();

			m_callback = callback;
			m_procCallback = OnWinEvent;

			m_hookHandle = WinEventNativeMethods.SetWinEventHook(
				WinEventNativeMethods.EVENT_MIN,
				WinEventNativeMethods.EVENT_MAX,
				IntPtr.Zero,
				m_procCallback,
				(uint) nprocessid,
				0,
				WinEventNativeMethods.WINEVENT_OUTOFCONTEXT | WinEventNativeMethods.WINEVENT_SKIPOWNPROCESS);

			if (m_hookHandle == IntPtr.Zero) {
				AppLog.w($"WinEventMonitorService.Start: SetWinEventHook failed for PID {nprocessid}");
			}
			else {
				AppLog.i($"WinEventMonitorService.Start: hooked PID {nprocessid}");
			}
		}

		public void Stop() {
			if (m_hookHandle != IntPtr.Zero) {
				WinEventNativeMethods.UnhookWinEvent(m_hookHandle);
				m_hookHandle = IntPtr.Zero;
				AppLog.i("WinEventMonitorService.Stop: hook released");
			}

			m_procCallback = null;
			m_callback = null;
		}

		private void OnWinEvent(IntPtr hwineventhook, uint neventtype, IntPtr hwnd, int nidobject, int nidchild, uint neventthread, uint ndwmseventtime) {
			if (s_dicEventNames.ContainsKey(neventtype) == false) {
				return;
			}

			m_callback?.Invoke(new WinEventItem(DateTime.Now, GetEventName(neventtype), hwnd, nidobject, nidchild));
		}

		public void Dispose() {
			Stop();
		}
	}
}
