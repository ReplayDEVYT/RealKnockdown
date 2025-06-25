using BepInEx;
using System;
using RealKnockdown.Patches;

namespace RealKnockdown
{
    [BepInPlugin("com.mizmii.realknockdown", "Real Knockdown", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            try
            {
                new KillPatch().Enable();
                new JumpPatch().Enable();
            }
            catch (Exception ex)
            {
                Logger.LogError($"A PATCH IN {GetType().Name} FAILED. SUBSEQUENT PATCHES HAVE NOT LOADED");
                Logger.LogError($"{GetType().Name}: {ex}");
                throw;
            }
        }

    }
}
