using FSLoader;

namespace ModPack
{
    [ModInfo("rush_instant", "Instant room rushes", "Robot9706", 1, 0)]
    public class ModInstantRush : Mod
    {
        [Hook("ProductionRoom/ProductionRoomRushing::.ctor(ProductionRoom)")]
        public void Hook_ProductionRoomRushing_Ctor(CallingContext context, ProductionRoom room)
        {
            context.IsHandled = true;

            ProductionRoom.ProductionRoomRushing instance = (ProductionRoom.ProductionRoomRushing)context.This;
            instance.M_room = room;
            instance.M_animationTime = 0.0f;
        }
    }
}
