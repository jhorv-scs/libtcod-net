using System;
using System.Runtime.InteropServices;

namespace libtcodWrapper
{   
    /// <summary>
    /// Callback made from pathfinding engine to determine cell pathfinding information
    /// </summary>
    /// <param name="xFrom">staring x coord</param>
    /// <param name="yFrom">starting y coord</param>
    /// <param name="xTo">ending x coord</param>
    /// <param name="yTo">ending y coord</param>
    /// <returns>"Cost" to pass through cell</returns>
    public delegate float TCODPathCallback(int xFrom, int yFrom, int xTo, int yTo);

    /// <summary>
    /// Base class for calculating path, A* and Dijkstra are specific instances
    /// </summary>
    public abstract class TCODPathFindingBase : IDisposable
    {
        /// <summary>
        /// Internal function used to trampoline between c# and unmanaged worlds
        /// </summary>
        /// <param name="xFrom"></param>
        /// <param name="yFrom"></param>
        /// <param name="xTo"></param>
        /// <param name="yTo"></param>
        /// <param name="nullPtr"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        protected delegate float TCODPathCallbackInternal(int xFrom, int yFrom, int xTo, int yTo, IntPtr nullPtr);

        /// <summary>
        /// Internal function used to trampoline between c# and unmanaged worlds
        /// </summary>
        /// <param name="xFrom"></param>
        /// <param name="yFrom"></param>
        /// <param name="xTo"></param>
        /// <param name="yTo"></param>
        /// <param name="nullPtr"></param>
        /// <returns></returns>
        protected float TCODPathCallInternal(int xFrom, int yFrom, int xTo, int yTo, IntPtr nullPtr)
        {
            return m_callback(xFrom, yFrom, xTo, yTo);
        }

        /// <summary>
        /// Internal instance pointer
        /// </summary>
        protected IntPtr m_instance;

        /// <summary>
        /// Internal callback function
        /// </summary>
        protected TCODPathCallback m_callback;

        /// <summary>
        /// Internal callback function
        /// </summary>
        protected TCODPathCallbackInternal m_internalCallback;

        /// <summary>
        /// Used to clean up unmanaged resources
        /// </summary>
        public abstract void Dispose();
    }
}
