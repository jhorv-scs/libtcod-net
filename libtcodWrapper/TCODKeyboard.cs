using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    #pragma warning disable 1591  //Disable warning about lack of xml comments

    /// <summary>
    /// Types of "special" keycodes"
    /// </summary>
    public enum TCOD_keycode
    {
        TCODK_NONE,
	    TCODK_ESCAPE,
	    TCODK_BACKSPACE,
	    TCODK_TAB,
	    TCODK_ENTER,
	    TCODK_SHIFT,
	    TCODK_CONTROL,
	    TCODK_ALT,
	    TCODK_PAUSE,
	    TCODK_CAPSLOCK,
	    TCODK_PAGEUP,
	    TCODK_PAGEDOWN,
	    TCODK_END,
	    TCODK_HOME,
	    TCODK_UP,
	    TCODK_LEFT,
	    TCODK_RIGHT,
	    TCODK_DOWN,
	    TCODK_PRINTSCREEN,
	    TCODK_INSERT,
	    TCODK_DELETE,
	    TCODK_LWIN,
	    TCODK_RWIN,
	    TCODK_APPS,
	    TCODK_0,
	    TCODK_1,
	    TCODK_2,
	    TCODK_3,
	    TCODK_4,
	    TCODK_5,
	    TCODK_6,
	    TCODK_7,
	    TCODK_8,
	    TCODK_9,
	    TCODK_KP0,
	    TCODK_KP1,
	    TCODK_KP2,
	    TCODK_KP3,
	    TCODK_KP4,
	    TCODK_KP5,
	    TCODK_KP6,
	    TCODK_KP7,
	    TCODK_KP8,
	    TCODK_KP9,
	    TCODK_KPADD,
	    TCODK_KPSUB,
	    TCODK_KPDIV,
	    TCODK_KPMUL,
	    TCODK_KPDEC,
	    TCODK_KPENTER,
	    TCODK_F1,
	    TCODK_F2,
	    TCODK_F3,
	    TCODK_F4,
	    TCODK_F5,
	    TCODK_F6,
	    TCODK_F7,
	    TCODK_F8,
	    TCODK_F9,
	    TCODK_F10,
	    TCODK_F11,
	    TCODK_F12,
	    TCODK_NUMLOCK,
	    TCODK_SCROLLLOCK,
	    TCODK_SPACE,
        TCODK_CHAR
    }
    
    /// <summary>
    /// Keystroke structure returned from TCODKeyboard methods
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TCOD_key
    {
        public TCOD_keycode vk; //  key code
		public byte c; // character if vk == TCODK_CHAR else 0
		
		//This field is set by libtcod when struct is marshalled. Disable the incorrect warning. 
		#pragma warning disable 0649
#if Linux
        private byte modifiers;
#else
        private int modifiers;
#endif
		#pragma warning restore 0649

        public bool pressed
        {
            get { return ((modifiers & 0x01) > 0); }
        }
        public bool lalt
        {
            get { return ((modifiers & 0x02) > 0); }
        }
        public bool lctrl
        {
            get { return ((modifiers & 0x04) > 0); }
        }
        public bool ralt
        {
            get { return ((modifiers & 0x8) > 0); }
        }
        public bool rctrl
        {
            get { return ((modifiers & 0x10) > 0); }
        }
        public bool shift
        {
            get { return ((modifiers & 0x20) > 0); }
        }
    }

    /// <summary>
    /// Is event returned when key is pressed, release, or both?
    /// </summary>
    public enum TCOD_keypressed
    {
        TCOD_KEY_PRESSED =1,
	    TCOD_KEY_RELEASED=2,
        TCOD_KEY_PRESSEDANDRELEASED=3,
    };

    #pragma warning restore 1591  //Disable warning about lack of xml comments

    /// <summary>
    /// Holds static methods for interacting with keyboard
    /// </summary>
    public class TCODKeyboard
    {
        /// <summary>
        /// Block until user presses key
        /// </summary>
        /// <param name="flushInputBuffer">Flush all outstanding keystrokes and wait for next stroke</param>
        /// <returns>Keypress</returns>
        public static TCOD_key WaitForKeyPress(bool flushInputBuffer)
        {
            return TCOD_console_wait_for_keypress(flushInputBuffer);
        }

        /// <summary>
        /// Non-blockingly check for user key press
        /// </summary>
        /// <param name="pressFlags">Determines what type of events are returned</param>
        /// <returns>Keypress</returns>
        public static TCOD_key CheckForKeypress(TCOD_keypressed pressFlags)
        {
            return TCOD_console_check_for_keypress(pressFlags);
        }

        /// <summary>
        /// Determine if a given key is currently being pressed
        /// </summary>
        /// <param name="key">Key in question</param>
        /// <returns>Is Key Pressed?</returns>
        public static bool IsKeyPressed(TCOD_keycode key)
        {
            return TCOD_console_is_key_pressed(key);
        }

        /// <summary>
        /// Set repeat rate of keyboard
        /// </summary>
        /// <param name="initialDelay">How long before repeating</param>
        /// <param name="interval">How often afterwards to repeat</param>
        public static void SetRepeat(int initialDelay, int interval)
        {
            TCOD_console_set_keyboard_repeat(initialDelay, interval);
        }

        /// <summary>
        /// Disable all repeating of keystrokes
        /// </summary>
        public static void DisableRepeat()
        {
            TCOD_console_disable_keyboard_repeat();
        }

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static TCOD_key TCOD_console_wait_for_keypress(bool flush);

        [DllImport(DLLName.name)]
        private extern static TCOD_key TCOD_console_check_for_keypress(TCOD_keypressed flags);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_console_is_key_pressed(TCOD_keycode key);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_keyboard_repeat(int initial_delay, int interval);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_disable_keyboard_repeat();
        #endregion
    }
}
