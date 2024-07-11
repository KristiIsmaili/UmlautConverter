using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UmlautConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Umlaut Name Converter and Variation Generator");
            Console.WriteLine("---------------------------------------------");

            while (true)
            {
                Console.Write("Enter a name (in capital letters) or 'exit' to quit: ");
                string inputName = Console.ReadLine();

                if (inputName.ToLower() == "exit")
                {
                    break;
                }

                List<string> variations = GenerateVariations(inputName);
                Console.WriteLine("Possible variations:");
                foreach (string variation in variations)
                {
                    Console.WriteLine(variation);
                }

                string sqlQuery = GenerateSqlQuery(variations);
                Console.WriteLine($"Generated SQL Query: {sqlQuery}");
                Console.WriteLine();
            }
        }

        static List<string> GenerateVariations(string name)
        {
            Dictionary<string, string> replacements = new Dictionary<string, string>
            {
                { "AE", "Ä" },
                { "OE", "Ö" },
                { "UE", "Ü" },
                { "SS", "ß" }
            };

            var results = new HashSet<string>();
            GenerateVariationsRecursive(name, replacements, results);
            return results.ToList();
        }

        static void GenerateVariationsRecursive(string name, Dictionary<string, string> replacements, HashSet<string> results)
        {
            results.Add(name);

            foreach (var replacement in replacements)
            {
                int index = name.IndexOf(replacement.Key);
                while (index != -1)
                {
                    var newName = name.Substring(0, index) + replacement.Value + name.Substring(index + replacement.Key.Length);
                    GenerateVariationsRecursive(newName, replacements, results);
                    index = name.IndexOf(replacement.Key, index + 1);
                }
            }
        }

        static string GenerateSqlQuery(List<string> variations)
        {
            var conditions = variations.Select(v => $"last_name = '{v.Trim()}'");
            return $"SELECT * FROM tbl_phonebook WHERE {string.Join(" OR ", conditions)}";
        }
    }
}
