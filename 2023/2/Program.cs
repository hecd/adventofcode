using System.Text.RegularExpressions;

var maxValues = new Dictionary<string, int> { ["red"] = 12, ["green"] = 13, ["blue"] = 14 };
var lines = Utils.Utils.GetLines("input.txt");
var possibleGameIdsSum = lines.Select((line, lineNumber) =>
{
    var isImpossible = new[] { "red", "green", "blue" }.Any(color =>
        new Regex($@"(\d+) {color}")
            .Matches(line)
            .Select(m => int.Parse(m.Groups[1].Value))
            .Any(count => count > maxValues[color])
    );
    return isImpossible ? 0 : lineNumber + 1;
}).Sum();

var productOfSets = lines.Select((line) =>
{
    var minByColor = new[] { "red", "green", "blue" }.Select(color =>
        new Regex($@"(\d+) {color}")
            .Matches(line)
            .Select(m => int.Parse(m.Groups[1].Value))
            .Max(m => m)
    );
    return minByColor.Aggregate((acc, x) => acc * x);
}).Sum();

Console.WriteLine(possibleGameIdsSum);
Console.WriteLine(productOfSets);