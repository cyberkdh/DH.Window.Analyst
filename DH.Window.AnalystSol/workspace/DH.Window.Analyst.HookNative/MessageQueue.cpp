//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

#include "MessageQueue.h"

HookNativeMessageQueue::HookNativeMessageQueue() : m_nHead(0), m_nTail(0), m_nCount(0) {
	InitializeCriticalSection(&m_critsecQueue);
	m_heventDataAvailable = CreateEventW(nullptr, TRUE, FALSE, nullptr);
}

HookNativeMessageQueue::~HookNativeMessageQueue() {
	if (m_heventDataAvailable != nullptr) {
		CloseHandle(m_heventDataAvailable);
		m_heventDataAvailable = nullptr;
	}
	DeleteCriticalSection(&m_critsecQueue);
}

void HookNativeMessageQueue::Push(const HookNativeMessageEntry& entry) {
	EnterCriticalSection(&m_critsecQueue);

	if (m_nCount < HOOKNATIVE_QUEUE_CAPACITY) {
		m_arrEntries[m_nTail] = entry;
		m_nTail = (m_nTail + 1) % HOOKNATIVE_QUEUE_CAPACITY;
		m_nCount++;
		SetEvent(m_heventDataAvailable);
	}
	// else: queue full, drop the newest entry rather than block the hooked thread

	LeaveCriticalSection(&m_critsecQueue);
}

int HookNativeMessageQueue::PopBatch(HookNativeMessageEntry* pentries, int nmaxcount, DWORD ndwtimeoutms) {
	if (m_nCount == 0 && ndwtimeoutms > 0) {
		WaitForSingleObject(m_heventDataAvailable, ndwtimeoutms);
	}

	EnterCriticalSection(&m_critsecQueue);

	int npopped = 0;
	while (npopped < nmaxcount && m_nCount > 0) {
		pentries[npopped] = m_arrEntries[m_nHead];
		m_nHead = (m_nHead + 1) % HOOKNATIVE_QUEUE_CAPACITY;
		m_nCount--;
		npopped++;
	}

	if (m_nCount == 0) {
		ResetEvent(m_heventDataAvailable);
	}

	LeaveCriticalSection(&m_critsecQueue);
	return npopped;
}
