using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarGame.Data.Game.Definitions;
using WinWarGame.Data.Resources;
using WinWarGame.Util;

namespace WinWarGame.Data.Game
{
    class Campaign
    {
        public int CurrentLevelIndex { get; private set; }

        public int MaxLevels => CampaignDefinition.Levels.Length;

        internal Race Race { get; private set; }

        public CampaignDefinition CampaignDefinition { get; set; } = new CampaignDefinition();
        
        internal Campaign(Race setRace)
        {
            Race = setRace;
        }

        public bool LoadDefinitions(string definitionFile)
        {
            try
            {
                string fn = Path.Combine(MainGame.AssetProvider.AssetsDirectory, definitionFile);
                CampaignDefinition = System.Text.Json.JsonSerializer.Deserialize<CampaignDefinition>(File.ReadAllText(fn));
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return false;
            }
        }

        internal void StartNew()
        {
            CurrentLevelIndex = 0;
        }

        internal void AdvanceNextLevel()
        {
            CurrentLevelIndex++;

            if (CurrentLevelIndex >= MaxLevels)
            {
                // TODO: End campaign
            }
        }

        public Map LoadCurrentMap()
        {
            string basename = GetCurrentLevelName();
            
            LevelInfoResource levelInfo = WarFile.GetResourceByName(basename) as LevelInfoResource;
            LevelPassableResource levelPassable =
                WarFile.GetResource(levelInfo.PassableResourceIndex) as LevelPassableResource;
            LevelVisualResource levelVisual = WarFile.GetResource(levelInfo.VisualResourceIndex) as LevelVisualResource;
            
            Map res = new Map(basename, levelInfo, levelVisual, levelPassable);
            return res;
        }

        public string GetCurrentLevelName()
        {
            return Race + " " + (CurrentLevelIndex + 1);
        }

        public static string GetDefaultCampaignFilename(Race race, bool isDemo)
        {
            return (isDemo ? "demo_" : "") + "campaign_" + race.ToString().ToLowerInvariant() + ".json";
        }
    }
}