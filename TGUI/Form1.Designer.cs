namespace TGUI
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新規プロジェクト作成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.プロジェクトを開くToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ファイル保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buildButton = new System.Windows.Forms.ToolStripButton();
            this.runButton = new System.Windows.Forms.ToolStripButton();
            this.editTextBox = new System.Windows.Forms.TextBox();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(15, 316);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "コンパイラバージョン情報";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.textBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox1.Location = new System.Drawing.Point(15, 348);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(737, 114);
            this.textBox1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(768, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新規プロジェクト作成ToolStripMenuItem,
            this.プロジェクトを開くToolStripMenuItem,
            this.ファイル保存ToolStripMenuItem});
            this.ファイルToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Info;
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(63, 24);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            this.ファイルToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // 新規プロジェクト作成ToolStripMenuItem
            // 
            this.新規プロジェクト作成ToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.新規プロジェクト作成ToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Info;
            this.新規プロジェクト作成ToolStripMenuItem.Name = "新規プロジェクト作成ToolStripMenuItem";
            this.新規プロジェクト作成ToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.新規プロジェクト作成ToolStripMenuItem.Text = "新規プロジェクト作成";
            this.新規プロジェクト作成ToolStripMenuItem.Click += new System.EventHandler(this.新規プロジェクト作成ToolStripMenuItem_Click);
            // 
            // プロジェクトを開くToolStripMenuItem
            // 
            this.プロジェクトを開くToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.プロジェクトを開くToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Info;
            this.プロジェクトを開くToolStripMenuItem.Name = "プロジェクトを開くToolStripMenuItem";
            this.プロジェクトを開くToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.プロジェクトを開くToolStripMenuItem.Text = "プロジェクトを開く";
            this.プロジェクトを開くToolStripMenuItem.Click += new System.EventHandler(this.プロジェクトを開くToolStripMenuItem_Click);
            // 
            // ファイル保存ToolStripMenuItem
            // 
            this.ファイル保存ToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.ファイル保存ToolStripMenuItem.ForeColor = System.Drawing.SystemColors.Info;
            this.ファイル保存ToolStripMenuItem.Name = "ファイル保存ToolStripMenuItem";
            this.ファイル保存ToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.ファイル保存ToolStripMenuItem.Text = "ファイル保存";
            this.ファイル保存ToolStripMenuItem.Click += new System.EventHandler(this.ファイル保存ToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildButton,
            this.runButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(768, 27);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // buildButton
            // 
            this.buildButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.buildButton.ForeColor = System.Drawing.SystemColors.Info;
            this.buildButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(47, 24);
            this.buildButton.Text = "ビルド";
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // runButton
            // 
            this.runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.runButton.ForeColor = System.Drawing.SystemColors.Info;
            this.runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(43, 24);
            this.runButton.Text = "実行";
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // editTextBox
            // 
            this.editTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.editTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.editTextBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.editTextBox.ForeColor = System.Drawing.SystemColors.Info;
            this.editTextBox.Location = new System.Drawing.Point(15, 86);
            this.editTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editTextBox.Multiline = true;
            this.editTextBox.Name = "editTextBox";
            this.editTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editTextBox.Size = new System.Drawing.Size(736, 223);
            this.editTextBox.TabIndex = 4;
            this.editTextBox.WordWrap = false;
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(54)))));
            this.fileNameLabel.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fileNameLabel.ForeColor = System.Drawing.SystemColors.Info;
            this.fileNameLabel.Location = new System.Drawing.Point(16, 64);
            this.fileNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(168, 24);
            this.fileNameLabel.TabIndex = 5;
            this.fileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.fileNameLabel.UseMnemonic = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(768, 476);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.editTextBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Micsosoft Visual Stdio";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新規プロジェクト作成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem プロジェクトを開くToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton runButton;
        private System.Windows.Forms.ToolStripButton buildButton;
        private System.Windows.Forms.TextBox editTextBox;
        private System.Windows.Forms.ToolStripMenuItem ファイル保存ToolStripMenuItem;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

