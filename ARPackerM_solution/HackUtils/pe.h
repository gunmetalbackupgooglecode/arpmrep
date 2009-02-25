#pragma once


#include <Windows.h>
#include <stdio.h>

#define LAST_ERROR_SIZE 1024
#define PARAGRAPH_SIZE 16


class PE
{
public:
	PE(DWORD fileMapAddress);
	~PE();
	
	const CHAR* GetLastError() const;
	BOOL Parse();
  BOOL SetDefaultValues();
  const IMAGE_FILE_HEADER* GetImageFileHeader();
  const IMAGE_SECTION_HEADER* AddSection(const CHAR* name, DWORD size);
  const IMAGE_SECTION_HEADER* GetSection(DWORD index);
  BOOL AddImportDll(const CHAR* dllName, const CHAR** funcNames, DWORD funcAmount);

private:
  DWORD AlignDown(DWORD addr, DWORD align) const;
  DWORD AlignUp(DWORD addr, DWORD align) const;
	DWORD RvaToFileOffset(DWORD);
	DWORD FileOffsetToRva(DWORD);

private:
  DWORD fileMapAddress;
  CHAR* lastError;
  BOOL parsed;

  IMAGE_DOS_HEADER* imageDosHeader;
  IMAGE_FILE_HEADER* imageFileHeader;
  IMAGE_OPTIONAL_HEADER* imageOptionalHeader;
  IMAGE_SECTION_HEADER* imageSectionHeader;
};
