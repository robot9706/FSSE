using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_pet_count_caps", "Remove pet count caps", "Robot9706", 1, 0)]
    public class ModRemovePetCountCaps : Mod
    {
        [Hook("Room::CanDwellerWithPetBeAssigned()")]
        public void Hook_CanDwellerWithPetBeAssigned(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }

        [Hook("Vault::get_MaximumPetCount()")]
        public void Hook_get_MaximumPetCount(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = Int32.MaxValue;
        }

        [Hook("Vault::VaultIsAtPetCapacity()")]
        public void Hook_VaultIsAtPetCapacity(CallingContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = false;
        }
    }
}
