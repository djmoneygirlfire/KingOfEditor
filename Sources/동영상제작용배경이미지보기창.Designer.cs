namespace 편집기의_제왕
{
    partial class 동영상제작용배경이미지보기창
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
            this.동영상제작용배경웹페이지 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // 동영상제작용배경웹페이지
            // 
            this.동영상제작용배경웹페이지.Location = new System.Drawing.Point(5, 25);
            this.동영상제작용배경웹페이지.MinimumSize = new System.Drawing.Size(20, 20);
            this.동영상제작용배경웹페이지.Name = "동영상제작용배경웹페이지";
            this.동영상제작용배경웹페이지.Size = new System.Drawing.Size(1280, 720);
            this.동영상제작용배경웹페이지.TabIndex = 0;
            // 
            // 동영상제작용배경이미지보기창
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 750);
            this.Controls.Add(this.동영상제작용배경웹페이지);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "동영상제작용배경이미지보기창";
            this.Text = "동영상제작용배경이미지보기창";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser 동영상제작용배경웹페이지;
    }
}