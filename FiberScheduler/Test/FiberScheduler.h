#pragma once

#include <vector>
#include <Windows.h>
#include "FiberInfo.h"


#define EASY_TO_FIND_ADDRESS 0x12345678


class FiberScheduler
{
private:
  static HANDLE mainThreadHandle;
  static HANDLE fiberSchedulerThreadHandle;
  static CRITICAL_SECTION criticalSection;
  static bool isInited;
  static std::vector<FiberInfo*> fiberInfoList;
  static LPVOID activeFiber;

  FiberScheduler();

public:
  static void Init(LPVOID mainFiberParam);
  static void CreateFiber(LPFIBER_START_ROUTINE fiberStartAddress, LPVOID fiberParam);

private:
  static DWORD WINAPI FiberSchedulerThreadProc(LPVOID);
  static VOID CALLBACK FiberProcTemplate(PVOID fiberParam);
  static void ReplaceAddress(PVOID buf, DWORD pattern, DWORD value);
};
