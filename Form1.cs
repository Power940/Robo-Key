using Gma.System.MouseKeyHook;
using WindowsInput;

namespace RoboKey
{
    public partial class Form1 : Form
    {
        private static InputSimulator inputSim = new();
        private static IKeyboardMouseEvents keyboardMouseHook;

        private static Keys holdKeysKey;
        private static Keys autoClickKeysKey;
        private static Keys runCommandsKey;

        private static int autoClickIntervalMS = 1;
        private static HashSet<string> loggedKeysText = [];
        private static readonly HashSet<Keys> pressedKeys = [];
        private static readonly List<string> commandList = [];

        private static bool holdKeysActive = false;
        private static bool autoClickKeysActive = false;
        private static bool runCommandsActive = false;
        private static bool runCommandsToggleMode = true;

        private static readonly Thread holdKeysThread = new(HoldKeysMethod);
        private static readonly Thread autoClickThread = new(AutoClickKeysMethod);
        private static readonly Thread commandKeysThread = new(CommandKeysMethod);

        private static readonly Dictionary<string, (Keys key, Action down, Action up, Action press)> KeyConverter = new()
        {
            { "lmouse", (Keys.LButton, () => inputSim.Mouse.LeftButtonDown(), () => inputSim.Mouse.LeftButtonUp(), () => inputSim.Mouse.LeftButtonClick()) },
            { "mmouse", (Keys.MButton, () => inputSim.Mouse.MiddleButtonDown(), () => inputSim.Mouse.MiddleButtonUp(), () => inputSim.Mouse.MiddleButtonClick()) },
            { "rmouse", (Keys.RButton, () => inputSim.Mouse.RightButtonDown(), () => inputSim.Mouse.RightButtonUp(), () => inputSim.Mouse.RightButtonClick()) },

            { "backspace", (Keys.Back, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BACK), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BACK), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BACK)) },
            { "tab", (Keys.Tab, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.TAB), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.TAB), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.TAB)) },
            { "enter", (Keys.Enter, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.RETURN), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.RETURN), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN)) },
            { "escape", (Keys.Escape, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.ESCAPE), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.ESCAPE), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.ESCAPE)) },
            { "space", (Keys.Space, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.SPACE), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.SPACE), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.SPACE)) },
            { "capslock", (Keys.CapsLock, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.CAPITAL), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.CAPITAL), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.CAPITAL)) },

            { "lshift", (Keys.LShiftKey, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.LSHIFT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.LSHIFT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.LSHIFT)) },
            { "rshift", (Keys.RShiftKey, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.RSHIFT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.RSHIFT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.RSHIFT)) },
            { "lcontrol", (Keys.LControlKey, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.LCONTROL), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.LCONTROL), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.LCONTROL)) },
            { "rcontrol", (Keys.RControlKey, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.RCONTROL), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.RCONTROL), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.RCONTROL)) },
            { "lalt", (Keys.LMenu, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.LMENU), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.LMENU), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.LMENU)) },
            { "ralt", (Keys.RMenu, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.RMENU), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.RMENU), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.RMENU)) },

            { "pageup", (Keys.PageUp, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.PRIOR), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.PRIOR), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.PRIOR)) },
            { "pagedown", (Keys.PageDown, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NEXT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NEXT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NEXT)) },
            { "end", (Keys.End, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.END), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.END), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.END)) },
            { "home", (Keys.Home, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.HOME), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.HOME), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.HOME)) },

            { "leftarrow", (Keys.Left, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.LEFT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.LEFT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.LEFT)) },
            { "uparrow", (Keys.Up, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.UP), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.UP), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.UP)) },
            { "rightarrow", (Keys.Right, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.RIGHT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.RIGHT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.RIGHT)) },
            { "downarrow", (Keys.Down, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.DOWN), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.DOWN), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.DOWN)) },

            { "insert", (Keys.Insert, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.INSERT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.INSERT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.INSERT)) },
            { "delete", (Keys.Delete, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.DELETE), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.DELETE), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.DELETE)) },
            { "printscreen", (Keys.PrintScreen, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.SNAPSHOT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.SNAPSHOT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.SNAPSHOT)) },

            { "leftwindows", (Keys.LWin, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.LWIN), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.LWIN), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.LWIN)) },
            { "rightwindows", (Keys.RWin, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.RWIN), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.RWIN), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.RWIN)) },

            { "0", (Keys.D0, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_0), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_0), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_0)) },
            { "1", (Keys.D1, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_1), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_1), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_1)) },
            { "2", (Keys.D2, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_2), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_2), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_2)) },
            { "3", (Keys.D3, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_3), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_3), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_3)) },
            { "4", (Keys.D4, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_4), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_4), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_4)) },
            { "5", (Keys.D5, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_5), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_5), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_5)) },
            { "6", (Keys.D6, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_6), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_6), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_6)) },
            { "7", (Keys.D7, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_7), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_7), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_7)) },
            { "8", (Keys.D8, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_8), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_8), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_8)) },
            { "9", (Keys.D9, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_9), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_9), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_9)) },

            { "a", (Keys.A, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_A), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_A), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_A)) },
            { "b", (Keys.B, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_B), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_B), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_B)) },
            { "c", (Keys.C, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_C), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_C), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_C)) },
            { "d", (Keys.D, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_D), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_D), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_D)) },
            { "e", (Keys.E, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_E), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_E), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_E)) },
            { "f", (Keys.F, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_F), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_F), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_F)) },
            { "g", (Keys.G, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_G), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_G), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_G)) },
            { "h", (Keys.H, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_H), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_H), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_H)) },
            { "i", (Keys.I, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_I), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_I), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_I)) },
            { "j", (Keys.J, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_J), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_J), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_J)) },
            { "k", (Keys.K, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_K), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_K), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_K)) },
            { "l", (Keys.L, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_L), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_L), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_L)) },
            { "m", (Keys.M, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_M), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_M), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_M)) },
            { "n", (Keys.N, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_N), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_N), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_N)) },
            { "o", (Keys.O, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_O), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_O), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_O)) },
            { "p", (Keys.P, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_P), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_P), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_P)) },
            { "q", (Keys.Q, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_Q), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_Q), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_Q)) },
            { "r", (Keys.R, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_R), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_R), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_R)) },
            { "s", (Keys.S, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_S), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_S), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_S)) },
            { "t", (Keys.T, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_T), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_T), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_T)) },
            { "u", (Keys.U, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_U), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_U), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_U)) },
            { "v", (Keys.V, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_V), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_V), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_V)) },
            { "w", (Keys.W, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_W), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_W), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_W)) },
            { "x", (Keys.X, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_X), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_X), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_X)) },
            { "y", (Keys.Y, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_Y), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_Y), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_Y)) },
            { "z", (Keys.Z, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VK_Z), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VK_Z), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VK_Z)) },

            { "keypad0", (Keys.NumPad0, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD0), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD0), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD0)) },
            { "keypad1", (Keys.NumPad1, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD1), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD1), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD1)) },
            { "keypad2", (Keys.NumPad2, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD2), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD2), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD2)) },
            { "keypad3", (Keys.NumPad3, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD3), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD3), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD3)) },
            { "keypad4", (Keys.NumPad4, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD4), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD4), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD4)) },
            { "keypad5", (Keys.NumPad5, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD5), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD5), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD5)) },
            { "keypad6", (Keys.NumPad6, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD6), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD6), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD6)) },
            { "keypad7", (Keys.NumPad7, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD7), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD7), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD7)) },
            { "keypad8", (Keys.NumPad8, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD8), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD8), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD8)) },
            { "keypad9", (Keys.NumPad9, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMPAD9), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMPAD9), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9)) },

            { "multiply", (Keys.Multiply, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.MULTIPLY), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.MULTIPLY), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.MULTIPLY)) },
            { "add", (Keys.Add, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.ADD), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.ADD), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.ADD)) },
            { "subtract", (Keys.Subtract, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.SUBTRACT), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.SUBTRACT), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.SUBTRACT)) },
            { "divide", (Keys.Divide, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.DIVIDE), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.DIVIDE), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.DIVIDE)) },
            { "decimal", (Keys.Decimal, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.DECIMAL), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.DECIMAL), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.DECIMAL)) },

            { "f1", (Keys.F1, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F1), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F1), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F1)) },
            { "f2", (Keys.F2, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F2), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F2), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F2)) },
            { "f3", (Keys.F3, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F3), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F3), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F3)) },
            { "f4", (Keys.F4, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F4), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F4), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F4)) },
            { "f5", (Keys.F5, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F5), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F5), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F5)) },
            { "f6", (Keys.F6, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F6), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F6), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F6)) },
            { "f7", (Keys.F7, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F7), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F7), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F7)) },
            { "f8", (Keys.F8, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F8), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F8), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F8)) },
            { "f9", (Keys.F9, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F9), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F9), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F9)) },
            { "f10", (Keys.F10, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F10), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F10), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F10)) },
            { "f11", (Keys.F11, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F11), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F11), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F11)) },
            { "f12", (Keys.F12, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F12), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F12), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F12)) },
            { "f13", (Keys.F13, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F13), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F13), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F13)) },
            { "f14", (Keys.F14, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F14), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F14), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F14)) },
            { "f15", (Keys.F15, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F15), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F15), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F15)) },
            { "f16", (Keys.F16, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F16), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F16), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F16)) },
            { "f17", (Keys.F17, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F17), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F17), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F17)) },
            { "f18", (Keys.F18, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F18), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F18), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F18)) },
            { "f19", (Keys.F19, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F19), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F19), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F19)) },
            { "f20", (Keys.F20, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F20), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F20), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F20)) },
            { "f21", (Keys.F21, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F21), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F21), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F21)) },
            { "f22", (Keys.F22, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F22), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F22), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F22)) },
            { "f23", (Keys.F23, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F23), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F23), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F23)) },
            { "f24", (Keys.F24, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.F24), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.F24), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.F24)) },

            { "numlock", (Keys.NumLock, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.NUMLOCK), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.NUMLOCK), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.NUMLOCK)) },
            { "scrolllock", (Keys.Scroll, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.SCROLL), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.SCROLL), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.SCROLL)) },

            { "browserback", (Keys.BrowserBack, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BROWSER_BACK), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BROWSER_BACK), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BROWSER_BACK)) },
            { "browserforward", (Keys.BrowserForward, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BROWSER_FORWARD), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BROWSER_FORWARD), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BROWSER_FORWARD)) },
            { "browserrefresh", (Keys.BrowserRefresh, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BROWSER_REFRESH), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BROWSER_REFRESH), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BROWSER_REFRESH)) },
            { "browserstop", (Keys.BrowserStop, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BROWSER_STOP), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BROWSER_STOP), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BROWSER_STOP)) },
            { "browsersearch", (Keys.BrowserSearch, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BROWSER_SEARCH), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BROWSER_SEARCH), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BROWSER_SEARCH)) },
            { "browserfavorites", (Keys.BrowserFavorites, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BROWSER_FAVORITES), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BROWSER_FAVORITES), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BROWSER_FAVORITES)) },
            { "browserstart", (Keys.BrowserHome, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.BROWSER_HOME), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.BROWSER_HOME), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.BROWSER_HOME)) },

            { "volumemute", (Keys.VolumeMute, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VOLUME_MUTE), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VOLUME_MUTE), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_MUTE)) },
            { "volumedown", (Keys.VolumeDown, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VOLUME_DOWN), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VOLUME_DOWN), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_DOWN)) },
            { "volumeup", (Keys.VolumeUp, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.VOLUME_UP), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.VOLUME_UP), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.VOLUME_UP)) },
            { "nexttrack", (Keys.MediaNextTrack, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.MEDIA_NEXT_TRACK), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.MEDIA_NEXT_TRACK), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.MEDIA_NEXT_TRACK)) },
            { "previoustrack", (Keys.MediaPreviousTrack, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.MEDIA_PREV_TRACK), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.MEDIA_PREV_TRACK), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PREV_TRACK)) },
            { "play/pausemedia", (Keys.MediaPlayPause, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.MEDIA_PLAY_PAUSE), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.MEDIA_PLAY_PAUSE), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE)) },
            { "startmail", (Keys.LaunchMail, () => inputSim.Keyboard.KeyDown(VirtualKeyCode.LAUNCH_MAIL), () => inputSim.Keyboard.KeyUp(VirtualKeyCode.LAUNCH_MAIL), () => inputSim.Keyboard.KeyPress(VirtualKeyCode.LAUNCH_MAIL)) }
        };


        public Form1()
        {
            InitializeComponent();
            keyboardMouseHook = Hook.GlobalEvents();
            keyboardMouseHook.KeyDown += HookOnKeyDown;
            keyboardMouseHook.MouseDown += HookOnMouseDown;
            keyboardMouseHook.KeyUp += HookOnKeyUp;
            keyboardMouseHook.MouseUp += HookOnMouseUp;

            holdKeysThread.IsBackground = true;
            autoClickThread.IsBackground = true;
            commandKeysThread.IsBackground = true;

            holdKeysThread.Start();
            autoClickThread.Start();
            commandKeysThread.Start();
        }


        #region HoldKeys
        private void HoldKeys_Click(object sender, EventArgs e)
        {
            holdKeysActive = !holdKeysActive;
        }


        private void HoldKeys_Key_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedKey = (sender as ComboBox).Text;
            try
            {
                holdKeysKey = KeyConverter[selectedKey.ToLower()].key;
                HoldKeys.Text = "Hold Keys (" + selectedKey + ")";
            }
            catch (Exception) { }
        }


        private void HoldKeys_ClearKey_Click(object sender, EventArgs e)
        {
            HoldKeys.Text = "Hold Keys ()";
            HoldKeys_Key.Text = string.Empty;
            holdKeysKey = Keys.None;
        }


        private static void HoldKeysMethod()
        {
            while (true)
            {
                while (holdKeysActive)
                {
                    foreach (string key in loggedKeysText)
                        KeyConverterHelper(key.ToLower(), 0);

                    Thread.Sleep(10);

                    if (!holdKeysActive)
                    {
                        foreach (string key in loggedKeysText)
                            KeyConverterHelper(key.ToLower(), 1);
                    }
                }
                Thread.Sleep(1);
            }
        }
        #endregion HoldKeys


        #region AutoClick
        private void AutoClick_Click(object sender, EventArgs e)
        {
            autoClickKeysActive = !autoClickKeysActive;
        }


        private void AutoClick_Key_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedKey = (sender as ComboBox).Text;
            try
            {
                autoClickKeysKey = KeyConverter[selectedKey.ToLower()].key;
                AutoClick.Text = "AutoClick Keys (" + selectedKey + ")";
            }
            catch (Exception) { }
        }


        private void AutoClick_ClearKey_Click(object sender, EventArgs e)
        {
            AutoClick.Text = "AutoClick Keys ()";
            AutoClick_Key.Text = string.Empty;
            autoClickKeysKey = Keys.None;
        }


        private void AutoClick_MS_ValueChanged(object sender, EventArgs e)
        {
            autoClickIntervalMS = (int)(sender as NumericUpDown).Value;
        }


        private static void AutoClickKeysMethod()
        {
            while (true)
            {
                while (autoClickKeysActive)
                {
                    foreach (string key in loggedKeysText)
                        KeyConverterHelper(key.ToLower(), 2);

                    Thread.Sleep(autoClickIntervalMS);
                }
                Thread.Sleep(1);
            }
        }
        #endregion AutoClick


        #region CLI
        private void CLI_Run_Click(object sender, EventArgs e)
        {
            if (runCommandsToggleMode)
                runCommandsActive = !runCommandsActive;
            else
                runCommandsActive = true;
        }


        private void CLI_Run_Key_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedKey = (sender as ComboBox).Text;
            try
            {
                runCommandsKey = KeyConverter[selectedKey.ToLower()].key;
                CLI_Run.Text = "Run Commands (" + selectedKey + ")";
            }
            catch (Exception) { }
        }


        private void CLI_ClearKey_Click(object sender, EventArgs e)
        {
            CLI_Run.Text = "Run Commands ()";
            CLI_Run_Key.Text = string.Empty;
            runCommandsKey = Keys.None;
        }


        private void CLI_Run_Toggle_CheckedChanged(object sender, EventArgs e)
        {
            runCommandsToggleMode = CLI_Run_Toggle.Checked;
        }


        private void CLI_Stop_Click(object sender, EventArgs e)
        {
            runCommandsActive = false;
        }


        private void CLI_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "- case does not matter for the command system\n" +
                "- ALL commands must end with a semi-colon\n" +
                "(this character -> ; )\n" +
                "- because it only simulates a key press if you click a key while it's simulated down, it will no longer be held once you physically release the key\n\n" +
                "Commands:\n" +
                "- repeat | after repeat put a number and it will repeat all commands until stop that many times. repeat with no number after it will work the same but forever\n" +
                "- stop | ends the current repeat section\n" +
                "- wait | stops commands from running based on the number after the command (milliseconds)\n" +
                "- down | simulates the key after it being pressed down\n" +
                "- up | simulates the key after it being released\n" +
                "- click | simulates the key after it being clicked\n" +
                "- movex | moves the mouse's x coordinate to the number after the command\n" +
                "- mousey | moves the mouse's y coordinate to the number after the command",
                "CLI Help");
        }


        private void CLI_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string[] possibleCommands;
                int repeatCheck = 0;
                Keys keyTest;

                if (CLI.Text.Last() == ';')
                    possibleCommands = CLI.Text.Split(';')[..^1];
                else
                    possibleCommands = ["error"];

                foreach (string command in possibleCommands)
                {
                    string commandTrim = command.Trim().ToLower();

                    if (commandTrim.StartsWith("repeat"))
                    {
                        if (string.IsNullOrWhiteSpace(commandTrim.Substring(6)))
                        {
                            commandList.Add("repeat");
                            repeatCheck++;
                        }
                        else if (int.TryParse(commandTrim.Substring(6), out int times))
                        {
                            if (int.IsNegative(times) || times == 0)
                                throw new Exception("repeat times value bad");
                            commandList.Add("repeat" + times);
                            repeatCheck++;
                        }
                        else
                        {
                            throw new Exception("bad repeat command");
                        }
                    }
                    else if (commandTrim.Equals("stop"))
                    {
                        commandList.Add("stop");
                        repeatCheck--;
                    }
                    else if (commandTrim.StartsWith("wait"))
                    {
                        if (int.TryParse(commandTrim.Substring(4), out int time))
                        {
                            if (int.IsNegative(time) || time == 0)
                                throw new Exception("wait time value bad");
                            commandList.Add("wait" + time);
                        }
                        else
                        {
                            throw new Exception("bad wait command");
                        }
                    }
                    else if (commandTrim.StartsWith("down"))
                    {
                        keyTest = KeyConverter[commandTrim.Substring(4).Trim().ToLower()].key;
                        commandList.Add("down" + commandTrim.Substring(4).Trim());
                    }
                    else if (commandTrim.StartsWith("up"))
                    {
                        keyTest = KeyConverter[commandTrim.Substring(2).Trim().ToLower()].key;
                        commandList.Add("up" + commandTrim.Substring(2).Trim());
                    }
                    else if (commandTrim.StartsWith("click"))
                    {
                        keyTest = KeyConverter[commandTrim.Substring(5).Trim().ToLower()].key;
                        commandList.Add("click" + commandTrim.Substring(5).Trim());
                    }
                    else if (commandTrim.StartsWith("mousex"))
                    {
                        if (int.TryParse(commandTrim.Substring(6), out int pos))
                        {
                            if (int.IsNegative(pos))
                                throw new Exception("mouse pos value bad");
                            commandList.Add("mousex" + pos);
                        }
                        else
                        {
                            throw new Exception("bad mouse command");
                        }
                    }
                    else if (commandTrim.StartsWith("mousey"))
                    {
                        if (int.TryParse(commandTrim.Substring(6), out int pos))
                        {
                            if (int.IsNegative(pos))
                                throw new Exception("mouse pos value bad");
                            commandList.Add("mousey" + pos);
                        }
                        else
                        {
                            throw new Exception("bad mouse command");
                        }
                    }
                    else
                    {
                        throw new Exception("unkown command");
                    }
                }
                if (repeatCheck != 0)
                    throw new Exception("count of repeat and end are not equal");
            }
            catch (Exception)
            {
                CLI_Check.Text = "Not A Valid Command List";
                commandList.Clear();
                return;
            }
            CLI_Check.Text = "Valid Command List";
        }


        private static void CommandKeysMethod()
        {
            int commandStep;
            List<(int count, int startIndex)> loopStack;
            string currentCommand;

            while (true)
            {
                commandStep = 0;
                loopStack = [];

                while (runCommandsActive)
                {
                    if (commandStep < commandList.Count)
                    {
                        currentCommand = commandList[commandStep];

                        if (currentCommand == null)
                        {
                            commandStep++;
                            continue;
                        }

                        else if (currentCommand == "stop")
                        {
                            if (loopStack.Count == 0)
                                throw new Exception("stop without matching repeat");

                            var (count, startIndex) = loopStack[^1];

                            if (count == -1)
                            {
                                commandStep = startIndex;
                            }
                            else if (count > 1)
                            {
                                loopStack[^1] = (count - 1, startIndex);
                                commandStep = startIndex;
                            }
                            else
                            {
                                loopStack.RemoveAt(loopStack.Count - 1);
                            }
                        }
                        else if (currentCommand.StartsWith("repeat"))
                        {
                            if (int.TryParse(currentCommand.Substring(6), out int times))
                                loopStack.Add((times, commandStep));
                            else
                                loopStack.Add((-1, commandStep));
                        }
                        else if (currentCommand.StartsWith("wait"))
                        {
                            Thread.Sleep(int.Parse(currentCommand.Substring(4)));
                        }
                        else if (currentCommand.StartsWith("down"))
                        {
                            KeyConverterHelper(currentCommand.Substring(4), 0);
                        }
                        else if (currentCommand.StartsWith("up"))
                        {
                            KeyConverterHelper(currentCommand.Substring(2), 1);
                        }
                        else if (currentCommand.StartsWith("click"))
                        {
                            KeyConverterHelper(currentCommand.Substring(5), 2);
                        }
                        else if (currentCommand.StartsWith("mousex"))
                        {
                            Cursor.Position = new Point(int.Parse(currentCommand.Substring(6)), Cursor.Position.Y);
                        }
                        else if (currentCommand.StartsWith("mousey"))
                        {
                            Cursor.Position = new Point(Cursor.Position.X, int.Parse(currentCommand.Substring(6)));
                        }

                        commandStep++;
                        continue;
                    }

                    if (!runCommandsToggleMode)
                        runCommandsActive = false;
                }
                Thread.Sleep(1);
            }
        }
        #endregion CLI


        #region LoggedKeys
        private void AddLogged_SelectedIndexChanged(object sender, EventArgs e)
        {
            loggedKeysText.Add((sender as ComboBox).Text);
            LoggedKeys.Text = HashSetToStringNewLine(loggedKeysText);
        }


        private void RemoveLogged_SelectedIndexChanged(object sender, EventArgs e)
        {
            loggedKeysText.Remove((sender as ComboBox).Text);
            LoggedKeys.Text = HashSetToStringNewLine(loggedKeysText);
        }


        private void ClearLogged_Click(object sender, EventArgs e)
        {
            loggedKeysText = [];
            AddLogged.Text = string.Empty;
            RemoveLogged.Text = string.Empty;
            LoggedKeys.Text = "";
        }
        #endregion LoggedKeys


        #region KeyHook
        private void HookOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == holdKeysKey)
                holdKeysActive = !holdKeysActive;

            if (e.KeyCode == autoClickKeysKey)
                autoClickKeysActive = !autoClickKeysActive;

            if (e.KeyCode == runCommandsKey)
            {
                if (runCommandsToggleMode)
                    runCommandsActive = !runCommandsActive;
                else
                    runCommandsActive = true;
            }

            pressedKeys.Add(e.KeyCode);
        }


        private void HookOnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Keys.LButton == holdKeysKey)
                    holdKeysActive = !holdKeysActive;

                if (Keys.LButton == autoClickKeysKey)
                    autoClickKeysActive = !autoClickKeysActive;

                if (Keys.LButton == runCommandsKey)
                {
                    if (runCommandsToggleMode)
                        runCommandsActive = !runCommandsActive;
                    else
                        runCommandsActive = true;
                }
                pressedKeys.Add(Keys.LButton);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                if (Keys.MButton == holdKeysKey)
                    holdKeysActive = !holdKeysActive;

                if (Keys.MButton == autoClickKeysKey)
                    autoClickKeysActive = !autoClickKeysActive;

                if (Keys.MButton == runCommandsKey)
                {
                    if (runCommandsToggleMode)
                        runCommandsActive = !runCommandsActive;
                    else
                        runCommandsActive = true;
                }
                pressedKeys.Add(Keys.MButton);
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Keys.RButton == holdKeysKey)
                    holdKeysActive = !holdKeysActive;

                if (Keys.RButton == autoClickKeysKey)
                    autoClickKeysActive = !autoClickKeysActive;

                if (Keys.RButton == runCommandsKey)
                {
                    if (runCommandsToggleMode)
                        runCommandsActive = !runCommandsActive;
                    else
                        runCommandsActive = true;
                }
                pressedKeys.Add(Keys.RButton);
            }

        }


        private void HookOnKeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }


        private void HookOnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                pressedKeys.Remove(Keys.LButton);
            else if (e.Button == MouseButtons.Middle)
                pressedKeys.Remove(Keys.MButton);
            else if (e.Button == MouseButtons.Right)
                pressedKeys.Remove(Keys.RButton);
        }
        #endregion KeyHook


        #region Other
        private static string HashSetToStringNewLine(HashSet<string> set)
        {
            string temp = "";

            foreach (string s in set)
            {
                temp += s + Environment.NewLine;
            }

            return temp;
        }


        private static void KeyConverterHelper(string key, int mode)
        {
            switch (mode)
            {
                case 0:
                    if (pressedKeys.Contains(KeyConverter[key.ToLower()].key))
                        return;
                    else
                        KeyConverter[key.ToLower()].down(); break;
                case 1:
                    if (!pressedKeys.Contains(KeyConverter[key.ToLower()].key))
                        return;
                    else
                        KeyConverter[key.ToLower()].up(); break;
                case 2:
                    KeyConverter[key.ToLower()].press(); break;
            }
        }
        #endregion Other
    }
}
