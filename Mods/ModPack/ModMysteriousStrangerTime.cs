using FSLoader;
using System.Collections.Generic;

namespace ModPack
{
    [ModInfo("mysterious_stranger_time", "Mysterious stranger", "Robot9706", 1, 0, "Change the timing of the MysteriousStranger.")]
    public class ModMysteriousStrangerTime : Mod
    {
        private float _timeToAppear = 5.0f;
        private float _timeToHide = 5.0f;

        public override void OnInit()
        {
            ConfigSection section = GetModConfig();
            _timeToAppear = section.GetValue("time_to_appear", 300.0f);
            _timeToHide = section.GetValue("time_to_hide", 10.0f);
        }

        [Hook("MysteriousStrangerMgr::GetTimeToAppear()")]
        public void Hook_TimeToAppear(CallContext context)
        {
            MysteriousStrangerMgr mgr = (MysteriousStrangerMgr)context.This;
            mgr.M_canAppear = true;

            context.IsHandled = true;
            context.ReturnValue = _timeToAppear;
        }

        [Hook("MysteriousStrangerMgr::GetTimeToHide()")]
        public void Hook_TimeToHide(CallContext context)
        {
            context.IsHandled = true;
            context.ReturnValue = _timeToHide;
        }

        [Hook("MysteriousStrangerMgr::Deserialize(System.Collections.Generic.Dictionary`2<System.String,System.Object>)")]
        public void Hook_Deserialize(CallContext context, Dictionary<string, object> data)
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
