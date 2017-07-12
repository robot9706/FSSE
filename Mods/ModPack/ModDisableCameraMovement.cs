using FSLoader;
using System;
using UnityEngine;

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

		[Hook("CameraController::MoveToCloserPositionCentered(System.Boolean)")]
		public void Hook_MoveToCloserPositionCentered(CallContext context, bool b)
		{
			context.IsHandled = true;
		}

		[Hook("CameraController::MoveToPosition(UnityEngine.Vector2,System.Action)")]
		public void Hook_MoveToCloserPositionCentered(CallContext context, Vector2 v2, Action act)
		{
			context.IsHandled = true;
		}

		[Hook("CameraController::MoveToPosition(UnityEngine.Vector3,System.Single)")]
		public void Hook_MoveToCloserPositionCentered(CallContext context, Vector3 v2, float f)
		{
			context.IsHandled = true;
		}

		[Hook("CameraController::MoveToStopEmergencyPosition(System.Single)")]
		public void Hook_MoveToCloserPositionCentered(CallContext context, float f)
		{
			context.IsHandled = true;
		}
	}
}
