using FSLoader;

namespace ModPack
{
    [ModInfo("disable_auto_camera_movement", "Disable automatic camera movement.", "Robot9706", 1, 0, "Disable automatic camera movement (to incidents, etc.).")]
    public class ModDisableCameraMovement : Mod
    {
        [Hook("VaultViewState::MoveCamera(EVaultInitPos)")]
        public void Hook_MoveCamera(CallContext context, EVaultInitPos pos)
        {
            if (pos != EVaultInitPos.SavedZoom)
            {
                context.IsHandled = true;
            }
        }
    }
}
