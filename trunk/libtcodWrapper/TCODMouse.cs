using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    /// <summary>
    /// Constains information about current mouse status
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Mouse 
    {
        
        private int x;
        /// <summary>
        /// Position in x direction in pixels
        /// </summary>
        public int PixelX
        {
            get { return x; }
        }

        private int y;
        /// <summary>
        /// Position in y direction in pixels
        /// </summary>
        public int PixelY
        {
            get { return y; }
        }

        /// <summary>
        /// Position of the mouse cursor.
        /// </summary>
        public System.Drawing.Point PixelLocation
        {
            get { return new System.Drawing.Point(x, y); }
        }

        
        private int dx;
        /// <summary>
        /// Mouse velocity in x direction in pixels
        /// </summary>
        public int PixelVelocityX
        {
            get { return dx; }
        }

        private int dy;
        /// <summary>
        /// Mouse velocity in y direction in pixels
        /// </summary>
        public int PixelVelocityY
        {
            get { return dy; }
        }

        private int cx;
        /// <summary>
        /// Position in x directory in character sized blocks
        /// </summary>
        public int CellX
        {
            get { return cx; }
        }

        private int cy;
        /// <summary>
        /// Position in Y directory in character sized blocks
        /// </summary>
        public int CellY
        {
            get { return cy; }
        }

        /// <summary>
        /// Position of the mouse cursor character sized blocks
        /// </summary>
        public System.Drawing.Point CellLocation
        {
            get { return new System.Drawing.Point(cx, cy); }
        }

        private int dcx;
        /// <summary>
        /// Mouse velocity in x direction in character sized blocks
        /// </summary>
        public int CellVelocityX
        {
            get { return dcx; }
        }

        private int dcy;
        /// <summary>
        /// Mouse velocity in y direction in character sized blocks
        /// </summary>
        public int CellVelocityY
        {
            get { return dcy; }
        }
                
        #pragma warning disable 1591  //Disable warning about lack of xml comments
        private byte lbutton;
        public bool LeftButton
        {
            get { return lbutton == 1; }
        }

        private byte rbutton;
        public bool RightButton
        {
            get { return rbutton == 1; }
        }

        private byte mbutton;
        public bool MiddleButton
        {
            get { return mbutton == 1; }
        }

        private byte lbutton_pressed;
        public bool LeftButtonPressed
        {
            get { return lbutton_pressed == 1; }
        }

        private byte rbutton_pressed;
        public bool RightButtonPressed
        {
            get { return rbutton_pressed == 1; }
        }

        private byte mbutton_pressed;
        public bool MiddleButtonPressed
        {
            get { return mbutton_pressed == 1; }
        }

        private byte wheel_up;
        public bool WheelUp
        {
            get { return wheel_up == 1; }
        }

        private byte wheel_down;
        public bool WheelDown
        {
            get { return wheel_down == 1; }
        }

        #pragma warning restore 1591

        /// <summary>
        /// Changes visiblity of mouse while in our window(s)
        /// </summary>
        /// <param name="visible">Is mouse visible?</param>
        public static void ShowCursor(bool visible)
        {
            TCOD_mouse_show_cursor(visible);
        }
        
        /// <summary>
        /// Return if cursor is visible
        /// </summary>
        /// <returns>Is Visible?</returns>
        public static bool IsVisible
        {
            get { return TCOD_mouse_is_cursor_visible(); }
        }
        
        /// <summary>
        /// Move user's mouse to that location
        /// </summary>
        /// <param name="x">Pixel x location</param>
        /// <param name="y">Pixel y location</param>
        public static void MoveMouse(int x, int y)
        {
            TCOD_mouse_move(x, y);
        }
        
        /// <summary>
        /// Get current mouse status
        /// </summary>
        /// <returns>Mouse struct with location, movement, and buttom presses</returns>
        public static Mouse GetStatus()
        {
            return TCOD_mouse_get_status();
        }

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static void TCOD_mouse_show_cursor(bool visible);
        
        [DllImport(DLLName.name)]
        [return: MarshalAs(UnmanagedType.I1)]
        private extern static bool TCOD_mouse_is_cursor_visible();
        
        [DllImport(DLLName.name)]
        private extern static void TCOD_mouse_move(int x, int y);
        
        [DllImport(DLLName.name)]
        private extern static Mouse TCOD_mouse_get_status();
        #endregion
    };

}
