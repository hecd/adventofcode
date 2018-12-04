using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;
using ReposeRecord;

namespace ReposeRecords
{
  class Program
  {
    const string DATA_DIR = "data";
    const string TEST_INPUT_FILENAME = "input_test";
    const string PROD_INPUT_FILENAME = "input";

    static void Main(string[] args)
    {
      var lines = GetInputFileData(PROD_INPUT_FILENAME).Split('\n', StringSplitOptions.RemoveEmptyEntries);
      IOrderedEnumerable<Observation> chronologicalObservations = GetChronologicalObservations(lines);
      IEnumerable<Guard> guards = GetGuards(chronologicalObservations);
      CalculateStrategyAnswers(guards);
    }

    static void CalculateStrategyAnswers(IEnumerable<Guard> guards)
    {
      var sleepiestGuard = guards.OrderByDescending(g => g.GetTotalSleepTime())
                                 .FirstOrDefault();
      int strategyOneAnswer = sleepiestGuard.Id * sleepiestGuard.GetMostFrequentMinuteAsleep().Key;

      var guardIdAndHighestMinuteFrequency = guards.Select(g => new { MostFrequentMinute = g.GetMostFrequentMinuteAsleep(), g.Id })
                                                   .OrderByDescending(g => g.MostFrequentMinute.Value)
                                                   .FirstOrDefault();
      int strategyTwoAnswer = guardIdAndHighestMinuteFrequency.Id * guardIdAndHighestMinuteFrequency.MostFrequentMinute.Key;
      Console.WriteLine($"Strategy one answer: {strategyOneAnswer}");
      Console.WriteLine($"Strategy two answer: {strategyTwoAnswer}");
    }

    private static IEnumerable<Guard> GetGuards(IOrderedEnumerable<Observation> chronologicalObservations)
    {
      Dictionary<int, Guard> knownGuards = new Dictionary<int, Guard>();
      Guard guard = null;
      foreach (Observation observation in chronologicalObservations)
      {
        bool guardOnDutyChange = observation.GuardId.HasValue;
        if (guardOnDutyChange)
        {
          int guardId = observation.GuardId.Value;
          bool hasGuardAlreadyBeenOnDuty = knownGuards.TryGetValue(guardId, out guard);
          if (!hasGuardAlreadyBeenOnDuty)
          {
            guard = new Guard(guardId);
            knownGuards.Add(guardId, guard);
          }
        }
        guard.ProcessObservation(observation);
      }
      return knownGuards.Values;
    }

    private static IOrderedEnumerable<Observation> GetChronologicalObservations(string[] lines)
    {
      return lines.Select(line => GetObservation(line)).OrderBy(observation => observation.Timestamp);
    }

    private static Observation GetObservation(string line)
    {
      var regex = new Regex(@"\[(.*)\] (?:(Guard #(\d+))|(falls asleep)|(wakes up))");
      var matches = regex.Match(line);
      var timestamp = matches.Groups[1]?.Value;
      // Jumps from group index 1 to 3 since 'Guard #' is captured in order to apply OR conditions between guard id / asleep / awake.
      var guardIdString = matches.Groups[3]?.Value;
      var asleep = matches.Groups[4]?.Value;
      var awake = matches.Groups[5]?.Value;

      Observation observation = new Observation();
      if (!string.IsNullOrWhiteSpace(asleep)) observation.Event = EventType.Sleep;
      if (!string.IsNullOrWhiteSpace(awake)) observation.Event = EventType.Wake;
      if (Int32.TryParse(guardIdString, out int guardId))
      {
        observation.GuardId = guardId;
      }
      observation.Timestamp = DateTime.Parse(timestamp);
      return observation;
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
