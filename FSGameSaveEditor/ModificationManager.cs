using Newtonsoft.Json.Linq;

namespace GameSaveEditor
{

    public class ModificationManager
    {
        public void ModifyValue(JObject json, string key)
        {
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine($"Enter new value for {key}:");
                string newValue = Console.ReadLine();

                if (double.TryParse(newValue, out double doubleValue))
                {
                    json["vault"]["storage"]["resources"][key] = doubleValue;
                    Console.WriteLine($"{key} has been updated to {newValue}.");
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid value. Please enter a numeric value.");
                }
            }
        }

        public void ModifySpecialValue(JObject json, string key, int boxId)
        {
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine($"Enter new value for {key}:");
                string newValue = Console.ReadLine();

                if (int.TryParse(newValue, out int intValue))
                {
                    json["vault"]["LunchBoxesCount"] = (int)json["vault"]["LunchBoxesCount"] + intValue;
                    for (int i = 0; i < intValue; i++)
                    {
                        ((JArray)json["vault"]["LunchBoxesByType"]).Add(boxId);
                    }
                    Console.WriteLine($"{key} has been updated by adding {newValue}.");
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid value. Please enter a numeric value.");
                }
            }
        }
    }
}