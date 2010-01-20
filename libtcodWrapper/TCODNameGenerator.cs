using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libtcodWrapper
{    
    /// <summary>
    /// Generate random names using sylable files
    /// </summary>
    public static class TCODNameGenerator
    {
        // We never clean this up, but since it has global scope, that is OK.
        private static TCODRandom m_random;

        /// <summary>
        /// TCODNameGenerator, and it's unmanaged cousin, both have global scope
        /// </summary>
        static TCODNameGenerator()
        {
            m_random = new TCODRandom();
        }

        /// <summary>
        /// Load a sylable file into the name generator.
        /// May be called more that once.
        /// </summary>
        /// <param name="filename">Relative or absolute path to file.</param>
        public static void LoadSyllableFile(string filename)
        {
            TCOD_namegen_parse(filename, m_random.m_instance);
        }

        /// <summary>
        /// Throw out all parser data structures.
        /// LoadSyllableFile will need to be called again before using again
        /// </summary>
        public static void Reset()
        {
            TCOD_namegen_destroy();
        }

        /// <summary>
        /// Generate a random string based upon a type
        /// </summary>
        /// <param name="type">Type of string, such as "celtic female"</param>
        /// <returns></returns>
        public static string Generate(string type)
        {
            return Marshal.PtrToStringAnsi(TCOD_namegen_generate(type, false));
        }

        /// <summary>
        /// Generate a custom string based on internal string rules
        /// </summary>
        /// <param name="type">Type of string, such as "celtic female"</param>
        /// <param name="rule">Rules to follow, see libtcod documentation for more details</param>
        /// <returns></returns>
        public static string GenerateCustom(string type, string rule)
        {
            return Marshal.PtrToStringAnsi(TCOD_namegen_generate_custom(type, rule, false));
        }

        /// <summary>
        /// Generate a list of available set names.
        /// </summary>
        /// <returns>List of available set names</returns>
        public static List<string> GetSet()
        {
            List<string> returnList = new List<string>();
            IntPtr tcodStringList = TCOD_namegen_get_sets();

            int listSize = TCOD_list_size(tcodStringList);
            for(int i = 0 ; i < listSize ; ++i)
            {
                string str = Marshal.PtrToStringAnsi(TCOD_list_get(tcodStringList, i));
                returnList.Add(str);
            }

            TCOD_list_delete(tcodStringList);
            return returnList;
        }

        #region DllImport
        [DllImport(DLLName.name)]
        private extern static void TCOD_namegen_parse([MarshalAs(UnmanagedType.LPStr)]string filename, IntPtr random);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_namegen_generate([MarshalAs(UnmanagedType.LPStr)]string name, bool allocate);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_namegen_generate_custom(string name, string rule, bool allocate);
        
        [DllImport(DLLName.name)]
        private extern static void TCOD_namegen_destroy();

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_namegen_get_sets();

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_list_get(IntPtr l, int idx);

        [DllImport(DLLName.name)]
        private extern static int TCOD_list_size(IntPtr l);

        [DllImport(DLLName.name)]
        private extern static void TCOD_list_delete(IntPtr l);

        #endregion
    }
}
