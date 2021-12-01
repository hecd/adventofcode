using System;
using ReposeRecords;

namespace ReposeRecord
{

  public class Observation
  {
    public DateTime Timestamp { get; set; }
    public EventType Event { get; set; }
    public int? GuardId { get; set; }
  }
}