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
    class 메시지박스
    {
        static 메시지를스태틱으로만들기위한클래스 _메시지박스;

        public static void 보여주기(string 보여줄내용, Form this라고쓰세요)
        {
            _메시지박스 = new 메시지를스태틱으로만들기위한클래스(보여줄내용);
            _메시지박스.Owner = this라고쓰세요;
            _메시지박스.StartPosition = FormStartPosition.Manual;
            _메시지박스.Location = new Point(this라고쓰세요.Location.X + this라고쓰세요.Width / 2 - (333 / 2), this라고쓰세요.Location.Y + this라고쓰세요.Height / 2 - 77 / 2);
            if (_메시지박스.ShowDialog() == DialogResult.OK)
            {
                this라고쓰세요.Focus();
            }

        }
    }


}
