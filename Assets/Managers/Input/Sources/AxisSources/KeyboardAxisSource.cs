using System;
using UnityEngine;

namespace Ascent.Managers.Input.Sources
{
    public class KeyboardAxisSource : AxisSource
    {
        public MappableKeys positiveKeyCode { set; get; }
        public MappableKeys negativeKeyCode { set; get; }

        public KeyboardAxisSource() { }

        public KeyboardAxisSource(MappableKeys positiveKeyCode, MappableKeys negativeKeyCode)
        {
            this.positiveKeyCode = positiveKeyCode;
            this.negativeKeyCode = negativeKeyCode;
        }

        public override float GetAxisCurrentState(float mouseSensibility, float joystickSensibility, bool invertVerticalAxis, bool isVertical)
        {
            var positive = UnityEngine.Input.GetKey((KeyCode)this.positiveKeyCode);
            var negative = UnityEngine.Input.GetKey((KeyCode)this.negativeKeyCode);

            var value = 0;

            if (positive && !negative)
            {
                value = 1;
            }
            else if (negative && !positive)
            {
                value = -1;
            }

            if (isVertical && invertVerticalAxis)
            {
                value *= -1;
            }

            return value;
        }

        public override string GetDescription()
        {
            return string.Format(
                "Keyboard {0}/{1}", 
                InputManagerHelpers.GetMappableKeysDescription(positiveKeyCode),
                InputManagerHelpers.GetMappableKeysDescription(negativeKeyCode)
            );
        }
    }
}