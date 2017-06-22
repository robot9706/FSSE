using FSLoader;

namespace ModPack
{
    [ModInfo("instant_quest_travel", "Instant quest travel", "Robot9706", 1, 0, "Quest teams travel instantly.")]
    public class ModInstantQuestTravel : Mod
    {
        [Hook("QuestEquipmentWindow::CreateWastelandTeam()")]
        public void Hook_CreateTeam(CallContext context)
        {
            QuestEquipmentWindow window = (QuestEquipmentWindow)context.This;

            QuestParameters.QuestSetupInformationInterface info = (QuestParameters.QuestSetupInformationInterface)window.M_questInfo;
            if(info is QuestParameters.QuestSetupInformation)
            {
                ((QuestParameters.QuestSetupInformation)info).M_timeToReach = 0;
            }
            else if(info is QuestParameters.ThumbNailQuestSetupInformation)
            {
                ((QuestParameters.ThumbNailQuestSetupInformation)info).m_timeToReach = 0;
            }
        }

        [Hook("WastelandTeam::SetupReturnTimeAndDuration(System.Int32)")]
        public void Hook_SetGoingTime(CallContext context, int time)
        {
            WastelandTeam team = (WastelandTeam)context.This;

            if (!team.IsQuest())
            {
                return;
            }

            context.IsHandled = true;

            ModInstantWastelandReturn.InstantTeamTravel(team);
        }
    }
}
