using FSLoader;

namespace ModPack
{
    [ModInfo("instant_wasteland_quests", "Instant wasteland quests", "Robot9706", 1, 0, "Exploring dwellers find a random quest as soon as they leave the vault.")]
    public class ModInstantWastelandQuests : Mod
    {
        [Hook("Wasteland::StartSurpriseQuestCheckProcessIfAble()")]
        public void Hook_StartSurpriseQuestCheckProcessIfAble(CallContext context)
        {
            Wasteland wasteland = (Wasteland)context.This;

            wasteland.M_forceSurpriseQuestCheck = true;
        }
    }
}
