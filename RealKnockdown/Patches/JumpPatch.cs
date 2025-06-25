using EFT;
using EFT.Communications;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace RealKnockdown.Patches
{
    internal class JumpPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.Jump));
        }

        [PatchPrefix]  
        static bool Prefix(Player __instance)
        {
            Player player = __instance;
            
            if (player.IsAI) { return true; }

            if (HelperMethods.Ragdolled)
            {
                HelperMethods.GetUp(player);

                return false;
            }
            else
            {
                // if the player is not ragdolled, we allow the jump to proceed
                return true;
            }
        }
    }
}
