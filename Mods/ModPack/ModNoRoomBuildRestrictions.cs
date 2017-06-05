using FSLoader;

namespace ModPack
{
    [ModInfo("no_room_build_restrictions", "No room build restrictions", "Robot9706", 1, 0)]
    public class ModNoRoomBuildRestrictions : Mod
    {
        [Hook("UIRoomBuildListItem::SetNotAvailable(Objective)")]
        public void Hook_SetNotAvailable(CallingContext context, Objective obj)
        {
            context.IsHandled = true;

            UIRoomBuildListItem item = (UIRoomBuildListItem)context.This;
            item.SetAvailable();
        }
    }
}
