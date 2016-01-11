using Ascent.Managers.Audio;
using DG.Tweening;
using System;
using UnityEngine;

namespace Ascent.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIWindow: MonoBehaviour
    {
        protected float fadeDuration = 0.15f;
        protected bool active;

        [HideInInspector]
        public CanvasGroup canvasGroup;

        public virtual void AwakeFromManager()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                throw new Exception("canvasGroup is null.");
        }

        public void DisableAndHide()
        {
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            active = false;
        }

        protected virtual void OnBeforeFadeIn() { }
        protected virtual void OnAfterFadeIn() { }
        public void FadeIn(Action onComplete = null, float? duration = null)
        {
            gameObject.SetActive(true);
            OnBeforeFadeIn();
            canvasGroup.DOFade(1, duration ?? fadeDuration).OnComplete(() =>
            {
                OnAfterFadeIn();

                if (onComplete != null)
                    onComplete();

                active = true;
            });
        }

        protected virtual void OnBeforeBackFadeIn() { }
        public void BackFadeIn(Action onComplete = null, float? duration = null)
        {
            gameObject.SetActive(true);
            OnBeforeBackFadeIn();
            canvasGroup.DOFade(1, duration ?? fadeDuration).OnComplete(() =>
            {
                if (onComplete != null)
                    onComplete();

                active = true;
            });
        }

        protected virtual void OnBeforeFadeOut() { }
        protected virtual void OnAfterFadeOut() { }
        public void FadeOut(Action onComplete = null)
        {
            active = false;
            OnBeforeFadeOut();
            canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
            {
                OnAfterFadeOut();

                if (onComplete != null)
                    onComplete();

                gameObject.SetActive(false);
            });
        }
    }

}