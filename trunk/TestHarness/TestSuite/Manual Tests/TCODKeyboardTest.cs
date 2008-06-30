using System;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    //This is a manual test for the keyboard code.
    public class TCODKeyboardTest
    {
        private static bool inRealTimeTest = false;
        public static bool TestTCODKeyboard()
        {

            bool testPassed = true;
            try
            {
                CustomFontRequest font = new CustomFontRequest("Herrbdog_16x16_tileset.bmp", 16, 16, 16, 16, true, new TCODColor(255, 0, 255));
                TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "Keyboard Tester", false, font);
                TCODKeyboard keyboard = new TCODKeyboard();

                TCOD_key key = new TCOD_key();
                do
                {
                    if (inRealTimeTest)
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