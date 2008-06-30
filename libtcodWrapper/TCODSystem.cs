using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    public class TCODSystem
    {
        public static UInt32 GetElapsedMilli()
        {
            return TCOD_sys_elapsed_milli();
        }

        public static float GetElapsedSeconds()
        {
            return TCOD_sys_elapsed_seconds();
        }

        public static void Sleep(UInt32 miliseconds)
        {
            TCOD_sys_sleep_milli(miliseconds);
        }

        public static void saveScreenshot(String fileName)
        {
            TCOD_sys_save_screenshot(new StringBuilder(fileName));
        }

        public static void ForceFullscrenResolution(int width, int height)
        {
            TCOD_sys_force_fullscreen_resolution(width, height);
        }

        public static void GetCurrentResolution(ref int w, ref int h)
        {
            TCOD_sys_get_current_resolution(ref w, ref h);
        }

        public static float GetLastFrameLength()
        {
            return TCOD_sys_get_last_frame_length();
        }

        public static void SetFPS(int fps)
        {
            TCOD_sys_set_fps(fps);
        }

        public static int GetFPS()
        {
            return TCOD_sys_get_fps();
        }

        [DllImport(DLLName.name)]
        private extern static UInt32 TCOD_sys_elapsed_milli();

        [DllImport(DLLName.name)]
        private extern static float TCOD_sys_elapsed_seconds();

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_sleep_milli(UInt32 val);

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_save_screenshot(StringBuilder filename);

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_force_fullscreen_resolution(int width, int height);

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_get_current_resolution(ref int w, ref int h);

        [DllImport(DLLName.name)]
        private extern static float TCOD_sys_get_last_frame_length();

        [DllImport(DLLName.name)]
        private extern static void TCOD_sys_set_fps(int val);

        [DllImport(DLLName.name)]
        private extern static int TCOD_sys_get_fps();
    }
}
