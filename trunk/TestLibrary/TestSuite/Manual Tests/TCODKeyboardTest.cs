using System;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    //This is a manual test for the keyboard code.
    public class TCODKeyboardTest
    {
        private static bool inRealTimeTest = false;
        public static void TestTCODKeyboard()
        {
            using(TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "Keyboard Tester", false))
            {
            	TCODKeyboard keyboard = new TCODKeyboard();

            	TCOD_key key = new TCOD_key();
	            do
            	{
	                if (inRealTimeTest)
                    	RealTimeLoopTest(console, keyboard);
	                else
                    	TurnBasedLoopTest(console, keyboard, ref key);
                    System.Console.Out.WriteLine((char)key.c);
	            }
            	while (key.c != 'q' && !console.IsWindowClosed());
			}
        }
		
		private static void PrintStatus(TCODConsole console, string name, bool status, int x, int y)
		{
			console.PrintLine("Pressed " + name + " = " + (status  ? "On" : "Off"), x, y, TCODLineAlign.Left);
		}

        private static void TurnBasedLoopTest(TCODConsoleRoot console, TCODKeyboard keyboard, ref TCOD_key key)
        {
            console.Clear();
            console.PrintLine("Keyboard Test Suite", 40, 5, TCODLineAlign.Center);
            console.PrintLine("Press 'F10' to enter Real Time Test.", 40, 6, TCODLineAlign.Center);
            console.PrintLine("Press 'q' to quit.", 40, 7, TCODLineAlign.Center);

            if (key.vk == TCOD_keycode.TCODK_CHAR)
                console.PrintLine("Key Hit = \"" + (char)key.c + "\"", 10, 10, TCODLineAlign.Left);
            else
                console.PrintLine("Special Key Hit = " + key.vk.ToString(), 10, 10, TCODLineAlign.Left);

			PrintStatus(console, "Status", key.pressed, 10, 12);
			PrintStatus(console, "lalt", key.lalt, 10, 13);
			PrintStatus(console, "lctrl", key.lctrl, 10, 14);
			PrintStatus(console, "ralt", key.ralt, 10, 15);
			PrintStatus(console, "rctrl", key.rctrl, 10, 16);
			PrintStatus(console, "shift", key.shift, 10, 17);


            console.PrintLine("F1 Key Pressed = " + (keyboard.IsKeyPressed(TCOD_keycode.TCODK_F1) ? "Yes" : "No"), 10, 20, TCODLineAlign.Left);

            console.Flush();

            key = keyboard.WaitForKeyPress(false);

            if (key.vk == TCOD_keycode.TCODK_F10)
                inRealTimeTest = true;
        }

        private static void RealTimeLoopTest(TCODConsoleRoot console, TCODKeyboard keyboard)
        {
            TCODSystem.SetFPS(25);

            console.Clear();

            console.PrintLine("Keyboard Test Suite", 40, 5, TCODLineAlign.Center);
            console.PrintLine("Press 'F10' to enter Turn Based Test.", 40, 6, TCODLineAlign.Center);

            TCOD_key pressedKey = keyboard.CheckForKeypress(TCOD_keypressed.TCOD_KEY_PRESSEDANDRELEASED);

            console.PrintLine("F2 Key Pressed = " + ((pressedKey.vk == TCOD_keycode.TCODK_F2 && pressedKey.pressed) ? "Yes" : "No"), 10, 10, TCODLineAlign.Left);
            console.PrintLine("'d' to disable repeat keys", 10, 11, TCODLineAlign.Left);
            console.PrintLine("'e' to enable repeat keys", 10, 12, TCODLineAlign.Left);

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