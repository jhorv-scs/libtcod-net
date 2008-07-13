using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    /// <summary>
    /// Represents System Level Functions
    /// </summary>
    public class TCODSystem
    {
        /// <summary>
        /// Obtain number of milliseconds since the program started
        /// </summary>
        /// <returns>Milliseconds since the program started</returns>
        public static UInt32 GetElapsedMilli()
        {
            return TCOD_sys_elapsed_milli();
        }

        /// <summary>
        /// Obtain number of seconds since the program started
        /// </summary>
        /// <returns>Seconds since the program started</returns>
        public static float GetElapsedSeconds()
        {
            return TCOD_sys_elapsed_seconds();
        }

        /// <summary>
        /// Stop program execution for specified amount of time
        /// </summary>
        /// <param name="milliseconds">Milliseconds to sleep</param>
        public static void Sleep(UInt32 milliseconds)
        {
            TCOD_sys_sleep_milli(milliseconds);
        }

        /// <summary>
        /// Save a screenshot of the root console to disk
        /// </summary>
        /// <param name="fileName">Filename to save image to</param>
        public static void SaveScreenshot(String fileName)
        {
            TCOD_sys_save_screenshot(new StringBuilder(fileName));
        }

        /// <summary>
        /// Force specific resolution in fullscreen
        /// </summary>
        /// <param name="width">Fullscreen Width</param>
        /// <param name="height">Fullscreen Height</param>
        public static void ForceFullscrenResolution(int width, int height)
        {
            TCOD_sys_force_fullscreen_resolution(width, height);
        }

        /// <summary>
        /// Get Current Screen Resolution
        /// </summary>
        /// <param name="w">Current Width</param>
        /// <param name="h">Current Height</param>
        public static void GetCurrentResolution(out int w, out int h)
        {
            TCOD_sys_get_current_resolution(out w, out h);
        }

        /// <summary>
        /// Return the length in seconds of the last rendered frame.
        /// </summary>
        /// <returns>Number of seconds</returns>
        public static float GetLastFrameLength()
        {
            return TCOD_sys_get_last_frame_length();
        }

        /// <summary>
        /// Limit number of frames per seconds rendered to this value.
        /// </summary>
        /// <param name="fps">Frames Per Second</param>
        public static void SetFPS(int fps)
        {
            TCOD_sys_set_fps(fps);
        }

        /// <summary>
        /// Obtain number of frames rendered in the last second.
        /// </summary>
        /// <returns>Number of Seconds</returns>
        public static int GetFPS()
        {
            return TCOD_sys_get_fps();
        }

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static uint TCOD_sys_elapsed_milli();

        [DllImport(DLLName.name)]
        private extern static float TCOD_sys_elapsed_seconds();

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_sleep_milli(UInt32 val);

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_save_screenshot(StringBuilder filename);

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_force_fullscreen_resolution(int width, int height);

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_get_current_resolution(out int w, out int h);

        [DllImport(DLLName.name)]
        private extern static float TCOD_sys_get_last_frame_length();

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_set_fps(int val);

        [DllImport(DLLName.name)]
        private extern static int TCOD_sys_get_fps();
        #endregion
    }
}
