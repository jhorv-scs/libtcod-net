using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    public enum TCODValueType
    { 
	    TCOD_TYPE_NONE,
	    TCOD_TYPE_BOOL,
	    TCOD_TYPE_CHAR,
	    TCOD_TYPE_INT,
	    TCOD_TYPE_FLOAT,
	    TCOD_TYPE_STRING,
	    TCOD_TYPE_COLOR,
	    TCOD_TYPE_DICE,
	    TCOD_TYPE_VALUELIST00,
	    TCOD_TYPE_VALUELIST01,
	    TCOD_TYPE_VALUELIST02,
	    TCOD_TYPE_VALUELIST03,
	    TCOD_TYPE_VALUELIST04,
	    TCOD_TYPE_VALUELIST05,
	    TCOD_TYPE_VALUELIST06,
	    TCOD_TYPE_VALUELIST07,
	    TCOD_TYPE_VALUELIST08,
	    TCOD_TYPE_VALUELIST09,
	    TCOD_TYPE_VALUELIST10,
	    TCOD_TYPE_VALUELIST11,
	    TCOD_TYPE_VALUELIST12,
	    TCOD_TYPE_VALUELIST13,
	    TCOD_TYPE_VALUELIST14,
	    TCOD_TYPE_VALUELIST15,
	    TCOD_TYPE_CUSTOM00,
	    TCOD_TYPE_CUSTOM01,
	    TCOD_TYPE_CUSTOM02,
	    TCOD_TYPE_CUSTOM03,
	    TCOD_TYPE_CUSTOM04,
	    TCOD_TYPE_CUSTOM05,
	    TCOD_TYPE_CUSTOM06,
	    TCOD_TYPE_CUSTOM07,
	    TCOD_TYPE_CUSTOM08,
	    TCOD_TYPE_CUSTOM09,
	    TCOD_TYPE_CUSTOM10,
	    TCOD_TYPE_CUSTOM11,
	    TCOD_TYPE_CUSTOM12,
	    TCOD_TYPE_CUSTOM13,
	    TCOD_TYPE_CUSTOM14,
	    TCOD_TYPE_CUSTOM15	
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
    unsafe public struct TCODValue
    {
        [FieldOffset(0)]
       	public bool b;
          
           [FieldOffset(0)]
           public byte c;
        
           [FieldOffset(0)]
           public int i; 
        
           [FieldOffset(0)]
           public float f;
        
           [FieldOffset(0)]
           public fixed char s[512];        

           [FieldOffset(0)]
           public TCODColor col;
        
           [FieldOffset(0)]
           public TCODDice dice;

           [FieldOffset(0)]
           public IntPtr custom;
    }

    [StructLayout(LayoutKind.Sequential) ]
    public struct TCODDice
    {
        public int nb_dices;
        public int nb_faces;
        public float multiplier;
        public float addsub;

        public TCODDice(int dices, int faces, int mult, int constant)
        {
            nb_dices = dices;
            nb_faces = faces;
            multiplier = mult;
            addsub = constant;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            TCODDice rhs = (TCODDice)obj;
            return (nb_dices == rhs.nb_dices) && (nb_faces == rhs.nb_faces) && (multiplier == rhs.multiplier) && (addsub == rhs.addsub);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TCODDice lhs, TCODDice rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(TCODDice lhs, TCODDice rhs)
        {
            return !lhs.Equals(rhs);
        }
    }

    //Parser Struct
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool new_struct_delegate(IntPtr str, StringBuilder name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool new_flag_delegate(StringBuilder name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool new_property_delegate(StringBuilder name, TCODValueType type, TCODValue value);

    //Parser Struct
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool end_struct_delegate(IntPtr str, StringBuilder name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void error_delegate(StringBuilder msg);

    [StructLayout(LayoutKind.Sequential) ]
    public struct TCODParserCallbackStruct
    {
        public new_struct_delegate new_structure;

        public new_flag_delegate new_flag;

        public new_property_delegate new_property;

        public end_struct_delegate end_struct;

        public error_delegate error;

        public void ReturnErrorToParser(string error)
        {
            TCOD_parser_error(new StringBuilder(error));
        }

        [DllImport(DLLName.name)]
        private extern static void TCOD_parser_error(StringBuilder msg);
	};

    public class TCODFileParser : IDisposable
    {
        internal IntPtr m_fileParser;

        public TCODFileParser()
        {
            m_fileParser = TCOD_parser_new();
        }

        public void Dispose()
        {
            TCOD_parser_delete(m_fileParser);
        }

        public void Run(string filename, ref TCODParserCallbackStruct listner)
        {
            TCOD_parser_run(m_fileParser, new StringBuilder(filename), ref listner);
        }

        public void Run(string filename)
        {
            TCOD_parser_run(m_fileParser, new StringBuilder(filename), IntPtr.Zero);
        }

        public TCODParserStructure RegisterNewStructure(string name)
        {
            return new TCODParserStructure(TCOD_parser_new_struct(m_fileParser, new StringBuilder(name)));
        }

        public bool GetBoolProperty(string name)
        {
            return TCOD_parser_get_bool_property(m_fileParser, new StringBuilder(name));
        }

        public int GetIntProperty(string name)
        {
            return TCOD_parser_get_int_property(m_fileParser, new StringBuilder(name));
        }

        public float GetFloatProperty(string name)
        {
            return TCOD_parser_get_float_property(m_fileParser, new StringBuilder(name));
        }

        public string GetStringProperty(string name)
        {
            return TCOD_parser_get_string_property_helper(m_fileParser, new StringBuilder(name));
        }

        public TCODColor GetColorProperty(string name)
        {
            return TCOD_parser_get_color_property(m_fileParser, new StringBuilder(name));
        }

        public TCODDice GetDiceProperty(string name)
        {
            return TCOD_parser_get_dice_property(m_fileParser, new StringBuilder(name));
        }
        
        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_parser_new();

        [DllImport(DLLName.name)]
        private extern static void TCOD_parser_run(IntPtr parser, StringBuilder filename, ref TCODParserCallbackStruct listener);

        [DllImport(DLLName.name)]
        private extern static void TCOD_parser_run(IntPtr parser, StringBuilder filename, IntPtr nullListener);

        [DllImport(DLLName.name)]
        private extern static void TCOD_parser_delete(IntPtr parser);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_parser_new_struct(IntPtr parser, StringBuilder name);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_parser_get_bool_property(IntPtr parser, StringBuilder name);
        
        [DllImport(DLLName.name)]
        private extern static int TCOD_parser_get_int_property(IntPtr parser, StringBuilder name);
        
        [DllImport(DLLName.name)]
        private extern static float TCOD_parser_get_float_property(IntPtr parser, StringBuilder name);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_parser_get_string_property(IntPtr parser, StringBuilder name);
        
        private static string TCOD_parser_get_string_property_helper(IntPtr parser, StringBuilder name)
        {
            return Marshal.PtrToStringAnsi(TCOD_parser_get_string_property(parser, name));
        }

        [DllImport(DLLName.name)]
        private extern static TCODColor TCOD_parser_get_color_property(IntPtr parser, StringBuilder name);

        [DllImport(DLLName.name)]
        private extern static TCODDice TCOD_parser_get_dice_property(IntPtr parser, StringBuilder name);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_parser_get_custom_property(IntPtr parser, StringBuilder name);
    }

    public class TCODParserStructure
    {
        public TCODParserStructure(IntPtr p)
        {
            m_parserStructure = p;
        }

        internal IntPtr m_parserStructure;

        public void AddFlag(string name)
        {
            TCOD_struct_add_flag(m_parserStructure, new StringBuilder(name));
        }

        public void AddProperty(string name, TCODValueType type, bool mandatory)
        {
            TCOD_struct_add_property(m_parserStructure, new StringBuilder(name), type, mandatory);
        }

        public void AddValueList(string name, string[] list, bool mandatory)
        {
            TCOD_struct_add_value_list_sized(m_parserStructure, new StringBuilder(name), list, list.Length, mandatory);
        }

        public void AddSubStructure(TCODParserStructure substructure)
        {
            TCOD_struct_add_structure(m_parserStructure, substructure.m_parserStructure);
        }

        public string GetName()
        {
            return TCOD_struct_get_name_helper(m_parserStructure);
        }

        public bool IsManatory(string name)
        {
            return TCOD_struct_is_mandatory(m_parserStructure, new StringBuilder(name));
        }

        public TCODValueType GetType(string name)
        {
            return TCOD_struct_get_type(m_parserStructure, new StringBuilder(name));
        }

        [DllImport(DLLName.name)]
        private extern static void TCOD_struct_add_flag(IntPtr str, StringBuilder name);

        [DllImport(DLLName.name)]
        private extern static void TCOD_struct_add_property(IntPtr def, StringBuilder name, TCODValueType type, bool mandatory);

        [DllImport(DLLName.name)]
        private extern static void TCOD_struct_add_value_list_sized(IntPtr def, StringBuilder name, [In, Out] String[] value_list, int size, bool mandatory);

        [DllImport(DLLName.name)]
        private extern static void TCOD_struct_add_structure(IntPtr str, IntPtr sub_structure);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_struct_get_name(IntPtr str);
        
        private static string TCOD_struct_get_name_helper(IntPtr str)
        {
			return Marshal.PtrToStringAnsi(TCOD_struct_get_name(str));
        }

        [DllImport(DLLName.name)]
        private extern static bool TCOD_struct_is_mandatory(IntPtr str, StringBuilder name);

        [DllImport(DLLName.name)]
        private extern static TCODValueType TCOD_struct_get_type(IntPtr str, StringBuilder name);
    }
}
