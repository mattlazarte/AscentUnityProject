using System;
using UnityEngine;
using XboxCtrlrInput;

namespace Ascent.Managers.Input.Sources
{
    public class XboxJoystickButtonSource : ButtonSource
    {
        public XboxJoystickButtons button;

        public XboxJoystickButtonSource(XboxJoystickButtons button)
        {
            this.button = button;
        }

        public override bool GetButtonCurrentState()
        {
            // As XboxJoystickButtons is an exact copy of XboxButton
            // in this implementation, I can just cast the enum.
            return XCI.GetButton((XboxButton)this.button);
        }

        public override bool GetButtonCurrentDownState()
        {
            // As XboxJoystickButtons is an exact copy of XboxButton
            // in this implementation, I can just cast the enum.
            return XCI.GetButtonDown((XboxButton)this.button);
        }

        public override bool GetButtonCurrentUpState()
        {
            // As XboxJoystickButtons is an exact copy of XboxButton
            // in this implementation, I can just cast the enum.
            return XCI.GetButtonUp((XboxButton)this.button);
        }

        public override string GetDescription()
        {
            return string.Format(
                "Joystick {0}",
                InputManagerHelpers.GetXboxJoystickButtonsDescription(button)
            );
        }
    }
}
