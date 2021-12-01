using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;

namespace SumOfItsParts
{
  class Program
  {
    const string DATA_DIR = "data";
    const string TEST_INPUT_FILENAME = "input_test";
    const string PROD_INPUT_FILENAME = "input";

    static void Main(string[] args)
    {
      var lines = GetInputFileData(PROD_INPUT_FILENAME).Split('\n', StringSplitOptions.RemoveEmptyEntries);
      PartOne(lines);
      PartTwo(lines);
    }

    private static void PartOne(string[] lines)
    {
      IEnumerable<WorkUnit> workUnits = GetWorkUnits(lines).ToList();
      Console.WriteLine(GetInstructionOrder(workUnits.ToList(), GetWorkers(workerCount: 2, baseStepTicks: 0)));

    }
    private static void PartTwo(string[] lines)
    {
      IEnumerable<WorkUnit> workUnits = GetWorkUnits(lines).ToList();
      Console.WriteLine(GetInstructionOrder(workUnits.ToList(), GetWorkers(workerCount: 5, baseStepTicks: 60)));
    }

    private static List<Worker> GetWorkers(int workerCount, int baseStepTicks)
    {
      List<Worker> workers = new List<Worker>();
      for (int i = 0; i < workerCount; i++)
      {
        workers.Add(new Worker(baseStepTicks));
      }
      return workers;
    }

    private static string GetInstructionOrder(List<WorkUnit> initialWork, List<Worker> workers)
    {
      List<WorkUnit> remainingWork = initialWork.ToList();
      int ticks = 0;
      Queue<WorkUnit> visited = new Queue<WorkUnit>();
      while (visited.Count < initialWork.Count)
      {
        AssignWorkToIdlers(remainingWork, workers);
        workers.ForEach(w => w.DoWork());
        HandleCompletedWork(remainingWork, workers, visited);
        ticks++;
      }

      Console.WriteLine($"Took {ticks} seconds");
      var instructionOrder = string.Empty;
      while (visited.Any())
      {
        instructionOrder += visited.Dequeue().Step;
      }
      return instructionOrder;
    }

    private static void AssignWorkToIdlers(List<WorkUnit> remainingWork, List<Worker> workers)
    {
      foreach (Worker idler in workers.Where(w => w.IsIdle()))
      {
        var pendingWork = remainingWork.OrderBy(n => n.GetRemainingDependencyCount()).ThenBy(n => n.Step).FirstOrDefault();
        if (pendingWork == null || pendingWork.GetRemainingDependencyCount() > 0) break;

        idler.StartWork(pendingWork);
        remainingWork.Remove(pendingWork);
      }
    }

    private static void HandleCompletedWork(List<WorkUnit> remainingWork, List<Worker> workers, Queue<WorkUnit> completed)
    {
      foreach (Worker worker in workers.Where(w => w.IsWorkComplete()))
      {
        var completedWork = worker.CurrentWork;
        remainingWork.ForEach(workUnit => workUnit.RemoveDependency(completedWork));
        completed.Enqueue(completedWork);
        worker.Idle();
      }
    }

    private static IEnumerable<WorkUnit> GetWorkUnits(string[] lines)
    {
      List<WorkUnit> workUnits = new List<WorkUnit>();
      foreach (string line in lines)
      {
        Regex regex = new Regex(@"Step ([A-Z]{1}) must be finished before step ([A-Z]{1}) can begin.");
        var match = regex.Match(line);
        int index = 1;
        var dependencyStep = match.Groups.ElementAt(index++).Value;
        var currentStep = match.Groups.ElementAt(index++).Value;

        WorkUnit dependencyworkUnit = workUnits.FirstOrDefault(workUnitCandidate => workUnitCandidate.Step.Equals(dependencyStep, StringComparison.InvariantCultureIgnoreCase));
        if (dependencyworkUnit == null)
        {
          dependencyworkUnit = new WorkUnit(dependencyStep);
          workUnits.Add(dependencyworkUnit);
        }

        WorkUnit currentworkUnit = workUnits.FirstOrDefault(workUnitCandidate => workUnitCandidate.Step.Equals(currentStep, StringComparison.InvariantCultureIgnoreCase));
        if (currentworkUnit == null)
        {
          currentworkUnit = new WorkUnit(currentStep);
          workUnits.Add(currentworkUnit);
        }
        currentworkUnit.AddDependency(dependencyworkUnit);
      }
      return workUnits;
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
