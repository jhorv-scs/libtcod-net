using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    public enum TCOD_keycode
    {
	    TCODK_NONE,
	    TCODK_ESCAPE,
	    TCODK_BACKSPACE,
	    TCODK_TAB,
	    TCODK_ENTER,
	    TCODK_SHIFT,
	    TCODK_CONTROL,
	    TCODK_ALT,
	    TCODK_PAUSE,
	    TCODK_CAPSLOCK,
	    TCODK_PAGEUP,
	    TCODK_PAGEDOWN,
	    TCODK_END,
	    TCODK_HOME,
	    TCODK_UP,
	    TCODK_LEFT,
	    TCODK_RIGHT,
	    TCODK_DOWN,
	    TCODK_PRINTSCREEN,
	    TCODK_INSERT,
	    TCODK_DELETE,
	    TCODK_LWIN,
	    TCODK_RWIN,
	    TCODK_APPS,
	    TCODK_0,
	    TCODK_1,
	    TCODK_2,
	    TCODK_3,
	    TCODK_4,
	    TCODK_5,
	    TCODK_6,
	    TCODK_7,
	    TCODK_8,
	    TCODK_9,
	    TCODK_KP0,
	    TCODK_KP1,
	    TCODK_KP2,
	    TCODK_KP3,
	    TCODK_KP4,
	    TCODK_KP5,
	    TCODK_KP6,
	    TCODK_KP7,
	    TCODK_KP8,
	    TCODK_KP9,
	    TCODK_KPADD,
	    TCODK_KPSUB,
	    TCODK_KPDIV,
	    TCODK_KPMUL,
	    TCODK_KPDEC,
	    TCODK_KPENTER,
	    TCODK_F1,
	    TCODK_F2,
	    TCODK_F3,
	    TCODK_F4,
	    TCODK_F5,
	    TCODK_F6,
	    TCODK_F7,
	    TCODK_F8,
	    TCODK_F9,
	    TCODK_F10,
	    TCODK_F11,
	    TCODK_F12,
	    TCODK_NUMLOCK,
	    TCODK_SCROLLLOCK,
	    TCODK_SPACE,
	    TCODK_CHAR
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TCOD_key
    {
        public TCOD_keycode vk; //  key code
	    public byte c; // character if vk == TCODK_CHAR else 0
        private UInt32 modifiers;
	    /*public byte pressed; // does this correspond to a key press or key release event ?
        public byte lalt;
        public byte lctrl;
        public byte ralt;
        public byte rctrl;
        public byte shift;*/

        public bool pressed
        {
            get { return ((modifiers & 0x01) > 0); }
            set { }
        }
        public bool lalt
        {
            get { return ((modifiers & 0x02) > 0); }
            set { }
        }
        public bool lctrl
        {
            get { return ((modifiers & 0x04) > 0); }
            set { }
        }
        public bool ralt
        {
            get { return ((modifiers & 0x8) > 0); }
            set { }
        }
        public bool rctrl
        {
            get { return ((modifiers & 0x10) > 0); }
            set { }
        }
        public bool shift
        {
            get { return ((modifiers & 0x20) > 0); }
            set { }
        }
    }

    public enum TCOD_keypressed
    {
	    TCOD_KEY_PRESSED=1,
	    TCOD_KEY_RELEASED=2,
        TCOD_KEY_PRESSEDANDRELEASED=3,
    };

    public class TCODKeyboard
    {
        public TCOD_key WaitForKeyPress(bool flushInputBuffer)
        {
            return TCOD_console_wait_for_keypress(flushInputBuffer);
        }

        public TCOD_key CheckForKeypress(TCOD_keypressed pressFlags)
        {
            return TCOD_console_check_for_keypress(pressFlags);
        }

        public bool IsKeyPressed(TCOD_keycode key)
        {
            return TCOD_console_is_key_pressed(key);
        }

        public void SetRepeat(int initialDelay, int interval)
        {
            TCOD_console_set_keyboard_repeat(initialDelay, interval);
        }

        public void DisableRepeat()
        {
            TCOD_console_disable_keyboard_repeat();
        }


        [DllImport(DLLName.name)]
        private extern static TCOD_key TCOD_console_wait_for_keypress(bool flush);

        [DllImport(DLLName.name)]
        private extern static TCOD_key TCOD_console_check_for_keypress(TCOD_keypressed flags);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_console_is_key_pressed(TCOD_keycode key);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_keyboard_repeat(int initial_delay, int interval);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_disable_keyboard_repeat();
    }


    public class TCODKeyboardTest
    {
        private static bool inRealTimeTest = false;
        public static bool TestTCODKeyboard()
        {
            TCODKeyboard keyboard = new TCODKeyboard();
            bool testPassed = true;
            try
            {
                CustomFontRequest font = new CustomFontRequest("Herrbdog_16x16_tileset.bmp", 16, 16, 16, 16, true, new TCODColor(255, 0, 255));
                TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "Keyboard Tester", false, font);
                
                TCOD_key key = new TCOD_key();
                do
                {
                    if( inRealTimeTest )
                        RealTimeLoopTest(console, keyboard);
                    else
                        TurnBasedLoopTest(console, keyboard, ref key);
                }
                while (key.c != 'q' && !console.IsWindowClosed());
            }
            catch
            {
                testPassed = false;
            }
            return testPassed;
        }

        private static void TurnBasedLoopTest(TCODConsoleRoot console, TCODKeyboard keyboard, ref TCOD_key key)
        {
            console.Clear();
            TCODConsoleLinePrinter.PrintLine(console, "Keyboard Test Suite", 40, 5, TCODLineAlign.Center);
            TCODConsoleLinePrinter.PrintLine(console, "Press 'F10' to enter Real Time Test.", 40, 6, TCODLineAlign.Center);
            TCODConsoleLinePrinter.PrintLine(console, "Press 'q' to quit.", 40, 7, TCODLineAlign.Center);
            
            if (key.vk == TCOD_keycode.TCODK_CHAR)
                TCODConsoleLinePrinter.PrintLine(console, "Key Hit = \"" + (char)key.c + "\"", 10, 10, TCODLineAlign.Left);
            else
                TCODConsoleLinePrinter.PrintLine(console, "Special Key Hit = " + key.vk.ToString(), 10, 10, TCODLineAlign.Left);

            TCODConsoleLinePrinter.PrintLine(console, "Pressed Status = " + (key.pressed ? "On" : "Off"), 10, 12, TCODLineAlign.Left);
            TCODConsoleLinePrinter.PrintLine(console, "lalt Status = " + (key.lalt ? "On" : "Off"), 10, 13, TCODLineAlign.Left);
            TCODConsoleLinePrinter.PrintLine(console, "lctrl Status = " + (key.lctrl ? "On" : "Off"), 10, 14, TCODLineAlign.Left);
            TCODConsoleLinePrinter.PrintLine(console, "ralt Status = " + (key.ralt ? "On" : "Off"), 10, 15, TCODLineAlign.Left);
            TCODConsoleLinePrinter.PrintLine(console, "rctrl Status = " + (key.rctrl ? "On" : "Off"), 10, 16, TCODLineAlign.Left);
            TCODConsoleLinePrinter.PrintLine(console, "shift Status = " + (key.shift ? "On" : "Off"), 10, 17, TCODLineAlign.Left);

            TCODConsoleLinePrinter.PrintLine(console, "F1 Key Pressed = " + (keyboard.IsKeyPressed(TCOD_keycode.TCODK_F1) ? "Yes" : "No"), 10, 20, TCODLineAlign.Left);

            console.Flush();
            
            key = keyboard.WaitForKeyPress(false);

            if (key.vk == TCOD_keycode.TCODK_F10)
                inRealTimeTest = true;    
        }

        private static void RealTimeLoopTest(TCODConsoleRoot console, TCODKeyboard keyboard)
        {
            TCODSystem.SetFPS(25);

            console.Clear();

            TCODConsoleLinePrinter.PrintLine(console, "Keyboard Test Suite", 40, 5, TCODLineAlign.Center);
            TCODConsoleLinePrinter.PrintLine(console, "Press 'F10' to enter Turn Based Test.", 40, 6, TCODLineAlign.Center);

            TCOD_key pressedKey = keyboard.CheckForKeypress(TCOD_keypressed.TCOD_KEY_PRESSEDANDRELEASED);

            TCODConsoleLinePrinter.PrintLine(console, "F2 Key Pressed = " + ((pressedKey.vk == TCOD_keycode.TCODK_F2 && pressedKey.pressed) ? "Yes" : "No"), 10, 10, TCODLineAlign.Left);
            TCODConsoleLinePrinter.PrintLine(console, "'d' to disable repeat keys", 10, 11, TCODLineAlign.Left);
            TCODConsoleLinePrinter.PrintLine(console, "'e' to enable repeat keys", 10, 12, TCODLineAlign.Left);

            console.Flush();

            if (pressedKey.c == 'd')
                keyboard.DisableRepeat();
            if (pressedKey.c == 'e')
                keyboard.SetRepeat(0, 10);

            if (pressedKey.vk == TCOD_keycode.TCODK_F10 && pressedKey.pressed)
                inRealTimeTest = false;
        }
    }
}
