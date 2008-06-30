using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODLineDrawingTest
    {
        [Test]
        public void TestTCODLineDrawing()
        {
            TCODLineDrawing.InitLine(2, 2, 5, 5);
            int x = 2, y = 2;
            for (int i = 0; i < 4; ++i)
            {
                Assert.IsTrue((2 + i == x) && (2 + i == y));
                TCODLineDrawing.StepLine(ref x, ref y);
            }
        }
    }
}