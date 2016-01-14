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

            //SoundFxsManager.instance.PlayOneShot(SoundFx.ItemCatch, transform.position);
            playerShipWeaponryController.IncreaseShield(increaseValue);
            return true;
        }
    }
}