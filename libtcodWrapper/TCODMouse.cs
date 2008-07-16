using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    /// <summary>
    /// Constains information about current mouse status
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
	public struct TCODMouse 
	{
        /// <summary>
        /// Position in x direction in pixels
        /// </summary>
		public int x;

        /// <summary>
        /// Position in y direction in pixels
        /// </summary>
		public int y;

        /// <summary>
        /// Mouse velocity in x direction in pixels
        /// </summary>
		public int dx;

        /// <summary>
        /// Mouse velocity in y direction in pixels
        /// </summary>
		public int dy;

        /// <summary>
        /// Position in x directory in character sized blocks
        /// </summary>
		public int cx;

        /// <summary>
        /// Position in y directory in character sized blocks
        /// </summary>
		public int cy;

        /// <summary>
        /// Mouse velocity in x direction in character sized blocks
        /// </summary>
		public int dcx;

        /// <summary>
        /// Mouse velocity in y direction in character sized blocks
        /// </summary>
		public int dcy;
		
		//This field is set by libtcod when struct is marshalled. Disable the incorrect warning. 
		#pragma warning disable 0649
		private byte modifiers;
		#pragma warning restore 0649

        #pragma warning disable 1591  //Disable warning about lack of xml comments
        public bool lbutton
		{
			get { return ((modifiers & 0x01) > 0); }
        }
        public bool rbutton
        {
            get { return ((modifiers & 0x02) > 0); }
        }
        public bool mbutton
        {
            get { return ((modifiers & 0x4) > 0); }
        }
        public bool lbutton_pressed
        {
            get { return ((modifiers & 0x8) > 0); }
        }
        public bool rbutton_pressed
        {
            get { return ((modifiers & 0x10) > 0); }
        }
		public bool mbutton_pressed
        {
            get { return ((modifiers & 0x20) > 0); }
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
		public static  bool IsVisible()
		{
			return TCOD_mouse_is_cursor_visible();
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
        /// <returns>TCODMouse struct with location, movement, and buttom presses</returns>
		public static TCODMouse GetStatus()
		{
			return TCOD_mouse_get_status();
        }

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static void TCOD_mouse_show_cursor(bool visible);
		
		[DllImport(DLLName.name)]
        private extern static bool TCOD_mouse_is_cursor_visible();
		
		[DllImport(DLLName.name)]
        private extern static void TCOD_mouse_move(int x, int y);
		
		[DllImport(DLLName.name)]
        private extern static TCODMouse TCOD_mouse_get_status();
        #endregion
    };

}