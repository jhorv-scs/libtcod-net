using System;
using System.Text;
using System.Runtime.InteropServices;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
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

        private static bool NewStructCallbackTest(TCODParserStructure cur, string name)
        {
            switch (currentState)
            {
                case currentStructType.none:
                    if (name.ToString() != "blade" && cur.GetName() != "item_type")
                        return false;

                    if (!cur.IsManatory("cost"))
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
        private static bool NewFlagCallbackTest(string name)
        {
            if (name.ToString() != "abstract")
                return false;
            if (!cost_defined || !weight_defined || !deal_damage_defined || !damages_defined || !color_defined ||
                !damaged_color_defined || !damage_type_defined || !list_defined)
                return false;
            return true;
        }
        private static bool NewPropertyCallbackTest(string name, TCODValueType type, TCODValue value)
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
        private static bool EndStructCallbackTest(TCODParserStructure cur, string name)
        {
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
        private static void ErrorCallbackTest(string msg)
        {
        }

        [Test]
        public void ImplementedParserTest()
        {
			TCODParserCallbackStruct callback = new TCODParserCallbackStruct(new NewStructureCallback(NewStructCallbackTest), new NewFlagCallback(NewFlagCallbackTest),			                                                                 
			                                                                 new NewPropertyCallback(NewPropertyCallbackTest), new EndStructureCallback(EndStructCallbackTest),
			                                                                 new ErrorCallback(ErrorCallbackTest));

            using (TCODFileParser parser = new TCODFileParser())
            {
                AddParserTestStructs(parser);
                parser.Run("exampleConfig.txt", ref callback);
            }
        }

        [Test]
        public void DefaultParserTest()
        {
            using (TCODFileParser parser = new TCODFileParser())
            {
                AddParserTestStructs(parser);
                parser.Run("exampleConfig.txt");

                Assert.IsTrue(parser.GetIntProperty("item_type.cost") == 300);
                Assert.IsTrue(parser.GetFloatProperty("item_type.weight") == 3.5);
                Assert.IsTrue(parser.GetBoolProperty("item_type.deal_damage") == true);

                TCODDice testDice = new TCODDice(3, 6, 1, 2);
                Assert.IsTrue(parser.GetDiceProperty("item_type.damages") == testDice);

                TCODColor color1 = new TCODColor(255, 0, 0);
                TCODColor color2 = new TCODColor(128, 96, 96);
                Assert.IsTrue(parser.GetColorProperty("item_type.color") == color1);
                Assert.IsTrue(parser.GetColorProperty("item_type.damaged_color") == color2);
                Assert.IsTrue(parser.GetStringProperty("item_type.damage_type") == "slash");
                
                //parser.getValueList("list");
                //parser.GetFlag("abstract");
                Assert.IsTrue(parser.GetStringProperty("video.mode") == "800x600");
                Assert.IsTrue(parser.GetBoolProperty("video.fullscreen") == false);
                Assert.IsTrue(parser.GetFloatProperty("input.mouse.sensibility") == .5);
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