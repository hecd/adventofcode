using System.Text.RegularExpressions;

static int GetNumericValue(string word)
{
    string[] words = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    if (int.TryParse(word, out int result))
    {
        return result;
    }
    for (int i = 0; i < words.Length; i++)
    {
        if (word.Contains(words[i]))
        {
            return i + 1;
        }
    }
    throw new FormatException($"Invalid numeric word: {word}");
}

static int SumCalibrationValues(Regex regex)
{
    var lines = Utils.Utils.GetLines("input.txt");
    return lines.Select(line =>
    {
        var matches = regex.Matches(line).Select(m => m.Value);
        var firstDigit = GetNumericValue(matches.First());
        var lastDigit = GetNumericValue(matches.Last());
        return firstDigit * 10 + lastDigit;
    }).Sum();
}

Regex partOneRegex = new Regex(@"\d");
Regex partTwoRegex = new Regex(@"one|two|three|four|five|six|seven|eight|nine|\d");
Console.WriteLine(SumCalibrationValues(partOneRegex));
Console.WriteLine(SumCalibrationValues(partTwoRegex));