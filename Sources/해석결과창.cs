using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 편집기의_제왕
{
	public partial class 해석결과창 : Form
	{
		public 해석결과창()
		{
			InitializeComponent();
		}

		public 해석결과창(string 해석결과)
		{
			InitializeComponent();
			richTextBox1.Text = 해석결과;
		}

		private void 종료버튼_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
