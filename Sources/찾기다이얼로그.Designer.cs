namespace 편집기의_제왕
{
    partial class 찾기다이얼로그
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(찾기다이얼로그));
            this.다음찾기 = new System.Windows.Forms.Button();
            this.취소 = new System.Windows.Forms.Button();
            this.찾을내용 = new System.Windows.Forms.RichTextBox();
            this.위로 = new System.Windows.Forms.RadioButton();
            this.아래로 = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.편집ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.붙여넣기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.실행취소ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 다음찾기
            // 
            this.다음찾기.BackColor = System.Drawing.Color.Transparent;
            this.다음찾기.Location = new System.Drawing.Point(413, 75);
            this.다음찾기.Name = "다음찾기";
            this.다음찾기.Size = new System.Drawing.Size(94, 23);
            this.다음찾기.TabIndex = 3;
            this.다음찾기.Text = "다음 찾기(F)";
            this.다음찾기.UseVisualStyleBackColor = false;
            this.다음찾기.Click += new System.EventHandler(this.다음찾기_Click);
            // 
            // 취소
            // 
            this.취소.BackColor = System.Drawing.Color.Transparent;
            this.취소.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.취소.Location = new System.Drawing.Point(413, 104);
            this.취소.Name = "취소";
            this.취소.Size = new System.Drawing.Size(94, 23);
            this.취소.TabIndex = 4;
            this.취소.Text = "취소";
            this.취소.UseVisualStyleBackColor = false;
            this.취소.Click += new System.EventHandler(this.취소_Click);
            // 
            // 찾을내용
            // 
            this.찾을내용.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.찾을내용.Location = new System.Drawing.Point(12, 12);
            this.찾을내용.Name = "찾을내용";
            this.찾을내용.Size = new System.Drawing.Size(392, 170);
            this.찾을내용.TabIndex = 5;
            this.찾을내용.Text = "";
            this.찾을내용.WordWrap = false;
            // 
            // 위로
            // 
            this.위로.AutoSize = true;
            this.위로.BackColor = System.Drawing.Color.Transparent;
            this.위로.Location = new System.Drawing.Point(413, 22);
            this.위로.Name = "위로";
            this.위로.Size = new System.Drawing.Size(65, 16);
            this.위로.TabIndex = 0;
            this.위로.Text = "위로(U)";
            this.위로.UseVisualStyleBackColor = false;
            // 
            // 아래로
            // 
            this.아래로.AutoSize = true;
            this.아래로.BackColor = System.Drawing.Color.Transparent;
            this.아래로.Checked = true;
            this.아래로.Location = new System.Drawing.Point(413, 44);
            this.아래로.Name = "아래로";
            this.아래로.Size = new System.Drawing.Size(77, 16);
            this.아래로.TabIndex = 1;
            this.아래로.TabStop = true;
            this.아래로.Text = "아래로(D)";
            this.아래로.UseVisualStyleBackColor = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.편집ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(516, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // 편집ToolStripMenuItem
            // 
            this.편집ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.붙여넣기ToolStripMenuItem,
            this.삭제ToolStripMenuItem,
            this.실행취소ToolStripMenuItem});
            this.편집ToolStripMenuItem.Name = "편집ToolStripMenuItem";
            this.편집ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.편집ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.편집ToolStripMenuItem.Text = "편집";
            // 
            // 붙여넣기ToolStripMenuItem
            // 
            this.붙여넣기ToolStripMenuItem.Name = "붙여넣기ToolStripMenuItem";
            this.붙여넣기ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.붙여넣기ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.붙여넣기ToolStripMenuItem.Text = "붙여넣기";
            this.붙여넣기ToolStripMenuItem.Click += new System.EventHandler(this.붙여넣기ToolStripMenuItem_Click);
            // 
            // 삭제ToolStripMenuItem
            // 
            this.삭제ToolStripMenuItem.Name = "삭제ToolStripMenuItem";
            this.삭제ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.삭제ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.삭제ToolStripMenuItem.Text = "삭제";
            this.삭제ToolStripMenuItem.Click += new System.EventHandler(this.삭제ToolStripMenuItem_Click);
            // 
            // 실행취소ToolStripMenuItem
            // 
            this.실행취소ToolStripMenuItem.Name = "실행취소ToolStripMenuItem";
            this.실행취소ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.실행취소ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.실행취소ToolStripMenuItem.Text = "실행 취소";
            this.실행취소ToolStripMenuItem.Click += new System.EventHandler(this.실행취소ToolStripMenuItem_Click);
            // 
            // 찾기다이얼로그
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(516, 194);
            this.Controls.Add(this.위로);
            this.Controls.Add(this.아래로);
            this.Controls.Add(this.찾을내용);
            this.Controls.Add(this.취소);
            this.Controls.Add(this.다음찾기);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "찾기다이얼로그";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.Text = "찾기";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button 다음찾기;
        private System.Windows.Forms.Button 취소;
        private System.Windows.Forms.RichTextBox 찾을내용;
        private System.Windows.Forms.RadioButton 위로;
        private System.Windows.Forms.RadioButton 아래로;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 편집ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 붙여넣기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 삭제ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 실행취소ToolStripMenuItem;
    }
}