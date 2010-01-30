#pragma once

#include <Windows.h>
#include <stdio.h>

#define PARAGRAPH_SIZE 16


class PE
{
public:
	PE(DWORD fileMapAddress);
	~PE();
	
  BOOL AddImportDll(LPCTSTR dllName, LPCTSTR funcNames[], DWORD funcAmount);
  const IMAGE_SECTION_HEADER* AddSection(LPCTSTR name, DWORD size);
  const IMAGE_FILE_HEADER* GetImageFileHeader() const;
  const IMAGE_OPTIONAL_HEADER* GetImageOptionalHeader() const;
  const IMAGE_SECTION_HEADER* GetSection(DWORD index) const;
  void Parse();
  void ParseDotNetDirectoty() const;
  void ParseExportDirectoty() const;
  void SetDefaultValues();

private:
  DWORD AlignDown(DWORD addr, DWORD align) const;
  DWORD AlignUp(DWORD addr, DWORD align) const;
  DWORD FileOffsetToRva(DWORD) const;
  BOOL IsParsed() const;
  void Reset();
  DWORD RvaToFileOffset(DWORD) const;

private:
  DWORD fileMapAddress;
  BOOL parsed;

  IMAGE_DOS_HEADER* imageDosHeader;
  IMAGE_FILE_HEADER* imageFileHeader;
  IMAGE_OPTIONAL_HEADER* imageOptionalHeader;
  IMAGE_SECTION_HEADER* imageSectionHeader;
};
