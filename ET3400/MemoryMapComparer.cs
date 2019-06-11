using System.Collections;
using System.Windows.Forms;
using ET3400.Debugger;

namespace ET3400
{
    public class MemoryMapComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((MemoryMap)((ListViewItem)x).Tag).Start.CompareTo(((MemoryMap)((ListViewItem)y).Tag).Start);
        }
    }
}