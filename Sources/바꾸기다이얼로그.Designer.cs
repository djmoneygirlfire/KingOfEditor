namespace 편집기의_제왕
{
    partial class 바꾸기다이얼로그
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(바꾸기다이얼로그));
            this.찾을내용 = new System.Windows.Forms.RichTextBox();
            this.바꿀내용 = new System.Windows.Forms.RichTextBox();
            this.다음찾기 = new System.Windows.Forms.Button();
            this.바꾸기 = new System.Windows.Forms.Button();
            this.모두바꾸기 = new System.Windows.Forms.Button();
            this.취소 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.편집ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.붙여넣기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 찾을내용
            // 
            this.찾을내용.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.찾을내용.Location = new System.Drawing.Point(12, 12);
            this.찾을내용.Name = "찾을내용";
            this.찾을내용.Size = new System.Drawing.Size(378, 161);
            this.찾을내용.TabIndex = 0;
            this.찾을내용.Text = "";
            // 
            // 바꿀내용
            // 
            this.바꿀내용.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.바꿀내용.Location = new System.Drawing.Point(12, 179);
            this.바꿀내용.Name = "바꿀내용";
            this.바꿀내용.Size = new System.Drawing.Size(378, 162);
            this.바꿀내용.TabIndex = 1;
            this.바꿀내용.Text = "";
            // 
            // 다음찾기
            // 
            this.다음찾기.BackColor = System.Drawing.Color.Linen;
            this.다음찾기.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.다음찾기.ForeColor = System.Drawing.SystemColors.GrayText;
            this.다음찾기.Location = new System.Drawing.Point(396, 12);
            this.다음찾기.Name = "다음찾기";
            this.다음찾기.Size = new System.Drawing.Size(108, 23);
            this.다음찾기.TabIndex = 2;
            this.다음찾기.Text = "다음 찾기(&F)";
            this.다음찾기.UseVisualStyleBackColor = false;
            this.다음찾기.Click += new System.EventHandler(this.다음찾기_Click);
            // 
            // 바꾸기
            // 
            this.바꾸기.BackColor = System.Drawing.Color.Silver;
            this.바꾸기.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.바꾸기.ForeColor = System.Drawing.SystemColors.GrayText;
            this.바꾸기.Location = new System.Drawing.Point(396, 42);
            this.바꾸기.Name = "바꾸기";
            this.바꾸기.Size = new System.Drawing.Size(108, 23);
            this.바꾸기.TabIndex = 3;
            this.바꾸기.Text = "바꾸기(&R)";
            this.바꾸기.UseVisualStyleBackColor = false;
            this.바꾸기.Click += new System.EventHandler(this.바꾸기_Click);
            // 
            // 모두바꾸기
            // 
            this.모두바꾸기.BackColor = System.Drawing.Color.Snow;
            this.모두바꾸기.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.모두바꾸기.ForeColor = System.Drawing.SystemColors.GrayText;
            this.모두바꾸기.Location = new System.Drawing.Point(396, 72);
            this.모두바꾸기.Name = "모두바꾸기";
            this.모두바꾸기.Size = new System.Drawing.Size(108, 23);
            this.모두바꾸기.TabIndex = 4;
            this.모두바꾸기.Text = "모두 바꾸기(&A)";
            this.모두바꾸기.UseVisualStyleBackColor = false;
            this.모두바꾸기.Click += new System.EventHandler(this.모두바꾸기_Click);
            // 
            // 취소
            // 
            this.취소.BackColor = System.Drawing.Color.DarkSalmon;
            this.취소.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.취소.Location = new System.Drawing.Point(396, 129);
            this.취소.Name = "취소";
            this.취소.Size = new System.Drawing.Size(108, 23);
            this.취소.TabIndex = 5;
            this.취소.Text = "취소";
            this.취소.UseVisualStyleBackColor = false;
            this.취소.Click += new System.EventHandler(this.취소_Click);
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
            this.붙여넣기ToolStripMenuItem});
            this.편집ToolStripMenuItem.Name = "편집ToolStripMenuItem";
            this.편집ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.편집ToolStripMenuItem.Text = "편집";
            // 
            // 붙여넣기ToolStripMenuItem
            // 
            this.붙여넣기ToolStripMenuItem.Name = "붙여넣기ToolStripMenuItem";
            this.붙여넣기ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.붙여넣기ToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.붙여넣기ToolStripMenuItem.Text = "붙여넣기";
            // 
            // 바꾸기다이얼로그
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(516, 353);
            this.Controls.Add(this.취소);
            this.Controls.Add(this.모두바꾸기);
            this.Controls.Add(this.바꾸기);
            this.Controls.Add(this.다음찾기);
            this.Controls.Add(this.바꿀내용);
            this.Controls.Add(this.찾을내용);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "바꾸기다이얼로그";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.Text = "바꾸기";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox 찾을내용;
        private System.Windows.Forms.RichTextBox 바꿀내용;
        private System.Windows.Forms.Button 다음찾기;
        private System.Windows.Forms.Button 바꾸기;
        private System.Windows.Forms.Button 모두바꾸기;
        private System.Windows.Forms.Button 취소;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 편집ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 붙여넣기ToolStripMenuItem;
    }
}