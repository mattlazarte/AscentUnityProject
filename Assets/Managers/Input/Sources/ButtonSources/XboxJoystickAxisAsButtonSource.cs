using System;
using UnityEngine;
using XboxCtrlrInput;

namespace Ascent.Managers.Input.Sources
{
    public class XboxJoystickAxisAsButtonSource : ButtonSource
    {
        public XboxJoystickAxis axis;

        private float minValueToOn = 0.2f;
        private bool state;
        private bool down;
        private bool up;

        public XboxJoystickAxisAsButtonSource() { }

        public XboxJoystickAxisAsButtonSource(XboxJoystickAxis axis)
        {
            this.axis = axis;
            this.state = false;
            this.down = false;
            this.up = false;
        }

        public override void Update()
        {
            // As XboxJoystickAxis is an exact copy of XboxAxis
            // in this implementation, I can just cast the enum.
            var currValue = XCI.GetAxis((XboxAxis)this.axis);
            var newState = currValue >= minValueToOn;

            down = !state && newState;
            up = state && !newState;

            state = newState;
        }

        public override bool GetButtonCurrentState()
        {
            return state;
        }

        public override bool GetButtonCurrentDownState()
        {
            return down;
        }

        public override bool GetButtonCurrentUpState()
        {
            return up;
        }

        public override string GetDescription()
        {
            return string.Format(
                "Joystick {0}",
                InputManagerHelpers.GetXboxJoystickAxisDescription(axis)
            );
        }
    }
}
