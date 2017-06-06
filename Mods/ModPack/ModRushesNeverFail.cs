using FSLoader;

namespace ModPack
{
    [ModInfo("rush_never_fails", "Rushes never fail", "Robot9706", 1, 0)]
    public class ModRushesNeverFail : Mod
    {
        [Hook("ProductionRoom::StartRush(System.Boolean)")]
        public void Hook_StartRush(CallContext context, bool silent)
        {
            context.IsHandled = true; //Don't execute FalloutShelter code

            ProductionRoom room = (ProductionRoom)context.This;
            room.M_rushingState.SetSilentRush(silent);
            room.M_rushingState.SetAlwaysSuccess(true);
            room.ChangeRoomState(room.M_rushingState);
        }
    }
}
