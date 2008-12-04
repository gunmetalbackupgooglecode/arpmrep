#pragma once

#define IMAGE_DOS_ZM_SIGNATURE 0x4D5A	// undocumented ZM signature - if it is set program will write "This program cannot be run in DOS mode."


BYTE* fileMapAddress; // parsed file address after mapping

bool isDll;
bool isImportSectionWritable;

// Image Dos Header
WORD e_magic; // Dos Header Magic Number
LONG e_lfanew; // Image Header Address

// Image File Header
WORD machine;
WORD numberOfSections;
WORD sizeOfOptionalHeader;
WORD characteristics;

// Image Optional Header
WORD magic;
DWORD imageBase;
DWORD addressOfEntryPoint;
DWORD fileAlignment;
DWORD sectionAlignment;
DWORD sizeOfImage;
DWORD sizeOfHeaders;
DWORD checkSum;
IMAGE_DATA_DIRECTORY* dataDirectory;

// IAT
IMAGE_IMPORT_DESCRIPTOR* importDescriptorTable;
DWORD numberOfImportedDlls;
DWORD maxFreeSpaceSize;
DWORD maxFreeSpaceAddress;

// Section Headers
IMAGE_SECTION_HEADER* sections;


DWORD RvaToFileOffset(DWORD);
DWORD FileOffsetToRva(DWORD);

void ParseFile();
bool ParseImageDosHeader(IMAGE_DOS_HEADER*);
bool ParseImageFileHeader(IMAGE_FILE_HEADER*);
bool ParseImageOptionalHeader(IMAGE_OPTIONAL_HEADER*);
bool ParseImageDataDirectory();
bool ParseImageImportDirectory(IMAGE_IMPORT_DESCRIPTOR* firstImportDescriptor);
bool ParseSectionHeaders(IMAGE_SECTION_HEADER* firstSectionHeader);

bool PatchIAT(char* dllName, int numberOfFunctions, char* functionName);
