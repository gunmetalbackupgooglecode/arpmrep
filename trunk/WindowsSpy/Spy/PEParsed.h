#pragma once


#include <Windows.h>
#include <stdio.h>

#define LAST_ERROR_SIZE 1024


class PEParsed
{
public:
	PEParsed(DWORD fileMapAddress);
	~PEParsed();
	
	char* GetLastError();
	void PrintInfo();
	bool PatchHeader(DWORD xCodeSize);
	bool PatchSectionTail(int sectionIndex, bool isSetWrite);
	bool PatchIAT(char* dllName, char* functionName);

private:
	DWORD RvaToFileOffset(DWORD);
	DWORD FileOffsetToRva(DWORD);

	void ParseImageDosHeader(bool verbose = false);
	void ParseImageFileHeader(bool verbose = false);
	void ParseImageOptionalHeader(bool verbose = false);

	void ParseHeaderForFreeSpace(bool verbose = false);
	void ParseSectionsForFreeSpace();
	bool ParseImageImportDirectory(); // true if we can to patch IAT

private:
	DWORD fileMapAddress;
	char* lastError;

	IMAGE_DOS_HEADER* imageDosHeader;
	IMAGE_FILE_HEADER* imageFileHeader;
	IMAGE_OPTIONAL_HEADER* imageOptionalHeader;
	IMAGE_SECTION_HEADER* imageSectionHeader; // array of imageFileHeader->NumberOfSections elements

	DWORD headerFreeSpaceAddress;
	DWORD headerFreeSpaceSize;
	bool isXCodeInHeader;
	
	IMAGE_IMPORT_DESCRIPTOR* imageImportDescriptor;
	int numberOfImportedDlls;
	DWORD maxFreeSpaceIATAddress;
};
