using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODColorTest
    {
        private TCODColor emptyConstructor;
        private TCODColor copyConstructor;
        private TCODColor normalConstructor;
        private TCODColor sameConstructor;
        private TCODColor diffConstructor;
        private TCODColor HSVConstructor;

        [SetUp]
        public void Init()
        {
            emptyConstructor = new TCODColor();
            copyConstructor = new TCODColor(TCODColor.TCOD_silver);
            normalConstructor = new TCODColor(2, 4, 6);
            sameConstructor = new TCODColor(2, 4, 6);
            diffConstructor = new TCODColor(4, 8, 12);
            HSVConstructor = new TCODColor();
            HSVConstructor.SetHSV(1.0f, .5f, .3f);
        }

        [TearDown]
        public void Cleanup()
        {
        }

        [Test]
        public void TestCopyConstructor()
        {
            Assert.IsTrue(copyConstructor.Equals(TCODColor.TCOD_silver));
        }

        [Test]
        public void TestEqualityOperators()
        {
            Assert.IsTrue(normalConstructor == sameConstructor);
            Assert.IsTrue(!(normalConstructor == diffConstructor));
            Assert.IsTrue(normalConstructor != diffConstructor);
        }

        [Test]
        public void TestHashCode()
        {
            emptyConstructor.GetHashCode();
            Assert.IsTrue(normalConstructor.GetHashCode() == sameConstructor.GetHashCode());
        }

        [Test]
        public void TestNormalOperators()
        {
            TCODColor mult = sameConstructor * diffConstructor;
            mult = mult * 1.5f;
            mult = mult * 1.5;
            Assert.IsTrue((normalConstructor + sameConstructor) == diffConstructor);

            TCODColor.Interpolate(mult, normalConstructor, .4f);
        }

        [Test]
        public void TestDivide()
        {
            TCODColor Orig = new TCODColor(200, 100, 50);
            TCODColor Div = Orig / 10;
            Assert.AreEqual(Div.r, 20);
            Assert.AreEqual(Div.g, 10);
            Assert.AreEqual(Div.b, 5);
        }

        [Test]
        public void TestSubtract()
        {
            TCODColor Big = new TCODColor(255, 255, 255);
            TCODColor Tiny = new TCODColor(50, 50, 50);
            TCODColor Zero = Tiny - Big;
            TCODColor Middle = Big - Tiny;
            Assert.IsTrue(Zero == new TCODColor(0, 0, 0));
            Assert.IsTrue(Middle == new TCODColor(205, 205, 205));
        }

        [Test]
        public void TestHSV()
        {
            float h, s, v;
            HSVConstructor.GetHSV(out h, out s, out v);
            normalConstructor.GetHSV(out h, out s, out v);
        }
    }
}