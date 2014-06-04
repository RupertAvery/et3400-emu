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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MemoryViewComboBox = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MemoryViewScrollBar = new System.Windows.Forms.VScrollBar();
            this.MemeoryViewPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DasmViewComboBox = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.DasmViewScrollBar = new System.Windows.Forms.VScrollBar();
            this.DasmViewPictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemeoryViewPictureBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DasmViewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MemoryViewComboBox);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.MemoryViewScrollBar);
            this.groupBox1.Controls.Add(this.MemeoryViewPictureBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 423);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Memory";
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(135, 20);
            this.textBox1.TabIndex = 6;
            // 
            // MemoryViewScrollBar
            // 
            this.MemoryViewScrollBar.Location = new System.Drawing.Point(328, 46);
            this.MemoryViewScrollBar.Maximum = 1000;
            this.MemoryViewScrollBar.Name = "MemoryViewScrollBar";
            this.MemoryViewScrollBar.Size = new System.Drawing.Size(23, 367);
            this.MemoryViewScrollBar.TabIndex = 5;
            this.MemoryViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.MemoryViewScrollBar_Scroll);
            // 
            // MemeoryViewPictureBox
            // 
            this.MemeoryViewPictureBox.BackColor = System.Drawing.Color.White;
            this.MemeoryViewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MemeoryViewPictureBox.Location = new System.Drawing.Point(6, 46);
            this.MemeoryViewPictureBox.Name = "MemeoryViewPictureBox";
            this.MemeoryViewPictureBox.Size = new System.Drawing.Size(319, 367);
            this.MemeoryViewPictureBox.TabIndex = 4;
            this.MemeoryViewPictureBox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DasmViewComboBox);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.DasmViewScrollBar);
            this.groupBox2.Controls.Add(this.DasmViewPictureBox);
            this.groupBox2.Location = new System.Drawing.Point(375, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 423);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Disassembly";
            // 
            // DasmViewComboBox
            // 
            this.DasmViewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DasmViewComboBox.FormattingEnabled = true;
            this.DasmViewComboBox.Location = new System.Drawing.Point(147, 19);
            this.DasmViewComboBox.Name = "DasmViewComboBox";
            this.DasmViewComboBox.Size = new System.Drawing.Size(178, 21);
            this.DasmViewComboBox.TabIndex = 10;
            this.DasmViewComboBox.SelectedIndexChanged += new System.EventHandler(this.DasmViewComboBox_SelectedIndexChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 20);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(135, 20);
            this.textBox2.TabIndex = 9;
            // 
            // DasmViewScrollBar
            // 
            this.DasmViewScrollBar.Location = new System.Drawing.Point(328, 46);
            this.DasmViewScrollBar.Maximum = 1000;
            this.DasmViewScrollBar.Name = "DasmViewScrollBar";
            this.DasmViewScrollBar.Size = new System.Drawing.Size(23, 367);
            this.DasmViewScrollBar.TabIndex = 8;
            this.DasmViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DasmViewScrollBar_Scroll);
            // 
            // pictureBox1
            // 
            this.DasmViewPictureBox.BackColor = System.Drawing.Color.White;
            this.DasmViewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DasmViewPictureBox.Location = new System.Drawing.Point(6, 46);
            this.DasmViewPictureBox.Name = "DasmViewPictureBox";
            this.DasmViewPictureBox.Size = new System.Drawing.Size(319, 367);
            this.DasmViewPictureBox.TabIndex = 7;
            this.DasmViewPictureBox.TabStop = false;
            // 
            // DebuggerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 499);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "DebuggerView";
            this.Text = "DebuggerView";
            this.Load += new System.EventHandler(this.DebuggerView_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemeoryViewPictureBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DasmViewPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox MemoryViewComboBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.VScrollBar MemoryViewScrollBar;
        private System.Windows.Forms.PictureBox MemeoryViewPictureBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox DasmViewComboBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.VScrollBar DasmViewScrollBar;
        private System.Windows.Forms.PictureBox DasmViewPictureBox;

    }
}