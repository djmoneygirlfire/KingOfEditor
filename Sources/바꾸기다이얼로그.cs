using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices; // DLL import

namespace 편집기의_제왕
{
    public partial class 바꾸기다이얼로그 : Form
    {
        #region 하이라이트 기능을 위한 변수들
        public Regex 키워드들;
        public Regex 구문분석들;
        public Regex 문제형식들;
        public Regex 정답들;
        public Regex ABC형보기들;
        public bool _텍스트변경사항자동저장불필요;
        #endregion

        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public 바꾸기다이얼로그(string 첫선택내용)
        {
            InitializeComponent();

            찾을내용.Text = 첫선택내용;

            #region 하이라이트 기능을 위한 초기화
            키워드들 = new Regex("<CAKE>|</CAKE>|<B>|</B>|<T>|</T>|<TR>|</TR>|<TBAR></TBAR>|<PAGEEND></PAGEEND>|<Q>|</Q>|<A>|</A>|<A0>|</A0>|<A1>|</A1>|<A2>|</A2>|<A3>|</A3>|<A4>|</A4>|<A5>|</A5>");
            구문분석들 = new Regex("ⓢ|ⓥ|ⓞ|ⓒ|\\(ⓒ\\)|ⓧ|ⓘ|ⓓ|\\(ⓓ\\)|㉨|{|}");
            문제형식들 = new Regex("{지시}|{주제}|{제목}|{속담}|{빈칸}|{요약}|{어법}|{어휘}|{분위기}|{일치}|{흐름}");
            정답들 = new Regex("정답 ①번|정답 ②번|정답 ③번|정답 ④번|정답 ⑤번");
            ABC형보기들 = new Regex("\\[|\\]|\\(a\\)|\\(b\\)|\\(c\\)|\\(A\\)|\\(B\\)|\\(C\\)|/");
            #endregion

            if (!찾을내용.Text.Contains("\n"))
                찾을내용.WordWrap = true;
            화면업데이트중지();
            전체화면하이라이트표시();
            화면업데이트재개();
            _텍스트변경사항자동저장불필요 = false;
        }

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);

			//            MessageBox.Show(string.Format("{0}", key));

			switch (key)
			{
				case Keys.V:

					if ((keyData & Keys.Control) != 0)
					{
						if (찾을내용.Focused)
							찾을내용.Text += Clipboard.GetText(); 
						else if(바꿀내용.Focused)
						   바꿀내용.Text += Clipboard.GetText(); 

						return true;
					}



					break;

			}

			return base.ProcessCmdKey(ref msg, keyData);
		}


		#region 버튼

		private void 다음찾기_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;
            
            bool 찾은결과 = 폼1.다음찾기(찾을내용.Text, true);
            폼1.Focus();

            if (!찾은결과)    메시지박스.보여주기("더 이상 찾는 내용이 없습니다.", this);
        }
        private void 바꾸기_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            bool 바꾼결과 = 폼1.바꾸기(찾을내용.Text, 바꿀내용.Text);
            폼1.Focus();

            if (!바꾼결과) 메시지박스.보여주기("더 이상 바꿀 내용이 없습니다.", this);
        }
        private void 모두바꾸기_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;
            int 바꾼결과 = 폼1.모두바꾸기(찾을내용.Text, 바꿀내용.Text);
            폼1.Focus();


        }
        private void 취소_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void 화면업데이트중지()
        {
            Message 업데이트중지 = Message.Create(찾을내용.Handle, 0x000B, IntPtr.Zero, IntPtr.Zero);
            NativeWindow window = NativeWindow.FromHandle(찾을내용.Handle);
            window.DefWndProc(ref 업데이트중지);
        }
        private void 화면업데이트재개()
        {
            IntPtr wparam = new IntPtr(1);
            Message 업데이트재개 = Message.Create(찾을내용.Handle, 0x000B, wparam, IntPtr.Zero);
            NativeWindow window = NativeWindow.FromHandle(찾을내용.Handle);

            window.DefWndProc(ref 업데이트재개);

            찾을내용.Invalidate();
        }

        private void 전체화면하이라이트표시()
        {

            int 선택시작위치 = 찾을내용.SelectionStart;
            int 선택길이 = 찾을내용.SelectionLength;

            찾을내용.SelectAll();
            찾을내용.SelectionColor = Color.Black;
            찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Bold);

            foreach (Match ABC형보기 in ABC형보기들.Matches(찾을내용.Text))
            {
                찾을내용.Select(ABC형보기.Index, ABC형보기.Length);
                찾을내용.SelectionColor = Color.MidnightBlue;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Bold);
            }


            foreach (Match 키워드매치 in 키워드들.Matches(찾을내용.Text))
            {
                찾을내용.Select(키워드매치.Index, 키워드매치.Length);
                찾을내용.SelectionColor = Color.Blue;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }

            foreach (Match 구문분석 in 구문분석들.Matches(찾을내용.Text))
            {
                찾을내용.Select(구문분석.Index, 구문분석.Length);
                찾을내용.SelectionColor = Color.DarkGreen;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }

            foreach (Match 문제형식 in 문제형식들.Matches(찾을내용.Text))
            {
                찾을내용.Select(문제형식.Index, 문제형식.Length);
                찾을내용.SelectionColor = Color.OrangeRed;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }



            foreach (Match 정답 in 정답들.Matches(찾을내용.Text))
            {
                찾을내용.Select(정답.Index, 정답.Length);
                찾을내용.SelectionColor = Color.IndianRed;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }




            찾을내용.SelectionStart = 선택시작위치;
            찾을내용.SelectionLength = 선택길이;

        }




    }
}
