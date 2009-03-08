#pragma once

#include <tchar.h>
#include <Windows.h>

extern LPCTSTR PE_EXCEPTION_NOT_IMPLEMENTED;
extern LPCTSTR PE_EXCEPTION_ACCESS_VIOLATION;
extern LPCTSTR PE_EXCEPTION_INVALID_FILE_FORMAT;
extern LPCTSTR PE_EXCEPTION_OBJECT_NOT_INITIALIZED;

class PEException
{
public:
  explicit PEException(LPCTSTR message);
  PEException(const PEException&);
  ~PEException();

  LPCTSTR GetMessage() const;

private:
  LPTSTR message;
};
