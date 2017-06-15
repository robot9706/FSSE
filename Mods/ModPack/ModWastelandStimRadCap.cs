using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("remove_wasteland_stimpacks_radaways_cap", "Remove max stimpack and radaway caps", "Robot9706", 1, 0)]
    public class ModWastelandStimRadCap : Mod
    {
        #region Global
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
        #endregion

        #region Explorers
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
        #endregion

        #region Quest teams
        [Hook("QuestEquipmentWindow::MaxStimpak()")]
        public void Hook_MaxQuestStimpack(CallContext context)
        {
            context.IsHandled = true;

            AddResource((QuestEquipmentWindow)context.This, EResource.StimPack, 25);
        }

        [Hook("QuestEquipmentWindow::MaxRadaway()")]
        public void Hook_MaxQuestRadaway(CallContext context)
        {
            context.IsHandled = true;

            AddResource((QuestEquipmentWindow)context.This, EResource.RadAway, 25);
        }

        private void AddResource(QuestEquipmentWindow window, EResource resource, int count)
        {
            if(MonoSingleton<Vault>.Instance.Storage.HasResources(new GameResources(resource, count + window.M_teamResources.Resources.GetResource(resource))))
            {
                window.M_teamResources.AddResource(new GameResources(resource, count), true, true);
            }
            else
            {
                window.M_teamResources.AddResource(new GameResources(resource, MonoSingleton<Vault>.Instance.Storage.Resources.GetResource(resource) - window.M_teamResources.Resources.GetResource(resource)), true, true);
            }
            window.RefreshResourceDisplay();
        }
        #endregion
    }
}
