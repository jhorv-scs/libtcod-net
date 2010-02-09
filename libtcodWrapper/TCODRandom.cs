using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    /// <summary>
    /// Produces random numbers from the Mersenne Twister
    /// </summary>
    public class TCODRandom : IDisposable
    {
        private bool m_globalInstanceUsed;

        /// <summary>
        /// Types of random number generator algorithms
        /// </summary>
        public enum RandomGeneratorTypes
        {
            /// <summary>
            /// Mersenne Twister
            /// </summary>
            MersenneTwister=0,

            /// <summary>
            /// Complementary Multiply With Carry
            /// </summary>
            ComplementaryMultiplyWithCarry=1
        }

        /// <summary>
        /// Create a new instance of a random number generator.
        /// </summary>
        public TCODRandom()
        {
            m_instance = TCOD_random_get_instance();
            m_globalInstanceUsed = true;
        }

        /// <summary>
        /// Create a new instance of a random number generator with a specific algorithm
        /// </summary>
        /// <param name="type">Type of algorithm</param>
        public TCODRandom(RandomGeneratorTypes type)
        {
            m_instance = TCOD_random_new(type);
            m_globalInstanceUsed = false;
        }

        /// <summary>
        /// Create new instance of a random number generator with a starting seed.
        /// </summary>
        /// <param name="seed">Intial Seed</param>
        /// <param name="type">Type of algorithm</param>
        public TCODRandom(RandomGeneratorTypes type, uint seed)
        {
            m_instance = TCOD_random_new_from_seed(type, seed);
            m_globalInstanceUsed = false;
        }

        private TCODRandom(IntPtr instance)
        {
            m_instance = instance;
            m_globalInstanceUsed = false;
        }

        /// <summary>
        /// Destroy unmanaged random number generator
        /// </summary>
        public void Dispose()
        {
            if(!m_globalInstanceUsed)
                TCOD_random_delete(m_instance);
        }

        /// <summary>
        /// Obtain a random integer in a given range
        /// </summary>
        /// <param name="min">Minimum number to generate</param>
        /// <param name="max">Maximum number to generate</param>
        /// <returns>Random Number</returns>
        public int GetRandomInt(int min, int max)
        {
            return TCOD_random_get_int(m_instance, min, max);
        }

        /// <summary>
        /// Obtain a random floating point number in a given range
        /// </summary>
        /// <param name="min">Minimum number to generate</param>
        /// <param name="max">Maximum number to generate</param>
        /// <returns>Random Number</returns>
        public float GetRandomFloat(double min, double max)
        {
            return TCOD_random_get_float(m_instance, (float)min, (float)max);
        }

        /// <summary>
        /// Obtain a random floating point number in a given range
        /// </summary>
        /// <param name="min">Minimum number to generate</param>
        /// <param name="max">Maximum number to generate</param>
        /// <returns>Random Number</returns>
        public float GetRandomFloat(float min, float max)
        {
            return TCOD_random_get_float(m_instance, min, max);
        }

        /// <summary>
        /// Generate a floating point number with an approximated Gaussian distribution
        /// </summary>
        /// <param name="min">Minimum number to generate</param>
        /// <param name="max">Max number to generate</param>
        /// <returns>Generated number</returns>
        public float GetGaussianFloat(float min, float max)
        {
            return TCOD_random_get_gaussian_float(m_instance, min, max);
        }

        /// <summary>
        /// Generate an integer number with an approximated Gaussian distribution
        /// </summary>
        /// <param name="min">Minimum number to generate</param>
        /// <param name="max">Max number to generate</param>
        /// <returns>Generated number</returns>
        public int GetGaussianInt(int min, int max)
        {
            return TCOD_random_get_gaussian_int(m_instance, min, max);
        }

        /// <summary>
        /// Save state of generator and create clone.
        /// </summary>
        /// <returns>New TCODRandom with the given state.</returns>
        public TCODRandom Save()
        {
            return new TCODRandom(TCOD_random_save(m_instance));
        }

        internal IntPtr m_instance;

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_random_get_instance();

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_random_save(IntPtr mersenne);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_random_new(RandomGeneratorTypes type);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_random_new_from_seed(RandomGeneratorTypes type, uint seed);

        [DllImport(DLLName.name)]
        private extern static int TCOD_random_get_int(IntPtr mersenne, int min, int max);
        
        [DllImport(DLLName.name)]
        private extern static float TCOD_random_get_float(IntPtr mersenne, float min, float max);

        [DllImport(DLLName.name)]
        private extern static float TCOD_random_get_gaussian_float (IntPtr mersenne, float min, float max);

        [DllImport(DLLName.name)]
        private extern static int TCOD_random_get_gaussian_int (IntPtr mersenne, int min, int max);

        [DllImport(DLLName.name)]
        private extern static void TCOD_random_delete(IntPtr mersenne);
        #endregion
    }
}

