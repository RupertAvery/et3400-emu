using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharp6800.Debugger
{

    partial class DebuggerView : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            //Control control = Control.FromChildHandle(m.HWnd);

            //if (control != MemoryViewPictureBox || control != DasmViewPictureBox)
            //{
            //    return false;
            //}

            switch (m.Msg)
            {
                case WM_SYSKEYDOWN:
                    {
                        // Extract the keys being pressed
                        Keys keys = ((Keys)((int)m.WParam.ToInt64()));
                        switch (keys)
                        {
                            case Keys.F10:
                                if (!_trainer.IsRunning)
                                {
                                    _trainer.Step();
                                }
                                return true; // Prevent message reaching destination
                        }
                    }
                    break;
                //case WM_MOUSEWHEEL:
                //    {
                //        // WM_MOUSEWHEEL, find the control at screen position m.LParam
                //        Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                //        IntPtr hWnd = WindowFromPoint(pos);
                //        SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                //        return true;
                //        //if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                //        //{
                //        //}
                //    }
                //break;
                case WM_LBUTTONDOWN:
                    {
                        var lParam = ((int)m.LParam.ToInt64());

                        var pos = new Point()
                        {
                            X = lParam & 0xFFFF,
                            Y = (lParam >> 0x10) & 0xFFFF,
                        };

                        focusObject = FindControlAtPoint(this);

                        //WindowFromPoint(focusObject.PointToScreen(pos)) == this.Handle;

                        //                        if (focusObject == MemoryViewPictureBox || focusObject == DasmViewPictureBox)
                        //                        {
                        ////                            return true;
                        //                        }

                    }

                    break;
                case WM_KEYDOWN:
                    {
                        // Extract the keys being pressed
                        Keys keys = ((Keys)((int)m.WParam.ToInt64()));

                        switch (keys)
                        {
                            case Keys.G:
                                if ((keys & Keys.ControlKey) != Keys.ControlKey)
                                {
                                    //Application.RemoveMessageFilter(this);
                                    var gotoForm = new Goto();
                                    var result = gotoForm.ShowDialog(this);
                                    if (result == DialogResult.OK)
                                    {
                                        EnsureVisible(gotoForm.Address);
                                        focusObject = DasmViewPictureBox;
                                    }
                                    //Application.AddMessageFilter(this);
                                    return true;
                                }

                                break;
                            case Keys.F4:
                                if (_trainer.IsRunning)
                                {
                                    _trainer.Stop();
                                }

                                return true;

                            case Keys.F5:
                                if (!_trainer.IsRunning)
                                {
                                    _trainer.Start();
                                }

                                return true;

                            case Keys.F9:
                                AddBreakpoint();
                                return true;
                        }

                        if (focusObject == MemoryViewPictureBox)
                        {
                            return MemoryViewScrollBar_HandleKeyDown(keys);
                        }
                        else if (focusObject == DasmViewPictureBox)
                        {
                            return DasmViewPictureBox_HandleKeyDown(keys);
                        }

                        return false;
                    }
            }
            return false;
        }


        private bool MemoryViewScrollBar_HandleKeyDown(Keys keys)
        {
            var oldValue = MemoryViewScrollBar.Value;

            switch (keys)
            {
                case Keys.Down:
                    if (oldValue + 1 > MemoryViewScrollBar.Maximum)
                    {
                        MemoryViewScrollBar.Value = MemoryViewScrollBar.Maximum;
                    }
                    else
                    {
                        MemoryViewScrollBar.Value = oldValue + 1;
                    }
                    break;
                case Keys.Up:
                    if (oldValue - 1 < 0) return true;
                    MemoryViewScrollBar.Value = oldValue - 1;
                    break;
                case Keys.PageDown:
                    if (oldValue + 16 > MemoryViewScrollBar.Maximum)
                    {
                        MemoryViewScrollBar.Value = MemoryViewScrollBar.Maximum;
                    }
                    else
                    {
                        MemoryViewScrollBar.Value = oldValue + 16;
                    }
                    break;
                case Keys.PageUp:
                    if (oldValue - 16 < 0)
                    {
                        MemoryViewScrollBar.Value = 0;
                    }
                    else
                    {
                        MemoryViewScrollBar.Value = oldValue - 16;
                    }
                    break;
            }

            return false;
        }

        private bool DasmViewPictureBox_HandleKeyDown(Keys keys)
        {
            switch (keys)
            {

                case Keys.Up:
                    _disassemberDisplay.MoveUp();
                    break;

                case Keys.Down:
                    _disassemberDisplay.MoveDown();
                    break;

                case Keys.PageUp:
                    _disassemberDisplay.PageUp();
                    break;

                case Keys.PageDown:
                    _disassemberDisplay.PageDown();

                    break;
            }

            return true;
        }

    }

}
