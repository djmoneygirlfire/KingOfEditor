﻿using System;
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
    public partial class 찾기다이얼로그 : Form
    {
        #region 하이라이트 기능을 위한 변수들
        public Regex 키워드들;
        public Regex 구문분석들;
        public Regex 문제형식들;
        public Regex 정답들;
        public Regex ABC형하이픈들;
        public Regex ABC형보기들;
        public bool _텍스트변경사항자동저장불필요;
        #endregion

        public 찾기다이얼로그(string 첫선택내용)
        {
            InitializeComponent();

            찾을내용.Text = 첫선택내용;
            
            #region 하이라이트 기능을 위한 초기화
            키워드들 = new Regex("<CAKE>|</CAKE>|<B>|</B>|<T>|</T>|<TR>|</TR>|<TBAR></TBAR>|<PAGEEND></PAGEEND>|<Q>|</Q>|<A>|</A>|<A0>|</A0>|<A1>|</A1>|<A2>|</A2>|<A3>|</A3>|<A4>|</A4>|<A5>|</A5>");
            구문분석들 = new Regex("ⓢ|ⓥ|ⓞ|ⓒ|\\(ⓒ\\)|ⓧ|ⓘ|ⓓ|\\(ⓓ\\)|㉨|{|}");
            문제형식들 = new Regex("{지시}|{주제}|{제목}|{속담}|{빈칸}|{요약}|{어법}|{어휘}|{분위기}|{일치}|{흐름}");
            정답들 = new Regex("정답 ①번|정답 ②번|정답 ③번|정답 ④번|정답 ⑤번");
            ABC형하이픈들 = new Regex("…");
            ABC형보기들 = new Regex("\\[|\\]|\\(a\\)|\\(b\\)|\\(c\\)|\\(A\\)|\\(B\\)|\\(C\\)|/");
            #endregion

            if(!찾을내용.Text.Contains("\n"))
                찾을내용.WordWrap = true;
            화면업데이트중지();
            전체화면하이라이트표시();
            화면업데이트재개();
            _텍스트변경사항자동저장불필요 = false;
        }

        #region 버튼
        private void 다음찾기_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;
            bool 정방향인가 = true;

            if (위로.Checked == true)
            {
                정방향인가 = false;
            }
            bool 찾은결과 = 폼1.다음찾기(찾을내용.Text, 정방향인가);
            폼1.Focus();

            if (!찾은결과)
            {
                메시지박스.보여주기("더 이상 찾는 내용이 없습니다.", this);
            }
        }
        private void 취소_Click(object sender, EventArgs e)
        {
            this.DestroyHandle();
        }
        #endregion
        #region 하이라이트
        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

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

            foreach (Match ABC형하이픈 in ABC형하이픈들.Matches(찾을내용.Text))
            {
                찾을내용.Select(ABC형하이픈.Index, ABC형하이픈.Length);
                찾을내용.SelectionColor = Color.Gray;
                찾을내용.SelectionFont = new Font("굴림", 10F, FontStyle.Regular);
            }

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
        private void 선택위치에바꿀말넣고키워드색상입히기(string 바꿀말)
        {
            //
            int 처음선택시작위치 = 찾을내용.SelectionStart;
            찾을내용.SelectedText = 바꿀말;
            int 붙여넣기후선택시작위치 = 찾을내용.SelectionStart;

            찾을내용.SelectionStart = 처음선택시작위치;
            찾을내용.SelectionLength = 붙여넣기후선택시작위치 - 처음선택시작위치;
            선택위치의키워드색상입히기();
            찾을내용.SelectionLength = 0;
            찾을내용.SelectionStart = 붙여넣기후선택시작위치;

        }
        private void 선택위치의키워드색상입히기()
        {
            int 원래선택위치 = 찾을내용.SelectionStart;
            int 원래선택길이 = 찾을내용.SelectionLength;
            string 원래선택내용 = 찾을내용.SelectedText;

            찾을내용.SelectionColor = Color.Black;

            찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Bold);

            foreach (Match ABC형하이픈 in ABC형하이픈들.Matches(원래선택내용))
            {
                찾을내용.Select(원래선택위치 + ABC형하이픈.Index, ABC형하이픈.Length);
                찾을내용.SelectionColor = Color.Gray;
                찾을내용.SelectionFont = new Font("굴림", 10F, FontStyle.Regular);
            }

            foreach (Match ABC형보기 in ABC형보기들.Matches(원래선택내용))
            {
                찾을내용.Select(원래선택위치 + ABC형보기.Index, ABC형보기.Length);
                찾을내용.SelectionColor = Color.MidnightBlue;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Bold);
            }

            foreach (Match 키워드매치 in 키워드들.Matches(원래선택내용))
            {
                찾을내용.Select(원래선택위치 + 키워드매치.Index, 키워드매치.Length);
                찾을내용.SelectionColor = Color.Blue;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }

            foreach (Match 구문분석 in 구문분석들.Matches(원래선택내용))
            {
                찾을내용.Select(원래선택위치 + 구문분석.Index, 구문분석.Length);
                찾을내용.SelectionColor = Color.DarkGreen;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }

            foreach (Match 문제형식 in 문제형식들.Matches(원래선택내용))
            {
                찾을내용.Select(원래선택위치 + 문제형식.Index, 문제형식.Length);
                찾을내용.SelectionColor = Color.OrangeRed;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }

            foreach (Match 정답 in 정답들.Matches(원래선택내용))
            {
                찾을내용.Select(원래선택위치 + 정답.Index, 정답.Length);
                찾을내용.SelectionColor = Color.IndianRed;
                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
            }


            찾을내용.SelectionStart = 원래선택위치;
            찾을내용.SelectionLength = 원래선택길이;
        }
        private void 현재커서근방의키워드색상업데이트()
        {
            int 원래위치 = 0;
            int 원래길이 = 0;
            int 앞의빈칸위치 = -1;
            int 뒤의빈칸위치 = -1;



            화면업데이트중지();

            원래위치 = 찾을내용.SelectionStart;
            원래길이 = 찾을내용.SelectionLength;


            #region 커서 위치 확인
            if (찾을내용.SelectionStart != 0)
            {
                앞의빈칸위치 = 찾을내용.Text.LastIndexOf(" ", 찾을내용.SelectionStart - 1, 찾을내용.SelectionStart) + 1;
                뒤의빈칸위치 = 찾을내용.Text.IndexOf(" ", 찾을내용.SelectionStart);
            }
            else
            {
                앞의빈칸위치 = 0;
                뒤의빈칸위치 = 찾을내용.Text.IndexOf(" ", 0);
            }

            if (뒤의빈칸위치 == -1)
                뒤의빈칸위치 = 찾을내용.TextLength;

            int nPos = GetScrollPos(찾을내용.Handle, (int)ScrollBarType.SbVert);

            // 찾을내용.AutoScrollOffset
            //찾을내용.
            #endregion
            #region 키워드 색상 변경


                찾을내용.Select(앞의빈칸위치, 뒤의빈칸위치 - 앞의빈칸위치);
                //MessageBox.Show(찾을내용.SelectedText);
                찾을내용.SelectionColor = Color.Black;

                찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Bold);

                foreach (Match ABC형하이픈 in ABC형하이픈들.Matches(찾을내용.Text.Substring(앞의빈칸위치, 뒤의빈칸위치 - 앞의빈칸위치)))
                {
                    찾을내용.Select(앞의빈칸위치 + ABC형하이픈.Index, ABC형하이픈.Length);
                    찾을내용.SelectionColor = Color.Gray;
                    찾을내용.SelectionFont = new Font("굴림", 10F, FontStyle.Regular);
                }

                foreach (Match ABC형보기 in ABC형보기들.Matches(찾을내용.Text.Substring(앞의빈칸위치, 뒤의빈칸위치 - 앞의빈칸위치)))
                {
                    찾을내용.Select(앞의빈칸위치 + ABC형보기.Index, ABC형보기.Length);
                    찾을내용.SelectionColor = Color.MidnightBlue;
                    찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Bold);
                }


                foreach (Match 키워드매치 in 키워드들.Matches(찾을내용.Text.Substring(앞의빈칸위치, 뒤의빈칸위치 - 앞의빈칸위치)))
                {
                    찾을내용.Select(앞의빈칸위치 + 키워드매치.Index, 키워드매치.Length);
                    찾을내용.SelectionColor = Color.Blue;
                    찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
                }

                foreach (Match 구문분석 in 구문분석들.Matches(찾을내용.Text.Substring(앞의빈칸위치, 뒤의빈칸위치 - 앞의빈칸위치)))
                {
                    찾을내용.Select(앞의빈칸위치 + 구문분석.Index, 구문분석.Length);
                    찾을내용.SelectionColor = Color.DarkGreen;
                    찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
                }

                foreach (Match 문제형식 in 문제형식들.Matches(찾을내용.Text.Substring(앞의빈칸위치, 뒤의빈칸위치 - 앞의빈칸위치)))
                {
                    찾을내용.Select(앞의빈칸위치 + 문제형식.Index, 문제형식.Length);
                    찾을내용.SelectionColor = Color.OrangeRed;
                    찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
                }

                foreach (Match 정답 in 정답들.Matches(찾을내용.Text.Substring(앞의빈칸위치, 뒤의빈칸위치 - 앞의빈칸위치)))
                {
                    찾을내용.Select(앞의빈칸위치 + 정답.Index, 정답.Length);
                    찾을내용.SelectionColor = Color.IndianRed;
                    찾을내용.SelectionFont = new Font("맑은 고딕", 10F, FontStyle.Regular);
                }

            #endregion

            찾을내용.SelectionStart = 원래위치;
            찾을내용.SelectionLength = 원래길이;

            #region 커서위치 재설정
            nPos <<= 16;
            uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
            SendMessage(찾을내용.Handle, (int)스크롤업데이트메시지.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));

            #endregion

            화면업데이트재개();

        }
        #endregion
        #region 단축키
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);

            //            MessageBox.Show(string.Format("{0}", key));

            switch (key)
            {
                case Keys.Oemplus: if (((keyData & Keys.Shift) != 0) && ((keyData & Keys.Control) != 0)) { 찾을내용.SelectedText = string.Format("㉨{{{0}}}", 찾을내용.SelectedText); return true; } break;

                #region 방향키
                #region Right
                case Keys.Right:
                    if ((keyData & Keys.Shift) == 0)
                    {
                        _텍스트변경사항자동저장불필요 = true;
                        현재커서근방의키워드색상업데이트();
                        _텍스트변경사항자동저장불필요 = false;
                    }
                    break;
                #endregion
                #region Left
                case Keys.Left:
                    if ((keyData & Keys.Shift) == 0)
                    {
                        _텍스트변경사항자동저장불필요 = true;
                        현재커서근방의키워드색상업데이트();
                        _텍스트변경사항자동저장불필요 = false;
                    }
                    break;
                #endregion
                #endregion
                #region Space
                case Keys.Space:
                    _텍스트변경사항자동저장불필요 = true;

                    화면업데이트중지();
                    찾을내용.SelectedText = " ";

                    찾을내용.SelectionStart--;
                    현재커서근방의키워드색상업데이트();
                    찾을내용.SelectionStart++;
                    현재커서근방의키워드색상업데이트();
                    화면업데이트재개();
                    _텍스트변경사항자동저장불필요 = false;
                    return true;
                #endregion
                #region Enter
                case Keys.Enter:

                    break;
                #endregion
                #region BackSpace
                case Keys.Back:
                    화면업데이트중지();
                    _텍스트변경사항자동저장불필요 = true;

                    if (찾을내용.SelectedText != "")
                    {
                        찾을내용.SelectedText = "";

                        현재커서근방의키워드색상업데이트();
                    }
                    else
                    {
                        if (찾을내용.SelectionStart != 0)
                        {
                            찾을내용.SelectionStart--;
                            찾을내용.SelectionLength++;

                            찾을내용.SelectedText = "";
                            현재커서근방의키워드색상업데이트();
                        }
                    }

                    _텍스트변경사항자동저장불필요 = false;
                    화면업데이트재개();
                    return true;
                #endregion

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void 붙여넣기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            선택위치에바꿀말넣고키워드색상입히기(Clipboard.GetText());
        }
        private void 삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _텍스트변경사항자동저장불필요 = true;

            if (찾을내용.SelectedText != "")
            {
                찾을내용.SelectedText = "";

                현재커서근방의키워드색상업데이트();
            }
            else
            {
                if (찾을내용.TextLength != 찾을내용.SelectionStart)
                {
                    찾을내용.SelectionLength++;
                    찾을내용.SelectedText = "";
                    현재커서근방의키워드색상업데이트();
                }
            }

            _텍스트변경사항자동저장불필요 = false;
        }
        private void 실행취소ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            메시지를스태틱으로만들기위한클래스 메시지박스 = new 메시지를스태틱으로만들기위한클래스("찾기에서는 지원하지 않는 기능입니다.");
            메시지박스.Owner = this;
            메시지박스.StartPosition = FormStartPosition.Manual;
            메시지박스.Location = new Point(this.Location.X + 95, this.Location.Y + 90);
            if (메시지박스.ShowDialog() == DialogResult.OK)
            {
                this.Focus();
            }
        }
        #endregion
    }
}
