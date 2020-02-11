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
    public partial class 메시지를스태틱으로만들기위한클래스 : Form
    {
        public 메시지를스태틱으로만들기위한클래스(string 메시지내용)
        {
            InitializeComponent();
            label1.Text = 메시지내용;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DestroyHandle();
        }
    }
}