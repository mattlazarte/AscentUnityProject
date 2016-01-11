using System;
using UnityEngine;
using XboxCtrlrInput;

namespace Ascent.Managers.Input.Sources
{
    public enum DPadAsAxisSourceOrientation
    {
        Horizontal,
        Vertical
    }

    public class XboxJoystickDPadButtonAsAxisSource : AxisSource
    {
        public XboxJoystickDPad positiveButton { set; get; }
        public XboxJoystickDPad negativeButton { set; get; }
    
        public XboxJoystickDPadButtonAsAxisSource() { }
    
        public XboxJoystickDPadButtonAsAxisSource(DPadAsAxisSourceOrientation orientation)
        {
            if (orientation == DPadAsAxisSourceOrientation.Vertical)
            {
                this.positiveButton = XboxJoystickDPad.Up;
                this.negativeButton = XboxJoystickDPad.Down;
            }
            else
            {
                this.positiveButton = XboxJoystickDPad.Right;
                this.negativeButton = XboxJoystickDPad.Left;
            }
        }

        public override float GetAxisCurrentState(float mouseSensibility, float joystickSensibility, bool invertVerticalAxis, bool isVertical)
        {
            // As XboxJoystickButtons is an exact copy of XboxButton
            // in this implementation, I can just cast the enum.
            var positive = XCI.GetDPad((XboxDPad)this.positiveButton);
            var negative = XCI.GetDPad((XboxDPad)this.negativeButton);
            var value = 0;
    
            if (positive && !negative)
            {
                value = 1;
            }
            else if (negative && !positive)
            {
                value = -1;
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
                "Joystick D-Pad {0}/{1}",
                InputManagerHelpers.GetXboxJoystickDPadDescription(positiveButton),
                InputManagerHelpers.GetXboxJoystickDPadDescription(negativeButton)
            );
        }
    }
}
