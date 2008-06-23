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

        public static bool StepLine(out int xCur, out int yCur)
        {
            return TCOD_line_step(out xCur, out yCur);
        }

        [DllImport(DLLName.name)]
        private extern static void TCOD_line_init(int xFrom, int yFrom, int xTo, int yTo);

        //returns true when reached line endpoint
        [DllImport(DLLName.name)]
        private extern static bool TCOD_line_step(out int xCur, out int yCur);
    }

    public class TCODLineDrawingTest
    {
        public static bool TestTCODLineDrawing()
        {
            TCODLineDrawing.InitLine(2, 2, 5, 5);
            int x=2, y=2;
            for (int i = 0; i < 4; ++i)
            {
                if ((2 + i != x) || (2 + i != y))
                    return false;
                TCODLineDrawing.StepLine(out x, out y);
            }
            return true;
        }
    }
}
