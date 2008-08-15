using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class ColorTest
    {
        private Color normalConstructor;
        private Color sameConstructor;
        private Color diffConstructor;
        private Color HSVConstructor;

        [SetUp]
        public void Init()
        {
            normalConstructor = Color.FromRGB(2, 4, 6);
            sameConstructor = Color.FromRGB(2, 4, 6);
            diffConstructor = Color.FromRGB(4, 8, 12);
            HSVConstructor = Color.FromHSV(1.0f, .5f, .3f);
        }

        [TearDown]
        public void Cleanup()
        {
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
            normalConstructor.GetHashCode();
            Assert.IsTrue(normalConstructor.GetHashCode() == sameConstructor.GetHashCode());
        }

        [Test]
        public void TestNormalOperators()
        {
            Color mult = sameConstructor * diffConstructor;
            mult = mult * 1.5f;
            mult = mult * 1.5;
            Assert.IsTrue((normalConstructor + sameConstructor) == diffConstructor);

            Color.Interpolate(mult, normalConstructor, .4f);
        }

        [Test]
        public void TestDivide()
        {
            Color Orig = Color.FromRGB(200, 100, 50);
            Color Div = Orig / 10;
            Assert.AreEqual(Div.Red, 20);
            Assert.AreEqual(Div.Green, 10);
            Assert.AreEqual(Div.Blue, 5);
        }

        [Test]
        public void TestSubtract()
        {
            Color Big = Color.FromRGB(255, 255, 255);
            Color Tiny = Color.FromRGB(50, 50, 50);
            Color Zero = Tiny - Big;
            Color Middle = Big - Tiny;
            Assert.IsTrue(Zero == Color.FromRGB(0, 0, 0));
            Assert.IsTrue(Middle == Color.FromRGB(205, 205, 205));
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
