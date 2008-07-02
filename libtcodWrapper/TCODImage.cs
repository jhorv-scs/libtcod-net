using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    public class TCODImage : IDisposable
    {
        private IntPtr m_instance;

        public TCODImage(int width, int height)
        {
            m_instance = TCOD_image_new(width, height);
        }

        public TCODImage(string filename)
        {
            m_instance = TCOD_image_load(new StringBuilder(filename));
        }

        public TCODImage(TCODConsole console)
        {
            m_instance = TCOD_image_from_console(console.m_consolePtr);
        }

        public void Dispose()
        {
            TCOD_image_delete(m_instance);
        }

        public void Clear(TCODColor color)
        {
            TCOD_image_clear(m_instance, color); 
        }

        public void SaveImageToDisc(string filename)
        {
            TCOD_image_save(m_instance, new StringBuilder(filename));
        }

        public void GetSize(out int w, out int h)
        {
            TCOD_image_get_size(m_instance, out w, out h);
        }

        public TCODColor GetPixel(int x, int y)
        {
            return TCOD_image_get_pixel(m_instance, x, y);
        }

        public bool GetPixelTransparency(int x, int y)
        {
            return TCOD_image_is_pixel_transparent(m_instance, x, y);
        }

        public TCODColor GetMipMaps(float x0, float y0, float x1, float y1)
        {
            return TCOD_image_get_mipmap_pixel(m_instance, x0, y0, x1, y1);
        }

        public void PutPixel(int x, int y, TCODColor col)
        {
            TCOD_image_put_pixel(m_instance, x, y, col);
        }

        public void SetKeyColor(TCODColor keyColor)
        {
            TCOD_image_set_key_color(m_instance, keyColor);
        }

        public void Blit(TCODConsole console, float x, float y, TCOD_bkgnd_flag bkgnd_flag, double scalex, double scaley, double angle)
        {
            TCOD_image_blit(m_instance, console.m_consolePtr, x, y, bkgnd_flag, (float)scalex, (float)scaley, (float)angle);
        }

        public void BlitRect(TCODConsole console, int x, int y, int w, int h, TCOD_bkgnd_flag bkgnd_flag)
        {
            TCOD_image_blit_rect(m_instance, console.m_consolePtr, x, y, w, h, bkgnd_flag);
        }
        
        [DllImport(DLLName.name)]
        private extern static void TCOD_image_blit_rect(IntPtr image, IntPtr console, int x, int y, int w, int h, TCOD_bkgnd_flag bkgnd_flag);


        [DllImport(DLLName.name)]
        private extern static void TCOD_image_blit(IntPtr image, IntPtr console, float x, float y, TCOD_bkgnd_flag bkgnd_flag, float scalex, float scaley, float angle);

        [DllImport(DLLName.name)]
        private extern static void TCOD_image_set_key_color(IntPtr image, TCODColor key_color); 
        
        [DllImport(DLLName.name)]
        private extern static bool TCOD_image_is_pixel_transparent(IntPtr image, int x, int y);

        [DllImport(DLLName.name)]
        private extern static void TCOD_image_put_pixel(IntPtr image, int x, int y, TCODColor col);

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_image_get_mipmap_pixel(IntPtr image, float x0, float y0, float x1, float y1);

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_image_get_pixel(IntPtr image, int x, int y);

        [DllImport(DLLName.name)]
        private extern static void TCOD_image_get_size(IntPtr image, out int w, out int h);
        
        [DllImport(DLLName.name)]
        private extern static void TCOD_image_save(IntPtr image, StringBuilder filename);

        [DllImport(DLLName.name)]
        private extern static void TCOD_image_clear(IntPtr image, TCODColor color);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_image_from_console(IntPtr console);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_image_load(StringBuilder filename);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_image_new(int width, int height);

        [DllImport(DLLName.name)]
        private extern static void TCOD_image_delete(IntPtr image);
    }
}
