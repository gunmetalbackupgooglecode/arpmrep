#include "pe.h"


PE::PE(DWORD _fileMapAddress, BOOL dontParse /* = FALSE */) :
  fileMapAddress(_fileMapAddress)
{
  lastError = new CHAR[LAST_ERROR_SIZE];
  strcpy_s(lastError,
           LAST_ERROR_SIZE,
           "No error");

//  imageDosHeader = (IMAGE_DOS_HEADER*)fileMapAddress;
//  imageFileHeader = (IMAGE_FILE_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE));
//  imageOptionalHeader = (IMAGE_OPTIONAL_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE) + sizeof(IMAGE_FILE_HEADER));
//  imageSectionHeader = (IMAGE_SECTION_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE) + sizeof(IMAGE_FILE_HEADER) + sizeof(IMAGE_OPTIONAL_HEADER));
}


PE::~PE()
{
  delete[] lastError;
}


const CHAR* PE::GetLastError() const
{
  return lastError;
}


BOOL PE::Parse()
{
  strcpy_s(	lastError,
            LAST_ERROR_SIZE,
            "Not implemented");
  return FALSE;
}


DWORD PE::RvaToFileOffset(DWORD rva)
{
  strcpy_s(	lastError,
            LAST_ERROR_SIZE,
            "Not implemented");
  return -1;

/*
  for (int sectionIndex = 0; sectionIndex < imageFileHeader->NumberOfSections; ++ sectionIndex)
  {
    IMAGE_SECTION_HEADER curSection = imageSectionHeader[sectionIndex];

    if (rva >= curSection.VirtualAddress &&
        rva < curSection.VirtualAddress + curSection.Misc.VirtualSize)
    {
      return (rva - curSection.VirtualAddress + curSection.PointerToRawData);
    }
  }

  return rva;
*/
}


DWORD PE::FileOffsetToRva(DWORD fileOffset)
{
  strcpy_s(	lastError,
            LAST_ERROR_SIZE,
            "Not implemented");
  return -1;
  
/*
  for (int sectionIndex = 0; sectionIndex < imageFileHeader->NumberOfSections; ++ sectionIndex)
  {
    IMAGE_SECTION_HEADER curSection = imageSectionHeader[sectionIndex];

    if (fileOffset >= curSection.PointerToRawData &&
        fileOffset < curSection.PointerToRawData + curSection.SizeOfRawData)
    {
      return (fileOffset + curSection.VirtualAddress - curSection.PointerToRawData);
    }
  }

  return fileOffset;
*/
}


/*
void PEParsed::PrintInfo()
{
  printf("General analysis:\n");
  ParseImageDosHeader(true);
  ParseImageFileHeader(true);
  ParseImageOptionalHeader(true);

  ParseHeaderForFreeSpace();
  ParseSectionsForFreeSpace();

  ParseImageImportDirectory();
}


void PEParsed::ParseImageDosHeader(bool verbose)
{
  if (verbose)
  {
    printf("Parsing Image Dos Header...\n");
    if (imageDosHeader->e_magic != IMAGE_DOS_SIGNATURE)
    {
      printf("DOS MAGIC NUMBER IS NOT MZ. IT IS 0x%x\n",
             imageDosHeader->e_magic);
    }
    printf("\n");
  }
}


void PEParsed::ParseImageFileHeader(bool verbose)
{
  if (verbose)
  {
    printf("Parsing Image File Header...\n");
    if (IMAGE_FILE_MACHINE_I386 != imageFileHeader->Machine)
    {
      printf("FILE IS NOT FOR i386. MACHINE IS 0x%x\n",
             imageFileHeader->Machine);
    }

    WORD characteristics = imageFileHeader->Characteristics;

    if (!(characteristics & IMAGE_FILE_EXECUTABLE_IMAGE))
    {
      printf("IMAGE_FILE_EXECUTABLE_IMAGE CHARACTERISTICS IS NOT SET. THIS IS NOT EXECUTABLE FILE\n");
    }
    if (characteristics & IMAGE_FILE_DEBUG_STRIPPED)
    {
      printf("IMAGE_FILE_DEBUG_STRIPPED characteristic is set\n");
    }
    if (characteristics & IMAGE_FILE_RELOCS_STRIPPED)
    {
      printf("IMAGE_FILE_RELOCS_STRIPPED characteristic is set\n");
    }
    printf("\n");
  }
}


void PEParsed::ParseImageOptionalHeader(bool verbose)
{
  if (verbose)
  {
    printf("Parsing Image Optional Header...\n");
    if (imageOptionalHeader->Magic != IMAGE_NT_OPTIONAL_HDR32_MAGIC)
    {
      printf("OPTIONAL HEADER MAGIC NUMBER IS NOT FOR 32 PE. IT IS 0x%x\n",
             imageOptionalHeader->Magic);
    }
    if (0 == imageOptionalHeader->AddressOfEntryPoint)
    {
      printf("ADDRESS OF ENTRY POINT IS 0\n");
    }
    else
    {
      printf("Virtual Address of Entry Point is 0x%x\n",
             imageOptionalHeader->AddressOfEntryPoint + imageOptionalHeader->ImageBase);
    }

    DWORD sectionAlignment = imageOptionalHeader->SectionAlignment;
    DWORD fileAlignment = imageOptionalHeader->FileAlignment;

    printf("File Alignment is 0x%x\n",
           fileAlignment);
    printf("Section Alignment is 0x%x\n",
           sectionAlignment);
    if (sectionAlignment < 0x1000)
    {
      printf("SECTION ALIGNMENT IS LESS THEN 0x1000. It may be driver\n");
    }
    if (fileAlignment < 0x200)
    {
      printf("FILE ALIGNMENT IS LESS THEN 0x200. It may be driver\n");
    }
    if (sectionAlignment < fileAlignment)
    {
      printf("SECTION ALIGNMENT IS LESS THEN FILE ALIGNMENT\n");
    }
    if (imageOptionalHeader->CheckSum != 0)
    {
      // TODO: calc checksum
      printf("CHECKSUM IS 0x%x\n",
             imageOptionalHeader->CheckSum);
    }
    if (imageOptionalHeader->Subsystem != 2 &&
        imageOptionalHeader->Subsystem != 3)
    {
      printf("SUBSYSTEM IS NOT WINDOWS_GUI or WINDOWS_CUI. It is %d\n",
             imageOptionalHeader->Subsystem);
    }
    if (imageOptionalHeader->SizeOfStackCommit > imageOptionalHeader->SizeOfStackReserve)
    {
      printf("SizeOfStackCommit > SizeOfStackReserve\n");
    }
    if (imageOptionalHeader->SizeOfHeapCommit > imageOptionalHeader->SizeOfHeapReserve)
    {
      printf("SizeOfHeapCommit > SizeOfHeapReserve\n");
    }

    IMAGE_DATA_DIRECTORY securityDirectory = imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_SECURITY];

    if (securityDirectory.VirtualAddress != 0)
    {
      printf("There is Security Directory. DON'T PATCH THIS FILE!\n");
    }

    IMAGE_DATA_DIRECTORY comRuntimeDescriptor = imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR];

    if (comRuntimeDescriptor.VirtualAddress != 0)
    {
      printf("This is .NET executable. DON'T PATCH THIS FILE!\n");
    }

    printf("\n");
  }
}


void PEParsed::ParseHeaderForFreeSpace(bool verbose)
{
  // don't forget about DEP
  if (verbose)
  {
    printf("Check header for free space:\n");
  }

  // right after section table
  headerFreeSpaceAddress = imageDosHeader->e_lfanew +
                           sizeof(IMAGE_NT_SIGNATURE) +
                           sizeof(IMAGE_FILE_HEADER) +
                           sizeof(IMAGE_OPTIONAL_HEADER) +
                           imageFileHeader->NumberOfSections * sizeof(IMAGE_SECTION_HEADER);

  IMAGE_DATA_DIRECTORY boundImportDirectory = imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT];

  // check if there is a bound import table after section table
  if (boundImportDirectory.VirtualAddress != 0)
  {
    IMAGE_BOUND_IMPORT_DESCRIPTOR* imageBoundImportDescriptor;
    DWORD lastNameStringOffset = 0; // must be last entry dll name, but who knows... we'll check it
    DWORD curNameStringOffset;

    imageBoundImportDescriptor = (IMAGE_BOUND_IMPORT_DESCRIPTOR*)(fileMapAddress + RvaToFileOffset(boundImportDirectory.VirtualAddress));
    while (imageBoundImportDescriptor->OffsetModuleName != 0)
    {
      curNameStringOffset = imageBoundImportDescriptor->OffsetModuleName;
      lastNameStringOffset = max(lastNameStringOffset, curNameStringOffset);
      ++ imageBoundImportDescriptor;
    }
    if (0 == lastNameStringOffset)
    {
      // empty bound import table. Strange, but it is
      headerFreeSpaceAddress += sizeof(IMAGE_BOUND_IMPORT_DESCRIPTOR);
    }
    else
    {
      BYTE index = (BYTE)lastNameStringOffset;

      while (*(BYTE*)(fileMapAddress + headerFreeSpaceAddress + (index ++)) != 0) {}
      headerFreeSpaceAddress += index;
    }
  }

  // find first "real" (which file size is not 0) section file address
  DWORD firstSectionFileAddress;
  IMAGE_SECTION_HEADER* curSection = imageSectionHeader;
  DWORD curAddress = headerFreeSpaceAddress;

  isXCodeInHeader = false;
  while (0 == curSection->PointerToRawData)
  {
    ++ curSection;
  }
  firstSectionFileAddress = curSection->PointerToRawData;
  // check if this file already has X code
  while (curAddress < firstSectionFileAddress)
  {
    if (*(BYTE*)(fileMapAddress + curAddress) != 0)
    {
      isXCodeInHeader = true;
      break;
    }
    ++ curAddress;
  }
  if (!isXCodeInHeader)
  {
    headerFreeSpaceSize = firstSectionFileAddress - headerFreeSpaceAddress;
  }

  if (isXCodeInHeader)
  {
    printf(	"HEADER MAY CONTAIN X CODE. CHECK HEADER ADDRESS SPECE AFTER 0x%x\n",
      headerFreeSpaceAddress);
  }
  else
  {
    printf(	"Header free space size is 0x%x. Address is 0x%x\n",
      headerFreeSpaceSize,
      headerFreeSpaceAddress);
    if (imageOptionalHeader->SizeOfHeaders < firstSectionFileAddress)
    {
      printf("SizeOfHeaders is less then actual header size. Use -header [xCodeSize] option to set correct value\n");
    }
  }
  printf("\n");
}


void PEParsed::ParseSectionsForFreeSpace()
{
  printf("*****************************************************\n");
  printf("Sections:\t(be careful with free space r_off)\n");
  printf("May need to patch characteristics and v_sz. Use -sectiontail option\n");
  printf("\n");
  for (int sectionIndex = 0; sectionIndex < imageFileHeader->NumberOfSections; ++ sectionIndex)
  {
    IMAGE_SECTION_HEADER curSection = imageSectionHeader[sectionIndex];

    printf(	"Section %d: ",
      sectionIndex);
    printf(	"name: %s",
      curSection.Name);
    //		printf( "v_a: 0x%x, ",
    //				curSection.VirtualAddress);
    //		printf(	"v_sz: 0x%x, ",
    //				curSection.Misc.VirtualSize);
    //		printf( "r_off: 0x%x, ",
    //				curSection.PointerToRawData);
    //		printf(	"f_sz: 0x%x.",
    //			curSection.SizeOfRawData);
    if (imageOptionalHeader->AddressOfEntryPoint >= curSection.VirtualAddress &&
      imageOptionalHeader->AddressOfEntryPoint < curSection.VirtualAddress + curSection.Misc.VirtualSize)
    {
      printf("\tCODE SECTION");
    }
    if (0 == strncmp("INIT", (char*)curSection.Name, 4))
    {
      printf("\tIT MAY BE DRIVER");
    }
    if (curSection.Misc.VirtualSize != 0 &&
      curSection.SizeOfRawData >= curSection.Misc.VirtualSize + imageOptionalHeader->FileAlignment)
    {
      printf("\tCONTAINS OVERLAY");
    }
    printf("\n");

    DWORD characteristics = curSection.Characteristics;

    if (characteristics & IMAGE_SCN_MEM_SHARED)
    {
      printf("IMAGE_SCN_MEM_SHARED flag is set\n");
    }
    if (characteristics & IMAGE_SCN_MEM_DISCARDABLE)
    {
      printf("IMAGE_SCN_MEM_DISCARDABLE flag is set\n");
    }
    if (!(characteristics & IMAGE_SCN_MEM_READ) && !(characteristics & IMAGE_SCN_MEM_EXECUTE))
    {
      printf("Nor IMAGE_SCN_MEM_READ not IMAGE_SCN_MEM_EXECUTE flags are set\n");
    }
    if (!(characteristics & IMAGE_SCN_CNT_CODE) && !(characteristics & IMAGE_SCN_CNT_INITIALIZED_DATA))
    {
      printf("Nor IMAGE_SCN_CNT_CODE not IMAGE_SCN_CNT_INITIALIZED_DATA flags are set\n");
    }
    if (characteristics & IMAGE_SCN_MEM_WRITE)
    {
      printf("IMAGE_SCN_MEM_WRITE flag is set (optional)\n");
    }

    DWORD startSectionAddress;
    DWORD endSectionAddress;
    DWORD addressesDifference = 0;
    DWORD curAddress;

    startSectionAddress = curSection.PointerToRawData;
    if (sectionIndex + 1 < imageFileHeader->NumberOfSections)
    {
      if (imageSectionHeader[sectionIndex + 1].PointerToRawData - curSection.PointerToRawData < curSection.SizeOfRawData)
      {
        printf("ERROR! RawSize field value is greater then actual section size\n");
      }
      else if (imageSectionHeader[sectionIndex + 1].PointerToRawData - curSection.PointerToRawData > curSection.SizeOfRawData)
      {
        printf("RawSize field value is less then actual section size\n");
      }

      endSectionAddress = imageSectionHeader[sectionIndex + 1].PointerToRawData - 1;
      // what if section virtual size is less then section file size
      addressesDifference = (endSectionAddress + 1 - startSectionAddress) - (imageSectionHeader[sectionIndex + 1].VirtualAddress - curSection.VirtualAddress);
      addressesDifference = (addressesDifference < 0) ? 0 : addressesDifference;
    }
    else
    {
      endSectionAddress = imageSectionHeader[sectionIndex].PointerToRawData + imageSectionHeader[sectionIndex].SizeOfRawData - 1;
    }
    curAddress = endSectionAddress;
    while (curAddress >= startSectionAddress)
    {
      if (*(BYTE*)(fileMapAddress + curAddress) != 0)
      {
        break;
      }
      -- curAddress;
    }

    if (endSectionAddress - curAddress - addressesDifference <= 0)
    {
      printf("There is not free space\n");
    }
    else
    {
      printf(	"Free space: r_off = 0x%x, size = 0x%x\n",
        curAddress + 1,
        endSectionAddress - curAddress - addressesDifference);
    }

    printf("\n");
  }
}


bool PEParsed::ParseImageImportDirectory()
{
  bool canPatch = true;
  DWORD importTableRva = imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress;

  if (0 == importTableRva)
  {
    printf("COULD NOT FIND IMAGE IMPORT DIRECTORY\n");
    return false;
  }

  // Search for import section
  DWORD startAddress;
  DWORD endAddress;

  for (int i = 0; i < imageFileHeader->NumberOfSections; ++ i)
  {
    IMAGE_SECTION_HEADER curSection = imageSectionHeader[i];

    if ((DWORD)importTableRva >= curSection.VirtualAddress &&
      (DWORD)importTableRva < curSection.VirtualAddress + curSection.Misc.VirtualSize)
    {
      printf(	"%s (virtual address = 0x%x) is import section.",
        curSection.Name, curSection.VirtualAddress);
      if (! (curSection.Characteristics & IMAGE_SCN_MEM_WRITE))
      {
        printf(" IT IS NOT WRITABLE!");
        canPatch = false;
      }
      printf("\n");
      startAddress = curSection.PointerToRawData;
      endAddress = startAddress + curSection.SizeOfRawData;
      break;
    }
  }

  imageImportDescriptor = (IMAGE_IMPORT_DESCRIPTOR*)(fileMapAddress + RvaToFileOffset(importTableRva));

  IMAGE_IMPORT_DESCRIPTOR* curIID = imageImportDescriptor;
  char* dllName;
  DWORD neededFreeSpaceSize;

  while (true)
  {
    // we cannot just check for FirstThunk (for ex.), because it may be fake record with TimeDateStamp is -1
    if (0 == curIID->OriginalFirstThunk &&
      0 == curIID->TimeDateStamp &&
      0 == curIID->ForwarderChain &&
      0 == curIID->Name &&
      0 == curIID->FirstThunk)
    {
      // last entry
      break;
    }
    dllName = (char*)(fileMapAddress + RvaToFileOffset(curIID->Name));
    if (0 == _stricmp("hal.dll", dllName))
    {
      printf("APPLICATION EXPORTS FUNCTIONS FROM hal.dll\n");
      canPatch = false;
    }
    if (0 == _stricmp("ntoskrnl.exe", dllName))
    {
      printf("APPLICATION EXPORTS FUNCTIONS FROM ntoskrnl.exe\n");
      canPatch = false;
    }

    ++ numberOfImportedDlls;
    ++ curIID;
  }

  printf(	"%d dlls are imported\n",
    numberOfImportedDlls);
  neededFreeSpaceSize = sizeof(IMAGE_IMPORT_DESCRIPTOR) * (numberOfImportedDlls + 2) + (2 + 10);
  printf(	"0x%x bytes are needded for injecting\n",
    neededFreeSpaceSize); // 2+10 - for IMAGE_IMPORT_BY_NAME (10 - for function name)

  // Search for free space
  DWORD curAddress = startAddress;
  DWORD tempSize;
  DWORD tempAddress;
  DWORD maxFreeSpaceSize = 0;

  while (curAddress < endAddress)
  {
    if (0 == *(BYTE*)(fileMapAddress + curAddress))
    {
      tempSize = 0;
      tempAddress = curAddress;
      while (0 == *(BYTE*)(fileMapAddress + curAddress))
      {
        ++ tempSize;
        ++ curAddress;
      }
      if (tempSize > maxFreeSpaceSize)
      {
        maxFreeSpaceSize = tempSize;
        maxFreeSpaceIATAddress = tempAddress;
      }
      -- curAddress;
    }
    ++ curAddress;
  }

  maxFreeSpaceSize -= 0x10; // maybe some structure with zero-fields is part of this free space
  maxFreeSpaceIATAddress += 0x10;

  if (maxFreeSpaceSize > 0)
  {
    printf(	"Max free space: size - 0x%x, address - 0x%x\n",
      maxFreeSpaceSize,
      maxFreeSpaceIATAddress);
  }

  if (maxFreeSpaceSize < neededFreeSpaceSize)
  {
    printf("THERE IS NOT ENOUGH FREE SPACE FOR DLL INJECTION\n");
    canPatch = false;
  }

  return canPatch;
}


bool PEParsed::PatchHeader(DWORD xCodeSize)
{
  // Set correct value to SizeOfHeaders.
  // If xCodeSize == -1, SizeOfHeaders will be set to first section r_off

  ParseHeaderForFreeSpace();
  if (isXCodeInHeader)
  {
    strcpy_s(	lastError,
      LAST_ERROR_SIZE,
      "Header contains X code already");
    return false;
  }
  if (xCodeSize != -1 &&
    xCodeSize > headerFreeSpaceSize)
  {
    strcpy_s(	lastError,
      LAST_ERROR_SIZE,
      "X code size is greater then header free space size");
    return false;
  }
  if (-1 == xCodeSize)
  {
    // find first "real" (which file size is not 0) section file address
    DWORD firstSectionFileAddress;
    IMAGE_SECTION_HEADER* curSection = imageSectionHeader;

    while (0 == curSection->PointerToRawData)
    {
      ++ curSection;
    }
    firstSectionFileAddress = curSection->PointerToRawData;

    imageOptionalHeader->SizeOfHeaders = firstSectionFileAddress;
  }
  else
  {
    imageOptionalHeader->SizeOfHeaders = headerFreeSpaceAddress + xCodeSize;
  }

  return true;
}


bool PEParsed::PatchSectionTail(int sectionIndex, bool isSetWrite)
{
  // reset IMAGE_SCN_MEM_SHARED and IMAGE_SCN_MEM_DISCARDABLE,
  // set IMAGE_SCN_MEM_READ and IMAGE_SCN_CNT_CODE, set IMAGE_SCN_MEM_WRITE if isSetWrite;
  // VirtualSize will be set to (next section v_a - current section v_a)

  if (sectionIndex < 0 ||
    sectionIndex >= imageFileHeader->NumberOfSections)
  {
    strcpy_s(	lastError,
      LAST_ERROR_SIZE,
      "Incorrect sectionIndex value");
    return false;
  }

  IMAGE_SECTION_HEADER curSection = imageSectionHeader[sectionIndex];

  if (curSection.Misc.VirtualSize != 0 &&
    curSection.SizeOfRawData >= curSection.Misc.VirtualSize + imageOptionalHeader->FileAlignment)
  {
    strcpy_s(	lastError,
      LAST_ERROR_SIZE,
      "Section contains overlay");
    return false;
  }

  imageSectionHeader[sectionIndex].Characteristics &= ~IMAGE_SCN_MEM_SHARED;
  imageSectionHeader[sectionIndex].Characteristics &= ~IMAGE_SCN_MEM_DISCARDABLE;
  imageSectionHeader[sectionIndex].Characteristics |= IMAGE_SCN_MEM_READ;
  imageSectionHeader[sectionIndex].Characteristics |= IMAGE_SCN_CNT_CODE;
  if (isSetWrite)
  {
    imageSectionHeader[sectionIndex].Characteristics |= IMAGE_SCN_MEM_WRITE;
  }

  if (curSection.Misc.VirtualSize != 0 &&
    curSection.Misc.VirtualSize < curSection.SizeOfRawData)
  {
    if (sectionIndex + 1 < imageFileHeader->NumberOfSections)
    {
      imageSectionHeader[sectionIndex].Misc.VirtualSize = imageSectionHeader[sectionIndex + 1].VirtualAddress - curSection.VirtualAddress;
    }
    else
    {
      if (curSection.Misc.VirtualSize < curSection.SizeOfRawData)
      {
        imageSectionHeader[sectionIndex].Misc.VirtualSize = curSection.SizeOfRawData;
      }
    }
  }

  return true;
}


bool PEParsed::PatchIAT(char* dllName, char* functionName)
{
  // Place X dll name and 4 IMAGE_THUNK_DATA ((imported function + zero-fields) * 2 for OriginalFirstThunk and FirstThunk)
  // in space of former IAT
  // Place new IAT (old IAT + new dll) and IMAGE_IMPORT_BY_NAME in free space

  if (!ParseImageImportDirectory())
  {
    return false;
  }

  DWORD oldImportTableAddress = (DWORD)imageImportDescriptor;
  DWORD oldImportTableSize = sizeof(IMAGE_IMPORT_DESCRIPTOR) * (numberOfImportedDlls + 1);
  DWORD freeSpaceAddress = fileMapAddress + maxFreeSpaceIATAddress;

  // Copy old import table into new place
  printf(	"New address of import table - 0x%x\n",
    maxFreeSpaceIATAddress);
  memcpy(	(void*)freeSpaceAddress,
    imageImportDescriptor,
    sizeof(IMAGE_IMPORT_DESCRIPTOR) * numberOfImportedDlls);
  freeSpaceAddress += sizeof(IMAGE_IMPORT_DESCRIPTOR) * numberOfImportedDlls;

  // Copy xDll name in space of old import table
  memcpy(	(void*)oldImportTableAddress,
    dllName,
    strlen(dllName) + 1);

  oldImportTableAddress += (strlen(dllName) + 1);

  // Create IMAGE_THUNK_DATA array
  IMAGE_THUNK_DATA thunkData;
  WORD ordinal = 1;

  memcpy(	(void*)(freeSpaceAddress + 2 * sizeof(IMAGE_IMPORT_DESCRIPTOR)),
    &ordinal,
    2);
  memcpy(	(void*)(freeSpaceAddress + 2 * sizeof(IMAGE_IMPORT_DESCRIPTOR) + 2),
    functionName,
    strlen(functionName));

  // thunkData.u1.Ordinal = 0x1 | 0x80000000; // import by ordinal
  thunkData.u1.AddressOfData = FileOffsetToRva(freeSpaceAddress + 2 * sizeof(IMAGE_IMPORT_DESCRIPTOR) - fileMapAddress);
  memcpy(	(void*)oldImportTableAddress,
    &thunkData,
    sizeof(IMAGE_THUNK_DATA));
  memcpy(	(void*)(oldImportTableAddress + 2 * sizeof(IMAGE_THUNK_DATA)),
    &thunkData,
    sizeof(IMAGE_THUNK_DATA));
  ZeroMemory(	&thunkData,
    sizeof(IMAGE_THUNK_DATA));
  memcpy(	(void*)(oldImportTableAddress + sizeof(IMAGE_THUNK_DATA)),
    &thunkData,
    sizeof(IMAGE_THUNK_DATA));
  memcpy(	(void*)(oldImportTableAddress + 3 * sizeof(IMAGE_THUNK_DATA)),
    &thunkData,
    sizeof(IMAGE_THUNK_DATA));

  // Create IMAGE_IMPORT_DESCRIPTOR structure for X dll
  IMAGE_IMPORT_DESCRIPTOR xDll;

  xDll.Characteristics = FileOffsetToRva(oldImportTableAddress - fileMapAddress);
  xDll.TimeDateStamp = 0;
  xDll.ForwarderChain = 0;                                   
  xDll.Name = FileOffsetToRva((DWORD)imageImportDescriptor - (DWORD)fileMapAddress);
  xDll.FirstThunk = FileOffsetToRva(oldImportTableAddress + 2 * sizeof(IMAGE_THUNK_DATA) - fileMapAddress);

  printf(	"X function address will be in: fo - 0x%x, rva - 0x%x\n",
    oldImportTableAddress + 2 * sizeof(IMAGE_THUNK_DATA) - fileMapAddress,
    FileOffsetToRva(oldImportTableAddress + 2 * sizeof(IMAGE_THUNK_DATA) - fileMapAddress));

  // Add new descriptor with X dll
  memcpy(	(void*)freeSpaceAddress,
    &xDll,
    sizeof(IMAGE_IMPORT_DESCRIPTOR));
  freeSpaceAddress += sizeof(IMAGE_IMPORT_DESCRIPTOR);

  // Create final zero-fields IMAGE_IMPORT_DESCRIPTOR structure
  IMAGE_IMPORT_DESCRIPTOR finalRecord;

  ZeroMemory(	&finalRecord,
    sizeof(IMAGE_IMPORT_DESCRIPTOR));
  memcpy(	(void*)freeSpaceAddress,
    &finalRecord,
    sizeof(IMAGE_IMPORT_DESCRIPTOR));

  // Fix address in DataDirectory
  DWORD newImportTableRVA = FileOffsetToRva(maxFreeSpaceIATAddress);

  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress = newImportTableRVA;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].Size = sizeof(IMAGE_IMPORT_DESCRIPTOR) * (numberOfImportedDlls + 1);

  return true;
}
*/
