using FSLoader;
using System.Collections.Generic;

namespace ModPack
{
    [ModInfo("unlimited_resoures", "Unlimited resources", "Robot9706", 1, 0, "Unlimited resources (caps, water, food, etc, all of them are configurable).")]
    public class ModUnlimitedResources : Mod
    {
        private bool _unlimitedCaps;
        private bool _unlimitedResources;
        private bool _unlimitedQuantum;
        private bool _unlimitedStims;
        private bool _unlimitedRads;

        private bool _hasEnabledFlag;

        private static List<EResource> _notTested = new List<EResource>()
        {
            EResource.Nuka,
            EResource.NukaColaQuantum,
            EResource.Water,
            EResource.Food,
            EResource.Energy,
            EResource.StimPack,
            EResource.RadAway
        };

        public override void OnInit()
        {
            ConfigSection config = GetModConfig();
            _unlimitedCaps = config.GetValue<bool>("unlimited_caps");
            _unlimitedResources = config.GetValue<bool>("unlimited_resoures");
            _unlimitedQuantum = config.GetValue<bool>("unlimited_quantum");
            _unlimitedStims = config.GetValue<bool>("unlimited_stimpacks");
            _unlimitedRads = config.GetValue<bool>("unlimited_radaways");

            _hasEnabledFlag = _unlimitedCaps || _unlimitedQuantum || _unlimitedResources ||_unlimitedStims || _unlimitedRads;
        }

        [Hook("Storage::get_Resources()")]
        public void Hook_GetResources(CallContext context)
        {
            if (!_hasEnabledFlag)
                return;

            Storage storage = (Storage)context.This;
            if(storage is VaultStorage)
            {
                GameResources original = storage.M_resources;

                context.IsHandled = true;
                context.ReturnValue = new GameResources(
                        (_unlimitedResources ? 100000 : original.GetResource(EResource.Food)),
                        (_unlimitedResources ? 100000 : original.GetResource(EResource.Energy)),

                        (_unlimitedCaps ? 100000 : original.GetResource(EResource.Nuka)),

                        (_unlimitedResources ? 100000 : original.GetResource(EResource.Water)),

                        (_unlimitedStims ? 100000 : original.GetResource(EResource.StimPack)),
                        (_unlimitedRads ? 100000 : original.GetResource(EResource.RadAway)),

                        original.GetResource(EResource.Lunchbox),
                        original.GetResource(EResource.MrHandy),
                        original.GetResource(EResource.PetCarrier),

                        (_unlimitedQuantum ? 100000 : original.GetResource(EResource.NukaColaQuantum))
                    );
            }
        }

        [Hook("Storage::HasResources(GameResources)")]
        public void Hook_HasResources(CallContext context, GameResources test)
        {
            if (!_hasEnabledFlag)
                return;

            Storage storage = (Storage)context.This;
            if (storage is VaultStorage)
            {
                GameResources original = storage.M_resources;

                context.IsHandled = true;

                bool caps = (_unlimitedCaps ? true : original.Nuka >= test.Nuka);
                bool quant = (_unlimitedQuantum ? true : original.NukeColaQuantum >= test.NukeColaQuantum);
                bool res = (_unlimitedResources ? true : (original.Power >= test.Power && original.Water >= test.Water && original.Food >= test.Food));
                bool stims = (_unlimitedStims ? true : original.StimPack >= test.StimPack);
                bool rads = (_unlimitedRads ? true : original.RadAway >= test.RadAway);
                bool other = original.IsGreaterOrEqualThan(test, _notTested);

                context.ReturnValue = (caps && quant && res && stims && rads && other);
            }
        }
    }
}
