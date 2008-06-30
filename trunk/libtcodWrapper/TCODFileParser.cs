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
        internal TCODParserStructure(IntPtr p)
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


    public class TCODFileParserTest
    {
        enum currentStructType { none, item, video, input, mouse, inputDone };
        static private currentStructType currentState = currentStructType.none;
        static bool cost_defined = false;
        static bool weight_defined = false;
        static bool deal_damage_defined = false;
        static bool damages_defined = false;
        static bool color_defined = false;
        static bool damaged_color_defined = false;
        static bool damage_type_defined = false;
        static bool list_defined = false;

        static public bool TestFileParser()
        {

            if (DefaultParserTest())
            {
                if (ImplementedParserTest())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool NewStructCallbackTest(IntPtr str, StringBuilder name)
        {
            TCODParserStructure cur = new TCODParserStructure(str);                      

            switch (currentState)
            {
                case currentStructType.none:
                    if (name.ToString() != "blade" && cur.GetName() != "item_type")
                        return false;
                    
                    if(!cur.IsManatory("cost"))
                        return false;
                    if (cur.GetType("cost") != TCODValueType.TCOD_TYPE_INT)
                        return false;
                    
                    currentState = currentStructType.item;
                    break;
                case currentStructType.item:
                    if (name != null && cur.GetName() != "video")
                        return false;
                    currentState = currentStructType.video;
                    break;
                case currentStructType.video:
                    if (name != null && cur.GetName() != "input")
                        return false;
                    currentState = currentStructType.input;
                    break;
                case currentStructType.input:
                    if (name != null && cur.GetName() != "mouse")
                        return false;
                    currentState = currentStructType.mouse;
                    break;
                case currentStructType.inputDone:
                    return false;   //Should never get here
                case currentStructType.mouse:
                    return false;   //Should never get here
            }
            return true;
        }
        private static bool NewFlagCallbackTest(StringBuilder name)
        {
            if (name.ToString() != "abstract")
                return false;
            if (!cost_defined || !weight_defined || !deal_damage_defined || !damages_defined || !color_defined ||
                !damaged_color_defined || !damage_type_defined || !list_defined)
                return false;
            return true;           
        }
        private static bool NewPropertyCallbackTest(StringBuilder name, TCODValueType type, TCODValue value)
        {
            switch (type)
            {
                case TCODValueType.TCOD_TYPE_BOOL:
                    deal_damage_defined = true;
                    break;
                case TCODValueType.TCOD_TYPE_COLOR:
                    if (name.ToString() == "color")
                        color_defined = true;
                    else
                        damaged_color_defined = true;
                    break;
                case TCODValueType.TCOD_TYPE_DICE:
                    damages_defined = true;
                    break;
                case TCODValueType.TCOD_TYPE_FLOAT:
                    weight_defined = true;
                    break;
                case TCODValueType.TCOD_TYPE_INT:
                    cost_defined = true;
                    break;
                case TCODValueType.TCOD_TYPE_STRING:
                    damage_type_defined = true;
                    break;
                case TCODValueType.TCOD_TYPE_VALUELIST00:
                    list_defined = true;
                    break;
                default:
                    return false;
            }
            return true;
        }
        private static bool EndStructCallbackTest(IntPtr str, StringBuilder name)
        {
            TCODParserStructure cur = new TCODParserStructure(str);
            switch (currentState)
            {
                case currentStructType.none:
                    return false;
                case currentStructType.item:
                    if (name.ToString() != "blade" || cur.GetName() != "item_type")
                        return false;
                    break;
                case currentStructType.video:
                    if (name != null || cur.GetName() != "video")
                        return false;
                    break;
                case currentStructType.mouse:
                    if (name != null || cur.GetName() != "mouse")
                        return false;
                    currentState = currentStructType.inputDone;
                    break;
                case currentStructType.inputDone:
                    if (name != null || cur.GetName() != "input")
                        return false;
                    break;
                case currentStructType.input:
                    return false;
            }
            return true;
        }
        private static void ErrorCallbackTest(StringBuilder msg)
        {
        }

        private static bool ImplementedParserTest()
        {
            TCODParserCallbackStruct callback = new TCODParserCallbackStruct();
            callback.new_structure = new new_struct_delegate(NewStructCallbackTest);
            callback.new_flag = new new_flag_delegate(NewFlagCallbackTest);
            callback.new_property = new new_property_delegate(NewPropertyCallbackTest);
            callback.end_struct = new end_struct_delegate(EndStructCallbackTest);
            callback.error = new error_delegate(ErrorCallbackTest);

            using (TCODFileParser parser = new TCODFileParser())
            {
                AddParserTestStructs(parser);
                parser.Run("exampleConfig.txt", ref callback);
            }
          
            return true;
        }

        private static bool DefaultParserTest()
        {
            using (TCODFileParser parser = new TCODFileParser())
            {
                bool testPassed = true;

                AddParserTestStructs(parser);
                parser.Run("exampleConfig.txt");

                if (!testPassed || parser.GetIntProperty("item_type.cost") != 300)
                    testPassed = false;
                if (!testPassed || parser.GetFloatProperty("item_type.weight") != 3.5)
                    testPassed = false;
                if (!testPassed || parser.GetBoolProperty("item_type.deal_damage") != true)
                    testPassed = false;

                TCODDice testDice = new TCODDice(3, 6, 1, 2);
                if (!testPassed || parser.GetDiceProperty("item_type.damages") != testDice)
                    testPassed = false;

                TCODColor color1 = new TCODColor(255, 0, 0);
                TCODColor color2 = new TCODColor(128, 96, 96);
                if (!testPassed || parser.GetColorProperty("item_type.color") != color1)
                    testPassed = false;
                if (!testPassed || parser.GetColorProperty("item_type.damaged_color") != color2)
                    testPassed = false;
                if (!testPassed || parser.GetStringProperty("item_type.damage_type") != "slash")
                    testPassed = false;
                //parser.getValueList("list");
                //parser.GetFlag("abstract");
                if (!testPassed || parser.GetStringProperty("video.mode") != "800x600")
                    testPassed = false;
                if (!testPassed || parser.GetBoolProperty("video.fullscreen") != false)
                    testPassed = false;
                if (!testPassed || parser.GetFloatProperty("input.mouse.sensibility") != .5)
                    testPassed = false;
                return testPassed;
            }
        }

        private static void AddParserTestStructs(TCODFileParser parser)
        {
            string[] strList = new string[3];
            strList[0] = "moo";
            strList[1] = "foo";
            strList[2] = "bar";

            TCODParserStructure item = parser.RegisterNewStructure("item_type");
            item.AddProperty("cost", TCODValueType.TCOD_TYPE_INT, true);
            item.AddProperty("weight", TCODValueType.TCOD_TYPE_FLOAT, true);
            item.AddProperty("deal_damage", TCODValueType.TCOD_TYPE_BOOL, true);
            item.AddProperty("damages", TCODValueType.TCOD_TYPE_DICE, true);
            item.AddProperty("color", TCODValueType.TCOD_TYPE_COLOR, true);
            item.AddProperty("damaged_color", TCODValueType.TCOD_TYPE_COLOR, true);
            item.AddProperty("damage_type", TCODValueType.TCOD_TYPE_STRING, true);
            item.AddValueList("list", strList, true);
            item.AddFlag("abstract");

            TCODParserStructure video = parser.RegisterNewStructure("video");
            video.AddProperty("mode", TCODValueType.TCOD_TYPE_STRING, true);
            video.AddProperty("fullscreen", TCODValueType.TCOD_TYPE_BOOL, true);

            TCODParserStructure input = parser.RegisterNewStructure("input");
            TCODParserStructure mouse = parser.RegisterNewStructure("mouse");
            mouse.AddProperty("sensibility", TCODValueType.TCOD_TYPE_FLOAT, true);
            input.AddSubStructure(mouse);
        }
        
    }
}
