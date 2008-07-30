using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODBSPTest
    {
        [Test]
        public void SplitOnce()
        {
            TCODBSP bspSized = new TCODBSP(0, 0, 10, 10);
            TCODBSPHandler bspHandle = new TCODBSPHandler();
            bspHandle.SplitOnce(ref bspSized, false, 5);
            TCODBSP left = bspHandle.GetLeft(bspSized);
            Assert.AreEqual(left.w, 5);
            TCODBSP right = bspHandle.GetRight(bspSized);
            Assert.AreEqual(right.w, 5);
        }
    }
}
