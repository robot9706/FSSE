using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_wasteland_mrhandy_caps_limit", "Remove MrHandy cap limit.", "Robot9706", 1, 0, "MrHandies can collect infinite amount of caps.")]
    public class ModNoMrHandyCapLimit : Mod
    {
        [Hook("WastelandParameters::get_MaxCapsPerMrHandy()")]
        public void Hook_MaxCapsPerMrHandy(CallContext context)
        {
            context.IsHandled = true;
			context.ReturnValue = Int32.MaxValue;
		}
    }
}
