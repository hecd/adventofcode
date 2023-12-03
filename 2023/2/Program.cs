using System.Text.RegularExpressions;

var maxValues = new Dictionary<string, int> { ["red"] = 12, ["green"] = 13, ["blue"] = 14 };
var lines = Utils.Utils.GetLines("input.txt");
var possibleGameIdsSum = lines.Select((line, gameId) =>
{
    var isImpossible = new[] { "red", "green", "blue" }.Any(color =>
        new Regex($@"(\d+) {color}")
            .Matches(line)
            .Select(m => int.Parse(m.Groups[1].Value))
            .Any(count => count > maxValues[color])
    );
    return isImpossible ? 0 : gameId + 1;
}).Sum();

Console.WriteLine(possibleGameIdsSum);
