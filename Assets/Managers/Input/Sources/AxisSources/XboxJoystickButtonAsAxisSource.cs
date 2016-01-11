using System;
using UnityEngine;
using XboxCtrlrInput;

namespace Ascent.Managers.Input.Sources
{
    public class XboxJoystickButtonAsAxisSource : AxisSource
    {
        public XboxJoystickButtons positiveButton { set; get; }
        public XboxJoystickButtons negativeButton { set; get; }

        public XboxJoystickButtonAsAxisSource() { }

        public XboxJoystickButtonAsAxisSource(XboxJoystickButtons positiveButton, XboxJoystickButtons negativeButton)
        {
            this.positiveButton = positiveButton;
            this.negativeButton = negativeButton;
        }

        public override float GetAxisCurrentState(float mouseSensibility, float joystickSensibility, bool invertVerticalAxis, bool isVertical)
        {
            // As XboxJoystickButtons is an exact copy of XboxButton
            // in this implementation, I can just cast the enum.
            var positive = XCI.GetButton((XboxButton)this.positiveButton);
            var negative = XCI.GetButton((XboxButton)this.negativeButton);
            var value = 0;

            if (positive && !negative)
            {
                value = -1;
            }
            else if (negative && !positive)
            {
                value = 1;
            }

            if (invertVerticalAxis)
            {
                value *= -1;
            }

            return value;
        }

        public override string GetDescription()
        {
            return string.Format(
                "Joystick {0}/{1}",
                InputManagerHelpers.GetXboxJoystickButtonsDescription(positiveButton),
                InputManagerHelpers.GetXboxJoystickButtonsDescription(negativeButton)
            );
        }
    }
}
