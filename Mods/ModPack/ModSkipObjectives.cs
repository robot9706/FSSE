using FSLoader;

namespace ModPack
{
    [ModInfo("skip_more_objectives_per_day", "Skip objectives", "Robot9706", 1, 0, "Skip more than 1 objective per day.")]
    public class ModSkipObjectives : Mod
    {
        [Hook("ObjectiveMgr::ReserveObjectiveChange()")]
        public void Hook_ReserveObjectiveChange(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }

        [Hook("ObjectiveMgr::get_CanChangeObjective()")]
        public void Hook_get_CanChangeObjective(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }
    }
}
