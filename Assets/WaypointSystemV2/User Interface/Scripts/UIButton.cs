using Ascent.Managers.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UIButton : Button, ISelectHandler
    {
        public bool muted;
        protected Text labelText;
        protected CanvasGroup canvasGroup;

        public Color labelColor
        {
            get { return labelText.color; }
            set { labelText.color = value; }
        }
        
        public string label
        {
            get { return labelText.text; }
            set { labelText.text = value; }
        }

        public float alpha
        {
            get { return canvasGroup.alpha; }
            set { canvasGroup.alpha = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            labelText = GetComponentInChildren<Text>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            
            base.OnSelect(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!muted) AudioManager.instance.Play(AudioBank.SFX_UI_HOVER, this.gameObject);
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            //if (!muted) SoundFxsManager.instance.PlayOneShot2D(SoundFx.PlasmaGunShot);
            if (!muted) AudioManager.instance.Play(AudioBank.SFX_UI_CONFIRM, this.gameObject);
            base.OnPointerDown(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            //if (!muted) SoundFxsManager.instance.PlayOneShot2D(SoundFx.PlasmaGunShot);
            if (!muted) AudioManager.instance.Play(AudioBank.SFX_UI_CONFIRM, this.gameObject);
            base.OnSubmit(eventData);
        }

        public void MuteSelect()
        {
            muted = true;
            Select();
            muted = false;
        }

        public void Disable()
        {
            this.enabled = false;
            this.alpha = 0.3f;
        }
        public void Enable()
        {
            this.enabled = true;
            this.alpha = 1f;
        }

        private void OnHover()
        {
            if (!muted) AudioManager.instance.Play(AudioBank.SFX_UI_HOVER, this.gameObject);
        }
    }
}