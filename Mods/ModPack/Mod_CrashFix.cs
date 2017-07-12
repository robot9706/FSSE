using FSLoader;
using Game.Actors;
using Game.Actors.Pets;
using Game.Actors.States;

namespace ModPack
{
	[ModInfo(null, "", "", 0, 0)]
	public class Mod_CrashFix : Mod
	{
		[Hook("Game.Actors.States.PetFollowDweller::PrepareToFollow(System.Boolean)")]
		public void Hook_Fix(CallContext context, bool isResumed)
		{
			context.IsHandled = true;

			PetFollowDweller pfd = (PetFollowDweller)context.This;

			if (pfd.M_Actor == null || !(pfd.M_Actor is Pet))
				return;

			if (((Pet)pfd.M_Actor).IsNearFollowed(false))
			{
				pfd.M_isFollowingStarted = true;
			}
			else
			{
				if (pfd.M_Actor.AI == null || !(pfd.M_Actor.AI is PetAI))
					return;

				((PetAI)pfd.M_Actor.AI).NavigateToFollowed(null, false);

				if (isResumed && pfd.M_Actor.CurrentPathNode != null && pfd.M_Actor.CurrentPathNode.Room.RoomType != ERoomType.Elevator)
				{
					if (((PetAI)pfd.M_Actor.AI).MovingState == null)
						return;

					((PetAI)pfd.M_Actor.AI).MovingState.SetAccelerationTimeToMax();

					pfd.M_resetPetAccelerationTime = true;
				}
			}
		}
	}
}
