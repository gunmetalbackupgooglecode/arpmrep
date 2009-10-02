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

  DWORD threadId;

  fiberSchedulerThreadHandle = CreateThread(NULL, 0, FiberSchedulerThreadProc, NULL, 0, &threadId);
  if (NULL == fiberSchedulerThreadHandle)
  {
    tstringstream messageStream;

    messageStream << TEXT("CreateThread() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }

  isInited = true;
}


void FiberScheduler::CreateFiber(LPFIBER_START_ROUTINE address, LPVOID fiberParam)
{
  if (!isInited)
  {
    throw FiberSchedulerException(TEXT("Not inited error. Call FiberScheduler::Init() first.\n"));
  }

  FiberInfo* mainFiberInfo = new FiberInfo(address, FiberInfo::NORMAL);
  FiberProcTemplateParam fiberTemplateParam(mainFiberInfo, address, fiberParam);
  LPVOID fiberAddress = ::CreateFiber(0, FiberProcTemplate, &fiberTemplateParam);

  if (NULL == fiberAddress)
  {
    tstringstream messageStream;

    messageStream << TEXT("CreateFiber() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }

  EnterCriticalSection(&criticalSection);
  fiberInfoList.push_back(mainFiberInfo);
  LeaveCriticalSection(&criticalSection);
}


DWORD WINAPI FiberScheduler::FiberSchedulerThreadProc(LPVOID)
{
  while (true)
  {
    // TODO: implement
    Sleep(100);
  }

  return 0;
}


VOID CALLBACK FiberScheduler::FiberProcTemplate(PVOID param)
{
  FiberProcTemplateParam* fiberTemplateParam = (FiberProcTemplateParam*)param;

  __try
  {
    (fiberTemplateParam->address)(fiberTemplateParam->param);
  }
  __finally
  {
    fiberTemplateParam->fiberInfo->Kill();
  }
}
