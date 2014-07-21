using System;
using System.Collections.Generic;

namespace WinWarCS.Data.Game
{
   public class MapPath
   {
      private List<Node> path;

      public int StartX { get; set; }
      public int StartY { get; set; }

      public int EndX { get; set; }
      public int EndY { get; set; }

      internal Node this[int index]
      {
         get 
         { 
            if (index < 0 || index >= Count)
               return null;

            return path[index]; 
         }
      }

      public int Count
      {
         get { return path.Count; }
      }

      public MapPath ()
      {
         path = new List<Node> ();
      }

      internal void BuildFromFinalNode(Node finalNode)
      {
         path.Clear ();

         Node node = finalNode;

         path.Add(node);
         while (node.parent != null)
         {
            path.Add(node);
            node = node.parent;
         }
         path.Reverse();
      }
   }
}

