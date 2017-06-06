using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_wasteland_stimpacks_radaways_cap", "Remove max stimpacks and radaways per exploring dweller", "Robot9706", 1, 0)]
    public class ModWastelandStimRadCap : Mod
    {
        [Hook("WastelandParameters::get_MaxStimpackPerDweller()")]
        public void Hook_get_MaxStimpackPerDweller(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = Int32.MaxValue;
        }

        [Hook("WastelandParameters::get_MaxRadawayPerDweller()")]
        public void Hook_get_MaxRadawayPerDweller(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = Int32.MaxValue;
        }

        [Hook("WastelandEquipmentWindow::MaxStimpak()")]
        public void Hook_MaxStimpak(CallContext context)
        {
            context.IsHandled = true;

            AddResource((WastelandEquipmentWindow)context.This, EResource.StimPack, 25);
        }

        [Hook("WastelandEquipmentWindow::MaxRadaway()")]
        public void Hook_MaxRadaway(CallContext context)
        {
            context.IsHandled = true;

            AddResource((WastelandEquipmentWindow)context.This, EResource.RadAway, 25);
        }

        //Adds a specific amount of resource to the team inventory
        private void AddResource(WastelandEquipmentWindow window, EResource resource, int count)
        {
            if (MonoSingleton<Vault>.Instance.Storage.HasResources(new GameResources(resource, count + window.TeamResources.Resources.GetResource(resource))))
            {
                window.TeamResources.AddResource(new GameResources(resource, count), true, true);
            }
            else
            {
                window.TeamResources.AddResource(new GameResources(resource, MonoSingleton<Vault>.Instance.Storage.Resources.GetResource(resource) - window.TeamResources.Resources.GetResource(resource)), true, true);
            }
            window.UpdateItemData();
        }
    }
}
