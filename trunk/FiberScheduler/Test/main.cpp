#include <stdio.h>
#include <tchar.h>
#include <Windows.h>
#include "FiberScheduler.h"
#include "FiberSchedulerException.h"


VOID CALLBACK FiberProc(PVOID param)
{
  _tprintf(TEXT("FiberProc\n"));
}


int _tmain()
{
  try
  {
    FiberScheduler::Init(NULL);
    FiberScheduler::CreateFiber(FiberProc, NULL);
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
