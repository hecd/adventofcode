using System.Numerics;

class Program
{
    public static void Main()
    {
        var inputFile = "input.txt.sample";
        var input = GetInputContent(inputFile).Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var result1 = solve(input);
        var result2 = 0;
        Console.WriteLine($"Part 1: {result1}");
        Console.WriteLine($"Part 2: {result2}");
    }

    private static int solve(string[] input)
    {
        var matrix = new char[input.Length, input.First().Length];
        var survivorMatrix = new bool[2, input.Length, input.First().Length];

        int i = 0;
        int j = 0;
        foreach (var line in input)
        {
            j = 0;
            foreach (var c in line)
            {
                matrix[i, j] = c;
                j++;
            }
            i++;
        }


        string gamma = string.Empty;
        string epsilon = string.Empty;

        Nullable<char> mostCommon = null;
        Nullable<char> leastCommon = null;
        for(int x = 0; x < input.First().Length; x++)
        {
            string verticalLine = string.Empty;
            for(int y = 0; y < input.Length; y++)
            {
                var v = matrix[y, x];
                verticalLine += v.ToString();

                char lastValue = matrix[y, x-1];
                if (mostCommon != null && lastValue != mostCommon)
                {
                    survivorMatrix[0, y, x] = false;
                }
            }
            mostCommon = verticalLine.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
            leastCommon = verticalLine.GroupBy(x => x).OrderByDescending(x => x.Count()).Last().Key;
            gamma += mostCommon;
            epsilon += leastCommon; 
        }
        var gammaValue = Convert.ToInt32(gamma, 2);
        var epsilonValue = Convert.ToInt32(epsilon, 2);

        return gammaValue * epsilonValue;
    }

    private static string GetInputContent(string fileName)
    {
        try 
        {
            using (var sr = new StreamReader(fileName))
            {
                return sr.ReadToEnd();
            }

        }
        catch(Exception ex)
        {
            Console.WriteLine($"could not read file {fileName}", ex);
        }
        return string.Empty;
    }
}