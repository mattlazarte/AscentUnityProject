using System;
using UnityEngine;
using XboxCtrlrInput;

namespace Ascent.Managers.Input.Sources
{
    public class XboxJoystickAxisSource : AxisSource
    {
        public XboxJoystickAxis axis;

        public XboxJoystickAxisSource() { }

        public XboxJoystickAxisSource(XboxJoystickAxis axis)
        {
            this.axis = axis;
        }

        public override float GetAxisCurrentState(float mouseSensibility, float joystickSensibility, bool invertVerticalAxis, bool isVertical)
        {
            // As XboxJoystickAxis is an exact copy of XboxAxis
            // in this implementation, I can just cast the enum.
            var value = XCI.GetAxis((XboxAxis)this.axis) * joystickSensibility;

            if (isVertical && invertVerticalAxis)
            {
                value *= -1;
            }

            return value;
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
