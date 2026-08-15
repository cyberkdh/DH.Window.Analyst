//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

#pragma once

#include <Windows.h>

#define HOOKNATIVE_QUEUE_CAPACITY 4096

// layout must stay in sync with MessageHookNativeMethods.cs's marshaled struct on the C# side
struct HookNativeMessageEntry {
	HWND hwnd;
	UINT nmessage;
	WPARAM wparam;
	LPARAM lparam;
	DWORD ndwtimestamp;
	DWORD ndwthreadid;
	BOOL bposted; // FALSE = WH_CALLWNDPROC (sent), TRUE = WH_GETMESSAGE (posted/queued)
};

// Push() runs on the hooked target thread and must never block, so a full queue drops entries instead of stalling the target's message pump
class HookNativeMessageQueue {
public:
	HookNativeMessageQueue();
	~HookNativeMessageQueue();

	void Push(const HookNativeMessageEntry& entry);

	int PopBatch(HookNativeMessageEntry* pentries, int nmaxcount, DWORD ndwtimeoutms);

private:
	CRITICAL_SECTION m_critsecQueue;
	HANDLE m_heventDataAvailable;
	HookNativeMessageEntry m_arrEntries[HOOKNATIVE_QUEUE_CAPACITY];
	int m_nHead;
	int m_nTail;
	int m_nCount;
};
