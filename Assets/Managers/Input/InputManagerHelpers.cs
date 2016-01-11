using System;
using UnityEngine;

namespace Ascent.Managers.Input
{
    public static class InputManagerHelpers
    {
        public static string GetMouseButtonDescription(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left: return "Left";
                case MouseButton.Right: return "Right";
                case MouseButton.Middle: return "Middle";

                default:
                    throw new Exception("Unexpected MouseButton value.");
            }
        }

        public static string GetMappableKeysDescription(MappableKeys key)
        {
            switch (key)
            {
                case MappableKeys.Alpha1: return "1";
                case MappableKeys.Alpha2: return "2";
                case MappableKeys.Alpha3: return "3";
                case MappableKeys.Alpha4: return "4";
                case MappableKeys.Alpha5: return "5";
                case MappableKeys.Alpha6: return "6";
                case MappableKeys.Alpha7: return "7";
                case MappableKeys.Alpha8: return "8";
                case MappableKeys.Alpha9: return "9";
                case MappableKeys.Alpha0: return "0";
                case MappableKeys.Keypad1: return "Keypad 1";
                case MappableKeys.Keypad2: return "Keypad 2";
                case MappableKeys.Keypad3: return "Keypad 3";
                case MappableKeys.Keypad4: return "Keypad 4";
                case MappableKeys.Keypad5: return "Keypad 5";
                case MappableKeys.Keypad6: return "Keypad 6";
                case MappableKeys.Keypad7: return "Keypad 7";
                case MappableKeys.Keypad8: return "Keypad 8";
                case MappableKeys.Keypad9: return "Keypad 9";
                case MappableKeys.Keypad0: return "Keypad 0";
                case MappableKeys.KeypadPeriod: return "Keypad Period";
                case MappableKeys.KeypadDivide: return "Keypad Divide";
                case MappableKeys.KeypadMultiply: return "Keypad Multiply";
                case MappableKeys.KeypadMinus: return "Keypad Minus";
                case MappableKeys.KeypadPlus: return "Keypad Plus";
                case MappableKeys.KeypadEnter: return "Keypad Return";
                case MappableKeys.KeypadEquals: return "Keypad Equals";
                case MappableKeys.Q: return "Q";
                case MappableKeys.W: return "W";
                case MappableKeys.E: return "E";
                case MappableKeys.R: return "R";
                case MappableKeys.T: return "T";
                case MappableKeys.Y: return "Y";
                case MappableKeys.U: return "U";
                case MappableKeys.I: return "I";
                case MappableKeys.O: return "O";
                case MappableKeys.P: return "P";
                case MappableKeys.A: return "A";
                case MappableKeys.S: return "S";
                case MappableKeys.D: return "D";
                case MappableKeys.F: return "F";
                case MappableKeys.G: return "G";
                case MappableKeys.H: return "H";
                case MappableKeys.J: return "J";
                case MappableKeys.K: return "K";
                case MappableKeys.L: return "L";
                case MappableKeys.Z: return "Z";
                case MappableKeys.X: return "X";
                case MappableKeys.C: return "C";
                case MappableKeys.V: return "V";
                case MappableKeys.B: return "B";
                case MappableKeys.N: return "N";
                case MappableKeys.M: return "M";
                case MappableKeys.Escape: return "Escape";
                case MappableKeys.BackQuote: return "Back Quote";
                case MappableKeys.Tab: return "Tab";
                case MappableKeys.CapsLock: return "Caps Lock";
                case MappableKeys.LeftShift: return "Left Shift";
                case MappableKeys.RightShift: return "Right Shift";
                case MappableKeys.LeftControl: return "Left Control";
                case MappableKeys.RightControl: return "Right Control";
                case MappableKeys.LeftAlt: return "Left Alt";
                case MappableKeys.RightAlt: return "Right Alt";
                case MappableKeys.Backspace: return "Backspace";
                case MappableKeys.Minus: return "Minus";
                case MappableKeys.Equals: return "Equals";
                case MappableKeys.LeftBracket: return "Left Bracket";
                case MappableKeys.RightBracket: return "Right Bracket";
                case MappableKeys.Semicolon: return "Semicolon";
                case MappableKeys.Quote: return "Quote";
                case MappableKeys.Comma: return "Comma";
                case MappableKeys.Period: return "Period";
                case MappableKeys.Slash: return "Slash";
                case MappableKeys.Backslash: return "Backslash";
                case MappableKeys.Return: return "Return";
                case MappableKeys.UpArrow: return "Up";
                case MappableKeys.DownArrow: return "Down";
                case MappableKeys.LeftArrow: return "Left";
                case MappableKeys.RightArrow: return "Right";
                case MappableKeys.Insert: return "Insert";
                case MappableKeys.Home: return "Home";
                case MappableKeys.Delete: return "Delete";
                case MappableKeys.End: return "End";
                case MappableKeys.PageUp: return "Page Up";
                case MappableKeys.PageDown: return "Page Down";
                case MappableKeys.F1: return "F1";
                case MappableKeys.F2: return "F2";
                case MappableKeys.F3: return "F3";
                case MappableKeys.F4: return "F4";
                case MappableKeys.F5: return "F5";
                case MappableKeys.F6: return "F6";
                case MappableKeys.F7: return "F7";
                case MappableKeys.F8: return "F8";
                case MappableKeys.F9: return "F9";
                case MappableKeys.F10: return "F10";
                case MappableKeys.F11: return "F11";
                case MappableKeys.F12: return "F12";
                case MappableKeys.Space: return "Space";

                default:
                    throw new Exception("Unexpected MappableKeys value: " + key.ToString());
            }
        }

        public static string GetXboxJoystickButtonsDescription(XboxJoystickButtons button)
        {
            switch (button)
            {
                case XboxJoystickButtons.A: return "A";
                case XboxJoystickButtons.B: return "B";
                case XboxJoystickButtons.X: return "X";
                case XboxJoystickButtons.Y: return "Y";
                case XboxJoystickButtons.Start: return "Start";
                case XboxJoystickButtons.Back: return "Back";
                case XboxJoystickButtons.LeftStick: return "Left Stick";
                case XboxJoystickButtons.RightStick: return "Right Stick";
                case XboxJoystickButtons.LeftBumper: return "Left Bumper";
                case XboxJoystickButtons.RightBumper: return "Right Bumper";

                default:
                    throw new Exception("Unexpected XboxJoystickButtons value.");
            }
        }

        public static string GetXboxJoystickAxisDescription(XboxJoystickAxis axis)
        {
            switch (axis)
            {
                case XboxJoystickAxis.LeftStickX: return "Left Stick Horizontal";
                case XboxJoystickAxis.LeftStickY: return "Left Stick Vertical";
                case XboxJoystickAxis.RightStickX: return "Right Stick Horizontal";
                case XboxJoystickAxis.RightStickY: return "Right Stick Vertical";
                case XboxJoystickAxis.LeftTrigger: return "Left Trigger";
                case XboxJoystickAxis.RightTrigger: return "Right Trigger";

                default:
                    throw new Exception("Unexpected XboxJoystickAxis value.");
            }
        }

        public static string GetXboxJoystickDPadDescription(XboxJoystickDPad button)
        {
            switch (button)
            {
                case XboxJoystickDPad.Up: return "Up";
                case XboxJoystickDPad.Down: return "Down";
                case XboxJoystickDPad.Left: return "Left";
                case XboxJoystickDPad.Right: return "Right";

                default:
                    throw new Exception("Unexpected XboxJoystickDPad value.");
            }
        }
    }
}
