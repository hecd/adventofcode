using System.Text.RegularExpressions;
using Utils;

Regex regex = new Regex(@"\d");
var result = Utils.Utils.GetLines("input.txt")
                        .Select(l => regex.Matches(l).Select(m => m.Value))
                        .Select(v => (v.FirstOrDefault() ?? string.Empty) + (v.LastOrDefault() ?? v.FirstOrDefault() ?? string.Empty))
                        .Where(p => !string.IsNullOrEmpty(p))
                        .Sum(p => int.Parse(p));
Console.WriteLine(result);