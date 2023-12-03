using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileProviders;

namespace _2
{
  class Program
  {
    static void Main(string[] args)
    {
      //var boxIdArray = GetFileContent().Split('\n', StringSplitOptions.RemoveEmptyEntries);
      //Console.WriteLine(GetBoxIdListChecksum(boxIdArray));

      var oneCostLines = new List<string>();
      var boxIdArray = GetFileContent().Split('\n', StringSplitOptions.RemoveEmptyEntries);
      foreach (var src in boxIdArray)
      {
        int[] distances = new int[boxIdArray.Length]; 
        foreach (string target in boxIdArray)
        {
          var cost = CalculateLevenstein(src, target);
          if (cost == 1)
          {
            Console.WriteLine($"{src} -> {target}");
            oneCostLines.Add(src);
          }
        }
      }

      var a = oneCostLines.SelectMany(s => s.ToCharArray()).Distinct());

      // Found strings "qysdtrkloagnfozuwujmhrbvx "qysdtrkloagnpfozuwujmhrbvx" and "qysdtrkloagnxfozuwujmhrbvx" with a cost of 1. Manually compared them to find 
      Console.WriteLine(a.First().ToString());
    }

    private static int GetBoxIdListChecksum(string[] boxIdArray)
    {
      int twoCount = 0, threeCount = 0;

      foreach (var line in boxIdArray)
      {
        var characterOccuranceGrouping = line.GroupBy(c => c).ToDictionary(grp => grp.Key, grp => grp.Count());
        bool hasTwo = characterOccuranceGrouping.Any(grp => grp.Value == 2);
        bool hasThree = characterOccuranceGrouping.Any(grp => grp.Value == 3);

        if (hasTwo) twoCount++;
        if (hasThree) threeCount++;

      }
      var product = twoCount * threeCount;
      return product;
    }

    private static string GetFileContent()
    {
      var output = string.Empty;
      IFileProvider provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
      var file = provider.GetFileInfo("input.txt");
      using (var stream = file.CreateReadStream())
      using (var reader = new StreamReader(stream))
      {
        output = reader.ReadToEnd();
      }
      return output;
    }

    public static int CalculateLevenstein(string source1, string source2)
    {
        var source1Length = source1.Length;
        var source2Length = source2.Length;

        // First calculation, if one entry is empty return full length
        if (source1Length == 0)
            return source2Length;

        if (source2Length == 0)
            return source1Length;

        var matrix = new int[source1Length + 1, source2Length + 1];
        // Initialization of matrix with row size source1Length and columns size source2Length
        for (var i = 0; i <= source1Length; matrix[i, 0] = i++){}
        for (var j = 0; j <= source2Length; matrix[0, j] = j++){}

        // Calculate rows and columns distances
        for (var i = 1; i <= source1Length; i++)
        {
            for (var j = 1; j <= source2Length; j++)
            {
                var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }
        // return result
        return matrix[source1Length, source2Length];
    }
  }
}