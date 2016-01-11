using System;
using UnityEngine;

namespace Ascent.Managers.Input
{
    public class AxisInputMap
    {
        public bool isVertical;
        public AxisSource PrimarySource;
        public AxisSource SecondarySource;

        public AxisInputMap()
        {
        }

        public AxisInputMap(bool isVertical, AxisSource primarySource, AxisSource secondarySource = null)
        {
            if (primarySource == null)
                throw new Exception("An axis must have at least the primary source.");

            this.isVertical = isVertical;
            this.PrimarySource = primarySource;
            this.SecondarySource = secondarySource;
        }

        public float GetAxisCurrentState(float mouseSensibility, float joystickSensibility, bool invertVerticalAxis)
        {
            var val = 0f;
    
            if (PrimarySource != null)
            {
                val = PrimarySource.GetAxisCurrentState(mouseSensibility, joystickSensibility, invertVerticalAxis, isVertical);
            }
    
            if (val == 0 && SecondarySource != null)
            {
                val = SecondarySource.GetAxisCurrentState(mouseSensibility, joystickSensibility, invertVerticalAxis, isVertical);
            }
    
            return val;
        }

        public string GetPrimarySourceDescription()
        {
            if (PrimarySource != null)
            {
                return PrimarySource.GetDescription();
            }
            else
            {
                return "Not Assigned";
            }
        }

        public string GetSecondarySourceDescription()
        {
            if (SecondarySource != null)
            {
                return SecondarySource.GetDescription();
            }
            else
            {
                return "Not Assigned";
            }
        }
    }
}
