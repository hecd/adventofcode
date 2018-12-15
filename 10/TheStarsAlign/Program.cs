using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;

namespace TheStarsAlign
{
  class Program
  {
    const string DATA_DIR = "data";
    const string TEST_INPUT_FILENAME = "input_test";
    const string PROD_INPUT_FILENAME = "input";

    class Point
    {
      public int X { get; private set; }
      public int Y { get; private set; }
      public int VelocityX { get; private set; }
      public int VelocityY { get; private set; }

      public Point(int initialX, int initialY, int velocityX, int velocityY)
      {
        X = initialX;
        Y = initialY;
        VelocityX = velocityX;
        VelocityY = velocityY;
      }

      public void Tick()
      {
        X += VelocityX;
        Y += VelocityY;
      }

      public bool IsAt(int x, int y)
      {
        return X == x && Y == y;
      }
    }

    static void Main(string[] args)
    {
      var lines = GetInputFileData(PROD_INPUT_FILENAME).Split('\n', StringSplitOptions.RemoveEmptyEntries);
      IEnumerable<Point> points = GetPointsFromLines(lines);
      int tick = 0;
      while(tick < 100000)
      {
        int minX = points.Min(p => p.X);
        int minY = points.Min(p => p.Y);
        int maxX = points.Max(p => p.X);
        int maxY = points.Max(p => p.Y);
        int diffY = maxY - minY;
        // Found by manual exploration. Assuming one line. Easier to guess height than width based on the assumption of one line.
        int estimatedFontSize = 12;
        if (diffY <= estimatedFontSize)
        {
          Console.WriteLine($"Tick: {tick} {diffY}");
          PrintGrid(points, minX, minY, maxX, maxY);
          Console.WriteLine();
        }
        foreach (Point point in points)
        {
          point.Tick();
        }
        tick++;
      }
    }

    static void PrintGrid(IEnumerable<Point> points, int minX, int minY, int maxX, int maxY)
    {
      for (int y = minY; y <= maxY; y++)
      {
        for (int x = minX; x <= maxX; x++)
        {
          char character = '.';
          if (points.Where(p => p.IsAt(x, y)).Any())
          {
            character = '#';
          }
          Console.Write(character);
        }
        Console.WriteLine();
      }
    }

    private static IEnumerable<Point> GetPointsFromLines(string[] lines)
    {
      List<Point> points = new List<Point>();
      foreach (string line in lines)
      {
        string regexLine = line.Replace(" ", "");
        Regex regex = new Regex(@"position=<([-0-9]+),([-0-9]+)>velocity=<([-0-9]+),([-0-9]+)>");
        var match = regex.Match(regexLine);
        var groups = match.Groups;
        int index = 1;
        int x = Int32.Parse(groups.ElementAt(index++).Value);
        int y = Int32.Parse(groups.ElementAt(index++).Value);
        int velocityX = Int32.Parse(groups.ElementAt(index++).Value);
        int velocityY = Int32.Parse(groups.ElementAt(index++).Value);

        Point point = new Point(x, y, velocityX, velocityY);
        points.Add(point);
      }
      return points;
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
