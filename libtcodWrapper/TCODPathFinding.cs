using System;
using System.Runtime.InteropServices;

namespace libtcodWrapper
{
    /// <summary>
    /// Calculates paths in maps using djikstra's algorithms
    /// </summary>
    public class TCODPathFinding : IDisposable
    {
        /// <summary>
        /// Callback made from pathfinding engine to determine cell pathfinding information
        /// </summary>
        /// <param name="x">x coord</param>
        /// <param name="y">c coord</param>
        /// <returns>IsPassable?</returns>
        public delegate bool TCODPathCallback(int x, int y);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool TCODPathCallbackInternal(int x, int y, IntPtr nullPtr);

        bool TCODPathCallInternal(int x, int y, IntPtr nullPtr)
        {
            return m_callback(x, y);
        }

        private IntPtr m_instance;
        private TCODPathCallback m_callback;

        /// <summary>
        /// Create a new TCODPathFinding with a callback to determine cell information
        /// </summary>
        /// <param name="width">Map Width</param>
        /// <param name="height">Map Height</param>
        /// <param name="slowDiagonals">Diagonal move weights 1.4 compared to a horizontal/vertical move</param>
        /// <param name="callback">Callback from path finder</param>
        public TCODPathFinding(int width, int height, bool slowDiagonals, TCODPathCallback callback)
        {
            m_callback = callback;
            m_instance = TCOD_path_new_using_function(width, height, new TCODPathCallbackInternal(this.TCODPathCallInternal), IntPtr.Zero, slowDiagonals);
        }

        /// <summary>
        /// Create new TCODPathFinding using map from TCODFov instance
        /// </summary>
        /// <param name="fovMap">Existing map</param>
        /// <param name="slowDiagonals">Diagonal move weights 1.4 compared to a horizontal/vertical move</param>
        public TCODPathFinding(TCODFov fovMap, bool slowDiagonals)
        {
            m_instance = TCOD_path_new_using_map(fovMap.m_mapPtr, slowDiagonals);
        }

        /// <summary>
        /// Compute a path from source to destination. 
        /// </summary>
        /// <param name="origX">Starting point x coord</param>
        /// <param name="origY">Starting point y coord</param>
        /// <param name="destX">Destination point x coord</param>
        /// <param name="destY">Destination point y coord</param>
        /// <returns>IsPathFound?</returns>
        public bool ComputePath(int origX, int origY, int destX, int destY)
        {
            return TCOD_path_compute(m_instance, origX, origY, destX, destY);
        }

        /// <summary>
        /// Walk along a path. Fill x and y with previous step's coord to get next point.
        /// </summary>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <param name="recalculateWhenNeeded">If path comes to abrupt end, can we spend time looking for route?</param>
        /// <returns>MoreToWalkAlong?</returns>
        public bool WalkPath(ref int x, ref int y, bool recalculateWhenNeeded)
        {
            return TCOD_path_walk(m_instance, ref x, ref y, recalculateWhenNeeded);
        }

        /// <summary>
        /// Query individual point on path
        /// </summary>
        /// <param name="index">0-based index of points in path list</param>
        /// <param name="x">x coord of point</param>
        /// <param name="y">y coord of point</param>
        public void GetPointOnPath(int index, out int x, out int y)
        {
            TCOD_path_get(m_instance, index, out x, out y);
        }

        /// <summary>
        /// Returns if path is empty of points
        /// </summary>
        /// <returns>IsEmpty?</returns>
        public bool IsPathEmpty()
        {
            return TCOD_path_is_empty(m_instance);
        }

        /// <summary>
        /// Get remainding points on path
        /// </summary>
        /// <returns>Path Size</returns>
        public int GetPathSize()
        {
            return TCOD_path_size(m_instance);
        }

        /// <summary>
        /// Destory unmanaged pathfinding resource.
        /// </summary>
        public void Dispose()
        {
            TCOD_path_delete(m_instance);
        }

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_path_new_using_function(int map_width, int map_height, TCODPathCallbackInternal func, IntPtr nullData, bool slowDiagonals);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_path_new_using_map(IntPtr map, bool slowDiagonals);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_path_compute(IntPtr path, int origX, int origY, int destX, int destY);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_path_walk(IntPtr path, ref int x, ref int y, bool recalculate_when_needed);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_path_is_empty(IntPtr path);

        [DllImport(DLLName.name)]
        private extern static int TCOD_path_size(IntPtr path);

        [DllImport(DLLName.name)]
        private extern static void TCOD_path_get(IntPtr path, int index, out int x, out int y);
        
        [DllImport(DLLName.name)]
        private extern static void TCOD_path_delete(IntPtr path);
        #endregion

    }
}
