//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once

#include <Windows.h>

// dynamic load via GetProcAddress (not [DllImport]), since the DLL path is chosen at runtime by target bitness
extern "C" {
	__declspec(dllexport) int __stdcall HookNative_GetVersion(char* pszbuffer, int ncchbuffer);

	__declspec(dllexport) BOOL __stdcall HookNative_SetReportTarget(HWND hwndreporttarget, DWORD ndwdataid);

	__declspec(dllexport) LRESULT CALLBACK HookNative_CallWndProc(int ncode, WPARAM wparam, LPARAM lparam);

	__declspec(dllexport) LRESULT CALLBACK HookNative_GetMsgProc(int ncode, WPARAM wparam, LPARAM lparam);

	// must be called before FreeLibrary; skipping it crashes the process because the sender thread is still executing code in this DLL's pages when FreeLibrary unmaps them
	__declspec(dllexport) void __stdcall HookNative_StopAndJoinSenderThread(DWORD ndwtimeoutms);
}
