#pragma once

#include <list>
#include <Windows.h>
#include "FiberInfo.h"


// Singleton
class FiberScheduler
{
private:
  struct FiberProcTemplateParam
  {
    FiberInfo* fiberInfo;
    LPFIBER_START_ROUTINE address;
    PVOID param;

    FiberProcTemplateParam(FiberInfo* _fiberInfo, LPFIBER_START_ROUTINE _address, PVOID _param) :
      fiberInfo(_fiberInfo), address(_address), param(_param) {}
  };

private:
  static HANDLE fiberSchedulerThreadHandle;
  static CRITICAL_SECTION criticalSection;
  static bool isInited;
  static std::list<FiberInfo*> fiberInfoList;

  FiberScheduler();

public:
  static void Init(LPVOID mainFiberParam);
  static void CreateFiber(LPFIBER_START_ROUTINE fiberStartAddress, LPVOID fiberParam);

private:
  static DWORD WINAPI FiberSchedulerThreadProc(LPVOID);
  static VOID CALLBACK FiberProcTemplate(PVOID fiberParam);
};
