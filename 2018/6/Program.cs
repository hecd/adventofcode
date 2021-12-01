using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;

namespace ChronalCoordinates
{
  class Program
  {
    const string DATA_DIR = "data";
    const string TEST_INPUT_FILENAME = "input_test";
    const string PROD_INPUT_FILENAME = "input";

    class Point
    {
      public int X { get; set; }
      public int Y { get; set; }
      public Point(int x, int y)
      {
        X = x;
        Y = y;
      }
    }
    static void Main(string[] args)
    {
      var lines = GetInputFileData(PROD_INPUT_FILENAME).Split('\n', StringSplitOptions.RemoveEmptyEntries);
      List<Point> coordinates = GetCoordinates(lines);
      Console.WriteLine(PartOne(coordinates));
      Console.WriteLine(PartTwo(coordinates));
    }
    private static int PartOne(List<Point> coordinates)
    {
      Dictionary<Point, int> closestCoordinateMapping = GetClosestCoordinateMappingForPoints(coordinates);
      int largestNonInfiniteArea = closestCoordinateMapping.OrderByDescending(coord => coord.Value).FirstOrDefault().Value;
      return largestNonInfiniteArea;
    }

    private static int PartTwo(List<Point> coordinates)
    {
      return GetRegionSize(coordinates, maxTotalDistance: 10000);
    }

    private static Dictionary<Point, int> GetClosestCoordinateMappingForPoints(IEnumerable<Point> coordinates)
    {
      Dictionary<Point, int> closestMapping = coordinates.Select(coord => new KeyValuePair<Point, int>(coord, 0))
                                                                     .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
      foreach (Point point in GetNonInfinitePoints(coordinates))
      {
        var distanceMap = GetDistanceToCoordinates(point, coordinates).OrderBy(kvp => kvp.Value);
        var wasDraw = distanceMap.Take(2).Select(distanceMapping => distanceMapping.Value).Distinct().Count() == 1;
        if (wasDraw) continue;

        var winner = distanceMap.FirstOrDefault().Key;
        closestMapping[winner] += 1;
      }
      return closestMapping;
    }

    private static IEnumerable<Point> GetNonInfinitePoints(IEnumerable<Point> coordinates)
    {
      List<Point> points = new List<Point>();
      int top = coordinates.Min(coord => coord.Y);
      int bottom = coordinates.Max(coord => coord.Y);
      int left = coordinates.Min(coord => coord.X);
      int right = coordinates.Max(coord => coord.X);
      for (int x = left; x < right; x++)
      {
        for (int y = top; y < bottom; y++)
        {
          points.Add(new Point(x, y));
        }
      }
      return points;
    }

    private static int GetRegionSize(IEnumerable<Point> coordinates, int maxTotalDistance)
    {
      int regionSize = 0;
      foreach (Point point in GetNonInfinitePoints(coordinates))
      {
        var distanceMap = GetDistanceToCoordinates(point, coordinates);
        if (distanceMap.Sum(kvp => kvp.Value) < maxTotalDistance)
        {
          regionSize++;
        }
      }
      return regionSize;
    }
    private static List<Point> GetCoordinates(string[] lines)
    {
      List<Point> coordinates = new List<Point>();
      foreach (var line in lines)
      {
        var coordinateString = line.Split(", ", StringSplitOptions.RemoveEmptyEntries);
        int x = Int32.Parse(coordinateString.ElementAt(0));
        int y = Int32.Parse(coordinateString.ElementAt(1));
        coordinates.Add(new Point(x, y));
      }
      return coordinates;
    }

    private static Dictionary<Point, int> GetDistanceToCoordinates(Point point, IEnumerable<Point> coordinates)
    {
      return coordinates.Select(coord => new KeyValuePair<Point, int>(coord, GetManhattanDistance(point.X, point.Y, coord.X, coord.Y)))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    static int GetManhattanDistance(int startX, int startY, int endX, int endY)
    {
      return Math.Abs(startX - endX) + Math.Abs(startY - endY);
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
