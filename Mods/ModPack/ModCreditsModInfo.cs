using FSLoader;
using System.Text;

namespace ModPack
{
    [ModInfo(null, "Credits mod info", "Robot9706", 1, 0)] //A "null" ID means it's a persistent mod, it's always enabled
    public class ModCreditsModInfo : Mod
    {
        [Hook("UILabel::OnStart()")]
        public void Hook_UILabel_OnStart(CallingContext context)
        {
            UILabel label = (UILabel)context.This;

            context.IsHandled = false; //Keep executing FalloutShelter code

            //Okay this is a very poor solution, need to find a better one
            string text = label.MText.ToLower();
            if(text.Contains("todd") && text.Contains("howard")) //Search for the name of our lord and savior
            {
                //This label is the credits text label (probably)
                label.MText = GetModifiedCreditsText(label.MText); //Add the modding info
            }
        }

        private static string GetModifiedCreditsText(string original)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[FF0000]Modded Fallout Shelter");

            sb.AppendLine();
            sb.Append("[212F21]Config file: ");
            if (FSHooks.Config.LoadedFromFile)
            {
                sb.AppendLine("[FF0000] Not found: \"" + FSHooks.Config.FilePath + "\"");
            }
            else
            {
                sb.AppendLine("[00AA00] Loaded.");
            }

            sb.AppendLine();

            sb.Append("[212F21]Loaded mods: x" + FSHooks.LoadedMods.Count.ToString());
            sb.AppendLine();
            foreach (Mod mod in FSHooks.LoadedMods)
            {
                sb.AppendLine("[212F21] \"" + mod.Info.DisplayName + "\" : " + (mod.IsEnabled ? "[00FF00]Enabled" : "[FF0000]Disabled"));
            }

            sb.AppendLine();

            //Do some custom hooking
            FSHooks.DoEvent(null, "FSLOADER::GetCreditsText()", new object[] { sb });

            return sb.ToString() + original;
        }
    }
}
