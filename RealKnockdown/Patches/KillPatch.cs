using BepInEx;
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

        private static DateTime RagdollTime = DateTime.Now;

        protected override MethodBase GetTargetMethod() => typeof(ActiveHealthController).GetMethod(nameof(ActiveHealthController.Kill));

        [PatchPrefix]
        private static bool Prefix(ActiveHealthController __instance)
        {

            Player player = __instance.Player;

            if (HelperMethods.Ragdolled && DateTime.Now - LastMessage > TimeSpan.FromSeconds(4))
            {
                // kill player if they get shot to death while knocked
                HelperMethods.Ragdolled = false;
                return true;
            }

            if (player.IsAI) { return true; }

            if (DateTime.Now - LastMessage > TimeSpan.FromSeconds(1))
            {
                NotificationManagerClass.DisplayMessageNotification("You fell! Press space to get back up, quickly!", ENotificationDurationType.Default, ENotificationIconType.Alert, Color.red);
                NotificationManagerClass.DisplayMessageNotification("Your character will die if shot while on the ground!", ENotificationDurationType.Default, ENotificationIconType.Alert, Color.red);
                LastMessage = DateTime.Now;
            }

            HelperMethods.RestoreHealth(__instance);
            HelperMethods.RagdollPlayer(player);

            RagdollTime = DateTime.Now;

            return false;
        }
    }
}
