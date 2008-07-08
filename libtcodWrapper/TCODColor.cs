using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TCODColor
    {
        public byte r, g, b;

        public TCODColor(TCODColor c)
        {
            r = c.r;
            g = c.g;
            b = c.b;
        }

        public TCODColor(byte red, byte green, byte blue)
        {
            r = red;
            g = green;
            b = blue;
        }

        public TCODColor(float h, float s, float v)
        {
            r = 0;
            g = 0;
            b = 0;

            TCOD_color_set_HSV(ref this, h, s, v);
        }

        public void GetHSV(out float h, out float s, out float v)
        {
            TCOD_color_get_HSV(this, out h, out s, out v);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return TCOD_color_equals(this, (TCODColor)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TCODColor lhs, TCODColor rhs)
        {
            return TCOD_color_equals(lhs, rhs);
        }

        public static bool operator !=(TCODColor lhs, TCODColor rhs)
        {
            return !TCOD_color_equals(lhs, rhs);
        }

        public static TCODColor operator +(TCODColor lhs, TCODColor rhs)
        {
            return TCOD_color_add(lhs, rhs);
        }

        public static TCODColor operator *(TCODColor lhs, TCODColor rhs)
        {
            return TCOD_color_multiply(lhs, rhs);
        }

        public static TCODColor operator *(TCODColor lhs, float rhs)
        {
            return TCOD_color_multiply_scalar(lhs, rhs);
        }

        public static TCODColor operator *(TCODColor lhs, double rhs)
        {
            return TCOD_color_multiply_scalar(lhs, (float)rhs);
        }

        public static TCODColor Interpolate(TCODColor c1, TCODColor c2, float coef)
        {
        	TCODColor ret =  new TCODColor(); 
			ret.r=(byte)(c1.r+(c2.r-c1.r)*coef);
			ret.g=(byte)(c1.g+(c2.g-c1.g)*coef);
			ret.b=(byte)(c1.b+(c2.b-c1.b)*coef);
			return ret;
        }

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
    }
}
