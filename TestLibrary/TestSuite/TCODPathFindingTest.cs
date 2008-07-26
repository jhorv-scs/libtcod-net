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
        
        private TCODPathFinding pathFindingFOV;
        private TCODPathFinding pathFindingCallback;
        private TCODFov fov;

        public bool TCODPathCallback(int x, int y)
        {
            return (room[y, x] == '.');
        }

        int hugeMapSize = 150;
        public bool TCODPathCallbackHugeMap(int x, int y)
        {
            if (x == 0 || y == 0 || x == (hugeMapSize - 1) || y == (hugeMapSize - 1))
                return false;
            return true;
        }

        [TestFixtureSetUp]
        public void Init()
        {
            fov = new TCODFov(5, 5);

            for (int i = 0; i < 5; ++i)    //width
                for (int j = 0; j < 5; ++j) //height
                    fov.SetCell(i, j, room[j, i] == '.', room[j, i] == '.');

            pathFindingFOV = new TCODPathFinding(fov);

            pathFindingCallback = new TCODPathFinding(5, 5, new TCODPathFinding.TCODPathCallback(TCODPathCallback));
        }

        [TestFixtureTearDown]
        public void Cleaup()
        {
            pathFindingFOV.Dispose();
            pathFindingCallback.Dispose();
            fov.Dispose();
        }

        [Test]
        public void TestHugeMap()
        {
            TCODPathFinding p = new TCODPathFinding(hugeMapSize, hugeMapSize, new TCODPathFinding.TCODPathCallback(TCODPathCallbackHugeMap));
            p.ComputePath(1, 1, hugeMapSize - 3, hugeMapSize - 3);
        }

        [Test]
        public void ComputeSucessful()
        {
            Assert.IsTrue(pathFindingFOV.ComputePath(1, 1, 3, 3));
            Assert.IsTrue(pathFindingCallback.ComputePath(1,1,3,3));
        }

        [Test]
        public void ComputImpossiblePath()
        {
            Assert.IsFalse(pathFindingFOV.ComputePath(1, 1, 4, 4));
            Assert.IsFalse(pathFindingCallback.ComputePath(1, 1, 4, 4));
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
            x = 1;
            y = 1;
            i = 0;
            while (pathFindingCallback.WalkPath(ref x, ref y, true))
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

            pathFindingCallback.GetPointOnPath(index, out x, out y);
            Assert.IsTrue(x == 2 && y == 3);
        }

        [Test]
        public void CheckPathEmpty()
        {
            ComputImpossiblePath();
            Assert.IsTrue(pathFindingFOV.IsPathEmpty());
            Assert.IsTrue(pathFindingCallback.IsPathEmpty());

            ComputeSucessful();
            Assert.IsFalse(pathFindingFOV.IsPathEmpty());
            Assert.IsFalse(pathFindingCallback.IsPathEmpty());
        }

        [Test]
        public void CheckPathSize()
        {
            ComputImpossiblePath();
            Assert.IsTrue(pathFindingFOV.GetPathSize() == 0);
            Assert.IsTrue(pathFindingCallback.GetPathSize() == 0);
            ComputeSucessful();
            Assert.IsTrue(pathFindingFOV.GetPathSize() == 3);
            Assert.IsTrue(pathFindingCallback.GetPathSize() == 3);
        }
    }
}