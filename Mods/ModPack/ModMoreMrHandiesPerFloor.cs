using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_1_mr_handy_cap", "Allow more MrHandies per floor", "Robot9706", 1, 0, "Multiple MrHandies are allowed on the same floor.")]
    public class ModMoreMrHandiesPerFloor : Mod //Best class name ever
    {
        [Hook("DwellerParameters::get_MaxMrHandyPerSector()")]
        public void Hook_get_MaxMrHandyPerSector(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = Int32.MaxValue;
        }
    }
}
