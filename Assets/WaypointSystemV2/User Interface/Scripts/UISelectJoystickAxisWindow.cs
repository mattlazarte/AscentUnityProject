using Ascent.Managers.Input;
using Ascent.Managers.Input.Sources;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectJoystickAxisWindow : UIWindow
    {
        public static UISelectJoystickAxisWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<AxisSource> onSelect;
    
        public Text label;
        public UIButton buttonCancel;
        public UIButton buttonLeftStickHorizontal;
        public UIButton buttonLeftStickVertical;
        public UIButton buttonRightStickHorizontal;
        public UIButton buttonRightStickVertical;
        public UIButton buttonLeftTrigger;
        public UIButton buttonRightTrigger;
        public UIButton buttonDpadHorizontal;
        public UIButton buttonDpadVertical;
        public UIButton buttonButtonsAsAxis;
    
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

            if (buttonLeftStickHorizontal == null)
                throw new Exception("buttonLeftStickHorizontal is null.");

            if (buttonLeftStickVertical == null)
                throw new Exception("buttonLeftStickVertical is null.");

            if (buttonRightStickHorizontal == null)
                throw new Exception("buttonRightStickHorizontal is null.");

            if (buttonRightStickVertical == null)
                throw new Exception("buttonRightStickVertical is null.");

            if (buttonLeftTrigger == null)
                throw new Exception("buttonLeftTrigger is null.");

            if (buttonRightTrigger == null)
                throw new Exception("buttonRightTrigger is null.");

            if (buttonDpadHorizontal == null)
                throw new Exception("buttonDpadHorizontal is null.");

            if (buttonDpadVertical == null)
                throw new Exception("buttonDpadVertical is null.");

            if (buttonButtonsAsAxis == null)
                throw new Exception("buttonButtonsAsAxis is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());

            buttonLeftStickHorizontal.onClick.AddListener(() => onSelect(new XboxJoystickAxisSource(XboxJoystickAxis.LeftStickX)));
            buttonLeftStickVertical.onClick.AddListener(() => onSelect(new XboxJoystickAxisSource(XboxJoystickAxis.LeftStickY)));
            buttonRightStickHorizontal.onClick.AddListener(() => onSelect(new XboxJoystickAxisSource(XboxJoystickAxis.RightStickX)));
            buttonRightStickVertical.onClick.AddListener(() => onSelect(new XboxJoystickAxisSource(XboxJoystickAxis.RightStickY)));
            buttonLeftTrigger.onClick.AddListener(() => onSelect(new XboxJoystickAxisSource(XboxJoystickAxis.LeftTrigger)));
            buttonRightTrigger.onClick.AddListener(() => onSelect(new XboxJoystickAxisSource(XboxJoystickAxis.RightTrigger)));

            buttonDpadHorizontal.onClick.AddListener(() => onSelect(new XboxJoystickDPadButtonAsAxisSource(DPadAsAxisSourceOrientation.Horizontal)));
            buttonDpadVertical.onClick.AddListener(() => onSelect(new XboxJoystickDPadButtonAsAxisSource(DPadAsAxisSourceOrientation.Vertical)));

            buttonButtonsAsAxis.onClick.AddListener(() =>
            {
                UISelectAxisFromJoystickButtonsWindow.controlName = controlName;
                UISelectAxisFromJoystickButtonsWindow.onCancel = () =>
                {
                    UISelectAxisFromJoystickButtonsWindow.instance.FadeOut(() => BackFadeIn());
                };
                UISelectAxisFromJoystickButtonsWindow.onSelect = (positiveButton, negativeButton) =>
                {
                    UISelectAxisFromJoystickButtonsWindow.instance.FadeOut(() =>
                    {
                        onSelect(new XboxJoystickButtonAsAxisSource(positiveButton, negativeButton));
                    });
                };

                FadeOut(() => UISelectAxisFromJoystickButtonsWindow.instance.FadeIn());
            });
        }
        protected override void OnBeforeFadeIn()
        {
            label.text = controlName;
            buttonCancel.MuteSelect();
        }
    }
}
