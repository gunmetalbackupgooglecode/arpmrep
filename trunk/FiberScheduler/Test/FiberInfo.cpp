#include "FiberInfo.h"

FiberInfo::FiberInfo(LPFIBER_START_ROUTINE _fiberStartAddress, PVOID _fiberParam, PRIORITY _priority)
  : fiberContextAddress(NULL), fiberStartAddress(_fiberStartAddress),
    fiberParam(_fiberParam), priority(_priority), isDead(false) {}


LPVOID FiberInfo::GetFiberContextAddress() const
{
  return fiberContextAddress;
}


void FiberInfo::SetFiberContextAddress(LPVOID _fiberContextAddress)
{
  fiberContextAddress = _fiberContextAddress;
}


LPFIBER_START_ROUTINE FiberInfo::GetFiberStartAddress() const
{
  return fiberStartAddress;
}


PVOID FiberInfo::GetFiberParam() const
{
  return fiberParam;
}


void FiberInfo::Kill()
{
  isDead = true;
}


bool FiberInfo::DeadFiberPredicate(const FiberInfo* fiberInfo)
{
  bool isDead = fiberInfo->isDead;

  if (isDead)
  {
    delete fiberInfo;
  }
  return isDead;
}
