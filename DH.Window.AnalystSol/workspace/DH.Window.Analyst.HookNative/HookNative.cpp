//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

#include "HookNative.h"
#include "MessageQueue.h"
#include <cstring>

static const char* s_pszVersion = "DH.Window.Analyst.HookNative 1.0";

// shared data section: each hooked process gets its own DLL instance/statics, so g_hwndReportTarget/g_ndwDataId must live in shared memory to be visible across processes
#pragma data_seg(".shared")
HWND g_hwndReportTarget = nullptr;
DWORD g_ndwDataId = 0;
#pragma data_seg()
#pragma comment(linker, "/SECTION:.shared,RWS")

static HookNativeMessageQueue g_queue;
static HANDLE g_hthreadSender = nullptr;
static volatile LONG g_lRunning = 0;

#define HOOKNATIVE_SENDER_BATCH_MAX 64
#define HOOKNATIVE_SENDER_POLL_MS 50

#define HOOKNATIVE_WM_WINDOWPOSCHANGED 0x0047
#define HOOKNATIVE_WM_DPICHANGED 0x02E0

// only WM_WINDOWPOSCHANGED/WM_DPICHANGED point at a struct instead of packing values directly into wParam/lParam,
// and that pointer is only valid in this (target) process — decode it here before it crosses into the managed side
static void DecodeLayoutMessage(HookNativeMessageEntry& entry) {
	entry.bhasdecodedrect = FALSE;
	entry.nrectleft = entry.nrecttop = entry.nrectright = entry.nrectbottom = 0;
	entry.ndpi = 0;

	if (entry.nmessage == HOOKNATIVE_WM_WINDOWPOSCHANGED) {
		const WINDOWPOS* pwindowpos = (const WINDOWPOS*) entry.lparam;
		entry.bhasdecodedrect = TRUE;
		entry.nrectleft = pwindowpos->x;
		entry.nrecttop = pwindowpos->y;
		entry.nrectright = pwindowpos->x + pwindowpos->cx;
		entry.nrectbottom = pwindowpos->y + pwindowpos->cy;
	}
	else if (entry.nmessage == HOOKNATIVE_WM_DPICHANGED) {
		const RECT* prect = (const RECT*) entry.lparam;
		entry.bhasdecodedrect = TRUE;
		entry.nrectleft = prect->left;
		entry.nrecttop = prect->top;
		entry.nrectright = prect->right;
		entry.nrectbottom = prect->bottom;
		entry.ndpi = LOWORD(entry.wparam);
	}
}

static DWORD WINAPI SenderThreadProc(LPVOID pvparam) {
	UNREFERENCED_PARAMETER(pvparam);

	HookNativeMessageEntry arrbatch[HOOKNATIVE_SENDER_BATCH_MAX];

	while (InterlockedCompareExchange(&g_lRunning, 0, 0) != 0) {
		int ncount = g_queue.PopBatch(arrbatch, HOOKNATIVE_SENDER_BATCH_MAX, HOOKNATIVE_SENDER_POLL_MS);
		if (ncount > 0 && g_hwndReportTarget != nullptr) {
			COPYDATASTRUCT copydata;
			copydata.dwData = (ULONG_PTR) g_ndwDataId;
			copydata.cbData = (DWORD) (sizeof(HookNativeMessageEntry) * ncount);
			copydata.lpData = arrbatch;

			// runs on the dedicated sender thread, never the hooked thread, so a slow Inspector UI can't stall the target's message pump
			SendMessageW(g_hwndReportTarget, WM_COPYDATA, (WPARAM) nullptr, (LPARAM) &copydata);
		}
	}

	return 0;
}

int __stdcall HookNative_GetVersion(char* pszbuffer, int ncchbuffer) {
	if (pszbuffer == nullptr || ncchbuffer <= 0) {
		return 0;
	}

	int nlen = (int) strlen(s_pszVersion);
	int ncopied = nlen < ncchbuffer - 1 ? nlen : ncchbuffer - 1;
	memcpy(pszbuffer, s_pszVersion, ncopied);
	pszbuffer[ncopied] = '\0';
	return ncopied;
}

BOOL __stdcall HookNative_SetReportTarget(HWND hwndreporttarget, DWORD ndwdataid) {
	g_hwndReportTarget = hwndreporttarget;
	g_ndwDataId = ndwdataid;
	return TRUE;
}

LRESULT CALLBACK HookNative_CallWndProc(int ncode, WPARAM wparam, LPARAM lparam) {
	if (ncode >= 0 && g_hwndReportTarget != nullptr) {
		CWPSTRUCT* pcwp = (CWPSTRUCT*) lparam;

		HookNativeMessageEntry entry;
		entry.hwnd = pcwp->hwnd;
		entry.nmessage = pcwp->message;
		entry.wparam = pcwp->wParam;
		entry.lparam = pcwp->lParam;
		entry.ndwtimestamp = GetTickCount();
		entry.ndwthreadid = GetCurrentThreadId();
		entry.bposted = FALSE;
		DecodeLayoutMessage(entry);

		g_queue.Push(entry);
	}

	return CallNextHookEx(nullptr, ncode, wparam, lparam);
}

LRESULT CALLBACK HookNative_GetMsgProc(int ncode, WPARAM wparam, LPARAM lparam) {
	if (ncode >= 0 && g_hwndReportTarget != nullptr) {
		MSG* pmsg = (MSG*) lparam;

		HookNativeMessageEntry entry;
		entry.hwnd = pmsg->hwnd;
		entry.nmessage = pmsg->message;
		entry.wparam = pmsg->wParam;
		entry.lparam = pmsg->lParam;
		entry.ndwtimestamp = GetTickCount();
		entry.ndwthreadid = GetCurrentThreadId();
		entry.bposted = TRUE;
		DecodeLayoutMessage(entry);

		g_queue.Push(entry);
	}

	return CallNextHookEx(nullptr, ncode, wparam, lparam);
}

// CreateThread (start-only, no wait) is safe to call from DllMain, unlike joining a thread there
void HookNative_StartSenderThread() {
	if (InterlockedCompareExchange(&g_lRunning, 1, 0) == 0) {
		g_hthreadSender = CreateThread(nullptr, 0, SenderThreadProc, nullptr, 0, nullptr);
		if (g_hthreadSender == nullptr) {
			InterlockedExchange(&g_lRunning, 0);
		}
	}
}

// for a hooked target process (Chrome/Edge), this DllMain-triggered detach is the only place the sender thread can be stopped; the bounded WaitForSingleObject here is safe because SenderThreadProc never touches the loader (no LoadLibrary/FreeLibrary/CRT init), so it can't deadlock the loader lock — without this wait, the target crashes when FreeLibrary unmaps the DLL out from under the still-running thread
void HookNative_StopSenderThread() {
	InterlockedExchange(&g_lRunning, 0);

	if (g_hthreadSender != nullptr) {
		WaitForSingleObject(g_hthreadSender, 500);
		CloseHandle(g_hthreadSender);
		g_hthreadSender = nullptr;
	}
}

// called explicitly by the C# controlling process before FreeLibrary, never from DllMain
void __stdcall HookNative_StopAndJoinSenderThread(DWORD ndwtimeoutms) {
	InterlockedExchange(&g_lRunning, 0);

	if (g_hthreadSender != nullptr) {
		WaitForSingleObject(g_hthreadSender, ndwtimeoutms);
		CloseHandle(g_hthreadSender);
		g_hthreadSender = nullptr;
	}
}
