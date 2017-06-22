using FSLoader;
using System;
using System.Collections.Generic;

namespace ModPack
{
    [ModInfo("remove_vault_storage_limits", "Remove resources limit", "Robot9706", 1, 0, "Infinite storage count which doesn't depend on storage rooms.")]
    public class ModNoStorageLimits : Mod
    {
        //Stores which resources are "unlocked"
        private List<EResource> _overwriteLimits = new List<EResource>();

        private bool _itemStorage;

        public override void OnInit()
        {
            ConfigSection config = GetModConfig(); //Load the config section and do some basic setup
            if (config.GetValue<bool>("remove_caps_limit"))
            {
                _overwriteLimits.Add(EResource.Nuka);
            }
            if (config.GetValue<bool>("remove_quantum_limit"))
            {
                _overwriteLimits.Add(EResource.NukaColaQuantum);
            }
            if (config.GetValue<bool>("remove_stim_rad_limit"))
            {
                _overwriteLimits.Add(EResource.StimPack);
                _overwriteLimits.Add(EResource.RadAway);
            }

            _itemStorage = config.GetValue<bool>("remove_item_limit");
        }

        [Hook("Inventory::SetMaxItems(System.Int32)")]
        public void Hook_SetMaxItems(CallContext context, int count)
        {
            if (!_itemStorage)
                return;

            Inventory inv = (Inventory)context.This;
            if (inv is VaultInventory)
            {
                context.IsHandled = true;

                inv.M_itemCountMax = Int32.MaxValue / 2; //Int32 max seems to break things, but half semms to work fine
            }
        }

        [Hook("FSLOADER::VaultStorage.SetMaxResources(Storage,EResource,System.Single)")]
        public void Hook_SetMaxResources(CallContext context, Storage storage, EResource resource, float oldMax)
        {
            if (_overwriteLimits.Contains(resource))
            {
                context.IsHandled = true;
                context.ReturnValue = Int32.MaxValue / 2;
            }
        }
    }
}
