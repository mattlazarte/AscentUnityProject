using System;
using UnityEngine;
using XboxCtrlrInput;

namespace Ascent.Managers.Input.Sources
{
    public class XboxJoystickDPadButtonSource : ButtonSource
    {
        public XboxJoystickDPad button;

        public XboxJoystickDPadButtonSource() {}

        public XboxJoystickDPadButtonSource(XboxJoystickDPad button)
        {
            this.button = button;
        }

        public override bool GetButtonCurrentState()
        {
            // As XboxJoystickDPad is an exact copy of XboxDPad
            // in this implementation, I can just cast the enum.
            return XCI.GetDPad((XboxDPad)this.button);
        }

        public override bool GetButtonCurrentDownState()
        {
            // As XboxJoystickDPad is an exact copy of XboxDPad
            // in this implementation, I can just cast the enum.
            return XCI.GetDPadDown((XboxDPad)this.button);
        }

        public override bool GetButtonCurrentUpState()
        {
            // As XboxJoystickDPad is an exact copy of XboxDPad
            // in this implementation, I can just cast the enum.
            return XCI.GetDPadUp((XboxDPad)this.button);
        }

        public override string GetDescription()
        {
            return string.Format(
                "Joystick D-Pad {0}",
                InputManagerHelpers.GetXboxJoystickDPadDescription(button)
            );
        }
    }
}
