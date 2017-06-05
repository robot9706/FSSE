using FSLoader;

namespace ModPack
{
    [ModInfo("skip_more_objectives_per_day", "Skip more than 1 objective per day", "Robot9706", 1, 0)]
    public class ModSkipObjectives : Mod
    {
        [Hook("ObjectiveMgr::ReserveObjectiveChange()")]
        public void Hook_ReserveObjectiveChange(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }

        [Hook("ObjectiveMgr::get_CanChangeObjective()")]
        public void Hook_get_CanChangeObjective(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }
    }
}
