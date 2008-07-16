using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    /// <summary>
    /// Generates various types of "noise"
    /// </summary>
    public class TCODNoise : IDisposable
    {
        /// <summary>
        /// Create noise object.
        /// </summary>
        /// <param name="dimensions">Number of dimensions</param>
        public TCODNoise(int dimensions)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, NoiseDefaultHurst, NoiseDefaultLacunarity, IntPtr.Zero);
        }

        /// <summary>
        /// Create noise object.
        /// </summary>
        /// <param name="dimensions">Number of dimensions</param>
        /// <param name="random">Random Generator</param>
        public TCODNoise(int dimensions, TCODRandom random)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, NoiseDefaultHurst, NoiseDefaultLacunarity, random.m_instance);
        }

        /// <summary>
        /// Create noise object.
        /// </summary>
        /// <param name="dimensions">Number of dimensions</param>
        /// <param name="hurst">Hurst</param>
        /// <param name="lacunarity">Lacunarity</param>
        public TCODNoise(int dimensions, double hurst, double lacunarity)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, (float)hurst, (float)lacunarity, IntPtr.Zero);
        }

        /// <summary>
        /// Create noise object.
        /// </summary>
        /// <param name="dimensions">Number of dimensions</param>
        /// <param name="hurst">Hurst</param>
        /// <param name="lacunarity">Lacunarity</param>
        /// <param name="random">Random Generator</param>
        public TCODNoise(int dimensions, double hurst, double lacunarity, TCODRandom random)
        {
            m_dimensions = dimensions;
            m_instance = TCOD_noise_new(dimensions, (float)hurst, (float)lacunarity, random.m_instance);
        }

        /// <summary>
        /// Get Perlin Noise
        /// </summary>
        /// <param name="f">An array of coordinates</param>
        /// <returns>Perlin noise for that point (-1.0 - 1.0) </returns>
        public float GetPerlinNoise(float[] f)
        {
            CheckDimension(f.Length);
            return TCOD_noise_get(m_instance, f);
        }

        /// <summary>
        /// Get Browian Motion
        /// </summary>
        /// <param name="f">An array of coordinates</param>
        /// <param name="octaves">Number of iterations. (0-127)</param>
        /// <returns>Browian motion for that point (-1.0 - 1.0)</returns>
        public float GetBrownianMotion(float[] f, float octaves)
        {
            CheckDimension(f.Length);
            return TCOD_noise_fbm(m_instance, f, octaves);
        }

        /// <summary>
        /// Get Turbulence
        /// </summary>
        /// <param name="f">An array of coordinates</param>
        /// <param name="octaves">Number of iterations. (0-127)</param>
        /// <returns>Turbulence for that point (-1.0 - 1.0)</returns>
        public float GetTurbulence(float[] f, float octaves)
        {
            CheckDimension(f.Length);
            return TCOD_noise_turbulence(m_instance, f, octaves);
        }
      
        /// <summary>
        /// Destory unmanaged noice generator.
        /// </summary>
        public void Dispose()
        {
            TCOD_noise_delete(m_instance);
        }

        /// <summary>
        /// Default hurst value for noise generator
        /// </summary>
        public const float NoiseDefaultHurst = 0.5f;
        
        /// <summary>
        /// Default Lacunarity value for noise generator
        /// </summary>
        public const float NoiseDefaultLacunarity = 2.0f;

        private IntPtr m_instance;
        private int m_dimensions;

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_noise_new(int dimensions, float hurst, float lacunarity, IntPtr random);
        
        // basic perlin noise
        [DllImport(DLLName.name)]
        private extern static float TCOD_noise_get(IntPtr noise, float [] f);

        // fractional brownian motion
        [DllImport(DLLName.name)]
        private extern static float TCOD_noise_fbm(IntPtr noise, float [] f, float octaves);

        // turbulence
        [DllImport(DLLName.name)]
        private extern static float TCOD_noise_turbulence(IntPtr noise, float [] f, float octaves);

        [DllImport(DLLName.name)]
        private extern static void TCOD_noise_delete(IntPtr noise);
        #endregion

        private void CheckDimension(int dims)
        {
            if (m_dimensions != dims)
                throw new Exception("TCODNoise: Dimension mismatch");
        }
    };
}
