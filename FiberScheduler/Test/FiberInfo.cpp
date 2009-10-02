#include "FiberInfo.h"

FiberInfo::FiberInfo(LPVOID _fiberAddress, PRIORITY _priority)
  : fiberAddress(_fiberAddress), priority(_priority), isDead(false) {}


LPVOID FiberInfo::GetFiberAddress() const
{
  return fiberAddress;
}


void FiberInfo::SetFiberAddress(LPVOID _fiberAddress)
{
  fiberAddress = _fiberAddress;
}


void FiberInfo::Kill()
{
  isDead = true;
}


bool FiberInfo::DeadFiberPredicate(const FiberInfo* fiberInfo)
{
  if (fiberInfo->isDead)
  {
    delete fiberInfo;
  }
  return fiberInfo->isDead;
}
