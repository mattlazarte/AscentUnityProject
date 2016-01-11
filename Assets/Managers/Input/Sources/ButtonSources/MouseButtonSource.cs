using System;
using UnityEngine;

namespace Ascent.Managers.Input.Sources
{
    public class MouseButtonSource : ButtonSource
    {
        public MouseButton button;

        public MouseButtonSource() { }

        public MouseButtonSource(MouseButton button)
        {
            this.button = button;
        }

        public override bool GetButtonCurrentState()
        {
            return UnityEngine.Input.GetMouseButton((int)this.button);
        }

        public override bool GetButtonCurrentDownState()
        {
            return UnityEngine.Input.GetMouseButtonDown((int)this.button);
        }

        public override bool GetButtonCurrentUpState()
        {
            return UnityEngine.Input.GetMouseButtonUp((int)this.button);
        }

        public override string GetDescription()
        {
            return string.Format(
                "Mouse Button {0}",
                InputManagerHelpers.GetMouseButtonDescription(button)
            );
        }
    }
}
