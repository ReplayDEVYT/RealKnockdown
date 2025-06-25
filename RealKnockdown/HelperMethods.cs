using EFT;
using EFT.Communications;
using EFT.HealthSystem;
using EFT.Interactive;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace RealKnockdown
{
    public class HelperMethods
    {

        public static bool Ragdolled = false;

        public static Corpse PCorpse;

        public static void RestoreHealth(ActiveHealthController __instance)
        {
            Task.Delay(1000).ContinueWith(T =>
            {
                foreach (EBodyPart BodyPart in Enum.GetValues(typeof(EBodyPart))) // Remove negative effects
                {
                    __instance.method_18(BodyPart, (ignore) => true);
                }
                __instance.RestoreFullHealth();
                __instance.DoPainKiller();
            });
        }

        public static void RagdollPlayer(Player player)
        {
            if (Ragdolled) { return; }

            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            Ragdolled = true;

            // disables all player movement and animations
            player.BodyAnimatorCommon.enabled = false;

            // toggles prone to start animation when getting back up
            player.ToggleProne();

            player.MovementContext.ReleaseDoorIfInteractingWithOne();

            player.PlayDeathSound();

            // creates the corpse
            PCorpse = player.CreateCorpse<Corpse>(Vector3.zero);
            player.ApplyCorpseImpulse();

            if (BackendConfigAbstractClass.Config.UseBodyFastAnimator)
            {
                player.PlayerBones.PlayableAnimator.Stop();
            }

            if (player.MovementContext.StationaryWeapon != null)
            {
                player.MovementContext.StationaryWeapon.Show();
                player.ReleaseHand();
            }
        }

        public static void GetUp(Player player)
        {
            // enables all player movement and animations
            player.ArmsAnimatorCommon.enabled = true;
            player.BodyAnimatorCommon.enabled = true;

            NotificationManagerClass.DisplayMessageNotification("Player getting up!", ENotificationDurationType.Default, ENotificationIconType.Alert, Color.green);

            HelperMethods.Ragdolled = false;
        }
    }
}
