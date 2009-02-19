#pragma once


#include <Windows.h>
#include <stdio.h>

#define LAST_ERROR_SIZE 1024


class PE
{
public:
	PE(DWORD fileMapAddress, BOOL dontParse = FALSE);
	~PE();
	
	const CHAR* GetLastError() const;
	BOOL Parse();

private:
	DWORD RvaToFileOffset(DWORD);
	DWORD FileOffsetToRva(DWORD);

//	void ParseImageDosHeader(bool verbose = false);
//	void ParseImageFileHeader(bool verbose = false);
//	void ParseImageOptionalHeader(bool verbose = false);
//	bool ParseImageImportDirectory(); // true if we can to patch IAT

private:
  DWORD fileMapAddress;
  CHAR* lastError;

//	IMAGE_DOS_HEADER* imageDosHeader;
//	IMAGE_FILE_HEADER* imageFileHeader;
//	IMAGE_OPTIONAL_HEADER* imageOptionalHeader;
//	IMAGE_SECTION_HEADER* imageSectionHeader; // array of imageFileHeader->NumberOfSections elements
//	IMAGE_IMPORT_DESCRIPTOR* imageImportDescriptor;
};
