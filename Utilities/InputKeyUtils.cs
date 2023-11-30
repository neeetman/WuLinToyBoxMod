using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaxxToyBox.Utilities;
internal class InputKeyUtils
{
    public static string UniversalKeyName(KeyCode keyCode)
    {
        switch (keyCode) {
            case KeyCode.None: return "-";

            case KeyCode.Backspace: return "Backspace";
            case KeyCode.Tab: return "Tab";
            case KeyCode.Clear: return "Clear";
            case KeyCode.Return: return "Return";
            case KeyCode.Pause: return "Pause";
            case KeyCode.Escape: return "Esc";
            case KeyCode.Space: return "Space";
            case KeyCode.Exclaim: return "!";
            case KeyCode.DoubleQuote: return "\"";
            case KeyCode.Hash: return "#";
            case KeyCode.Dollar: return "$";
            case KeyCode.Percent: return "%";
            case KeyCode.Ampersand: return "&";
            case KeyCode.Quote: return "'";
            case KeyCode.LeftParen: return "(";
            case KeyCode.RightParen: return ")";
            case KeyCode.Asterisk: return "*";
            case KeyCode.Plus: return "+";
            case KeyCode.Comma: return ",";
            case KeyCode.Minus: return "-";
            case KeyCode.Period: return ".";
            case KeyCode.Slash: return "/";
            case KeyCode.Alpha0: return "0";
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";
            case KeyCode.Colon: return ":";
            case KeyCode.Semicolon: return ";";
            case KeyCode.Less: return "<";
            case KeyCode.Equals: return "=";
            case KeyCode.Greater: return ">";
            case KeyCode.Question: return "?";
            case KeyCode.At: return "@";
            case KeyCode.LeftBracket: return "[";
            case KeyCode.Backslash: return "\\";
            case KeyCode.RightBracket: return "]";
            case KeyCode.Caret: return "^";
            case KeyCode.Underscore: return "_";
            case KeyCode.BackQuote: return "`";
            case KeyCode.A: return "A";
            case KeyCode.B: return "B";
            case KeyCode.C: return "C";
            case KeyCode.D: return "D";
            case KeyCode.E: return "E";
            case KeyCode.F: return "F";
            case KeyCode.G: return "G";
            case KeyCode.H: return "H";
            case KeyCode.I: return "I";
            case KeyCode.J: return "J";
            case KeyCode.K: return "K";
            case KeyCode.L: return "L";
            case KeyCode.M: return "M";
            case KeyCode.N: return "N";
            case KeyCode.O: return "O";
            case KeyCode.P: return "P";
            case KeyCode.Q: return "Q";
            case KeyCode.R: return "R";
            case KeyCode.S: return "S";
            case KeyCode.T: return "T";
            case KeyCode.U: return "U";
            case KeyCode.V: return "V";
            case KeyCode.W: return "W";
            case KeyCode.X: return "X";
            case KeyCode.Y: return "Y";
            case KeyCode.Z: return "Z";
            case KeyCode.LeftCurlyBracket: return "{";
            case KeyCode.Pipe: return "|";
            case KeyCode.RightCurlyBracket: return "}";
            case KeyCode.Tilde: return "~";
            case KeyCode.Delete: return "Del";
            case KeyCode.Keypad0: return "0";
            case KeyCode.Keypad1: return "1";
            case KeyCode.Keypad2: return "2";
            case KeyCode.Keypad3: return "3";
            case KeyCode.Keypad4: return "4";
            case KeyCode.Keypad5: return "5";
            case KeyCode.Keypad6: return "6";
            case KeyCode.Keypad7: return "7";
            case KeyCode.Keypad8: return "8";
            case KeyCode.Keypad9: return "9";
            case KeyCode.KeypadPeriod: return ".";
            case KeyCode.KeypadDivide: return "/";
            case KeyCode.KeypadMultiply: return "*";
            case KeyCode.KeypadMinus: return "-";
            case KeyCode.KeypadPlus: return "+";
            case KeyCode.KeypadEnter: return "Enter";
            case KeyCode.KeypadEquals: return "=";
            case KeyCode.UpArrow: return "Up Arrow";
            case KeyCode.DownArrow: return "Down Arrow";
            case KeyCode.RightArrow: return "Right Arrow";
            case KeyCode.LeftArrow: return "Left Arrow";
            case KeyCode.Insert: return "Insert";
            case KeyCode.Home: return "Home";
            case KeyCode.End: return "End";
            case KeyCode.PageUp: return "Page Up";
            case KeyCode.PageDown: return "Page Down";
            case KeyCode.F1: return "F1";
            case KeyCode.F2: return "F2";
            case KeyCode.F3: return "F3";
            case KeyCode.F4: return "F4";
            case KeyCode.F5: return "F5";
            case KeyCode.F6: return "F6";
            case KeyCode.F7: return "F7";
            case KeyCode.F8: return "F8";
            case KeyCode.F9: return "F9";
            case KeyCode.F10: return "F10";
            case KeyCode.F11: return "F11";
            case KeyCode.F12: return "F12";
            case KeyCode.Numlock: return "NumLock";
            case KeyCode.CapsLock: return "CapsLock";
            case KeyCode.ScrollLock: return "ScrollLock";
            case KeyCode.RightShift: return "Shift";
            case KeyCode.LeftShift: return "Shift";
            case KeyCode.RightControl: return "Ctrl";
            case KeyCode.LeftControl: return "Ctrl";
            case KeyCode.RightAlt: return "Alt";
            case KeyCode.LeftAlt: return "Alt";
            case KeyCode.RightCommand: return "Cmd";
            case KeyCode.LeftCommand: return "Cmd";
            case KeyCode.LeftWindows: return "Win";
            case KeyCode.RightWindows: return "RightWindows";
            case KeyCode.AltGr: return "AltGr";
            case KeyCode.Help: return "Help";
            case KeyCode.Print: return "Print";
            case KeyCode.SysReq: return "SysReq";
            case KeyCode.Break: return "Break";
            case KeyCode.Menu: return "Menu";

            default:
                return keyCode.ToString();
        }
    }

    public static KeyCode[] KeyCodes;
    static void BuildKeyCodeCache()
    {
        if (KeyCodes == null) {
            KeyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        }
    }

    public static KeyCode GetUniversalKeyUp(bool excludeModifierKeys, bool excludeMouseButtons)
    {
        BuildKeyCodeCache();
        foreach (KeyCode keyCode in KeyCodes) {
            if (excludeModifierKeys) {
                if (keyCode == KeyCode.LeftShift) continue;
                if (keyCode == KeyCode.RightShift) continue;
                if (keyCode == KeyCode.Tab) continue;
                if (keyCode == KeyCode.LeftControl) continue;
                if (keyCode == KeyCode.RightControl) continue;
                if (keyCode == KeyCode.LeftCommand) continue;
                if (keyCode == KeyCode.RightCommand) continue;
                if (keyCode == KeyCode.LeftAlt) continue;
                if (keyCode == KeyCode.RightAlt) continue;
            }

            if (excludeMouseButtons) {
                if (keyCode == KeyCode.Mouse0) continue;
                if (keyCode == KeyCode.Mouse1) continue;
                if (keyCode == KeyCode.Mouse2) continue;
                if (keyCode == KeyCode.Mouse3) continue;
                if (keyCode == KeyCode.Mouse4) continue;
                if (keyCode == KeyCode.Mouse5) continue;
                if (keyCode == KeyCode.Mouse6) continue;
            }

            if (Input.GetKeyUp(keyCode)) {
                return keyCode;
            }
        }

        return KeyCode.None;
    }

    public static bool MouseUp()
    {
        return
           Input.GetMouseButtonUp(0)
        || Input.GetMouseButtonUp(1)
        || Input.GetMouseButtonUp(2)
        || Input.GetMouseButtonUp(3)
        || Input.GetMouseButtonUp(4)
        || Input.GetMouseButtonUp(5)
        || Input.GetMouseButtonUp(6);
    }

    public static bool AnyKey()
    {
        return Input.anyKey;
    }

    public static bool AnyKeyDown()
    {
        return Input.anyKeyDown;
    }

    public static KeyCode GetModifierKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) return KeyCode.LeftShift;
        if (Input.GetKeyDown(KeyCode.RightShift)) return KeyCode.RightShift;
        if (Input.GetKeyDown(KeyCode.Tab)) return KeyCode.Tab;
        if (Input.GetKeyDown(KeyCode.LeftControl)) return KeyCode.LeftControl;
        if (Input.GetKeyDown(KeyCode.RightControl)) return KeyCode.RightControl;
        if (Input.GetKeyDown(KeyCode.LeftCommand)) return KeyCode.LeftCommand;
        if (Input.GetKeyDown(KeyCode.RightCommand)) return KeyCode.RightCommand;
        if (Input.GetKeyDown(KeyCode.LeftAlt)) return KeyCode.LeftAlt;
        if (Input.GetKeyDown(KeyCode.RightAlt)) return KeyCode.RightAlt;

        return KeyCode.None;
    }

    public static KeyCode GetUniversalKeyDown(bool excludeModifierKeys, bool excludeMouseButtons)
    {
        BuildKeyCodeCache();
        foreach (KeyCode keyCode in KeyCodes) {
            if (excludeModifierKeys) {
                if (keyCode == KeyCode.LeftShift) continue;
                if (keyCode == KeyCode.RightShift) continue;
                if (keyCode == KeyCode.Tab) continue;
                if (keyCode == KeyCode.LeftControl) continue;
                if (keyCode == KeyCode.RightControl) continue;
                if (keyCode == KeyCode.LeftCommand) continue;
                if (keyCode == KeyCode.RightCommand) continue;
                if (keyCode == KeyCode.LeftAlt) continue;
                if (keyCode == KeyCode.RightAlt) continue;
            }

            if (excludeMouseButtons) {
                if (keyCode == KeyCode.Mouse0) continue;
                if (keyCode == KeyCode.Mouse1) continue;
                if (keyCode == KeyCode.Mouse2) continue;
                if (keyCode == KeyCode.Mouse3) continue;
                if (keyCode == KeyCode.Mouse4) continue;
                if (keyCode == KeyCode.Mouse5) continue;
                if (keyCode == KeyCode.Mouse6) continue;
            }

            if (Input.GetKeyDown(keyCode)) {
                return keyCode;
            }
        }

        return KeyCode.None;
    }

}
