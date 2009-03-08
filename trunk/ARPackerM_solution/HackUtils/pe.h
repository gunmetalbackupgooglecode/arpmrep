#pragma once

#include <Windows.h>
#include <stdio.h>

#define PARAGRAPH_SIZE 16


class PE
{
public:
	PE(DWORD fileMapAddress);
	~PE();
	
	void Parse();
  void SetDefaultValues();
  const IMAGE_FILE_HEADER* GetImageFileHeader() const;
  const IMAGE_OPTIONAL_HEADER* GetImageOptionalHeader() const;
  const IMAGE_SECTION_HEADER* AddSection(LPCTSTR name, DWORD size);
  const IMAGE_SECTION_HEADER* GetSection(DWORD index) const;
  BOOL AddImportDll(const CHAR* dllName, const CHAR** funcNames, DWORD funcAmount);

private:
  BOOL IsParsed() const;
  void Reset();
  DWORD AlignDown(DWORD addr, DWORD align) const;
  DWORD AlignUp(DWORD addr, DWORD align) const;
	DWORD RvaToFileOffset(DWORD) const;
	DWORD FileOffsetToRva(DWORD) const;

private:
  DWORD fileMapAddress;
  BOOL parsed;

  IMAGE_DOS_HEADER* imageDosHeader;
  IMAGE_FILE_HEADER* imageFileHeader;
  IMAGE_OPTIONAL_HEADER* imageOptionalHeader;
  IMAGE_SECTION_HEADER* imageSectionHeader;
};
