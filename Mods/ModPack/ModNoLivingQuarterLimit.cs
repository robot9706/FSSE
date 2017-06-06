using FSLoader;

namespace ModPack
{
    [ModInfo("no_vault_dweller_limit", "No living quarter limits", "Robot9706", 1, 0)]
    public class ModNoLivingQuarterLimit : Mod
    {
        [Hook("Vault::get_MaxDwellers()")]
        public void Hook_get_MaxDwellers(CallContext context)
        {
            Vault vault = (Vault)context.This;

            context.IsHandled = true;
            context.ReturnValue = vault.M_dwellers.Count + 1;
        }

        [Hook("Vault::CanAddDwellers(System.Int32)")]
        public void Hook_CanAddDwellers(CallContext context, int count)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }

        [Hook("Vault::get_ClampedMaxDwellers()")]
        public void Hook_get_ClampedMaxDwellers(CallContext context)
        {
            Vault vault = (Vault)context.This;

            context.IsHandled = true;
            context.ReturnValue = vault.MaxDwellers;
        }

        [Hook("LivingQuartersRoom::CanBeDestroyed()")]
        public void Hook_CanBeDestroyed(CallContext context)
        {
            LivingQuartersRoom room = (LivingQuartersRoom)context.This;

            context.IsHandled = true;
            context.ReturnValue = MonoSingleton<ConstructionMgr>.Instance.CanDestroyRoom(room);
        }
    }
}
