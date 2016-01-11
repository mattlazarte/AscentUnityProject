using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers;

namespace Ascent.Weaponry
{
    public class FadeOutDecal<PoolType> : TempPooledMonoBehavior<PoolType>, IPoolOnInstantiateListener
        where PoolType : Component
    {
        public float fadeSpeed = 0.01f;
        public float currentFadeStep;
        public Color initialColor = new Color(1f, 1f, 1f, .4f);
        public Color endColor = new Color(1f, 1f, 1f, 0);

        private MeshRenderer meshRenderer;

        public void OnInstantiate()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
                throw new Exception("Renderer is null.");
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            currentFadeStep = 0;
            SetColor(initialColor);
        }

        protected override void Update()
        {
            base.Update();

            currentFadeStep = currentFadeStep + fadeSpeed;

            if (currentFadeStep > 1)
                currentFadeStep = 1;

            var newColor = Color.Lerp(initialColor, endColor, currentFadeStep);

            SetColor(newColor);
        }

        private void SetColor(Color color)
        {
            if (meshRenderer != null)
                if (meshRenderer.material != null)
                    meshRenderer.material.SetColor("_TintColor", color);
        }
    }
}