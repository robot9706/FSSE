using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_200_dweller_cap", "Remove 200 dweller cap", "Robot9706", 1, 0, "Removes the 200 dweller cap, however this can impact performance.")]
    public class ModRemove200DwellerCap : Mod
    {
        [Hook("DwellerManager::get_MaximumDwellerCount()")]
        public void Hook_get_MaximumDwellerCount(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = Int32.MaxValue;
        }

        [Hook("DwellerManager::get_VaultIsWithMaxPopulation()")]
        public void Hook_get_VaultIsWithMaxPopulation(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = false;
        }
    }
}
