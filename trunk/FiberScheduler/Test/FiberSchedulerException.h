#pragma once

#include <Windows.h>
#include "defines.h"


class FiberSchedulerException
{
private:
  tstring message;

public:
  FiberSchedulerException(LPCTSTR _message);
  FiberSchedulerException(tstring _message);
  tstring GetMessage() const;
};
