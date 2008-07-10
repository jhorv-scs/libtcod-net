using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    internal class DLLName
    {
#if Linux
        internal const string name = @"libtcod.so";
#else
        internal const string name = @"libtcod.dll";
#endif
		
    }
    
    public class TCODBackground
    {
    	internal int m_value;
    	
    	public TCODBackground(TCOD_bkgnd_flag flag)
    	{
    		if(flag == TCOD_bkgnd_flag.TCOD_BKGND_ADDA || flag == TCOD_bkgnd_flag.TCOD_BKGND_ALPH)
    			throw new Exception("Must use TCODBackagroudn constructor which takes value");
    		m_value = (int)flag;
		}
		
		public TCODBackground(TCOD_bkgnd_flag flag, float val)
    	{
            NewBackgroundCore(flag, val);
		}

        public TCODBackground(TCOD_bkgnd_flag flag, double val)
        {
            NewBackgroundCore(flag, (float)val);
        }

        private void NewBackgroundCore(TCOD_bkgnd_flag flag, float val)
        {
            if (flag != TCOD_bkgnd_flag.TCOD_BKGND_ADDA && flag != TCOD_bkgnd_flag.TCOD_BKGND_ALPH)
                throw new Exception("Must not use TCODBackagroudn constructor which takes value");
            m_value = (int)flag | (((byte)(val * 255)) << 8);
        }


        public static TCODBackground operator ++(TCODBackground lhs)
        {
            if (lhs.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ALPH)
                throw new Exception("Can not increment past end of TCOD_bkgnd_flag enum");
            lhs.m_value += 1;
            return lhs;
        }

        public static TCODBackground operator --(TCODBackground lhs)
        {
            if (lhs.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_NONE)
                throw new Exception("Can not decrement past end of TCOD_bkgnd_flag enum");
            lhs.m_value -= 1;
            return lhs;
        }

        public TCOD_bkgnd_flag GetBackgroundFlag()
        {
            return (TCOD_bkgnd_flag)(m_value & 0xff);
        }

        public byte GetAlphaValue()
        {
            return (byte)(m_value >> 8);
        }
	}

    public enum TCOD_bkgnd_flag
    {
        TCOD_BKGND_NONE,
        TCOD_BKGND_SET,
        TCOD_BKGND_MULTIPLY,
        TCOD_BKGND_LIGHTEN,
        TCOD_BKGND_DARKEN,
        TCOD_BKGND_SCREEN,
        TCOD_BKGND_COLOR_DODGE,
        TCOD_BKGND_COLOR_BURN,
        TCOD_BKGND_ADD,
        TCOD_BKGND_ADDA,
        TCOD_BKGND_BURN,
        TCOD_BKGND_OVERLAY,
        TCOD_BKGND_ALPH
    };
    
    public class TCODSpecialChar 
    {
		public const char TCOD_CHAR_HLINE = (char)196;
		public const char TCOD_CHAR_VLINE = (char)179;
		public const char TCOD_CHAR_NE = (char)191;
		public const char TCOD_CHAR_NW = (char)218; 
		public const char TCOD_CHAR_SE = (char)217; 
		public const char TCOD_CHAR_SW = (char)192;
		public const char TCOD_CHAR_DHLINE = (char)205;
		public const char TCOD_CHAR_DVLINE = (char)186;
		public const char TCOD_CHAR_DNE = (char)187;
		public const char TCOD_CHAR_DNW = (char)201; 
		public const char TCOD_CHAR_DSE = (char)188; 
		public const char TCOD_CHAR_DSW = (char)200;
		public const char TCOD_CHAR_TEEW = (char)180; 
		public const char TCOD_CHAR_TEEE = (char)195; 
		public const char TCOD_CHAR_TEEN = (char)193;
		public const char TCOD_CHAR_TEES = (char)194;
		public const char TCOD_CHAR_DTEEW = (char)181; 
		public const char TCOD_CHAR_DTEEE = (char)198; 
		public const char TCOD_CHAR_DTEEN = (char)208;
		public const char TCOD_CHAR_DTEES = (char)210;
		public const char TCOD_CHAR_CHECKER = (char)178; 
		public const char TCOD_CHAR_BLOCK = (char)219;
		public const char TCOD_CHAR_BLOCK1 = (char)178;
		public const char TCOD_CHAR_BLOCK2 = (char)177; 
		public const char TCOD_CHAR_BLOCK3 = (char)176;
		public const char TCOD_CHAR_BLOCK_B = (char)220; 
		public const char TCOD_CHAR_BLOCK_T = (char)223;
		public const char TCOD_CHAR_DS_CROSSH = (char)216; 
		public const char TCOD_CHAR_DS_CROSSV = (char)215;
		public const char TCOD_CHAR_CROSS = (char)197;
		public const char TCOD_CHAR_LIGHT = (char)15;
		public const char TCOD_CHAR_TREE = (char)5;
		public const char TCOD_CHAR_ARROW_N = (char)24; 
		public const char TCOD_CHAR_ARROW_S = (char)25; 
		public const char TCOD_CHAR_ARROW_E = (char)26; 
		public const char TCOD_CHAR_ARROW_W = (char)27;
	};

    public struct CustomFontRequest
    {
        public CustomFontRequest(String fontFile, int char_width, int char_height, int nb_char_horiz, int nb_char_vertic, bool chars_by_row, TCODColor key_color)
        {
            m_fontFile = fontFile;
            m_char_width = char_width;
            m_char_height = char_height;
            m_nb_char_horiz = nb_char_horiz;
            m_nb_char_vertic = nb_char_vertic;
            m_chars_by_row = chars_by_row;
            m_key_color = key_color;
        }

        public String m_fontFile;
        public int m_char_width;
        public int m_char_height;
        public int m_nb_char_horiz;
        public int m_nb_char_vertic;
        public bool m_chars_by_row;
        public TCODColor m_key_color;
    }

    public enum TCODLineAlign
    {
        Left,
        Right,
        Center
    }

    public class TCODConsole : IDisposable
    {
        internal TCODConsole(IntPtr w)
        {
            m_consolePtr = w;
        }
        
        public void Dispose()
        {
            //Don't try to dispose Root Consoles
            if (m_consolePtr != IntPtr.Zero)
                TCOD_console_delete(m_consolePtr);
        }

        internal IntPtr m_consolePtr;

        public void SetBackgroundColor(TCODColor background)
        {
            TCOD_console_set_background_color(m_consolePtr, background);
        }

        public void SetForegroundColor(TCODColor foreground)
        {
            TCOD_console_set_foreground_color(m_consolePtr, foreground);
        }

        public void Clear()
        {
            TCOD_console_clear(m_consolePtr);
        }

        public void PutChar(int x, int y, char c, TCODBackground flag)
        {
            TCOD_console_put_char(m_consolePtr, x, y, (int)c, flag.m_value);
        }

        public void PutChar(int x, int y, char c)
        {
            PutChar(x, y, c, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_NONE));
        }

        public void SetCharBackground(int x, int y, TCODColor col, TCODBackground flag)
        {
            TCOD_console_set_back(m_consolePtr, x, y, col, flag.m_value);
        }

        public void SetCharForeground(int x, int y, TCODColor col)
        {
            TCOD_console_set_fore(m_consolePtr, x, y, col);
        }

        public void SetCharAscii(int x, int y, char c)
        {
            TCOD_console_set_char(m_consolePtr, x, y, (int)c);
        }

        public TCODColor GetBackgroudColor()
        {
            return TCOD_console_get_background_color(m_consolePtr);
        }

        public TCODColor GetForegroundColor()
        {
            return TCOD_console_get_foreground_color(m_consolePtr);
        }

        public TCODColor GetCharBackground(int x, int y)
        {
            return TCOD_console_get_back(m_consolePtr, x, y);
        }

        public TCODColor GetCharForground(int x, int y)
        {
            return TCOD_console_get_fore(m_consolePtr, x, y);
        }

        public char GetChar(int x, int y)
        {
            return (char)TCOD_console_get_char(m_consolePtr, x, y);
        }

        public void PrintLine(string str, int x, int y, TCODLineAlign align)
        {
            PrintLine(str, x, y, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_NONE), align);
        }
        public void PrintLine(string str, int x, int y, TCODBackground flag, TCODLineAlign align)
        {
            switch (align)
            {
                case TCODLineAlign.Left:
                    TCOD_console_print_left(m_consolePtr, x, y, flag.m_value, new StringBuilder(str));
                    break;
                case TCODLineAlign.Center:
                    TCOD_console_print_center(m_consolePtr, x, y, flag.m_value, new StringBuilder(str));
                    break;
                case TCODLineAlign.Right:
                    TCOD_console_print_right(m_consolePtr, x, y, flag.m_value, new StringBuilder(str));
                    break;
            }
        }

        public int PrintLineRect(string str, int x, int y, int w, int h, TCODLineAlign align)
        {
            return PrintLineRect(str, x, y, w, h, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_NONE), align);
        }

        public int PrintLineRect(string str, int x, int y, int w, int h, TCODBackground flag, TCODLineAlign align)
        {
            switch (align)
            {
                case TCODLineAlign.Left:
                    return TCOD_console_print_left_rect(m_consolePtr, x, y, w, h, flag.m_value, new StringBuilder(str));
                case TCODLineAlign.Center:
                    return TCOD_console_print_center_rect(m_consolePtr, x, y, w, h, flag.m_value, new StringBuilder(str));
                case TCODLineAlign.Right:
                    return TCOD_console_print_right_rect(m_consolePtr, x, y, w, h, flag.m_value, new StringBuilder(str));
                default:
                    throw new Exception("Must Pass Alignment to PrintLineRect");
            }
        }

        public void Blit(int xSrc, int ySrc, int wSrc, int hSrc, TCODConsole dest, int xDst, int yDst, int fade)
        {
            TCOD_console_blit(m_consolePtr, xSrc, ySrc, wSrc, hSrc, dest.m_consolePtr, xDst, yDst, fade);
        }

        public void DrawRect(int x, int y, int w, int h, bool clear, TCODBackground flag)
        {
            TCOD_console_rect(m_consolePtr, x, y, w, h, clear, flag.m_value);
        }
        
		public void DrawRect(int x, int y, int w, int h, bool clear)
        {
             DrawRect(x, y, w, h, clear, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_NONE));
        }

        public void DrawHLine(int x, int y, int l)
        {
            TCOD_console_hline(m_consolePtr, x, y, l);
        }

        public void DrawVLine(int x, int y, int l)
        {
            TCOD_console_vline(m_consolePtr, x, y, l);
        }
        
        public void DrawBox(int x, int y, int w, int h, bool clear, String str)
        {
            TCOD_console_print_frame(m_consolePtr, x, y, w, h, clear, new StringBuilder(str));
        }

        #region DLLImports

        /* Printing shapes to console */

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_rect(IntPtr con, int x, int y, int w, int h, bool clear, /*TCOD_bkgnd_flag*/ int flag);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_hline(IntPtr con, int x, int y, int l);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_vline(IntPtr con, int x, int y, int l);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_print_frame(IntPtr con, int x, int y, int w, int h, bool clear, StringBuilder str);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_blit(IntPtr src, int xSrc, int ySrc, int wSrc, int hSrc, IntPtr dst, int xDst, int yDst, int fade);

        /* Prints Strings to Screen */

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_print_left(IntPtr con, int x, int y, /*TCOD_bkgnd_flag*/ int flag, StringBuilder str);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_print_right(IntPtr con, int x, int y, /*TCOD_bkgnd_flag*/ int flag, StringBuilder str);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_print_center(IntPtr con, int x, int y, /*TCOD_bkgnd_flag*/ int flag, StringBuilder str);

        //Returns height of the printed string
        [DllImport(DLLName.name)]
        private extern static int TCOD_console_print_left_rect(IntPtr con, int x, int y, int w, int h, /*TCOD_bkgnd_flag*/ int flag, StringBuilder str);

        //Returns height of the printed string
        [DllImport(DLLName.name)]
        private extern static int TCOD_console_print_right_rect(IntPtr con, int x, int y, int w, int h, /*TCOD_bkgnd_flag*/ int flag, StringBuilder str);

        //Returns height of the printed string
        [DllImport(DLLName.name)]
        private extern static int TCOD_console_print_center_rect(IntPtr con, int x, int y, int w, int h, /*TCOD_bkgnd_flag*/ int flag, StringBuilder str);



        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_background_color(IntPtr con, TCODColor back);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_foreground_color(IntPtr con, TCODColor back);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_clear(IntPtr con);

        /* Single Character Manipulation */

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_put_char(IntPtr con, int x, int y, int c, /*TCOD_bkgnd_flag*/ int flag);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_back(IntPtr con, int x, int y, TCODColor col, /*TCOD_bkgnd_flag*/ int flag);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_fore(IntPtr con, int x, int y, TCODColor col);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_char(IntPtr con, int x, int y, int c);


        /* Get things from console */

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_console_get_background_color(IntPtr con);

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_console_get_foreground_color(IntPtr con);

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_console_get_back(IntPtr con, int x, int y);

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_console_get_fore(IntPtr con, int x, int y);

        [DllImport(DLLName.name)]
        private extern static int TCOD_console_get_char(IntPtr con, int x, int y);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_delete(IntPtr console);
        #endregion
    }

    /* One should not make more than one of these, on pain of bugs */
    public class TCODConsoleRoot : TCODConsole
    {
        public TCODConsoleRoot(int w, int h, String title, bool fullscreen) : base(IntPtr.Zero)
        {
            TCOD_console_init_root(w, h, new StringBuilder(title), fullscreen);
        }

        public TCODConsoleRoot(int w, int h, String title, bool fullscreen, CustomFontRequest r)  : base(IntPtr.Zero)
        {
            TCOD_console_set_custom_font(new StringBuilder(r.m_fontFile), r.m_char_width, r.m_char_height, r.m_nb_char_horiz, r.m_nb_char_vertic, r.m_chars_by_row, r.m_key_color);
            TCOD_console_init_root(w, h, new StringBuilder(title), fullscreen);
        }

        public bool IsWindowClosed()
        {
            return TCOD_console_is_window_closed();
        }

        public void Flush()
        {
            TCOD_console_flush();
        }

        public void SetFade(byte fade, TCODColor fadingColor)
        {
            TCOD_console_set_fade(fade, fadingColor);
        }

        public byte GetFadeLevel()
        {
            return TCOD_console_get_fade();
        }

        public TCODColor GetFadeColor()
        {
            return TCOD_console_get_fading_color();
        }

        public void SetFullscreen(bool fullScreen)
        {
            TCOD_console_set_fullscreen(fullScreen);
        }

        public bool IsFullscreen()
        {
            return TCOD_console_is_fullscreen();
        }

        public TCODConsole GetNewConsole(int w, int h)
        {
            return new TCODConsole(TCOD_console_new(w, h));
        }

        #region DLLImports

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_init_root(int w, int h, StringBuilder title, bool fullscreen);

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_custom_font(StringBuilder fontFile, int char_width, int char_height, int nb_char_horiz, int nb_char_vertic, bool chars_by_row, TCODColor key_color);


        [DllImport(DLLName.name)]
        private extern static bool TCOD_console_is_window_closed();

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_flush();


        /* Fading */

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_fade(byte fade, TCODColor fadingColor);

        [DllImport(DLLName.name)]
        private extern static byte TCOD_console_get_fade();

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_console_get_fading_color();


        /* Fullscreen */

        [DllImport(DLLName.name)]
        private extern static bool TCOD_console_is_fullscreen();

        [DllImport(DLLName.name)]
        private extern static void TCOD_console_set_fullscreen(bool fullscreen);

        /* Offscreen console */

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_console_new(int w, int h);

        #endregion
    }

}
