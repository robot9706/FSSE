using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_pet_count_caps", "Remove pet count caps", "Robot9706", 1, 0, "Removes pet limitations (equip every dweller with pets in the same room).")]
    public class ModRemovePetCountCaps : Mod
    {
        [Hook("Room::CanDwellerWithPetBeAssigned()")]
        public void Hook_CanDwellerWithPetBeAssigned(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }

        [Hook("Vault::get_MaximumPetCount()")]
        public void Hook_get_MaximumPetCount(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = Int32.MaxValue;
        }

        [Hook("Vault::VaultIsAtPetCapacity()")]
        public void Hook_VaultIsAtPetCapacity(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = false;
        }
    }
}
