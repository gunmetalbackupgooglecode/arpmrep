#include <stdio.h>
#include <tchar.h>

#include <Windows.h>
#include "FiberScheduler.h"
#include "FiberSchedulerException.h"


HANDLE FiberScheduler::fiberSchedulerThreadHandle = NULL;
CRITICAL_SECTION FiberScheduler::criticalSection;
bool FiberScheduler::isInited = false;
std::list<FiberInfo*> FiberScheduler::fiberInfoList;


void FiberScheduler::Init(LPVOID mainFiberParam)
{
  if (isInited)
  {
    throw FiberSchedulerException(TEXT("Already inited error\n"));
  }

  InitializeCriticalSection(&criticalSection);

  // create first fiber (from main thread)
  LPVOID mainFiberAddress = ConvertThreadToFiber(mainFiberParam);

  if (NULL == mainFiberAddress)
  {
    tstringstream messageStream;

    messageStream << TEXT("ConvertThreadToFiber() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }

  FiberInfo* mainFiberInfo = new FiberInfo(mainFiberAddress, FiberInfo::NORMAL);

  EnterCriticalSection(&criticalSection);
  fiberInfoList.push_back(mainFiberInfo);
  LeaveCriticalSection(&criticalSection);

  // create scheduler thread with above normal priority
  DWORD threadId;

  fiberSchedulerThreadHandle = CreateThread(NULL, 0, FiberSchedulerThreadProc, NULL, CREATE_SUSPENDED, &threadId);
  if (NULL == fiberSchedulerThreadHandle)
  {
    tstringstream messageStream;

    messageStream << TEXT("CreateThread() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }
  if (!SetThreadPriority(fiberSchedulerThreadHandle, THREAD_PRIORITY_ABOVE_NORMAL))
  {
    tstringstream messageStream;

    messageStream << TEXT("SetThreadPriority() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }
  ResumeThread(fiberSchedulerThreadHandle);

  isInited = true;
}


void FiberScheduler::CreateFiber(LPFIBER_START_ROUTINE fiberStartAddress, LPVOID fiberParam)
{
  if (!isInited)
  {
    throw FiberSchedulerException(TEXT("Not inited error. Call FiberScheduler::Init() first.\n"));
  }

  FiberInfo* fiberInfo = new FiberInfo(NULL, FiberInfo::NORMAL);
  FiberProcTemplateParam fiberTemplateParam(fiberInfo, fiberStartAddress, fiberParam);
  LPVOID fiberAddress = ::CreateFiber(0, FiberProcTemplate, &fiberTemplateParam);

  if (NULL == fiberAddress)
  {
    tstringstream messageStream;

    messageStream << TEXT("CreateFiber() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }
  fiberInfo->SetFiberAddress(fiberAddress);

  EnterCriticalSection(&criticalSection);
  fiberInfoList.push_back(fiberInfo);
  LeaveCriticalSection(&criticalSection);
}


DWORD WINAPI FiberScheduler::FiberSchedulerThreadProc(LPVOID)
{
  int curFiber = 0;

  while (true)
  {
    EnterCriticalSection(&criticalSection);

    // delete dead fiber entries
    fiberInfoList.remove_if(FiberInfo::DeadFiberPredicate);

    // there are no alive fiber - it's time to kill myself
    if (fiberInfoList.empty())
    {
      LeaveCriticalSection(&criticalSection);
      return 0;
    }

    if (curFiber >= fiberInfoList.size())
    {
      curFiber = 0;
    }

    FiberInfo* selectedFiberInfo = fiberInfoList[curFiber];

    LeaveCriticalSection(&criticalSection);

    Sleep(100);
  }

  return 0;
}


VOID CALLBACK FiberScheduler::FiberProcTemplate(PVOID fiberParam)
{
  FiberProcTemplateParam* fiberTemplateParam = (FiberProcTemplateParam*)fiberParam;

  __try
  {
    (fiberTemplateParam->address)(fiberTemplateParam->param);
  }
  __finally
  {
    fiberTemplateParam->fiberInfo->Kill();
    while (true)
    {
      Sleep(1000);
    }
  }
}
