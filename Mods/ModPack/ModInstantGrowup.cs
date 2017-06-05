using FSLoader;

namespace ModPack
{
    [ModInfo("instant_growup", "Instant child group", "Robot9706", 1, 0)]
    public class ModInstantGrowup : Mod
    {
        [Hook("DwellerParameters::get_ChildhoodDuration()")]
        public void Hook_ChildhoodDuration(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = 0.01f;

            DwellerParameters param = (DwellerParameters)context.This;
            param.M_childhoodDuration = (float)context.ReturnValue;
        }

        [Hook("DwellerParameters::get_RelationshipBabyBirthTime()")]
        public void Hook_RelationshipBabyBirthTime(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = 0.01f;

            DwellerParameters param = (DwellerParameters)context.This;
            param.M_relationshipBabyBirthTime = (float)context.ReturnValue;
        }
    }
}
