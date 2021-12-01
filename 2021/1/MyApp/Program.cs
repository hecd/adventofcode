using System;
using System.IO;

class Program
{
    public static void Main()
    {
        var inputFile = "input.txt";
        var inputContent = GetInputContent(inputFile);
        var numberArray = Array.ConvertAll(inputContent.Split('\n', StringSplitOptions.RemoveEmptyEntries), Int32.Parse);
        Console.WriteLine($"Part 1 answer: {GetIncreases(numberArray, 1)}");
        Console.WriteLine($"Part 2 answer: {GetIncreases(numberArray, 3)}");
    }

    private static int GetIncreases(int[] inputIntegers, int windowSize)
    {
        int increases = 0;
        for (int i = 0; i < inputIntegers.Count() - windowSize; i++)
        {
            if (inputIntegers[i + windowSize] > inputIntegers[i]) increases++;
        }
        return increases;
    }

    private static string GetInputContent(string fileName)
    {
        var result = string.Empty;
        try
        {
            using (var sr = new StreamReader(fileName))
            {
                result = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not read file {fileName}", e);
        }
        return result;
    }
}