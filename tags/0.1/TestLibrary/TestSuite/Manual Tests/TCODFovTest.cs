using System;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    //This is a manual test for the FOV code.
    public class TCODFovTest
    {
        static char[,] room =
            {
                {'#', '#', '#', '#', '#'},
                {'#', '.', '#', '.', '#'},
                {'#', '.', '#', '.', '#'},
                {'#', '.', '.', '.', '#'},
                {'#', '#', '#', '#', '#'}
            };

        public static void TestTCODFovTest()
        {
            int x = 1;
            int y = 1;

            using(TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "FOV Tester", false))
            {
        		TCODKeyboard keyboard =  new TCODKeyboard();
        	    console.Clear();

	            using (TCODFov fov = new TCODFov(5, 5))
	            {
	                for (int i = 0; i < 5; ++i)    //width
	                    for (int j = 0; j < 5; ++j) //height
	                        fov.SetCell(i, j, room[j, i] == '.', room[j, i] == '.');

	                TCOD_key key;
	                do
	                {
	                    PaintFOVTest(console, x, y, fov);

	                    key = keyboard.WaitForKeyPress(false);

	                    switch (key.vk)
	                    {
	                        case TCOD_keycode.TCODK_UP:
	                            if (room[y - 1, x] == '.')
	                                y--;
	                            break;
	                        case TCOD_keycode.TCODK_DOWN:
	                            if (room[y + 1, x] == '.')
	                                y++;
	                            break;
	                        case TCOD_keycode.TCODK_LEFT:
	                            if (room[y, x - 1] == '.')
	                                x--;
	                            break;
	                        case TCOD_keycode.TCODK_RIGHT:
	                            if (room[y, x + 1] == '.')
	                                x++;
	                            break;
	                    }
	                }
	                while (key.c != 'q' && !console.IsWindowClosed());
            	}
            }
        }

        private static void PaintFOVTest(TCODConsoleRoot console, int x, int y, TCODFov fov)
        {
            fov.CalculateFOV(x, y, 3);

            for (int i = 0; i < 5; ++i)    //width
            {
                for (int j = 0; j < 5; ++j) //height
                {
                    if (room[j, i] == '.')
                    {
                        if (fov.CheckTileFOV(i, j))
                            console.PutChar(i, j, '.');
                        else
                            console.PutChar(i, j, '~');
                    }
                    else
                    {
                        console.PutChar(i, j, '#');
                    }
                }
            }
            console.PutChar(x, y, '@');
            console.Flush();
        }
    }
}