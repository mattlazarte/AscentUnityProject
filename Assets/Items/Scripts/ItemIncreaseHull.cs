using UnityEngine;
using System.Collections;
using Ascent.PlayerShip;
using Ascent.Managers.Audio;

namespace Ascent.Items
{
    public class ItemIncreaseHull : Item
    {
        public float increaseValue = 5f;

        public override bool OnCatchInternal(PlayerShipWeaponryController playerShipWeaponryController)
        {
            if (playerShipWeaponryController.IsHullAtMaxValue)
            {
                return false;
            }
            Invoke("BoostHealth", 0.2f);

            AudioManager.instance.Play(AudioBank.SFX_ITEM_HEALTH, this.gameObject);
            //SoundFxsManager.instance.PlayOneShot(SoundFx.ItemCatch, transform.position);
            playerShipWeaponryController.IncreaseHull(increaseValue);
            return true;
        }

        void BoostHealth()
        {
            AudioManager.instance.Play(AudioBank.SFX_SHIELD_UP, this.gameObject);
        }
    }
}
