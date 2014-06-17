using System;
using System.Collections.Generic;

namespace WinWarCS.Data.Game
{
   /// <summary>
   /// Hate list entry
   /// </summary>
   internal struct HateListEntry
   {
      /// <summary>
      /// Target
      /// </summary>
      internal Entity Target;
      /// <summary>
      /// Value
      /// </summary>
      internal int Value;
   } // class HateListEntry

   /// <summary>
   /// A combination of one or more of the following values defines custom behaviour when dealing with hate list functions.
   /// </summary>
   internal enum HateListParam
   {
      /// <summary>
      /// No additional flags.
      /// </summary>
      None = 0,
      /// <summary>
      /// If the target entity is already on the list, the hate value will be added to the current value, otherwise it will be set to the new hate value.
      /// </summary>
      AddValue = 1,
      /// <summary>
      /// Method does nothing, if the entity is already on the hate list.
      /// </summary>
      IgnoreIfPresent = 2,
      /// <summary>
      /// Pushes this entity to the top of the list. The new hate value of the target will be PreviousHighestEntity.Value + <c>Value</c>.
      /// </summary>
      PushToTop = 4
   } // enum HateListParam

   internal class HateList
   {
      /// <summary>
      /// Hate list
      /// </summary>
      internal HateListEntry[] entries;

      public HateList ()
      {
         entries = new HateListEntry[10];
      }

      /// <summary>
      /// Clears the hate list
      /// </summary>
      internal void Wipe()
      {
         for (int i = 0; i < entries.Length; i++)
         {
            entries[i].Target = null;
            entries[i].Value = 0;
         }
      } // Wipe()

      /// <summary>
      /// Find hate list entry. Returns -1 if the target is not on the hate list
      /// </summary>
      internal int IndexOfTarget(Entity Target)
      {
         for (int i = 0; i < entries.Length; i++)
            if (entries[i].Target == Target)
               return i;

         return -1;
      } // IndexOfTarget(Target)

      /// <summary>
      /// Get next free hate list index
      /// </summary>
      private int GetNextFreeHateListIndex()
      {
         for (int i = 0; i < entries.Length; i++)
            if (entries[i].Target == null)
               return i;

         return -1;
      } // GetNextFreeHateListIndex()

      /// <summary>
      /// Sort hate list
      /// </summary>
      private void SortHateList()
      {
         // Bubblesort the hatelist
         int i, j;

         for (i = 9; i >= 0; i--)
         {
            bool bChanged = false;

            for (j = 0; j < i; j++)
            {
               HateListEntry hle = entries[j];
               HateListEntry hle2 = entries[j + 1];

               if (hle.Target == null)
                  break;

               if (hle.Value < hle2.Value)
               {
                  HateListEntry tmp = hle;
                  hle = hle2;
                  hle2 = tmp;

                  bChanged = true;
               }
            } // for

            if (!bChanged)
               return;
         } // for
      } // SortHateList()

      /// <summary>
      /// Sets the hate value or adds to the current hate value of the given target entity.
      /// If the entity is not on the list, it will be added to it. If the list is full, the entity
      /// with the lowest value is dropped from the list (even if the dropped entity had a higher hate
      /// than this entity)
      /// </summary>
      /// <param name="Target">The entity on the hate list to modify.</param>
      /// <param name="Value">The hate value to add or set.</param>
      /// <param name="Flags">Flags defining custom behaviour of the method.</param>
      internal void SetHateValue(Entity Target, int Value, HateListParam Flags)
      {
         int idx = IndexOfTarget(Target);
         if (idx == -1)
         {
            int newValue = Value;
            if ((Flags & HateListParam.PushToTop) > 0)
               newValue += entries[0].Value;

            idx = GetNextFreeHateListIndex();
            if (idx == -1)
            {
               entries[9].Target = Target;
               entries[9].Value = newValue;
            }
            else
            {
               entries[idx].Target = Target;
               entries[idx].Value = newValue;
            }
         } // if
         else
         {
            if ((Flags & HateListParam.IgnoreIfPresent) == 0)
            {
               if ((Flags & HateListParam.AddValue) > 0)
                  entries[idx].Value += Value;
               else
                  entries[idx].Value = Value;
            }
         } // else

         SortHateList();
      } // SetHateValue(Target, Value, Flags)

      /// <summary>
      /// Returns the highest entry in the hate list.
      /// </summary>
      /// <returns>The highest hate list entry.</returns>
      internal HateListEntry GetHighestHateListEntry()
      {
         return entries[0];
      } // GetHighestHateListEntry()
   }
}

