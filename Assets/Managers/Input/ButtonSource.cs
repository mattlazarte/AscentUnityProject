
using System;
namespace Ascent.Managers.Input
{
    public abstract class ButtonSource
    {
        public abstract bool GetButtonCurrentState();
        public abstract bool GetButtonCurrentDownState();
        public abstract bool GetButtonCurrentUpState();

        public virtual void Update() { }

        public abstract string GetDescription();
    }
}
