using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libtcodWrapper
{
    /// <summary>
    /// TCOD Pathfinding use Dijkstra's
    /// </summary>
    public class TCODDijkstraPathFinding : TCODPathFindingBase
    {
        /// <summary>
        /// Create a new TCODDijkstraPathFinding with a callback to determine cell information
        /// </summary>
        /// <param name="width">Map Width</param>
        /// <param name="height">Map Height</param>
        /// <param name="diagonalCost">Factor diagonal moves cost more</param>
        /// <param name="callback">Callback from path finder</param>
        public TCODDijkstraPathFinding(int width, int height, double diagonalCost, TCODPathCallback callback)
        {
            m_callback = callback;
            m_internalCallback = new TCODPathCallbackInternal(this.TCODPathCallInternal);
            m_instance = TCOD_dijkstra_new_using_function(width, height, m_internalCallback, IntPtr.Zero, (float)diagonalCost);
        }

        /// <summary>
        /// Create new TCODDijkstraPathFinding using map from TCODFov instance
        /// </summary>
        /// <param name="fovMap">Existing map</param>
        /// <param name="diagonalCost">Factor diagonal moves cost more</param>
        public TCODDijkstraPathFinding(TCODFov fovMap, double diagonalCost)
        {
            m_instance = TCOD_dijkstra_new(fovMap.m_mapPtr, (float)diagonalCost);
        }

        /// <summary>
        /// Compute distances with a given origin point
        /// </summary>
        /// <param name="origX">Origin X Position</param>
        /// <param name="origY">Origin Y Position</param>
        /// <returns></returns>
        public void Compute(int origX, int origY)
        {
            TCOD_dijkstra_compute(m_instance, origX, origY);
        }

        /// <summary>
        /// Calculate distance from origin to given point
        /// </summary>
        /// <param name="x">Destination x</param>
        /// <param name="y">Destination y</param>
        public float GetDistance(int x, int y)
        {
            return TCOD_dijkstra_get_distance(m_instance, x, y);
        }

        /// <summary>
        /// Set path to given point
        /// </summary>
        /// <param name="destX">Destination X</param>
        /// <param name="destY">Destination Y</param>
        /// <returns>If there's a path from the root node to the destination node</returns>
        public bool SetPath(int destX, int destY)
        {
            return TCOD_dijkstra_path_set(m_instance, destX, destY);
        }

        /// <summary>
        /// Walk a path set by SetPath to the next position
        /// </summary>
        /// <param name="x">Set to current X spot on path, updated to next X spot</param>
        /// <param name="y">Set to current Y spot on path, update to next Y spot</param>
        public void WalkPath(ref int x, ref int y)
        {
            TCOD_dijkstra_path_walk(m_instance, ref x, ref y);
        }

        /// <summary>
        /// Is the currently set path empty
        /// </summary>
        /// <returns>Is current path empty</returns>
        public bool IsPathEmpty()
        {
            return TCOD_dijkstra_is_empty(m_instance);
        }

        /// <summary>
        /// Current Path's Length
        /// </summary>
        /// <returns>Current Path Length</returns>
        public int PathLength()
        {
            return TCOD_dijkstra_size(m_instance);
        }

        /// <summary>
        /// Query individual point on path
        /// </summary>
        /// <param name="index">0-based index of points in path list</param>
        /// <param name="x">x coord of point</param>
        /// <param name="y">y coord of point</param>
        public void GetPointOnPath(int index, out int x, out int y)
        {
            TCOD_dijkstra_get(m_instance, index, out x, out y);
        }

        /// <summary>
        /// Dispose unmanaged resources
        /// </summary>
        public override void Dispose()
        {
            TCOD_dijkstra_delete(m_instance);
        }

        #region DllImport
        // Dijkstra's
        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_dijkstra_new_using_function(int map_width, int map_height, TCODPathCallbackInternal func, IntPtr nullData, float diagonalCost);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_dijkstra_new(IntPtr map, float diagonalCost);

        [DllImport(DLLName.name)]
        [return: MarshalAs(UnmanagedType.I1)]
        private extern static void TCOD_dijkstra_compute(IntPtr path, int origX, int origY);

        [DllImport(DLLName.name)]
        private extern static float TCOD_dijkstra_get_distance (IntPtr dijkstra, int x, int y);

        [DllImport(DLLName.name)]
        [return: MarshalAs(UnmanagedType.I1)]
        private extern static bool TCOD_dijkstra_path_set (IntPtr dijkstra, int x, int y);

        [DllImport(DLLName.name)]
        [return: MarshalAs(UnmanagedType.I1)]
        private extern static bool TCOD_dijkstra_path_walk(IntPtr path, ref int x, ref int y);

        [DllImport(DLLName.name)]
        [return: MarshalAs(UnmanagedType.I1)]
        private extern static bool TCOD_dijkstra_is_empty(IntPtr path);

        [DllImport(DLLName.name)]
        private extern static int TCOD_dijkstra_size(IntPtr path);

        [DllImport(DLLName.name)]
        private extern static void TCOD_dijkstra_get(IntPtr path, int index, out int x, out int y);

        [DllImport(DLLName.name)]
        private extern static void TCOD_dijkstra_delete(IntPtr path);

        #endregion
    }
}
