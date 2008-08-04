using System;
using NUnit.Framework;
using libtcodWrapper;

namespace libtcodWrapperTests
{
    [TestFixture]
    public class TCODBSPTest
    {
        [Test]
        public void LeftRightTest()
        {
            using (TCODBSP bspSized = new TCODBSP(0, 0, 10, 10))
            {
                bspSized.SplitOnce(false, 6);

                TCODBSP left = bspSized.GetLeft();
                Assert.AreEqual(left.w, 6);
                Assert.AreEqual(left.level, 1);

                TCODBSP right = bspSized.GetRight();
                Assert.AreEqual(right.w, 4);
                Assert.AreEqual(right.level, 1);
            }
        }

        [Test]
        public void FatherTest()
        {
            using (TCODBSP bspSized = new TCODBSP(0, 0, 10, 10))
            {
                bspSized.SplitOnce(false, 6);

                TCODBSP father1 = bspSized.GetLeft().GetFather();
                TCODBSP father2 = bspSized.GetRight().GetFather();
                Assert.IsTrue(bspSized == father1 && father1 == father2 && bspSized == father2);
            }
        }


        [Test]
        public void TestLeaf()
        {
            using (TCODBSP bspSized = new TCODBSP(0, 0, 10, 10))
            {
                bspSized.SplitOnce(false, 6);

                Assert.IsFalse(bspSized.IsLeaf());
                Assert.IsTrue(bspSized.GetLeft().IsLeaf());
                Assert.IsTrue(bspSized.GetRight().IsLeaf());
            }
        }


        [Test]
        public void TestResize()
        {
            using (TCODBSP origSized = new TCODBSP(0, 0, 10, 10))
            {
                using(TCODBSP newSized = new TCODBSP(0, 0, 20, 25))
                {             
                    Assert.IsFalse(origSized == newSized);
                    origSized.Resize(0, 0, 20, 25);
                    Assert.IsTrue(origSized == newSized);
                }
            }
        }

        [Test]
        public void TestRemoveKids()
        {
            using (TCODBSP bspSized = new TCODBSP(0, 0, 10, 10))
            {
                bspSized.SplitOnce(false, 6);
                Assert.IsNotNull(bspSized.GetLeft());
                Assert.IsNotNull(bspSized.GetRight());
                bspSized.RemoveSons();
                Assert.IsNull(bspSized.GetLeft());
                Assert.IsNull(bspSized.GetRight());
            }
        }

        [Test]
        public void FindNodeTest()
        {
            using (TCODBSP bspSized = new TCODBSP(0, 0, 10, 10))
            {
                bspSized.SplitOnce(false, 6);
                TCODBSP nullNode = bspSized.FindNode(10, 9);
                Assert.IsNull(nullNode);
                TCODBSP foundNode = bspSized.FindNode(1, 1);
                Assert.IsNotNull(foundNode);
                Assert.IsTrue(foundNode.w == 6 && foundNode.h == 10);
            }
        }

        [Test]
        public void IsMapCellInsideNodeTest()
        {
            using (TCODBSP bspSized = new TCODBSP(0, 0, 10, 10))
            {
                Assert.IsTrue(bspSized.IsMapCellInsideNode(0, 0));
                Assert.IsFalse(bspSized.IsMapCellInsideNode(10, 10));
                bspSized.SplitOnce(false, 6);
                Assert.IsTrue(bspSized.GetLeft().IsMapCellInsideNode(1, 1));
                Assert.IsFalse(bspSized.GetRight().IsMapCellInsideNode(1, 1));
            }
        }

        int passes;
        
        bool TCODBSPTraversal(TCODBSP bsp)
        {
            passes++;
            return true;
        }
        
        [Test]
        public void TestWalking()
        {
            TCODBSP bspSized = new TCODBSP(0, 0, 10, 10);
            bspSized.SplitOnce(false, 5);
            bspSized.GetLeft().SplitOnce(true, 5);
            bspSized.GetRight().SplitOnce(true, 5);

            passes = 0;
            bspSized.TraversePreOrder(new TCODBSPTraversalDelegate(this.TCODBSPTraversal));
            Assert.AreEqual(7, passes);

            passes = 0;
            bspSized.TraverseInOrder(new TCODBSPTraversalDelegate(this.TCODBSPTraversal));
            Assert.AreEqual(7, passes);

            passes = 0;
            bspSized.TraverseInvertedOrder(new TCODBSPTraversalDelegate(this.TCODBSPTraversal));
            Assert.AreEqual(7, passes);

            passes = 0;
            bspSized.TraverseLevelOrder(new TCODBSPTraversalDelegate(this.TCODBSPTraversal));
            Assert.AreEqual(7, passes);

            passes = 0;
            bspSized.TraversePostOrder(new TCODBSPTraversalDelegate(this.TCODBSPTraversal));
            Assert.AreEqual(7, passes);
        }
    }
}
