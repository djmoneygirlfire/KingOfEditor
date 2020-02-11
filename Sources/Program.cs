using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 편집기의_제왕
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] 매개변수들)
        {
            string 시작할때열_파일 = "";
            
            if(매개변수들.Length > 0)
            {
                시작할때열_파일 = Convert.ToString(매개변수들[0]);
                //MessageBox.Show(시작할때열_파일);
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(시작할때열_파일));
        }
    }
}
