#pragma once

#include <Windows.h>

class FiberInfo
{
public:
  enum PRIORITY
  {
    LOW, NORMAL, HIGH
  };

private:
  LPVOID fiberAddress;
  PRIORITY priority;
  bool isDead;

public:
  FiberInfo(LPVOID fiberAddress, PRIORITY priority);
  LPVOID GetFiberAddress() const;
  void SetFiberAddress(LPVOID _fiberAddress);
  void Kill();
  static bool DeadFiberPredicate(const FiberInfo* fiberInfo);
};
