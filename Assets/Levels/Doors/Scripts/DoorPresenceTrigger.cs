using Ascent.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ascent
{
    [RequireComponent(typeof(Collider))]
    public class DoorPresenceTrigger : MonoBehaviour
    {
        public LayerMask maskToIgnore;

        private Collider myCollider;
        private List<Collider> collidersInside;

        private void Start()
        {
            myCollider = GetComponent<Collider>();
            if (myCollider.isTrigger == false)
                throw new Exception("Collider is not trigger.");

            collidersInside = new List<Collider>();

            StartCoroutine(CheckForDestroyedColliders());
        }

        private IEnumerator CheckForDestroyedColliders()
        {
            while (true)
            {
                for (var i = collidersInside.Count - 1; i >= 0; i--)
                {
                    if (collidersInside[i] == null || !collidersInside[i].gameObject.activeSelf)
                    {
                        collidersInside.Remove(collidersInside[i]);
                    }
                }

                yield return new WaitForSeconds(.5f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!maskToIgnore.ContainsLayer(other.gameObject.layer))
            {
                collidersInside.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            collidersInside.Remove(other);
        }

        public bool ContainsObjects()
        {
            return collidersInside.Count > 0;
        }
    }

}