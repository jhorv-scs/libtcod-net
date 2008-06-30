using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    public class TCODRandom : IDisposable
    {

        public TCODRandom()
        {
            m_instance = TCOD_random_new();
        }

        public TCODRandom(uint seed)
        {
            m_instance = TCOD_random_new_from_seed(seed);
        }

        public void Dispose()
        {
            TCOD_random_delete(m_instance);
        }

        public int GetRandomInt(int min, int max)
        {
            return TCOD_random_get_int(m_instance, min, max);
        }

        public float GetRandomFloat(double min, double max)
        {
            return TCOD_random_get_float(m_instance, (float)min, (float)max);
        }

        public float GetRandomFloat(float min, float max)
        {
            return TCOD_random_get_float(m_instance, min, max);
        }

        public int GetIntFromByteArray(int min, int max, string data)
        {
            return TCOD_random_get_int_from_byte_array(min, max, new StringBuilder(data), data.Length);
        }

        private IntPtr m_instance;

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_random_get_instance();

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_random_new();

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_random_new_from_seed(uint seed);

        [DllImport(DLLName.name)]
        private extern static int TCOD_random_get_int(IntPtr mersenne, int min, int max);
        
        [DllImport(DLLName.name)]
        private extern static float TCOD_random_get_float(IntPtr mersenne, float min, float max);

        [DllImport(DLLName.name)]
        private extern static int TCOD_random_get_int_from_byte_array(int min, int max, StringBuilder data, int len);

        [DllImport(DLLName.name)]
        private extern static void TCOD_random_delete(IntPtr mersenne);

    }
}

