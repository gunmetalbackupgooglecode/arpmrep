#include <stdio.h>
#include <tchar.h>
#include <Windows.h>
#include "FiberScheduler.h"
#include "FiberSchedulerException.h"


VOID CALLBACK Fiber1Proc(PVOID param)
{
  _tprintf(TEXT("Fiber1Proc\n"));
}


VOID CALLBACK Fiber2Proc(PVOID param)
{
  _tprintf(TEXT("Fiber2Proc\n"));
}


int _tmain()
{
  try
  {
    FiberScheduler::Init(NULL);
    FiberScheduler::CreateFiber(Fiber1Proc, NULL);
    FiberScheduler::CreateFiber(Fiber2Proc, NULL);
  }
  catch (FiberSchedulerException& e)
  {
    _tprintf(TEXT("FiberScheduler::Init() failed: %s\n"), e.GetMessage().c_str());
    return 1;
  }

  while (true)
  {
    Sleep(1000);
  }

  return 0;
}
