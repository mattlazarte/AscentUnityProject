using System;
using UnityEngine;

namespace Ascent.Managers.Input.Sources
{
    public class MouseAxisSource : AxisSource
    {

        public MouseAxis axis;
        public String axisName;
        public String description;

        public MouseAxisSource() { }

        public MouseAxisSource(MouseAxis axis)
        {
            this.axis = axis;

            switch (axis)
            {
                case MouseAxis.Horizontal:
                    axisName = "Mouse X";
                    description = "Mouse Horizontal";
                    break;

                case MouseAxis.Vertical:
                    axisName = "Mouse Y";
                    description = "Mouse Vertical";
                    break;

                default:
                    throw new Exception("Unexpected axis value.");
            }
        }

        public override float GetAxisCurrentState(float mouseSensibility, float joystickSensibility, bool invertVerticalAxis, bool isVertical)
        {
            var value = UnityEngine.Input.GetAxis(axisName) * mouseSensibility;

            if (isVertical && invertVerticalAxis)
            {
                value *= -1;
            }

            return value;
        }

        public override string GetDescription()
        {
            return description;
        }
    }
}
