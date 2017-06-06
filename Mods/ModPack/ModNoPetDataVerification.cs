using FSLoader;

namespace ModPack
{
    [ModInfo("allow_pet_bonus_modification", "Allow pet bonus modification", "Robot9706", 1, 0)]
    public class ModNoPetDataVerification : Mod
    {
        [Hook("DwellerItem::VerifyPetData(PetUniqueData)")]
        public void Hook_VerifyPetData(CallContext context, PetUniqueData data)
        {
            context.IsHandled = true; //No FalloutShelter code will be executed, the hooked method is a "Void", so no return value is required
        }
    }
}
