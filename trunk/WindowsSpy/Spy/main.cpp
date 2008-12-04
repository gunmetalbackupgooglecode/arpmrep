#include <stdio.h>
#include <Windows.h>
#include "PEParsed.h"


int main(int argc, char* argv[])
{
	// print help
	if (argc < 2)
	{
		printf("Usage: PEParse.exe [options]\n");
		
    printf(	"\t-file [filePath] - display file info\n");

		printf(	"\t-file [filePath] -header [xCodeSize] - set correct value to SizeOfHeaders. "
				"If xCodeSize is not specified, SizeOfHeaders will be set to first section r_off\n");
				
		printf(	"\t-file [filePath] -sectiontail sectionIndex [-write] - reset IMAGE_SCN_MEM_SHARED and IMAGE_SCN_MEM_DISCARDABLE, "
				"set IMAGE_SCN_MEM_READ and IMAGE_SCN_CNT_CODE, set IMAGE_SCN_MEM_WRITE if -write is specified; "
				"VirtualSize will be set to (next section v_a - current section v_a). sectionIndex is zero-based\n");
				
		printf("\t-file [filePath] -i dllName functionName - import function from dll (fix IAT)\n");
		
		return 0;
	}
	
//*************************************************************************************************
// file
//*************************************************************************************************
  if (argc >= 3 &&
      0 == strcmp("-file", argv[1]))
  {
    char* fileName = argv[2];
//    char* fileName = "c:\\develop\\test.exe";
    HANDLE hFile; 

    hFile = CreateFile(fileName,
                       GENERIC_READ | GENERIC_WRITE,
                       FILE_SHARE_WRITE,
                       NULL,
                       OPEN_EXISTING,
                       FILE_ATTRIBUTE_NORMAL,
                       NULL);

    if (INVALID_HANDLE_VALUE == hFile)
    {
      printf("Could not open file. Error = %d\n",
             GetLastError());
      return 1;
    }

    HANDLE hFileMap = CreateFileMapping(hFile,
                                        NULL,
                                        PAGE_READWRITE,
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
                                           FILE_MAP_WRITE,
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

    // remember file attributes, we'll restore them later
    GET_FILEEX_INFO_LEVELS fInfoLevelId = GetFileExInfoStandard;
    WIN32_FILE_ATTRIBUTE_DATA fileAttributeData;

    GetFileAttributesEx(fileName,
                        fInfoLevelId,
                        &fileAttributeData);

    PEParsed peParsed((DWORD)fileMapAddress);
    bool isPatched = false;

    // display file info
    if (3 == argc)
    {
      peParsed.PrintInfo();
    }

    if (argc >= 4 &&
        0 == strcmp("-header", argv[3]))
    {
      DWORD xCodeSize = -1;

      if (5 == argc)
      {
        xCodeSize = atoi(argv[4]);
      }
      isPatched = peParsed.PatchHeader(xCodeSize);
      if (!isPatched)
      {
        printf("ERROR! %s\n",
               peParsed.GetLastError());
      }
      else
      {
        printf("Success\n");
      }
    }

    if (argc >= 5 &&
        0 == strcmp("-sectiontail", argv[3]))
    {
      int sectionIndex = atoi(argv[4]);
      bool isSetWrite = false;

      if (6 == argc &&
          0 == strcmp("-write", argv[5]))
      {
        isSetWrite = true;
      }

      isPatched = peParsed.PatchSectionTail(sectionIndex, isSetWrite);
      if (!isPatched)
      {
        printf("ERROR! %s\n",
               peParsed.GetLastError());
      }
      else
      {
        printf("Success\n");
      }
    }

    if (6 == argc &&
        0 == strcmp("-i", argv[3]))
    {
      isPatched = peParsed.PatchIAT(argv[4], argv[5]);
    }

    UnmapViewOfFile(fileMapAddress);
    CloseHandle(hFileMap);

    if (isPatched)
    {
      BOOL setFileTimeResult;

      setFileTimeResult = SetFileTime(hFile,
                                      &fileAttributeData.ftCreationTime,
                                      &fileAttributeData.ftLastAccessTime,
                                      &fileAttributeData.ftLastWriteTime);
      if (!setFileTimeResult)
      {
        printf("COULD NOT RESTORE FILE ATTRIBUTES. Error = %d\n",
               GetLastError());
      }
    }

    CloseHandle(hFile);
  }

//	printf("Press ENTER to exit...");
//	getchar();

	return 0;
}
