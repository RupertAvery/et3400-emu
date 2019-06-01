using System.Collections;
using System.Windows.Forms;

namespace Sharp6800.Debugger
{
    public class MemoryMapComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((MemoryMap)((ListViewItem)x).Tag).Start.CompareTo(((MemoryMap)((ListViewItem)y).Tag).Start);
        }
    }
}