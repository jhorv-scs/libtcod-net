//#define ManualTest

using System;
using libtcodWrapper;

namespace libtcodWrapperTests
{
#if ManualTest
    public class ManualTest
    {
        static void WaitForDebugger()
        {
            TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "Waiting for Debugger", false);
            TCODKeyboard keyboard = new TCODKeyboard();
            keyboard.WaitForKeyPress(false);
        }

        static int Main()
        {
            //TCODFovTest.TestTCODFovTest();
            //TCODKeyboardTest.TestTCODKeyboard();
            //WaitForDebugger();
            return 0;
        }
    }
#endif
}
