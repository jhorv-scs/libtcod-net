using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    public class TCODFov : IDisposable
    {
        private IntPtr m_mapPtr;

        public TCODFov(int width, int height)
        {
            m_mapPtr = TCOD_map_new(width, height);
            TCOD_map_clear(m_mapPtr);
        }

        public void ClearMap()
        {
            TCOD_map_clear(m_mapPtr);
        }

        public void Dispose()
        {
            TCOD_map_delete(m_mapPtr);
        }

        // x - width ; y - height
        public void SetCell(int x, int y, bool transparent, bool walkable)
        {
            TCOD_map_set_properties(m_mapPtr, x, y, transparent, walkable);
        }

        public void GetCell(int x, int y, out bool transparent, out bool walkable)
        {
            transparent = TCOD_map_is_transparent(m_mapPtr, x, y);
            walkable = TCOD_map_is_walkable(m_mapPtr, x, y);
        }

        public void CalculateFOV(int playerX, int playerY, int radius)
        {
            TCOD_map_compute_fov(m_mapPtr, playerX, playerY, radius);
        }

        public bool CheckTileFOV(int x, int y)
        {
            return TCOD_map_is_in_fov(m_mapPtr, x, y);
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
}
