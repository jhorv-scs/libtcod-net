using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class ConsoleTest
    {
        [Test]
        public void TestConsoleBackground()
        {
            Background alpha = new Background(BackgroundFlag.ColorDodge);
            Background beta = new Background(BackgroundFlag.AddA, .75f);
            Assert.IsTrue(alpha.BackgroundFlag == BackgroundFlag.ColorDodge);
            Assert.AreEqual(alpha.AlphaValue, 0);
            Assert.IsTrue(beta.BackgroundFlag == BackgroundFlag.AddA);
            Assert.AreEqual(beta.AlphaValue, 191);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestInvalidBackground1()
        {
            //Not alpha w\ alpha set.
            Background alpha = new Background(BackgroundFlag.Burn, .75f);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestInvalidBackground2()
        {
            //Alpha w\o alpha set.
            Background alpha = new Background(BackgroundFlag.AddA);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestInvalidBackground3()
        {
            //Alpha w\o alpha set.
            Background alpha = new Background(BackgroundFlag.Alph);
        }

        [Test]
        public void TestIncrement()
        {
            Background b = new Background(BackgroundFlag.Multiply);
            b++;
            Assert.IsTrue(b.BackgroundFlag == BackgroundFlag.Lighten);
            b++;
            Assert.IsTrue(b.BackgroundFlag == BackgroundFlag.Darken);
        }

        [Test]
        public void TestDecrement()
        {
            Background b = new Background(BackgroundFlag.Burn);
            b--;
            Assert.IsTrue(b.BackgroundFlag == BackgroundFlag.AddA);
            b--;
            Assert.IsTrue(b.BackgroundFlag == BackgroundFlag.Add);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestIncrementOffEdge()
        {
            Background b = new Background(BackgroundFlag.Overlay);
            b++;
            b++;
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestDecrementOffEdge()
        {
            Background b = new Background(BackgroundFlag.Lighten);
            b--;
            b--;
            b--;
            b--;
        }

        [Test]
        public void TestBackgroundCopyConstructor()
        {
            Background b = new Background(BackgroundFlag.Burn);
            Background newB = new Background(b);
            Assert.IsTrue(b.AlphaValue == newB.AlphaValue);
            Assert.IsTrue(b.BackgroundFlag == newB.BackgroundFlag);
            b++;
            Assert.IsFalse(b.BackgroundFlag == newB.BackgroundFlag);
        }
    }
}
