using FSLoader;
using System;

namespace ModPack
{
    [ModInfo("instant_training", "Instant training", "Robot9706", 1, 0)]
    public class ModInstantTraining : Mod
    {
        [Hook("TrainingSlot::TrainingTimeSpan()")]
        public void Hook_GetTrainingTime(CallContext context)
        {
            TrainingSlot slot = (TrainingSlot)context.This;

            context.IsHandled = true;

            context.ReturnValue = new Task.TimeSpanFetch(new Func<TimeUnit>(() =>
            {
                slot.M_actualSpeedFactor = 5000000;
                return new TimeUnit((double)MonoSingleton<GameParameters>.Instance.Room.Class.Training.TaskCycle);
            }));
        }
    }
}