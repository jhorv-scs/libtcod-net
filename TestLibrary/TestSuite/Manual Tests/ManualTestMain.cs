#define ManualTest

using System;
using libtcodWrapper;

namespace libtcodWrapperTests
{
#if ManualTest
    public class ManualTest
    {
        //This field is used in manual tests, and not a warning 
        #pragma warning disable 0649
        static void WaitForDebugger()
        {
            RootConsole.Width = 80;
            RootConsole.Height = 50;
            RootConsole.WindowTitle = "Waiting for Debugger";
            RootConsole.Fullscreen = false;

            using(RootConsole console = RootConsole.GetInstance())
            {
                Keyboard.WaitForKeyPress(false);
            }
        }
#pragma warning restore 0649

        static int Main()
        {
            //WaitForDebugger();

            //TCODMouseTest.TestTCODMouse();
            //TCODFovTest.TestTCODFovTest();
            KeyboardTest.TestKeyboard();
            return 0;
        }
    }
#endif
}
