using Newtonsoft.Json.Linq;
using GameSaveEditor;
public class Program
{
    public static void Main()
    {
        EncryptionManager encryptionManager = new EncryptionManager();
        ModificationManager modificationManager = new ModificationManager();

        while (true)
        {
            Console.WriteLine("Do you want to modify the file, see its content, create a backup, restore from backup or exit the program?");
            Console.WriteLine("(Enter 'modify' to modify, 'view' to see content, 'backup' to create a backup, 'restore' to restore from backup, 'exit' to exit the program):");
            string userChoice = Console.ReadLine().ToLower();

            if (userChoice == "modify")
            {
                string filePath = GetFilePath();

                if (filePath.EndsWith(".sav"))
                {
                    string decryptedContent = encryptionManager.Decrypt(File.ReadAllText(filePath));
                    JObject json = JObject.Parse(decryptedContent);
                    ModifySaveFile(json, filePath, encryptionManager, modificationManager);
                }
                else
                {
                    Console.WriteLine("Unsupported file type. Please try again.");
                }
            }
            else if (userChoice == "view")
            {
                string filePath = GetFilePath();

                if (filePath.EndsWith(".sav"))
                {
                    string decryptedContent = encryptionManager.Decrypt(File.ReadAllText(filePath));
                    Console.WriteLine("Decrypted file contents:");
                    Console.WriteLine(decryptedContent);
                    Console.WriteLine("Press any key to go back.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Unsupported file type. Please try again.");
                }
            }
            else if (userChoice == "backup")
            {
                string filePath = GetFilePath();

                if (filePath.EndsWith(".sav"))
                {
                    string directoryPath = GetDirectoryPath();
                    CreateBackup(filePath, directoryPath);
                }
                else
                {
                    Console.WriteLine("Unsupported file type. Please try again.");
                }
            }
            else if (userChoice == "restore")
            {
                string filePath = GetFilePath();

                if (filePath.EndsWith(".bak"))
                {
                    string directoryPath = GetDirectoryPath();
                    RestoreBackup(filePath, directoryPath);
                }
                else
                {
                    Console.WriteLine("Unsupported file type. Please try again.");
                }
            }
            else if (userChoice == "exit")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }

    public static void ModifySaveFile(JObject json, string filePath, EncryptionManager encryptionManager, ModificationManager modificationManager)
    {
        bool modifying = true;

        while (modifying)
        {
            Console.WriteLine("Which value would you like to modify? (Enter corresponding number)");
            Console.WriteLine("1. Nuka: " + json["vault"]["storage"]["resources"]["Nuka"]);
            Console.WriteLine("2. Food: " + json["vault"]["storage"]["resources"]["Food"]);
            Console.WriteLine("3. Energy: " + json["vault"]["storage"]["resources"]["Energy"]);
            Console.WriteLine("4. Water: " + json["vault"]["storage"]["resources"]["Water"]);
            Console.WriteLine("5. StimPack: " + json["vault"]["storage"]["resources"]["StimPack"]);
            Console.WriteLine("6. RadAway: " + json["vault"]["storage"]["resources"]["RadAway"]);
            Console.WriteLine("7. Lunchbox: " + ((JArray)json["vault"]["LunchBoxesByType"]).Count(x => (int)x == 0));
            Console.WriteLine("8. MrHandy: " + ((JArray)json["vault"]["LunchBoxesByType"]).Count(x => (int)x == 1));
            Console.WriteLine("9. PetCarrier: " + ((JArray)json["vault"]["LunchBoxesByType"]).Count(x => (int)x == 2));
            Console.WriteLine("10. NukaColaQuantum: " + json["vault"]["storage"]["resources"]["NukaColaQuantum"]);
            Console.WriteLine("11. Finish modifying");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    modificationManager.ModifyValue(json, "Nuka");
                    break;
                case "2":
                    modificationManager.ModifyValue(json, "Food");
                    break;
                case "3":
                    modificationManager.ModifyValue(json, "Energy");
                    break;
                case "4":
                    modificationManager.ModifyValue(json, "Water");
                    break;
                case "5":
                    modificationManager.ModifyValue(json, "StimPack");
                    break;
                case "6":
                    modificationManager.ModifyValue(json, "RadAway");
                    break;
                case "7":
                    modificationManager.ModifySpecialValue(json, "Lunchbox", 0);
                    break;
                case "8":
                    modificationManager.ModifySpecialValue(json, "MrHandy", 1);
                    break;
                case "9":
                    modificationManager.ModifySpecialValue(json, "PetCarrier", 2);
                    break;
                case "10":
                    modificationManager.ModifyValue(json, "NukaColaQuantum");
                    break;
                case "11":
                    modifying = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        string modifiedJson = json.ToString(Newtonsoft.Json.Formatting.Indented);
        string encryptedContent = encryptionManager.Encrypt(modifiedJson);
        File.WriteAllText(filePath, encryptedContent);
        Console.WriteLine("Modifications complete. File encrypted and saved as .sav.");
    }

    private static string GetFilePath()
    {
        while (true)
        {
            Console.WriteLine("Please enter the path to the file:");
            string filePath = Console.ReadLine();

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid file path. Please try again.");
            }
            else
            {
                return filePath;
            }
        }
    }
    
    private static string GetDirectoryPath()
    {
        while (true)
        {
            Console.WriteLine("Please enter the path to the directory:");
            string directoryPath = Console.ReadLine();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Invalid directory path. Please try again.");
            }
            else
            {
                return directoryPath;
            }
        }
    }

    private static void CreateBackup(string filePath, string backupDirectoryPath)
    {
        string backupPath = Path.Combine(backupDirectoryPath, Path.GetFileName(filePath) + ".bak");
        File.Copy(filePath, backupPath, overwrite: true);

        Console.WriteLine($"Backup created at {backupPath}");
    }

    private static void RestoreBackup(string backupFilePath, string restoreDirectoryPath)
    {            
        string restorePath = Path.Combine(restoreDirectoryPath, Path.GetFileName(backupFilePath));
        File.Copy(backupFilePath, restorePath.Replace(".bak", ""), overwrite: true);

        Console.WriteLine($"Backup restored from {backupFilePath}");
    }
}
