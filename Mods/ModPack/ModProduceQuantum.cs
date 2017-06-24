using FSLoader;
using System.Collections.Generic;

namespace ModPack
{
    [ModInfo("produce_quantum", "Produce quantum", "Robot9706", 1, 0, "Nuka factories produce quantum.", "One cycle takes 30 minutes, which can be reduced by 15 minutes with higher Endurance levels. The nuka production is based on the room upgrade level and the merge level. Each upgrade level adds +1 quantum and production is multiplied by the merge level.")]
    public class ModProduceQuantum : Mod
    {
        [Hook("ProductionRoom::GetProducedResources()")]
        public void Hook_GetProducedResources(CallContext context)
        {
            ProductionRoom room = (ProductionRoom)context.This;

            if (room.RoomType != ERoomType.NukaCola)
                return;

            if (room.ResourcesNotConsideredForCycleToBeCompleted.Contains(EResource.NukaColaQuantum))
                room.ResourcesNotConsideredForCycleToBeCompleted.Remove(EResource.NukaColaQuantum);

            int merge = room.MergeLevel;

            for (int x = 0; x < room.m_currentRoomLevelController.m_roomLevels.Length; x++)
            {
                RoomLevel lvl = room.m_currentRoomLevelController.m_roomLevels[x];
                if(lvl is ProductionLevel)
                {
                    int production = (x + 1) * merge;

                    ProductionLevel plvl = (ProductionLevel)lvl;
                    if(plvl.m_resourcesReserve.NukeColaQuantum < production)
                    {
                        plvl.m_resourcesReserve.Clear();
                        plvl.m_resourcesReserve.NukeColaQuantum = production;

                        plvl.m_resourcesProduced.Clear();
                        plvl.m_resourcesProduced.NukeColaQuantum = production;       
                    }
                }
            }

            int maxDwellers = ((ProductionLevel)room.CurrentLevelStats).m_maxDwellerCount;

            float endurance = 0;
            foreach(Dweller d in room.Dwellers)
            {
                endurance += d.Stats.GetEffectiveStat(ESpecialStat.Endurance);
            }

            float max = maxDwellers * 10.0f;
            if (endurance > max)
                endurance = 1.0f;
            else
                endurance /= max;
            endurance = 1.0f - endurance;

            GameResourcesBuilder builder = room.M_resourcesProduced;
            builder.Initialize();

            builder.Add(((ProductionLevel)room.CurrentLevelStats).m_resourcesProduced);
            builder.Divide(600.0f * (15.0f + 15.0f * endurance));

            context.IsHandled = true;
            context.ReturnValue = builder;
        }

        [Hook("ResourceParameters::GetResourceData(EResource)")]
        public void Hook_ResourceData(CallContext context, EResource res)
        {
            ResourceParameters param = (ResourceParameters)context.This;

            context.IsHandled = true;

            if (!param.M_resourceToBinary.ContainsKey(res))
            {
                context.ReturnValue = param.M_resourcesIcons[param.M_resourceToBinary[EResource.Nuka]]; //Dummy
            }

            int num = param.M_resourceToBinary[res];
            if(!param.M_resourcesIcons.ContainsKey(num))
            {
                context.ReturnValue = param.M_resourcesIcons[param.M_resourceToBinary[EResource.Nuka]]; //Dummy
            }

            context.ReturnValue = param.M_resourcesIcons[num];
        }

        [Hook("ResourceParameters::GetResourceData(System.Collections.Generic.List`1<EResource>)")]
        public void Hook_ResourcesData(CallContext context, List<EResource> res)
        {
            ResourceParameters param = (ResourceParameters)context.This;

            int num = 0;

            foreach (EResource r in res)
            {
                if (!param.M_resourceToBinary.ContainsKey(r))
                    continue;

                int tmp = param.M_resourceToBinary[r];
                if (!param.M_resourcesIcons.ContainsKey(tmp))
                    continue;

                int nextNum = num | tmp;
                if (param.M_resourcesIcons.ContainsKey(nextNum))
                    num = nextNum;
            }

            context.IsHandled = true;
            context.ReturnValue = param.M_resourcesIcons[num];
        }
    }
}
