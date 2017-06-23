using FSLoader;
using System.Collections.Generic;
using System.IO;

namespace ModPack
{
    [ModInfo("higher_dweller_level_cap", "Higher dweller levels", "Robot9706", 1, 0, "Gives an option to change the maximum dweller level. Requires additional configuration.",
@"The config has the following parameters:
generate: True (enabled) or False (disabled), if enabled the new level cap will be 'generate_cap'. The new levels will require +129600 experience compared to the level before.
generate_cap: The new generated level cap.
levels: If 'generate' is disabled this array contains the custom defined new levels. Each item is a level, the first item is the 51th level, the second element is the 52th level and so on.
An entry looks like the following (each entry need to be separated by ','):
{
""min_xp"": 2918000,
""add"": 5000,
""multiply"":1.1,
""caps_reward"": 51,
}

Only one can be used from one of these 3, if multiple parameters are defined they will overwrite each other.
min_xp is the minimum required experience for the level (level 50 is 2916000).
add is the experience added to the last level to calculate the min_xp for this level.
multiply is a multiplier to calculate the experience for this level based on the last level experience.

caps_reward is the amount of caps rewarded on levelup.
")]
    public class ModDwellerLevelCap : Mod
    {
        class LevelInfo
        {
            public int MinXP;
            public int Add;
            public float Multiply;
            public int Caps;
        }

        private List<LevelInfo> _levelInfo;
        private int _autoGenerateLevels;

        private bool _levelsAdded = false;

        public override void OnInit()
        {
            _levelInfo = new List<LevelInfo>();

            ConfigSection config = GetModConfig();
            int newCap = config.GetValue("generate_cap", 50);
            if (config.GetValue<bool>("generate"))
            {
                if (newCap > 50)
                {
                    _autoGenerateLevels = newCap - 50;
                }
            }
            else
            {
                ConfigArray levels = config.GetSectionArray("levels");
                if (levels.Length > 0)
                {
                    _autoGenerateLevels = -1;

                    for (int x = 0; x < levels.Length; x++)
                    {
                        ConfigSection level = levels[x];

                        _levelInfo.Add(new LevelInfo()
                        {
                            MinXP = level.Has("min_xp") ? level.GetValue<int>("min_xp") : -1,
                            Add = level.Has("add") ? level.GetValue<int>("add") : -1,
                            Multiply = level.Has("multiply") ? level.GetValue<float>("multiply") : -1.0f,
                            Caps = level.GetValue<int>("caps_reward")
                        });
                    }
                }
            }
        }

        [Hook("DwellerExperience::.ctor(Dweller)")]
        public void Hook_DwellerXP(CallContext context, Dweller dweller)
        {
            if (!_levelsAdded)
            {
                LevelsInformation info = MonoSingleton<LevelTables>.Instance.Character;
                List<LevelInformation> infoList = new List<LevelInformation>(info.M_levelInformation);

                float xp = info.M_levelInformation[info.M_levelInformation.Count - 1].MinimumExp;

                if (_autoGenerateLevels != -1)
                {
                    for (int x = 0; x < _autoGenerateLevels; x++)
                    {
                        xp += 129600.0f;

                        infoList.Add(new LevelInformation(51 + x, xp, 51 + x));
                    }
                }
                else
                {
                    for (int x = 0; x < _levelInfo.Count; x++)
                    {
                        LevelInfo inf = _levelInfo[x];

                        if (inf.MinXP != -1)
                            xp = inf.MinXP;
                        else if (inf.Add != -1)
                            xp += inf.Add;
                        else if (inf.Multiply != -1)
                            xp *= inf.Multiply;

                        infoList.Add(new LevelInformation(x + 51, xp, inf.Caps));
                    }
                }

                using (StreamWriter sw = new StreamWriter(Path.Combine(FSPaths.ModsFolder, "levels.txt")))
                {
                    foreach (LevelInformation inf in infoList)
                    {
                        sw.WriteLine(inf.LevelNumber.ToString() + " - " + inf.MinimumExp.ToString() + " - " + inf.CapsReward.ToString());
                    }
                }

                info.M_levelInformation = infoList;
                info.M_keyLevels = infoList.ToArray();
                info.M_maxLevel = infoList.Count;

                _levelsAdded = true;
            }
        }
    }
}
