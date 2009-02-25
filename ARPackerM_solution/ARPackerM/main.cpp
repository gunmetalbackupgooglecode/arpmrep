#include <stdio.h>
#include <windows.h>
#include "..\HackUtils\pe.h"


int main(int argc, char** argv)
{
// open original file
  char* fileName = argv[1];
//  char* fileName = "c:\\Develop\\arpmrep\\ARPackerM_solution\\Debug\\test.exe";
  HANDLE hFile;
  DWORD fileSize;

  hFile = CreateFile(fileName,
                     GENERIC_READ,
                     0,
                     NULL,
                     OPEN_EXISTING,
                     FILE_ATTRIBUTE_READONLY,
                     NULL);
  if (INVALID_HANDLE_VALUE == hFile)
  {
    printf("Could not open file. Error = %d\n",
           GetLastError());
    return 1;
  }
  fileSize = GetFileSize(hFile, NULL);

  HANDLE hFileMap;
  
  hFileMap = CreateFileMapping(hFile,
                               NULL,
                               PAGE_READONLY,
                               0,
                               0,
                               NULL);
  if (NULL == hFileMap)
  {
    printf("Could not create file mapping. Error = %d\n",
           GetLastError());
    CloseHandle(hFile);
    return 1;
  }

  DWORD* fileMapAddress;

  fileMapAddress = (DWORD*)MapViewOfFile(hFileMap,
                                         FILE_MAP_READ,
                                         0,
                                         0,
                                         0);
  if (NULL == fileMapAddress)
  {
    printf("Could not map file. Error = %d\n",
           GetLastError());
    CloseHandle(hFileMap);
    CloseHandle(hFile);
    return 1;
  }
  
// create packed file
  char* packedFileName = argv[2];
//  char* packedFileName = "c:\\Develop\\arpmrep\\ARPackerM_solution\\Debug\\PackedStub_release.exe";
  HANDLE hPackedFile; 

  hPackedFile = CreateFile(packedFileName,
                           GENERIC_READ | GENERIC_WRITE, // need GENERIC_READ?
                           0,
                           NULL,
                           OPEN_EXISTING, //CREATE_ALWAYS,
                           FILE_ATTRIBUTE_NORMAL,
                           NULL);
  if (INVALID_HANDLE_VALUE == hPackedFile)
  {
    printf("Could not open file. Error = %d\n",
           GetLastError());
    return 1;
  }
  
/*
  DWORD packedFileSize = 0x666;
  DWORD packedFilePtr;
  
  packedFilePtr = SetFilePointer(hPackedFile,
                                 packedFileSize,
                                 NULL,
                                 FILE_BEGIN);
  if (INVALID_SET_FILE_POINTER == packedFilePtr)
  { 
    printf("Could not set file pointer. Error = %d\n",
           GetLastError());
    return 1;
  }
  
  if (!SetEndOfFile(hPackedFile))
  {
    printf("Could not set end of file. Error = %d\n",
            GetLastError());
    return 1;
  }
*/

  HANDLE hPackedFileMap;
  
  hPackedFileMap = CreateFileMapping(hPackedFile,
                                     NULL,
                                     PAGE_READWRITE,
                                     0,
                                     0,
                                     NULL);
  if (NULL == hPackedFileMap)
  {
    printf("Could not create file mapping. Error = %d\n",
           GetLastError());
    CloseHandle(hPackedFile);
    return 1;
  }

  DWORD* packedFileMapAddress;

  packedFileMapAddress = (DWORD*)MapViewOfFile(hPackedFileMap,
                                               FILE_MAP_ALL_ACCESS,
                                               0,
                                               0,
                                               0);
  if (NULL == packedFileMapAddress)
  {
    printf("Could not map file. Error = %d\n",
           GetLastError());
    CloseHandle(hPackedFileMap);
    CloseHandle(hPackedFile);
    return 1;
  }
  
  PE packedPE((DWORD)packedFileMapAddress);

//  packedPE.SetDefaultValues();
  packedPE.Parse();

  const IMAGE_SECTION_HEADER* packedSection = packedPE.AddSection(".arpm", fileSize);

  if (NULL == packedSection)
  {
    printf("PE::AddSection failed: %s\n", packedPE.GetLastError());
    UnmapViewOfFile(packedFileMapAddress);
    CloseHandle(hPackedFileMap);
    CloseHandle(hPackedFile);
    return 1;
  }

  DWORD packedSectionStart = packedSection->PointerToRawData;

  UnmapViewOfFile(packedFileMapAddress);
  CloseHandle(hPackedFileMap);

  DWORD packedFilePtr;

  packedFilePtr = SetFilePointer( hPackedFile,
                                  packedSectionStart,
                                  NULL,
                                  FILE_BEGIN);
  if (INVALID_SET_FILE_POINTER == packedFilePtr)
  { 
    printf( "Could not set file pointer. Error = %d\n",
            GetLastError());
    return 1;
  }

  DWORD bytesWritten;
  BOOL writeFileResult;

  writeFileResult = WriteFile(hPackedFile, fileMapAddress, fileSize, &bytesWritten, NULL);

  if (!writeFileResult)
  { 
    printf( "Could not write to file. Error = %d\n",
            GetLastError());
    return 1;
  }

  if (!SetEndOfFile(hPackedFile))
  {
    printf("Could not set end of file. Error = %d\n",
      GetLastError());
    return 1;
  }

//  CHAR* funcNames[1];
//  funcNames[0] = "TerminateProcess";
//  packedPE.AddImportDll("kernel32.dll", (const CHAR**)funcNames, 1);
  
// clean all
  CloseHandle(hPackedFile);

  UnmapViewOfFile(fileMapAddress);
  CloseHandle(hFileMap);
  CloseHandle(hFile);

  return 0;
}
