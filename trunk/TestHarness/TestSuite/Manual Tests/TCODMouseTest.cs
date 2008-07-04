using System;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    //This is a manual test for the mouse code.
    public class TCODMouseTest
    {
		private static void PrintStatus(TCODConsole console, string name, bool status, int x, int y)
		{
			TCODConsoleLinePrinter.PrintLine(console, "Pressed " + name + " = " + (status  ? "On" : "Off"), x, y, TCODLineAlign.Left);
		}
		
        public static bool TestTCODMouse()
        {
            bool testPassed = true;
            try
            {
                TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "Mouse Tester", false);
				TCODKeyboard keyboard = new TCODKeyboard();
				TCODSystem.SetFPS(30);

				do
                {
                    console.Clear();
					TCOD_key pressedKey = keyboard.CheckForKeypress(TCOD_keypressed.TCOD_KEY_PRESSEDANDRELEASED);
					TCODMouse m = TCODMouse.GetStatus();

		            TCODConsoleLinePrinter.PrintLine(console, "Mouse Test Suite", 40, 5, TCODLineAlign.Center);
		            TCODConsoleLinePrinter.PrintLine(console, "Close Window to quit.", 40, 7, TCODLineAlign.Center);
					
					PrintStatus(console, "lbutton", m.lbutton, 10, 12);
					PrintStatus(console, "rbutton", m.rbutton, 10, 13);
					PrintStatus(console, "mbutton", m.mbutton, 10, 14);
					PrintStatus(console, "lbutton_pressed", m.lbutton_pressed, 10, 15);
					PrintStatus(console, "rbutton_pressed", m.rbutton_pressed, 10, 16);
					PrintStatus(console, "mbutton_pressed", m.mbutton_pressed, 10, 17);
					
					TCODConsoleLinePrinter.PrintLine(console, "x = " + m.x, 10, 20, TCODLineAlign.Left);
					TCODConsoleLinePrinter.PrintLine(console, "y = " + m.y, 10, 21, TCODLineAlign.Left);
					TCODConsoleLinePrinter.PrintLine(console, "dx = " + m.dx, 10, 22, TCODLineAlign.Left);
					TCODConsoleLinePrinter.PrintLine(console, "dy = " + m.dy, 10, 23, TCODLineAlign.Left);
					TCODConsoleLinePrinter.PrintLine(console, "cx = " + m.cx, 10, 24, TCODLineAlign.Left);
					TCODConsoleLinePrinter.PrintLine(console, "cy = " + m.cy, 10, 25, TCODLineAlign.Left);
					TCODConsoleLinePrinter.PrintLine(console, "dcx = " + m.dcx, 10, 26, TCODLineAlign.Left);
					TCODConsoleLinePrinter.PrintLine(console, "dcy = " + m.dcy, 10, 27, TCODLineAlign.Left);
					
					console.Flush();
                }
				while(!console.IsWindowClosed());
            }
            catch
            {
                testPassed = false;
            }
            return testPassed;
        }
    }
}