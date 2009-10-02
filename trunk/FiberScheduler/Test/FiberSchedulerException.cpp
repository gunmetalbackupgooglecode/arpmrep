#include "FiberSchedulerException.h"


FiberSchedulerException::FiberSchedulerException(LPCTSTR _message)
  : message(_message) {}


FiberSchedulerException::FiberSchedulerException(tstring _message)
  : message(_message) {}


tstring FiberSchedulerException::GetMessage() const
{
  return message;
}
