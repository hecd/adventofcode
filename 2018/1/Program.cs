using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace _1
{
    class Program
    {
        static void Main(string[] args)
        {
            string newlineSeparatedContent = GetFileContent();
            var numberArray = Array.ConvertAll(newlineSeparatedContent.Split('\n', StringSplitOptions.RemoveEmptyEntries), Int32.Parse);
            //Console.WriteLine(GetSum(numberArray));
            Console.WriteLine(GetFirstDuplicateFrequency(numberArray));
        }

        private static int GetSum(int[] numberArray)
        {
            var sum = 0;
            foreach (var n in numberArray)
            {
              sum += n;
            }
            return sum;
        }

        private static int GetFirstDuplicateFrequency(int[] numberArray)
        {
          List<int> previousSumList = new List<int>();
          bool foundDuplicate = false;
          int sum = 0;
          while(!foundDuplicate)
          {
            foreach (var n in numberArray)
            {
              sum += n;
              if (previousSumList.Contains(sum)) {
                return sum;
              }
              previousSumList.Add(sum);
            }
          }
          return -1;
        }

        private static string GetFileContent()
        {
            var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            IFileInfo file = provider.GetFileInfo("input.txt");
            string output = string.Empty;
            using (var stream = file.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                output = reader.ReadToEnd();
            }
            return output;
        }
    }
}