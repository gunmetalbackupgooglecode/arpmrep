#include <windows.h>
#include <tchar.h>
#include "..\HackUtils\pe.h"
#include "..\HackUtils\PEException.h"

int main()
{
// parse this file
  DWORD thisImageBase = (DWORD)GetModuleHandle(NULL);

  PE thisPE(thisImageBase);

  const IMAGE_FILE_HEADER* thisImageFileHeader;
  const IMAGE_SECTION_HEADER* thisLastSection;

  try
  {
    thisPE.Parse();
  }
  catch (PEException& e)
  {
    _tprintf("Error while parsing this big file: %s\n", e.GetMessage());
    return 1;
  }

  // TODO: do we need to store this file header?

  try
  {
    thisImageFileHeader = thisPE.GetImageFileHeader();
  }
  catch (PEException& e)
  {
    _tprintf("PE::GetImageFileHeader for this big file failed: %s\n", e.GetMessage());
    return 1;
  }

  try
  {
    thisLastSection = thisPE.GetSection(thisImageFileHeader->NumberOfSections - 1);
  }
  catch (PEException& e)
  {
    _tprintf("PE::GetSection for this big file failed: %s\n", e.GetMessage());
    return 1;
  }

// parse original packed file
  DWORD packedImageBase = thisImageBase + thisLastSection->VirtualAddress;
  PE packedPE(packedImageBase);

  try
  {
    packedPE.Parse();
  }
  catch (PEException& e)
  {
    _tprintf("Error while parsing original packed file: %s\n", e.GetMessage());
    return 1;
  }

// store original file header

  const IMAGE_OPTIONAL_HEADER* parsedImageOptionalHeader;

  try
  {
    parsedImageOptionalHeader = thisPE.GetImageOptionalHeader();
  }
  catch (PEException& e)
  {
    _tprintf("PE::GetImageOptionalHeader for original packed file failed: %s\n", e.GetMessage());
    return 1;
  }

// restore header
  memcpy_s( (void*)thisImageBase, parsedImageOptionalHeader->SizeOfHeaders,
            (void*)packedImageBase, parsedImageOptionalHeader->SizeOfHeaders);

  return 0;
}
