// Copyright (C) 2001 Gerry Shaw
//
// This software is provided 'as-is', without any express or implied
// warranty.  In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not
//	claim that you wrote the original software. If you use this software
//	in a product, an acknowledgment in the product documentation would be
//	appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
//
// Gerry Shaw (gerry_shaw@yahoo.com)

/* #zlib - Wrapping and enhancing the zlib
 * Copyright (C) 2005-06, Tyron Madlener <zlib@tyron.at>
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * See COPYING for details
 */

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace UsefulHeap.Zip {

	/// <summary>Support methods for uncompressing zip files.</summary>
	/// <remarks>
	///   <para>This unzip package allow extract file from .ZIP file, compatible with PKZip 2.04g WinZip, InfoZip tools and compatible.</para>
	///   <para>Encryption and multi volume ZipFile (span) are not supported.  Old compressions used by old PKZip 1.x are not supported.</para>
	///   <para>Copyright (C) 1998 Gilles Vollant.  http://www.winimage.com/zLibDll/unzip.htm</para>
	///   <para>C# wrapper by Gerry Shaw (gerry_shaw@yahoo.com).  http://www.organicbit.com/zip/</para>
	/// </remarks>
	internal sealed class ZipLib {
		public const int MAX_PATH = 260;

		// prevent instances of this class from being constructed
		private ZipLib() {}

		/*
			Create a zipfile.
			pathname contain on Windows NT a filename like "c:\\zlib\\zlib111.zip" or on an Unix computer "zlib/zlib111.zip".
			if the file pathname exist and append=1, the zip will be created at the end of the file. (useful if the file contain a self extractor code)
			If the zipfile cannot be opened, the return value is NULL.
			Else, the return value is a zipFile Handle, usable with other function of this zip package.
		*/
		/// <summary>Create a zip file.</summary>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern IntPtr zipOpen(string fileName, int append);

		/*
			Open a file in the ZIP for writing.
			filename : the filename in zip (if NULL, '-' without quote will be used
			*zipfi contain supplemental information
			if extrafield_local!=NULL and size_extrafield_local>0, extrafield_local contains the extrafield data the the local header
			if extrafield_global!=NULL and size_extrafield_global>0, extrafield_global contains the extrafield data the the local header
			if comment != NULL, comment contain the comment string
			method contain the compression method (0 for store, Z_DEFLATED for deflate)
			level contain the level of compression (can be Z_DEFAULT_COMPRESSION)
		*/
		/// <summary>Open a new zip entry for writing.</summary>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern int zipOpenNewFileInZip(IntPtr handle,
			string entryName,
			out ZipFileEntryInfo entryInfoPtr,
			byte[] extraField,
			uint extraFieldLength,
			byte[] extraFieldGlobal,
			uint extraFieldGlobalLength,
			string comment,
			int method,
			int level);


		/// <summary>Write data to the zip file.</summary>
		[DllImport("zlib.dll")]
		public static extern int zipWriteInFileInZip(IntPtr handle, byte[] buffer, uint count);

		/// <summary>Close the current entry in the zip file.</summary>
		[DllImport("zlib.dll")]
		public static extern int zipCloseFileInZip(IntPtr handle);

		/// <summary>Close the zip file.</summary>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern int zipClose(IntPtr handle, string comment);


		/// <summary>Opens a zip file for reading.</summary>
		/// <param name="fileName">The name of the zip to open.  At this time only file names with ANSI (8 bit) characters are supported.</param>
		/// <returns>
		///   <para>A handle usable with other functions of the ZipLib class.</para>
		///   <para>Otherwise IntPtr.Zero if the zip file could not e opened (file doen not exist or is not valid).</para>
		/// </returns>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern IntPtr unzOpen(string fileName);

		/// <summary>Closes a zip file opened with unzipOpen.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <remarks>If there are files inside the zip file opened with <see cref="unzOpenCurrentFile"/> these files must be closed with <see cref="unzCloseCurrentFile"/> before call <c>unzClose</c>.</remarks>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>Otherwise a value less than zero.  See <see cref="ErrorCode"/> for the specific reason.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzClose(IntPtr handle);

		/// <summary>Get global information about the zip file.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <param name="globalInfoPtr">An address of a <see cref="ZipFileInfo"/> struct to hold the information.  No preparation of the structure is needed.</param>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>Otherwise a value less than zero.  See <see cref="ErrorCode"/> for the specific reason.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzGetGlobalInfo(IntPtr handle, out ZipFileInfo globalInfoPtr);

		/// <summary>Get the comment associated with the entire zip file.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/></param>
		/// <param name="commentBuffer">The buffer to hold the comment.</param>
		/// <param name="commentBufferLength">The length of the buffer in bytes (8 bit characters).</param>
		/// <returns>
		///   <para>The number of characters in the comment if there was no error.</para>
		///   <para>Otherwise a value less than zero.  See <see cref="ErrorCode"/> for the specific reason.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzGetGlobalComment(IntPtr handle, sbyte[] commentBuffer, uint commentBufferLength);

		/// <summary>Set the current file of the zip file to the first file.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>Otherwise a value less than zero.  See <see cref="ErrorCode"/> for the specific reason.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzGoToFirstFile(IntPtr handle);

		/// <summary>Set the current file of the zip file to the next file.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>Otherwise <see cref="ErrorCode.EndOfListOfFile"/> if there are no more entries.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzGoToNextFile(IntPtr handle);

		/// <summary>Try locate the entry in the zip file.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <param name="entryName">The name of the entry to look for.</param>
		/// <param name="caseSensitivity">If 0 use the OS default.  If 1 use case sensitivity like strcmp, Unix style.  If 2 do not use case sensitivity like strcmpi, Windows style.</param>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>Otherwise <see cref="ErrorCode.EndOfListOfFile"/> if there are no more entries.</para>
		/// </returns>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern int unzLocateFile(IntPtr handle, string entryName, int caseSensitivity);

		/// <summary>Get information about the current entry in the zip file.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <param name="entryInfoPtr">A ZipEntryInfo struct to hold information about the entry or null.</param>
		/// <param name="entryNameBuffer">An array of sbyte characters to hold the entry name or null.</param>
		/// <param name="entryNameBufferLength">The length of the entryNameBuffer in bytes.</param>
		/// <param name="extraField">An array to hold the extra field data for the entry or null.</param>
		/// <param name="extraFieldLength">The length of the extraField array in bytes.</param>
		/// <param name="commentBuffer">An array of sbyte characters to hold the entry name or null.</param>
		/// <param name="commentBufferLength">The length of theh commentBuffer in bytes.</param>
		/// <remarks>
		///   <para>If entryInfoPtr is not null the structure will contain information about the current file.</para>
		///   <para>If entryNameBuffer is not null the name of the entry will be copied into it.</para>
		///   <para>If extraField is not null the extra field data of the entry will be copied into it.</para>
		///   <para>If commentBuffer is not null the comment of the entry will be copied into it.</para>
		/// </remarks>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>Otherwise a value less than zero.  See <see cref="ErrorCode"/> for the specific reason.</para>
		/// </returns>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern int unzGetCurrentFileInfo(IntPtr handle, out ZipEntryInfo entryInfoPtr,
			sbyte[] entryNameBuffer, uint entryNameBufferLength,
			byte[]  extraField,	  uint extraFieldLength,
			sbyte[] commentBuffer,   uint commentBufferLength);

		/// <summary>Open the zip file entry for reading.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>Otherwise a value from <see cref="ErrorCode"/>.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzOpenCurrentFile(IntPtr handle);

		/// <summary>Close the file entry opened by <see cref="unzOpenCurrentFile"/>.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <returns>
		///   <para>Zero if there was no error.</para>
		///   <para>CrcError if the file was read but the Crc does not match.</para>
		///   <para>Otherwise a value from <see cref="ErrorCode"/>.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzCloseCurrentFile(IntPtr handle);

		/// <summary>Read bytes from the current zip file entry.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <param name="buffer">Buffer to store the uncompressed data into.</param>
		/// <param name="count">Number of bytes to write from <paramref name="buffer"/>.</param>
		/// <returns>
		///   <para>The number of byte copied if somes bytes are copied.</para>
		///   <para>Zero if the end of file was reached.</para>
		///   <para>Less than zero with error code if there is an error.  See <see cref="ErrorCode"/> for a list of possible error codes.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzReadCurrentFile(IntPtr handle, byte[] buffer, uint count);

		/// <summary>Give the current position in uncompressed data of the zip file entry currently opened.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <returns>The number of bytes into the uncompressed data read so far.</returns>
		[DllImport("zlib.dll")]
		public static extern long unztell(IntPtr handle);

		/// <summary>Determine if the end of the zip file entry has been reached.</summary>
		/// <param name="handle">The zip file handle opened by <see cref="unzOpenCurrentFile"/>.</param>
		/// <returns>
		///   <para>One if the end of file was reached.</para>
		///   <para>Zero if elsewhere.</para>
		/// </returns>
		[DllImport("zlib.dll")]
		public static extern int unzeof(IntPtr handle);

		/// <summary>
		/// Same as unzOpenCurrentFile but with the possibility to read raw data
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="method"></param>
		/// <param name="level"></param>
		/// <param name="raw"></param>
		/// <returns></returns>
		[DllImport("zlib.dll")]
		public static extern int unzOpenCurrentFile2(IntPtr handle,out int method, out int level, int raw);

		/// <summary>
		/// Same as zipOpenNewFileInZip but with the possibility to write raw data
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="entryName"></param>
		/// <param name="entryInfoPtr"></param>
		/// <param name="extraField"></param>
		/// <param name="extraFieldLength"></param>
		/// <param name="extraFieldGlobal"></param>
		/// <param name="extraFieldGlobalLength"></param>
		/// <param name="comment"></param>
		/// <param name="method"></param>
		/// <param name="level"></param>
		/// <param name="raw"></param>
		/// <returns></returns>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern int zipOpenNewFileInZip2(IntPtr handle,
			string entryName,
			out ZipFileEntryInfo entryInfoPtr,
			byte[] extraField,
			uint extraFieldLength,
			byte[] extraFieldGlobal,
			uint extraFieldGlobalLength,
			string comment,
			int method,
			int level,
			int raw);

		/// <summary>
		/// Same as zipCloseFile. Needs to be called instead when using zipOpenNewFileInZip2
		/// </summary>
		/// <param name="Handle">Handle to the opened zip file</param>
		/// <param name="uncompressed_size"></param>
		/// <param name="crc32"></param>
		/// <returns></returns>
		[DllImport("zlib.dll", ExactSpelling=true, CharSet=CharSet.Ansi)]
		public static extern int zipCloseFileInZipRaw(IntPtr Handle, UInt32 uncompressed_size, UInt32 crc32);


		/// <summary>Converts a CLR string to a 8 bit ANSI array of characters.</summary>
		/// <param name="str">The string to convert.</param>
		/// <returns>A 8 bit ANSI array of characters.</returns>
		public static sbyte[] StringToAnsi(string str) {
			int length = str.Length;
			sbyte[] chars = new sbyte[length + 1];
			for (int i = 0; i < length; i++) {
				chars[i] = (sbyte) str[i];
			}

			return chars;
		}

		/// <summary>Converst an 8 bit ANSI C style string to a CLR string.</summary>
		/// <param name="chars">The array of a characters that holds the string.</param>
		/// <returns>The CLR string representing the characters passed in.</returns>
		public static string AnsiToString(sbyte[] chars) {
			int length = chars.Length;
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < length; i++) {
				builder.Append((char) chars[i]);
			}
			return builder.ToString();
		}
	}

	/// <summary>List of possible error codes.</summary>
	internal enum ErrorCode : int {
		/// <summary>No error.</summary>
		Ok = 0,

		/// <summary>Unknown error.</summary>
		Error = -1,

		/// <summary>Last entry in directory reached.</summary>
		EndOfListOfFile = -100,

		/// <summary>Parameter error.</summary>
		ParameterError = -102,

		/// <summary>Zip file is invalid.</summary>
		BadZipFile = -103,

		/// <summary>Internal program error.</summary>
		InternalError = -104,

		/// <summary>Crc values do not match.</summary>
		CrcError = -105
	}

	/// <summary>Global information about the zip file.</summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct ZipFileInfo {
		/// <summary>The number of entries in the directory.</summary>
		public UInt32 EntryCount;

		/// <summary>Length of zip file comment in bytes (8 bit characters).</summary>
		public UInt32 CommentLength;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct ZipFileEntryInfo {
		public ZipDateTimeInfo DateTime;
		public UInt32 DosDate;
		public UInt32 InternalFileAttributes; // 2 bytes
		public UInt32 ExternalFileAttributes; // 4 bytes
	}

	/// <summary>Custom ZipLib date time structure.</summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct ZipDateTimeInfo {
		/// <summary>Seconds after the minute - [0,59]</summary>
		public UInt32 Seconds;

		/// <summary>Minutes after the hour - [0,59]</summary>
		public UInt32 Minutes;

		/// <summary>Hours since midnight - [0,23]</summary>
		public UInt32 Hours;

		/// <summary>Day of the month - [1,31]</summary>
		public UInt32 Day;

		/// <summary>Months since January - [0,11]</summary>
		public UInt32 Month;

		/// <summary>Years - [1980..2044]</summary>
		public UInt32 Year;

		// implicit conversion from DateTime to ZipDateTimeInfo
		public static implicit operator ZipDateTimeInfo(DateTime date)  {
			ZipDateTimeInfo d;
			d.Seconds = (uint) date.Second;
			d.Minutes = (uint) date.Minute;
			d.Hours = (uint) date.Hour;
			d.Day = (uint) date.Day;
			d.Month = (uint) date.Month - 1;
			d.Year = (uint) date.Year;
			return d;
		}
	}

	/// <summary>Information stored in zip file directory about an entry.</summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct ZipEntryInfo {
		// <summary>Version made by (2 bytes).</summary>
		public UInt32 Version;				 
		
		/// <summary>Version needed to extract (2 bytes).</summary>
		public UInt32 VersionNeeded;		   
		
		/// <summary>General purpose bit flag (2 bytes).</summary>
		public UInt32 Flag;					
		
		/// <summary>Compression method (2 bytes).</summary>
		public UInt32 CompressionMethod;	   
		
		/// <summary>Last mod file date in Dos fmt (4 bytes).</summary>
		public UInt32 DosDate;				 
		
		/// <summary>Crc-32 (4 bytes).</summary>
		public UInt32 Crc;					 
		
		/// <summary>Compressed size (4 bytes).</summary>
		public UInt32 CompressedSize;		  
		
		/// <summary>Uncompressed size (4 bytes).</summary>
		public UInt32 UncompressedSize;		
		
		/// <summary>Filename length (2 bytes).</summary>
		public UInt32 FileNameLength;		  
		
		/// <summary>Extra field length (2 bytes).</summary>
		public UInt32 ExtraFieldLength;		
		
		/// <summary>File comment length (2 bytes).</summary>
		public UInt32 CommentLength;		   

		/// <summary>Disk number start (2 bytes).</summary>
		public UInt32 DiskStartNumber;		 
		
		/// <summary>Internal file attributes (2 bytes).</summary>
		public UInt32 InternalFileAttributes;  
		
		/// <summary>External file attributes (4 bytes).</summary>
		public UInt32 ExternalFileAttributes;  

		/// <summary>File modification date of entry.</summary>
		public ZipDateTimeInfo DateTime;
	}

	public enum AppendStatus {
		Create = 0,
		CreateAfter = 1,
		AddInZip = 2
	}
}
