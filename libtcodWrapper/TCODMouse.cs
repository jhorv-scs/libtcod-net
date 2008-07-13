using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    [StructLayout(LayoutKind.Sequential)]
	public struct TCODMouse 
	{
		public int x;
		public int y;
		public int dx;
		public int dy;
		public int cx;
		public int cy;
		public int dcx;
		public int dcy;
		
		//This field is set by libtcod when struct is marshalled. Disable the incorrect warning. 
		#pragma warning disable 0649
		private byte modifiers;
		#pragma warning restore 0649

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