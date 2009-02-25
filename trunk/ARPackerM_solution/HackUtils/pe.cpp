// PE and COFF File Format Specification
// Pietrek M. - Peering Inside the PE
// Kris Kaspersky "Hacker Source-missed Debugging Uncovered"
// http://www.wasm.ru/article.php?article=1005005

#include "pe.h"


PE::PE(DWORD _fileMapAddress) :
  fileMapAddress(_fileMapAddress), parsed(FALSE),
  imageDosHeader(NULL), imageFileHeader(NULL), imageOptionalHeader(NULL), imageSectionHeader(NULL)
{
  lastError = new CHAR[LAST_ERROR_SIZE];
  strcpy_s(lastError,
           LAST_ERROR_SIZE,
           "No error");
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
  imageDosHeader = (IMAGE_DOS_HEADER*)fileMapAddress;
  imageFileHeader = (IMAGE_FILE_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE));
  imageOptionalHeader = (IMAGE_OPTIONAL_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE) + sizeof(IMAGE_FILE_HEADER));
  imageSectionHeader = (IMAGE_SECTION_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE) + sizeof(IMAGE_FILE_HEADER) + sizeof(IMAGE_OPTIONAL_HEADER));

  parsed = TRUE;
  
  return TRUE;
}


BOOL PE::SetDefaultValues()
{
// IMAGE_DOS_HEADER and MS-DOS stub
  imageDosHeader = (IMAGE_DOS_HEADER*)fileMapAddress;

  imageDosHeader->e_magic = IMAGE_DOS_SIGNATURE; // "MZ" - search "PE"
                                                 // "ZM" (undocumented) - show MS-DOS stub, so following values make sense
                                                 // other - undefined (?)
  // TODO: fix!!! if IMAGE_DOS_SIGNATURE == e_magic all values (except e_lfanew) can be set to 0 for Windows
  imageDosHeader->e_cblp = 0; // any
  imageDosHeader->e_cp = 1; // 1 <= x <= N (sometimes file size / 512 == N)
  imageDosHeader->e_crlc = 0; // ?
  imageDosHeader->e_cparhdr = 4; // e_cparhdr * PARAGRAPH_SIZE - base address
  imageDosHeader->e_minalloc = 0; // ?
  imageDosHeader->e_maxalloc = 0; // 0xFFFF traditionally
  imageDosHeader->e_ss = 0; // any if program does not use stack
  imageDosHeader->e_sp = 0; // any if program does not use stack
  imageDosHeader->e_csum = 0; // any
  imageDosHeader->e_ip = 0; // e_cparhdr * PARAGRAPH_SIZE + e_ip - address of entry point
  imageDosHeader->e_cs = 0; // ?
  imageDosHeader->e_lfarlc = 0; // ?
  imageDosHeader->e_ovno = 0; // ?
  ZeroMemory(imageDosHeader->e_res, 4); // any
  imageDosHeader->e_oemid = 0; // ?
  imageDosHeader->e_oeminfo = 0; // ?
  ZeroMemory(imageDosHeader->e_res2, 10); // any

  // push cs
  // pop ds
  // mov dx E
  // mov ah 9
  // int 21 ; print string
  // mov ax 4C01
  // int 21 ; terminate process with return code
  int msdosStubSize = 56;
  char msdosStub[] = "\x0E\x1F\xBA\x0E\x00\xB4\x09\xCD\x21\xB8\x01\x4c\xCD\x21"
                     "This program cannot be run in DOS mode.\x0D\x0A\x24";

  memcpy_s((void*)(fileMapAddress + imageDosHeader->e_cparhdr * PARAGRAPH_SIZE + imageDosHeader->e_ip), msdosStubSize,
           msdosStub, msdosStubSize);

  imageDosHeader->e_lfanew = sizeof(IMAGE_DOS_HEADER) + msdosStubSize; // 0x3C - address of "PE"

//////////////////////////////////////////////////////////////////////////

// IMAGE_FILE_HEADER
  *(DWORD*)(fileMapAddress + imageDosHeader->e_lfanew) = IMAGE_NT_SIGNATURE;
  imageFileHeader = (IMAGE_FILE_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE));

  imageFileHeader->Machine = IMAGE_FILE_MACHINE_I386; // only
  imageFileHeader->NumberOfSections = 0; // see AddSection()
  imageFileHeader->TimeDateStamp = 0; // any
  imageFileHeader->PointerToSymbolTable = 0; // any
  imageFileHeader->NumberOfSymbols = 0; // any
  imageFileHeader->SizeOfOptionalHeader = sizeof(IMAGE_OPTIONAL_HEADER); // used to find section table
  imageFileHeader->Characteristics = IMAGE_FILE_EXECUTABLE_IMAGE; // need also IMAGE_FILE_DLL for dll

//////////////////////////////////////////////////////////////////////////

// IMAGE_OPTIONAL_HEADER
  imageOptionalHeader = (IMAGE_OPTIONAL_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE) + sizeof(IMAGE_FILE_HEADER));

  imageOptionalHeader->Magic = IMAGE_NT_OPTIONAL_HDR32_MAGIC; // only
  imageOptionalHeader->MajorLinkerVersion = 0; // any
  imageOptionalHeader->MinorLinkerVersion = 0; // any
  imageOptionalHeader->SizeOfCode = 0; // any
  imageOptionalHeader->SizeOfInitializedData = 0; // any
  imageOptionalHeader->SizeOfUninitializedData = 0; // any
  imageOptionalHeader->AddressOfEntryPoint = 0; // INIT ME
  imageOptionalHeader->BaseOfCode = 0; // any
  imageOptionalHeader->BaseOfData = 0; // any
  imageOptionalHeader->ImageBase = 0x400000; // default for Win32
  imageOptionalHeader->SectionAlignment = 0x1000; // x >= 0x1000 and sa >= fa
  imageOptionalHeader->FileAlignment = 0x200; // x >= 0x200 and sa >= fa
  imageOptionalHeader->MajorOperatingSystemVersion = 0; // any
  imageOptionalHeader->MinorOperatingSystemVersion = 0; // any
  imageOptionalHeader->MajorImageVersion = 0; // any
  imageOptionalHeader->MinorImageVersion = 0; // any
  imageOptionalHeader->MajorSubsystemVersion = 3; // min value is 3.1 (3 0x10)
  imageOptionalHeader->MinorSubsystemVersion = 0x10;
  imageOptionalHeader->Win32VersionValue = 0; // any
  imageOptionalHeader->SizeOfImage = 0; // see AddSection()
  imageOptionalHeader->SizeOfHeaders = 0x400; // better to use min(pFirstSection->RawOffset, pFirstSection->va)
  imageOptionalHeader->CheckSum = 0; // any, is checked only for some system libs and kernel (dll in boot time?)
  imageOptionalHeader->Subsystem = IMAGE_SUBSYSTEM_WINDOWS_GUI; // without console window
  imageOptionalHeader->DllCharacteristics = 0; // any
  imageOptionalHeader->SizeOfStackReserve = 0; // default value is set
  imageOptionalHeader->SizeOfStackCommit = 0; // default value is set
  imageOptionalHeader->SizeOfHeapReserve = 0; // default value is set
  imageOptionalHeader->SizeOfHeapCommit = 0; // default value is set
  imageOptionalHeader->LoaderFlags = 0; // any
  imageOptionalHeader->NumberOfRvaAndSizes = IMAGE_NUMBEROF_DIRECTORY_ENTRIES; // always should be 0x10

// DataDirectory
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_EXPORT].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_EXPORT].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_RESOURCE].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_RESOURCE].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_EXCEPTION].VirtualAddress = 0; // any - not used in Intel
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_EXCEPTION].Size = 0; // any - not used in Intel
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_SECURITY].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_SECURITY].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_BASERELOC].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_BASERELOC].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_DEBUG].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_DEBUG].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_ARCHITECTURE].VirtualAddress = 0; // any
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_ARCHITECTURE].Size = 0; // any
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_GLOBALPTR].VirtualAddress = 0; // any - not used in Intel
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_GLOBALPTR].Size = 0; // any - not used in Intel
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_TLS].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_TLS].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IAT].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IAT].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR].VirtualAddress = 0;
  imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR].Size = 0;
  imageOptionalHeader->DataDirectory[IMAGE_NUMBEROF_DIRECTORY_ENTRIES - 1].VirtualAddress = 0; // last element
  imageOptionalHeader->DataDirectory[IMAGE_NUMBEROF_DIRECTORY_ENTRIES - 1].Size = 0; // last element

//////////////////////////////////////////////////////////////////////////

// IMAGE_SECTION_HEADER
  imageSectionHeader = (IMAGE_SECTION_HEADER*)(fileMapAddress + imageDosHeader->e_lfanew + sizeof(IMAGE_NT_SIGNATURE) + sizeof(IMAGE_FILE_HEADER) + sizeof(IMAGE_OPTIONAL_HEADER));

  ZeroMemory(imageSectionHeader, sizeof(IMAGE_SECTION_HEADER));

  parsed = TRUE;

  return TRUE;
}


const IMAGE_FILE_HEADER* PE::GetImageFileHeader()
{
  if (!parsed || NULL == imageFileHeader)
  {
    strcpy_s(	lastError,
              LAST_ERROR_SIZE,
              "PE object was not initialized");
    return NULL;
  }

  return imageFileHeader;
}


const IMAGE_SECTION_HEADER* PE::AddSection(const CHAR* name, DWORD size)
{
  if (!parsed ||
      NULL == imageFileHeader || NULL == imageOptionalHeader)
  {
    strcpy_s(	lastError,
              LAST_ERROR_SIZE,
              "PE object was not initialized");
    return NULL;
  }

  DWORD sectionIndex = imageFileHeader->NumberOfSections ++;
  IMAGE_SECTION_HEADER* newSection = &imageSectionHeader[sectionIndex];

  ZeroMemory(newSection->Name, IMAGE_SIZEOF_SHORT_NAME);
  memcpy_s((void*)newSection->Name, IMAGE_SIZEOF_SHORT_NAME,
           name, strlen(name));
  if (0 == sectionIndex)
  {
    // first section
    newSection->VirtualAddress = AlignUp(imageOptionalHeader->SizeOfHeaders, imageOptionalHeader->SectionAlignment);
    newSection->PointerToRawData = 0x400; // usually header is about 0x300
  }
  else
  {
    IMAGE_SECTION_HEADER* lastSection = &imageSectionHeader[sectionIndex - 1];

    newSection->VirtualAddress = AlignUp(lastSection->VirtualAddress + lastSection->Misc.VirtualSize, imageOptionalHeader->SectionAlignment);
    newSection->PointerToRawData = AlignUp(lastSection->PointerToRawData + lastSection->SizeOfRawData, imageOptionalHeader->FileAlignment);
  }
  newSection->Misc.VirtualSize = AlignUp(size, imageOptionalHeader->SectionAlignment);
  newSection->SizeOfRawData = AlignUp(size, imageOptionalHeader->FileAlignment);
  newSection->PointerToRelocations = 0; // any - used in obj files only
  newSection->PointerToLinenumbers = 0; // any - used in obj files only
  newSection->NumberOfRelocations = 0; // any - used in obj files only
  newSection->NumberOfLinenumbers = 0; // any - used in obj files only
  newSection->Characteristics = IMAGE_SCN_MEM_READ; //IMAGE_SCN_CNT_CODE | IMAGE_SCN_MEM_EXECUTE;

  ZeroMemory(&imageSectionHeader[sectionIndex + 1], sizeof(IMAGE_SECTION_HEADER));

  imageOptionalHeader->SizeOfImage = newSection->VirtualAddress + AlignUp(newSection->Misc.VirtualSize, imageOptionalHeader->SectionAlignment);

  return newSection;
}


const IMAGE_SECTION_HEADER* PE::GetSection(DWORD index)
{
  if (!parsed ||
      NULL == imageFileHeader || NULL == imageOptionalHeader)
  {
    strcpy_s(	lastError,
              LAST_ERROR_SIZE,
              "PE object was not initialized");
    return NULL;
  }

  if (index >= imageFileHeader->NumberOfSections)
  {
    strcpy_s(	lastError,
              LAST_ERROR_SIZE,
              "Wrong section index");
    return NULL;
  }

  return &imageSectionHeader[index];
}


BOOL PE::AddImportDll(const CHAR* dllName, const CHAR** funcNames, DWORD funcAmount)
{
  strcpy_s(	lastError,
            LAST_ERROR_SIZE,
            "Not implemented");
  return FALSE;

/*
// Place dll name and ((funcAmount + 1) * 2) IMAGE_THUNK_DATA (funcAmount imported functions + zero-fields for OriginalFirstThunk and FirstThunk)
// in space of former IAT
// Place new IAT (old IAT + new dll) and IMAGE_IMPORT_BY_NAME in free space

  DWORD importTableRva = imageOptionalHeader->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress;

  if (0 == importTableRva)
  {
    // TODO: create
  }

  IMAGE_IMPORT_DESCRIPTOR* imageImportDescriptor = (IMAGE_IMPORT_DESCRIPTOR*)(fileMapAddress + RvaToFileOffset(importTableRva));

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

  return TRUE;
*/
}

//////////////////////////////////////////////////////////////////////////

DWORD PE::AlignDown(DWORD addr, DWORD align) const
{
  return (addr & ~(align - 1));
}


DWORD PE::AlignUp(DWORD addr, DWORD align) const
{
  return ((addr & (align - 1)) ? AlignDown(addr, align) + align : addr);
}


DWORD PE::RvaToFileOffset(DWORD rva)
{
  if (NULL == imageFileHeader || NULL == imageSectionHeader)
  {
    strcpy_s(	lastError,
              LAST_ERROR_SIZE,
              "PE object was not initialized");
    return -1;
  }

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
}


DWORD PE::FileOffsetToRva(DWORD fileOffset)
{
  if (NULL == imageFileHeader || NULL == imageSectionHeader)
  {
    strcpy_s(	lastError,
              LAST_ERROR_SIZE,
              "PE object was not initialized");
    return -1;
  }
  
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
}
