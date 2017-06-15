using FSLoader;

namespace ModPack
{
    [ModInfo("instant_wasteland_return", "Instant wasteland exporer return", "Robot9706", 1, 0)]
    public class ModInstantWastelandReturn : Mod
    {
        [Hook("WastelandTeam::SetupReturnTimeAndDuration(System.Int32)")]
        public void Hook_SetGoingTime(CallContext context, int time)
        {
            WastelandTeam team = (WastelandTeam)context.This;

            if(team.IsQuest())
            {
                return;
            }

            context.IsHandled = true;

            InstantTeamTravel(team);
        }

        public static void InstantTeamTravel(WastelandTeam team)
        {
            int goingTime = 1;

            team.ElapsedReturningTime = 0;
            team.M_returnTripDuration = new TimeUnit((goingTime) / team.M_returnTimeReduction);
            float bonus = team.GetBonusForEffectMax(EBonusEffect.FasterWastelandReturnSpeed);
            if (bonus > 0f)
            {
                team.M_returnTripDuration = team.M_returnTripDuration / (bonus);
            }
        }
    }
}
