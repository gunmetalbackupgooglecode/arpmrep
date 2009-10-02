#pragma once

#include <sstream>
#include <string>

#ifdef UNICODE
  typedef std::wstring tstring;
  typedef std::wstringstream tstringstream;
#else
  typedef std::string tstring;
  typedef std::stringstream stringstream;
#endif
