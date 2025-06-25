using EFT;
using EFT.Communications;
using EFT.HealthSystem;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace RealKnockdown.Patches
{
    public class KillPatch : ModulePatch
    {
        private static DateTime LastMessage = DateTime.Now;

        protected override MethodBase GetTargetMethod() => typeof(ActiveHealthController).GetMethod(nameof(ActiveHealthController.Kill));

        [PatchPrefix]
        private static bool Prefix(ActiveHealthController __instance)
        {
            if (HelperMethods.Ragdolled)
            {
                // kill player if they get shot to death while knocked
                return true;
            }


            Player player = __instance.Player;

            if (player.IsAI) { return true; }

            if (DateTime.Now - LastMessage > TimeSpan.FromSeconds(1))
            {
                NotificationManagerClass.DisplayMessageNotification("You fell! Press space to get back up, quickly!", ENotificationDurationType.Default, ENotificationIconType.Alert, Color.red);
                LastMessage = DateTime.Now;
            }

            HelperMethods.RestoreHealth(__instance);
            HelperMethods.RagdollPlayer(player);

            return false;
        }
    }
}
