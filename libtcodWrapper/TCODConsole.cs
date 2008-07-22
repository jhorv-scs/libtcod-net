using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    
    internal class DLLName
    {
        /// <summary>
        /// Defines the name of the DLL we look for on disk. The runtime adds {.dll,.so} to the end
        /// </summary>
        internal const string name = @"libtcod";
    }
    
    /// <summary>
    /// Defines how draw operations affect background of the console.
    /// </summary>
    public class TCODBackground
    {
    	internal int m_value;
    	
        /// <summary>
        /// Create background with a given flag that does not take alpha paramater
        /// </summary>
        /// <param name="flag">Background Type</param>
    	public TCODBackground(TCOD_bkgnd_flag flag)
    	{
    		if(flag == TCOD_bkgnd_flag.TCOD_BKGND_ADDA || flag == TCOD_bkgnd_flag.TCOD_BKGND_ALPH)
    			throw new Exception("Must use TCODBackagroudn constructor which takes value");
    		m_value = (int)flag;
		}
		
        /// <summary>
        /// Create background with a given flag that does take alpha paramater
        /// </summary>
        /// <param name="flag">Background Type</param>
        /// <param name="val">Alpha Value</param>
		public TCODBackground(TCOD_bkgnd_flag flag, float val)
    	{
            NewBackgroundCore(flag, val);
		}

        /// <summary>
        /// Create background with a given flag that does take alpha paramater
        /// </summary>
        /// <param name="flag">Background Type</param>
        /// <param name="val">Alpha Value</param>
        public TCODBackground(TCOD_bkgnd_flag flag, double val)
        {
            NewBackgroundCore(flag, (float)val);
        }

        /// <summary>
        /// Create a copy of a background flag 
        /// </summary>
        /// <param name="b">Background to copy</param>
        public TCODBackground(TCODBackground b)
        {
            m_value = b.m_value;
        }

        private void NewBackgroundCore(TCOD_bkgnd_flag flag, float val)
        {
            if (flag != TCOD_bkgnd_flag.TCOD_BKGND_ADDA && flag != TCOD_bkgnd_flag.TCOD_BKGND_ALPH)
                throw new Exception("Must not use TCODBackagroudn constructor which takes value");
            m_value = (int)flag | (((byte)(val * 255)) << 8);
        }


        /// <summary>
        /// Increment background type to next background in TCOD_bkgnd_flag enum
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <returns>New Background</returns>
        public static TCODBackground operator ++(TCODBackground lhs)
        {
            if (lhs.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ALPH)
                throw new Exception("Can not increment past end of TCOD_bkgnd_flag enum");
            lhs.m_value += 1;
            return lhs;
        }

        /// <summary>
        /// Decrement background type to next background in TCOD_bkgnd_flag enum
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <returns>New Background</returns>
        public static TCODBackground operator --(TCODBackground lhs)
        {
            if (lhs.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_NONE)
                throw new Exception("Can not decrement past end of TCOD_bkgnd_flag enum");
            lhs.m_value -= 1;
            return lhs;
        }

        /// <summary>
        /// Get Current Background Type
        /// </summary>
        /// <returns>Background Enum</returns>
        public TCOD_bkgnd_flag GetBackgroundFlag()
        {
            return (TCOD_bkgnd_flag)(m_value & 0xff);
        }

        /// <summary>
        /// Get Current Alpha value
        /// </summary>
        /// <returns>Alpha Value</returns>
        public byte GetAlphaValue()
        {
            return (byte)(m_value >> 8);
        }
    }

    #pragma warning disable 1591  //Disable warning about lack of xml comments
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
    #pragma warning restore 1591

    /// <summary>
    /// "Special" ascii characters such as arrows and lines
    /// </summary>
    public class TCODSpecialChar
    {
        #pragma warning disable 1591  //Disable warning about lack of xml comments
        public const byte TCOD_CHAR_HLINE = 196;
        public const byte TCOD_CHAR_VLINE = 179;
        public const byte TCOD_CHAR_NE = 191;
        public const byte TCOD_CHAR_NW = 218;
        public const byte TCOD_CHAR_SE = 217;
        public const byte TCOD_CHAR_SW = 192;
        public const byte TCOD_CHAR_DHLINE = 205;
        public const byte TCOD_CHAR_DVLINE = 186;
        public const byte TCOD_CHAR_DNE = 187;
        public const byte TCOD_CHAR_DNW = 201;
        public const byte TCOD_CHAR_DSE = 188;
        public const byte TCOD_CHAR_DSW = 200;
        public const byte TCOD_CHAR_TEEW = 180;
        public const byte TCOD_CHAR_TEEE = 195;
        public const byte TCOD_CHAR_TEEN = 193;
        public const byte TCOD_CHAR_TEES = 194;
        public const byte TCOD_CHAR_DTEEW = 181;
        public const byte TCOD_CHAR_DTEEE = 198;
        public const byte TCOD_CHAR_DTEEN = 208;
        public const byte TCOD_CHAR_DTEES = 210;
        public const byte TCOD_CHAR_CHECKER = 178;
        public const byte TCOD_CHAR_BLOCK = 219;
        public const byte TCOD_CHAR_BLOCK1 = 178;
        public const byte TCOD_CHAR_BLOCK2 = 177;
        public const byte TCOD_CHAR_BLOCK3 = 176;
        public const byte TCOD_CHAR_BLOCK_B = 220;
        public const byte TCOD_CHAR_BLOCK_T = 223;
        public const byte TCOD_CHAR_DS_CROSSH = 216;
        public const byte TCOD_CHAR_DS_CROSSV = 215;
        public const byte TCOD_CHAR_CROSS = 197;
        public const byte TCOD_CHAR_LIGHT = 15;
        public const byte TCOD_CHAR_TREE = 5;
        public const byte TCOD_CHAR_ARROW_N = 24;
        public const byte TCOD_CHAR_ARROW_S = 25;
        public const byte TCOD_CHAR_ARROW_E = 26;
        public const byte TCOD_CHAR_ARROW_W = 27;
        #pragma warning restore 1591  //Disable warning about lack of xml comments
    };

    /// <summary>
    /// Request for console to draw with font other than "terminal.bmp"
    /// </summary>
    public struct CustomFontRequest
    {
        /// <summary>
        /// Create new custom font request
        /// </summary>
        /// <param name="fontFile">File name to load font from</param>
        /// <param name="char_width">Pixels each character is wide</param>
        /// <param name="char_height">Pixels each character is high</param>
        /// <param name="nb_char_horiz">Number of characters per horizontal row</param>
        /// <param name="nb_char_vertic">Number of characters per vertical row</param>
        /// <param name="chars_in_row">Is the first set of ascii characters in a row (not a column)</param>
        /// <param name="key_color">Color in bitmap that represents background</param>
        public CustomFontRequest(String fontFile, int char_width, int char_height, int nb_char_horiz, int nb_char_vertic, bool chars_in_row, TCODColor key_color)
        {
            m_fontFile = fontFile;
            m_char_width = char_width;
            m_char_height = char_height;
            m_nb_char_horiz = nb_char_horiz;
            m_nb_char_vertic = nb_char_vertic;
            m_chars_in_row = chars_in_row;
            m_key_color = key_color;
        }

        internal String m_fontFile;
        internal int m_char_width;
        internal int m_char_height;
        internal int m_nb_char_horiz;
        internal int m_nb_char_vertic;
        internal bool m_chars_in_row;
        internal TCODColor m_key_color;
    }

    /// <summary>
    /// Types of alignment for printing of strings
    /// </summary>
    public enum TCODLineAlign
    {
        #pragma warning disable 1591  //Disable warning about lack of xml comments
        Left,
        Right,
        Center
        #pragma warning restore 1591  //Disable warning about lack of xml comments
    }

    /// <summary>
    /// Represents any console, either on screen or off
    /// </summary>
    public class TCODConsole : IDisposable
    {
        internal TCODConsole(IntPtr w, int width, int height)
        {
            m_consolePtr = w;
            m_width = width;
            m_height = height;
        }
        
        /// <summary>
        /// Destory unmanaged console resources
        /// </summary>
        public void Dispose()
        {
            //Don't try to dispose Root Consoles
            if (m_consolePtr != IntPtr.Zero)
                TCOD_console_delete(m_consolePtr);
        }

        internal IntPtr m_consolePtr;
        internal int m_width;
        internal int m_height;

        /// <summary>
        /// Returns console's width
        /// </summary>
        /// <returns>Width</returns>
        public int GetConsoleWidth()
        {
            return m_width;
        }

        /// <summary>
        /// Returns console's height
        /// </summary>
        /// <returns>Height</returns>
        public int GetConsoleHeight()
        {
            return m_height;
        }

        /// <summary>
        /// Set the default background color of the console
        /// </summary>
        /// <param name="background">Background color</param>
        public void SetBackgroundColor(TCODColor background)
        {
            TCOD_console_set_background_color(m_consolePtr, background);
        }

        /// <summary>
        /// Set the default foreground color of the console
        /// </summary>
        /// <param name="foreground">Foreground color</param>
        public void SetForegroundColor(TCODColor foreground)
        {
            TCOD_console_set_foreground_color(m_consolePtr, foreground);
        }

        /// <summary>
        /// Clear the console by setting each cell to default background, foreground color, and ascii value to ' '
        /// </summary>
        public void Clear()
        {
            TCOD_console_clear(m_consolePtr);
        }

        /// <summary>
        /// Put ascii character onto console
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <param name="c">Ascii character</param>
        /// <param name="flag">Background flag</param>
        public void PutChar(int x, int y, char c, TCODBackground flag)
        {
            TCOD_console_put_char(m_consolePtr, x, y, (int)c, flag.m_value);
        }

        /// <summary>
        /// Put ascii character onto console
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <param name="c">TCODSpecialChar or ascii byte</param>
        /// <param name="flag">Background flag</param>
        public void PutChar(int x, int y, byte c, TCODBackground flag)
        {
            TCOD_console_put_char(m_consolePtr, x, y, c, flag.m_value);
        }

        /// <summary>
        /// Put ascii character onto console
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <param name="c">Ascii character</param>
        public void PutChar(int x, int y, char c)
        {
            TCOD_console_put_char(m_consolePtr, x, y, (int)c, (int)TCOD_bkgnd_flag.TCOD_BKGND_SET);
        }

        /// <summary>
        /// Put ascii character onto console
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <param name="c">Ascii character</param>
        public void PutChar(int x, int y, byte c)
        {
            TCOD_console_put_char(m_consolePtr, x, y, c, (int)TCOD_bkgnd_flag.TCOD_BKGND_SET);
        }
        
        /// <summary>
        /// Set background color of single cell
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <param name="col">Background color</param>
        /// <param name="flag">Background flag</param>
        public void SetCharBackground(int x, int y, TCODColor col, TCODBackground flag)
        {
            TCOD_console_set_back(m_consolePtr, x, y, col, flag.m_value);
        }

        /// <summary>
        /// Set background color of single cell
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <param name="col">Background color</param>
        public void SetCharBackground(int x, int y, TCODColor col)
        {
            SetCharBackground(x, y, col, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
        }

        /// <summary>
        /// Set foreground color of single cell
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <param name="col">Foreground color</param>
        public void SetCharForeground(int x, int y, TCODColor col)
        {
            TCOD_console_set_fore(m_consolePtr, x, y, col);
        }

        /// <summary>
        /// Get default background color
        /// </summary>
        /// <returns>Background color</returns>
        public TCODColor GetBackgroudColor()
        {
            return TCOD_console_get_background_color(m_consolePtr);
        }

        /// <summary>
        /// Get default foreground color
        /// </summary>
        /// <returns>Foreground color</returns>
        public TCODColor GetForegroundColor()
        {
            return TCOD_console_get_foreground_color(m_consolePtr);
        }

        /// <summary>
        /// Get Background of single cell
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <returns>Background color</returns>
        public TCODColor GetCharBackground(int x, int y)
        {
            return TCOD_console_get_back(m_consolePtr, x, y);
        }

        /// <summary>
        /// Get Forground of single cell
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <returns>Forground color</returns>
        public TCODColor GetCharForeground(int x, int y)
        {
            return TCOD_console_get_fore(m_consolePtr, x, y);
        }

        /// <summary>
        /// Get ascii value of single cell
        /// </summary>
        /// <param name="x">x (Width) position</param>
        /// <param name="y">y (Height) position</param>
        /// <returns>Ascii value</returns>
        public char GetChar(int x, int y)
        {
            return (char)TCOD_console_get_char(m_consolePtr, x, y);
        }

        /// <summary>
        /// Print string to line of console, using default foreground/background colors
        /// </summary>
        /// <param name="str">String to print</param>
        /// <param name="x">x (Width) position of first character</param>
        /// <param name="y">y (Height) position of first character</param>
        /// <param name="align">Alignment of string</param>
        public void PrintLine(string str, int x, int y, TCODLineAlign align)
        {
            PrintLine(str, x, y, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET), align);
        }

        /// <summary>
        /// Print string to line of console, using default foreground/background colors
        /// </summary>
        /// <param name="str">String to print</param>
        /// <param name="x">x (Width) position of first character</param>
        /// <param name="y">y (Height) position of first character</param>
        /// <param name="flag">Background flag</param>
        /// <param name="align">Alignment of string</param>
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

        /// <summary>
        /// Print aligned string inside the defined rectangle, truncating if bottom is reached
        /// </summary>
        /// <param name="str">String to print</param>
        /// <param name="x">x (Width) position of first character</param>
        /// <param name="y">y (Height) position of first character</param>
        /// <param name="w">Width of rectangle to print in</param>
        /// <param name="h">Height of rectangle to print in. If 0, string is only truncated if reaches bottom of console.</param>
        /// <param name="align">Alignment of string</param>
        /// <returns>Number of lines printed</returns>
        public int PrintLineRect(string str, int x, int y, int w, int h, TCODLineAlign align)
        {
            return PrintLineRect(str, x, y, w, h, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET), align);
        }

        /// <summary>
        /// Print aligned string inside the defined rectangle, truncating if bottom is reached
        /// </summary>
        /// <param name="str">String to print</param>
        /// <param name="x">x (Width) position of first character</param>
        /// <param name="y">y (Height) position of first character</param>
        /// <param name="w">Width of rectangle to print in</param>
        /// <param name="h">Height of rectangle to print in. If 0, string is only truncated if reaches bottom of console.</param>
        /// <param name="flag">Background flag</param>
        /// <param name="align">Alignment of string</param>
        /// <returns>Number of lines printed</returns>
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

        /// <summary>
        /// Blit console onto another console
        /// </summary>
        /// <param name="xSrc">Upper left corner x coord of area to blit from</param>
        /// <param name="ySrc">Upper left corner y coord of area to blit from</param>
        /// <param name="wSrc">Width of source area</param>
        /// <param name="hSrc">Height of source area</param>
        /// <param name="dest">Destination console</param>
        /// <param name="xDst">Upper left corner x coord of area to blit to</param>
        /// <param name="yDst">Upper left corner y coord of area to blit to</param>
        public void Blit(int xSrc, int ySrc, int wSrc, int hSrc, TCODConsole dest, int xDst, int yDst)
        {
            Blit(xSrc, ySrc, wSrc, hSrc, dest, xDst, yDst, 255);
        }

        /// <summary>
        /// Blit console onto another console
        /// </summary>
        /// <param name="xSrc">Upper left corner x coord of area to blit from</param>
        /// <param name="ySrc">Upper left corner y coord of area to blit from</param>
        /// <param name="wSrc">Width of source area</param>
        /// <param name="hSrc">Height of source area</param>
        /// <param name="dest">Destination console</param>
        /// <param name="xDst">Upper left corner x coord of area to blit to</param>
        /// <param name="yDst">Upper left corner y coord of area to blit to</param>
        /// <param name="fade">Transparency of blitted console. 255 = fully replace destination. (0-254) simulate real transparency with varying degrees of fading.</param>
        public void Blit(int xSrc, int ySrc, int wSrc, int hSrc, TCODConsole dest, int xDst, int yDst, int fade)
        {
            TCOD_console_blit(m_consolePtr, xSrc, ySrc, wSrc, hSrc, dest.m_consolePtr, xDst, yDst, fade);
        }

        /// <summary>
        /// Draw rectangle of color to console, setting background color to default
        /// </summary>
        /// <param name="x">Upper left corner x coord</param>
        /// <param name="y">Upper left corner y coord</param>
        /// <param name="w">Width of rectangle</param>
        /// <param name="h">Height of rectangle</param>
        /// <param name="clear">Clear cells of any ascii character</param>
        /// <param name="flag">Background flag</param>
        public void DrawRect(int x, int y, int w, int h, bool clear, TCODBackground flag)
        {
            TCOD_console_rect(m_consolePtr, x, y, w, h, clear, flag.m_value);
        }

        /// <summary>
        /// Draw rectangle of color to console, setting background color to default
        /// </summary>
        /// <param name="x">Upper left corner x coord</param>
        /// <param name="y">Upper left corner y coord</param>
        /// <param name="w">Width of rectangle</param>
        /// <param name="h">Height of rectangle</param>
        /// <param name="clear">Clear cells of any ascii character</param>
        public void DrawRect(int x, int y, int w, int h, bool clear)
        {
            DrawRect(x, y, w, h, clear, new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_SET));
        }

        /// <summary>
        /// Draw horizontal line using default background/foreground color
        /// </summary>
        /// <param name="x">Left endpoint x coord</param>
        /// <param name="y">Left endpoint y coord</param>
        /// <param name="l">Length</param>
        public void DrawHLine(int x, int y, int l)
        {
            TCOD_console_hline(m_consolePtr, x, y, l);
        }

        /// <summary>
        /// Draw vertical line using default background/foreground color
        /// </summary>
        /// <param name="x">Upper endpoint x coord</param>
        /// <param name="y">Upper endpoint y coord</param>
        /// <param name="l">Length</param>
        public void DrawVLine(int x, int y, int l)
        {
            TCOD_console_vline(m_consolePtr, x, y, l);
        }
        
        /// <summary>
        /// Draw "Frame" with title onto console
        /// </summary>
        /// <param name="x">Upper left corner x coord</param>
        /// <param name="y">Upper left corner y coord</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="clear">Clear area</param>
        /// <param name="str">Title of frame</param>
        public void DrawFrame(int x, int y, int w, int h, bool clear, String str)
        {
            TCOD_console_print_frame(m_consolePtr, x, y, w, h, clear, new StringBuilder(str));
        }

        /// <summary>
        /// Draw "Frame" with title onto console
        /// </summary>
        /// <param name="x">Upper left corner x coord</param>
        /// <param name="y">Upper left corner y coord</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="clear">Clear area</param>
        public void DrawFrame(int x, int y, int w, int h, bool clear)
        {
            TCOD_console_print_frame(m_consolePtr, x, y, w, h, clear, IntPtr.Zero);
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
        private extern static void TCOD_console_print_frame(IntPtr con, int x, int y, int w, int h, bool clear, IntPtr nullStr);

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

    /// <summary>
    /// "Root" console, one which blits onto window or fullscreen
    /// </summary>
    /// <remarks>One should not make more than one of these</remarks>
    public class TCODConsoleRoot : TCODConsole
    {
        /// <summary>
        /// Create the root console with the default font
        /// </summary>
        /// <param name="w">Width in characters</param>
        /// <param name="h">Height in characters</param>
        /// <param name="title">Title of window</param>
        /// <param name="fullscreen">Fullscreen?</param>
        public TCODConsoleRoot(int w, int h, String title, bool fullscreen) : base(IntPtr.Zero, w, h)
        {
            TCOD_console_init_root(w, h, new StringBuilder(title), fullscreen);
        }

        /// <summary>
        /// Create the root console with custom font
        /// </summary>
        /// <param name="w">Width in characters</param>
        /// <param name="h">Height in characters</param>
        /// <param name="title">Title of window</param>
        /// <param name="fullscreen">Fullscreen?</param>
        /// <param name="font">Custom font request</param>
        public TCODConsoleRoot(int w, int h, String title, bool fullscreen, CustomFontRequest font)  : base(IntPtr.Zero, w, h)
        {
            TCOD_console_set_custom_font(new StringBuilder(font.m_fontFile), font.m_char_width, 
                font.m_char_height, font.m_nb_char_horiz, font.m_nb_char_vertic, font.m_chars_in_row, 
                font.m_key_color);
            TCOD_console_init_root(w, h, new StringBuilder(title), fullscreen);
        }

        /// <summary>
        /// Has the window been closed by the user
        /// </summary>
        /// <returns>Is Window Closed?</returns>
        public bool IsWindowClosed()
        {
            return TCOD_console_is_window_closed();
        }

        /// <summary>
        /// "Flush" console by rendering new frame
        /// </summary>
        public void Flush()
        {
            TCOD_console_flush();
        }

        /// <summary>
        /// Fade console to specified color
        /// </summary>
        /// <param name="fade">Fading amount (0 {fully faded} - 255 {no fade} )</param>
        /// <param name="fadingColor">Color to fade to</param>
        public void SetFade(byte fade, TCODColor fadingColor)
        {
            TCOD_console_set_fade(fade, fadingColor);
        }

        /// <summary>
        /// Get current fade level
        /// </summary>
        /// <returns>Fading amount (0 {fully faded} - 255 {no fade} )</returns>
        public byte GetFadeLevel()
        {
            return TCOD_console_get_fade();
        }

        /// <summary>
        /// Get current fade color
        /// </summary>
        /// <returns>Fade Color</returns>
        public TCODColor GetFadeColor()
        {
            return TCOD_console_get_fading_color();
        }

        /// <summary>
        /// Set console full screen status
        /// </summary>
        /// <param name="fullScreen">Fullscreen?</param>
        public void SetFullscreen(bool fullScreen)
        {
            TCOD_console_set_fullscreen(fullScreen);
        }

        /// <summary>
        /// Is console currently fullscreen
        /// </summary>
        /// <returns>Fullscreen?</returns>
        public bool IsFullscreen()
        {
            return TCOD_console_is_fullscreen();
        }

        /// <summary>
        /// Create new offscreen (secondary) console 
        /// </summary>
        /// <param name="w">Width in characters</param>
        /// <param name="h">Height in characters</param>
        /// <returns>New console</returns>
        public TCODConsole GetNewConsole(int w, int h)
        {
            return new TCODConsole(TCOD_console_new(w, h), w, h);
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
