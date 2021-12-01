using System.Collections.Generic;
using System.Linq;

namespace MemoryManeuver
{
  public class Node
  {
    public IEnumerable<Node> Children { get; set; }
    public IEnumerable<int> MetadataEntries { get; }

    public int TotalMetadataSum
    {
      get
      {
        return MetadataEntries.Sum() + Children.Select(child => child.TotalMetadataSum).Sum();
      }
    }

    public int Value
    {
      get
      {
        if (!Children.Any()) return MetadataEntries.Sum();

        return MetadataEntries.Select(mde => Children.ElementAtOrDefault(mde - 1))
                              .OfType<Node>()
                              .Sum(c => c.Value);
      }
    }

    public Node(IEnumerable<Node> children, IEnumerable<int> metadataEntries)
    {
      Children = children;
      MetadataEntries = metadataEntries;
    }
  }
}