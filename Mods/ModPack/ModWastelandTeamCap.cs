using FSLoader;

namespace ModPack
{
    [ModInfo("remove_quest_team_limit", "Remove wasteland explorer team limit", "Robot9706", 1, 0)]
    public class ModWastelandTeamCap : Mod
    {
        [Hook("Wasteland::IsQuestTeamLimitReached()")]
        public void Hook_IsQuestTeamLimitReached(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = false;
        }
    }
}
