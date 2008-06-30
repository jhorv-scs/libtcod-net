//#define ManualTest

using System;
using libtcodWrapper;

namespace libtcodWrapperTests
{
#if ManualTest
    public class ManualTest
    {
        static int Main()
        {
            TCODFovTest.TestTCODFovTest();
            TCODKeyboardTest.TestTCODKeyboard();
            return 0;
        }
    }
#endif
}
