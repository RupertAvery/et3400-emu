namespace Sharp6800
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.ResetButton = new System.Windows.Forms.Button();
            this.OUTCHCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.LargeChange = 1;
            this.hScrollBar1.Location = new System.Drawing.Point(28, 47);
            this.hScrollBar1.Maximum = 10;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(233, 24);
            this.hScrollBar1.TabIndex = 20;
            this.hScrollBar1.Value = 5;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Clock Speed: 100000 cycles/sec";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(102, 200);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 22;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // OUTCHCheckBox
            // 
            this.OUTCHCheckBox.AutoSize = true;
            this.OUTCHCheckBox.Checked = true;
            this.OUTCHCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OUTCHCheckBox.Location = new System.Drawing.Point(28, 94);
            this.OUTCHCheckBox.Name = "OUTCHCheckBox";
            this.OUTCHCheckBox.Size = new System.Drawing.Size(93, 17);
            this.OUTCHCheckBox.TabIndex = 23;
            this.OUTCHCheckBox.Text = "OUTCH Hack";
            this.toolTip1.SetToolTip(this.OUTCHCheckBox, resources.GetString("OUTCHCheckBox.ToolTip"));
            this.OUTCHCheckBox.UseVisualStyleBackColor = true;
            this.OUTCHCheckBox.CheckedChanged += new System.EventHandler(this.OUTCHCheckBox_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.OUTCHCheckBox);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hScrollBar1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.CheckBox OUTCHCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}