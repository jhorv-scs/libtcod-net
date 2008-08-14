using System;
using System.IO;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODImageTest
    {
        private RootConsole console;
        [TestFixtureSetUp]
        public void Init()
        {
            RootConsole.Width = 80;
            RootConsole.Height = 50;
            RootConsole.WindowTitle = "Image Testing";
            RootConsole.Fullscreen = false;

            console = RootConsole.GetInstance();
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
            using (Image i = new Image(40, 40))
            {
                using (Image j = new Image("terminal.bmp"))
                {
                    //TODO: Test this once TCOD_image_from_console gets fixed for root console.
                    //using (Image k = new Image(console))
                    {
                    }
                }
            }
        }

        [Test]
        public void ClearTest()
        {
            using (Image i = new Image(40, 40))
            {
                i.Clear(Color.Black);
            }
        }

        [Test]
        public void SaveToDiskTest()
        {
            bool testPasses = true;
            using (Image j = new Image("terminal.bmp"))
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
            using (Image i = new Image(40, 80))
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
            using (Image i = new Image(40, 80))
            {
                i.Clear(Color.DarkRed);
                Assert.IsTrue(i.GetPixel(5, 5) == Color.DarkRed);
                i.PutPixel(5, 5, Color.Gold);
                Assert.IsTrue(i.GetPixel(5, 5) != Color.DarkRed);
                Assert.IsTrue(i.GetPixel(5, 5) == Color.Gold);
            }
        }

        [Test]
        public void CheckMipMap()
        {
            using (Image i = new Image(40, 80))
            {
                i.Clear(Color.DarkPurple);
                i.AverageColorOfRegion(2, 4, 10, 20);
                
            }
        }

        [Test]
        public void BlitTest()
        {
            using (Image i = new Image(40, 80))
            {
                console.Clear();
                i.Clear(Color.BrightRed);
                i.BlitRect(console, 10, 10, 40, 80, new Background(BackgroundFlag.Add));
                i.Clear(Color.Purple);
                i.Blit(console, 40, 50, new Background(BackgroundFlag.Add), 1.0, 1.5, 45);
                console.Flush();
            }
        }

        [Test]
        public void TransparencyTest()
        {
            using (Image i = new Image(40, 80))
            {
                i.Clear(Color.Purple);
                Assert.IsFalse(i.GetPixelTransparency(2, 2));
                i.SetKeyColor(Color.Purple);
                Assert.IsTrue(i.GetPixelTransparency(2, 2));
                i.PutPixel(2, 2, Color.Orange);
                Assert.IsFalse(i.GetPixelTransparency(2, 2));
                Assert.IsTrue(i.GetPixelTransparency(2, 4));
            }
        }
    }
}
