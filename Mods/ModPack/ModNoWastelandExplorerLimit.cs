using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_wasteland_dweller_limit", "No wasteland explorer limit", "Robot9706", 1, 0)]
    public class ModNoWastelandExplorerLimit : Mod
    {
        [Hook("WastelandParameters::get_DwellerLimit()")]
        public void Hook_get_DwellerLimit(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = Int32.MaxValue;
        }
    }
}
