using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SumOfItsParts
{
  class Worker
  {
    public int TicksWorked { get; set; }
    public WorkUnit CurrentWork { get; set; }
    public int BaseTicksPerStep { get; }

    public Worker(int minimumWorkTicks)
    {
      Idle();
      BaseTicksPerStep = minimumWorkTicks;
    }

    public bool IsWorkComplete()
    {
      return CurrentWork != null && TicksWorked >= BaseTicksPerStep + GetExtraStepTime();
    }


    public void StartWork(WorkUnit workUnit)
    {
      CurrentWork = workUnit;
      TicksWorked = 0;
    }

    public void Idle()
    {
      CurrentWork = null;
      TicksWorked = 0;
    }

    public bool IsIdle()
    {
      return CurrentWork == null;
    }

    private int GetExtraStepTime()
    {
      return CurrentWork.Step.FirstOrDefault() - ('A') + 1;
    }

    public void DoWork()
    {
      if (CurrentWork == null) return;

      TicksWorked++;
    }
  }
}