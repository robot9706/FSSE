using FSLoader;
using System.Collections.Generic;

namespace ModPack
{
    [ModInfo("higher_dweller_level_cap", "Higher dweller levels", "Robot9706", 1, 0, "Gives an option to change the maximum dweller level. Requires additional configuration.")]
    public class ModDwellerLevelCap : Mod
    {
        private List<LevelInformation> _additionalInfo;
        private int _autoGenerateLevels;

        public override void OnInit()
        {
            _additionalInfo = new List<LevelInformation>();

            ConfigSection config = GetModConfig();

            config.GetString("note", "If the generate feature is enabled the levels data won't be used. The auto generate feature generates new levels based on the new cap, each new level requires +129600 more xp than the previous one.");

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

                        _additionalInfo.Add(new LevelInformation(0, level.GetValue("min_xp", 0.0f), level.GetValue("caps_reward", 0)));
                    }
                }
            }
        }

        [Hook("DwellerExperience::.ctor(Dweller)")]
        public void Hook_DwellerXP(CallContext context, Dweller dweller)
        {
            if (_additionalInfo.Count == 0 && _autoGenerateLevels == -1)
                return;

            LevelsInformation info = MonoSingleton<LevelTables>.Instance.Character;
            if (info.M_maxLevel <= 50)
            {
                List<LevelInformation> infoList = new List<LevelInformation>(info.M_levelInformation);
                {
                    if (_autoGenerateLevels != -1)
                    {
                        float xp = info.M_levelInformation[info.M_levelInformation.Count - 1].MinimumExp;

                        for (int x = 0; x < _autoGenerateLevels; x++)
                        {
                            xp += 129600.0f;

                            _additionalInfo.Add(new LevelInformation(51 + x, xp, 51 + x));
                        }
                    }
                    else
                    {
                        for (int x = 0; x < _additionalInfo.Count; x++)
                        {
                            _additionalInfo[x].M_levelNumber = info.M_maxLevel + 1 + x;
                        }
                    }

                    infoList.AddRange(_additionalInfo);
                }
                info.M_levelInformation = infoList;
                info.M_keyLevels = infoList.ToArray();
                info.M_maxLevel = infoList.Count;
            }
        }
    }
}
