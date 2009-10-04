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
  LPVOID fiberContextAddress;
  LPFIBER_START_ROUTINE fiberStartAddress;
  PVOID fiberParam;
  PRIORITY priority;
  bool isDead;

public:
  FiberInfo(LPFIBER_START_ROUTINE fiberStartAddress, PVOID fiberParam, PRIORITY priority);
  LPVOID GetFiberContextAddress() const;
  void SetFiberContextAddress(LPVOID fiberContextAddress);
  LPFIBER_START_ROUTINE GetFiberStartAddress() const;
  PVOID GetFiberParam() const;
  void Kill();
  static bool DeadFiberPredicate(const FiberInfo* fiberInfo);
};
