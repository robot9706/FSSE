using FSLoader;

namespace ModPack
{
    [ModInfo("no_room_build_restrictions", "No room build restrictions", "Robot9706", 1, 0, "Every room is unlocked by default.")]
    public class ModNoRoomBuildRestrictions : Mod
    {
        [Hook("UnlockablesMgr::GetNumCompletedObjectives()")]
        public void Hook_CompletedObjectives(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = 0;
        }

        [Hook("UIRoomBuildListItem::SetNotAvailable(Objective)")]
        public void Hook_SetNotAvailable(CallContext context, Objective obj)
        {
            context.IsHandled = true;

            UIRoomBuildListItem item = (UIRoomBuildListItem)context.This;
            item.SetAvailable();
        }
    }
}
