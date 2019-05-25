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
            this.DummyButton = new System.Windows.Forms.Button();
            this.MemoryViewComboBox = new System.Windows.Forms.ComboBox();
            this.MemAddrTextBox = new System.Windows.Forms.TextBox();
            this.MemoryViewScrollBar = new System.Windows.Forms.VScrollBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DasmViewComboBox = new System.Windows.Forms.ComboBox();
            this.DasmAddrTextBox = new System.Windows.Forms.TextBox();
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
            this.DasmViewPictureBox = new System.Windows.Forms.PictureBox();
            this.MemoryViewPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.disassemblerContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DasmViewPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MemoryViewPictureBox)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DummyButton);
            this.groupBox1.Controls.Add(this.MemoryViewComboBox);
            this.groupBox1.Controls.Add(this.MemAddrTextBox);
            this.groupBox1.Controls.Add(this.MemoryViewScrollBar);
            this.groupBox1.Controls.Add(this.MemoryViewPictureBox);
            this.groupBox1.Location = new System.Drawing.Point(9, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 423);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Memory";
            // 
            // DummyButton
            // 
            this.DummyButton.Location = new System.Drawing.Point(-100, -100);
            this.DummyButton.Name = "DummyButton";
            this.DummyButton.Size = new System.Drawing.Size(75, 23);
            this.DummyButton.TabIndex = 2;
            this.DummyButton.Text = "button1";
            this.DummyButton.UseVisualStyleBackColor = true;
            // 
            // MemoryViewComboBox
            // 
            this.MemoryViewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MemoryViewComboBox.FormattingEnabled = true;
            this.MemoryViewComboBox.Location = new System.Drawing.Point(147, 19);
            this.MemoryViewComboBox.Name = "MemoryViewComboBox";
            this.MemoryViewComboBox.Size = new System.Drawing.Size(178, 21);
            this.MemoryViewComboBox.TabIndex = 7;
            this.MemoryViewComboBox.SelectedIndexChanged += new System.EventHandler(this.MemoryViewComboBox_SelectedIndexChanged);
            // 
            // MemAddrTextBox
            // 
            this.MemAddrTextBox.Location = new System.Drawing.Point(6, 20);
            this.MemAddrTextBox.Name = "MemAddrTextBox";
            this.MemAddrTextBox.Size = new System.Drawing.Size(135, 20);
            this.MemAddrTextBox.TabIndex = 6;
            this.MemAddrTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MemAddrTextBox_KeyDown);
            // 
            // MemoryViewScrollBar
            // 
            this.MemoryViewScrollBar.Location = new System.Drawing.Point(328, 46);
            this.MemoryViewScrollBar.Maximum = 1000;
            this.MemoryViewScrollBar.Name = "MemoryViewScrollBar";
            this.MemoryViewScrollBar.Size = new System.Drawing.Size(18, 367);
            this.MemoryViewScrollBar.TabIndex = 5;
            this.MemoryViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.MemoryViewScrollBar_Scroll);
            this.MemoryViewScrollBar.ValueChanged += new System.EventHandler(this.MemoryViewScrollBar_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DasmViewComboBox);
            this.groupBox2.Controls.Add(this.DasmAddrTextBox);
            this.groupBox2.Controls.Add(this.DasmViewScrollBar);
            this.groupBox2.Controls.Add(this.DasmViewPictureBox);
            this.groupBox2.Location = new System.Drawing.Point(372, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 423);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Disassembly";
            // 
            // DasmViewComboBox
            // 
            this.DasmViewComboBox.DisplayMember = "Description";
            this.DasmViewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DasmViewComboBox.FormattingEnabled = true;
            this.DasmViewComboBox.Location = new System.Drawing.Point(147, 19);
            this.DasmViewComboBox.Name = "DasmViewComboBox";
            this.DasmViewComboBox.Size = new System.Drawing.Size(178, 21);
            this.DasmViewComboBox.TabIndex = 10;
            this.DasmViewComboBox.SelectedIndexChanged += new System.EventHandler(this.DasmViewComboBox_SelectedIndexChanged);
            // 
            // DasmAddrTextBox
            // 
            this.DasmAddrTextBox.Location = new System.Drawing.Point(6, 20);
            this.DasmAddrTextBox.Name = "DasmAddrTextBox";
            this.DasmAddrTextBox.Size = new System.Drawing.Size(135, 20);
            this.DasmAddrTextBox.TabIndex = 9;
            this.DasmAddrTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DasmAddrTextBox_KeyDown);
            // 
            // DasmViewScrollBar
            // 
            this.DasmViewScrollBar.Location = new System.Drawing.Point(328, 46);
            this.DasmViewScrollBar.Maximum = 1000;
            this.DasmViewScrollBar.Name = "DasmViewScrollBar";
            this.DasmViewScrollBar.Size = new System.Drawing.Size(18, 367);
            this.DasmViewScrollBar.TabIndex = 8;
            this.DasmViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DasmViewScrollBar_Scroll);
            this.DasmViewScrollBar.ValueChanged += new System.EventHandler(this.DasmViewScrollBar_ValueChanged);
            // 
            // PCTextBox
            // 
            this.PCTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PCTextBox.Location = new System.Drawing.Point(76, 26);
            this.PCTextBox.Name = "PCTextBox";
            this.PCTextBox.Size = new System.Drawing.Size(100, 22);
            this.PCTextBox.TabIndex = 2;
            // 
            // ACCATextBox
            // 
            this.ACCATextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ACCATextBox.Location = new System.Drawing.Point(76, 77);
            this.ACCATextBox.Name = "ACCATextBox";
            this.ACCATextBox.Size = new System.Drawing.Size(100, 22);
            this.ACCATextBox.TabIndex = 3;
            // 
            // ACCBTextBox
            // 
            this.ACCBTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ACCBTextBox.Location = new System.Drawing.Point(240, 77);
            this.ACCBTextBox.Name = "ACCBTextBox";
            this.ACCBTextBox.Size = new System.Drawing.Size(100, 22);
            this.ACCBTextBox.TabIndex = 4;
            // 
            // SPTextBox
            // 
            this.SPTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SPTextBox.Location = new System.Drawing.Point(240, 26);
            this.SPTextBox.Name = "SPTextBox";
            this.SPTextBox.Size = new System.Drawing.Size(100, 22);
            this.SPTextBox.TabIndex = 5;
            // 
            // IXTextBox
            // 
            this.IXTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IXTextBox.Location = new System.Drawing.Point(404, 26);
            this.IXTextBox.Name = "IXTextBox";
            this.IXTextBox.Size = new System.Drawing.Size(100, 22);
            this.IXTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "PC";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(210, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "SP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(374, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "IX";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(30, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "ACCA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(194, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "ACCB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(374, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "CC";
            // 
            // CCTextBox
            // 
            this.CCTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CCTextBox.Location = new System.Drawing.Point(404, 76);
            this.CCTextBox.Name = "CCTextBox";
            this.CCTextBox.Size = new System.Drawing.Size(100, 22);
            this.CCTextBox.TabIndex = 12;
            this.CCTextBox.Text = "00000000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(404, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 14;
            this.label7.Text = "--HINZVC";
            // 
            // disassemblerContextMenuStrip
            // 
            this.disassemblerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRangeToolStripMenuItem,
            this.removeRangeToolStripMenuItem});
            this.disassemblerContextMenuStrip.Name = "disassemblerContextMenuStrip";
            this.disassemblerContextMenuStrip.Size = new System.Drawing.Size(154, 48);
            // 
            // addRangeToolStripMenuItem
            // 
            this.addRangeToolStripMenuItem.Name = "addRangeToolStripMenuItem";
            this.addRangeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.addRangeToolStripMenuItem.Text = "Add Range";
            this.addRangeToolStripMenuItem.Click += new System.EventHandler(this.AddRangeToolStripMenuItem_Click);
            // 
            // removeRangeToolStripMenuItem
            // 
            this.removeRangeToolStripMenuItem.Name = "removeRangeToolStripMenuItem";
            this.removeRangeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.removeRangeToolStripMenuItem.Text = "Remove Range";
            this.removeRangeToolStripMenuItem.Click += new System.EventHandler(this.RemoveRangeToolStripMenuItem_Click);
            // 
            // DasmViewPictureBox
            // 
            this.DasmViewPictureBox.BackColor = System.Drawing.Color.White;
            this.DasmViewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DasmViewPictureBox.Location = new System.Drawing.Point(6, 46);
            this.DasmViewPictureBox.Name = "DasmViewPictureBox";
            this.DasmViewPictureBox.Size = new System.Drawing.Size(319, 367);
            this.DasmViewPictureBox.TabIndex = 7;
            this.DasmViewPictureBox.TabStop = false;
            this.DasmViewPictureBox.Click += new System.EventHandler(this.DasmViewPictureBox_Click);
            this.DasmViewPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DasmViewPictureBox_MouseClick);
            // 
            // MemoryViewPictureBox
            // 
            this.MemoryViewPictureBox.BackColor = System.Drawing.Color.White;
            this.MemoryViewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MemoryViewPictureBox.Location = new System.Drawing.Point(6, 46);
            this.MemoryViewPictureBox.Name = "MemoryViewPictureBox";
            this.MemoryViewPictureBox.Size = new System.Drawing.Size(319, 367);
            this.MemoryViewPictureBox.TabIndex = 4;
            this.MemoryViewPictureBox.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.PCTextBox);
            this.groupBox3.Controls.Add(this.ACCATextBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.ACCBTextBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.SPTextBox);
            this.groupBox3.Controls.Add(this.CCTextBox);
            this.groupBox3.Controls.Add(this.IXTextBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(9, 436);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(723, 125);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // DebuggerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 565);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DebuggerView";
            this.ShowIcon = false;
            this.Text = "ET-3400 Debugger";
            this.Load += new System.EventHandler(this.DebuggerView_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.disassemblerContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DasmViewPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MemoryViewPictureBox)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox MemoryViewComboBox;
        private System.Windows.Forms.TextBox MemAddrTextBox;
        private System.Windows.Forms.VScrollBar MemoryViewScrollBar;
        private System.Windows.Forms.PictureBox MemoryViewPictureBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox DasmViewComboBox;
        private System.Windows.Forms.TextBox DasmAddrTextBox;
        private System.Windows.Forms.VScrollBar DasmViewScrollBar;
        private System.Windows.Forms.PictureBox DasmViewPictureBox;
        private System.Windows.Forms.Button DummyButton;
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
    }
}