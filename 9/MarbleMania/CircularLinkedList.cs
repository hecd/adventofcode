using System;
using System.Collections.Generic;

namespace MarbleMania
{
  static class CircularLinkedList
  {
    public static LinkedListNode<T> Rotate<T>(this LinkedListNode<T> current, int steps)
    {
      if (steps > 0)
      {
        for(int i = 0; i < steps; i++)
        {
          current = current.Next ?? current.List.First;
        }
      }
      else if (steps < 0)
      {
        for(int i = 0; i < Math.Abs(steps); i++)
        {
          current = current.Previous ?? current.List.Last;
        }
      }
      return current;
    }
  }
}