using FSLoader;

namespace ModPack
{
    [ModInfo("instant_breeding", "Instant breeding", "Robot9706", 1, 0)]
    public class ModInstantBreeding : Mod
    {
        [Hook("DwellerParameters::get_BreedingCycleTime()")]
        public void Hook_BreedingCycleTime(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = 0.1f;

            DwellerParameters param = (DwellerParameters)context.This;
            param.M_breedingCycleTime = (float)context.ReturnValue;
        }
    }
}
