using System.Diagnostics;
using System.Windows.Forms;

namespace ET3400.Common
{
    public static class VScrollBarExtensions
    {
        public static void SetDeltaValue(this VScrollBar scrollbar, int delta)
        {
            Debug.WriteLine(delta);
            var value = scrollbar.Value ;
            var adjustedDelta = (int)(delta / 8);
            if (value + adjustedDelta < 0)
            {
                value = 0;
            }
            else if (value + adjustedDelta > scrollbar.Maximum)
            {
                value = scrollbar.Maximum;
            }else
            {
                value += adjustedDelta;
            }
            scrollbar.Value = value;
        }
    }
}