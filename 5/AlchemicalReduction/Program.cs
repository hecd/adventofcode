using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;

namespace AlchemicalReduction
{
  class Program
  {
    const string DATA_DIR = "data";
    const string TEST_INPUT_FILENAME = "input_test";
    const string PROD_INPUT_FILENAME = "input";

    static void Main(string[] args)
    {
      var content = GetInputFileData(PROD_INPUT_FILENAME).Replace("\n", "");
      PartOne(content);
      PartTwo(content);
    }

    static void PartOne(string polymerString)
    {
      string content = TriggerReaction(polymerString);
      Console.WriteLine($"Length after fully reacting the polymer is {content.Length}");
    }

    static void PartTwo(string polymerString)
    {
      var distinctPolymerUnits = polymerString.Select(x => char.ToUpper(x)).Distinct();
      Dictionary<char, int> polymerLengthAfterUnitRemovalAndReaction = new Dictionary<char, int>();
      foreach (var unit in distinctPolymerUnits)
      {
        string polymerAfterUnitRemoved = polymerString.Replace(char.ToLower(unit).ToString(), string.Empty).Replace(char.ToUpper(unit).ToString(), string.Empty);
        int lengthAfterReaction = TriggerReaction(polymerAfterUnitRemoved).Length;
        polymerLengthAfterUnitRemovalAndReaction.Add(unit, lengthAfterReaction);
      }
      var shortestLength = polymerLengthAfterUnitRemovalAndReaction.OrderBy(kvp => kvp.Value).First();
      Console.WriteLine($"Shortest length was {shortestLength.Value} for unit '{shortestLength.Key}'");
    }

    private static string TriggerReaction(string content)
    {
      for (int i = 0; i < content.Length;)
      {
        if (i + 1 >= content.Length) break;

        char current = content[i];
        char neighbour = content[i + 1];
        if (IsReactive(current, neighbour))
        {
          content = content.Remove(i, 2);
          i = Math.Max(0, i - 2);
        }
        else
        {
          i++;
        }
      }
      return content;
    }
    private static bool IsReactive(char current, char neighbour)
    {
      // Same character but different casing
      return char.ToUpper(current).Equals(char.ToUpper(neighbour)) && !current.Equals(neighbour);
    }
    private static string GetInputFileData(string fileName)
    {
      string output = string.Empty;
      IFileProvider provider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), DATA_DIR));
      IFileInfo file = provider.GetFileInfo(fileName);
      using (var stream = file.CreateReadStream())
      using (var reader = new StreamReader(stream))
      {
        output = reader.ReadToEnd();
      }
      return output;
    }
  }
}
