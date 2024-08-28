﻿using System.Text.RegularExpressions;

namespace EldenRingSaveManager
{
    internal class Program
    {

        static string currentUserName = Environment.UserName;
        static string backupDirectoryPath = @$"C:\Users\{currentUserName}\AppData\Roaming\EldenRingSaveBackUp\";

        static void Main(string[] args)
        {
            if (!Directory.Exists(backupDirectoryPath))
            {
                Directory.CreateDirectory(backupDirectoryPath);
            }
            ShowMenu();
        }

        static void ShowMenu()
        {
            try
            {
                Console.Clear();
                DisplayMenuOptions();
#pragma warning disable CS8600
                string userInput = Console.ReadLine();
#pragma warning restore CS8600
                int selectedOption = Convert.ToInt32(userInput);
                switch (selectedOption)
                {
                    case 1:
                        BackupSaveFile();
                        break;
                    case 2:
                        BackupSaveFile(true);
                        break;
                    case 3:
                        DuplicateSaveFile();
                        break;
                    case 4:
                        DuplicateSaveFile(true);
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                    default:
                        throw new Exception("Invalid choice.");

                }
            }
            catch (Exception ex)
            {
                Console.Clear() ;
                Console.WriteLine(ex.Message);
                Thread.Sleep(1500);
                Console.Write("Press anything to continue...");
                Console.ReadKey();
                ShowMenu();
            }
        }

        private static void DisplayMenuOptions()
        {
            Console.WriteLine(ColoredText("Elden Ring Save Manager by wheezyrs", ConsoleColor.Green));

            string text = $"Backup directory: {backupDirectoryPath.Substring(0, backupDirectoryPath.Length - 1)}\n";
            Console.WriteLine(ColoredText(text, ConsoleColor.Cyan));

            Console.Write(ColoredText(new string('-', text.Length), ConsoleColor.Yellow));

            Console.WriteLine(ColoredText("\n\nPlease choose an option:\n", ConsoleColor.White));

            //#1
            Console.WriteLine(
                ColoredText("1. ", ConsoleColor.Green) +
                ColoredText("Backup your ", ConsoleColor.Yellow) +
                ColoredText(".sl2", ConsoleColor.DarkYellow) +
                ColoredText(" save file to the Backup", ConsoleColor.Yellow) +
                ColoredText(" Directory", ConsoleColor.DarkYellow)
            );


            ///////////////////////////////////////////////////////////////////////
            //#2
            Console.WriteLine(
                ColoredText("2. ", ConsoleColor.Green) +
                ColoredText("Backup your ", ConsoleColor.Yellow) +
                ColoredText(".co2", ConsoleColor.DarkYellow) +
                ColoredText(" save file to the Backup", ConsoleColor.Yellow) +
                ColoredText(" Directory", ConsoleColor.DarkYellow)
            );
            Console.WriteLine();



            ///////////////////////////////////////////////////////////////////////
            //#3
            Console.WriteLine(
            ColoredText("3. ", ConsoleColor.Green) +
                ColoredText("Duplicate ", ConsoleColor.Cyan) +
                ColoredText(".sl2", ConsoleColor.DarkYellow) +
                ColoredText(" save file and rename the copy to ", ConsoleColor.Cyan) +
                ColoredText(".co2", ConsoleColor.DarkYellow) +
                ColoredText(" in the game directory", ConsoleColor.Cyan)

            ///////////////////////////////////////////////////////////////////////

            //#4
            );
            Console.WriteLine(
                ColoredText("4. ", ConsoleColor.Green) +
                ColoredText("Duplicate ", ConsoleColor.Cyan) +
                ColoredText(".co2", ConsoleColor.DarkYellow) +
                ColoredText(" save file and rename the copy to ", ConsoleColor.Cyan) +
                ColoredText(".sl2", ConsoleColor.DarkYellow) +
                ColoredText(" in the game directory", ConsoleColor.Cyan)
            );
            Console.WriteLine();



            ///////////////////////////////////////////////////////////////////////
            //#5
            Console.WriteLine(ColoredText("5. ", ConsoleColor.DarkRed) + ColoredText("Exit", ConsoleColor.Red));

            Console.WriteLine();
            Console.Write(ColoredText("Your choice: ", ConsoleColor.Magenta));
            Console.ResetColor();
        }


        static void DuplicateSaveFile(bool isCo2 = false)
        {
            GetSaveFileNames(isCo2, out string sourceFileName, out string sourceBakFileName);
            GetSaveFileNames(!isCo2, out string targetFileName, out string targetBakFileName);
            FindEldenRingSaveDirectory(out string eldenRingSaveDirectory, out bool directoryExists);
            string sourceFilePath = @$"{eldenRingSaveDirectory}\{sourceFileName}";
            string sourceBakFilePath = $@"{eldenRingSaveDirectory}\{sourceBakFileName}";
            string targetFilePath = $@"{eldenRingSaveDirectory}\{targetFileName}";
            string targetBakFilePath = $@"{eldenRingSaveDirectory}\{targetBakFileName}";
            if (!directoryExists)
            {
                Console.WriteLine("Directory is not found.");
            }
            else
            {
                if (File.Exists(sourceFilePath))
                {
                    File.Copy(sourceFilePath, targetFilePath, false);

                }
                else
                {
                    Console.WriteLine($"{sourceFilePath} is not found");
                }
                if (File.Exists(targetFilePath))
                {
                    File.Copy(targetFilePath, targetBakFilePath, false);
                }
                else
                {
                    Console.WriteLine(@$"{eldenRingSaveDirectory}\{sourceBakFileName} is not found");
                }
            }
            Console.Write("Press anything to continue....");
            Console.ReadKey();
            ShowMenu();
        }

        static void BackupSaveFile(bool isCo2 = false)
        {
            string filePath = backupDirectoryPath;
            string bakFilePath = filePath;
            GetSaveFileNames(isCo2, out string fileName, out string bakFileName);
            filePath += fileName;
            bakFilePath += bakFileName;
            FindEldenRingSaveDirectory(out string eldenRingSaveDirectory, out bool directoryFound);
            if (!directoryFound)
            {
                Console.WriteLine("Directory is not found.");
            }

            else
            {
                if (File.Exists(@$"{eldenRingSaveDirectory}\{fileName}"))
                {
                    File.Copy($@"{eldenRingSaveDirectory}\{fileName}", filePath, true);
                }
                else
                {
                    Console.WriteLine(@$"{eldenRingSaveDirectory}\{fileName} is not found");
                }
                if (File.Exists($@"{eldenRingSaveDirectory}\{bakFileName}"))
                {
                    File.Copy($@"{eldenRingSaveDirectory}\{bakFileName}", bakFilePath, true);
                }
                else
                {
                    Console.WriteLine($@"{eldenRingSaveDirectory}\{bakFileName} is not found");
                }
                Console.WriteLine("Files copied successfully!");
            }
            Console.Write("Press anything to continue....");
            Console.ReadKey();
            ShowMenu();

        }

        private static void FindEldenRingSaveDirectory(out string eldenRingSaveDirectory, out bool directoryFound)
        {
            string[] subDirectories = Directory.GetDirectories(@$"C:\Users\Wheezy\AppData\Roaming\EldenRing");
            eldenRingSaveDirectory = string.Empty;
            directoryFound = LocateSaveFileDirectory(subDirectories, ref eldenRingSaveDirectory);
        }

        static void GetSaveFileNames(bool isCo2, out string fileName, out string bakFileName)
        {
            if (!isCo2)
            {
                fileName = "ER0000.sl2";
            }
            else
            {
                fileName = "ER0000.co2";
            }
            bakFileName = fileName + ".bak";
        }

        static bool LocateSaveFileDirectory(string[] subDirectories, ref string eldenRingDirectory)
        {
            foreach (string directory in subDirectories)
            {
                string folderName = Path.GetFileName(directory);
                if (Regex.IsMatch(folderName, @"\d{17}"))
                {

                    eldenRingDirectory = directory;
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Writes the specified text to the console in the specified foreground color and resets the color afterward.
        /// </summary>
        /// <param name="text">The text to be written to the console.</param>
        /// <param name="color">The foreground color to use for the text.</param>
        /// <returns>
        /// An empty string, as the text is directly written to the console, and the return value is only for convenience in formatting.
        /// </returns>
        /// <example>
        /// <code>
        /// Console.WriteLine(ColoredText("This is green text", ConsoleColor.Green));
        /// </code>
        /// This will output "This is green text" in green color in the console.
        /// </example>
        static string ColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
            return "";
        }

    }
}
