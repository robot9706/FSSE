using FSLoader;

namespace ModPack
{
    [ModInfo("instant_crafting", "Instant crafting", "Robot9706", 1, 0, "Crafting rooms craft instantly.")]
    public class ModInstantCrafting : Mod
    {
        [Hook("CraftingRoom::GetActualWorkTime(ESpecialStat,System.Single,System.Single)")]
        public void Hook_GetCraftTime(CallContext context, ESpecialStat stat, float min, float max)
        {
            context.IsHandled = true;
            context.ReturnValue = 1.0f;
        }
    }
}
