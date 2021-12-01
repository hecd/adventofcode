using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SumOfItsParts
{
  public class WorkUnit
  {
    public string Step { get; set; }
    private LinkedList<WorkUnit> Dependencies { get; set; }
    public WorkUnit(string step)
    {
      Step = step;
      Dependencies = new LinkedList<WorkUnit>();
    }

    public void AddDependency(WorkUnit dependency)
    {
      Dependencies.AddLast(dependency);
    }

    public void RemoveDependency(WorkUnit workUnit)
    {
      if (Dependencies.Contains(workUnit))
      {
        Dependencies.Remove(workUnit);
      }
    }

    public int GetRemainingDependencyCount()
    {
      return Dependencies.Count;
    }
  }
}