using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    public class TCODNoise : IDisposable
    {
        public TCODNoise(int dimensions)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, NoiseDefaultHurst, NoiseDefaultLacunarity, IntPtr.Zero);
        }

        public TCODNoise(int dimensions, TCODRandom random)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, NoiseDefaultHurst, NoiseDefaultLacunarity, random.m_instance);
        }

        public TCODNoise(int dimensions, double hurst, double lacunarity)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, (float)hurst, (float)lacunarity, IntPtr.Zero);
        }

        public TCODNoise(int dimensions, double hurst, double lacunarity, TCODRandom random)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, (float)hurst, (float)lacunarity, random.m_instance);
        }

        public float GetPerlinNoise(ref float[] f)
        {
            CheckDimension(f.Length);
            return TCOD_noise_get(m_instance, ref f);
        }

        public float GetBrownianMotion(ref float[] f, float octaves)
        {
            CheckDimension(f.Length);
            return TCOD_noise_fbm(m_instance, ref f, octaves);
        }

        public float GetTurbulence(ref float[] f, float octaves)
        {
            CheckDimension(f.Length);
            return TCOD_noise_fbm(m_instance, ref f, octaves);
        }
      
        public void Dispose()
        {
            TCOD_noise_delete(m_instance);
        }

        private const float NoiseDefaultHurst = 0.5f;
        private const float NoiseDefaultLacunarity = 2.0f;

        private IntPtr m_instance;
        int m_dimensions;

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_noise_new(int dimensions, float hurst, float lacunarity, IntPtr random);
        
        // basic perlin noise
        [DllImport(DLLName.name)]
        private extern static float TCOD_noise_get(IntPtr noise, ref float [] f);

        // fractional brownian motion
        [DllImport(DLLName.name)]
        private extern static float TCOD_noise_fbm(IntPtr noise, ref float [] f, float octaves);

        // turbulence
        [DllImport(DLLName.name)]
        private extern static float TCOD_noise_turbulence(IntPtr noise, ref float [] f, float octaves);

        [DllImport(DLLName.name)]
        private extern static void TCOD_noise_delete(IntPtr noise);

        private void CheckDimension(int dims)
        {
            if (m_dimensions != dims)
                throw new Exception("TCODNoise: Dimension mismatch");
        }
    };
}
