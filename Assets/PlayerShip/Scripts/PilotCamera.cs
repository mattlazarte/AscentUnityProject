using Ascent.Managers.Game;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;

namespace Ascent.PlayerShip
{
    [RequireComponent(typeof(Blur))]
    public class PilotCamera : MonoBehaviour
    {
        public Image faderImage;

        private Blur blurFx;
    
        private void Awake()
        {
            if (faderImage == null)
                throw new Exception("faderImage is null.");

            blurFx = GetComponent<Blur>();
            blurFx.enabled = false;
        }

        public void FadeIn(float duration, Action onComplete = null)
        {
            faderImage.color = Color.black;
            faderImage.DOFade(0f, duration).OnComplete(() =>
            {
                if (onComplete != null) onComplete();
                faderImage.gameObject.SetActive(false);
            });
        }

        public void FadeOut(float duration, Action onComplete = null)
        {
            faderImage.color = Color.black.Copy(0f);
            faderImage.gameObject.SetActive(true);
            faderImage.DOFade(1f, duration).OnComplete(() =>
            {
                if (onComplete != null) onComplete();
            });
        }
    
        public void EnableBlur()
        {
            blurFx.enabled = true;
        }
    
        public void DisableBlur()
        {
            blurFx.enabled = false;
        }
    }
}
