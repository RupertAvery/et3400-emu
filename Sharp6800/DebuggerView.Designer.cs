namespace Sharp6800.Debugger
{
    partial class DebuggerView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MemoryViewScrollBar = new System.Windows.Forms.VScrollBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DasmViewScrollBar = new System.Windows.Forms.VScrollBar();
            this.PCTextBox = new System.Windows.Forms.TextBox();
            this.ACCATextBox = new System.Windows.Forms.TextBox();
            this.ACCBTextBox = new System.Windows.Forms.TextBox();
            this.SPTextBox = new System.Windows.Forms.TextBox();
            this.IXTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CCTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.disassemblerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.DebuggerToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.MemToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.DasmToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BreakpointsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BreakpointsToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.RangesListView = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RangesToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripSeparator();
            this.AddBreakpointButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveBreakpointButton = new System.Windows.Forms.ToolStripButton();
            this.ClearAllBreakpointsButton = new System.Windows.Forms.ToolStripButton();
            this.GotoBreakpointButton = new System.Windows.Forms.ToolStripButton();
            this.AddRangeButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveRangeButton = new System.Windows.Forms.ToolStripButton();
            this.ClearAllRangesButton = new System.Windows.Forms.ToolStripButton();
            this.GotoRangeButton = new System.Windows.Forms.ToolStripButton();
            this.StartButton = new System.Windows.Forms.ToolStripButton();
            this.StopButton = new System.Windows.Forms.ToolStripButton();
            this.StepButton = new System.Windows.Forms.ToolStripButton();
            this.ResetButton = new System.Windows.Forms.ToolStripButton();
            this.DasmViewPictureBox = new System.Windows.Forms.PictureBox();
            this.MemoryViewPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.disassemblerContextMenuStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.DebuggerToolStrip.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.BreakpointsToolStrip.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.RangesToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DasmViewPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MemoryViewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MemoryViewScrollBar);
            this.groupBox1.Controls.Add(this.MemoryViewPictureBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 423);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Memory";
            // 
            // MemoryViewScrollBar
            // 
            this.MemoryViewScrollBar.Location = new System.Drawing.Point(328, 19);
            this.MemoryViewScrollBar.Maximum = 1000;
            this.MemoryViewScrollBar.Name = "MemoryViewScrollBar";
            this.MemoryViewScrollBar.Size = new System.Drawing.Size(18, 394);
            this.MemoryViewScrollBar.TabIndex = 5;
            this.MemoryViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.MemoryViewScrollBar_Scroll);
            this.MemoryViewScrollBar.ValueChanged += new System.EventHandler(this.MemoryViewScrollBar_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DasmViewScrollBar);
            this.groupBox2.Controls.Add(this.DasmViewPictureBox);
            this.groupBox2.Location = new System.Drawing.Point(380, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 423);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Disassembly";
            // 
            // DasmViewScrollBar
            // 
            this.DasmViewScrollBar.Location = new System.Drawing.Point(328, 19);
            this.DasmViewScrollBar.Maximum = 1000;
            this.DasmViewScrollBar.Name = "DasmViewScrollBar";
            this.DasmViewScrollBar.Size = new System.Drawing.Size(18, 394);
            this.DasmViewScrollBar.TabIndex = 8;
            this.DasmViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DasmViewScrollBar_Scroll);
            this.DasmViewScrollBar.ValueChanged += new System.EventHandler(this.DasmViewScrollBar_ValueChanged);
            // 
            // PCTextBox
            // 
            this.PCTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PCTextBox.Location = new System.Drawing.Point(58, 26);
            this.PCTextBox.Name = "PCTextBox";
            this.PCTextBox.Size = new System.Drawing.Size(100, 22);
            this.PCTextBox.TabIndex = 2;
            this.PCTextBox.Text = "0000";
            this.PCTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ACCATextBox
            // 
            this.ACCATextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ACCATextBox.Location = new System.Drawing.Point(58, 110);
            this.ACCATextBox.Name = "ACCATextBox";
            this.ACCATextBox.Size = new System.Drawing.Size(100, 22);
            this.ACCATextBox.TabIndex = 8;
            this.ACCATextBox.Text = "00";
            this.ACCATextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ACCBTextBox
            // 
            this.ACCBTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ACCBTextBox.Location = new System.Drawing.Point(58, 138);
            this.ACCBTextBox.Name = "ACCBTextBox";
            this.ACCBTextBox.Size = new System.Drawing.Size(100, 22);
            this.ACCBTextBox.TabIndex = 10;
            this.ACCBTextBox.Text = "00";
            this.ACCBTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SPTextBox
            // 
            this.SPTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SPTextBox.Location = new System.Drawing.Point(58, 54);
            this.SPTextBox.Name = "SPTextBox";
            this.SPTextBox.Size = new System.Drawing.Size(100, 22);
            this.SPTextBox.TabIndex = 4;
            this.SPTextBox.Text = "0000";
            this.SPTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IXTextBox
            // 
            this.IXTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IXTextBox.Location = new System.Drawing.Point(58, 82);
            this.IXTextBox.Name = "IXTextBox";
            this.IXTextBox.Size = new System.Drawing.Size(100, 22);
            this.IXTextBox.TabIndex = 6;
            this.IXTextBox.Text = "0000";
            this.IXTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "PC";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "SP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "IX";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "ACCA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "ACCB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "CC";
            // 
            // CCTextBox
            // 
            this.CCTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CCTextBox.Location = new System.Drawing.Point(55, 180);
            this.CCTextBox.Name = "CCTextBox";
            this.CCTextBox.Size = new System.Drawing.Size(100, 22);
            this.CCTextBox.TabIndex = 13;
            this.CCTextBox.Text = "00000000";
            this.CCTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.label7.Location = new System.Drawing.Point(84, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 16);
            this.label7.TabIndex = 11;
            this.label7.Text = "--HINZVC";
            // 
            // disassemblerContextMenuStrip
            // 
            this.disassemblerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRangeToolStripMenuItem,
            this.removeRangeToolStripMenuItem,
            this.addCommentToolStripMenuItem,
            this.removeCommentToolStripMenuItem});
            this.disassemblerContextMenuStrip.Name = "disassemblerContextMenuStrip";
            this.disassemblerContextMenuStrip.Size = new System.Drawing.Size(181, 92);
            // 
            // addRangeToolStripMenuItem
            // 
            this.addRangeToolStripMenuItem.Name = "addRangeToolStripMenuItem";
            this.addRangeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addRangeToolStripMenuItem.Text = "Add Data Range";
            this.addRangeToolStripMenuItem.Click += new System.EventHandler(this.AddRangeToolStripMenuItem_Click);
            // 
            // removeRangeToolStripMenuItem
            // 
            this.removeRangeToolStripMenuItem.Name = "removeRangeToolStripMenuItem";
            this.removeRangeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeRangeToolStripMenuItem.Text = "Remove Data Range";
            this.removeRangeToolStripMenuItem.Click += new System.EventHandler(this.RemoveRangeToolStripMenuItem_Click);
            // 
            // addCommentToolStripMenuItem
            // 
            this.addCommentToolStripMenuItem.Name = "addCommentToolStripMenuItem";
            this.addCommentToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addCommentToolStripMenuItem.Text = "Add Comment";
            this.addCommentToolStripMenuItem.Click += new System.EventHandler(this.addCommentToolStripMenuItem_Click);
            // 
            // removeCommentToolStripMenuItem
            // 
            this.removeCommentToolStripMenuItem.Name = "removeCommentToolStripMenuItem";
            this.removeCommentToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeCommentToolStripMenuItem.Text = "Remove Comment";
            this.removeCommentToolStripMenuItem.Click += new System.EventHandler(this.removeCommentToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.CCTextBox);
            this.groupBox3.Controls.Add(this.PCTextBox);
            this.groupBox3.Controls.Add(this.ACCATextBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.ACCBTextBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.SPTextBox);
            this.groupBox3.Controls.Add(this.IXTextBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(746, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(177, 423);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(930, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // DebuggerToolStrip
            // 
            this.DebuggerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartButton,
            this.StopButton,
            this.toolStripSeparator3,
            this.StepButton,
            this.ResetButton,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.MemToolStripComboBox,
            this.toolStripButton1,
            this.toolStripLabel2,
            this.DasmToolStripComboBox,
            this.toolStripButton2});
            this.DebuggerToolStrip.Location = new System.Drawing.Point(0, 0);
            this.DebuggerToolStrip.Name = "DebuggerToolStrip";
            this.DebuggerToolStrip.Size = new System.Drawing.Size(930, 25);
            this.DebuggerToolStrip.TabIndex = 1;
            this.DebuggerToolStrip.Text = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(52, 22);
            this.toolStripLabel1.Text = "Memory";
            // 
            // MemToolStripComboBox
            // 
            this.MemToolStripComboBox.Name = "MemToolStripComboBox";
            this.MemToolStripComboBox.Size = new System.Drawing.Size(75, 25);
            this.MemToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.MemToolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(72, 22);
            this.toolStripLabel2.Text = "Disassembly";
            // 
            // DasmToolStripComboBox
            // 
            this.DasmToolStripComboBox.Name = "DasmToolStripComboBox";
            this.DasmToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.DasmToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.DasmToolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(6, 25);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 456);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(918, 180);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.BreakpointsListView);
            this.tabPage1.Controls.Add(this.BreakpointsToolStrip);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(910, 154);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Breakpoints";
            // 
            // BreakpointsListView
            // 
            this.BreakpointsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.BreakpointsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BreakpointsListView.FullRowSelect = true;
            this.BreakpointsListView.GridLines = true;
            this.BreakpointsListView.Location = new System.Drawing.Point(3, 28);
            this.BreakpointsListView.Name = "BreakpointsListView";
            this.BreakpointsListView.Size = new System.Drawing.Size(904, 123);
            this.BreakpointsListView.TabIndex = 1;
            this.BreakpointsListView.UseCompatibleStateImageBehavior = false;
            this.BreakpointsListView.View = System.Windows.Forms.View.Details;
            this.BreakpointsListView.DoubleClick += new System.EventHandler(this.BreakpointsListView_DoubleClick);
            this.BreakpointsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BreakpointsListView_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Address";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Enabled";
            // 
            // BreakpointsToolStrip
            // 
            this.BreakpointsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBreakpointButton,
            this.RemoveBreakpointButton,
            this.ClearAllBreakpointsButton,
            this.toolStripSeparator2,
            this.GotoBreakpointButton});
            this.BreakpointsToolStrip.Location = new System.Drawing.Point(3, 3);
            this.BreakpointsToolStrip.Name = "BreakpointsToolStrip";
            this.BreakpointsToolStrip.Size = new System.Drawing.Size(904, 25);
            this.BreakpointsToolStrip.TabIndex = 0;
            this.BreakpointsToolStrip.Text = "toolStrip2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.RangesListView);
            this.tabPage2.Controls.Add(this.RangesToolStrip);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(914, 133);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Memory Maps";
            // 
            // RangesListView
            // 
            this.RangesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader5,
            this.columnHeader3,
            this.columnHeader4});
            this.RangesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RangesListView.FullRowSelect = true;
            this.RangesListView.GridLines = true;
            this.RangesListView.Location = new System.Drawing.Point(3, 28);
            this.RangesListView.Name = "RangesListView";
            this.RangesListView.Size = new System.Drawing.Size(908, 102);
            this.RangesListView.TabIndex = 1;
            this.RangesListView.UseCompatibleStateImageBehavior = false;
            this.RangesListView.View = System.Windows.Forms.View.Details;
            this.RangesListView.SelectedIndexChanged += new System.EventHandler(this.RangesListView_SelectedIndexChanged);
            this.RangesListView.DoubleClick += new System.EventHandler(this.RangesListView_DoubleClick);
            this.RangesListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RangesListView_KeyDown);
            // 
            // columnHeader6
            // 
            this.columnHeader6.DisplayIndex = 3;
            this.columnHeader6.Text = "Description";
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 2;
            this.columnHeader5.Text = "Type";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 0;
            this.columnHeader3.Text = "Start";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 1;
            this.columnHeader4.Text = "End";
            this.columnHeader4.Width = 100;
            // 
            // RangesToolStrip
            // 
            this.RangesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddRangeButton,
            this.RemoveRangeButton,
            this.ClearAllRangesButton,
            this.toolStripButton12,
            this.GotoRangeButton});
            this.RangesToolStrip.Location = new System.Drawing.Point(3, 3);
            this.RangesToolStrip.Name = "RangesToolStrip";
            this.RangesToolStrip.Size = new System.Drawing.Size(908, 25);
            this.RangesToolStrip.TabIndex = 0;
            this.RangesToolStrip.Text = "toolStrip3";
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(6, 25);
            // 
            // AddBreakpointButton
            // 
            this.AddBreakpointButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddBreakpointButton.Image = global::Sharp6800.Properties.Resources.Add_16x;
            this.AddBreakpointButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddBreakpointButton.Name = "AddBreakpointButton";
            this.AddBreakpointButton.Size = new System.Drawing.Size(23, 22);
            this.AddBreakpointButton.Text = "Add breakpoint";
            this.AddBreakpointButton.Click += new System.EventHandler(this.AddBreakpointButton_Click);
            // 
            // RemoveBreakpointButton
            // 
            this.RemoveBreakpointButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveBreakpointButton.Image = global::Sharp6800.Properties.Resources.Remove_16x;
            this.RemoveBreakpointButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveBreakpointButton.Name = "RemoveBreakpointButton";
            this.RemoveBreakpointButton.Size = new System.Drawing.Size(23, 22);
            this.RemoveBreakpointButton.Text = "Remove breakpoint";
            this.RemoveBreakpointButton.Click += new System.EventHandler(this.RemoveBreakpointButton_Click);
            // 
            // ClearAllBreakpointsButton
            // 
            this.ClearAllBreakpointsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ClearAllBreakpointsButton.Image = global::Sharp6800.Properties.Resources.ClearCollection_16x;
            this.ClearAllBreakpointsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearAllBreakpointsButton.Name = "ClearAllBreakpointsButton";
            this.ClearAllBreakpointsButton.Size = new System.Drawing.Size(23, 22);
            this.ClearAllBreakpointsButton.Text = "Clear all breakpoints";
            this.ClearAllBreakpointsButton.Click += new System.EventHandler(this.ClearAllBreakpointsButton_Click);
            // 
            // GotoBreakpointButton
            // 
            this.GotoBreakpointButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GotoBreakpointButton.Image = global::Sharp6800.Properties.Resources.GoToSourceCode_16x;
            this.GotoBreakpointButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GotoBreakpointButton.Name = "GotoBreakpointButton";
            this.GotoBreakpointButton.Size = new System.Drawing.Size(23, 22);
            this.GotoBreakpointButton.Text = "Go to breakpoint";
            this.GotoBreakpointButton.Click += new System.EventHandler(this.GotoBreakpointButton_Click);
            // 
            // AddRangeButton
            // 
            this.AddRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddRangeButton.Image = global::Sharp6800.Properties.Resources.Add_16x;
            this.AddRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddRangeButton.Name = "AddRangeButton";
            this.AddRangeButton.Size = new System.Drawing.Size(23, 22);
            this.AddRangeButton.Text = "Add range";
            this.AddRangeButton.ToolTipText = "Add range";
            this.AddRangeButton.Click += new System.EventHandler(this.AddRangeButton_Click);
            // 
            // RemoveRangeButton
            // 
            this.RemoveRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveRangeButton.Image = global::Sharp6800.Properties.Resources.Remove_16x;
            this.RemoveRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveRangeButton.Name = "RemoveRangeButton";
            this.RemoveRangeButton.Size = new System.Drawing.Size(23, 22);
            this.RemoveRangeButton.Text = "Remove range";
            this.RemoveRangeButton.ToolTipText = "Remove range";
            this.RemoveRangeButton.Click += new System.EventHandler(this.RemoveRangeButton_Click);
            // 
            // ClearAllRangesButton
            // 
            this.ClearAllRangesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ClearAllRangesButton.Image = global::Sharp6800.Properties.Resources.ClearCollection_16x;
            this.ClearAllRangesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearAllRangesButton.Name = "ClearAllRangesButton";
            this.ClearAllRangesButton.Size = new System.Drawing.Size(23, 22);
            this.ClearAllRangesButton.Text = "Clear all ranges";
            this.ClearAllRangesButton.ToolTipText = "Clear all ranges";
            this.ClearAllRangesButton.Click += new System.EventHandler(this.ClearAllRangesButton_Click);
            // 
            // GotoRangeButton
            // 
            this.GotoRangeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GotoRangeButton.Image = global::Sharp6800.Properties.Resources.GoToSourceCode_16x;
            this.GotoRangeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GotoRangeButton.Name = "GotoRangeButton";
            this.GotoRangeButton.Size = new System.Drawing.Size(23, 22);
            this.GotoRangeButton.Text = "Go to range";
            this.GotoRangeButton.ToolTipText = "Go to range";
            this.GotoRangeButton.Click += new System.EventHandler(this.GotoRangeButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StartButton.Image = global::Sharp6800.Properties.Resources.Run_16x;
            this.StartButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(23, 22);
            this.StartButton.Text = "Start";
            this.StartButton.ToolTipText = "Start (F5)";
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopButton.Image = global::Sharp6800.Properties.Resources.Stop_16x;
            this.StopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(23, 22);
            this.StopButton.Text = "Stop";
            this.StopButton.ToolTipText = "Stop (F4)";
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StepButton
            // 
            this.StepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepButton.Image = global::Sharp6800.Properties.Resources.StepIntoArrow_16x;
            this.StepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(23, 22);
            this.StepButton.Text = "Step";
            this.StepButton.ToolTipText = "Step (F10)";
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ResetButton.Image = global::Sharp6800.Properties.Resources.Restart_16x;
            this.ResetButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(23, 22);
            this.ResetButton.Text = "Reset";
            this.ResetButton.ToolTipText = "Reset (ESC)";
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // DasmViewPictureBox
            // 
            this.DasmViewPictureBox.BackColor = System.Drawing.Color.White;
            this.DasmViewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DasmViewPictureBox.Location = new System.Drawing.Point(6, 19);
            this.DasmViewPictureBox.Name = "DasmViewPictureBox";
            this.DasmViewPictureBox.Size = new System.Drawing.Size(319, 394);
            this.DasmViewPictureBox.TabIndex = 7;
            this.DasmViewPictureBox.TabStop = false;
            this.DasmViewPictureBox.Click += new System.EventHandler(this.DasmViewPictureBox_Click);
            this.DasmViewPictureBox.DoubleClick += new System.EventHandler(this.DasmViewPictureBox_DoubleClick);
            this.DasmViewPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DasmViewPictureBox_MouseClick);
            // 
            // MemoryViewPictureBox
            // 
            this.MemoryViewPictureBox.BackColor = System.Drawing.Color.White;
            this.MemoryViewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MemoryViewPictureBox.Location = new System.Drawing.Point(6, 19);
            this.MemoryViewPictureBox.Name = "MemoryViewPictureBox";
            this.MemoryViewPictureBox.Size = new System.Drawing.Size(319, 394);
            this.MemoryViewPictureBox.TabIndex = 4;
            this.MemoryViewPictureBox.TabStop = false;
            // 
            // DebuggerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 661);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.DebuggerToolStrip);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(946, 766);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(946, 700);
            this.Name = "DebuggerView";
            this.ShowIcon = false;
            this.Text = "Debugger";
            this.Activated += new System.EventHandler(this.DebuggerView_Activated);
            this.Deactivate += new System.EventHandler(this.DebuggerView_Deactivate);
            this.Load += new System.EventHandler(this.DebuggerView_Load);
            this.Enter += new System.EventHandler(this.DebuggerView_Enter);
            this.Leave += new System.EventHandler(this.DebuggerView_Leave);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.disassemblerContextMenuStrip.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.DebuggerToolStrip.ResumeLayout(false);
            this.DebuggerToolStrip.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.BreakpointsToolStrip.ResumeLayout(false);
            this.BreakpointsToolStrip.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.RangesToolStrip.ResumeLayout(false);
            this.RangesToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DasmViewPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MemoryViewPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.VScrollBar MemoryViewScrollBar;
        private System.Windows.Forms.PictureBox MemoryViewPictureBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.VScrollBar DasmViewScrollBar;
        private System.Windows.Forms.PictureBox DasmViewPictureBox;
        //private System.Windows.Forms.Button DummyButton;
        private System.Windows.Forms.TextBox PCTextBox;
        private System.Windows.Forms.TextBox ACCATextBox;
        private System.Windows.Forms.TextBox ACCBTextBox;
        private System.Windows.Forms.TextBox SPTextBox;
        private System.Windows.Forms.TextBox IXTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox CCTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ContextMenuStrip disassemblerContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeRangeToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip DebuggerToolStrip;
        private System.Windows.Forms.ToolStripButton StartButton;
        private System.Windows.Forms.ToolStripButton StopButton;
        private System.Windows.Forms.ToolStripButton StepButton;
        private System.Windows.Forms.ToolStripButton ResetButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView BreakpointsListView;
        private System.Windows.Forms.ToolStrip BreakpointsToolStrip;
        private System.Windows.Forms.ToolStripButton AddBreakpointButton;
        private System.Windows.Forms.ToolStripButton RemoveBreakpointButton;
        private System.Windows.Forms.ToolStripButton ClearAllBreakpointsButton;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView RangesListView;
        private System.Windows.Forms.ToolStrip RangesToolStrip;
        private System.Windows.Forms.ToolStripButton AddRangeButton;
        private System.Windows.Forms.ToolStripButton RemoveRangeButton;
        private System.Windows.Forms.ToolStripButton ClearAllRangesButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton GotoBreakpointButton;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripSeparator toolStripButton12;
        private System.Windows.Forms.ToolStripButton GotoRangeButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox MemToolStripComboBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox DasmToolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem addCommentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeCommentToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}