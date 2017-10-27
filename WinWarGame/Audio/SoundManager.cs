using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using WinWarCS.Data;
using WinWarCS.Data.Resources;

namespace WinWarCS.Audio
{
   public class SoundManager
   {
      private Dictionary<int, SoundEffect> effectCache;

      public SoundManager()
      {
         effectCache = new Dictionary<int, SoundEffect>();
      }

      public bool LoadSound(int resIndex)
      {
         if (effectCache.ContainsKey(resIndex) || WarFile.AreResoucesLoaded == false)
         {
            return false;
         }

         RawResource res = WarFile.GetResource(resIndex) as RawResource;
         if (res == null || 
            res.Type != ContentFileType.FileWave)
         {
            return false;
         }

         MemoryStream memStream = new MemoryStream(res.Resource.data);
         SoundEffect eff = SoundEffect.FromStream(memStream);
         effectCache.Add(resIndex, eff);
         memStream.Dispose();
         memStream = null;

         return true;
      }

      public void PlaySound(int resIndex)
      {
         if (effectCache.ContainsKey(resIndex) == false)
         {
            if (LoadSound(resIndex) == false)
            {
               return;
            }
         }

         SoundEffect eff = effectCache[resIndex];
         eff.CreateInstance().;
         eff.Play();
      }
   }
}
