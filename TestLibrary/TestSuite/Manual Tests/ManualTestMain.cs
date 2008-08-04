//#define ManualTest

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
            using (TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "Waiting for Debugger", false))
			{
                TCODKeyboard.WaitForKeyPress(false);
			}
        }
		#pragma warning restore 0649

        static int Main()
        {
			//WaitForDebugger();

			//TCODMouseTest.TestTCODMouse();
            //TCODFovTest.TestTCODFovTest();
            //TCODKeyboardTest.TestTCODKeyboard();
            return 0;
        }
    }
#endif
}
