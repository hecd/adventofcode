using System.Text.RegularExpressions;

static int GetImpossibleGamesGameIdSum(Dictionary<string, int> maxValues, IEnumerable<string> lines)
{
    return lines.Select((line, lineNumber) =>
    {
        var isImpossible = new[] { "red", "green", "blue" }.Any(color =>
            new Regex($@"(\d+) {color}")
                .Matches(line)
                .Select(m => int.Parse(m.Groups[1].Value))
                .Any(count => count > maxValues[color])
        );
        return isImpossible ? 0 : lineNumber + 1;
    }).Sum();
}

static int GetMinimumCubeColorPowerSum(IEnumerable<string> lines)
{
    return lines.Select(line =>
    {
        var minByColor = new[] { "red", "green", "blue" }.Select(color =>
            new Regex($@"(\d+) {color}")
                .Matches(line)
                .Select(m => int.Parse(m.Groups[1].Value))
                .Max(m => m)
        );
        return minByColor.Aggregate((acc, x) => acc * x);
    }).Sum();
}

var maxValues = new Dictionary<string, int> { ["red"] = 12, ["green"] = 13, ["blue"] = 14 };
var lines = Utils.Utils.GetLines("input.txt");
int partOne = GetImpossibleGamesGameIdSum(maxValues, lines);
int partTwo = GetMinimumCubeColorPowerSum(lines);

Console.WriteLine(partOne);
Console.WriteLine(partTwo);
