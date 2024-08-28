using System.Text.RegularExpressions;

namespace EldenRingSaveManager
{
    internal class Program
    {
        
        static string username = Environment.UserName;
        static string backUpDirectory = @$"C:\Users\{username}\AppData\Roaming\EldenRingSaveBackUp\";

        static void Main(string[] args)
        {
            if (!Directory.Exists(backUpDirectory))
            {
                Directory.CreateDirectory(backUpDirectory);
            }
            Menu();
        }

        static void Menu()
        {
            try
            {
                Console.Clear();
                Console.WriteLine($"Backup directory: {backUpDirectory}");
                Console.WriteLine("Options");
                Console.WriteLine("1.Make a copy of your .sl2 save into your Backup directory");
                Console.WriteLine("2.Make a copy of your .co2 save into your Backup directory");
                Console.WriteLine("3.Add .co2 in your savefile directory");
                Console.WriteLine("4.Add .sl2 in your savefile directory");
                Console.WriteLine("5.Exit");
                Console.Write("Your choice: ");
#pragma warning disable CS8600
                string input = Console.ReadLine();
#pragma warning restore CS8600
                int choice = Convert.ToInt32(input);
                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        MakeCopyOfSaveFile();
                        break;
                    case 2:
                        MakeCopyOfSaveFile(true);
                        break;
                    case 3:
                        DuplicateFile();
                        break;
                    case 4:
                        DuplicateFile(true);
                        break;
                    default:
                        throw new Exception("Invalid choice.");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(3000);
                Menu();
            }
        }

        static void DuplicateFile(bool isCo2 = false)
        {
            GetSaveType(isCo2, out string firstFile, out string firstBakFile);
            GetSaveType(!isCo2, out string secondFile, out string secondBakFile);
            GetEldenRingDirectory(out string eldenRingDirectory, out bool isFound);
            string firstFilePath = @$"{eldenRingDirectory}\{firstFile}";
            string firstBakFilePath = $@"{eldenRingDirectory}\{firstBakFile}";
            string secondFilePath = $@"{eldenRingDirectory}\{secondFile}";
            string secondBakFilePath = $@"{eldenRingDirectory}\{secondBakFile}";
            if (!isFound)
            {
                Console.WriteLine("Directory is not found.");
            }
            else
            {
                if (File.Exists(firstFilePath))
                {
                    File.Copy(firstFilePath, secondFilePath, false);

                }
                else
                {
                    Console.WriteLine($"{firstFilePath} is not found");
                }
                if (File.Exists(secondFilePath))
                {
                    File.Copy(secondFilePath, secondBakFilePath, false);
                }
                else
                {
                    Console.WriteLine(@$"{eldenRingDirectory}\{firstBakFile} is not found");
                }
            }
            Console.Write("Press anything to continue....");
            Console.ReadKey();
            Menu();
        }

        static void MakeCopyOfSaveFile(bool isCo2 = false)
        {
            string path = backUpDirectory;
            string bakPath = path;
            GetSaveType(isCo2, out string file, out string bakFile);
            path += file;
            bakPath += bakFile;
            GetEldenRingDirectory(out string eldenRingDirectory, out bool isFound);
            if (!isFound)
            {
                Console.WriteLine("Directory is not found.");
            }

            else
            {
                if (File.Exists(@$"{eldenRingDirectory}\{file}"))
                {
                    File.Copy($@"{eldenRingDirectory}\{file}", path, true);
                }
                else
                {
                    Console.WriteLine(@$"{eldenRingDirectory}\{file} is not found");
                }
                if (File.Exists($@"{eldenRingDirectory}\{bakFile}"))
                {
                    File.Copy($@"{eldenRingDirectory}\{bakFile}", bakPath, true);
                }
                else
                {
                    Console.WriteLine($@"{eldenRingDirectory}\{bakFile} is not found");
                }
                Console.WriteLine("Files copied successfully!");
            }
            Console.Write("Press anything to continue....");
            Console.ReadKey();
            Menu();

        }

        private static void GetEldenRingDirectory(out string eldenRingDirectory, out bool isFound)
        {
            string[] subDirectories = Directory.GetDirectories(@$"C:\Users\Wheezy\AppData\Roaming\EldenRing");
            eldenRingDirectory = string.Empty;
            isFound = FindSaveFileLocation(subDirectories, ref eldenRingDirectory);
        }

        static void GetSaveType(bool isCo2, out string file, out string bakFile)
        {
            if (!isCo2)
            {
                file = "ER0000.sl2";
            }
            else
            {
                file = "ER0000.co2";
            }
            bakFile = file + ".bak";
        }

        static bool FindSaveFileLocation(string[] subDirectories, ref string eldenRingDirectory)
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
    }
}
