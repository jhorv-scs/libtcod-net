using System;
using libtcodWrapper;

using Console = libtcodWrapper.Console;

namespace libtcodWrapperTests
{
    //This is a manual test for the mouse code.
    public class TCODMouseTest
    {
        private static void PrintStatus(Console console, string name, bool status, int x, int y)
        {
            console.PrintLine("Pressed " + name + " = " + (status ? "On" : "Off"), x, y, LineAlignment.Left);
        }
        
        public static void TestTCODMouse()
        {
            bool cursorVisible = true;

            RootConsole.Width = 80;
            RootConsole.Height = 50;
            RootConsole.WindowTitle = "Mouse Tester";
            RootConsole.Fullscreen = false;

            using (RootConsole console = RootConsole.GetInstance())
            {
                TCODSystem.FPS = 30;

                do
                {
                    console.Clear();
                    KeyPress pressedKey = Keyboard.CheckForKeypress(KeyPressType.Released);
                    Mouse m = Mouse.GetStatus();;                    
                    Mouse.ShowCursor(cursorVisible);

                    if(pressedKey.Character == 't')
                        cursorVisible = !Mouse.IsVisible;
                    if(pressedKey.Character == 'm')
                        Mouse.MoveMouse(10, 10);

                    console.PrintLine("Mouse Test Suite", 40, 5, LineAlignment.Center);
                    console.PrintLine("Close Window to quit.", 40, 7, LineAlignment.Center);
                    console.PrintLine("Press \"t\" to toggle cursor", 40, 8, LineAlignment.Center);
                    console.PrintLine("Press \"m\" to move cursor to (10,10)", 40, 9, LineAlignment.Center);
                    
                    PrintStatus(console, "LeftButton", m.LeftButton, 10, 12);
                    PrintStatus(console, "RightButton", m.RightButton, 10, 13);
                    PrintStatus(console, "MiddleButton", m.MiddleButton, 10, 14);
                    PrintStatus(console, "LeftButtonPressed", m.LeftButtonPressed, 10, 15);
                    PrintStatus(console, "RightButtonPressed", m.RightButtonPressed, 10, 16);
                    PrintStatus(console, "MiddleButtonPressed", m.MiddleButtonPressed, 10, 17);
                    console.PrintLine("Cursor Visible = " + (cursorVisible ? "On" : "Off"), 10, 18, LineAlignment.Left);

                    console.PrintLine("x = " + m.PixelX, 10, 20, LineAlignment.Left);
                    console.PrintLine("y = " + m.PixelY, 10, 21, LineAlignment.Left);
                    console.PrintLine("dx = " + m.PixelVelocityX, 10, 22, LineAlignment.Left);
                    console.PrintLine("dy = " + m.PixelVelocityY, 10, 23, LineAlignment.Left);
                    console.PrintLine("cx = " + m.CellX, 10, 24, LineAlignment.Left);
                    console.PrintLine("cy = " + m.CellY, 10, 25, LineAlignment.Left);
                    console.PrintLine("dcx = " + m.CellVelocityX, 10, 26, LineAlignment.Left);
                    console.PrintLine("dcy = " + m.CellVelocityY, 10, 27, LineAlignment.Left);
                    
                    console.Flush();
            
                }
                while(!console.IsWindowClosed());
            }
        }
    }
}
