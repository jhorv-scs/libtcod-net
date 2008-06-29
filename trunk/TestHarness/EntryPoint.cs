using System;
using libtcodWrapper;

namespace TestHarness
{
    class EntryPoint
    {
        public static int Main()
        {
            //For C/C++ debug attachment
            //TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "Keyboard Tester", false);
            //TCODKeyboard keyboard = new TCODKeyboard();
            //TCOD_key key = keyboard.WaitForKeyPress(false);
            return TCODFileParserTest.TestFileParser() == true ? 1 : 0;
        }
    }
}
