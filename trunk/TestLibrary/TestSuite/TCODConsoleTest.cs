using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODConsoleTest
    {
        [Test]
        public void TestConsoleBackground()
        {
            TCODBackground alpha = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_COLOR_DODGE);
            TCODBackground beta = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_ADDA, .75f);
            Assert.IsTrue(alpha.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_COLOR_DODGE);
            Assert.AreEqual(alpha.GetAlphaValue(), 0);
            Assert.IsTrue(beta.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ADDA);
            Assert.AreEqual(beta.GetAlphaValue(), 191);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestInvalidBackground1()
        {
            //Not alpha w\ alpha set.
            TCODBackground alpha = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_BURN, .75f);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestInvalidBackground2()
        {
            //Alpha w\o alpha set.
            TCODBackground alpha = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_ADDA);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestInvalidBackground3()
        {
            //Alpha w\o alpha set.
            TCODBackground alpha = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_ALPH);
        }

        [Test]
        public void TestIncrement()
        {
            TCODBackground b = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_MULTIPLY);
            b++;
            Assert.IsTrue(b.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_LIGHTEN);
            b++;
            Assert.IsTrue(b.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_DARKEN);
        }

        [Test]
        public void TestDecrement()
        {
            TCODBackground b = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_BURN);
            b--;
            Assert.IsTrue(b.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ADDA);
            b--;
            Assert.IsTrue(b.GetBackgroundFlag() == TCOD_bkgnd_flag.TCOD_BKGND_ADD);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestIncrementOffEdge()
        {
            TCODBackground b = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_OVERLAY);
            b++;
            b++;
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void TestDecrementOffEdge()
        {
            TCODBackground b = new TCODBackground(TCOD_bkgnd_flag.TCOD_BKGND_LIGHTEN);
            b--;
            b--;
            b--;
            b--;
        }
    }
}