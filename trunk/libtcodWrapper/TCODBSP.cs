using System;
using System.Runtime.InteropServices;

namespace libtcodWrapper
{
    #pragma warning disable 1591  //Disable warning about lack of xml comments

    public delegate void TCODBSPTraversalDelegate(TCODBSP bsp);

    [StructLayout(LayoutKind.Sequential)]
    public struct  TCODBSP
    {
        public IntPtr ptr1;
        public IntPtr ptr2;
        public IntPtr ptr3;
        public int x;
        public int y;
        public int w;
        public int h;
        public int position;
        public bool horizontal;
        public byte level;

        public TCODBSP(int x, int y, int w, int h)
        {
            ptr1 = IntPtr.Zero;
            ptr2 = IntPtr.Zero;
            ptr3 = IntPtr.Zero;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            position = 0;
            horizontal = false;
            level = 0;
        }
    }

    public class TCODBSPHandler
    {
        private TCODBSPTraversalDelegate m_delegate;
        private TCODBSPTraversalDelegatePrivate m_privateDelegate;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TCODBSPTraversalDelegatePrivate(TCODBSP bsp, IntPtr nullPtr);

        public TCODBSPHandler()
        {
            m_delegate = null;
            m_privateDelegate = new TCODBSPTraversalDelegatePrivate(this.TCODBSPTraversalDel);
        }

        void TCODBSPTraversalDel(TCODBSP bsp, IntPtr nullPtr)
        {
            if (m_delegate != null)
                m_delegate(bsp);
        }

        public TCODBSP GetLeft(TCODBSP bsp)
        {
            return (TCODBSP)Marshal.PtrToStructure(TCOD_bsp_left(ref bsp), typeof(TCODBSP));
        }

        public TCODBSP GetRight(TCODBSP bsp)
        {
            return (TCODBSP)Marshal.PtrToStructure(TCOD_bsp_right(ref bsp), typeof(TCODBSP));
        }

        public bool IsLeaf(TCODBSP bsp)
        {
            return TCOD_bsp_is_a_leaf(ref bsp);
        }

        public void Resize(ref TCODBSP bsp, int x, int y, int w, int h)
        {
            TCOD_bsp_resize(ref bsp, x, y, w, h);
        }

        public void RemoveSons(ref TCODBSP bsp)
        {
            TCOD_bsp_remove_sons(ref bsp);
        }

        public void SplitOnce(ref TCODBSP bsp, bool horizontal, int position)
        {
            TCOD_bsp_split_once(ref bsp, horizontal, position);
        }

        public void SplitRecursive(ref TCODBSP bsp, TCODRandom randomizer, int nb, int minHSize, int maxHRatio, int minVSize, int maxVRatio)
        {
            TCOD_bsp_split_recursive(ref bsp, randomizer.m_instance, nb, minHSize, maxHRatio, minVSize, maxVRatio);
        }

        public void TraversePreOrder(TCODBSP bsp, TCODBSPTraversalDelegate listner)
        {
            m_delegate = listner;
            TCOD_bsp_traverse_pre_order(ref bsp, m_privateDelegate, IntPtr.Zero);
        }

        public void TraverseInOrder(TCODBSP bsp, TCODBSPTraversalDelegate listner)
        {
            m_delegate = listner;
            TCOD_bsp_traverse_in_order(ref bsp, m_privateDelegate, IntPtr.Zero);
        }

        public void TraversePostOrder(TCODBSP bsp, TCODBSPTraversalDelegate listner)
        {
            m_delegate = listner;
            TCOD_bsp_traverse_in_order(ref bsp, m_privateDelegate, IntPtr.Zero);
        }

        public void TraverseLevelOrder(TCODBSP bsp, TCODBSPTraversalDelegate listner)
        {
            m_delegate = listner;
            TCOD_bsp_traverse_level_order(ref bsp, m_privateDelegate, IntPtr.Zero);
        }

        public void TraverseInvertedOrder(TCODBSP bsp, TCODBSPTraversalDelegate listner)
        {
            m_delegate = listner;
            TCOD_bsp_traverse_level_inverted_order(ref bsp, m_privateDelegate, IntPtr.Zero);
        }


        #region DllImport
        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_bsp_left(ref TCODBSP node);

        [DllImport(DLLName.name)]
        private extern static IntPtr TCOD_bsp_right(ref TCODBSP node);

        [DllImport(DLLName.name)]
        private extern static bool TCOD_bsp_is_a_leaf(ref TCODBSP node);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_resize(ref TCODBSP node, int x, int y, int w, int h);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_remove_sons(ref TCODBSP node);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_split_once(ref TCODBSP node, bool horizontal, int position);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_split_recursive(ref TCODBSP node, IntPtr randomizer, int nb, int minHSize, int maxHRatio, int minVSize, int maxVRatio);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_traverse_pre_order(ref TCODBSP node, TCODBSPTraversalDelegatePrivate listener, IntPtr userData);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_traverse_in_order(ref TCODBSP node, TCODBSPTraversalDelegatePrivate listener, IntPtr userData);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_traverse_post_order(ref TCODBSP node, TCODBSPTraversalDelegatePrivate listener, IntPtr userData);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_traverse_level_order(ref TCODBSP node, TCODBSPTraversalDelegatePrivate listener, IntPtr userData);

        [DllImport(DLLName.name)]
        private extern static void TCOD_bsp_traverse_level_inverted_order(ref TCODBSP node, TCODBSPTraversalDelegatePrivate listener, IntPtr userData);

        #endregion
    }

    #pragma warning restore 1591  //Disable warning about lack of xml comments
}
