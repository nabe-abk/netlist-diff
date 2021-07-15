namespace netlist_diff
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.FileLabel1 = new System.Windows.Forms.Label();
			this.FileName1 = new System.Windows.Forms.TextBox();
			this.FileSelectBtn1 = new System.Windows.Forms.Button();
			this.FileLabel2 = new System.Windows.Forms.Label();
			this.FileName2 = new System.Windows.Forms.TextBox();
			this.FileSelectBtn2 = new System.Windows.Forms.Button();
			this.RunBtn = new System.Windows.Forms.Button();
			this.MsgBox = new System.Windows.Forms.RichTextBox();
			this.TrReplaceSetting = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.AutoChangePinSetting = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// FileLabel1
			// 
			this.FileLabel1.AutoSize = true;
			this.FileLabel1.Location = new System.Drawing.Point(8, 15);
			this.FileLabel1.Name = "FileLabel1";
			this.FileLabel1.Size = new System.Drawing.Size(34, 15);
			this.FileLabel1.TabIndex = 0;
			this.FileLabel1.Text = "File1";
			// 
			// FileName1
			// 
			this.FileName1.Location = new System.Drawing.Point(48, 12);
			this.FileName1.Name = "FileName1";
			this.FileName1.Size = new System.Drawing.Size(424, 23);
			this.FileName1.TabIndex = 1;
			// 
			// FileSelectBtn1
			// 
			this.FileSelectBtn1.Location = new System.Drawing.Point(478, 12);
			this.FileSelectBtn1.Name = "FileSelectBtn1";
			this.FileSelectBtn1.Size = new System.Drawing.Size(75, 23);
			this.FileSelectBtn1.TabIndex = 2;
			this.FileSelectBtn1.Text = "SELECT";
			this.FileSelectBtn1.UseVisualStyleBackColor = true;
			this.FileSelectBtn1.Click += new System.EventHandler(this.FileSelectBtn1_Click);
			// 
			// FileLabel2
			// 
			this.FileLabel2.AutoSize = true;
			this.FileLabel2.Location = new System.Drawing.Point(8, 44);
			this.FileLabel2.Name = "FileLabel2";
			this.FileLabel2.Size = new System.Drawing.Size(34, 15);
			this.FileLabel2.TabIndex = 0;
			this.FileLabel2.Text = "File2";
			// 
			// FileName2
			// 
			this.FileName2.Location = new System.Drawing.Point(48, 41);
			this.FileName2.Name = "FileName2";
			this.FileName2.Size = new System.Drawing.Size(424, 23);
			this.FileName2.TabIndex = 3;
			// 
			// FileSelectBtn2
			// 
			this.FileSelectBtn2.Location = new System.Drawing.Point(478, 41);
			this.FileSelectBtn2.Name = "FileSelectBtn2";
			this.FileSelectBtn2.Size = new System.Drawing.Size(75, 23);
			this.FileSelectBtn2.TabIndex = 4;
			this.FileSelectBtn2.Text = "SELECT";
			this.FileSelectBtn2.UseVisualStyleBackColor = true;
			this.FileSelectBtn2.Click += new System.EventHandler(this.FileSelectBtn2_Click);
			// 
			// RunBtn
			// 
			this.RunBtn.Location = new System.Drawing.Point(478, 69);
			this.RunBtn.Name = "RunBtn";
			this.RunBtn.Size = new System.Drawing.Size(75, 23);
			this.RunBtn.TabIndex = 7;
			this.RunBtn.Text = "RUN";
			this.RunBtn.UseVisualStyleBackColor = true;
			this.RunBtn.Click += new System.EventHandler(this.RunBtn_Click);
			// 
			// MsgBox
			// 
			this.MsgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MsgBox.Location = new System.Drawing.Point(8, 98);
			this.MsgBox.Name = "MsgBox";
			this.MsgBox.Size = new System.Drawing.Size(542, 271);
			this.MsgBox.TabIndex = 8;
			this.MsgBox.Text = "";
			// 
			// TrReplaceSetting
			// 
			this.TrReplaceSetting.FormattingEnabled = true;
			this.TrReplaceSetting.Items.AddRange(new object[] {
            "SMD",
            "TO-92",
            "None"});
			this.TrReplaceSetting.Location = new System.Drawing.Point(414, 69);
			this.TrReplaceSetting.Name = "TrReplaceSetting";
			this.TrReplaceSetting.Size = new System.Drawing.Size(58, 23);
			this.TrReplaceSetting.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(306, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 15);
			this.label1.TabIndex = 9;
			this.label1.Text = "Tr B/E/C replace";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 15);
			this.label2.TabIndex = 10;
			this.label2.Text = "Auto change pin 1<>2";
			// 
			// AutoChangePinSetting
			// 
			this.AutoChangePinSetting.Location = new System.Drawing.Point(152, 69);
			this.AutoChangePinSetting.Name = "AutoChangePinSetting";
			this.AutoChangePinSetting.Size = new System.Drawing.Size(137, 23);
			this.AutoChangePinSetting.TabIndex = 5;
			this.AutoChangePinSetting.Text = "R|L|C|FB|B";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 381);
			this.Controls.Add(this.AutoChangePinSetting);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TrReplaceSetting);
			this.Controls.Add(this.RunBtn);
			this.Controls.Add(this.MsgBox);
			this.Controls.Add(this.FileSelectBtn2);
			this.Controls.Add(this.FileName2);
			this.Controls.Add(this.FileLabel2);
			this.Controls.Add(this.FileSelectBtn1);
			this.Controls.Add(this.FileName1);
			this.Controls.Add(this.FileLabel1);
			this.MinimumSize = new System.Drawing.Size(580, 420);
			this.Name = "MainForm";
			this.Text = "netlist-diff";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FileLabel1;
        private System.Windows.Forms.TextBox FileName1;
        private System.Windows.Forms.Button FileSelectBtn1;
        private System.Windows.Forms.Label FileLabel2;
        private System.Windows.Forms.TextBox FileName2;
        private System.Windows.Forms.Button FileSelectBtn2;
        private System.Windows.Forms.Button RunBtn;
		private System.Windows.Forms.RichTextBox MsgBox;
		private System.Windows.Forms.ComboBox TrReplaceSetting;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox AutoChangePinSetting;
	}
}

