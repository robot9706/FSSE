using FSLoader;

namespace ModPack
{
    [ModInfo("disable_rats", "Disable rats", "Robot9706", 1, 0)]
    public class ModDisableRats : Mod
    {
        [Hook("RatsManager::IsRatForbiddenRoom(ERoomType)")]
        public void Hook_Rats(CallContext context, ERoomType room)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }
    }
}
