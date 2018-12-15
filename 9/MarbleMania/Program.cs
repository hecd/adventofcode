using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileProviders;

namespace MarbleMania
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine(PartOne());
      // Note: PartTwo() takes ~30 min to run
      //Console.WriteLine(PartTwo());
    }

    private static long PartOne()
    {
      int playerCount = 464;
      int lastMarble = 71730;
      return GetMarbleHighscore(playerCount, lastMarble);
    }

    private static long PartTwo()
    {
      int playerCount = 464;
      int lastMarble = 71730 * 100;
      return GetMarbleHighscore(playerCount, lastMarble);
    }
    private static long GetMarbleHighscore(int playerCount, int lastMarble)
    {
      long[] scores = new long[playerCount];
      LinkedList<int> gameCircle = new LinkedList<int>();
      gameCircle.AddFirst(0);
      LinkedListNode<int> currentMarble = gameCircle.Find(0);

      for (int marbleNumber = 1; marbleNumber <= lastMarble; marbleNumber++)
      {
        if (marbleNumber % 23 == 0)
        {
          LinkedListNode<int> removed = currentMarble.Rotate(-7);
          scores[marbleNumber % playerCount] += marbleNumber + removed.Value;
          currentMarble = removed.Rotate(1);
          gameCircle.Remove(removed.Value);
        }
        else
        {
          var insertAfter = currentMarble.Rotate(1);
          currentMarble = gameCircle.AddAfter(insertAfter, marbleNumber);
        }
      }
      return scores.Max();
    }
  }
}