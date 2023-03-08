using System;
using System.Collections.Generic;

namespace WinWarGame.Data.Game
{
   public interface IMapPathNode
   {
      int X { get; set; }
      int Y { get; set; }
   }

   public class MapPath
   {
      private List<IMapPathNode> path;

      public int StartX { get; set; }
      public int StartY { get; set; }

      public int EndX { get; set; }
      public int EndY { get; set; }

      internal IMapPathNode this[int index]
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
         path = new List<IMapPathNode> ();
      }

      internal void BuildFromFinalAStarNode(AStarNode finalNode)
      {
         path.Clear ();

         if (finalNode == null)
            return;

         AStarNode node = finalNode;

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

