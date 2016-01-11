using Ascent.Managers.Input;
using Ascent.Managers.Input.Sources;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectAxisSourceWindow : UIWindow
    {
        public static bool showNotAssignedOption;
        public static UISelectAxisSourceWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<AxisSource> onSelect;

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

            buttonNotAssigned.onClick.AddListener(() =>
            {
                onSelect(null);
                FadeOut(() => UIControlsWindow.instance.BackFadeIn());
            });

            buttonKeyboard.onClick.AddListener(() =>
            {
                UISelectAxisFromKeyboardWindow.controlName = controlName;
                UISelectAxisFromKeyboardWindow.onCancel = () =>
                {
                    UISelectAxisFromKeyboardWindow.instance.FadeOut(() => FadeIn());
                };
                UISelectAxisFromKeyboardWindow.onSelect = (positiveKey, negativeKey) =>
                {
                    onSelect(new KeyboardAxisSource(positiveKey, negativeKey));
                };

                FadeOut(() => UISelectAxisFromKeyboardWindow.instance.FadeIn());
            });

            buttonMouse.onClick.AddListener(() =>
            {
                UISelectMouseAxisWindow.controlName = controlName;
                UISelectMouseAxisWindow.onCancel = () =>
                {
                    UISelectMouseAxisWindow.instance.FadeOut(() => FadeIn());
                };
                UISelectMouseAxisWindow.onSelect = (mouseAxis) =>
                {
                    onSelect(new MouseAxisSource(mouseAxis));
                    UISelectMouseAxisWindow.instance.FadeOut(() => UIControlsWindow.instance.BackFadeIn());
                };

                FadeOut(() => UISelectMouseAxisWindow.instance.FadeIn());
            });

            buttonJoystick.onClick.AddListener(() =>
            {
                UISelectJoystickAxisWindow.controlName = controlName;
                UISelectJoystickAxisWindow.onCancel = () =>
                {
                    UISelectJoystickAxisWindow.instance.FadeOut(() => FadeIn());
                };
                UISelectJoystickAxisWindow.onSelect = (axisSource) =>
                {
                    onSelect(axisSource);
                    UISelectJoystickAxisWindow.instance.FadeOut(() => UIControlsWindow.instance.BackFadeIn());
                };

                FadeOut(() => UISelectJoystickAxisWindow.instance.FadeIn());
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
