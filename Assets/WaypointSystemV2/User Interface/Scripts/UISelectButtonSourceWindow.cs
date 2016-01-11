using Ascent.Managers.Input;
using Ascent.Managers.Input.Sources;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectButtonSourceWindow : UIWindow
    {
        public static bool showNotAssignedOption;
        public static UISelectButtonSourceWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<ButtonSource> onSelect;
    
        public Text label;
        public UIButton buttonKeyboard;
        public UIButton buttonMouse;
        public UIButton buttonJoystick;
        public UIButton buttonNotAssigned;
        public UIButton buttonCancel;
    
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
    
            if (buttonKeyboard == null)
                throw new Exception("buttonKeyboard is null.");
    
            if (buttonMouse == null)
                throw new Exception("buttonMouse is null.");

            if (buttonJoystick == null)
                throw new Exception("buttonJoystick is null.");

            if (buttonNotAssigned == null)
                throw new Exception("buttonNotAssigned is null.");
    
            if (buttonCancel == null)
                throw new Exception("buttonCancel is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());

            buttonNotAssigned.onClick.AddListener(() => {
                onSelect(null);
                FadeOut(() => UIControlsWindow.instance.BackFadeIn());
            });

            buttonKeyboard.onClick.AddListener(() =>
            {
                UISelectKeyboardKeyWindow.onCancel = () =>
                {
                    UISelectKeyboardKeyWindow.instance.FadeOut(() => FadeIn());
                };
                UISelectKeyboardKeyWindow.onSelect = (key) =>
                {
                    onSelect(new KeyboardButtonSource(key));
                    UISelectKeyboardKeyWindow.instance.FadeOut(() => UIControlsWindow.instance.BackFadeIn());
                };

                FadeOut(() => UISelectKeyboardKeyWindow.instance.FadeIn());
            });

            buttonMouse.onClick.AddListener(() =>
            {
                UISelectMouseButtonWindow.controlName = controlName;
                UISelectMouseButtonWindow.onCancel = () =>
                {
                    UISelectMouseButtonWindow.instance.FadeOut(() => FadeIn());
                };
                UISelectMouseButtonWindow.onSelect = (mouseButton) =>
                {
                    onSelect(new MouseButtonSource(mouseButton));
                    UISelectMouseButtonWindow.instance.FadeOut(() => UIControlsWindow.instance.BackFadeIn());
                };

                FadeOut(() => UISelectMouseButtonWindow.instance.FadeIn());
            });

            buttonJoystick.onClick.AddListener(() =>
            {
                UISelectJoystickButtonWindow.showDPadButtons = true;
                UISelectJoystickButtonWindow.controlName = controlName;
                UISelectJoystickButtonWindow.onCancel = () =>
                {
                    UISelectJoystickButtonWindow.instance.FadeOut(() => FadeIn());
                };
                UISelectJoystickButtonWindow.onSelect = (selection) =>
                {
                    switch (selection.type)
                    {
                        case UISelectJoystickButtonWindow.JoystickButtonSelectionType.Button:
                            onSelect(new XboxJoystickButtonSource(selection.button));
                            break;

                        case UISelectJoystickButtonWindow.JoystickButtonSelectionType.DPad:
                            onSelect(new XboxJoystickDPadButtonSource(selection.dpadButton));
                            break;

                        default:
                            throw new Exception("Unexpected value of selection.type.");
                    }

                    UISelectJoystickButtonWindow.instance.FadeOut(() => UIControlsWindow.instance.BackFadeIn());
                };

                FadeOut(() => UISelectJoystickButtonWindow.instance.FadeIn());
            });
        }
        protected override void OnBeforeFadeIn()
        {
            if (showNotAssignedOption)
            {
                buttonNotAssigned.Enable();
            }
            else
            {
                buttonNotAssigned.Disable();
            }

            label.text = controlName;
            buttonCancel.MuteSelect();
        }
    }
}
