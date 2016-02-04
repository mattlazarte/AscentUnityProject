using UnityEngine;
using System.Collections;
using Ascent.PlayerShip;
using Ascent.Managers.Audio;

namespace Ascent.Items
{
    public class ItemUnguidedMissileAmmo : Item
    {
        public int amount = 10;
    
        public override bool OnCatchInternal(PlayerShipWeaponryController playerShipWeaponryController)
        {
            AudioManager.instance.Play(AudioBank.SFX_ITEM_AMMO, this.gameObject);
            //SoundFxsManager.instance.PlayOneShot(SoundFx.ItemCatch, transform.position);
            playerShipWeaponryController.IncreaseUnguidedMissilesAmmo(amount);
            return true;
        }
    }
}
