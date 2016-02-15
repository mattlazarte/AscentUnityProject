using UnityEngine;
using System.Collections;
using Ascent.PlayerShip;
using Ascent.Managers.Audio;

namespace Ascent.Items
{
    public class ItemIncreaseShield : Item
    {
        public float increaseValue = 5f;

        public override bool OnCatchInternal(PlayerShipWeaponryController playerShipWeaponryController)
        {
            if (playerShipWeaponryController.IsShieldAtMaxValue)
            {
                return false;
            }
            AudioManager.instance.Play(AudioBank.SFX_ITEM_ENERGY, this.gameObject);

            Invoke("BoostShield", 0.2f);
            //SoundFxsManager.instance.PlayOneShot(SoundFx.ItemCatch, transform.position);
            playerShipWeaponryController.IncreaseShield(increaseValue);
            return true;
        }

        void BoostShield()
        {
            AudioManager.instance.Play(AudioBank.SFX_SHIELD_UP, this.gameObject);
        }
    }
}