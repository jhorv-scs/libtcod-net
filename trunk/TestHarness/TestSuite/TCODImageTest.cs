using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODImageTest
    {
        private TCODConsoleRoot console;
        [SetUp]
        public void Init()
        {
            console = new TCODConsoleRoot(80, 50, "Image Testing", false);
        }

        [TearDown]
        public void Cleanup()
        {
            console.Dispose();
        }
        
        [Test]
        public void ImageConstructors()
        {
            TCODImage i = new TCODImage(40, 40);
            TCODImage j = new TCODImage("terminal.bmp");
            TCODImage k = new TCODImage(console);
        }
    }
}