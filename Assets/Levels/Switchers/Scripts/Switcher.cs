using UnityEngine;
using System.Collections;
using System;
using Ascent.Weaponry;
using Ascent.Managers.Audio;

namespace Ascent
{
    public class Switcher : MonoBehaviour, IHittableObject
    {
        public Door door;

        protected Light switcherLight;
        protected Material modelInnerPartMaterial;

        public void Start()
        {
            if (door == null)
                throw new Exception("door is null.");

            switcherLight = GetComponentInChildren<Light>();
            if (switcherLight == null)
                throw new Exception("light not found.");

            var modelMeshRenderer = GetComponentInChildren<MeshRenderer>();
            if (modelMeshRenderer == null)
                throw new Exception("modelMeshRenderer not found.");

            if (modelMeshRenderer.materials.Length != 3)
                throw new Exception("modelMeshRenderer.materials.Length != 3");

            modelInnerPartMaterial = modelMeshRenderer.materials[1];
        }

        public void OnHitByWeapon(float damage, bool shotByPlayer)
        {
            if (door.locked && shotByPlayer)
            {
                //SoundFxsManager.instance.PlayOneShot(SoundFx.Switcher, this.transform.position);

                door.SetLocked(false);
                switcherLight.color = Color.green;
                modelInnerPartMaterial.SetColor("_Color", Color.green);
            }
        }
    }
}