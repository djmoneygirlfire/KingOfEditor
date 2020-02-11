using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 편집기의_제왕
{
	public partial class 텔레키네시스시스템 : Form
	{
		[DllImport("user32.dll", SetLastError = true)]
		static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

		public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
		public const int VK_LCONTROL = 0xA2; //Left Control key code
		public const int A = 0x41; //A key code
		public const int C = 0x43; //C key code

		public const int V = 0x56; //V key code

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEMOVE = 0x0001;   // 마우스 이동
        private const uint ABSOLUTEMOVE = 0x8000;   // 전역 위치
        private const uint LBUTTONDOWN = 0x0002;   // 왼쪽 마우스 버튼 눌림
        private const uint LBUTTONUP = 0x0004;   // 왼쪽 마우스 버튼 떼어짐

		private int 맨처음만 = 0;

		Timer _1초타이머;
		Timer _전체복사타이머;
		Point _마우스포인트;
		string _시작전클립보드내용;

		public 텔레키네시스시스템()
		{
			InitializeComponent();

			_시작전클립보드내용 = Clipboard.GetText();
		}

		public 텔레키네시스시스템(string 주소)
		{
			InitializeComponent();

			_시작전클립보드내용 = Clipboard.GetText();

			webBrowser1.Navigate(주소);
		}

		void 할일1초뒤(object sender, EventArgs e)
		{
			_1초타이머.Stop();

			_마우스포인트 =	Cursor.Position;

			Cursor.Hide();
			마우스움직임(30000, 30000);

			마우스클릭();
			Cursor.Position = _마우스포인트;
			Cursor.Show();

			keybd_event(VK_LCONTROL, 0x9d, 0, 0);
			keybd_event(A, 0x9e, 0, 0);
			keybd_event(A, 0x9e, KEYEVENTF_KEYUP, 0);

			keybd_event(C, 0x9e, 0, 0);
			keybd_event(C, 0x9e, KEYEVENTF_KEYUP, 0);
			keybd_event(VK_LCONTROL, 0x9d, KEYEVENTF_KEYUP, 0);


			_전체복사타이머 = new System.Windows.Forms.Timer();
			_전체복사타이머.Interval = 1000;
			_전체복사타이머.Tick += new EventHandler(전체복사후);
			_전체복사타이머.Start();



		}

		void 전체복사후(object sender, EventArgs e)
		{
			_전체복사타이머.Stop();

			string 클립보드내용 = Clipboard.GetText();


			if(클립보드내용.Contains("이것을 찾으셨나요?"))
			{
				string 해석결과 = 클립보드내용.Substring(클립보드내용.IndexOf("이것을 찾으셨나요?"));

				해석결과 = 해석결과.Substring(해석결과.IndexOf("\n") + 1);

				if(해석결과.Contains("수정 제안하기"))
				{
					해석결과 = 해석결과.Substring(0, 해석결과.IndexOf("수정 제안하기"));

					if(해석결과.Trim() != "")
						Clipboard.SetText(해석결과.Trim());
					else
						Clipboard.SetText(" ");

					해석결과창 내해석결과 = new 해석결과창(해석결과.Trim());
					내해석결과.ShowDialog();
				}
			}
			else if(클립보드내용.Contains("5000"))
			{
				string 해석결과 = 클립보드내용.Substring(클립보드내용.IndexOf("5000") + 4);

				if(해석결과.Contains("수정 제안하기"))
				{
					해석결과 = 해석결과.Substring(0, 해석결과.IndexOf("수정 제안하기"));

					if(해석결과.Trim() != "")
						Clipboard.SetText(해석결과.Trim());
					else
						Clipboard.SetText(" ");

					해석결과창 내해석결과 = new 해석결과창(해석결과.Trim());
					내해석결과.ShowDialog();
				}

			}
			this.Close();
		}

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			if(맨처음만 == 0)
			{
				맨처음만++;

				/*
				//MessageBox.Show("준비 완료");

				// 준비완료 되면 Ctrl + V를 해준다.

				keybd_event(VK_LCONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);
				keybd_event(V, 0, KEYEVENTF_EXTENDEDKEY, 0);
				keybd_event(V, 0, KEYEVENTF_KEYUP, 0);
				keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);
				*/
				
				_1초타이머 = new System.Windows.Forms.Timer();
				_1초타이머.Interval = 1000;
				_1초타이머.Tick += new EventHandler(할일1초뒤);
				_1초타이머.Start();
			}

		}

        private void 마우스움직임(uint x, uint y)
        {
            mouse_event(MOUSEMOVE|ABSOLUTEMOVE, x, y, 0, 0);
        }

        private void 마우스클릭()
        {
            mouse_event(LBUTTONDOWN, 0, 0, 0, 0);

			//System.Threading.Thread.Sleep(100);

            mouse_event(LBUTTONUP, 0, 0, 0, 0);
        }
	}
}
