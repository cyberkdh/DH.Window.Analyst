//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using DH.Window.Analyst.Logging;
using DH.Window.Analyst.Models;

namespace DH.Window.Analyst.Services.Automation {
	public class ProcessDiagnosticsService : IProcessDiagnosticsService {
		public bool IsCurrentProcessElevated() {
			using (WindowsIdentity identity = WindowsIdentity.GetCurrent()) {
				return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
			}
		}

		public bool? IsProcessElevated(int nprocessid) {
			return TryGetIsElevatedLimited(nprocessid);
		}

		public ProcessDiagnosticsInfo GetProcessDiagnostics(int nprocessid) {
			string strfileversion = "(Unknown)";
			string strproductname = "(Unknown)";
			string strcompanyname = "(Unknown)";
			long nworkingsetbytes = 0;
			TimeSpan totalprocessortime = TimeSpan.Zero;
			int nthreadcount = 0;
			int? ngdiobjectcount = null;
			int? nuserobjectcount = null;

			// queried via its own PROCESS_QUERY_LIMITED_INFORMATION handle, not process.Handle below, since that request is granted by MIC even against an elevated target from a non-elevated caller, unlike the broader access mask .NET's Process.Handle requests
			bool? biselevated = TryGetIsElevatedLimited(nprocessid);

			try {
				using (Process process = Process.GetProcessById(nprocessid)) {
					nworkingsetbytes = process.WorkingSet64;
					totalprocessortime = process.TotalProcessorTime;
					nthreadcount = process.Threads.Count;

					string strexepath = process.MainModule?.FileName;
					if (string.IsNullOrEmpty(strexepath) == false) {
						FileVersionInfo versioninfo = FileVersionInfo.GetVersionInfo(strexepath);
						strfileversion = string.IsNullOrEmpty(versioninfo.FileVersion) == false ? versioninfo.FileVersion : "(Unknown)";
						strproductname = string.IsNullOrEmpty(versioninfo.ProductName) == false ? versioninfo.ProductName : "(Unknown)";
						strcompanyname = string.IsNullOrEmpty(versioninfo.CompanyName) == false ? versioninfo.CompanyName : "(Unknown)";
					}

					ngdiobjectcount = TryGetGuiResourceCount(process.Handle, DiagnosticsNativeMethods.GR_GDIOBJECTS);
					nuserobjectcount = TryGetGuiResourceCount(process.Handle, DiagnosticsNativeMethods.GR_USEROBJECTS);
				}
			}
			catch (Exception ex) {
				// access denied (e.g. elevated process) or process already exited — return what we have
				AppLog.d($"ProcessDiagnosticsService.GetProcessDiagnostics failed for PID {nprocessid}: {ex.Message}");
			}

			return new ProcessDiagnosticsInfo(strfileversion, strproductname, strcompanyname, nworkingsetbytes, totalprocessortime, nthreadcount, biselevated, ngdiobjectcount, nuserobjectcount);
		}

		private static int? TryGetGuiResourceCount(IntPtr hprocess, uint dwflag) {
			try {
				uint ncount = DiagnosticsNativeMethods.GetGuiResources(hprocess, dwflag);
				return ncount > 0 ? (int?) ncount : null;
			}
			catch (Exception ex) {
				AppLog.d($"ProcessDiagnosticsService.TryGetGuiResourceCount failed: {ex.Message}");
				return null;
			}
		}

		// opens its own limited-access handle rather than reusing process.Handle, so this succeeds even when the caller is not elevated and the target is
		private static bool? TryGetIsElevatedLimited(int nprocessid) {
			IntPtr hprocess = DiagnosticsNativeMethods.OpenProcess(DiagnosticsNativeMethods.PROCESS_QUERY_LIMITED_INFORMATION, false, nprocessid);
			if (hprocess == IntPtr.Zero) {
				return null;
			}

			try {
				return TryGetIsElevated(hprocess);
			}
			finally {
				DiagnosticsNativeMethods.CloseHandle(hprocess);
			}
		}

		// SeDebugPrivilege is not required here: TOKEN_QUERY against a same-user elevated token is granted by the token's DACL regardless of the caller's own integrity level
		private static bool? TryGetIsElevated(IntPtr hprocess) {
			IntPtr htokenhandle = IntPtr.Zero;

			try {
				if (DiagnosticsNativeMethods.OpenProcessToken(hprocess, DiagnosticsNativeMethods.TOKEN_QUERY, out htokenhandle) == false) {
					return null;
				}

				int nsize = Marshal.SizeOf(typeof(int));
				IntPtr ptokeninformation = Marshal.AllocHGlobal(nsize);

				try {
					if (DiagnosticsNativeMethods.GetTokenInformation(htokenhandle, DiagnosticsNativeMethods.TokenElevation, ptokeninformation, nsize, out int nreturnlength) == false) {
						return null;
					}

					return Marshal.ReadInt32(ptokeninformation) != 0;
				}
				finally {
					Marshal.FreeHGlobal(ptokeninformation);
				}
			}
			catch (Exception ex) {
				AppLog.d($"ProcessDiagnosticsService.TryGetIsElevated failed: {ex.Message}");
				return null;
			}
			finally {
				if (htokenhandle != IntPtr.Zero) {
					DiagnosticsNativeMethods.CloseHandle(htokenhandle);
				}
			}
		}
	}
}
