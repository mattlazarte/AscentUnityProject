using UnityEngine;
using System.Collections;
using Ascent.PlayerShip;
using Ascent.Managers.Audio;

namespace Ascent.Items
{
    public class ItemGuidedMissileAmmo : Item
    {
        public int amount = 5;
    
        public override bool OnCatchInternal(PlayerShipWeaponryController playerShipWeaponryController)
        {
            //SoundFxsManager.instance.PlayOneShot(SoundFx.ItemCatch, transform.position);
            playerShipWeaponryController.IncreaseGuidedMissilesAmmo(amount);
            return true;
        }
    }
}
