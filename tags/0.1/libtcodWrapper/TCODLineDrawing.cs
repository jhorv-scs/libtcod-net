using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcodWrapper
{
    //This class is not thread safe, nor is it safe to use more than one "instance" at the same time!
    public class TCODLineDrawing
    {
        public static void InitLine(int xFrom, int yFrom, int xTo, int yTo)
        {
            TCOD_line_init(xFrom, yFrom, xTo, yTo); 
        }

        public static bool StepLine(ref int xCur, ref int yCur)
        {
            return TCOD_line_step(ref xCur, ref yCur);
        }

        [DllImport(DLLName.name)]
        private extern static void TCOD_line_init(int xFrom, int yFrom, int xTo, int yTo);

        //returns true when reached line endpoint
        [DllImport(DLLName.name)]
        private extern static bool TCOD_line_step(ref int xCur, ref int yCur);
    }
}
