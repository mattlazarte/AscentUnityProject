using System;
using UnityEngine;

namespace Ascent.Managers.Input.Sources
{
    public class KeyboardButtonSource : ButtonSource
    {
        public MappableKeys keyCode { set; get; }

        public KeyboardButtonSource() { }

        public KeyboardButtonSource(MappableKeys keyCode)
        {
            this.keyCode = keyCode;
        }

        public override bool GetButtonCurrentState()
        {
            return UnityEngine.Input.GetKey((KeyCode)this.keyCode);
        }

        public override bool GetButtonCurrentDownState()
        {
            return UnityEngine.Input.GetKeyDown((KeyCode)this.keyCode);
        }

        public override bool GetButtonCurrentUpState()
        {
            return UnityEngine.Input.GetKeyUp((KeyCode)this.keyCode);
        }

        public override string GetDescription()
        {
            return string.Format(
                "Keyboard {0}",
                InputManagerHelpers.GetMappableKeysDescription(keyCode)
            );
        }
    }
}
