using Ascent.Managers.Input;
using System;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UISelectJoystickButtonWindow : UIWindow
    {
        public enum JoystickButtonSelectionType
        {
            Button, DPad
        }

        public class JoystickButtonSelection
        {
            public JoystickButtonSelection(XboxJoystickButtons button)
            {
                this.button = button;
                this.type = JoystickButtonSelectionType.Button;
            }

            public JoystickButtonSelection(XboxJoystickDPad dpadButton)
            {
                this.dpadButton = dpadButton;
                this.type = JoystickButtonSelectionType.DPad;
            }

            public JoystickButtonSelectionType type;
            public XboxJoystickButtons button;
            public XboxJoystickDPad dpadButton;
        }

        public static bool showDPadButtons;
        public static UISelectJoystickButtonWindow instance;
        public static string controlName;
        public static Action onCancel;
        public static Action<JoystickButtonSelection> onSelect;
    
        public Text label;
        public UIButton buttonCancel;
        public UIButton buttonA;
        public UIButton buttonB;
        public UIButton buttonX;
        public UIButton buttonY;
        public UIButton buttonStart;
        public UIButton buttonBack;
        public UIButton buttonLeftStick;
        public UIButton buttonRightStick;
        public UIButton buttonLeftBumper;
        public UIButton buttonRightBumper;
        public UIButton buttonDPadUp;
        public UIButton buttonDPadDown;
        public UIButton buttonDPadLeft;
        public UIButton buttonDPadRight;
    
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

            if (buttonA == null)
                throw new Exception("buttonA is null.");

            if (buttonB == null)
                throw new Exception("buttonB is null.");

            if (buttonX == null)
                throw new Exception("buttonX is null.");

            if (buttonY == null)
                throw new Exception("buttonY is null.");

            if (buttonStart == null)
                throw new Exception("buttonStart is null.");

            if (buttonBack == null)
                throw new Exception("buttonBack is null.");

            if (buttonLeftStick == null)
                throw new Exception("buttonLeftStick is null.");

            if (buttonRightStick == null)
                throw new Exception("buttonRightStick is null.");

            if (buttonLeftBumper == null)
                throw new Exception("buttonLeftBumper is null.");

            if (buttonRightBumper == null)
                throw new Exception("buttonRightBumper is null.");

            if (buttonDPadUp == null)
                throw new Exception("buttonDPadUp is null.");

            if (buttonDPadDown == null)
                throw new Exception("buttonDPadDown is null.");

            if (buttonDPadLeft == null)
                throw new Exception("buttonDPadLeft is null.");

            if (buttonDPadRight == null)
                throw new Exception("buttonDPadRight is null.");
        }
        protected void InitUI()
        {
            buttonCancel.onClick.AddListener(() => onCancel());
            buttonA.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.A)));
            buttonB.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.B)));
            buttonX.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.X)));
            buttonY.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.Y)));
            buttonStart.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.Start)));
            buttonBack.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.Back)));
            buttonLeftStick.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.LeftStick)));
            buttonRightStick.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.RightStick)));
            buttonLeftBumper.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.LeftBumper)));
            buttonRightBumper.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickButtons.RightBumper)));
            buttonDPadUp.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickDPad.Up)));
            buttonDPadDown.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickDPad.Down)));
            buttonDPadLeft.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickDPad.Left)));
            buttonDPadRight.onClick.AddListener(() => onSelect(new JoystickButtonSelection(XboxJoystickDPad.Right)));
        }
        protected override void OnBeforeFadeIn()
        {
            buttonDPadUp.gameObject.SetActive(showDPadButtons);
            buttonDPadDown.gameObject.SetActive(showDPadButtons);
            buttonDPadLeft.gameObject.SetActive(showDPadButtons);
            buttonDPadRight.gameObject.SetActive(showDPadButtons);

            label.text = controlName;
            buttonCancel.MuteSelect();
        }
    }
}
