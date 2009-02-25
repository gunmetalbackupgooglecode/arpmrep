#include <windows.h>
#include "..\HackUtils\pe.h"

int main()
{
  DWORD thisImageBase = (DWORD)GetModuleHandle(NULL);

  PE thisPE(thisImageBase);

  const IMAGE_FILE_HEADER* thisImageFileHeader;
  const IMAGE_SECTION_HEADER* thisLastSection;

  thisPE.Parse();

  thisImageFileHeader = thisPE.GetImageFileHeader();
  if (NULL == thisImageFileHeader)
  {
    printf("PE::GetImageFileHeader failed: %s\n", thisPE.GetLastError());
  }

  thisLastSection = thisPE.GetSection(thisImageFileHeader->NumberOfSections - 1);
  if (NULL == thisLastSection)
  {
    printf("PE::GetSection failed: %s\n", thisPE.GetLastError());
  }

  PE packedPE(thisImageBase + thisLastSection->VirtualAddress);

  packedPE.Parse();

  return 0;
}
