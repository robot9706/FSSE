using FSLoader;

namespace ModPack
{
    [ModInfo("no_breeding_restrictions", "No breeding restrictions", "Robot9706", 1, 0, "Family relations doesn't matter when dwellers are finding partners.")]
    public class ModNoBreedingRestrictions : Mod
    {
        [Hook("DwellerRelations::CheckFamilyRelation(Dweller,Dweller)")]
        public void Hook_CheckFamilyRelation(CallContext context, Dweller a, Dweller b)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }
    }
}
