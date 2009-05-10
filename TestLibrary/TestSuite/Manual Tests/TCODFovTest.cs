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

            RootConsole.Width = 80;
            RootConsole.Height = 50;
            RootConsole.WindowTitle = "FOV Tester";
            RootConsole.Fullscreen = false;

            using(RootConsole console = RootConsole.GetInstance())
            {
                console.Clear();

                using (TCODFov fov = new TCODFov(5, 5))
                {
                    for (int i = 0; i < 5; ++i)    //width
                        for (int j = 0; j < 5; ++j) //height
                            fov.SetCell(i, j, room[j, i] == '.', room[j, i] == '.');

                    KeyPress key;
                    do
                    {
                        PaintFOVTest(console, x, y, fov);

                        key = Keyboard.WaitForKeyPress(false);

                        switch (key.KeyCode)
                        {
                            case KeyCode.TCODK_UP:
                                if (room[y - 1, x] == '.')
                                    y--;
                                break;
                            case KeyCode.TCODK_DOWN:
                                if (room[y + 1, x] == '.')
                                    y++;
                                break;
                            case KeyCode.TCODK_LEFT:
                                if (room[y, x - 1] == '.')
                                    x--;
                                break;
                            case KeyCode.TCODK_RIGHT:
                                if (room[y, x + 1] == '.')
                                    x++;
                                break;
                        }
                    }
                    while (key.Character != 'q' && !console.IsWindowClosed());
                }
            }
        }

        private static void PaintFOVTest(RootConsole console, int x, int y, TCODFov fov)
        {
            fov.CalculateFOV(x, y, 3, false, FovAlgorithm.Basic);

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
