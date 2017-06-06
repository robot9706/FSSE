using FSLoader;
using XInputDotNetPure;

namespace ModPack
{
    [ModInfo("disable_controller_vibration", "Disable controller vibration.", "Robot9706", 1, 0)]
    public class ModNoControllerVibration : Mod
    {
        [Hook("InputManager::Vibrate()")]
        public void Hook_Vibrate(CallContext context)
        {
            InputManager.M_vibrating = false;

            for (PlayerIndex pi = PlayerIndex.One; pi <= PlayerIndex.Four; pi++)
            {
                GamePad.SetVibration(pi, 0.0f, 0.0f);
            }
        }
    }
}
