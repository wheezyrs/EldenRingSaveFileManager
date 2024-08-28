using System.Text.RegularExpressions;

namespace EldenRingSaveManager
{
    internal class Program
    {

        static readonly string currentUserName = Environment.UserName;
        static readonly string backupDirectoryPath = @$"C:\Users\{currentUserName}\AppData\Roaming\EldenRingSaveBackUp";

        static void Main()
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
                        RestoreSaveFromBackup();
                        break;
                    case 6:
                        RestoreSaveFromBackup(true);
                        break;
                    case 7:
                        AdditionalInformation();
                        break;
                    case 8:
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        throw new Exception("Invalid choice.");

                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Thread.Sleep(1500);
                Console.Write("Press anything to continue...");
                Console.ReadKey();
                ShowMenu();
            }
        }

        private static void AdditionalInformation()
        {
            Console.Clear();
            GetSaveFileNames(false, out string sl2FileName, out string sl2BakFileName);
            GetSaveFileNames(true, out string co2FileName, out string co2BakFileName);
            FindEldenRingSaveDirectory(out string eldenRingSaveDirectory, out bool directoryExists);
            if (!directoryExists)
            {
                throw new Exception("Directory does not exist");
            }
            string sl2FilePath = @$"{eldenRingSaveDirectory}\{sl2FileName}";
            string co2FilePath = @$"{eldenRingSaveDirectory}\{co2FileName}";
            string sl2BakFilePath = @$"{eldenRingSaveDirectory}\{sl2BakFileName}";
            string co2BakFilePath = @$"{eldenRingSaveDirectory}\{co2BakFileName}";

            DateTime sl2FileLastAccess = File.GetLastAccessTime(sl2FilePath);
            DateTime co2FileLastAccess = File.GetLastAccessTime(co2FilePath);
            DateTime sl2BakFileLastAccess = File.GetLastAccessTime(sl2BakFilePath);
            DateTime co2BakFileLastAccess = File.GetLastAccessTime(co2BakFilePath);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Last access of {sl2FileName} was on {sl2FileLastAccess}");
            Console.WriteLine($"Last access of {sl2BakFileName} was on {sl2BakFileLastAccess}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Last access of {co2FileName} was on {co2FileLastAccess}");
            Console.WriteLine($"Last access of {co2BakFileName} was on {co2BakFileLastAccess}");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Press anything to continue....");
            Console.ReadKey();
            Console.ResetColor();
            ShowMenu();
        }

        static void DisplayMenuOptions()
        {
            Console.WriteLine(ColoredText("Elden Ring Save Manager by wheezyrs", ConsoleColor.Green));

            string text = $"Backup directory: {backupDirectoryPath}\n";
            Console.WriteLine(ColoredText(text, ConsoleColor.Cyan));

            Console.Write(ColoredText(new string('-', text.Length), ConsoleColor.Yellow));

            Console.WriteLine(ColoredText("\n\nPlease choose an option:\n", ConsoleColor.White));

            //#1
            Console.WriteLine(
                ColoredText("1. ", ConsoleColor.Green) +
                ColoredText("Backup your ", ConsoleColor.Yellow) +
                ColoredText(".sl2", ConsoleColor.DarkYellow) +
                ColoredText(" save files to the Backup", ConsoleColor.Yellow) +
                ColoredText(" Directory", ConsoleColor.DarkYellow)
            );


            ///////////////////////////////////////////////////////////////////////
            //#2
            Console.WriteLine(
                ColoredText("2. ", ConsoleColor.Green) +
                ColoredText("Backup your ", ConsoleColor.Yellow) +
                ColoredText(".co2", ConsoleColor.DarkYellow) +
                ColoredText(" save files to the Backup", ConsoleColor.Yellow) +
                ColoredText(" Directory", ConsoleColor.DarkYellow)
            );
            Console.WriteLine();



            ///////////////////////////////////////////////////////////////////////
            //#3
            Console.WriteLine(
            ColoredText("3. ", ConsoleColor.Green) +
                ColoredText("Duplicate ", ConsoleColor.Cyan) +
                ColoredText(".sl2", ConsoleColor.DarkYellow) +
                ColoredText(" save files and rename the copy to ", ConsoleColor.Cyan) +
                ColoredText(".co2", ConsoleColor.DarkYellow) +
                ColoredText(" in the game directory", ConsoleColor.Cyan)

            ///////////////////////////////////////////////////////////////////////

            //#4
            );
            Console.WriteLine(
                ColoredText("4. ", ConsoleColor.Green) +
                ColoredText("Duplicate ", ConsoleColor.Cyan) +
                ColoredText(".co2", ConsoleColor.DarkYellow) +
                ColoredText(" save files and rename the copy to ", ConsoleColor.Cyan) +
                ColoredText(".sl2", ConsoleColor.DarkYellow) +
                ColoredText(" in the game directory", ConsoleColor.Cyan)
            );
            Console.WriteLine();

            Console.WriteLine(
            ColoredText("5. ", ConsoleColor.Green) +
            ColoredText("Restore ", ConsoleColor.Blue) +
            ColoredText(".sl2", ConsoleColor.DarkYellow) +
            ColoredText(" save files from the ", ConsoleColor.Blue) +
            ColoredText("Backup Directory", ConsoleColor.DarkYellow) +
            ColoredText(" to the game directory", ConsoleColor.Blue)
            );

            Console.WriteLine(
            ColoredText("6. ", ConsoleColor.Green) +
            ColoredText("Restore ", ConsoleColor.Blue) +
            ColoredText(".co2", ConsoleColor.DarkYellow) +
            ColoredText(" save files from the ", ConsoleColor.Blue) +
            ColoredText("Backup Directory", ConsoleColor.DarkYellow) +
            ColoredText(" to the game directory", ConsoleColor.Blue)
            );


            Console.WriteLine();
            Console.WriteLine(
            ColoredText("7. ", ConsoleColor.DarkGray) +
            ColoredText("Additional Information", ConsoleColor.Gray)
            );



            ///////////////////////////////////////////////////////////////////////
            //#5
            Console.WriteLine(ColoredText("8. ", ConsoleColor.DarkRed) + ColoredText("Exit", ConsoleColor.Red));

            Console.WriteLine();
            Console.Write(ColoredText("Your choice: ", ConsoleColor.Magenta));
            Console.ResetColor();
        }

        static void RestoreSaveFromBackup(bool isCo2 = false)
        {
            GetSaveFileNames(isCo2, out string fileName, out string bakFileName);
            FindEldenRingSaveDirectory(out string eldenRingSaveDirectory, out bool directoryExists);
            if (!directoryExists)
            {
                throw new Exception("Directory does not exist");
            }
            string backupFilePath = @$"{backupDirectoryPath}\{fileName}";
            string backupBakFilePath = @$"{backupDirectoryPath}\{bakFileName}";
            string targetFilePath = @$"{eldenRingSaveDirectory}\{fileName}";
            string targetBakFilePath = $@"{eldenRingSaveDirectory}\{bakFileName}";

            File.Copy(backupFilePath, targetFilePath, true);
            Console.WriteLine($"{backupFilePath} restored successfully");
            File.Copy(backupBakFilePath, targetBakFilePath, true);
            Console.WriteLine($"{backupBakFilePath} restored successfully");
            Console.Write("Press anything to continue....");
            Console.ReadKey();
            ShowMenu();
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
                throw new Exception("Directory not found");
            }
            File.Copy(sourceFilePath, targetFilePath, true);
            Console.WriteLine($"{sourceFilePath} duplicated successfully");
            File.Copy(sourceBakFilePath, targetBakFilePath, true);
            Console.WriteLine($"{sourceBakFilePath} duplicated successfully");



            Console.Write("Press anything to continue....");
            Console.ReadKey();
            ShowMenu();
        }

        static void BackupSaveFile(bool isCo2 = false)
        {
            string filePath = backupDirectoryPath + @"\";
            string bakFilePath = filePath;
            GetSaveFileNames(isCo2, out string fileName, out string bakFileName);
            filePath += fileName;
            bakFilePath += bakFileName;
            FindEldenRingSaveDirectory(out string eldenRingSaveDirectory, out bool directoryExists);
            if (!directoryExists)
            {
                throw new Exception("Directory not found");
            }
            File.Copy($@"{eldenRingSaveDirectory}\{fileName}", filePath, true);
            Console.WriteLine($@"{eldenRingSaveDirectory}\{fileName} copied successfully");
            File.Copy($@"{eldenRingSaveDirectory}\{bakFileName}", bakFilePath, true);
            Console.WriteLine($@"{eldenRingSaveDirectory}\{bakFileName} copied successfully");

            Console.Write("Press anything to continue....");
            Console.ReadKey();
            ShowMenu();

        }

        private static void FindEldenRingSaveDirectory(out string eldenRingSaveDirectory, out bool directoryExists)
        {
            string[] subDirectories = Directory.GetDirectories(@$"C:\Users\Wheezy\AppData\Roaming\EldenRing");
            eldenRingSaveDirectory = string.Empty;
            directoryExists = LocateSaveFileDirectory(subDirectories, ref eldenRingSaveDirectory);
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
