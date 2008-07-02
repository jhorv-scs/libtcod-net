using System;
using System.IO;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODImageTest
    {
        private TCODConsoleRoot console;
        [TestFixtureSetUp]
        public void Init()
        {
            console = new TCODConsoleRoot(120, 100, "Image Testing", false);
            console.Clear();
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            console.Dispose();
        }
        
        [Test]
        public void ImageConstructors()
        {
            using (TCODImage i = new TCODImage(40, 40))
            {
                using (TCODImage j = new TCODImage("terminal.bmp"))
                {
                    //TODO: Test this once TCOD_image_from_console gets fixed for root console.
                    //using (TCODImage k = new TCODImage(console))
                    {
                    }
                }
            }
        }

        [Test]
        public void ClearTest()
        {
            using (TCODImage i = new TCODImage(40, 40))
            {
                i.Clear(TCODColor.TCOD_black);
            }
        }

        [Test]
        public void SaveToDiskTest()
        {
            bool testPasses = true;
            using (TCODImage j = new TCODImage("terminal.bmp"))
            {
                j.SaveImageToDisc("testImage.bmp");
                FileInfo info = new FileInfo("testImage.bmp");
                if (!info.Exists)
                    testPasses = false;
                else
                    File.Delete("testImage.bmp");
                Assert.IsTrue(testPasses);
            }
        }

        [Test]
        public void CheckSize()
        {
            using (TCODImage i = new TCODImage(40, 80))
            {
                int w, h;
                i.GetSize(out w, out h);
                Assert.AreEqual(w, 40);
                Assert.AreEqual(h, 80);
            }
        }

        [Test]
        public void CheckPixel()
        {
            using (TCODImage i = new TCODImage(40, 80))
            {
                i.Clear(TCODColor.TCOD_dark_red);
                Assert.IsTrue(i.GetPixel(5, 5) == TCODColor.TCOD_dark_red);
                i.PutPixel(5, 5, TCODColor.TCOD_gold);
                Assert.IsTrue(i.GetPixel(5, 5) != TCODColor.TCOD_dark_red);
                Assert.IsTrue(i.GetPixel(5, 5) == TCODColor.TCOD_gold);
            }
        }

        [Test]
        public void CheckMipMap()
        {
            using (TCODImage i = new TCODImage(40, 80))
            {
                i.Clear(TCODColor.TCOD_dark_purple);
                TCODColor foo = i.GetMipMaps(2, 4, 10, 20);
            }
        }

        [Test]
        public void BlitTest()
        {
            using (TCODImage i = new TCODImage(40, 80))
            {
                console.Clear();
                i.Clear(TCODColor.TCOD_red);
                i.BlitRect(console, 10, 10, 40, 80, TCOD_bkgnd_flag.TCOD_BKGND_ADD);
                i.Clear(TCODColor.TCOD_purple);
                i.Blit(console, 40, 50, TCOD_bkgnd_flag.TCOD_BKGND_ADD, 1.0, 1.5, 45);
                console.Flush();
            }
        }

        [Test]
        public void TransparencyTest()
        {
            using (TCODImage i = new TCODImage(40, 80))
            {
                i.Clear(TCODColor.TCOD_purple);
                Assert.IsFalse(i.GetPixelTransparency(2, 2));
                i.SetKeyColor(TCODColor.TCOD_purple);
                Assert.IsTrue(i.GetPixelTransparency(2, 2));
                i.PutPixel(2, 2, TCODColor.TCOD_orange);
                Assert.IsFalse(i.GetPixelTransparency(2, 2));
                Assert.IsTrue(i.GetPixelTransparency(2, 4));
            }
        }
    }
}