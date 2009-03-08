#include "PEException.h"


LPCTSTR PE_EXCEPTION_NOT_IMPLEMENTED = _TEXT("Not implemented.");
LPCTSTR PE_EXCEPTION_ACCESS_VIOLATION = _TEXT("Access violation.");
LPCTSTR PE_EXCEPTION_INVALID_FILE_FORMAT = _TEXT("Invalid file format.");
LPCTSTR PE_EXCEPTION_OBJECT_NOT_INITIALIZED = _TEXT("PE object was not initialized.");


PEException::PEException(LPCTSTR _message)
{
  message = _tcsdup(_message);
}


PEException::PEException(const PEException& obj)
{
  message = _tcsdup(obj.message);
}


PEException::~PEException()
{
  free(message);
}


LPCTSTR PEException::GetMessage() const
{
  return message;
}
