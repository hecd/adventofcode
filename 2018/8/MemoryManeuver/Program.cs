using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileProviders;

namespace MemoryManeuver
{
  class Program
  {
    const string DATA_DIR = "data";
    const string TEST_INPUT_FILENAME = "input_test";
    const string PROD_INPUT_FILENAME = "input";

    static void Main(string[] args)
    {
      var lines = GetInputFileData(PROD_INPUT_FILENAME).Split('\n', StringSplitOptions.RemoveEmptyEntries);
      var initialTree = lines[0].Split(' ').Select(Int32.Parse).ToArray();
      Node rootNode = GetRootNode(initialTree, out _);
      PartOne(rootNode);
      PartTwo(rootNode);
    }

    private static void PartOne(Node rootNode)
    {
      int sum = rootNode.TotalMetadataSum;
      Console.WriteLine(sum);
    }

    private static void PartTwo(Node rootNode)
    {
      int sum = rootNode.Value;
      Console.WriteLine(sum);
    }

    private static Node GetRootNode(IEnumerable<int> tree, out IEnumerable<int> remainingData)
    {
      int childNodeQuantity = tree.ElementAt(0);
      int metadataEntryQuantity = tree.ElementAt(1);
      // Consume node header data.
      tree = tree.Skip(2);

      var children = Enumerable.Range(0, childNodeQuantity).Select(i => GetRootNode(tree, out tree)).ToList();

      // At this point only node metadata entries remain to take.
      var metadata = tree.Take(metadataEntryQuantity).ToList();

      // Consume node metadata so next node can be parsed from index 0
      remainingData = tree.Skip(metadataEntryQuantity);
      return new Node(children, metadata);
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
