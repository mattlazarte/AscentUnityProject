using UnityEngine;
using System.Collections;
using Ascent.PlayerShip;
using Ascent.Managers.Audio;

namespace Ascent.Items
{
    public class ItemLaserAmmo : Item
    {
        public int amount = 20;
    
        public override bool OnCatchInternal(PlayerShipWeaponryController playerShipWeaponryController)
        {
            AudioManager.instance.Play(AudioBank.SFX_ITEM_CATCH, this.gameObject);
            //SoundFxsManager.instance.PlayOneShot(SoundFx.ItemCatch, transform.position);
            playerShipWeaponryController.IncreaseLaserAmmo(amount);
            return true;
        }
    }
}
