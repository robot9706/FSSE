using FSLoader;
using System;
using System.Collections.Generic;

namespace ModPack
{
    [ModInfo("mysterious_stranger_time", "Mysterious stranger", "Robot9706", 1, 0, "Change the timing of the MysteriousStranger. Requires additional configuration.",
@"The config has the following parameters:
mode: The way the following parameters are used. Possible values are ""Seconds"" (times are in seconds) and ""Multiplier"" (times are used as multipliers to change the times).
time_to_appear: Time for the stranger to appear.
time_to_hide: Time for the stranger to hide.
")]
    public class ModMysteriousStrangerTime : Mod
    {
        enum Mode
        {
            Seconds,
            Multiplier
        }

        private Mode _mode;

        private float _timeToAppear = 5.0f;
        private float _timeToHide = 5.0f;

        public override void OnInit()
        {
            ConfigSection section = GetModConfig();
            _mode = GetMode(section);
            _timeToAppear = section.GetValue("time_to_appear", 300.0f);
            _timeToHide = section.GetValue("time_to_hide", 10.0f);
        }

        private Mode GetMode(ConfigSection section)
        {
            Mode mode = Mode.Seconds;

            try
            {
                mode = (Mode)Enum.Parse(typeof(Mode), section.GetString("mode"));
            }
            catch { }

            return mode;
        }

        [Hook("MysteriousStrangerMgr::GetTimeToAppear()")]
        public void Hook_TimeToAppear(CallContext context)
        {
            MysteriousStrangerMgr mgr = (MysteriousStrangerMgr)context.This;
            mgr.M_canAppear = true;

            context.IsHandled = true;

            if (_mode == Mode.Seconds)
            {
                context.ReturnValue = _timeToAppear;
            }
            else
            {
                FSHooks.DoWithDisabledHooks(() =>
                {
                    context.ReturnValue = mgr.GetTimeToAppear() * _timeToAppear;
                });
            }
        }

        [Hook("MysteriousStrangerMgr::GetTimeToHide()")]
        public void Hook_TimeToHide(CallContext context)
        {
            MysteriousStrangerMgr mgr = (MysteriousStrangerMgr)context.This;

            context.IsHandled = true;

            if (_mode == Mode.Seconds)
            {
                context.ReturnValue = _timeToHide;
            }
            else
            {
                FSHooks.DoWithDisabledHooks(() =>
                {
                    context.ReturnValue = mgr.GetTimeToAppear() * _timeToHide;
                });
            }
        }

        [Hook("MysteriousStrangerMgr::Deserialize(System.Collections.Generic.Dictionary`2<System.String,System.Object>)")]
        public void Hook_Deserialize(CallContext context, Dictionary<string, object> data)
        {
            if (_mode == Mode.Seconds)
            {
                context.IsHandled = true;

                MysteriousStrangerMgr mgr = (MysteriousStrangerMgr)context.This;

                //Call the original method so our hook will run "after" the hooked method
                FSHooks.DoWithDisabledHooks(() =>
                {
                    mgr.Deserialize(data);
                });

                mgr.M_timeToAppear = _timeToAppear;
                mgr.M_timeToHide = _timeToHide;
            }
        }
    }
}
