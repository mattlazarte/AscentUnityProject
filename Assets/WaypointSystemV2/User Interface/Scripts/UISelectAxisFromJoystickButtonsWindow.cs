using Ascent.Managers.Input;
using Ascent.Managers.Input.Sources;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectAxisFromJoystickButtonsWindow : UIWindow
    {
        public static UISelectAxisFromJoystickButtonsWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<XboxJoystickButtons, XboxJoystickButtons> onSelect;
    
        public Text label;
        public UIButton buttonCancel;
        public UIButton buttonPositiveButton;
        public UIButton buttonNegativeButton;
        public UIButton buttonOk;

        protected XboxJoystickButtons? positiveButton;
        protected XboxJoystickButtons? negativeButton;
    
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
    
            if (buttonPositiveButton == null)
                throw new Exception("buttonPositiveButton is null.");
    
            if (buttonNegativeButton == null)
                throw new Exception("buttonNegativeButton is null.");
    
            if (buttonOk == null)
                throw new Exception("buttonOk is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());
    
            buttonPositiveButton.onClick.AddListener(() =>
            {
                UISelectJoystickButtonWindow.showDPadButtons = false;
                UISelectJoystickButtonWindow.onCancel = () =>
                {
                    UISelectJoystickButtonWindow.instance.FadeOut(() => BackFadeIn());
                };
                UISelectJoystickButtonWindow.onSelect = (selection) =>
                {
                    positiveButton = selection.button;
                    UpdateButtons();
                    UISelectJoystickButtonWindow.instance.FadeOut(() => BackFadeIn());
                };

                FadeOut(() => UISelectJoystickButtonWindow.instance.FadeIn());
            });
    
            buttonNegativeButton.onClick.AddListener(() =>
            {
                UISelectJoystickButtonWindow.showDPadButtons = false;
                UISelectJoystickButtonWindow.onCancel = () =>
                {
                    UISelectJoystickButtonWindow.instance.FadeOut(() => BackFadeIn());
                };
                UISelectJoystickButtonWindow.onSelect = (selection) =>
                {
                    negativeButton = selection.button;
                    UpdateButtons();
                    UISelectJoystickButtonWindow.instance.FadeOut(() => BackFadeIn());
                };

                FadeOut(() => UISelectJoystickButtonWindow.instance.FadeIn());
            });
    
            buttonOk.onClick.AddListener(() =>
            {
                onSelect(positiveButton.Value, negativeButton.Value);
                FadeOut(() => UIControlsWindow.instance.FadeIn());
            });
        }
        protected override void OnBeforeFadeIn()
        {
            positiveButton = null;
            negativeButton = null;
            UpdateButtons();
    
            label.text = controlName;
            buttonCancel.MuteSelect();
        }
        protected void UpdateButtons()
        {
            buttonPositiveButton.label = positiveButton.HasValue ? InputManagerHelpers.GetXboxJoystickButtonsDescription(positiveButton.Value) : string.Empty;
            buttonNegativeButton.label = negativeButton.HasValue ? InputManagerHelpers.GetXboxJoystickButtonsDescription(negativeButton.Value) : string.Empty;
    
            if (positiveButton.HasValue && negativeButton.HasValue)
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
