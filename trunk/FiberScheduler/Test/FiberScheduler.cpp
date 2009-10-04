#include <algorithm>
#include <stdio.h>
#include <tchar.h>

#include <Windows.h>
#include "FiberScheduler.h"
#include "FiberSchedulerException.h"


HANDLE FiberScheduler::mainThreadHandle = NULL;
HANDLE FiberScheduler::fiberSchedulerThreadHandle = NULL;
CRITICAL_SECTION FiberScheduler::criticalSection;
bool FiberScheduler::isInited = false;
std::vector<FiberInfo*> FiberScheduler::fiberInfoList;
LPVOID FiberScheduler::activeFiber = NULL;


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
  activeFiber = mainFiberAddress;

  FiberInfo* mainFiberInfo = new FiberInfo(NULL, mainFiberParam, FiberInfo::NORMAL);

  mainFiberInfo->SetFiberContextAddress(mainFiberAddress);

  EnterCriticalSection(&criticalSection);
  fiberInfoList.push_back(mainFiberInfo);
  LeaveCriticalSection(&criticalSection);

  mainThreadHandle = OpenThread(THREAD_SUSPEND_RESUME | THREAD_GET_CONTEXT | THREAD_SET_CONTEXT,
                                FALSE,
                                GetCurrentThreadId());
  if (NULL == mainThreadHandle)
  {
    tstringstream messageStream;

    messageStream << TEXT("OpenThread() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }

  // create scheduler thread with above normal priority
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


void FiberScheduler::CreateFiber(LPFIBER_START_ROUTINE fiberStartAddress, LPVOID fiberParam)
{
  if (!isInited)
  {
    throw FiberSchedulerException(TEXT("Not inited error. Call FiberScheduler::Init() first.\n"));
  }

  FiberInfo* fiberInfo = new FiberInfo(fiberStartAddress, fiberParam, FiberInfo::NORMAL);
  LPVOID fiberAddress = ::CreateFiber(0, FiberProcTemplate, fiberInfo);

  if (NULL == fiberAddress)
  {
    tstringstream messageStream;

    messageStream << TEXT("CreateFiber() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }
  fiberInfo->SetFiberContextAddress(fiberAddress);

  EnterCriticalSection(&criticalSection);
  fiberInfoList.push_back(fiberInfo);
  LeaveCriticalSection(&criticalSection);
}


static __declspec(naked) VOID SwitcherTemplate()
{
  __asm
  {
    pusha
    push EASY_TO_FIND_ADDRESS
    mov eax, EASY_TO_FIND_ADDRESS
    call eax
    popa
    push EASY_TO_FIND_ADDRESS
    ret
  }
}

DWORD WINAPI FiberScheduler::FiberSchedulerThreadProc(LPVOID)
{
  size_t curFiber = 0;
  HMODULE hKernel32 = LoadLibrary(TEXT("kernel32.dll"));
  DWORD switchToFiberAddress;

  if (NULL == hKernel32)
  {
    tstringstream messageStream;

    messageStream << TEXT("LoadLibrary() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }

  switchToFiberAddress = (DWORD)GetProcAddress(hKernel32, "SwitchToFiber");
  if (NULL == switchToFiberAddress)
  {
    tstringstream messageStream;

    messageStream << TEXT("GetProcAddress() failed. Error = ") << GetLastError();
    throw FiberSchedulerException(messageStream.str());
  }

  while (true)
  {
    EnterCriticalSection(&criticalSection);
    SuspendThread(mainThreadHandle);

    // delete dead fiber entries
    fiberInfoList.erase(std::remove_if(fiberInfoList.begin(), fiberInfoList.end(), FiberInfo::DeadFiberPredicate), fiberInfoList.end());

    // there are no alive fiber - it's time to kill myself
    if (fiberInfoList.empty())
    {
      LeaveCriticalSection(&criticalSection);
      ResumeThread(mainThreadHandle);
      return 0;
    }

    FiberInfo* selectedFiberInfo;

    do 
    {
      ++ curFiber;
      if (curFiber >= fiberInfoList.size())
      {
        curFiber = 0;
      }
      selectedFiberInfo = fiberInfoList[curFiber];
      if (selectedFiberInfo->GetFiberContextAddress() != activeFiber)
      {
        break;
      }
    } while (fiberInfoList.size() != 1);
    LeaveCriticalSection(&criticalSection);

    DWORD oldEip;
    LPVOID switcher = VirtualAlloc(NULL, 64, MEM_COMMIT, PAGE_EXECUTE_READWRITE);

    if (NULL == switcher)
    {
      tstringstream messageStream;

      ResumeThread(mainThreadHandle);
      messageStream << TEXT("VirtualAlloc() failed. Error = ") << GetLastError();
      throw FiberSchedulerException(messageStream.str());
    }

    memcpy(switcher, &SwitcherTemplate, 64);

    CONTEXT mainThreadContext;

    mainThreadContext.ContextFlags = CONTEXT_CONTROL;
    if (!GetThreadContext(mainThreadHandle, &mainThreadContext))
    {
      tstringstream messageStream;

      ResumeThread(mainThreadHandle);
      messageStream << TEXT("GetThreadContext() failed. Error = ") << GetLastError();
      throw FiberSchedulerException(messageStream.str());
    }
    oldEip = mainThreadContext.Eip;
    mainThreadContext.Eip = (DWORD)switcher;
    if (!SetThreadContext(mainThreadHandle, &mainThreadContext))
    {
      tstringstream messageStream;

      ResumeThread(mainThreadHandle);
      messageStream << TEXT("SetThreadContext() failed. Error = ") << GetLastError();
      throw FiberSchedulerException(messageStream.str());
    }
    ReplaceAddress(switcher, EASY_TO_FIND_ADDRESS, (DWORD)selectedFiberInfo->GetFiberContextAddress());
    ReplaceAddress(switcher, EASY_TO_FIND_ADDRESS, switchToFiberAddress);
    ReplaceAddress(switcher, EASY_TO_FIND_ADDRESS, oldEip);

    ResumeThread(mainThreadHandle);
    Sleep(100);
  }

  return 0;
}


VOID CALLBACK FiberScheduler::FiberProcTemplate(PVOID _fiberParam)
{
  FiberInfo* fiberParam = (FiberInfo*)_fiberParam;

  __try
  {
    (fiberParam->GetFiberStartAddress())(fiberParam->GetFiberParam());
  }
  __finally
  {
    fiberParam->Kill();
    while (true)
    {
      Sleep(1000);
    }
  }
}


void FiberScheduler::ReplaceAddress(PVOID buf, DWORD pattern, DWORD value)
{
  DWORD bufPos = (DWORD)buf;

  while (*((DWORD*)(bufPos ++)) != pattern);
  -- bufPos;
  *((DWORD*)bufPos) = value;
}
