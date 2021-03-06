﻿using System;
using System.IO;
using System.Diagnostics;


namespace AutoUpdataCaD
{
	class Program
	{
		static void Main(string[] args)
		{
			string folderRepo = @"C:\\Users\\Slawek\\Repo\\";
			string fileRepo = "Deante.exe";
			string pathCadDecor = @"C:\CADDecor\Cad 2.2\";
			string[] listFile = ReadListFilesFromRepository(folderRepo);
			
			SevenZipExtract(pathCadDecor + fileRepo, folderRepo);

			foreach (var item in listFile)
			{
				int numberFiles = 0;
				if (checkFile(item))
				{
					numberFiles = SevenZipTest(item);

					bool success = false;
					if (numberFiles == 1)
						success = SevenZipExtract(item, folderRepo);
					else
						success = SevenZipExtract(item, pathCadDecor);

					if (success)
						File.Delete(item);

				}
			}

			listFile = ReadListFilesFromRepository(folderRepo);
			foreach (var item in listFile)
			{
				bool success = false;

				success = SevenZipExtract(item, pathCadDecor);
				if (success)
					File.Delete(item);
			}
			Console.ReadKey();
		}


		/// <summary>
		/// Wypakowuje wskazany plik do katalogo folderExtract
		/// </summary>
		/// <param name="folder">Folder w którym znajduje się archiwum</param>
		/// <param name="pathFile">pełna ścieżka do spakowanego pliku</param>
		static bool SevenZipExtract(string pathFile, string folderExtract)
		{
			int EverythingOK = -1;
			string testInfo = string.Empty;

			Process p = new Process();
			p.StartInfo.FileName = "7z.exe";
			p.StartInfo.Arguments = "e " + pathFile + " -o\"" + folderExtract + "\"" + " -y";
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.Start();

			testInfo = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			p.Close();

			EverythingOK = testInfo.IndexOf("Everything is Ok");

			return EverythingOK == -1 ? false : true;
		}

		/// <summary>
		/// Sprawdza archiwum i podaje liczbę spakowanych plików
		/// </summary>
		/// <param name="pathFile">Scieżka do archiwum</param>
		/// <returns>Zwraca liczbę spakowanych plików</returns>
		static int SevenZipTest(string pathFile)
		{
			Process p = new Process();
			p.StartInfo.FileName = "7z.exe";
			p.StartInfo.Arguments = "t " + pathFile;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.Start();
			string testInfo = p.StandardOutput.ReadToEnd();
			int filesNumber = AnalizeTestOutput(testInfo);
			p.Close();
			return filesNumber;
		}


		/// <summary>
		/// Analiza treści wyjścia konsoli po teście archiwum .zip próba wyciągnięcia liczby plików w archiwum
		/// </summary>
		/// <param name="stringTest">treśc do analizy</param>
		/// <returns></returns>
		static int AnalizeTestOutput(string stringTest)
		{
			//string sentence = "This sentence has five words.";
			// Extract the second word.

			Console.WriteLine(stringTest);
			int EverythingOK = stringTest.IndexOf("Everything is Ok");
			int n = 0;

			Console.WriteLine("EverythingOK = " + EverythingOK);

			if (EverythingOK > 0)
			{
				if (stringTest.IndexOf("Files:") == -1)
				{
					return 1;
				}
				else
				{
					int startPosition = stringTest.IndexOf("Files:") + 7;
					Console.WriteLine("startPosition = " + startPosition);
					string word2 = stringTest.Substring(startPosition, stringTest.IndexOf(" ", startPosition) - startPosition - 7);

					int.TryParse(word2, out n);
					//Console.WriteLine("Second word: " + word2);
				}
				// Console.WriteLine(n);
				//Console.ReadKey();
			}

			return n; // int.Parse(word2);
		}

		/// <summary>
		/// Odczytuje dane z pliku konfiguracyjnego utworzonego podczas instalacji
		/// </summary>
		/// <param name="fileName"></param>
		static void ReadConfigFile(string fileName)
		{
			string pathCADDecor = "";
			string patchFolderRepository = "";

		}

		/// <summary>
		/// Odczytuje listę wszystkich plików znajdujących się w repozytorium
		/// </summary>
		/// <param name="folderName">Ścieżka do folderu w którym znajdują się pliki do rozpakowania</param>
		/// <returns>Zwraca LISTE pełnych ścierzek do wszystkich plików w folderze</returns>
		static string[] ReadListFilesFromRepository(string folderName)
		{

			string[] fileEntries = null;
			if (Directory.Exists(folderName))
			{
				// Process the list of files found in the directory.
				fileEntries = Directory.GetFiles(folderName);

				// Recurse into subdirectories of this directory.
				string[] subdirectoryEntries = Directory.GetDirectories(folderName);
			}
			else
			{
				Console.WriteLine("{0} to niej prawidłowa sciezka do katalogu.", folderName);
			}

			return fileEntries;
		}

		/// <summary>
		/// Główna metoda programu - rozpakowuje plik i przerzuca do prawidłowego katalogu
		/// </summary>
		/// <param name="fileName"></param>
		static void Unpack(string folderRepo, string fileRepo)
		{
			//string zipPath = folderRepo + fileRepo;

			string zipPath = Path.GetFullPath(folderRepo) + fileRepo;
			if (File.Exists(zipPath))
			{
				string extractPath = @"C:\CADDecor\Cad 2.2";
			    //SevenZipExtractor. ExtractToDirectory(folderRepo + fileRepo, extractPath);
				//ZipFile
				//File.Delete(folderRepo + fileRepo);
				Console.WriteLine("Jest OK: ", zipPath);
			}

		}

		/// <summary>
		/// Określa rodzaj pliku - jego rozezrzenie, zip, msi, exe
		/// </summary>
		/// <param name="fileName"></param>
		static string Extention(string fileName)
		{

			return null;
		}

		/// <summary>
		/// Określenie czy plik jest .exe czy .zip
		/// </summary>
		/// <param name="fileName">Ścieżka do pliku</param>
		/// <returns>Zwraca True jeśli rozrzerzenie będzie .zip</returns>
		static bool checkFile(string fileName)
		{
			//string ext = Extention(fileName);
			string extension = "";
			extension = Path.GetExtension(fileName);
			//Console.WriteLine("GetExtension('{0}') returns '{1}'", fileName, extension);

			if (extension == ".zip")
			{
				// if( checkArchiw(fileNameZip) )
				// UnPackFirstZip(string fileNameZip)
				return true;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileNameZip"></param>
		/// <returns></returns>
		static string UnPackFirstZip(string fileNameZip)
		{
			// uzip c:\patchFolderRepository\
			// fileName = newFileName.exe
			// remove fileNameZip
			return String.Empty;
		}

		/// <summary>
		/// Sprawdzam czy w archiwum jest tylko jeden plik typu .exe
		/// </summary>
		/// <param name="fileNameZip"></param>
		static bool checkArchiw(string fileNameZip)
		{
			return false;
		}
	}
}

