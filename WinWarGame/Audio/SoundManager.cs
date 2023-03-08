using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using WinWarGame.Data;
using WinWarGame.Data.Resources;

namespace WinWarGame.Audio
{
   public class SoundManager
   {
      private Dictionary<int, SoundEffect> effectCache;

      private List<SoundEffectInstance> activeSoundEffects;

      /// <summary>
      /// Defines whether playback is enabled. Loading is possible regardles
      /// </summary>
      public bool IsEnabled { get; set; }

      public SoundManager()
      {
         IsEnabled = true;
         effectCache = new Dictionary<int, SoundEffect>();
         activeSoundEffects = new List<SoundEffectInstance>();
      }

      internal void Update(GameTime gameTime)
      {
         lock (activeSoundEffects)
         {
            for (int i = activeSoundEffects.Count - 1; i >= 0; i--)
            {
               SoundEffectInstance inst = activeSoundEffects[i];
               if (inst.State == SoundState.Stopped)
               {
                  activeSoundEffects.RemoveAt(i);
                  inst.Dispose();
               }
            }
         }
      }

      public void StopAll()
      {
         lock (activeSoundEffects)
         {
            for (int i = 0; i < activeSoundEffects.Count; i++)
            {
               activeSoundEffects[i].Stop(true);
            }
         }
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

         using (MemoryStream memStream = new MemoryStream(res.Resource.data))
         {
            SoundEffect eff = SoundEffect.FromStream(memStream);
            effectCache.Add(resIndex, eff);
         }

         return true;
      }

      public void PlaySound(int resIndex, bool stopOthers = false)
      {
         if (IsEnabled == false)
         {
            return;
         }

         if (effectCache.ContainsKey(resIndex) == false)
         {
            if (LoadSound(resIndex) == false)
            {
               return;
            }
         }

         if (stopOthers)
         {
            StopAll();
         }

         SoundEffect eff = effectCache[resIndex];
         var inst = eff.CreateInstance();
         lock (activeSoundEffects)
         {
            activeSoundEffects.Add(inst);
         }
         inst.Play();
      }
   }
}
