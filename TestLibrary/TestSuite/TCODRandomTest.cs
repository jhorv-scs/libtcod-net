using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODRandomTest
    {
        private TCODRandom r;

        [SetUp]
        public void Init()
        {
            r = new TCODRandom();
        }

        [TearDown]
        public void Cleanup()
        {
            r.Dispose();
        }

        [Test]
        public void TestConstructors()
        {
            TCODRandom a = new TCODRandom();
            TCODRandom c = new TCODRandom(42);
            a.Dispose();
            c.Dispose();
        }

        [Test]
        public void TestRandomInts()
        {
            for (int i = 0; i < 1000; ++i)
                AssertRange(r.GetRandomInt(0, i), 0, i);
        }

        [Test]
        public void TestRandomFloats()
        {
            for (int i = 0; i < 1000; ++i)
                AssertRange(r.GetRandomFloat(0, i), 0, i);
        }

        [Test]
        public void TestNegativeRequests()
        {
            for (int i = 0; i < 1000; ++i)
            {
                AssertRange(r.GetRandomInt(-10, -1), -10, -1);
                AssertRange(r.GetRandomFloat(-9.9, -.1), -9.9, -.1);
            }
        }

        [Test]
        public void TestByteArray()
        {
            int result = r.GetIntFromByteArray(0, 1000, "Hello World");
            AssertRange(result, 0, 1000);

            int secondResult = r.GetIntFromByteArray(0, 1000, "Hello World");
            Assert.AreEqual(result, secondResult);
            
            AssertRange(r.GetIntFromByteArray(0, 1000, "Hello"), 0, 1000);
        }

        private static void AssertRange(double result, double low, double high)
        {
            Assert.IsFalse(result < low || result > high);
        }
    }
}