using Ascent.Managers.Input;
using Ascent.Managers.Input.Sources;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectAxisFromKeyboardWindow : UIWindow
    {
        public static UISelectAxisFromKeyboardWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<MappableKeys, MappableKeys> onSelect;

        public Text label;
        public UIButton buttonCancel;
        public UIButton buttonPositiveKey;
        public UIButton buttonNegativeKey;
        public UIButton buttonOk;

        protected MappableKeys? positiveKey;
        protected MappableKeys? negativeKey;

        public override void AwakeFromManager()
        {
            base.AwakeFromManager();
            CheckBindings();
            InitUI();

            instance = this;
        }
        protected void CheckBindings()
        {
            if (label == null)
                throw new Exception("label is null.");

            if (buttonCancel == null)
                throw new Exception("buttonCancel is null.");

            if (buttonPositiveKey == null)
                throw new Exception("buttonPositiveKey is null.");

            if (buttonNegativeKey == null)
                throw new Exception("buttonNegativeKey is null.");

            if (buttonOk == null)
                throw new Exception("buttonOk is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());

            buttonPositiveKey.onClick.AddListener(() =>
            {
                UISelectKeyboardKeyWindow.onCancel = () =>
                {
                    UISelectKeyboardKeyWindow.instance.FadeOut(() => BackFadeIn());
                };
                UISelectKeyboardKeyWindow.onSelect = (key) =>
                {
                    positiveKey = key;
                    UpdateButtons();
                    UISelectKeyboardKeyWindow.instance.FadeOut(() => BackFadeIn());
                };

                FadeOut(() => UISelectKeyboardKeyWindow.instance.FadeIn());
            });

            buttonNegativeKey.onClick.AddListener(() =>
            {
                UISelectKeyboardKeyWindow.onCancel = () =>
                {
                    UISelectKeyboardKeyWindow.instance.FadeOut(() => BackFadeIn());
                };
                UISelectKeyboardKeyWindow.onSelect = (key) =>
                {
                    negativeKey = key;
                    UpdateButtons();
                    UISelectKeyboardKeyWindow.instance.FadeOut(() => BackFadeIn());
                };

                FadeOut(() => UISelectKeyboardKeyWindow.instance.FadeIn());
            });

            buttonOk.onClick.AddListener(() =>
            {
                onSelect(positiveKey.Value, negativeKey.Value);
                FadeOut(() => UIControlsWindow.instance.FadeIn());
            });
        }
        protected override void OnBeforeFadeIn()
        {
            positiveKey = null;
            negativeKey = null;
            UpdateButtons();

            label.text = controlName;
            buttonCancel.MuteSelect();
        }
        protected void UpdateButtons()
        {
            buttonPositiveKey.label = positiveKey.HasValue ? InputManagerHelpers.GetMappableKeysDescription(positiveKey.Value) : string.Empty;
            buttonNegativeKey.label = negativeKey.HasValue ? InputManagerHelpers.GetMappableKeysDescription(negativeKey.Value) : string.Empty;

            if (positiveKey.HasValue && negativeKey.HasValue)
            {
                buttonOk.Enable();
            }
            else
            {
                buttonOk.Disable();
            }
        }
    }
}
