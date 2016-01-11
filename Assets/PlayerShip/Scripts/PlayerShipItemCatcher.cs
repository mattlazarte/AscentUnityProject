using Ascent.Managers.Game;
using Ascent.Managers.Input;
using Ascent.Weaponry;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Ascent.Items;

namespace Ascent.PlayerShip
{
    public class PlayerShipItemCatcher : MonoBehaviour
    {
        private PlayerShipWeaponryController playerShipWeaponryController;
    
        private void Start()
        {
            playerShipWeaponryController = GetComponent<PlayerShipWeaponryController>();
            if (playerShipWeaponryController == null)
                throw new Exception("PlayerShipItemCatcher playerShipWeaponryController'is null.");
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            var itemComponent = collision.gameObject.GetComponent<Item>();
            if (itemComponent != null)
            {
                itemComponent.OnCatch(playerShipWeaponryController);
            }
        }
    }
}
