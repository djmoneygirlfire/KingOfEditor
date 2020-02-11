using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 편집기의_제왕
{
    public partial class 해석오른쪽클릭메뉴 : Form
    {
        public string _선택된스트링;
        public string _문제DB루트폴더;

        public 해석오른쪽클릭메뉴()
        {
            InitializeComponent();
        }

        public void 코퍼스에넣기버튼텍스트업데이트()
        {
            선택문구코퍼스에넣기.Text = _선택된스트링.을를(_선택된스트링) + " 코퍼스 데이터에 추가합니다.";
        }

        private void 선택문구코퍼스에넣기_Click(object sender, EventArgs e)
        {
            if(_선택된스트링.Contains(" "))
            {
                메시지박스.보여주기("코퍼스에 넣을 내용에 띄어쓰기를 포함하면 안됩니다.", this);
                this.Hide();
                return;
            }

            
            if(Form1._검색.한글코퍼스사전(_선택된스트링.Trim()))
            {
                메시지박스.보여주기("이미 존재하는 항목입니다.", this);
                this.Hide();
                return;
            }

            StreamWriter w = File.AppendText(_문제DB루트폴더 + "corpusKOR.txt");


            w.WriteLine(_선택된스트링.Trim());
            w.Close();
            // string 문자열_ = System.IO.File.ReadAllText(_문제DB루트폴더 + "corpusKOR.txt",System.Text.Encoding.Default);

            메시지박스.보여주기("성공적으로 추가하였습니다. 추가한 내용은 다음 실행시 반영됩니다.", this);
            this.Hide();
        }
    }
}
