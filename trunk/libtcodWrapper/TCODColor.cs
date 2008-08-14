using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    /// <summary>
    /// Represents a 32-bit color to the TCOD API.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        private byte r;
        /// <summary>
        /// Red
        /// </summary>
        public byte Red
        {
            get { return r; }
            set { r = value; }
        }

        private byte g;
        /// <summary>
        /// Green
        /// </summary>
        public byte Green
        {
            get { return g; }
            set { g = value; }
        }

        private byte b;
        /// <summary>
        /// Blue
        /// </summary>
        public byte Blue
        {
            get { return b; }
            set { b = value; }
        }

        /// <summary>
        /// Copy Constructor for Color
        /// </summary>
        /// <param name="c">Color to make copy of.</param>
        public Color(Color c)
        {
            r = c.r;
            g = c.g;
            b = c.b;
        }

        /// <summary>
        /// Form a Color from RGB components.
        /// </summary>
        /// <param name="red">Red Component (0 - 255)</param>
        /// <param name="green">Green Component (0 - 255)</param>
        /// <param name="blue">Blue Component (0 - 255)</param>
        public Color(byte red, byte green, byte blue)
        {
            r = red;
            g = green;
            b = blue;
        }
        
        /// <summary>
        /// Form a Color from HSV components.
        /// </summary>
        /// <param name="h">Hue Component (0.0 - 360.0)</param>
        /// <param name="s">Saturation Component (0.0 - 1.0)</param>
        /// <param name="v">Value Component (0.0 - 1.0)</param>
        public void SetHSV(float h, float s, float v)
        {
            r = 0;
            g = 0;
            b = 0;

            TCOD_color_set_HSV(ref this, h, s, v);
        }

        /// <summary>
        /// Returns HSV value from a Color.
        /// </summary>
        /// <param name="h">Hue Component (0.0 - 360.0)</param>
        /// <param name="s">Saturation Component (0.0 - 1.0)</param>
        /// <param name="v">Value Component (0.0 - 1.0)</param>
        public void GetHSV(out float h, out float s, out float v)
        {
            TCOD_color_get_HSV(this, out h, out s, out v);
        }

        /// <summary>
        /// Determine if two Colors are equal.
        /// </summary>
        /// <param name="obj">Other Color</param>
        /// <returns>Are Equal?</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return TCOD_color_equals(this, (Color)obj);
        }

        /// <summary>
        /// Calculate Hash Value of a Color
        /// </summary>
        /// <returns>Hash Value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determine if two Colors are equal.
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>Are Equal?</returns>
        public static bool operator ==(Color lhs, Color rhs)
        {
            return TCOD_color_equals(lhs, rhs);
        }

        /// <summary>
        /// Determine if two Colors are not equal.
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>Are Not Equal?</returns>
        public static bool operator !=(Color lhs, Color rhs)
        {
            return !TCOD_color_equals(lhs, rhs);
        }

        /// <summary>
        /// Add two Colors
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static Color operator +(Color lhs, Color rhs)
        {
            return TCOD_color_add(lhs, rhs);
        }

        /// <summary>
        /// Multiply two Colors
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static Color operator *(Color lhs, Color rhs)
        {
            return TCOD_color_multiply(lhs, rhs);
        }

        /// <summary>
        /// Multiple a Color by a constant
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static Color operator *(Color lhs, float rhs)
        {
            return TCOD_color_multiply_scalar(lhs, rhs);
        }

        /// <summary>
        /// Multiple a Color by a constant
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static Color operator *(Color lhs, double rhs)
        {
            return TCOD_color_multiply_scalar(lhs, (float)rhs);
        }

        /// <summary>
        /// Divide each component of a color by a give constant
        /// </summary>
        /// <param name="lhs">Left Hand Side Color</param>
        /// <param name="rhs">Right Hand Side Constant</param>
        /// <returns>New Color</returns>
        public static Color operator /(Color lhs, int rhs)
        {
            return new Color((byte)(lhs.r / rhs), (byte)(lhs.g / rhs), (byte)(lhs.b / rhs));
        }

        /// <summary>
        /// Subtract each component of a color from another, flooring to zero.
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>New Color</returns>
        public static Color operator -(Color lhs, Color rhs)
        {
            return new Color(SubFloor(lhs.r, rhs.r), SubFloor(lhs.g, rhs.g), SubFloor(lhs.b, rhs.b));
        }

        private static byte SubFloor(byte lhs, byte rhs)
        {
            if (lhs < rhs)
                return 0;
            else
                return (byte)(lhs - rhs);
        }

        /// <summary>
        /// Interpolate (lerp) a Color with another Color
        /// </summary>
        /// <param name="c1">First Color</param>
        /// <param name="c2">Second Color</param>
        /// <param name="coef">Interpolate Coefficient</param>
        /// <returns>New Color</returns>
        public static Color Interpolate(Color c1, Color c2, float coef)
        {
            Color ret =  new Color(); 
            ret.r=(byte)(c1.r+(c2.r-c1.r)*coef);
            ret.g=(byte)(c1.g+(c2.g-c1.g)*coef);
            ret.b=(byte)(c1.b+(c2.b-c1.b)*coef);
            return ret;
        }

        /// <summary>
        /// Interpolate (lerp) a Color with another Color
        /// </summary>
        /// <param name="c1">First Color</param>
        /// <param name="c2">Second Color</param>
        /// <param name="coef">Interpolate Coefficient</param>
        /// <returns>New Color</returns>
        public static Color Interpolate(Color c1, Color c2, double coef)
        {
            Color ret =  new Color(); 
            ret.r=(byte)(c1.r+(c2.r-c1.r)*coef);
            ret.g=(byte)(c1.g+(c2.g-c1.g)*coef);
            ret.b=(byte)(c1.b+(c2.b-c1.b)*coef);
            return ret;
        }

        [DllImport(DLLName.name)]
        private extern static Color TCOD_color_add(Color c1, Color c2);

        [DllImport(DLLName.name)]
        private extern static Color TCOD_color_multiply(Color c1, Color c2);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_color_equals(Color c1, Color c2);

        [DllImport(DLLName.name)]
        private extern static Color TCOD_color_multiply_scalar (Color c1, float value);

        // 0<= h < 360, 0 <= s <= 1, 0 <= v <= 1 
        [DllImport(DLLName.name)]
        private extern static void TCOD_color_set_HSV(ref Color c, float h, float s, float v);

        [DllImport(DLLName.name)]
        private extern static void TCOD_color_get_HSV(Color c, out float h, out float s, out float v);

        #pragma warning disable 1591  //Disable warning about lack of xml comments
        public static Color Black = new Color(0, 0, 0);
        public static Color DarkGray = new Color(96, 96, 96);
        public static Color Gray = new Color(196, 196, 196);
        public static Color White = new Color(255, 255, 255);
        public static Color DarkBlue = new Color(40, 40, 128);
        public static Color BrightBlue = new Color(120, 120, 255);
        public static Color DarkRed = new Color(128, 0, 0);
        public static Color NormalRed = new Color(255, 0, 0);
        public static Color BrightRed = new Color(255, 100, 50);
        public static Color Brown = new Color(32, 16, 0);
        public static Color BrightYellow = new Color(255, 255, 150);
        public static Color Yellow = new Color(255, 255, 0);
        public static Color DarkYellow = new Color(164, 164, 0);
        public static Color BrightGreen = new Color(0, 255, 0);
        public static Color NormalGreen = new Color(0, 220, 0);
        public static Color DarkGreen = new Color(0, 128, 0);
        public static Color Orange = new Color(255, 150, 0);
        public static Color Silver = new Color(203, 203, 203);
        public static Color Gold = new Color(255, 255, 102);
        public static Color Purple = new Color(204, 51, 153);
        public static Color DarkPurple = new Color(51, 0, 51);
        #pragma warning restore 1591
    }
}
