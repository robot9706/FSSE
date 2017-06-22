using FSLoader;

namespace ModPack
{
    [ModInfo("destroy_any_room", "No destroy restrcitions", "Robot9706", 1, 0, "Destroy any kind of room anywhere.")]
    public class ModDestroyEverything : Mod
    {
        [Hook("ConstructionMgr::CanDestroyRoom(Room)")]
        public void Hook_CanDestroyRoom(CallContext context, Room room)
        {
            context.IsHandled = true; //The event is handled (so no FalloutShelter code will be executed)
            context.ReturnValue = true; //The hooked method should return "true"
        }
    }
}
