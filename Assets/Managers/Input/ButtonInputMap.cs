using System;
using UnityEngine;

namespace Ascent.Managers.Input
{
    public class ButtonInputMap
    {
        public ButtonState StateToCapture;
        public ButtonSource PrimarySource;
        public ButtonSource SecondarySource;

        public ButtonInputMap()
        {
        }
    
        public ButtonInputMap(ButtonState stateToCapture, ButtonSource primarySource, ButtonSource secondarySource = null)
        {
            if (primarySource == null)
                throw new ArgumentException("A button input must have at least the primary source.");
    
            this.StateToCapture = stateToCapture;
            this.PrimarySource = primarySource;
            this.SecondarySource = secondarySource;
        }
    
        public bool GetButtonCurrentState()
        {
            switch (StateToCapture)
            {
                case ButtonState.Down:
                    return
                        (PrimarySource != null && PrimarySource.GetButtonCurrentDownState()) ||
                        (SecondarySource != null && SecondarySource.GetButtonCurrentDownState());
    
                case ButtonState.On:
                    return
                        (PrimarySource != null && PrimarySource.GetButtonCurrentState()) ||
                        (SecondarySource != null && SecondarySource.GetButtonCurrentState());
    
                case ButtonState.Up:
                    return
                        (PrimarySource != null && PrimarySource.GetButtonCurrentUpState()) ||
                        (SecondarySource != null && SecondarySource.GetButtonCurrentUpState());
    
                default:
                    throw new Exception("Unexpected state to capture.");
            }
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
