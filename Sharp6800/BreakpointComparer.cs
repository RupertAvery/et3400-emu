using System;
using System.Collections;
using System.Windows.Forms;

namespace Sharp6800.Debugger
{
    public class BreakpointComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((int)((ListViewItem)x).Tag).CompareTo((int)((ListViewItem)y).Tag);
        }
    }
}