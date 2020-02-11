namespace 편집기의_제왕
{
	partial class 해석결과창
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(해석결과창));
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.종료버튼 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.Color.LightSlateGray;
			this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.richTextBox1.Location = new System.Drawing.Point(8, 9);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(332, 100);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			// 
			// 종료버튼
			// 
			this.종료버튼.BackColor = System.Drawing.Color.IndianRed;
			this.종료버튼.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.종료버튼.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.종료버튼.Location = new System.Drawing.Point(305, 137);
			this.종료버튼.Name = "종료버튼";
			this.종료버튼.Size = new System.Drawing.Size(57, 23);
			this.종료버튼.TabIndex = 1;
			this.종료버튼.Text = "끄기";
			this.종료버튼.UseVisualStyleBackColor = false;
			this.종료버튼.Click += new System.EventHandler(this.종료버튼_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(111)))), ((int)(((byte)(128)))));
			this.panel1.Controls.Add(this.richTextBox1);
			this.panel1.Location = new System.Drawing.Point(13, 13);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(349, 118);
			this.panel1.TabIndex = 3;
			// 
			// 해석결과창
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(374, 168);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.종료버튼);
			this.Font = new System.Drawing.Font("청소년서체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "해석결과창";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button 종료버튼;
		private System.Windows.Forms.Panel panel1;
	}
}