class Program
{
    public static void Main()
    {
        var inputFile = "input.txt.sample";
        var input = GetInputContent(inputFile).Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var part1 = CalculatePosition(input, 0, 1);
        var part2 = CalculatePosition(input, 1, 0);

        Console.WriteLine($"Part 1: {part1.Item1 * part1.Item2}");
        Console.WriteLine($"Part 2: {part2.Item1 * part2.Item2}");
    }

    private static (int, int) CalculatePosition(string[] input, int horizontalDepthFactor, int verticalDepthFactor)
    {
        int posHorizontal = 0;
        int posDepth = 0;
        int aim = 0;
        foreach (var line in input)
        {
            var splittedLine = line.Split();
            var operationName = splittedLine[0];
            var operationValue = int.Parse(splittedLine[1]);
            switch (operationName)
            {
                case "forward":
                    posHorizontal += operationValue;
                    posDepth += (aim * operationValue) * horizontalDepthFactor;
                    break;
                case "up":
                    posDepth -= operationValue * verticalDepthFactor;
                    aim -= operationValue;
                    break;
                case "down":
                    posDepth += operationValue * verticalDepthFactor;
                    aim += operationValue;
                    break;
            }
        }
        return (posHorizontal, posDepth);
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