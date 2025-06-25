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
            // one way methods can be patched is by targeting both their class name and the name of the method itself
            // the example in this patch is the Jump() method in the Player class
            return AccessTools.Method(typeof(Player), nameof(Player.Jump));
        }

        [PatchPrefix]  
        static bool Prefix(Player __instance)
        {
            Player player = __instance;
            
            if (player.IsAI) { return true; }

            if (HelperMethods.Ragdolled)
            {
                // enables all player movement and animations
                player.ArmsAnimatorCommon.enabled = true;
                player.BodyAnimatorCommon.enabled = true;

                NotificationManagerClass.DisplayMessageNotification("Player getting up!", ENotificationDurationType.Default, ENotificationIconType.Alert, Color.green);

                HelperMethods.Ragdolled = false;

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
