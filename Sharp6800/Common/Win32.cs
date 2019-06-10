using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sharp6800.Common
{
    public static class Win32
    {
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_MOUSEWHEEL = 0x20a;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int KEY_PRESSED = 0x80;
        
        [DllImport("user32.dll")]
        public  static extern IntPtr WindowFromPoint(Point pt);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        
        [DllImport("USER32.dll")]
        public static extern short GetKeyState(Keys nVirtKey);
        
        
        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    //
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control FindControlAtPoint(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(Cursor.Position));
            return null;
        }

    }
}