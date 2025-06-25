using BepInEx;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace RealKnockdown.Patches
{
    public class ForceReinit : ModulePatch
    {

        protected override MethodBase GetTargetMethod() => typeof(Corpse).GetMethod(nameof(Corpse.CreateCorpse));

        [PatchPrefix]
        private static bool Prefix(
            GameObject gameObject,
            string playerProfileID,
            InventoryEquipment equipment,
            GClass1965 customization,
            bool reinitBody,
            GameWorld gameWorld,
            EPlayerSide side,
            Vector3 velocity,
            Transform pelvis,
            global::BindableStateClass<Item> itemInHands,
            bool foreStillCorpse,
            GClass746 containerCollectionView,
            MongoID firstID = default(MongoID))
        {
            if (HelperMethods.Ragdolled)
            {
                Corpse comp = gameObject.AddComponent<Corpse>();
                comp.method_17(playerProfileID, equipment, customization, reinitBody: true, gameWorld, side, velocity / 2, pelvis, ragdollEnabled: true, itemInHands, false, containerCollectionView, firstID);
                return comp;
            }
            else
            {
                // If not ragdolled, we allow the method to proceed normally  
                return true;
            }
        }
    }
}
