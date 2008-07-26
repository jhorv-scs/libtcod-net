using System;
using System.IO;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODPathFindingTest
    {
        static char[,] room =
            {
                {'#', '#', '#', '#', '#'},
                {'#', '.', '#', '.', '#'},
                {'#', '.', '#', '.', '#'},
                {'#', '.', '.', '.', '#'},
                {'#', '#', '#', '#', '#'}
            };

        public struct point
        {
            public point(int x, int y)
            {
                m_x = x;
                m_y = y;
            }
            public int m_x;
            public int m_y;
        }
        static point[] walkList = {new point(1, 2), new point(2, 3), new point(3, 3) };

        static bool PathCallback(int x, int y, ref Object userData)
        {
            return (room[x,y] == '.');
        }
        
        private TCODPathFinding pathFindingFOV;
        private TCODFov fov;
        [TestFixtureSetUp]
        public void Init()
        {
            fov = new TCODFov(5, 5);

            for (int i = 0; i < 5; ++i)    //width
                for (int j = 0; j < 5; ++j) //height
                    fov.SetCell(i, j, room[j, i] == '.', room[j, i] == '.');

            pathFindingFOV = new TCODPathFinding(fov);
        }

        [TestFixtureTearDown]
        public void Cleaup()
        {
            pathFindingFOV.Dispose();
            fov.Dispose();
        }

        [Test]
        public void ComputeSucessful()
        {
            Assert.IsTrue(pathFindingFOV.ComputePath(1, 1, 3, 3));
        }

        [Test]
        public void ComputImpossiblePath()
        {
            Assert.IsFalse(pathFindingFOV.ComputePath(1, 1, 4, 4));
        }

        [Test]
        public void WalkExpectedPath()
        {
            ComputeSucessful();
            int x = 1;
            int y = 1;
            int i = 0;
            while (pathFindingFOV.WalkPath(ref x, ref y, true))
            {
                Assert.IsTrue(x == walkList[i].m_x);
                Assert.IsTrue(y == walkList[i].m_y);
                i++;
            }
        }

        [Test]
        public void CheckIndividualPoints()
        {
            int index = 1;
            int x;
            int y;
            ComputeSucessful();
            pathFindingFOV.GetPointOnPath(index, out x, out y);
            Assert.IsTrue(x == 2 && y == 3);
        }

        [Test]
        public void CheckPathEmpty()
        {
            ComputImpossiblePath();
            Assert.IsTrue(pathFindingFOV.IsPathEmpty());
            ComputeSucessful();
            Assert.IsFalse(pathFindingFOV.IsPathEmpty());
        }

        [Test]
        public void CheckPathSize()
        {
            ComputImpossiblePath();
            Assert.IsTrue(pathFindingFOV.GetPathSize() == 0);
            ComputeSucessful();
            Assert.IsTrue(pathFindingFOV.GetPathSize() == 3);
        }
    }
}