using System;

namespace Ascent.Managers.Input
{
    public abstract class AxisSource
    {
        public abstract float GetAxisCurrentState(float mouseSensibility, float joystickSensibility, bool invertVerticalAxis, bool isVertical);
        public abstract string GetDescription();
        public virtual void Update() { }
    }
}
