using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    /// <summary>
    /// Represents a 32-bit color to the TCOD API.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TCODColor
    {
        /// <summary>
        /// Red
        /// </summary>
        public byte r;

        /// <summary>
        /// Green
        /// </summary>
        public byte g;

        /// <summary>
        /// Blue
        /// </summary>
        public byte b;

        /// <summary>
        /// Copy Constructor for TCODColor
        /// </summary>
        /// <param name="c">Color to make copy of.</param>
        public TCODColor(TCODColor c)
        {
            r = c.r;
            g = c.g;
            b = c.b;
        }

        /// <summary>
        /// Form a TCODColor from RGB components.
        /// </summary>
        /// <param name="red">Red Component (0 - 255)</param>
        /// <param name="green">Green Component (0 - 255)</param>
        /// <param name="blue">Blue Component (0 - 255)</param>
        public TCODColor(byte red, byte green, byte blue)
        {
            r = red;
            g = green;
            b = blue;
        }
        
        /// <summary>
        /// Form a TCODColor from HSV components.
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
        /// Returns HSV value from a TCODColor.
        /// </summary>
        /// <param name="h">Hue Component (0.0 - 360.0)</param>
        /// <param name="s">Saturation Component (0.0 - 1.0)</param>
        /// <param name="v">Value Component (0.0 - 1.0)</param>
        public void GetHSV(out float h, out float s, out float v)
        {
            TCOD_color_get_HSV(this, out h, out s, out v);
        }

        /// <summary>
        /// Determine if two TCODColors are equal.
        /// </summary>
        /// <param name="obj">Other TCODColor</param>
        /// <returns>Are Equal?</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return TCOD_color_equals(this, (TCODColor)obj);
        }

        /// <summary>
        /// Calculate Hash Value of a TCODColor
        /// </summary>
        /// <returns>Hash Value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determine if two TCODColors are equal.
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>Are Equal?</returns>
        public static bool operator ==(TCODColor lhs, TCODColor rhs)
        {
            return TCOD_color_equals(lhs, rhs);
        }

        /// <summary>
        /// Determine if two TCODColors are not equal.
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>Are Not Equal?</returns>
        public static bool operator !=(TCODColor lhs, TCODColor rhs)
        {
            return !TCOD_color_equals(lhs, rhs);
        }

        /// <summary>
        /// Add two TCODColors
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static TCODColor operator +(TCODColor lhs, TCODColor rhs)
        {
            return TCOD_color_add(lhs, rhs);
        }

        /// <summary>
        /// Multiply two TCODColors
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static TCODColor operator *(TCODColor lhs, TCODColor rhs)
        {
            return TCOD_color_multiply(lhs, rhs);
        }

        /// <summary>
        /// Multiple a TCODColor by a constant
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static TCODColor operator *(TCODColor lhs, float rhs)
        {
            return TCOD_color_multiply_scalar(lhs, rhs);
        }

        /// <summary>
        /// Multiple a TCODColor by a constant
        /// </summary>
        /// <param name="lhs">Left Hand Size</param>
        /// <param name="rhs">Right Hand Size</param>
        /// <returns>New Color</returns>
        public static TCODColor operator *(TCODColor lhs, double rhs)
        {
            return TCOD_color_multiply_scalar(lhs, (float)rhs);
        }

        /// <summary>
        /// Divide each component of a color by a give constant
        /// </summary>
        /// <param name="lhs">Left Hand Side Color</param>
        /// <param name="rhs">Right Hand Side Constant</param>
        /// <returns>New Color</returns>
        public static TCODColor operator /(TCODColor lhs, int rhs)
        {
            return new TCODColor((byte)(lhs.r / rhs), (byte)(lhs.g / rhs), (byte)(lhs.b / rhs));
        }

        /// <summary>
        /// Subtract each component of a color from another, flooring to zero.
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>New Color</returns>
        public static TCODColor operator -(TCODColor lhs, TCODColor rhs)
        {
            return new TCODColor(SubFloor(lhs.r, rhs.r), SubFloor(lhs.g, rhs.g), SubFloor(lhs.b, rhs.b));
        }

        private static byte SubFloor(byte lhs, byte rhs)
        {
            if (lhs < rhs)
                return 0;
            else
                return (byte)(lhs - rhs);
        }

        /// <summary>
        /// Interpolate (lerp) a TCODColor with another TCODColor
        /// </summary>
        /// <param name="c1">First Color</param>
        /// <param name="c2">Second Color</param>
        /// <param name="coef">Interpolate Coefficient</param>
        /// <returns>New Color</returns>
        public static TCODColor Interpolate(TCODColor c1, TCODColor c2, float coef)
        {
        	TCODColor ret =  new TCODColor(); 
			ret.r=(byte)(c1.r+(c2.r-c1.r)*coef);
			ret.g=(byte)(c1.g+(c2.g-c1.g)*coef);
			ret.b=(byte)(c1.b+(c2.b-c1.b)*coef);
			return ret;
        }

        /// <summary>
        /// Interpolate (lerp) a TCODColor with another TCODColor
        /// </summary>
        /// <param name="c1">First Color</param>
        /// <param name="c2">Second Color</param>
        /// <param name="coef">Interpolate Coefficient</param>
        /// <returns>New Color</returns>
        public static TCODColor Interpolate(TCODColor c1, TCODColor c2, double coef)
        {
        	TCODColor ret =  new TCODColor(); 
			ret.r=(byte)(c1.r+(c2.r-c1.r)*coef);
			ret.g=(byte)(c1.g+(c2.g-c1.g)*coef);
			ret.b=(byte)(c1.b+(c2.b-c1.b)*coef);
			return ret;
        }

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_color_add(TCODColor c1, TCODColor c2);

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_color_multiply(TCODColor c1, TCODColor c2);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_color_equals(TCODColor c1, TCODColor c2);

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_color_multiply_scalar (TCODColor c1, float value);

        // 0<= h < 360, 0 <= s <= 1, 0 <= v <= 1 
        [DllImport(DLLName.name)]
        private extern static void TCOD_color_set_HSV(ref TCODColor c, float h, float s, float v);

        [DllImport(DLLName.name)]
        private extern static void TCOD_color_get_HSV(TCODColor c, out float h, out float s, out float v);

        #pragma warning disable 1591  //Disable warning about lack of xml comments
        public static TCODColor TCOD_black = new TCODColor(0, 0, 0);
        public static TCODColor TCOD_dark_grey = new TCODColor(96, 96, 96);
        public static TCODColor TCOD_grey = new TCODColor(196, 196, 196);
        public static TCODColor TCOD_white = new TCODColor(255, 255, 255);
        public static TCODColor TCOD_dark_blue = new TCODColor(40, 40, 128);
        public static TCODColor TCOD_light_blue = new TCODColor(120, 120, 255);
        public static TCODColor TCOD_dark_red = new TCODColor(128, 0, 0);
        public static TCODColor TCOD_light_red = new TCODColor(255, 100, 50);
        public static TCODColor TCOD_dark_brown = new TCODColor(32, 16, 0);
        public static TCODColor TCOD_light_yellow = new TCODColor(255, 255, 150);
        public static TCODColor TCOD_yellow = new TCODColor(255, 255, 0);
        public static TCODColor TCOD_dark_yellow = new TCODColor(164, 164, 0);
        public static TCODColor TCOD_green = new TCODColor(0, 220, 0);
        public static TCODColor TCOD_orange = new TCODColor(255, 150, 0);
        public static TCODColor TCOD_red = new TCODColor(255, 0, 0);
        public static TCODColor TCOD_silver = new TCODColor(203, 203, 203);
        public static TCODColor TCOD_gold = new TCODColor(255, 255, 102);
        public static TCODColor TCOD_purple = new TCODColor(204, 51, 153);
        public static TCODColor TCOD_dark_purple = new TCODColor(51, 0, 51);
        #pragma warning restore 1591
    }
}
