using System;
using System.Collections.Generic;
using System.Linq;
using ReposeRecords;

namespace ReposeRecord
{
  class Guard
  {
    public DateTime FellAsleepAt { get; set; }

    private Dictionary<int, int> MinuteMap { get; set; }
    public int Id { get; }

    public Guard(int id)
    {
      Id = id;
      MinuteMap = new Dictionary<int, int>();
      for (int minute = 0; minute < 60; minute++)
      {
        MinuteMap.Add(minute, 0);
      }
    }

    public void Sleep(DateTime eventTime)
    {
      FellAsleepAt = eventTime;
    }

    public void Wake(DateTime eventTime)
    {
      int sleepDuration = (int)(eventTime.Subtract(FellAsleepAt).TotalMinutes);
      for (int minute = 0; minute < sleepDuration; minute++)
      {
        int minuteIndex = FellAsleepAt.Add(new TimeSpan(hours: 0, minutes: minute, seconds: 0)).Minute;
        MinuteMap[minuteIndex] = MinuteMap[minuteIndex] + 1;
      }
    }
    public KeyValuePair<int, int> GetMostFrequentMinuteAsleep()
    {
      return MinuteMap.Aggregate((x, y) => x.Value > y.Value ? x : y);
    }

    public int GetTotalSleepTime()
    {
      return MinuteMap.Sum(kvp => kvp.Value);
    }

    public void ProcessObservation(Observation observation)
    {
      switch (observation.Event)
      {
        case EventType.Sleep:
          Sleep(observation.Timestamp);
          break;
        case EventType.Wake:
          Wake(observation.Timestamp);
          break;
      }
    }
  }
}