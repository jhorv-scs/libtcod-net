using System;
using libtcodWrapper;

using Console = libtcodWrapper.Console;

namespace libtcodWrapperTests
{
    //This is a manual test for the keyboard code.
    public class KeyboardTest
    {
        private static bool inRealTimeTest = false;
        public static void TestKeyboard()
        {
            RootConsole.Width = 80;
            RootConsole.Height = 50;
            RootConsole.WindowTitle = "Keyboard Tester";
            RootConsole.Fullscreen = false;

            using (RootConsole console = RootConsole.GetInstance())
            {
                KeyPress key = new KeyPress();
                do
                {
                    if (inRealTimeTest)
                        RealTimeLoopTest(console);
                    else
                        TurnBasedLoopTest(console, ref key);
                    System.Console.Out.WriteLine((char)key.Character);
                }
                while (key.Character != 'q' && !console.IsWindowClosed());
            }
        }
        
        private static void PrintStatus(Console console, string name, bool status, int x, int y)
        {
            console.PrintLine("Pressed " + name + " = " + (status  ? "On" : "Off"), x, y, LineAlignment.Left);
        }

        private static void TurnBasedLoopTest(RootConsole console, ref KeyPress key)
        {
            console.Clear();
            console.PrintLine("Keyboard Test Suite", 40, 5, LineAlignment.Center);
            console.PrintLine("Press 'F10' to enter Real Time Test.", 40, 6, LineAlignment.Center);
            console.PrintLine("Press 'q' to quit.", 40, 7, LineAlignment.Center);

            if (key.KeyCode == KeyCode.TCODK_CHAR)
                console.PrintLine("Key Hit = \"" + (char)key.Character + "\"", 10, 10, LineAlignment.Left);
            else
                console.PrintLine("Special Key Hit = " + key.KeyCode.ToString(), 10, 10, LineAlignment.Left);

            PrintStatus(console, "Status", key.Pressed, 10, 12);
            PrintStatus(console, "lalt", key.LeftAlt, 10, 13);
            PrintStatus(console, "lctrl", key.LeftControl, 10, 14);
            PrintStatus(console, "ralt", key.RightAlt, 10, 15);
            PrintStatus(console, "rctrl", key.RightControl, 10, 16);
            PrintStatus(console, "shift", key.Shift, 10, 17);


            console.PrintLine("F1 Key Pressed = " + (Keyboard.IsKeyPressed(KeyCode.TCODK_F1) ? "Yes" : "No"), 10, 20, LineAlignment.Left);

            console.Flush();

            key = Keyboard.WaitForKeyPress(false);

            if (key.KeyCode == KeyCode.TCODK_F10)
                inRealTimeTest = true;
        }

		
		private static bool ctrlUpHit = false;
        private static void RealTimeLoopTest(RootConsole console)
        {
            TCODSystem.FPS = 25;

            console.Clear();

            console.PrintLine("Keyboard Test Suite", 40, 5, LineAlignment.Center);
            console.PrintLine("Press 'F10' to enter Turn Based Test.", 40, 6, LineAlignment.Center);

            KeyPress pressedKey = Keyboard.CheckForKeypress(KeyPressType.PressedAndReleased);

            console.PrintLine("F2 Key Pressed = " + ((pressedKey.KeyCode == KeyCode.TCODK_F2 && pressedKey.Pressed) ? "Yes" : "No"), 10, 10, LineAlignment.Left);
            console.PrintLine("'d' to disable repeat keys", 10, 11, LineAlignment.Left);
            console.PrintLine("'e' to enable repeat keys", 10, 12, LineAlignment.Left);
            console.PrintLine(string.Format("Ctrl: {0}", pressedKey.LeftControl), 10, 13, LineAlignment.Left);
			console.PrintLine(string.Format("Ctrl Up: {0}", ctrlUpHit), 10, 13, LineAlignment.Left);
			if(pressedKey.KeyCode == KeyCode.TCODK_UP && pressedKey.Control)
			{
				ctrlUpHit = true;
			}

            console.Flush();

            if (pressedKey.Character == 'd')
                Keyboard.DisableRepeat();
            if (pressedKey.Character == 'e')
                Keyboard.SetRepeat(0, 10);

            if (pressedKey.KeyCode == KeyCode.TCODK_F10 && pressedKey.Pressed)
                inRealTimeTest = false;
        }
    }
}
