using FSLoader;

namespace ModPack
{
    [ModInfo("no_random_incidents", "No random incidents", "Robot9706", 1, 0, "No random vault incidents.")]
    public class ModNoRandomIncidents : Mod
    {
        [Hook("VaultEmergencyState::CheckRandomEmergency(System.Boolean)")]
        public void Hook_Check(CallContext context, bool online)
        {
            context.IsHandled = true;
        }
    }
}
