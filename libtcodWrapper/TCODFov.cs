using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    class TCODFov : IDisposable
    {
        private IntPtr mapPtr;

        public TCODFov(int width, int height)
        {
            mapPtr = TCOD_map_new(width, height);
            TCOD_map_clear(mapPtr);
        }

        public void ClearMap()
        {
            TCOD_map_clear(mapPtr);
        }

        public void Dispose()
        {
            TCOD_map_delete(mapPtr);
        }

        // x - width ; y - height
        public void SetCell(int x, int y, bool transparent, bool walkable)
        {
            TCOD_map_set_properties(mapPtr, x, y, transparent, walkable);
        }

        public void GetCell(int x, int y, out bool transparent, out bool walkable)
        {
            transparent = TCOD_map_is_transparent(mapPtr, x, y);
            walkable = TCOD_map_is_walkable(mapPtr, x, y);
        }

        public void CalculateFOV(int playerX, int playerY, int radius)
        {
            TCOD_map_compute_fov(mapPtr, playerX, playerY, radius);
        }

        public bool CheckTileFOV(int x, int y)
        {
            return TCOD_map_is_in_fov(mapPtr, x, y);
        }

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_map_new(int width, int height);

        // set all cells as solid rock (cannot see through nor walk)
        [DllImport(DLLName.name)]
        private extern static void TCOD_map_clear(IntPtr map);

        // change a cell properties
        [DllImport(DLLName.name)]
        private extern static void TCOD_map_set_properties(IntPtr map, int x, int y, bool is_transparent, bool is_walkable);

        // destroy a map
        [DllImport(DLLName.name)]
        private extern static void TCOD_map_delete(IntPtr map);

        // calculate the field of view (potentially visible cells from player_x,player_y)
        [DllImport(DLLName.name)]
        private extern static void TCOD_map_compute_fov(IntPtr map, int player_x, int player_y, int max_radius);

        // check if a cell is in the last computed field of view
        [DllImport(DLLName.name)]
        private extern static bool TCOD_map_is_in_fov(IntPtr map, int x, int y);

        // retrieve properties from the map
        [DllImport(DLLName.name)]
        private extern static bool TCOD_map_is_transparent(IntPtr map, int x, int y);
        
        [DllImport(DLLName.name)]
        private extern static bool TCOD_map_is_walkable(IntPtr map, int x, int y);


    }

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

            TCODConsoleRoot console = new TCODConsoleRoot(80, 50, "FOV Tester", false);
            TCODKeyboard keyboard = new TCODKeyboard();

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
                while (key.c != 'q');
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
