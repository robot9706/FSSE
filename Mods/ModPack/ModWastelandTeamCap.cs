using FSLoader;

namespace ModPack
{
    [ModInfo("remove_quest_team_limit", "No explorer limits", "Robot9706", 1, 0, "Send as many dwellers to the wasteland as you like.")]
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
