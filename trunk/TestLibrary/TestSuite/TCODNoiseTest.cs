using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODNoiseTest
    {
        TCODNoise perlin1d;
        TCODNoise perlin2d;
        TCODNoise perlin3d;
        TCODNoise perlin4d;
        TCODNoise brownian1d;
        TCODNoise brownian2d;
        TCODNoise brownian3d;
        TCODNoise brownian4d;
        TCODNoise turbulence1d;
        TCODNoise turbulence2d;
        TCODNoise turbulence3d;
        TCODNoise turbulence4d;


        [SetUp]
        public void Init()
        {
            perlin1d = new TCODNoise(1);
            perlin2d = new TCODNoise(2);
            perlin3d = new TCODNoise(3);
            perlin4d = new TCODNoise(4);
            brownian1d = new TCODNoise(1);
            brownian2d = new TCODNoise(2);
            brownian3d = new TCODNoise(3);
            brownian4d = new TCODNoise(4);
            turbulence1d = new TCODNoise(1);
            turbulence2d = new TCODNoise(2);
            turbulence3d = new TCODNoise(3);
            turbulence4d = new TCODNoise(4);
        }

        [TearDown]
        public void Cleanup()
        {
            perlin1d.Dispose();
            perlin2d.Dispose();
            perlin3d.Dispose();
            perlin4d.Dispose();
            brownian1d.Dispose();
            brownian2d.Dispose();
            brownian3d.Dispose();
            brownian4d.Dispose();
            turbulence1d.Dispose();
            turbulence2d.Dispose();
            turbulence3d.Dispose();
            turbulence4d.Dispose();
        }

        [Test]
        public void GetPerlinTest1d()
        {
            float[] f = new float[1];
            f[0] = .3f;
            perlin1d.GetPerlinNoise(f);
            Assert.IsTrue(f.Length == 1);
        }

        [Test]
        public void GetPerlinTest2d()
        {
            float[] f = new float[2];
            f[0] = .1f;
            f[1] = .2f;
            perlin2d.GetPerlinNoise(f);
            Assert.IsTrue(f.Length == 2);
        }

        [Test]
        public void GetPerlinTest3d()
        {
            float[] f = new float[3];
            f[0] = .1f;
            f[1] = .2f;
            f[2] = .3f;
            perlin3d.GetPerlinNoise(f);
            Assert.IsTrue(f.Length == 3);
        }

        [Test]
        public void GetPerlinTest4d()
        {
            float[] f = new float[4];
            f[0] = .1f;
            f[1] = .2f;
            f[2] = .3f;
            f[3] = .6f;
            perlin4d.GetPerlinNoise(f);
            Assert.IsTrue(f.Length == 4);
        }

        [Test]
        public void GetBrownianTest1d()
        {
            float[] f = new float[1];
            f[0] = .3f;
            perlin1d.GetPerlinBrownianMotion(f, 32.0f);
            Assert.IsTrue(f.Length == 1);
        }

        [Test]
        public void GetBrownianTest2d()
        {
            float[] f = new float[2];
            f[0] = .1f;
            f[1] = .2f;
            perlin2d.GetPerlinBrownianMotion(f, 32.0f);
            Assert.IsTrue(f.Length == 2);
        }

        [Test]
        public void GetBrownianTest3d()
        {
            float[] f = new float[3];
            f[0] = .1f;
            f[1] = .2f;
            f[2] = .3f;
            perlin3d.GetPerlinBrownianMotion(f, 32.0f);
            Assert.IsTrue(f.Length == 3);
        }

        [Test]
        public void GetBrownianTest4d()
        {
            float[] f = new float[4];
            f[0] = .1f;
            f[1] = .2f;
            f[2] = .3f;
            f[3] = .6f;
            perlin4d.GetPerlinBrownianMotion(f, 32.0f);
            Assert.IsTrue(f.Length == 4);
        }

        [Test]
        public void GetTurbulenceTest1d()
        {
            float[] f = new float[1];
            f[0] = .3f;
            perlin1d.GetPerlinTurbulence(f, 32.0f);
            Assert.IsTrue(f.Length == 1);
        }

        [Test]
        public void GetTurbulenceTest2d()
        {
            float[] f = new float[2];
            f[0] = .1f;
            f[1] = .2f;
            perlin2d.GetPerlinTurbulence(f, 32.0f);
            Assert.IsTrue(f.Length == 2);
        }

        [Test]
        public void GetTurbulenceTest3d()
        {
            float[] f = new float[3];
            f[0] = .1f;
            f[1] = .2f;
            f[2] = .3f;
            perlin3d.GetPerlinTurbulence(f, 32.0f);
            Assert.IsTrue(f.Length == 3);
        }

        [Test]
        public void GetTurbulenceTest4d()
        {
            float[] f = new float[4];
            f[0] = .1f;
            f[1] = .2f;
            f[2] = .3f;
            f[3] = .6f;
            perlin4d.GetPerlinTurbulence(f, 32.0f);
            Assert.IsTrue(f.Length == 4);
        }


        [Test]
        [ExpectedException(typeof(Exception))]
        public void GetDimMismatchPerlin()
        {
            float[] d = new float[2];
            perlin1d.GetPerlinNoise(d);
        }

        [Test]
        public void ConstructorTest()
        {
            using (TCODRandom rand = new TCODRandom())
            {
                using (TCODNoise a = new TCODNoise(1, rand))
                {
                    using (TCODNoise b = new TCODNoise(2, .5, 2.0))
                    {
                        using (TCODNoise c = new TCODNoise(3, .2, .3, rand))
                        {

                        }
                    }
                }
            }
        }

    }
}
