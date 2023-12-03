using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileProviders;

namespace _3
{
  internal class Claim
  {
    public readonly int id;
    public readonly int startX;
    public readonly int startY;
    public readonly int width;
    public readonly int height;
    public bool IsOverlapped { get; set; }

    public Claim(int id, int startX, int startY, int width, int height)
    {
      this.id = id;
      this.startX = startX;
      this.startY = startY;
      this.width = width;
      this.height = height;
      IsOverlapped = false;
    }

    public bool Contains(int x, int y)
    {
      return x >= startX && x <= startX + width &&
             y >= startY && y <= startY + height;
    }
  }

  class Program
  {
    const int GRID_SQUARE_SIDE = 1000;
    static void Main(string[] args)
    {
      int[,] grid = new int[GRID_SQUARE_SIDE, GRID_SQUARE_SIDE];
      Array.Clear(grid, 0, grid.Length);

      var content = GetFileContent();
      var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

      List<Claim> claims = new List<Claim>();
      int overlapCount = 0;
      foreach (var line in lines)
      {
        Claim claim = GetClaimByLine(line);
        claims.Add(claim);
        for (int x = claim.startX; x < claim.startX + claim.width; x++)
        {
          for (int y = claim.startY; y < claim.startY + claim.height; y++)
          {
            grid[x, y] = grid[x, y] + 1;
            int currentValue = grid[x, y];
            // 2 since task is to find coordinates with at least two overlaps, not exact number of overlapping squares.
            if (currentValue == 2)
            {
              overlapCount++;
              foreach (Claim claimCandidate in claims)
              {
                if (claimCandidate.Contains(x, y))
                {
                  claimCandidate.IsOverlapped = true;
                }
              }
            }
          }
        }
      }
      Console.WriteLine($"Overlapped squares: {overlapCount}");
      var nonOverlappedClaim = claims.Where(c => c.IsOverlapped == false).FirstOrDefault();

      Console.WriteLine($"Claim not overlapped: {nonOverlappedClaim.id}");

    }

    static Claim GetClaimByLine(string line)
    {
      // Format: #1 @ 1,3: 4x4
      var regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");
      var matches = regex.Matches(line);
      Match match = matches.FirstOrDefault();
      var groups = match.Groups;
      int index = 1;
      int claimId = Int32.Parse(groups.ElementAt(index++).Value);
      int startX = Int32.Parse(groups.ElementAt(index++).Value);
      int startY = Int32.Parse(groups.ElementAt(index++).Value);
      int width = Int32.Parse(groups.ElementAt(index++).Value);
      int height = Int32.Parse(groups.ElementAt(index++).Value);
      return new Claim(claimId, startX, startY, width, height);
    }

    static string GetFileContent()
    {
      string output = string.Empty;
      IFileProvider provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
      IFileInfo file = provider.GetFileInfo("input");
      using (var stream = file.CreateReadStream())
      using (var reader = new StreamReader(stream))
      {
        output = reader.ReadToEnd();
      }
      return output;
    }
  }
}
