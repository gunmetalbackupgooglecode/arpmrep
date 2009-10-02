#include "FiberInfo.h"

FiberInfo::FiberInfo(LPVOID _fiberAddress, PRIORITY _priority)
  : fiberAddress(_fiberAddress), priority(_priority), isDead(false) {}


void FiberInfo::Kill()
{
  isDead = true;
}
