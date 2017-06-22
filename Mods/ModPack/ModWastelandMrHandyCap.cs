using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_wasteland_mrhandy_cap", "Remove wasteland MrHandy cap", "Robot9706", 1, 0, "Send as many MrHandies to the Wasteland as you want.")]
    public class ModWastelandMrHandyCap : Mod
    {
        [Hook("Wasteland::get_MaxMrHandy()"), Hook("Wasteland::get_CanSendMrHandy()")] //We need to overwrite the value when either of these methods are called
        public void Hook_GetMaxMrHandy(CallContext context)
        {
            Wasteland wasteland = (Wasteland)context.This;

            if (wasteland.M_maxMrHandy != Int32.MaxValue)
            {
                wasteland.M_maxMrHandy = Int32.MaxValue; //Change the value of the private field and let FalloutShelter do it's thing
            }
        }
    }
}
