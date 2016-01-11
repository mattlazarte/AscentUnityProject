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

            SoundFxsManager.instance.PlayOneShot(SoundFx.ItemCatch, transform.position);
            playerShipWeaponryController.IncreaseHull(increaseValue);
            return true;
        }
    }
}
