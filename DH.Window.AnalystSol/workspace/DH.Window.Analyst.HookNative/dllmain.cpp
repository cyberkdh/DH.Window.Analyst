//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.Core
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

#include <Windows.h>

extern void HookNative_StartSenderThread();
extern void HookNative_StopSenderThread();

BOOL APIENTRY DllMain(HMODULE hmodule, DWORD ndwreason, LPVOID pvreserved) {
	UNREFERENCED_PARAMETER(hmodule);
	UNREFERENCED_PARAMETER(pvreserved);

	switch (ndwreason) {
		case DLL_PROCESS_ATTACH:
			HookNative_StartSenderThread();
			break;
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_DETACH:
			HookNative_StopSenderThread();
			break;
	}
	return TRUE;
}
