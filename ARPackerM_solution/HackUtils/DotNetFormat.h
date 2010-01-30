#pragma once

#include <Windows.h>


#define META_DATA_TABLES_STREAM_NAME "#~"
#define STRINGS_STREAM_NAME          "#Strings"
#define GUID_STREAM_NAME             "#GUID"

#define MODULE_TABLE_INDEX 0


typedef unsigned __int64 QWORD, *PQWORD;


typedef struct META_DATA_HEADER_PART1
{
	DWORD Signature; // 0x424A5342
	WORD MajorVersion; // any
	WORD MinorVersion; // any
	DWORD Reserved; // 0
	DWORD VersionLength;
} META_DATA_HEADER_PART1, *PMETA_DATA_HEADER_PART1;
// VersionString here, VersionLength chars
typedef struct META_DATA_HEADER_PART2
{
  WORD Flags; // 0
  WORD MumberOfStreams;
} META_DATA_HEADER_PART2, *PMETA_DATA_HEADER_PART2;


typedef struct STREAM_HEADER
{
  DWORD Offset; // relative to the start of the MetaData section
  DWORD Size;
  // char Name[]; // string aligned to the next 4-byte boundary, size must be calculated dynamically
} STREAM_HEADER, *PSTREAM_HEADER;


typedef struct META_DATA_TABLES_HEADER
{
  DWORD Reserved_1; // 0
  BYTE MajorVersion;
  BYTE MinorVersion;
  BYTE HeapOffsetSizes;
  BYTE Reserved_2; // 1
  QWORD MaskValid;
  QWORD MaskSorted;
} META_DATA_TABLES_HEADER, *PMETA_DATA_TABLES_HEADER;


typedef struct MODULE_TABLE_ROW
{
  WORD Generation; // 0
//  WORD/DWORD Name; // #Strings index
//  WORD/DWORD Mvid; // #GUID index
//  WORD/DWORD EncId; // 0, #GUID index
//  WORD/DWORD EncBaseId; // 0, #GUID index
} MODULE_TABLE_ROW, *PMODULE_TABLE_ROW;

typedef struct TYPE_REF_TABLE_ROW
{
//  WORD/DWORD ResolutionScope ; // index
//  WORD/DWORD TypeName; // #Strings index
//  WORD/DWORD TypeNamespace; // #Strings index
} TYPE_REF_TABLE_ROW, *PTYPE_REF_TABLE_ROW;


inline DWORD GetIndexValue(const void* indexAddr, int indexSize)
{
  return 2 == indexSize ? *(WORD*)indexAddr : *(DWORD*)indexAddr;
}
