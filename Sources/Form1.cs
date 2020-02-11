using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;
using System.Media;
using Concatenation_Waves;
using 변환;
using 구문분석;
using 검색_진화하는;
using System.Timers;
using Microsoft.Win32;

namespace 편집기의_제왕
{
	#region enums
	public enum ScrollBarType : uint
    {
        SbHorz = 0,
        SbVert = 1,
        SbCtl = 2,
        SbBoth = 3
    }
    public enum 스크롤업데이트메시지 : uint
    {
        WM_VSCROLL = 0x0115
    }
    public enum ScrollBarCommands : uint
    {
        SB_THUMBPOSITION = 4
    }
	#endregion

	public partial class Form1 : Form
	{
		public string _편집기의제왕로고 = "⛪ 🕌 🕍 ⛩️ 🌋 🗼 편집기의 제왕, 완벽함으로 가는 여정 Ver 2020.0204 🏟️  🏛️  🏗️  🏝️  🏞️  🏭";

		#region 멤버변수들
		public string _DB루트 = Application.StartupPath + "/txt/";
		public string _IMG루트폴더 = Application.StartupPath + "/img/";
		int _타이틀바_변경한_이전_초 = -1;
		오른쪽클릭메뉴 _오른쪽클릭메뉴;
        해석오른쪽클릭메뉴 _해석오른쪽클릭메뉴;

		//System.Timers.Timer _타이머;
		// 줄간격 조절용
		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref PARAFORMAT lParam);
		const int PFM_SPACEBEFORE = 0x00000040;
		const int PFM_SPACEAFTER = 0x00000080;
		const int PFM_LINESPACING = 0x00000100;
		const int SCF_SELECTION = 1;
		const int EM_SETPARAFORMAT = 1095;

		private string _최근복사한클립보드내용 = "";

		#region 파일경로를 표시하기 위한 변수
		public string _파일경로 = "";
		public string _파일이름 = "";
		public string _현재폴더;
		public string _시작할때열파일;
		
		#endregion
		#region 파일 내용 변경을 표시하기 위한 변수
		public bool _내용변경여부;
		#endregion
		#region 커서를 나타내기 위한 변수들
		public int _현재커서위치;
		public int _선택끝;
		public int _선택시작;
		#endregion
		#region 찾기에 필요한 변수들
		public string _찾은내용;
		#endregion
		#region 하이라이트 기능을 위한 변수들
		public Regex 키워드들;
		public Regex 구문분석들;
		public Regex 문제형식들;
		public Regex 정답들;
		public Regex ABC형보기들;

		#endregion
		#region 동영상용 화면 뷰어
		List<한줄> _본문_여러줄;
        List<한줄> _A1_여러줄;
        List<한줄> _A2_여러줄;
        List<한줄> _A3_여러줄;
        List<한줄> _A4_여러줄;
        List<한줄> _A5_여러줄;


        Bitmap _배경과본문_비트맵;
		Graphics _배경과본문_그래픽;

		Bitmap _비트맵_문제유형;
		Graphics _그래픽_문제유형;

        int _y그림위치 = 0;

        int _이미지높이 = 0;

		public static System.Drawing.Font _해석기본글꼴;
		private int _해석_왼쪽여백 = 40;
		public static System.Drawing.Font _동영상용사전_기본글꼴;
		public static System.Drawing.Font _동영상용사전발음기호_기본글꼴;


		public static SolidBrush _해석해설붓_w;
		public static SolidBrush _해석해설붓_r;
		public static SolidBrush _동영상용사전_기본붓;
        public static System.Drawing.Font _동영상용사전_해설글꼴;
        public static SolidBrush _동영상용사전_해설붓;
        Image _동영상용사전_배경이미지;

        Bitmap _동영상용사전_비트맵;
        Graphics _동영상용사전_그래픽;
        int _현재본문번호 = -1000;
		string _현재어절 = "";
		string _JPG경로 = "";

		//private int _전체너비 = 265; // 동영상용 사전 전체 너비
		private int _전체너비 = 265; // 동영상용 사전 전체 너비

		private int _행간 = 33;
		Bitmap _해석보여주는로봇_비트맵;
		Graphics _해석보여주는로봇_그래픽;
		Image _해석로봇;
		List<string> _문장단위_해석들;

		#endregion

		public 구문자동분석 _구문분석;
        private string _최근구문분석 = "";

		public static 검색 _검색;
		private Point 이전마우스위치;
		private Point 현재마우스위치;

		#region 실행 취소 기능을 위한 변수들
		public int _현재취소용텍스트인덱스;
		public List<string> _실행취소용RTF들;
		public List<int> _실행취소용선택위치들;
		public List<int> _실행취소용선택길이들;
		public List<int> _실행취소용스크롤위치들;
		public bool _텍스트변경사항자동저장불필요;
		public List<float> _실행취소용확대배율들;

		bool _동영상배경사용이처음인지 = true;

		#endregion

		int _CAKE_인덱스;
		int _SUGAR_인덱스;
		public string _헤더;
		public List<string> _CAKE들;

        private int _문제고유번호에사용할값 = 0;

		Dictionary<string, string> _어휘문제목록해시;

		public string _현재_선택한_탭; // 트리뷰에서 항목을 선택할 때, 현재 선택한 탭이 그래픽인 경우, 다시 그려주는 것까지 하도록 합니다.
		#endregion
		public bool _편집시작;
		#region DLLImport
		[DllImport("User32.dll")]
		public extern static int GetScrollPos(IntPtr hWnd, int nBar);
		[DllImport("User32.dll")]
		public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		private const int MOUSEEVENTF_MOVE = 0x0001;
		#endregion
		#region 생성자
		public Form1(string 시작할때열파일)
		{

			_오른쪽클릭메뉴 = new 오른쪽클릭메뉴();
            _해석오른쪽클릭메뉴 = new 해석오른쪽클릭메뉴();
			
            _시작할때열파일 = 시작할때열파일;

			#region 레지스트리에 실행위치를 적습니다.
			string 레지스트리_서브키 = "Software\\KingOfEditor";
			RegistryKey 레지스트리_키 = Registry.CurrentUser.OpenSubKey(레지스트리_서브키, true);

			if (레지스트리_키 == null)
			{
				레지스트리_키 = Registry.CurrentUser.CreateSubKey(레지스트리_서브키);
			}

			string 실행위치 = Application.ExecutablePath;
			레지스트리_키.SetValue("실행위치", 실행위치);

			#endregion


			#region 디자이너 지원을 위한 초기화 (자동생성코드)
			InitializeComponent();
			#endregion
			#region 파일 내용 변경을 위한 초기화
			_내용변경여부 = false;
			#endregion
			#region 드래그앤드랍_이벤트처리 초기화
			본문.DragDrop += new DragEventHandler(드래그앤드랍_이벤트처리기);
			본문.AllowDrop = true;
			본문.KeyDown += new KeyEventHandler(키보드_특수키이벤트처리기);
			본문.KeyPress += new KeyPressEventHandler(키보드_이벤트처리기2);
			#endregion
			#region 커서를 나타내기 위한 초기화
			_현재커서위치 = -1;
			_선택끝 = -1;
			_선택시작 = -1;
			#endregion
			#region 찾기를 나타내기 위한 초기화
			_찾은내용 = "";
			#endregion
			#region 하이라이트 기능을 위한 초기화
			키워드들 = new Regex("<CAKE>|</CAKE>|<B>|</B>|<T>|</T>|<TR>|</TR>|<TBAR></TBAR>|<PAGEEND></PAGEEND>|<Q>|</Q>|<A>|</A>|<A0>|</A0>|<A1>|</A1>|<A2>|</A2>|<A3>|</A3>|<A4>|</A4>|<A5>|</A5>", RegexOptions.Compiled);
			구문분석들 = new Regex("ⓢ|ⓥ|ⓞ|ⓒ|\\(ⓒ\\)|ⓧ|ⓘ|ⓓ|\\(ⓓ\\)|㉨|{|}", RegexOptions.Compiled);
			문제형식들 = new Regex("{지시}|{주제}|{/주제}|{제목}|{속담}|{빈칸}|{/빈칸}|{요약}|{중요}|{어법}|{어휘}|{어법:|{어휘:|:}|{분위기}|{일치}|{흐름}", RegexOptions.Compiled);
			정답들 = new Regex("정답|①번|②번|③번|④번|⑤번", RegexOptions.Compiled);
			ABC형보기들 = new Regex("\\[|\\]|\\(a\\)|\\(b\\)|\\(c\\)|\\(A\\)|\\(B\\)|\\(C\\)|/", RegexOptions.Compiled);
			#endregion
			#region 실행 취소 기능을 위한 초기화
			_실행취소용RTF들 = new List<string>();
			_실행취소용RTF들.Add("");
			_현재취소용텍스트인덱스 = 0;

			_실행취소용선택위치들 = new List<int>();
			_실행취소용선택위치들.Add(0);

			_실행취소용선택길이들 = new List<int>();
			_실행취소용선택길이들.Add(0);

			_실행취소용스크롤위치들 = new List<int>();
			_실행취소용스크롤위치들.Add(0);

			_실행취소용확대배율들 = new List<float>();
			_실행취소용확대배율들.Add(1.0f);

			_텍스트변경사항자동저장불필요 = false;
			#endregion

			

			_검색 = new 검색();
			이전마우스위치 = new Point(0, 0);
			현재마우스위치 = new Point(0, 0);

			_구문분석 = new 구문자동분석();

			_본문_여러줄 = new List<한줄>();
            _A1_여러줄 = new List<한줄>();
            _A2_여러줄 = new List<한줄>();
            _A3_여러줄 = new List<한줄>();
            _A4_여러줄 = new List<한줄>();
            _A5_여러줄 = new List<한줄>();


            _CAKE들 = new List<string>();

			_CAKE_인덱스 = -2;
			_SUGAR_인덱스 = -2;

			_편집시작 = false;


			_해석기본글꼴 = new System.Drawing.Font("Youth", 14f);
			_해석해설붓_w = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));
			_해석해설붓_r = new SolidBrush(System.Drawing.Color.FromArgb(255, 80, 80));

			_배경과본문_비트맵 = new Bitmap(1, 1);
			_배경과본문_그래픽 = Graphics.FromImage(_배경과본문_비트맵);

			#region 동영상용 화면 사전의 반의어 보여주기
			_어휘문제목록해시 = new Dictionary<string, string>();
			List<string> 어휘문제목록 = new List<string>();
			변환.Ansi파일.문자열들로(_DB루트 + "voca.qst", ref 어휘문제목록);

			foreach (string 어휘문제목록_항목 in 어휘문제목록)
			{
				string[] 어휘문제목록_항목_배열 = 어휘문제목록_항목.Split(',');

				if (어휘문제목록_항목_배열.Length > 1)
				{
					if (!_어휘문제목록해시.ContainsKey(어휘문제목록_항목_배열[0].ToLower()))
						_어휘문제목록해시.Add(어휘문제목록_항목_배열[0].ToLower(), 어휘문제목록_항목_배열[1].ToLower());

					if (!_어휘문제목록해시.ContainsKey(어휘문제목록_항목_배열[1].ToLower()))
						_어휘문제목록해시.Add(어휘문제목록_항목_배열[1].ToLower(), 어휘문제목록_항목_배열[0].ToLower());
				}
			}
			#endregion


			//_타이머 = new System.Timers.Timer();
			//_타이머.Interval = 200; // 1/5초
			//_타이머.Elapsed += new ElapsedEventHandler(타이머시간지남);
			//_타이머.Start();

			해석.SelectionFont = new System.Drawing.Font("나눔바른고딕", 12F, FontStyle.Regular);

			줄간격_조절(2, 0);

			Text = _편집기의제왕로고;
		}

		~Form1()
		{
			//_타이머.Enabled = false;
		}

		// 리치텍스트박스의 줄 간격을 조정해보려고 함.
		//0
		//
		//Single spacing.The dyLineSpacing member is ignored.
		//1
		//
		//One-and-a-half spacing. The dyLineSpacing member is ignored.
		//2
		//Double spacing. The dyLineSpacing member is ignored.
		//3
		//The dyLineSpacing member specifies the spacing from one line to the next, in twips.However, if dyLineSpacing specifies a value that is less than single spacing, the control displays single-spaced text.
		//4
		//
		//The dyLineSpacing member specifies the spacing from one line to the next, in twips.The control uses the exact spacing specified, even if dyLineSpacing specifies a value that is less than single spacing.
		//5
		//
		//The value of dyLineSpacing / 20 is the spacing, in lines, from one line to the next. Thus, setting dyLineSpacing to 20 produces single-spaced text, 40 is double spaced, 60 is triple spaced, and so on.
		private void 줄간격_조절(byte rule, int space)
		{
			PARAFORMAT fmt = new PARAFORMAT();
			fmt.cbSize = Marshal.SizeOf(fmt);
			fmt.dwMask = PFM_LINESPACING;
			fmt.dyLineSpacing = space;
			fmt.bLineSpacingRule = rule;

			int 선택위치저장 = 본문.SelectionStart;
			int 선택길이저장 = 본문.SelectionLength;

			본문.SelectAll();			SendMessage(new HandleRef(본문, 본문.Handle), EM_SETPARAFORMAT,	 SCF_SELECTION,	 ref fmt);
			해석.SelectAll();			SendMessage(new HandleRef(해석, 해석.Handle), EM_SETPARAFORMAT,	 SCF_SELECTION,	 ref fmt);

			본문.SelectionStart = 선택위치저장;
			본문.SelectionLength = 선택길이저장;
		}


		void 타이머시간지남(object sender, ElapsedEventArgs e)
		{


		}

		void 타이틀바텍스트애니메이션()
		{
			타이틀바텍스트애니메이션("");
		}

		void 타이틀바텍스트애니메이션(string 추가할말)
		{
			
			if (_타이틀바_변경한_이전_초 == DateTime.Now.Second) return;

			_타이틀바_변경한_이전_초 = DateTime.Now.Second;

			if(Text.Contains("여정")) Text = Text.Replace("여정", "여행");
			else if(Text.Contains("여행")) Text = Text.Replace("여행", "열정");
			else if(Text.Contains("열정")) Text = Text.Replace("열정", "여정");

			if(Text.Contains("완벽함으로")) Text = Text.Replace("완벽함으로", "놀라움으로");
			else if(Text.Contains("놀라움으로")) Text = Text.Replace("놀라움으로", "신비함으로");
			else if(Text.Contains("신비함으로")) Text = Text.Replace("신비함으로", "기이함으로");
			else if(Text.Contains("기이함으로")) Text = Text.Replace("기이함으로", "완벽함으로");

			if(Text.Contains("🌋")) Text = Text.Replace("🌋", "🗻");
			else Text = Text.Replace("🗻", "🌋");

			if (Text.Contains("|")) Text = Text.Substring(0, Text.IndexOf("|"));

			if(추가할말 != "")
				Text += "|" + 추가할말;
		}

		// 깜빡거리는 것을 막아준다고 함
		protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        public void 우클릭_선택영역_PIXABAY열기(){	string s = 선택();								Process.Start("Chrome.exe", string.Format("https://pixabay.com/ko/images/search/{0}/", s));}
		public void 우클릭_선택영역_동의어찾기(){	string s = 선택();	s = s.Replace(" ", "%20");	Process.Start("Chrome.exe", string.Format("https://www.thesaurus.com/browse/{0}", s));}
		public void 우클릭_선택영역_위키사전(){		string s = 선택();	s = s.Replace(" ", "_");	Process.Start("Chrome.exe", string.Format("https://en.wiktionary.org/wiki/{0}", s));}
		public void 우클릭_선택영역_위키백과(){		string s = 선택();	s = s.Replace(" ", "_");	Process.Start("Chrome.exe", string.Format("https://en.wikipedia.org/wiki/{0}", s));}

		private void Form1_Load(object sender, EventArgs e)
		{
            if(_시작할때열파일 == "0")
            {
                TreeNode 전체노드 = new TreeNode("새 문서");

                TreeNode CAKE노드 = new TreeNode("항목 1번");

                전체노드.Nodes.Add(CAKE노드);

                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(전체노드);

                _CAKE들.Add(현재양식내용().Replace("\n", "\r\n").Replace("\r\r", "\r"));


            }
			else if (_시작할때열파일 != "")
			{
				열기(_시작할때열파일);
			}
			else
			{
				TreeNode 전체노드 = new TreeNode("새 문서");

				TreeNode CAKE노드 = new TreeNode("항목 1번");

				전체노드.Nodes.Add(CAKE노드);

				treeView1.Nodes.Clear();
				treeView1.Nodes.Add(전체노드);

				_CAKE들.Add(현재양식내용().Replace("\n", "\r\n").Replace("\r\r", "\r"));


            }
		}
		#endregion
		#region 단축키 설정 코드 (그대로 복사해서 쓰면 됨)
		//이게 단축키가 되는 거임
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{

			if(제목.Focused)
			{
				Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
				switch (key)
				{
					#region Tab
					case Keys.Tab:
						질문.Focus();

						return true;
						#endregion
				}
				return base.ProcessCmdKey(ref msg, keyData);

			}
			else if (질문.Focused)
			{
				Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
				switch (key)
				{
					#region Tab
					case Keys.Tab:
						본문.Focus();

						return true;
						#endregion
				}
				return base.ProcessCmdKey(ref msg, keyData);
			}
			#region 본문_Focused

			else if (본문.Focused)
			{
				Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);

				switch (key)
				{
					case Keys.Oemplus: if (((keyData & Keys.Shift) != 0) && ((keyData & Keys.Control) != 0)) { 본문.SelectedText = string.Format("㉨{{{0}}}", 본문.SelectedText); return true; } break;

					#region 방향키
					#region Right
					case Keys.Right:

						if (((keyData & Keys.Shift) != 0) && ((keyData & Keys.Control) != 0))
						{
							if (본문.SelectedText == "")
							{
								_현재커서위치 = 본문.SelectionStart;
								_선택시작 = _현재커서위치;
								_선택끝 = _현재커서위치;

								int 다음커서위치 = 유틸_오른쪽커서위치찾기(_현재커서위치);

								if (다음커서위치 != -1)
								{
									_선택끝 = 다음커서위치;
									본문.SelectionLength = _선택끝 - _선택시작;
									_현재커서위치 = 다음커서위치;
								}


							}
							else
							{
								if (_선택끝 == _현재커서위치) // 전통적으로 오른쪽으로 나가는 경우
								{
									_선택시작 = 본문.SelectionStart;

									int 다음커서위치 = 유틸_오른쪽커서위치찾기(_현재커서위치);

									if (다음커서위치 != -1)
									{
										_선택끝 = 다음커서위치;
										본문.SelectionLength = _선택끝 - _선택시작;
										_현재커서위치 = 다음커서위치;
									}
								}
								else
								{
									int 다음커서위치 = 유틸_오른쪽커서위치찾기(_현재커서위치);

									if (_선택끝 < 다음커서위치)
									{
										_선택시작 = _선택끝;
										_현재커서위치 = _선택끝;
										본문.SelectionLength = 0;
									}
									else if (다음커서위치 != -1)
									{
										_선택시작 = 다음커서위치;
										본문.SelectionStart = 다음커서위치;
										본문.SelectionLength = _선택끝 - _선택시작;
										_현재커서위치 = 다음커서위치;
									}
								}
							}
							return true;
						}

						if ((keyData & Keys.Shift) != 0)
						{
							if (본문.SelectedText == "")
							{
								_현재커서위치 = 본문.SelectionStart;
								_선택시작 = _현재커서위치;
								_선택끝 = _현재커서위치;

								if (_현재커서위치 != 본문.TextLength)
								{
									본문.SelectionLength = 1;
									_선택끝 = _선택시작 + 1;
									_현재커서위치 = _선택끝;
								}
							}
							else
							{
								if (_선택끝 == _현재커서위치) // 전통적으로 오른쪽으로 나가는 경우
								{
									if (_현재커서위치 != 본문.TextLength)
									{
										본문.SelectionLength++;
										_선택끝++;
										_현재커서위치 = _선택끝;
									}
								}
								else // 왼쪽으로 거꾸로 나가는 경우
								{
									본문.SelectionStart++;
									본문.SelectionLength--;
									_선택시작++;
									_현재커서위치 = _선택시작;
								}
							}
							return true;
						}

						_텍스트변경사항자동저장불필요 = true;
						현재커서근방의키워드색상업데이트();
						_텍스트변경사항자동저장불필요 = false;
						break;
					#endregion
					#region Left
					case Keys.Left:
						if (((keyData & Keys.Shift) != 0) && ((keyData & Keys.Control) != 0))
						{
							if (본문.SelectedText == "")
							{
								_현재커서위치 = 본문.SelectionStart;
								_선택시작 = _현재커서위치;
								_선택끝 = _현재커서위치;

								int 이전커서위치 = 유틸_왼쪽커서위치찾기(_현재커서위치);

								if (이전커서위치 != 0)
								{
									_선택시작 = 이전커서위치;

									본문.SelectionStart = _선택시작;
									본문.SelectionLength = _선택끝 - _선택시작;
									_현재커서위치 = 이전커서위치;
								}
							}
							else
							{
								if (_선택시작 == _현재커서위치) // 왼쪽키를 눌러서 이전으로 가는 경우
								{

									int 이전커서위치 = 유틸_왼쪽커서위치찾기(_현재커서위치);

									if (이전커서위치 != 0)
									{
										_선택시작 = 이전커서위치;
										본문.SelectionStart = _선택시작;
										본문.SelectionLength = _선택끝 - _선택시작;
										_현재커서위치 = 이전커서위치;
									}

								}
								else // 왼쪽키를 눌러서 이전으로 갔다가 되돌아 오는 경우
								{
									int 이전커서위치 = 유틸_오른쪽에서_되돌아온_왼쪽커서위치찾기(_현재커서위치);

									if (이전커서위치 < _선택시작)
									{
										_선택끝 = _선택시작;
										_현재커서위치 = _선택시작;
										본문.SelectionLength = 0;
									}
									else if (이전커서위치 != 0)
									{
										_선택끝 = 이전커서위치;

										본문.SelectionLength = _선택끝 - _선택시작;
										_현재커서위치 = 이전커서위치;
									}
								}
							}
							return true;
						}

						if ((keyData & Keys.Shift) != 0)
						{
							if (본문.SelectedText == "")
							{
								_현재커서위치 = 본문.SelectionStart;
								_선택시작 = _현재커서위치;
								_선택끝 = _현재커서위치;

								if (_현재커서위치 != 0)
								{
									본문.SelectionStart--;
									본문.SelectionLength = 1;
									_선택시작--;
									_현재커서위치 = _선택시작;
								}
							}
							else
							{
								if (_선택끝 == _현재커서위치)
								{
									본문.SelectionLength--;
									_선택끝--;
									_현재커서위치 = _선택끝;
								}
								else
								{
									if (_현재커서위치 != 0)
									{
										본문.SelectionStart--;
										본문.SelectionLength++;
										_선택시작--;
										_현재커서위치 = _선택시작;
									}
								}
							}
							return true;
						}
						_텍스트변경사항자동저장불필요 = true;
						현재커서근방의키워드색상업데이트();
						_텍스트변경사항자동저장불필요 = false;

						break;
					#endregion
					#endregion

					#region Space
					case Keys.Space:
						if (((keyData & Keys.Control) != 0) && ((keyData & Keys.Alt) != 0))
						{
                            if (본문.Focused)
                            {
                                현재내용을_실행취소용_클립보드에_저장();
                                _텍스트변경사항자동저장불필요 = true;

                                int 현재커서위치 = 본문.SelectionStart;
                                int 앞의개행문자위치 = 0;
                                if (현재커서위치 != 0) 앞의개행문자위치 = 본문.Text.LastIndexOf("\n", 현재커서위치 - 1, 현재커서위치);

                                if (현재커서위치 == 0)
                                    본문.SelectedText = "<TBAR></TBAR>";
                                else if (현재커서위치 != 앞의개행문자위치 + 1)
                                    본문.SelectedText = "<TBAR></TBAR>";
                                else
                                    본문.SelectedText = "<TBAR></TBAR>";

                                현재커서근방의키워드색상업데이트();

                                _텍스트변경사항자동저장불필요 = false;
                                현재내용을_실행취소용_클립보드에_저장();

                            }
                            else if(해석.Focused)
                            {
                                해석.SelectedText = "<TBAR></TBAR>";
                            }

							return true;
						}
						else
						{
							현재내용을_실행취소용_클립보드에_저장();
							_텍스트변경사항자동저장불필요 = true;

							화면업데이트중지();
							본문.SelectedText = " ";

							본문.SelectionStart--;
							현재커서근방의키워드색상업데이트();
							본문.SelectionStart++;
							현재커서근방의키워드색상업데이트();
							화면업데이트재개();
							_텍스트변경사항자동저장불필요 = false;
							현재내용을_실행취소용_클립보드에_저장();
							return true;
						}
					#endregion
					#region Enter
					case Keys.Enter:
						if (((keyData & Keys.Shift) != 0) && ((keyData & Keys.Alt) != 0))
						{
							메뉴_필터_선택부분의엔터제거();

							return true;
						}
						현재내용을_실행취소용_클립보드에_저장();
						break;
					#endregion
					#region "]" 격자 입히기
					case Keys.OemCloseBrackets:
						if ((keyData & Keys.Control) != 0)
						{
							선택위치에대괄호입히기();
							return true;
						}
						break;
					#endregion
					#region BackSpace
					case Keys.Back:
						화면업데이트중지();
						현재내용을_실행취소용_클립보드에_저장();
						_텍스트변경사항자동저장불필요 = true;

						if (본문.SelectedText != "")
						{
							본문.SelectedText = "";

							현재커서근방의키워드색상업데이트();
						}
						else
						{
							if (본문.SelectionStart != 0)
							{
								본문.SelectionStart--;
								본문.SelectionLength++;

								본문.SelectedText = "";
								현재커서근방의키워드색상업데이트();
							}
						}

						_텍스트변경사항자동저장불필요 = false;
						현재내용을_실행취소용_클립보드에_저장();
						화면업데이트재개();
						return true;
						#endregion

					#region Tab
					case Keys.Tab:
						ABC.Focus();
						return true;
					#endregion
				}
				return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 해석_Focused
			else if (해석.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {

					#region Tab
					case Keys.Tab:
						힌트.Focus();
						return true;
					#endregion

					#region Enter

					case Keys.Enter:
                        if (((keyData & Keys.Shift) != 0) && ((keyData & Keys.Alt) != 0))
                        {
                            메뉴_필터_선택부분의엔터제거();

                            return true;
                        }
                        return base.ProcessCmdKey(ref msg, keyData);

						#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);

            }

			#endregion
			#region ABC_FOCUSED
			else if (ABC.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						보기1Text.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 보기1Text
			else if (보기1Text.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						보기2Text.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 보기2Text
			else if (보기2Text.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						보기3Text.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 보기3Text
			else if (보기3Text.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						보기4Text.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 보기4Text
			else if (보기4Text.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						보기5Text.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 보기5Text
			else if (보기5Text.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						주관식정답.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 해석
			else if (해석.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						힌트.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 해설
			else if (힌트.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						중요어휘.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 중요어휘
			else if (중요어휘.Focused)
            {
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
                switch (key)
                {
					#region Tab
					case Keys.Tab:
						제목.Focus();
						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			#region 사전편집창 표제어
			else if(사전페이지_사전표제어.Focused)
			{
                Keys key = keyData & ~(Keys.Shift | Keys.Control | Keys.Alt);
				switch (key)
				{
					#region Enter
					case Keys.Enter:
					    사전페이지_사전의미.Text = _검색.영한사전_문장부호제거(사전페이지_사전표제어.Text);
			            사전페이지_사전발음기호.Text = _검색.영한발음기호사전(사전페이지_사전표제어.Text);

						return true;
					#endregion
				}
                return base.ProcessCmdKey(ref msg, keyData);
			}
			#endregion
			else
				return base.ProcessCmdKey(ref msg, keyData);

		}
		#endregion
		#region 키보드, 드래그앤 드랍 이벤트 처리

		void 키보드_이벤트처리기2(object sender, KeyPressEventArgs e)
		{
			if (_내용변경여부 == false && Text != "편집기의 제왕")
				Text += " *";

			_내용변경여부 = true;
		}

		private void 전체화면하이라이트표시()
		{

		}

		private void 전체텍스트에서_선택단어하이라이트(string 전체텍스트, string 선택단어)
		{
			for (int i = 0; i < 전체텍스트.Length; i++)
			{
				int 찾은위치 = 전체텍스트.IndexOf(선택단어, i);

				if (찾은위치 != -1)
				{
					본문.Select(찾은위치, 선택단어.Length);
					본문.SelectionColor = System.Drawing.Color.Blue;

					i = 찾은위치 + 선택단어.Length;
				}
				else
					break;
			}
		}

		void 키보드_특수키이벤트처리기(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) // || (e.KeyCode == Keys.ShiftKey))
			{
				if (_내용변경여부 == false && Text != "편집기의 제왕")	Text += " *";

				_내용변경여부 = true;
			}
			//throw new NotImplementedException();
		}

		private void 실행취소용리스트전체삭제()
		{
			_실행취소용RTF들.Clear();
			_실행취소용선택위치들.Clear();
			_실행취소용선택길이들.Clear();
			_실행취소용스크롤위치들.Clear();
			_실행취소용확대배율들.Clear();
		}

		private void 케이크표시하기_고유번호제외(string 케이크입력값)
		{
			제목.Text = 변환.문자열.B태그내용(케이크입력값);
			질문.Text = 변환.문자열.Q태그내용(케이크입력값);
//			고유번호.Text = 변환.문자열.문제고유번호(케이크입력값);
			본문.Text = "";	선택위치에바꿀말넣고키워드색상입히기(변환.문자열.T태그내용(케이크입력값));
			ABC.Text = 변환.문자열.A0태그내용(케이크입력값);
			보기1Text.Text = 변환.문자열.A1태그내용(케이크입력값);
			보기2Text.Text = 변환.문자열.A2태그내용(케이크입력값);
			보기3Text.Text = 변환.문자열.A3태그내용(케이크입력값);
			보기4Text.Text = 변환.문자열.A4태그내용(케이크입력값);
			보기5Text.Text = 변환.문자열.A5태그내용(케이크입력값);


			보기1.Checked = false;
			보기2.Checked = false;
			보기3.Checked = false;
			보기4.Checked = false;
			보기5.Checked = false;
			주관식정답.Text = "";

			string 정답 = 변환.문자열.정답추출(케이크입력값);
			if (정답 == "1") 보기1.Checked = true; 
			else if (정답 == "2") 보기2.Checked = true;
			else if (정답 == "3") 보기3.Checked = true;
			else if (정답 == "4") 보기4.Checked = true;
			else if (정답 == "5") 보기5.Checked = true;
			else 주관식정답.Text = 정답;

			해석.Text = 변환.문자열.해석추출(케이크입력값);
			해석.SelectAll();
			해석.SelectionFont = new System.Drawing.Font("나눔바른고딕", 12F, FontStyle.Regular);
			해석.Select(0, 0);


			힌트.Text = 변환.문자열.해설추출(케이크입력값);
			중요어휘.Text = 변환.문자열.중요어휘추출(케이크입력값);
		}
		private void 케이크표시하기(string 케이크입력값)
		{
			제목.Text = 변환.문자열.B태그내용(케이크입력값);
			질문.Text = 변환.문자열.Q태그내용(케이크입력값);
			고유번호.Text = 변환.문자열.문제고유번호(케이크입력값);
			본문.Text = "";	선택위치에바꿀말넣고키워드색상입히기(변환.문자열.T태그내용(케이크입력값));
			ABC.Text = 변환.문자열.A0태그내용(케이크입력값);
			보기1Text.Text = 변환.문자열.A1태그내용(케이크입력값);
			보기2Text.Text = 변환.문자열.A2태그내용(케이크입력값);
			보기3Text.Text = 변환.문자열.A3태그내용(케이크입력값);
			보기4Text.Text = 변환.문자열.A4태그내용(케이크입력값);
			보기5Text.Text = 변환.문자열.A5태그내용(케이크입력값);


			보기1.Checked = false;
			보기2.Checked = false;
			보기3.Checked = false;
			보기4.Checked = false;
			보기5.Checked = false;
			주관식정답.Text = "";

			string 정답 = 변환.문자열.정답추출(케이크입력값);
			if (정답 == "1") 보기1.Checked = true; 
			else if (정답 == "2") 보기2.Checked = true;
			else if (정답 == "3") 보기3.Checked = true;
			else if (정답 == "4") 보기4.Checked = true;
			else if (정답 == "5") 보기5.Checked = true;
			else 주관식정답.Text = 정답;

			해석.Text = 변환.문자열.해석추출(케이크입력값);
			해석.SelectAll();
			해석.SelectionFont = new System.Drawing.Font("나눔바른고딕", 12F, FontStyle.Regular);
			해석.Select(0, 0);

			힌트.Text = 변환.문자열.해설추출(케이크입력값);
			보기1_해설.Text = 변환.문자열.보기1_해설추출(케이크입력값);
			보기2_해설.Text = 변환.문자열.보기2_해설추출(케이크입력값);
			보기3_해설.Text = 변환.문자열.보기3_해설추출(케이크입력값);
			보기4_해설.Text = 변환.문자열.보기4_해설추출(케이크입력값);
			보기5_해설.Text = 변환.문자열.보기5_해설추출(케이크입력값);
			주관식_해설.Text = 변환.문자열.주관식_해설추출(케이크입력값);


			중요어휘.Text = 변환.문자열.중요어휘추출(케이크입력값);

			// 문서정보 부분

			주제.Text = 변환.문자열.주제추출(케이크입력값);
			변형지문.Text = 변환.문자열.변형지문추출(케이크입력값);
			변형지문해석.Text = 변환.문자열.변형지문해석추출(케이크입력값);
		}

		private void 헤더라디오버튼체크()
		{
			// 우선 모두 초기화
			교과서라디오버튼.Checked = false;
			독해집라디오버튼.Checked = false;
			모의고사라디오버튼.Checked = false;
			수능라디오버튼.Checked = false;
			어법연습문제라디오버튼.Checked = false;

			일반라디오버튼.Checked = false;
			고3라디오버튼.Checked = false;
			고2고1라디오버튼.Checked = false;
			중3라디오버튼.Checked = false;
			중2라디오버튼.Checked = false;
			중1라디오버튼.Checked = false;
			초등라디오버튼.Checked = false;

			// 헤더에 따라서 버튼 체크
			if(_헤더.Contains("교과서")) 교과서라디오버튼.Checked = true;
			else if(_헤더.Contains("독해집")) 독해집라디오버튼.Checked = true;
			else if(_헤더.Contains("모의고사")) 모의고사라디오버튼.Checked = true;
			else if(_헤더.Contains("수능")) 수능라디오버튼.Checked = true;
			else if(_헤더.Contains("어법연습문제")) 어법연습문제라디오버튼.Checked = true;


			if(_헤더.Contains("일반")) 일반라디오버튼.Checked = true;
			else if(_헤더.Contains("고3")) 고3라디오버튼.Checked = true;
			else if(_헤더.Contains("고2")) 고2고1라디오버튼.Checked = true;
			else if(_헤더.Contains("고1")) 고2고1라디오버튼.Checked = true;
			else if(_헤더.Contains("중3")) 중3라디오버튼.Checked = true;
			else if(_헤더.Contains("중2")) 중2라디오버튼.Checked = true;
			else if(_헤더.Contains("중1")) 중1라디오버튼.Checked = true;
			else if(_헤더.Contains("초등")) 초등라디오버튼.Checked = true;


		}

		private void 열기_파일내용으로(string 파일내용, string 파일이름, bool 새롭게열기)
		{
			파일내용 = 파일내용.Replace("“", "\"");
			파일내용 = 파일내용.Replace("”", "\"");
			파일내용 = 파일내용.Replace("’", "\'");
			파일내용 = 파일내용.Replace("‘", "\'");
			

			_헤더 = 변환.문자열.헤더추출(파일내용);
			헤더라디오버튼체크();

			변환.문자열.CAKE들로(파일내용, ref _CAKE들);

			if (_CAKE들.Count != 0 && 새롭게열기)
			{
				string CAKE0만 = "";
				string SUGAR0만 = "";

				변환.문자열.SUGAR추출(_CAKE들[0], ref CAKE0만, ref SUGAR0만);

				케이크표시하기(CAKE0만);
			}


			if (파일이름.Trim() == "")
				파일이름 = "파일을 저장하세요.";

			TreeNode 전체노드 = new TreeNode(파일이름);


            for (int i = 0; i < _CAKE들.Count(); i++)
			{
				string 현재CAKE = _CAKE들[i];

				string CAKE만 = "";
				string SUGAR만 = "";

				변환.문자열.SUGAR추출(현재CAKE, ref CAKE만, ref SUGAR만);

				string Tree제목 = "";
				if (변환.문자열.B태그내용(CAKE만) != "") Tree제목 = 변환.문자열.B태그내용(CAKE만).너비(180);
				else if (변환.문자열.Q태그내용(CAKE만) != "") Tree제목 = 변환.문자열.Q태그내용(CAKE만).너비(180);
				else Tree제목 = "항목 " + (i + 1).ToString() + "번";

				TreeNode CAKE노드 = new TreeNode(Tree제목);

                List<string> SUGAR들 = new List<string>();

				변환.문자열.CAKE들로(SUGAR만, ref SUGAR들);

				for (int j = 0; j < SUGAR들.Count(); j++)
				{
					string 현재SUGAR = SUGAR들[j];

					if (변환.문자열.B태그내용(현재SUGAR).Trim() != "") Tree제목 = 변환.문자열.B태그내용(현재SUGAR).너비(180);
					else if (변환.문자열.Q태그내용(현재SUGAR).Trim() != "") Tree제목 = 변환.문자열.Q태그내용(현재SUGAR).너비(180);
					else Tree제목 = "항목 " + (j + 1).ToString() + "번";


					TreeNode SUGAR노드 = new TreeNode(Tree제목);
					CAKE노드.Nodes.Add(SUGAR노드);
				}



				전체노드.Nodes.Add(CAKE노드);
			}

			treeView1.Nodes.Clear();
			treeView1.Nodes.Add(전체노드);

			_현재취소용텍스트인덱스 = 0;
			_실행취소용RTF들.Add(본문.Rtf);
			_실행취소용선택위치들.Add(0);
			_실행취소용선택길이들.Add(0);
			_실행취소용스크롤위치들.Add(0);
			_실행취소용확대배율들.Add(1.0f);

		}

		private void 열기(string 파일경로)
		{
			_편집시작 = false;
			_CAKE_인덱스 = 0; // -2로 되어있는 것을 0으로 바꿨다. 무슨 문제가 생길지 알 수 없다.
			_SUGAR_인덱스 = -2;

			화면업데이트중지();
			_텍스트변경사항자동저장불필요 = true;

			실행취소용리스트전체삭제();

			_파일경로 = 파일경로;
			_현재폴더 = Path.GetDirectoryName(_파일경로);
			_파일이름 = Path.GetFileName(_파일경로);

			열기_파일내용으로(변환.텍스트파일.문자열로(_파일경로), _파일이름, true);

			Text = _파일경로 + _편집기의제왕로고;

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();

			화면업데이트재개();

		}

		private void 드래그앤드랍_이벤트처리기(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] 파일경로들 = (string[])e.Data.GetData(DataFormats.FileDrop);

				열기(파일경로들[0]);
            }
		}

		#endregion
		#region 하이라이트
		public void 화면업데이트중지()
		{
			Message 업데이트중지 = Message.Create(본문.Handle, 0x000B, IntPtr.Zero, IntPtr.Zero);
			NativeWindow window = NativeWindow.FromHandle(본문.Handle);
			window.DefWndProc(ref 업데이트중지);
		}
		public void 화면업데이트재개()
		{
			IntPtr wparam = new IntPtr(1);
			Message 업데이트재개 = Message.Create(본문.Handle, 0x000B, wparam, IntPtr.Zero);
			NativeWindow window = NativeWindow.FromHandle(본문.Handle);

			window.DefWndProc(ref 업데이트재개);

			본문.Invalidate();
		}
		private void 현재내용을_실행취소용_클립보드에_저장()
		{
			// 만약 새롭게 글자를 썼다든가 했는데, 실행취소를 하다가 글자를 쓰는 경우,
			// 남겨둔 실행취소용텍스트를 지울 필요가 있다.
			// 0 바뀐내용
			// 1 바뀐내용
			// 2 바뀐내용
			// 3 바뀐내용 <-실행취소로 돌아온 곳
			// 4 바뀐내용 <-삭제해야 할 내용

			// 남겨진 미래를 지우는 부분
			for (int i = _실행취소용RTF들.Count - 1; i > _현재취소용텍스트인덱스; i--)
			{
				_실행취소용RTF들.RemoveAt(_실행취소용RTF들.Count - 1);
				_실행취소용선택위치들.RemoveAt(_실행취소용선택위치들.Count - 1);
				_실행취소용선택길이들.RemoveAt(_실행취소용선택길이들.Count - 1);
				_실행취소용스크롤위치들.RemoveAt(_실행취소용스크롤위치들.Count - 1);
				_실행취소용확대배율들.RemoveAt(_실행취소용확대배율들.Count - 1);
			}

			int nPos = GetScrollPos(본문.Handle, (int)ScrollBarType.SbVert);
			//Point p = richTextBox1.GetPositionFromCharIndex(0);



			if (_실행취소용RTF들.Count > 0)
			{
				if ((_실행취소용RTF들[_실행취소용RTF들.Count - 1] != 본문.Rtf)
					|| (_실행취소용선택위치들[_실행취소용선택위치들.Count - 1] != 본문.SelectionStart)
					|| (_실행취소용선택길이들[_실행취소용선택길이들.Count - 1] != 본문.SelectionLength))
				{
					_실행취소용RTF들.Add(본문.Rtf);
					_실행취소용선택위치들.Add(본문.SelectionStart);
					_실행취소용선택길이들.Add(본문.SelectionLength);
					_실행취소용스크롤위치들.Add(nPos);
					_실행취소용확대배율들.Add(본문.ZoomFactor);
					_현재취소용텍스트인덱스++;
				}
			}
		}
		private void 선택위치의키워드색상입히기()
		{
			int 원래선택위치 = 본문.SelectionStart;
			int 원래선택길이 = 본문.SelectionLength;
			string 원래선택내용 = 본문.SelectedText;

			본문.SelectionColor = System.Drawing.Color.Black;

			본문.SelectionFont = new System.Drawing.Font("맑은 고딕", 11F, FontStyle.Regular);


			foreach (Match ABC형보기 in ABC형보기들.Matches(원래선택내용))
			{
				본문.Select(원래선택위치 + ABC형보기.Index, ABC형보기.Length);
				본문.SelectionColor = System.Drawing.Color.MidnightBlue;
				본문.SelectionFont = new System.Drawing.Font("맑은 고딕", 11F, FontStyle.Regular);
			}

			foreach (Match 키워드매치 in 키워드들.Matches(원래선택내용))
			{
				본문.Select(원래선택위치 + 키워드매치.Index, 키워드매치.Length);
				본문.SelectionColor = System.Drawing.Color.Blue;
				본문.SelectionFont = new System.Drawing.Font("맑은 고딕", 11F, FontStyle.Regular);
			}

			foreach (Match 구문분석 in 구문분석들.Matches(원래선택내용))
			{
				본문.Select(원래선택위치 + 구문분석.Index, 구문분석.Length);
				본문.SelectionColor = System.Drawing.Color.DarkGreen;
				본문.SelectionFont = new System.Drawing.Font("맑은 고딕", 11F, FontStyle.Regular);
			}

			foreach (Match 문제형식 in 문제형식들.Matches(원래선택내용))
			{
				본문.Select(원래선택위치 + 문제형식.Index, 문제형식.Length);
				본문.SelectionColor = System.Drawing.Color.OrangeRed;
				본문.SelectionFont = new System.Drawing.Font("맑은 고딕", 11F, FontStyle.Regular);
			}

			foreach (Match 정답 in 정답들.Matches(원래선택내용))
			{
				본문.Select(원래선택위치 + 정답.Index, 정답.Length);
				본문.SelectionColor = System.Drawing.Color.IndianRed;
				본문.SelectionFont = new System.Drawing.Font("맑은 고딕", 11F, FontStyle.Regular);
			}


			본문.SelectionStart = 원래선택위치;
			본문.SelectionLength = 원래선택길이;
		}
		private void 선택위치에바꿀말넣고키워드색상입히기(string 바꿀말)
		{
			//
			int 처음선택시작위치 = 본문.SelectionStart;
			본문.SelectedText = 바꿀말;
			int 붙여넣기후선택시작위치 = 본문.SelectionStart;

			본문.SelectionStart = 처음선택시작위치;
			본문.SelectionLength = 붙여넣기후선택시작위치 - 처음선택시작위치;
			선택위치의키워드색상입히기();
			본문.SelectionLength = 0;
			본문.SelectionStart = 붙여넣기후선택시작위치;

		}
		private void 현재커서근방의키워드색상업데이트()
		{

		}
		#endregion
		#region 커서위치찾기
		protected int 유틸_왼쪽커서위치찾기(int 현재커서위치)
		{
			if (현재커서위치 == 0) return 0;

			int 빈칸위치 = 본문.Text.LastIndexOf(" ", 현재커서위치 - 1, 현재커서위치) + 1;
			int 개행문자위치 = 본문.Text.LastIndexOf("\n", 현재커서위치 - 1, 현재커서위치) + 1;

			int 열린중괄호위치 = 본문.Text.LastIndexOf("{", 현재커서위치 - 1, 현재커서위치) + 1;

			int 어포스트로피S위치 = 본문.Text.LastIndexOf("\'s", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피M위치 = 본문.Text.LastIndexOf("\'m", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피RE위치 = 본문.Text.LastIndexOf("\'re", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피VE위치 = 본문.Text.LastIndexOf("\'ve", 현재커서위치 - 1, 현재커서위치);

			int 이전커서위치 = 현재커서위치;

			if (이전커서위치 == 빈칸위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 개행문자위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 열린중괄호위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);

			if (이전커서위치 == 어포스트로피S위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 어포스트로피M위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 어포스트로피RE위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 어포스트로피VE위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);

			if ((빈칸위치 != 0) && (이전커서위치 == 현재커서위치))
			{
				이전커서위치 = 빈칸위치;
			}

			if ((개행문자위치 != 0) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 개행문자위치; }
			if ((개행문자위치 != 0) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 개행문자위치) 이전커서위치 = 개행문자위치; }

			if ((열린중괄호위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 열린중괄호위치; }
			if ((열린중괄호위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 열린중괄호위치) 이전커서위치 = 열린중괄호위치; }

			if ((어포스트로피S위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피S위치; }
			if ((어포스트로피S위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피S위치) 이전커서위치 = 어포스트로피S위치; }

			if ((어포스트로피M위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피M위치; }
			if ((어포스트로피M위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피M위치) 이전커서위치 = 어포스트로피M위치; }

			if ((어포스트로피RE위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피RE위치; }
			if ((어포스트로피RE위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피RE위치) 이전커서위치 = 어포스트로피RE위치; }

			if ((어포스트로피VE위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피VE위치; }
			if ((어포스트로피VE위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피VE위치) 이전커서위치 = 어포스트로피VE위치; }

			if (이전커서위치 == 현재커서위치)
				return 0;

			return 이전커서위치;
		}

		protected int 유틸_오른쪽에서_되돌아온_왼쪽커서위치찾기(int 현재커서위치)
		{
			if (현재커서위치 == 0) return 0;

			int 왼쪽빈칸위치 = 본문.Text.LastIndexOf(" ", 현재커서위치 - 1, 현재커서위치);
			int 개행문자위치 = 본문.Text.LastIndexOf("\n", 현재커서위치 - 1, 현재커서위치);

//			int 열린중괄호위치 = 본문.Text.LastIndexOf("{", 현재커서위치 - 1, 현재커서위치) + 1;

			int 어포스트로피D위치 = 본문.Text.LastIndexOf("\'d", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피S위치 = 본문.Text.LastIndexOf("\'s", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피M위치 = 본문.Text.LastIndexOf("\'m", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피RE위치 = 본문.Text.LastIndexOf("\'re", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피VE위치 = 본문.Text.LastIndexOf("\'ve", 현재커서위치 - 1, 현재커서위치);

			int 닫힌중괄호위치 = 본문.Text.LastIndexOf("}", 현재커서위치 - 1, 현재커서위치);


			int 점위치 = 본문.Text.LastIndexOf(".", 현재커서위치 - 1, 현재커서위치);
			int 쉼표위치 = 본문.Text.LastIndexOf(",", 현재커서위치 - 1, 현재커서위치);
			int 물음표위치 = 본문.Text.LastIndexOf("?", 현재커서위치 - 1, 현재커서위치);
			int 느낌표위치 = 본문.Text.LastIndexOf("!", 현재커서위치 - 1, 현재커서위치);


			int 이전커서후보 = 현재커서위치;				// 일단 이 함수는 이전 커서 위치를 내보내야 한다. 안전빵으로 현재커서위치부터 세팅한다는 뜻이다.
			if (왼쪽빈칸위치 != 0)	이전커서후보 = 왼쪽빈칸위치;	// 그 다음 바로 이전의 빈칸 위치를 안전빵으로 세팅한다.
			

			if ((개행문자위치 != 0) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 개행문자위치; }
			if ((개행문자위치 != 0) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 개행문자위치) 이전커서후보 = 개행문자위치; }

			if ((어포스트로피D위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 어포스트로피D위치; }
			if ((어포스트로피D위치 != -1) && (이전커서후보 != 현재커서위치)) /*뭐라도 후보가 있다면 비교*/ { if (이전커서후보 < 어포스트로피D위치) 이전커서후보 = 어포스트로피D위치; }

			if ((어포스트로피S위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 어포스트로피S위치; }
			if ((어포스트로피S위치 != -1) && (이전커서후보 != 현재커서위치)) /*뭐라도 후보가 있다면 비교*/ { if (이전커서후보 < 어포스트로피S위치) 이전커서후보 = 어포스트로피S위치; }

			if ((어포스트로피M위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 어포스트로피M위치; }
			if ((어포스트로피M위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 어포스트로피M위치) 이전커서후보 = 어포스트로피M위치; }

			if ((어포스트로피RE위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 어포스트로피RE위치; }
			if ((어포스트로피RE위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 어포스트로피RE위치) 이전커서후보 = 어포스트로피RE위치; }

			if ((어포스트로피VE위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 어포스트로피VE위치; }
			if ((어포스트로피VE위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 어포스트로피VE위치) 이전커서후보 = 어포스트로피VE위치; }


			if ((닫힌중괄호위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 닫힌중괄호위치; }
			if ((닫힌중괄호위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 닫힌중괄호위치) 이전커서후보 = 닫힌중괄호위치; }

			if ((점위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 점위치; }
			if ((점위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 점위치) 이전커서후보 = 점위치; }

			if ((쉼표위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 쉼표위치; }
			if ((쉼표위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 쉼표위치) 이전커서후보 = 쉼표위치; }

			if ((물음표위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 물음표위치; }
			if ((물음표위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 물음표위치) 이전커서후보 = 물음표위치; }

			if ((느낌표위치 != -1) && (이전커서후보 == 현재커서위치)) /*왼쪽 빈칸이 없으면 입력*/ { 이전커서후보 = 느낌표위치; }
			if ((느낌표위치 != -1) && (이전커서후보 != 현재커서위치)) /*왼쪽 빈칸이 있으면 비교*/ { if (이전커서후보 < 느낌표위치) 이전커서후보 = 느낌표위치; }

			if (이전커서후보 == 현재커서위치)
				return 0;

			return 이전커서후보;
		}

		

		protected int 유틸_전전커서위치찾기(int 현재커서위치)
		{
			if (현재커서위치 == 0) return 0;

			int 빈칸위치 = 본문.Text.LastIndexOf(" ", 현재커서위치 - 1, 현재커서위치) + 1;
			int 개행문자위치 = 본문.Text.LastIndexOf("\n", 현재커서위치 - 1, 현재커서위치) + 1;
			int 열린중괄호위치 = 본문.Text.LastIndexOf("{", 현재커서위치 - 1, 현재커서위치) + 1;

			int 어포스트로피S위치 = 본문.Text.LastIndexOf("\'s", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피M위치 = 본문.Text.LastIndexOf("\'m", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피RE위치 = 본문.Text.LastIndexOf("\'re", 현재커서위치 - 1, 현재커서위치);
			int 어포스트로피VE위치 = 본문.Text.LastIndexOf("\'ve", 현재커서위치 - 1, 현재커서위치);

			int 이전커서위치 = 현재커서위치;

			if (이전커서위치 == 빈칸위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 개행문자위치) return 현재커서위치;
			if (이전커서위치 == 열린중괄호위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);


			if (이전커서위치 == 어포스트로피S위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 어포스트로피M위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 어포스트로피RE위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);
			if (이전커서위치 == 어포스트로피VE위치) return 유틸_전전커서위치찾기(현재커서위치 - 1);

			if ((빈칸위치 != 0) && (이전커서위치 == 현재커서위치))
			{
				이전커서위치 = 빈칸위치;
			}

			if ((개행문자위치 != 0) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 개행문자위치; }
			if ((개행문자위치 != 0) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 개행문자위치) 이전커서위치 = 개행문자위치; }

			if ((열린중괄호위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 열린중괄호위치; }
			if ((열린중괄호위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 열린중괄호위치) 이전커서위치 = 열린중괄호위치; }

			if ((어포스트로피S위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피S위치; }
			if ((어포스트로피S위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피S위치) 이전커서위치 = 어포스트로피S위치; }

			if ((어포스트로피M위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피M위치; }
			if ((어포스트로피M위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피M위치) 이전커서위치 = 어포스트로피M위치; }

			if ((어포스트로피RE위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피RE위치; }
			if ((어포스트로피RE위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피RE위치) 이전커서위치 = 어포스트로피RE위치; }

			if ((어포스트로피VE위치 != -1) && (이전커서위치 == 현재커서위치)) { 이전커서위치 = 어포스트로피VE위치; }
			if ((어포스트로피VE위치 != -1) && (이전커서위치 != 현재커서위치)) { if (이전커서위치 < 어포스트로피VE위치) 이전커서위치 = 어포스트로피VE위치; }

			if (이전커서위치 == 현재커서위치)
				return 0;

			return 이전커서위치;
		}

		protected int 유틸_오른쪽커서위치찾기(int 현재커서위치)
		{
			int 쉼표위치 = 본문.Text.IndexOf(",", 현재커서위치);
			int 빈칸위치 = 본문.Text.IndexOf(" ", 현재커서위치);
			int 마침표위치 = 본문.Text.IndexOf(".", 현재커서위치);
			int 느낌표위치 = 본문.Text.IndexOf("!", 현재커서위치);
			int 물음표위치 = 본문.Text.IndexOf("?", 현재커서위치);
			int 닫힌중괄호위치 = 본문.Text.IndexOf("}", 현재커서위치);
			int 닫힌대괄호위치 = 본문.Text.IndexOf("]", 현재커서위치);
			int 콜론위치 = 본문.Text.IndexOf(":", 현재커서위치);

			int 개행문자위치 = 본문.Text.IndexOf("\n", 현재커서위치);

			int 어포스트로피D위치 = 본문.Text.IndexOf("\'d", 현재커서위치);
			int 어포스트로피S위치 = 본문.Text.IndexOf("\'s", 현재커서위치);
			int 어포스트로피M위치 = 본문.Text.IndexOf("\'m", 현재커서위치);
			int 어포스트로피LL위치 = 본문.Text.IndexOf("\'ll", 현재커서위치);
			int 어포스트로피RE위치 = 본문.Text.IndexOf("\'re", 현재커서위치);
			int 어포스트로피VE위치 = 본문.Text.IndexOf("\'ve", 현재커서위치);

			int 오른쪽커서위치후보 = 현재커서위치;


			if (오른쪽커서위치후보 == 쉼표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 빈칸위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 마침표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 느낌표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 물음표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 닫힌중괄호위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 닫힌대괄호위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 콜론위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);

			if (오른쪽커서위치후보 == 개행문자위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);

			if (오른쪽커서위치후보 == 어포스트로피D위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 어포스트로피S위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 어포스트로피M위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 어포스트로피LL위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 어포스트로피RE위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (오른쪽커서위치후보 == 어포스트로피VE위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);

			if ((쉼표위치 != -1) && (오른쪽커서위치후보 == 현재커서위치))
			{
				오른쪽커서위치후보 = 쉼표위치;
			}

			if ((빈칸위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 빈칸위치; }
			if ((빈칸위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (빈칸위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 빈칸위치; }

			if ((마침표위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 마침표위치; }
			if ((마침표위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (마침표위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 마침표위치; }

			if ((느낌표위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 느낌표위치; }
			if ((느낌표위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (느낌표위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 느낌표위치; }

			if ((물음표위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 물음표위치; }
			if ((물음표위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (물음표위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 물음표위치; }

			if ((닫힌중괄호위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 닫힌중괄호위치; }
			if ((닫힌중괄호위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (닫힌중괄호위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 닫힌중괄호위치; }

			if ((닫힌대괄호위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 닫힌대괄호위치; }
			if ((닫힌대괄호위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (닫힌대괄호위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 닫힌대괄호위치; }

			if ((콜론위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 콜론위치; }
			if ((콜론위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (콜론위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 콜론위치; }

			if ((개행문자위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 개행문자위치; }
			if ((개행문자위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (개행문자위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 개행문자위치; }

			if ((어포스트로피D위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 어포스트로피D위치; }
			if ((어포스트로피D위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (어포스트로피D위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 어포스트로피D위치; }

			if ((어포스트로피S위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 어포스트로피S위치; }
			if ((어포스트로피S위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (어포스트로피S위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 어포스트로피S위치; }

			if ((어포스트로피M위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 어포스트로피M위치; }
			if ((어포스트로피M위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (어포스트로피M위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 어포스트로피M위치; }

			if ((어포스트로피LL위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 어포스트로피LL위치; }
			if ((어포스트로피LL위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (어포스트로피LL위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 어포스트로피LL위치; }

			if ((어포스트로피RE위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 어포스트로피RE위치; }
			if ((어포스트로피RE위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (어포스트로피RE위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 어포스트로피RE위치; }

			if ((어포스트로피VE위치 != -1) && (오른쪽커서위치후보 == 현재커서위치)) { 오른쪽커서위치후보 = 어포스트로피VE위치; }
			if ((어포스트로피VE위치 != -1) && (오른쪽커서위치후보 != 현재커서위치)) { if (어포스트로피VE위치 < 오른쪽커서위치후보) 오른쪽커서위치후보 = 어포스트로피VE위치; }


			if (오른쪽커서위치후보 == 현재커서위치)
				return 본문.TextLength;


			return 오른쪽커서위치후보;
		}

		protected int 유틸_다다음커서위치찾기(int 현재커서위치)
		{

			int 쉼표위치 = 본문.Text.IndexOf(",", 현재커서위치);
			int 빈칸위치 = 본문.Text.IndexOf(" ", 현재커서위치);
			int 마침표위치 = 본문.Text.IndexOf(".", 현재커서위치);
			int 느낌표위치 = 본문.Text.IndexOf("!", 현재커서위치);
			int 물음표위치 = 본문.Text.IndexOf("?", 현재커서위치);
			int 닫힌중괄호위치 = 본문.Text.IndexOf("}", 현재커서위치);
			int 닫힌대괄호위치 = 본문.Text.IndexOf("]", 현재커서위치);
			int 콜론위치 = 본문.Text.IndexOf(":", 현재커서위치);

			int 개행문자위치 = 본문.Text.IndexOf("\n", 현재커서위치);

			int 어포스트로피S위치 = 본문.Text.IndexOf("\'s", 현재커서위치);
			int 어포스트로피M위치 = 본문.Text.IndexOf("\'m", 현재커서위치);
			int 어포스트로피LL위치 = 본문.Text.IndexOf("\'ll", 현재커서위치);
			int 어포스트로피RE위치 = 본문.Text.IndexOf("\'re", 현재커서위치);
			int 어포스트로피VE위치 = 본문.Text.IndexOf("\'ve", 현재커서위치);
			int 어포스트로피S위치_ = 본문.Text.IndexOf("’s", 현재커서위치);
			int 어포스트로피M위치_ = 본문.Text.IndexOf("’m", 현재커서위치);
			int 어포스트로피LL위치_ = 본문.Text.IndexOf("’ll", 현재커서위치);
			int 어포스트로피RE위치_ = 본문.Text.IndexOf("’re", 현재커서위치);
			int 어포스트로피VE위치_ = 본문.Text.IndexOf("’ve", 현재커서위치);

			int 다음커서위치 = 현재커서위치;

			if (다음커서위치 == 쉼표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 빈칸위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 마침표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 느낌표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 물음표위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 닫힌중괄호위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 닫힌대괄호위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 콜론위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);

			if (다음커서위치 == 개행문자위치) return 현재커서위치;

			if (다음커서위치 == 어포스트로피S위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피M위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피LL위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피RE위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피VE위치) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피S위치_) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피M위치_) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피LL위치_) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피RE위치_) return 유틸_다다음커서위치찾기(현재커서위치 + 1);
			if (다음커서위치 == 어포스트로피VE위치_) return 유틸_다다음커서위치찾기(현재커서위치 + 1);

			if ((쉼표위치 != -1) && (다음커서위치 == 현재커서위치))
			{
				다음커서위치 = 쉼표위치;
			}

			if ((빈칸위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 빈칸위치; }
			if ((빈칸위치 != -1) && (다음커서위치 != 현재커서위치)) { if (빈칸위치 < 다음커서위치) 다음커서위치 = 빈칸위치; }

			if ((마침표위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 마침표위치; }
			if ((마침표위치 != -1) && (다음커서위치 != 현재커서위치)) { if (마침표위치 < 다음커서위치) 다음커서위치 = 마침표위치; }

			if ((느낌표위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 느낌표위치; }
			if ((느낌표위치 != -1) && (다음커서위치 != 현재커서위치)) { if (느낌표위치 < 다음커서위치) 다음커서위치 = 느낌표위치; }

			if ((물음표위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 물음표위치; }
			if ((물음표위치 != -1) && (다음커서위치 != 현재커서위치)) { if (물음표위치 < 다음커서위치) 다음커서위치 = 물음표위치; }

			if ((닫힌중괄호위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 닫힌중괄호위치; }
			if ((닫힌중괄호위치 != -1) && (다음커서위치 != 현재커서위치)) { if (닫힌중괄호위치 < 다음커서위치) 다음커서위치 = 닫힌중괄호위치; }

			if ((닫힌대괄호위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 닫힌대괄호위치; }
			if ((닫힌대괄호위치 != -1) && (다음커서위치 != 현재커서위치)) { if (닫힌대괄호위치 < 다음커서위치) 다음커서위치 = 닫힌대괄호위치; }

			if ((콜론위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 콜론위치; }
			if ((콜론위치 != -1) && (다음커서위치 != 현재커서위치)) { if (콜론위치 < 다음커서위치) 다음커서위치 = 콜론위치; }

			if ((개행문자위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 개행문자위치; }
			if ((개행문자위치 != -1) && (다음커서위치 != 현재커서위치)) { if (개행문자위치 < 다음커서위치) 다음커서위치 = 개행문자위치; }

			if ((어포스트로피S위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피S위치; }
			if ((어포스트로피S위치 != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피S위치 < 다음커서위치) 다음커서위치 = 어포스트로피S위치; }

			if ((어포스트로피M위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피M위치; }
			if ((어포스트로피M위치 != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피M위치 < 다음커서위치) 다음커서위치 = 어포스트로피M위치; }

			if ((어포스트로피LL위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피LL위치; }
			if ((어포스트로피LL위치 != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피LL위치 < 다음커서위치) 다음커서위치 = 어포스트로피LL위치; }

			if ((어포스트로피RE위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피RE위치; }
			if ((어포스트로피RE위치 != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피RE위치 < 다음커서위치) 다음커서위치 = 어포스트로피RE위치; }

			if ((어포스트로피VE위치 != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피VE위치; }
			if ((어포스트로피VE위치 != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피VE위치 < 다음커서위치) 다음커서위치 = 어포스트로피VE위치; }

			if ((어포스트로피S위치_ != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피S위치_; }
			if ((어포스트로피S위치_ != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피S위치_ < 다음커서위치) 다음커서위치 = 어포스트로피S위치_; }

			if ((어포스트로피M위치_ != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피M위치_; }
			if ((어포스트로피M위치_ != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피M위치_ < 다음커서위치) 다음커서위치 = 어포스트로피M위치_; }

			if ((어포스트로피LL위치_ != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피LL위치_; }
			if ((어포스트로피LL위치_ != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피LL위치_ < 다음커서위치) 다음커서위치 = 어포스트로피LL위치_; }

			if ((어포스트로피RE위치_ != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피RE위치_; }
			if ((어포스트로피RE위치_ != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피RE위치_ < 다음커서위치) 다음커서위치 = 어포스트로피RE위치_; }

			if ((어포스트로피VE위치_ != -1) && (다음커서위치 == 현재커서위치)) { 다음커서위치 = 어포스트로피VE위치_; }
			if ((어포스트로피VE위치_ != -1) && (다음커서위치 != 현재커서위치)) { if (어포스트로피VE위치_ < 다음커서위치) 다음커서위치 = 어포스트로피VE위치_; }


			if (다음커서위치 == 현재커서위치)
				return 본문.TextLength;

			return 다음커서위치;
		}
		#endregion
		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
			if (한글입력모드인지확인()) return; // 한글일 경우 도깨비 불 문자로 인하여 글자하나마다 업데이트 하려고 하는 경우 문제가 발생한다.

			if (_텍스트변경사항자동저장불필요) return;

			현재내용을_실행취소용_클립보드에_저장();

			현재커서근방의키워드색상업데이트();
		}
		#region 유틸리티
		[DllImport("Imm32.dll")]
		public static extern bool ImmGetOpenStatus(IntPtr hImc);

		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ImmGetContext(IntPtr hWnd);

		private bool 한글입력모드인지확인()
		{
			IntPtr hIMC;
			hIMC = ImmGetContext(본문.Handle);

			return ImmGetOpenStatus(hIMC);
		}
		public string 선택영역텍스트()
		{
			return 본문.SelectedText;
		}

		public void 선택영역텍스트(string 바꿀말)
		{
			본문.SelectedText = 바꿀말;
		}

		private string 현재지문의CAKE내용추출(ref int 선택시작위치, ref int 선택끝위치)
		{
			int 현재선택위치 = 본문.SelectionStart;
			int 현재선택길이 = 본문.SelectionLength;

			int 앞의CAKE위치 = 본문.Text.LastIndexOf("<CAKE>", 본문.SelectionStart - 1, 본문.SelectionStart);

			int 뒤의CAKE위치 = 본문.Text.IndexOf("</CAKE>", 본문.SelectionStart);

			if (앞의CAKE위치 == -1 || 뒤의CAKE위치 == -1) return "";

			int 다음SUGAR의첫위치 = 본문.Text.IndexOf("<SUGAR>", 본문.SelectionStart);
			int 다음SUGAR의끝위치 = 본문.Text.IndexOf("</SUGAR>", 본문.SelectionStart);

			if (다음SUGAR의첫위치 != -1 && 다음SUGAR의끝위치 != -1 && 다음SUGAR의첫위치 < 뒤의CAKE위치)
			{
				뒤의CAKE위치 = 본문.Text.IndexOf("</CAKE>", 다음SUGAR의끝위치);
			}

			if (앞의CAKE위치 == -1 || 뒤의CAKE위치 == -1) return "";

			선택시작위치 = 앞의CAKE위치;
			선택끝위치 = 뒤의CAKE위치 + 7;

			화면업데이트중지();
			_텍스트변경사항자동저장불필요 = true;

			본문.Select(선택시작위치, 선택끝위치 - 선택시작위치);

			string CAKE내용 = 본문.SelectedText;

			본문.Select(현재선택위치, 현재선택길이);

			_텍스트변경사항자동저장불필요 = false;
			화면업데이트재개();

			return CAKE내용;
		}
		private string 현재문제의T내용추출(ref int 앞의T위치, ref int 뒤의T위치)
		{
			int 앞의CAKE위치 = 본문.Text.LastIndexOf("<CAKE>", 본문.SelectionStart - 1, 본문.SelectionStart);
			int 뒤의CAKE위치 = 본문.Text.IndexOf("</CAKE>", 본문.SelectionStart);

			if (앞의CAKE위치 == -1) return "";

			앞의T위치 = 본문.Text.IndexOf("<T>", 앞의CAKE위치);
			뒤의T위치 = 본문.Text.IndexOf("</T>", 앞의CAKE위치);

			if (앞의T위치 == -1 || 뒤의T위치 == -1) return "";
			else if (뒤의T위치 < 앞의T위치) return "";
			else
			{
				앞의T위치 += 4;

				return 본문.Text.Substring(앞의T위치, 뒤의T위치 - 앞의T위치);
			}
		}

		#endregion

		#region 파일
		private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.OpenFileDialog 파일열기다이얼로그 = new System.Windows.Forms.OpenFileDialog();
			파일열기다이얼로그.Filter = "XML과 텍스트 형식 파일(*.xml;*.txt;*.smi)|*.xml;*.txt;*.smi|모든 파일(*.*)|*.*";
			//파일열기다이얼로그.InitialDirectory = "";
			파일열기다이얼로그.Title = "열기";

			if (파일열기다이얼로그.ShowDialog() == DialogResult.OK)
			{
				열기(파일열기다이얼로그.FileName);
			}
		}

		private string 현재양식내용()
		{
			// 만약 질문 내용은 있는 데, 고유번호가 없다면, 재빨리 만들고 시작한다.
			if(!String.IsNullOrEmpty(질문.Text) && String.IsNullOrEmpty(고유번호.Text))
			{
				고유번호.Text = 질문.Text.문제고유번호만들어내기(_문제고유번호에사용할값);
                _문제고유번호에사용할값++;
                if (_문제고유번호에사용할값 == 100) _문제고유번호에사용할값 = 0;

            }

			string 결과값 = "";
			결과값 += "<CAKE>\r\n";
			if (제목.Text != "")		결과값 += "\t<B> " + 제목.Text + " </B>\r\n";
			if (질문.Text != "")		결과값 += "\t<Q> " + 질문.Text + " " + 고유번호.Text + " </Q>\r\n";
			if (본문.Text != "")		결과값 += "\t<T>\r\n" + 본문.Text.Replace("\n", "\r\n").Replace("\r\r", "\r") + "\r\n\t</T>\r\n";
			if (보기1.Text != "" || 보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked)	결과값 += "\t<A>\r\n";
			if (ABC.Text != "")		결과값 += "\t<A0> " + ABC.Text + " </A0>\r\n";
			if (보기1Text.Text != "" || 보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked)	결과값 += "\t<A1> " + 보기1Text.Text + " </A1>\r\n";
			if (보기1Text.Text != "" || 보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked)	결과값 += "\t<A2> " + 보기2Text.Text + " </A2>\r\n";
			if (보기1Text.Text != "" || 보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked)	결과값 += "\t<A3> " + 보기3Text.Text + " </A3>\r\n";
			if (보기1Text.Text != "" || 보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked)	결과값 += "\t<A4> " + 보기4Text.Text + " </A4>\r\n";
			if (보기1Text.Text != "" || 보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked)	결과값 += "\t<A5> " + 보기5Text.Text + " </A5>\r\n";
			if (보기1.Text != "" || 보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked)		결과값 += "\t</A>\r\n";

			if (보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked || 주관식정답.Text != "")
				결과값 += "\t<정답>\r\n";

			if (보기1.Checked) { 결과값 += "\t정답 ①번\r\n"; }
			else if (보기2.Checked) { 결과값 += "\t정답 ②번\r\n"; }
			else if (보기3.Checked) { 결과값 += "\t정답 ③번\r\n"; }
			else if (보기4.Checked) { 결과값 += "\t정답 ④번\r\n"; }
			else if (보기5.Checked) { 결과값 += "\t정답 ⑤번\r\n"; }
			else if (주관식정답.Text.Trim() != "" && !주관식정답.Text.Contains("정답"))
			{
				결과값 += "\t정답 : " + 주관식정답.Text + "\r\n";
			}
			else if (주관식정답.Text.Trim() != "")
			{
				결과값 += "\t" + 주관식정답.Text + "\r\n";
			}

			if (보기1.Checked || 보기2.Checked || 보기3.Checked || 보기4.Checked || 보기5.Checked || 주관식정답.Text != "")
				결과값 += "\t</정답>\r\n";



			if (해석.Text != "")
			{
				결과값 += "\t<해석>\r\n";
				결과값 += 해석.Text + "\r\n"; // 여기 탭 절대로 넣으면 안된다.
				결과값 += "\t</해석>\r\n";
			}

			if (힌트.Text != "")
			{
				결과값 += "\t<해설>\r\n";
				결과값 += "\t" + 힌트.Text + "\r\n";
				결과값 += "\t</해설>\r\n";
			}

			if (보기1_해설.Text != "")
			{
				결과값 += "\t<보기1_해설>\r\n";
				결과값 += "\t" + 보기1_해설.Text + "\r\n";
				결과값 += "\t</보기1_해설>\r\n";
			}

			if (보기2_해설.Text != "")
			{
				결과값 += "\t<보기2_해설>\r\n";
				결과값 += "\t" + 보기2_해설.Text + "\r\n";
				결과값 += "\t</보기2_해설>\r\n";
			}

			if (보기3_해설.Text != "")
			{
				결과값 += "\t<보기3_해설>\r\n";
				결과값 += "\t" + 보기3_해설.Text + "\r\n";
				결과값 += "\t</보기3_해설>\r\n";
			}

			if (보기4_해설.Text != "")
			{
				결과값 += "\t<보기4_해설>\r\n";
				결과값 += "\t" + 보기4_해설.Text + "\r\n";
				결과값 += "\t</보기4_해설>\r\n";
			}

			if (보기5_해설.Text != "")
			{
				결과값 += "\t<보기5_해설>\r\n";
				결과값 += "\t" + 보기5_해설.Text + "\r\n";
				결과값 += "\t</보기5_해설>\r\n";
			}

			if (주관식_해설.Text != "")
			{
				결과값 += "\t<주관식_해설>\r\n";
				결과값 += "\t" + 주관식_해설.Text + "\r\n";
				결과값 += "\t</주관식_해설>\r\n";
			}

			if (중요어휘.Text != "")
			{
				결과값 += "\t<중요어휘>\r\n";
				결과값 += "\t" + 중요어휘.Text + "\r\n";
				결과값 += "\t</중요어휘>\r\n";
			}

			if(주제.Text != "")
			{
				결과값 += "\t<주제>\r\n";
				결과값 += "\t" + 주제.Text + "\r\n";
				결과값 += "\t</주제>\r\n";
			}

			if(변형지문.Text != "")
			{
				결과값 += "\t<변형지문>\r\n";
				결과값 += "\t" + 변형지문.Text + "\r\n";
				결과값 += "\t</변형지문>\r\n";
			}

			if (변형지문해석.Text != "")
			{
				결과값 += "\t<변형지문해석>\r\n";
				결과값 += "\t" + 변형지문해석.Text + "\r\n";
				결과값 += "\t</변형지문해석>\r\n";
			}

			결과값 += "</CAKE>\r\n";

			return 결과값;
		}

		// 화면을 전환했을 때 여태까지 편집해둔 내용을 자동으로 기억하도록 만드는 기능이다.
		private void 메모리_저장CAKE()
		{
			if(_편집시작)
			{
				_편집시작 = false; return;
			}

            if (_CAKE들.Count() <= 0 || _CAKE_인덱스 < 0)
            {
                // 뭔가 내용을 입력하기는 했는데, 저장은 하지 않았을 경우
                if(!String.IsNullOrEmpty(제목.Text) || !String.IsNullOrEmpty(질문.Text) || !String.IsNullOrEmpty(본문.Text) || !String.IsNullOrEmpty(ABC.Text) ||
                    !String.IsNullOrEmpty(보기1Text.Text) || !String.IsNullOrEmpty(보기2Text.Text) || !String.IsNullOrEmpty(보기3Text.Text) || 
                    !String.IsNullOrEmpty(보기4Text.Text) || !String.IsNullOrEmpty(보기5Text.Text) || !String.IsNullOrEmpty(주관식정답.Text) ||
                    !String.IsNullOrEmpty(해석.Text) || !String.IsNullOrEmpty(힌트.Text) || !String.IsNullOrEmpty(중요어휘.Text) ||
					!String.IsNullOrEmpty(보기1_해설.Text) || !String.IsNullOrEmpty(보기2_해설.Text) || !String.IsNullOrEmpty(보기3_해설.Text) || !String.IsNullOrEmpty(보기4_해설.Text) || !String.IsNullOrEmpty(보기5_해설.Text) || !String.IsNullOrEmpty(주관식_해설.Text))
                {
                    if (_CAKE들.Count == 0)
                        _CAKE들.Add(현재양식내용().Replace("\n", "\r\n").Replace("\r\r", "\r"));
                    else
                    {
                        if(_CAKE들[0] == "<CAKE>\r\n</CAKE>\r\n")
                            _CAKE들[0] = 현재양식내용().Replace("\n", "\r\n").Replace("\r\r", "\r");
                    }
                    _CAKE_인덱스 = 0;
                }

                return;
            }

			string 원래케이크 = _CAKE들[_CAKE_인덱스];

			string 파일내용 = "", 예상문제 = "";
			변환.문자열.SUGAR추출(원래케이크, ref 파일내용, ref 예상문제);


			if (_SUGAR_인덱스 == -1 || _SUGAR_인덱스 == -2) // 예상문제를 선택하지 않고, 상위 문제를 선택했을 경우, 혹은 맨 처음에 열었을 때
			{
				if (예상문제 != "")
					_CAKE들[_CAKE_인덱스] = 현재양식내용().Replace("</CAKE>", "") + "<SUGAR>\r\n" + 예상문제 + "</SUGAR>\r\n" + "</CAKE>\r\n";
				else
					_CAKE들[_CAKE_인덱스] = 현재양식내용();
			}
			else if (_SUGAR_인덱스 >= 0) // 예상문제를 선택했을 경우
			{
				List<string> SUGAR들 = new List<string>();

				변환.문자열.CAKE들로(예상문제, ref SUGAR들);

				string 케이크임시 = 파일내용.Replace("</CAKE>", "") + "<SUGAR>\r\n";

				for (int i = 0; i < SUGAR들.Count; i++)
				{
					if (i == _SUGAR_인덱스)
					{
						케이크임시 += 현재양식내용();
					}
					else
						케이크임시 += SUGAR들[i];
				}


				케이크임시 += "</SUGAR>\r\n" + "</CAKE>\r\n";

				_CAKE들[_CAKE_인덱스] = 케이크임시;
			}
		}

		private bool 문서_종류와_난이도를_클릭했는지_확인()
		{
			bool 문서종류_클릭여부 = false;

			if (교과서라디오버튼.Checked) 문서종류_클릭여부 = true;
			else if (독해집라디오버튼.Checked) 문서종류_클릭여부 = true;
			else if (모의고사라디오버튼.Checked) 문서종류_클릭여부 = true;
			else if (수능라디오버튼.Checked) 문서종류_클릭여부 = true;
			else if (어법연습문제라디오버튼.Checked) 문서종류_클릭여부 = true;


			bool 난이도_클릭여부 = false;

			if (일반라디오버튼.Checked) 난이도_클릭여부 = true;
			else if (고3라디오버튼.Checked) 난이도_클릭여부 = true;
			else if (고2고1라디오버튼.Checked) 난이도_클릭여부 = true;
			else if (중3라디오버튼.Checked) 난이도_클릭여부 = true;
			else if (중2라디오버튼.Checked) 난이도_클릭여부 = true;
			else if (중1라디오버튼.Checked) 난이도_클릭여부 = true;
			else if (초등라디오버튼.Checked) 난이도_클릭여부 = true;

			if (문서종류_클릭여부 == true && 난이도_클릭여부 == true) return true;

			return false;
		}

		private string 헤더정보()
		{
			string 헤더정보 = "";

			_헤더 = "";

			string 문서종류 = "";

			if(교과서라디오버튼.Checked) 문서종류 = "교과서";
			else if(독해집라디오버튼.Checked) 문서종류 = "독해집";
			else if(모의고사라디오버튼.Checked) 문서종류 = "모의고사";
			else if(수능라디오버튼.Checked) 문서종류 = "수능";
			else if(어법연습문제라디오버튼.Checked) 문서종류 = "어법연습문제";

			string 난이도 = "";

			if(일반라디오버튼.Checked) 난이도 = "일반";
			else if(고3라디오버튼.Checked) 난이도 = "고3";
			else if(고2고1라디오버튼.Checked) 난이도 = "고2, 고1";
			else if(중3라디오버튼.Checked) 난이도 = "중3";
			else if(중2라디오버튼.Checked) 난이도 = "중2";
			else if(중1라디오버튼.Checked) 난이도 = "중1";
			else if(초등라디오버튼.Checked) 난이도 = "초등";

			if(문서종류 != "" && 난이도 != "")
				_헤더 = 문서종류 + ", " + 난이도;
			else if(문서종류 != "")
				_헤더 = 문서종류;
			else if(난이도 != "")
				_헤더 = 난이도;
			else
				_헤더 = "";

			헤더정보 += "<HEAD>\r\n";
			헤더정보 += "\t" + _헤더 + "\r\n";
			헤더정보 += "</HEAD>\r\n";

			return 헤더정보;
		}


		private void 저장()
		{
			Text = _파일경로 + " - " + _편집기의제왕로고;


			string 저장할문자열 = "";

			string 원래케이크 = "";

			if (_CAKE_인덱스 != -2 && _CAKE들.Count != 0) 원래케이크 = _CAKE들[_CAKE_인덱스];
			else if (_CAKE들.Count > 0) 원래케이크 = _CAKE들[0];
			else
			{
				// 아무것도 없이 처음 시작할 때 내용이 이곳으로 들어감
				_CAKE들.Add(현재양식내용().Replace("\n", "\r\n").Replace("\r\r", "\r"));
				_CAKE_인덱스 = 0;


			}

			string 파일내용 = "", 예상문제 = "";
			변환.문자열.SUGAR추출(원래케이크, ref 파일내용, ref 예상문제);

			if(_SUGAR_인덱스 == -2) // 처음부터 아무것도 선택하지 않았을 경우
			{
				_CAKE_인덱스 = 0;

                if (예상문제 != "")
					_CAKE들[_CAKE_인덱스] = 현재양식내용().Replace("</CAKE>", "") + "<SUGAR>\r\n" + 예상문제 + "</SUGAR>\r\n" + "</CAKE>\r\n";
				else
					_CAKE들[_CAKE_인덱스] = 현재양식내용();

				if (제목.Text != "")
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Text = 제목.Text.너비(180);
				else if (질문.Text != "")
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Text = 질문.Text.너비(180);
				else
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Text = "항목 1번";
			}
			else if (_SUGAR_인덱스 == -1) // 예상문제를 선택하지 않고, 상위 문제를 선택했을 경우
			{
				if(예상문제 != "")
					_CAKE들[_CAKE_인덱스] = 현재양식내용().Replace("</CAKE>", "") + "<SUGAR>\r\n" + 예상문제 + "</SUGAR>\r\n" + "</CAKE>\r\n";
				else
					_CAKE들[_CAKE_인덱스] = 현재양식내용();

				if(제목.Text != "")
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Text = 제목.Text.너비(180);
				else if (질문.Text != "")
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Text = 질문.Text.너비(180);
				else
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Text = "항목 " + (_CAKE_인덱스 + 1) + "번";
			}
			else if (_SUGAR_인덱스 >= 0) // 예상문제를 선택했을 경우
			{
				List<string> SUGAR들 = new List<string>();

				변환.문자열.CAKE들로(예상문제, ref SUGAR들);

				string 케이크임시 = 파일내용.Replace("</CAKE>", "") + "<SUGAR>\r\n";

				for (int i = 0; i < SUGAR들.Count; i++)
				{
					if (i == _SUGAR_인덱스)
					{
						케이크임시 += 현재양식내용();
					}
					else
						케이크임시 += SUGAR들[i] + "\r\n";
				}


				케이크임시 += "</SUGAR>\r\n" + "</CAKE>\r\n";
				케이크임시 = 케이크임시.Replace("</SUGAR>\r\n\r\n", "</SUGAR>\r\n");

				_CAKE들[_CAKE_인덱스] = 케이크임시;

				if (제목.Text != "")
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes[_SUGAR_인덱스].Text = 제목.Text.너비(180);
				else if (질문.Text != "")
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes[_SUGAR_인덱스].Text = 질문.Text.너비(180);
				else
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes[_SUGAR_인덱스].Text = "항목 " + (_CAKE_인덱스 + 1) + "번";

			}


			treeView1.Nodes[0].Text = Path.GetFileName(_파일경로);

			저장할문자열 += 헤더정보();

            for (int i = 0; i < _CAKE들.Count; i++) 저장할문자열 += _CAKE들[i] + "\r\n";

			저장할문자열 = 저장할문자열.Replace("</CAKE>\r\n\r\n", "</CAKE>\r\n").Replace("\n", "\r\n").Replace("\r\r", "\r");
			//변환.문자열.Ansi파일로(저장할문자열, _파일경로);
			변환.문자열.UTF8파일로(저장할문자열, _파일경로);
		}


		private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_파일경로 == "")
			{
				System.Windows.Forms.SaveFileDialog 파일저장다이얼로그 = new System.Windows.Forms.SaveFileDialog();

				파일저장다이얼로그.Filter = "XML 파일(*.xml)|*.xml|txt 파일(*.txt)|*.txt|smi 파일(*.smi)|*.smi";
				파일저장다이얼로그.Title = "저장";

				if (파일저장다이얼로그.ShowDialog() == DialogResult.OK)
				{
					_파일경로 = 파일저장다이얼로그.FileName;
					_현재폴더 = Path.GetDirectoryName(_파일경로);

					저장();
				}

				_내용변경여부 = false;
			}
			else
			{
				저장();
				_내용변경여부 = false;
			}
		}

		private void 다른이름으로저장AToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.SaveFileDialog 파일저장다이얼로그 = new System.Windows.Forms.SaveFileDialog();

			파일저장다이얼로그.Filter = "XML 파일(*.xml)|*.xml|txt 파일(*.txt)|*.txt|smi 파일(*.smi)|*.smi";
			파일저장다이얼로그.Title = "저장";

			if (파일저장다이얼로그.ShowDialog() == DialogResult.OK)
			{
				_파일경로 = 파일저장다이얼로그.FileName;
				_현재폴더 = Path.GetDirectoryName(_파일경로);

				저장();
			}

			_내용변경여부 = false;
		}

		#endregion
		#region 편집


		private void 실행취소ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (본문.Focused)
            {

                _텍스트변경사항자동저장불필요 = true;

                화면업데이트중지();

                if (한글입력모드인지확인() && (_실행취소용RTF들.Count == _현재취소용텍스트인덱스 + 1)) // 한글입력모드인 경우에는 글자가 바뀔 때마다 저장하지 않기 때문에, 현재 취소용 인덱스를 이전으로 돌리면, 상당히 이상한 곳까지 되돌아간다.
                    현재내용을_실행취소용_클립보드에_저장();


                if (_현재취소용텍스트인덱스 > 0)
                {
                    _현재취소용텍스트인덱스--;

                    본문.Rtf = _실행취소용RTF들[_현재취소용텍스트인덱스];
                    본문.SelectionStart = _실행취소용선택위치들[_현재취소용텍스트인덱스];
                    본문.SelectionLength = _실행취소용선택길이들[_현재취소용텍스트인덱스];
                    본문.ZoomFactor = 1.0f; // 참 기가막히는 용법이다. 1.0f로 초기화 해주어야 설정이 먹힌다.
                    본문.ZoomFactor = _실행취소용확대배율들[_현재취소용텍스트인덱스];
                    int nPos = _실행취소용스크롤위치들[_현재취소용텍스트인덱스];
                    nPos <<= 16;
                    uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
                    SendMessage(본문.Handle, (int)스크롤업데이트메시지.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));

                }

				줄간격_조절(2, 0);

				화면업데이트재개();

				_텍스트변경사항자동저장불필요 = false;
            }
            else if (해석.Focused) { 해석.Undo(); }
            else if (힌트.Focused) { 힌트.Undo(); }
            else if (중요어휘.Focused) { 중요어휘.Undo(); }
        }
		//바로 위와 똑같음 단지 Ctrl Shift Z도 먹히게 하려고 함
		private void 실행취소2ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (본문.Focused)
            {

                _텍스트변경사항자동저장불필요 = true;

                화면업데이트중지();

                if (한글입력모드인지확인() && (_실행취소용RTF들.Count == _현재취소용텍스트인덱스 + 1)) // 한글입력모드인 경우에는 글자가 바뀔 때마다 저장하지 않기 때문에, 현재 취소용 인덱스를 이전으로 돌리면, 상당히 이상한 곳까지 되돌아간다.
                    현재내용을_실행취소용_클립보드에_저장();


                if (_현재취소용텍스트인덱스 > 0)
                {
                    _현재취소용텍스트인덱스--;

                    본문.Rtf = _실행취소용RTF들[_현재취소용텍스트인덱스];
                    본문.SelectionStart = _실행취소용선택위치들[_현재취소용텍스트인덱스];
                    본문.SelectionLength = _실행취소용선택길이들[_현재취소용텍스트인덱스];
                    본문.ZoomFactor = 1.0f; // 참 기가막히는 용법이다. 1.0f로 초기화 해주어야 설정이 먹힌다.
                    본문.ZoomFactor = _실행취소용확대배율들[_현재취소용텍스트인덱스];
                    int nPos = _실행취소용스크롤위치들[_현재취소용텍스트인덱스];
                    nPos <<= 16;
                    uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
                    SendMessage(본문.Handle, (int)스크롤업데이트메시지.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));

                }
                화면업데이트재개();

                _텍스트변경사항자동저장불필요 = false;
            }
            else if (해석.Focused) { 해석.Undo(); }
            else if (힌트.Focused) { 힌트.Undo(); }
            else if (중요어휘.Focused) { 중요어휘.Undo(); }
		}
        private void 다시실행ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (본문.Focused)
			{
				_텍스트변경사항자동저장불필요 = true;
				화면업데이트중지();

				if (_실행취소용RTF들.Count > _현재취소용텍스트인덱스 + 1)
				{
					_현재취소용텍스트인덱스++;

					본문.Rtf = _실행취소용RTF들[_현재취소용텍스트인덱스];
					본문.SelectionStart = _실행취소용선택위치들[_현재취소용텍스트인덱스];
					본문.SelectionLength = _실행취소용선택길이들[_현재취소용텍스트인덱스];
					본문.ZoomFactor = 1.0f; // 참 기가막히는 용법이다. 1.0f로 초기화 해주어야 설정이 먹힌다.
					본문.ZoomFactor = _실행취소용확대배율들[_현재취소용텍스트인덱스];

					int nPos = _실행취소용스크롤위치들[_현재취소용텍스트인덱스];
					nPos <<= 16;
					uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
					SendMessage(본문.Handle, (int)스크롤업데이트메시지.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));

				}
				화면업데이트재개();

				_텍스트변경사항자동저장불필요 = false;
			}
		}
		private void 잘라내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (제목.Focused){ if (String.IsNullOrEmpty(제목.SelectedText)) return; Clipboard.SetText(제목.SelectedText);	제목.SelectedText = "";	}
			else if (질문.Focused) { if (String.IsNullOrEmpty(질문.SelectedText)) return; Clipboard.SetText(질문.SelectedText); 질문.SelectedText = ""; }
			else if (본문.Focused)
			{
				if (String.IsNullOrEmpty(본문.SelectedText))		return;

				현재내용을_실행취소용_클립보드에_저장();
				_텍스트변경사항자동저장불필요 = true;

				Clipboard.SetText(본문.SelectedText);
				본문.SelectedText = "";
				현재커서근방의키워드색상업데이트();

				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();
			}
			else if (ABC.Focused) { if (String.IsNullOrEmpty(ABC.SelectedText)) return; Clipboard.SetText(ABC.SelectedText); ABC.SelectedText = ""; }
			else if (보기1Text.Focused) { if (String.IsNullOrEmpty(보기1Text.SelectedText)) return; Clipboard.SetText(보기1Text.SelectedText); 보기1Text.SelectedText = ""; }
			else if (보기2Text.Focused) { if (String.IsNullOrEmpty(보기2Text.SelectedText)) return; Clipboard.SetText(보기2Text.SelectedText); 보기2Text.SelectedText = ""; }
			else if (보기3Text.Focused) { if (String.IsNullOrEmpty(보기3Text.SelectedText)) return; Clipboard.SetText(보기3Text.SelectedText); 보기3Text.SelectedText = ""; }
			else if (보기4Text.Focused) { if (String.IsNullOrEmpty(보기4Text.SelectedText)) return; Clipboard.SetText(보기4Text.SelectedText); 보기4Text.SelectedText = ""; }
			else if (보기5Text.Focused) { if (String.IsNullOrEmpty(보기5Text.SelectedText)) return; Clipboard.SetText(보기5Text.SelectedText); 보기5Text.SelectedText = ""; }
			else if (주관식정답.Focused) { if (String.IsNullOrEmpty(주관식정답.SelectedText)) return; Clipboard.SetText(주관식정답.SelectedText); 주관식정답.SelectedText = ""; }
			else if (해석.Focused) { if (String.IsNullOrEmpty(해석.SelectedText)) return; Clipboard.SetText(해석.SelectedText); 해석.SelectedText = ""; }
			else if (힌트.Focused) { if (String.IsNullOrEmpty(힌트.SelectedText)) return; Clipboard.SetText(힌트.SelectedText); 힌트.SelectedText = ""; }
			else if (중요어휘.Focused) { if (String.IsNullOrEmpty(중요어휘.SelectedText)) return; Clipboard.SetText(중요어휘.SelectedText); 중요어휘.SelectedText = ""; }
			else if (사전페이지_사전표제어.Focused) { if (String.IsNullOrEmpty(사전페이지_사전표제어.SelectedText)) return; Clipboard.SetText(사전페이지_사전표제어.SelectedText); 사전페이지_사전표제어.SelectedText = ""; }
			else if (사전페이지_사전발음기호.Focused) { if (String.IsNullOrEmpty(사전페이지_사전발음기호.SelectedText)) return; Clipboard.SetText(사전페이지_사전발음기호.SelectedText); 사전페이지_사전발음기호.SelectedText = ""; }
			else if (사전페이지_사전의미.Focused) { if (String.IsNullOrEmpty(사전페이지_사전의미.SelectedText)) return; Clipboard.SetText(사전페이지_사전의미.SelectedText); 사전페이지_사전의미.SelectedText = ""; }

			else if (주제.Focused) { if (String.IsNullOrEmpty(주제.SelectedText)) return; Clipboard.SetText(주제.SelectedText); 주제.SelectedText = ""; }
			else if (변형지문.Focused) { if (String.IsNullOrEmpty(변형지문.SelectedText)) return; Clipboard.SetText(변형지문.SelectedText); 변형지문.SelectedText = ""; }
			else if (변형지문해석.Focused) { if (String.IsNullOrEmpty(변형지문해석.SelectedText)) return; Clipboard.SetText(변형지문해석.SelectedText); 변형지문해석.SelectedText = ""; }
		}
		private void 복사(object sender, EventArgs e)
		{
			if (제목.Focused) {      if(제목.SelectedText != "") { Clipboard.SetText(제목.SelectedText); _최근복사한클립보드내용 = 제목.SelectedText;} }
			else if(treeView1.Focused) { Clipboard.SetText("<현재문제복사>" + 현재양식내용() + "</현재문제복사>"); }
			else if (질문.Focused) { if(질문.SelectedText != "") { Clipboard.SetText(질문.SelectedText); _최근복사한클립보드내용 = 질문.SelectedText;} }
			else if (본문.Focused) { if(본문.SelectedText != "") { Clipboard.SetText(본문.SelectedText); _최근복사한클립보드내용 = 본문.SelectedText;} }

			else if (ABC.Focused) {  if(ABC.SelectedText != "") { Clipboard.SetText(ABC.SelectedText);   _최근복사한클립보드내용 = ABC.SelectedText; } }
			else if (보기1Text.Focused) { if(보기1Text.SelectedText != "") { Clipboard.SetText(보기1Text.SelectedText); _최근복사한클립보드내용 = 보기1Text.SelectedText;} }
			else if (보기2Text.Focused) { if(보기2Text.SelectedText != "") { Clipboard.SetText(보기2Text.SelectedText); _최근복사한클립보드내용 = 보기2Text.SelectedText;} }
			else if (보기3Text.Focused) { if(보기3Text.SelectedText != "") { Clipboard.SetText(보기3Text.SelectedText); _최근복사한클립보드내용 = 보기3Text.SelectedText;} }
			else if (보기4Text.Focused) { if(보기4Text.SelectedText != "") { Clipboard.SetText(보기4Text.SelectedText); _최근복사한클립보드내용 = 보기4Text.SelectedText;} }
			else if (보기5Text.Focused) { if(보기5Text.SelectedText != "") { Clipboard.SetText(보기5Text.SelectedText); _최근복사한클립보드내용 = 보기5Text.SelectedText;} }
			else if (주관식정답.Focused) { if(주관식정답.SelectedText != "") { Clipboard.SetText(주관식정답.SelectedText); _최근복사한클립보드내용 = 주관식정답.SelectedText;} }
			else if (해석.Focused) { if (해석.SelectedText != "") { Clipboard.SetText(해석.SelectedText); _최근복사한클립보드내용 = 해석.SelectedText;} }
			else if (힌트.Focused) { if (힌트.SelectedText != "") { Clipboard.SetText(힌트.SelectedText); _최근복사한클립보드내용 = 힌트.SelectedText;} }
			else if (중요어휘.Focused) { if (중요어휘.SelectedText != "") { Clipboard.SetText(중요어휘.SelectedText); _최근복사한클립보드내용 = 중요어휘.SelectedText;} }

			else if (보기1_해설.Focused) { if (보기1_해설.SelectedText != "") { Clipboard.SetText(보기1_해설.SelectedText); _최근복사한클립보드내용 = 보기1_해설.SelectedText;} }
			else if (보기2_해설.Focused) { if (보기2_해설.SelectedText != "") { Clipboard.SetText(보기2_해설.SelectedText); _최근복사한클립보드내용 = 보기2_해설.SelectedText;} }
			else if (보기3_해설.Focused) { if (보기3_해설.SelectedText != "") { Clipboard.SetText(보기3_해설.SelectedText); _최근복사한클립보드내용 = 보기3_해설.SelectedText;} }
			else if (보기4_해설.Focused) { if (보기4_해설.SelectedText != "") { Clipboard.SetText(보기4_해설.SelectedText); _최근복사한클립보드내용 = 보기4_해설.SelectedText;} }
			else if (보기5_해설.Focused) { if (보기5_해설.SelectedText != "") { Clipboard.SetText(보기5_해설.SelectedText); _최근복사한클립보드내용 = 보기5_해설.SelectedText;} }
			
			else if (주관식_해설.Focused) { if (주관식_해설.SelectedText != "") { Clipboard.SetText(주관식_해설.SelectedText); _최근복사한클립보드내용 = 주관식_해설.SelectedText;} }

			else if (사전페이지_사전표제어.Focused) { if (사전페이지_사전표제어.SelectedText != "") { Clipboard.SetText(사전페이지_사전표제어.SelectedText); _최근복사한클립보드내용 = 사전페이지_사전표제어.SelectedText;} }
			else if (사전페이지_사전발음기호.Focused) { if (사전페이지_사전발음기호.SelectedText != "") { Clipboard.SetText(사전페이지_사전발음기호.SelectedText); _최근복사한클립보드내용 = 사전페이지_사전발음기호.SelectedText;} }
			else if (사전페이지_사전의미.Focused) { if (사전페이지_사전의미.SelectedText != "") { Clipboard.SetText(사전페이지_사전의미.SelectedText); _최근복사한클립보드내용 = 사전페이지_사전의미.SelectedText;} }

			else if (키워드.Focused) { if (키워드.SelectedText != "") { Clipboard.SetText(키워드.SelectedText); _최근복사한클립보드내용 = 키워드.SelectedText;} }
			else if (주제.Focused) { if (주제.SelectedText != "") { Clipboard.SetText(주제.SelectedText); _최근복사한클립보드내용 = 주제.SelectedText;} }
			else if (변형지문.Focused) { if (변형지문.SelectedText != "") { Clipboard.SetText(변형지문.SelectedText); _최근복사한클립보드내용 = 변형지문.SelectedText; } }
			else if (변형지문해석.Focused) { if (변형지문해석.SelectedText != "") { Clipboard.SetText(변형지문해석.SelectedText); _최근복사한클립보드내용 = 변형지문해석.SelectedText; } }
		}
		private void 복사_태그제거_Click(object sender, EventArgs e)
		{
			if (제목.Focused) {      if(제목.SelectedText != "") { Clipboard.SetText(제목.SelectedText); _최근복사한클립보드내용 = 제목.SelectedText;} }
			else if(treeView1.Focused) { Clipboard.SetText("<현재문제복사>" + 현재양식내용() + "</현재문제복사>"); }
			else if (질문.Focused) { if(질문.SelectedText != "") { Clipboard.SetText(질문.SelectedText); _최근복사한클립보드내용 = 질문.SelectedText;} }
			else if (본문.Focused) { if(본문.SelectedText != "") { Clipboard.SetText(본문.SelectedText.문법문제표지제거()); _최근복사한클립보드내용 = 본문.SelectedText.문법문제표지제거();} }

			else if (ABC.Focused) {  if(ABC.SelectedText != "") { Clipboard.SetText(ABC.SelectedText);   _최근복사한클립보드내용 = ABC.SelectedText; } }
			else if (보기1Text.Focused) { if(보기1Text.SelectedText != "") { Clipboard.SetText(보기1Text.SelectedText); _최근복사한클립보드내용 = 보기1Text.SelectedText;} }
			else if (보기2Text.Focused) { if(보기2Text.SelectedText != "") { Clipboard.SetText(보기2Text.SelectedText); _최근복사한클립보드내용 = 보기2Text.SelectedText;} }
			else if (보기3Text.Focused) { if(보기3Text.SelectedText != "") { Clipboard.SetText(보기3Text.SelectedText); _최근복사한클립보드내용 = 보기3Text.SelectedText;} }
			else if (보기4Text.Focused) { if(보기4Text.SelectedText != "") { Clipboard.SetText(보기4Text.SelectedText); _최근복사한클립보드내용 = 보기4Text.SelectedText;} }
			else if (보기5Text.Focused) { if(보기5Text.SelectedText != "") { Clipboard.SetText(보기5Text.SelectedText); _최근복사한클립보드내용 = 보기5Text.SelectedText;} }
			else if (주관식정답.Focused) { if(주관식정답.SelectedText != "") { Clipboard.SetText(주관식정답.SelectedText); _최근복사한클립보드내용 = 주관식정답.SelectedText;} }
			else if (해석.Focused) { if (해석.SelectedText != "") { Clipboard.SetText(해석.SelectedText); _최근복사한클립보드내용 = 해석.SelectedText;} }
			else if (힌트.Focused) { if (힌트.SelectedText != "") { Clipboard.SetText(힌트.SelectedText); _최근복사한클립보드내용 = 힌트.SelectedText;} }
			else if (중요어휘.Focused) { if (중요어휘.SelectedText != "") { Clipboard.SetText(중요어휘.SelectedText); _최근복사한클립보드내용 = 중요어휘.SelectedText;} }

		}

        private void 해설에붙여넣기(string 클립보드)
        {
            string 현재단계 = "초기단계";



            List<string> 클립보드개행문자로나눈것 = new List<string>();
            변환.문자열.개행문자로_구분한_문자열들로(클립보드, ref 클립보드개행문자로나눈것);

            string 해설_본문 = "";
            string 해설_해석 = "";
            string 해설_단어 = "";
            string 해설_해설 = "";

            for (int i = 0; i < 클립보드개행문자로나눈것.Count; i++)
            {
                string 현재문자열 = 클립보드개행문자로나눈것[i];

                string 다음줄문자열 = "";

                if (i != 클립보드개행문자로나눈것.Count - 1)
                    다음줄문자열 = 클립보드개행문자로나눈것[i + 1];


                if (현재문자열.Contains("[해설]") || 현재문자열.Contains("해설]") || 현재문자열.Contains("[해설") || 현재문자열.Contains("[풀이]"))
                {
                    현재단계 = "해설단계";

                    해설_해설 += 현재문자열.Replace("[풀이]", "").Replace("[해설]", "").Replace("해설]", "").Replace("[해설", "").Trim() + "\r";
                }
                else if (현재문자열.Contains("[어구]") || 현재문자열.Contains("[Words and Phrases]"))
                {
                    현재단계 = "단어설명단계";

                    해설_단어 += 현재문자열.Replace("[Words and Phrases]", "").Replace("[어구]", "").Trim() + "\r";
                }
                else if ((현재문자열.Contains("W:") || 현재문자열.Contains("M:") || (현재문자열.Contains("W :") || 현재문자열.Contains("M :") || 현재문자열.Contains("Telephone") || 현재문자열.Contains("rings")) && (현재단계 == "초기단계")))
                {
                    현재단계 = "본문단계";

                    해설_본문 += 현재문자열 + "\r";
                }
                else if (현재단계 == "초기단계" && !현재문자열.Contains("출제의도") && !현재문자열.Contains("출제 의도") && 현재문자열.한글포함했는지확인())
                {
                    현재단계 = "해석단계";

                    해설_해석 += 현재문자열.Replace("[해석]", "").Trim() + "\r";
                }
                else if ((현재문자열.Contains("①") || 현재문자열.Contains("②") || 현재문자열.Contains("③") || 현재문자열.Contains("④") || 현재문자열.Contains("⑤")) && ((현재단계 == "본문단계") || (현재단계 == "해석단계")) && !현재문자열.단어설명인지확인())
                {
                    현재단계 = "해설단계";

                    해설_해설 += 현재문자열 + "\r";
                }
                else if (질문.Text.Contains("문맥") && (질문.Text.Contains("낱말") || 질문.Text.Contains("어휘")) && 현재문자열.Contains("(A)") && (현재단계 == "해석단계"))
                {
                    현재단계 = "해설단계";

                    해설_해설 += 현재문자열 + "\r";
                }
                else if (현재문자열.단어설명인지확인() && (다음줄문자열.단어설명인지확인() || 다음줄문자열 == "") && (현재단계 != "단어설명단계") && !클립보드.Contains("[Words and Phrases]") && !클립보드.Contains("[어구]"))
                {
                    현재단계 = "단어설명단계";

                    해설_단어 += 현재문자열.Replace("[Words and Phrases]", "").Replace("[어구]", "").Replace("[어휘 및 어구]", "").Trim() + "\r";
                }
                else if (현재단계 == "본문단계")
                {
                    해설_본문 += 현재문자열 + "\r";
                }
                else if (현재단계 == "해석단계")
                {
                    해설_해석 += 현재문자열 + "\r";
                }
                else if (현재단계 == "해설단계")
                {
                    해설_해설 += 현재문자열 + "\r";
                }
                else if (현재단계 == "단어설명단계")
                {
                    해설_단어 += 현재문자열 + "\r";
                }
            }

            if (해설_본문 != "")
                본문.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(해설_본문).Trim().듣기해설후처리();

            해석.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(해설_해석).Trim();
            힌트.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(해설_해설).Trim();
            중요어휘.Text = 해설_단어.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();

            return;
        }

        private void 질문에붙여넣기(string 클립보드)
        {
            클립보드 = 클립보드.Replace("’", "\'");     // 이런 건 얼른 얼른 좀 바꾸자.

            if(클립보드.Q에서번호추출() == "" && 질문.Text.Q에서번호추출() == "")
            {
                string 이전Q = 앞부분Q내용가져오기().Q에서번호추출();
                int 새시대의Q = 0;
                if (이전Q != "")
                {
                    새시대의Q = Convert.ToInt32(이전Q) + 1;
                    클립보드 = 새시대의Q.ToString() + ". " + 클립보드;
                }
                else if(_CAKE_인덱스 == 0)
                {
                    클립보드 = "1. " + 클립보드;
                }
                
            }


            if (클립보드.Contains("(A)") && 클립보드.Contains("(B)") && 클립보드.Contains("(C)") && (클립보드.Contains("네모") || 클립보드.Contains("빈칸")))
            {
                bool 삼점슛가능 = false;

                if (클립보드.Contains("[3점]"))
                {
                    삼점슛가능 = true;
                    클립보드 = 클립보드.Replace("[3점]", "");
                }
                string[] 구분자들 = new string[] { "은?", "(A) (B)" };
                string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                if (구분자로나뉜것들.Count() == 3)
                {
                    if (삼점슛가능)
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기() + " [3점]";
                    else
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기();

                    if (클립보드.Contains("네모") || 클립보드.Contains("낱말") || 클립보드.Contains("낱\r\n말") || 클립보드.Contains("낱\r말") || 클립보드.Contains("낱\n말") || 클립보드.Contains("낱 말"))
                        본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim().격자만들기();
                    else
                        본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim().빈칸AB밑줄넣기();

                    string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

                    if (A태그원형복원.복원결과("(A) (B)" + 구분자로나뉜것들[2], 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
                    {
                        ABC.Text = A0;
                        보기1Text.Text = A1;
                        보기2Text.Text = A2;
                        보기3Text.Text = A3;
                        보기4Text.Text = A4;
                        보기5Text.Text = A5;
                    }
                }
                else if(구분자로나뉜것들.Count() == 2) 
                {
                    if (삼점슛가능)
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기() + " [3점]";
                    else
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기();

                    if (클립보드.Contains("네모") || 클립보드.Contains("낱말") || 클립보드.Contains("낱\r\n말") || 클립보드.Contains("낱\r말") || 클립보드.Contains("낱\n말") || 클립보드.Contains("낱 말"))
                        본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim().격자만들기();
                    else
                        본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim().빈칸AB밑줄넣기();
                }
				else
				{
					질문.SelectedText = 클립보드;
				}
            }
            else if (클립보드.Contains("빈칸") && 클립보드.Contains("(A)") && 클립보드.Contains("(B)"))
            {
                bool 삼점슛가능 = false;

                if (클립보드.Contains("[3점]"))
                {
                    삼점슛가능 = true;
                    클립보드 = 클립보드.Replace("[3점]", "");
                }
                string[] 구분자들 = new string[] { "은?", "(A) (B)" };
                string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                if (구분자로나뉜것들.Count() == 3)
                {
                    if (삼점슛가능)
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기() + " [3점]";
                    else
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기();

                    본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim().빈칸AB밑줄넣기();

                    string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

                    if (A태그원형복원.복원결과("(A) (B)" + 구분자로나뉜것들[2], 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
                    {
                        ABC.Text = A0;
                        보기1Text.Text = A1;
                        보기2Text.Text = A2;
                        보기3Text.Text = A3;
                        보기4Text.Text = A4;
                        보기5Text.Text = A5;


                    }
                }
                else
                {
                    질문.SelectedText = 클립보드.질문띄어쓰기고치기();
                }
            }

            // 문맥상 낱말
            else if ((클립보드.Contains("도표의 내용") || 클립보드.Contains("어법") || 클립보드.Contains("관계 없는 문장") || 클립보드.Contains("문맥상 낱말") || 클립보드.Contains("가리키는 대상") || 클립보드.Contains("들어가기에")) && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
            {
                bool 삼점슛가능 = false;

                if (클립보드.Contains("[3점]"))
                {
                    삼점슛가능 = true;
                    클립보드 = 클립보드.Replace("[3점]", "");
                }
                string[] 구분자들 = new string[] { "은?" };

                string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                if (구분자로나뉜것들.Count() == 2)
                {
                    if (삼점슛가능)
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기() + " [3점]";
                    else
                        질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기();

                    if (!구분자로나뉜것들[1].Contains("① (a)"))
                        본문.SelectedText = 강력하지만무거운변환.문자열.개행문자제거(구분자로나뉜것들[1]).Trim();
                    else
                    {
                        string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

                        if (A태그원형복원.복원결과(구분자로나뉜것들[1], 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
                        { ABC.Text = A0; 보기1Text.Text = A1; 보기2Text.Text = A2; 보기3Text.Text = A3; 보기4Text.Text = A4; 보기5Text.Text = A5; }
                    }
                }
            }
            else if (클립보드.Contains("은?") && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
            {
                bool 삼점슛가능 = false;

                if (클립보드.Contains("[3점]")) { 삼점슛가능 = true; 클립보드 = 클립보드.Replace("[3점]", ""); }

                string[] 구분자들 = new string[] { "은?", "①" };
                string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                if (구분자로나뉜것들.Count() == 3)
                {
                    if (삼점슛가능) 질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기() + " [3점]";
                    else 질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기();

                    본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim();

                    string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

                    if (A태그원형복원.복원결과("①" + 구분자로나뉜것들[2], 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
                    { ABC.Text = A0; 보기1Text.Text = A1; 보기2Text.Text = A2; 보기3Text.Text = A3; 보기4Text.Text = A4; 보기5Text.Text = A5; }
                }
            }
            else if (클립보드.Contains("오.") && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
            {
                bool 삼점슛가능 = false;

                if (클립보드.Contains("[3점]")) { 삼점슛가능 = true; 클립보드 = 클립보드.Replace("[3점]", ""); }

                string[] 구분자들 = new string[] { "오.", "①" };
                string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                if (구분자로나뉜것들.Count() == 3)
                {
                    if (삼점슛가능) 질문.SelectedText = (구분자로나뉜것들[0] + "오.").질문띄어쓰기고치기() + " [3점]";
                    else 질문.SelectedText = (구분자로나뉜것들[0] + "오.").질문띄어쓰기고치기();

                    본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim();

                    string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

                    if (A태그원형복원.복원결과("①" + 구분자로나뉜것들[2], 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
                    { ABC.Text = A0; 보기1Text.Text = A1; 보기2Text.Text = A2; 보기3Text.Text = A3; 보기4Text.Text = A4; 보기5Text.Text = A5; }
                }
            }
            else
            {
                질문.SelectedText = 클립보드.질문띄어쓰기고치기();
            }
        }

		private string 원문자  = "①②③④⑤⑥⑦⑧⑨⑩⑪⑫";

		private void 붙여넣기()
        {

            string 클립보드 = Clipboard.GetText().Trim();
            클립보드 = 클립보드.Replace("’", "\'");     // 이런 건 얼른 얼른 좀 바꾸자.
            클립보드 = 클립보드.Replace("‘", "\'");     // 이런 건 얼른 얼른 좀 바꾸자.
            
            

            while (클립보드.Contains("✽✽"))
            {
                클립보드 = 클립보드.Replace("✽✽", "✽");
            }
            클립보드 = 클립보드.Replace("✽", "*");

            if (클립보드.Contains("<현재문제복사>"))
			{
				클립보드 = 클립보드.Replace("<현재문제복사>", "");
				클립보드 = 클립보드.Replace("</현재문제복사>", "");

				케이크표시하기_고유번호제외(클립보드);

				return;
			}

            // EBS 교재 복사 
            // 01 9049-0096 문항번호가 있고, 고유번호가 있다는 특징이 있다.
            if(클립보드.Length > 12)
            {
                if(문자열.숫자(클립보드[0]) && 문자열.숫자(클립보드[1]) && 클립보드[2] == ' ' && 문자열.숫자(클립보드[3]) && 문자열.숫자(클립보드[4]) && 문자열.숫자(클립보드[5])
                    && 문자열.숫자(클립보드[6]) && 클립보드[7] == '-' && 문자열.숫자(클립보드[8]) && 문자열.숫자(클립보드[9]) && 문자열.숫자(클립보드[10]) && 문자열.숫자(클립보드[11]))
                {

                    string Q번호 = "";
                    string 본문텍스트 = "";

                    if (클립보드[0].ToString() == "0")
                        Q번호 = 클립보드[1].ToString();
                    else
                        Q번호 = 클립보드[0].ToString() + 클립보드[1].ToString();

                    List<string> 클립보드띄어쓰기단위 = new List<string>();

                    문자열.문자열들로n(클립보드, ref 클립보드띄어쓰기단위);


                    if (클립보드띄어쓰기단위.Count > 1)
                    {
                        if (클립보드띄어쓰기단위[1].한글포함했는지확인())
                            질문.Text = Q번호 + ". " + 클립보드띄어쓰기단위[1];
                        else
                        {
                            if(string.IsNullOrEmpty(질문.Text.Trim()))
                                질문.Text = Q번호 + ". ";
                            본문텍스트 += 클립보드띄어쓰기단위[1] + "\n";
                        }

                        if (클립보드띄어쓰기단위.Count > 2)
                        {
                            if(클립보드띄어쓰기단위[2].Replace("\r", "").Contains("은?") || 클립보드띄어쓰기단위[2].Replace("\r", "").Contains("오."))
                            {
                                질문.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(질문.Text + "\n" + 클립보드띄어쓰기단위[2]);
                            }

                            for(int i = 2; i < 클립보드띄어쓰기단위.Count; i++)
                            {
                                if(i == 2 && (클립보드띄어쓰기단위[2].Replace("\r", "").Contains("은?") || 클립보드띄어쓰기단위[2].Replace("\r", "").Contains("오.")))
                                {

                                }
                                else if (클립보드띄어쓰기단위[i].Contains("①") && !질문.Text.Contains("도표") && !질문.Text.Contains("어법상 틀린") && !질문.Text.Contains("가리키는 대상이")
                                     && !질문.Text.Contains("흐름과 관계") && !질문.Text.Contains("흐름으로 보아") && !질문.Text.Contains("낱말의 쓰임이"))
                                    break;
                                else
                                    본문텍스트 += 클립보드띄어쓰기단위[i] + "\n";
                            }

                            if(string.IsNullOrEmpty(본문.Text.Trim()))
                                본문.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(본문텍스트);
                        }

                        if (클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤") 
                            && !질문.Text.Contains("도표") && !질문.Text.Contains("어법상 틀린") && !질문.Text.Contains("가리키는 대상이") && !질문.Text.Contains("흐름과 관계")
                             && !질문.Text.Contains("흐름으로 보아") && !질문.Text.Contains("낱말의 쓰임이"))
                        {
                            보기1Text.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드.Substring(클립보드.IndexOf("①") + 1, 클립보드.IndexOf("②") - 클립보드.IndexOf("①") - 1)).Trim();
                            보기2Text.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드.Substring(클립보드.IndexOf("②") + 1, 클립보드.IndexOf("③") - 클립보드.IndexOf("②") - 1)).Trim();
                            보기3Text.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드.Substring(클립보드.IndexOf("③") + 1, 클립보드.IndexOf("④") - 클립보드.IndexOf("③") - 1)).Trim();
                            보기4Text.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드.Substring(클립보드.IndexOf("④") + 1, 클립보드.IndexOf("⑤") - 클립보드.IndexOf("④") - 1)).Trim();
                            보기5Text.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드.Substring(클립보드.IndexOf("⑤") + 1)).Trim();
                        }
                    }
                    else
                    {
                        질문.Text = Q번호;
                    }
                        //
                        //
                        //
                        
                        return;
                }
            }

            // 요즘 대세가 EBS 복사이다. 2018년 식
            if (클립보드.문항번호시작여부() != -1 && 클립보드.Contains("분류 :") && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤") && 클립보드.Contains("[출제의도]"))
			{
				if (클립보드.문항번호시작여부() < 18)
				{
					질문.Text = 클립보드.Substring(0, 클립보드.IndexOf("\r\n")).Trim();
					보기1Text.Text = 클립보드.Substring(클립보드.IndexOf("①") + 1, 클립보드.IndexOf("②") - 클립보드.IndexOf("①") - 1).Replace("\r\n", " ").Trim();
					보기2Text.Text = 클립보드.Substring(클립보드.IndexOf("②") + 1, 클립보드.IndexOf("③") - 클립보드.IndexOf("②") - 1).Replace("\r\n", " ").Trim();
					보기3Text.Text = 클립보드.Substring(클립보드.IndexOf("③") + 1, 클립보드.IndexOf("④") - 클립보드.IndexOf("③") - 1).Replace("\r\n", " ").Trim();
					보기4Text.Text = 클립보드.Substring(클립보드.IndexOf("④") + 1, 클립보드.IndexOf("⑤") - 클립보드.IndexOf("④") - 1).Replace("\r\n", " ").Trim();
					보기5Text.Text = 클립보드.Substring(클립보드.IndexOf("⑤") + 1, 클립보드.IndexOf("분류 :") - 클립보드.IndexOf("⑤") - 1).Replace("\r\n", " ").Trim();
					

					if(클립보드.IndexOf("W:") < 클립보드.IndexOf("M:") && 클립보드.IndexOf("W:") != -1)
					{
						본문.Text = 클립보드.Substring(클립보드.IndexOf("W:"));
					}
					else if (클립보드.IndexOf("M:") < 클립보드.IndexOf("W:") && 클립보드.IndexOf("M:") != -1)
					{
						본문.Text = 클립보드.Substring(클립보드.IndexOf("M:"));
					}
					else if (클립보드.IndexOf("M:") == -1 && 클립보드.IndexOf("W:") != -1)
					{
						본문.Text = 클립보드.Substring(클립보드.IndexOf("W:"));
					}
					else if (클립보드.IndexOf("W:") == -1 && 클립보드.IndexOf("M:") != -1)
					{
						본문.Text = 클립보드.Substring(클립보드.IndexOf("M:"));
					}
				}
				else
				{

					보기1Text.Text = 클립보드.Substring(클립보드.IndexOf("①") + 1, 클립보드.IndexOf("②") - 클립보드.IndexOf("①") - 1).Replace("\r\n", " ").Trim();
					보기2Text.Text = 클립보드.Substring(클립보드.IndexOf("②") + 1, 클립보드.IndexOf("③") - 클립보드.IndexOf("②") - 1).Replace("\r\n", " ").Trim();
					보기3Text.Text = 클립보드.Substring(클립보드.IndexOf("③") + 1, 클립보드.IndexOf("④") - 클립보드.IndexOf("③") - 1).Replace("\r\n", " ").Trim();
					보기4Text.Text = 클립보드.Substring(클립보드.IndexOf("④") + 1, 클립보드.IndexOf("⑤") - 클립보드.IndexOf("④") - 1).Replace("\r\n", " ").Trim();
					보기5Text.Text = 클립보드.Substring(클립보드.IndexOf("⑤") + 1, 클립보드.IndexOf("분류 :") - 클립보드.IndexOf("⑤") - 1).Replace("\r\n", " ").Trim();

					if (클립보드.Contains("[2점]"))
					{
						질문.Text = 클립보드.Substring(0, 클립보드.IndexOf("\r\n")).Trim();
						본문.Text = 클립보드.Substring(클립보드.IndexOf("\r\n") + 1, 클립보드.IndexOf("[2점]") - 클립보드.IndexOf("\r\n") - 1).Trim();
					}
					else if (클립보드.Contains("[3점]"))
					{
						질문.Text = 클립보드.Substring(0, 클립보드.IndexOf("\r\n")).Trim() + " [3점]";
						본문.Text = 클립보드.Substring(클립보드.IndexOf("\r\n") + 1, 클립보드.IndexOf("[3점]") - 클립보드.IndexOf("\r\n") - 1).Trim();
					}
					해석.Text = 클립보드.Substring(클립보드.IndexOf("[해석]") + 5).Trim();
				}

				return;
			}

			if ((!클립보드.Contains("출제의도") && !클립보드.Contains("출제 의도") && !클립보드.Contains("<CAKE>") && 클립보드.Contains("10.") && 클립보드.Contains("12.") && 클립보드.Contains("13.") && 클립보드.Contains("14.")
                 && 클립보드.Contains("15.") && 클립보드.Contains("16.") && 클립보드.Contains("17.") && 클립보드.Contains("18.") && 클립보드.Contains("19.") && 클립보드.Contains("고르")) && (_시작할때열파일 != "0"))
            {
                MessageBox.Show("문제전체 복사입니다.");
                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];

                //MessageBox.Show(클립보드.문제추출(20));

                //질문에붙여넣기(클립보드.문제추출(20));
                //----------------------------------------------------------

                //treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
                //treeView1.Focus();
                //선택한항목뒤에새로운항목만들기();

                //질문에붙여넣기(클립보드.문제추출(2));

                for (int i = 0; i < 44; i++)
                {

                    //MessageBox.Show(클립보드.문제추출(i + 1));
                    질문에붙여넣기(클립보드.문제추출(i + 1));

                    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[i];
                    treeView1.Focus();
                    선택한항목뒤에새로운항목만들기();

                }

                return;
            }


            if (((클립보드.Contains("출제의도") || 클립보드.Contains("출제 의도")) && !클립보드.Contains("<CAKE>") && 클립보드.Contains("10.") && 클립보드.Contains("12.") && 클립보드.Contains("13.") && 클립보드.Contains("14.")
                 && 클립보드.Contains("15.") && 클립보드.Contains("18.") && 클립보드.Contains("19.")) && (_시작할때열파일 != "0"))
            {
                MessageBox.Show("문제해설 전체 복사입니다.");

                for (int i = 0; i < 44; i++)
                {

                    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[i];

                    해설에붙여넣기(클립보드.해설추출(i + 1));

                }

                return;
            }

            #region 해설
            if ((클립보드.Contains("출제의도") || 클립보드.Contains("출제 의도"))  && (_시작할때열파일 != "0")) // 해설
            {
                해설에붙여넣기(클립보드);
                return;
            }
            #endregion
            #region 정답지
            else if ((((클립보드.Contains(" 42 ") && 클립보드.Contains(" 34 ") && 클립보드.Contains("③"))
                || (클립보드.Contains("10.") && 클립보드.Contains("12.") && 클립보드.Contains("13.") && 클립보드.Contains("14.") && 클립보드.Contains("15.") && 클립보드.Contains("18.") && 클립보드.Contains("19.")))
                && !클립보드.Contains("고르") && _CAKE들.Count < 45 && !클립보드.Contains("㈋"))  && (_시작할때열파일 != "0")) // 이거슨 정답지 ㄷㄷㄷ
            {
                MessageBox.Show("정답지를 입력할 문항 숫자가 45개보다 적습니다.");
                return;
            }
            else if ((((클립보드.Contains(" 42 ") && 클립보드.Contains(" 34 ") && 클립보드.Contains("③"))
                || (클립보드.Contains("10.") && 클립보드.Contains("12.") && 클립보드.Contains("13.") && 클립보드.Contains("14.") && 클립보드.Contains("15.") && 클립보드.Contains("18.") && 클립보드.Contains("19.")))
                && !클립보드.Contains("고르") && _CAKE들.Count > 44 && !클립보드.Contains("㈋"))  && (_시작할때열파일 != "0")) // 이거슨 정답지 ㄷㄷㄷ
            {
                MessageBox.Show("정답지입니다.");

                클립보드 = 클립보드.Replace(".", "");
                클립보드 = 클립보드.Replace("0", "");
                클립보드 = 클립보드.Replace("1", "");
                클립보드 = 클립보드.Replace("2", "");
                클립보드 = 클립보드.Replace("3", "");
                클립보드 = 클립보드.Replace("4", "");
                클립보드 = 클립보드.Replace("5", "");
                클립보드 = 클립보드.Replace("6", "");
                클립보드 = 클립보드.Replace("7", "");
                클립보드 = 클립보드.Replace("8", "");
                클립보드 = 클립보드.Replace("9", "");
                클립보드 = 클립보드.Replace(" ", "");
                클립보드 = 클립보드.Replace("\r", "");
                클립보드 = 클립보드.Replace("\n", "");
                클립보드 = 클립보드.Replace("\t", "");

                클립보드 = 클립보드.Replace("①", "①|");
                클립보드 = 클립보드.Replace("②", "②|");
                클립보드 = 클립보드.Replace("③", "③|");
                클립보드 = 클립보드.Replace("④", "④|");
                클립보드 = 클립보드.Replace("⑤", "⑤|");

                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[3];
                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[4];

                string[] 구분자들 = new string[] { "|" };
                string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                if (구분자로나뉜것들.Count() == 45)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        treeView1.SelectedNode = treeView1.Nodes[0].Nodes[i];

                        if (구분자로나뉜것들[i] == "①") { 보기1.Checked = true; 보기2.Checked = false; 보기3.Checked = false; 보기4.Checked = false; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "②") { 보기1.Checked = false; 보기2.Checked = true; 보기3.Checked = false; 보기4.Checked = false; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "③") { 보기1.Checked = false; 보기2.Checked = false; 보기3.Checked = true; 보기4.Checked = false; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "④") { 보기1.Checked = false; 보기2.Checked = false; 보기3.Checked = false; 보기4.Checked = true; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "⑤") { 보기1.Checked = false; 보기2.Checked = false; 보기3.Checked = false; 보기4.Checked = false; 보기5.Checked = true; }

                    }
                }

                return;
            }
            else if ((((클립보드.Contains(" 28 ") && 클립보드.Contains(" 27 ") && 클립보드.Contains("③"))
                || (클립보드.Contains("10.") && 클립보드.Contains("12.") && 클립보드.Contains("13.") && 클립보드.Contains("14.") && 클립보드.Contains("15.") && 클립보드.Contains("18.") && 클립보드.Contains("19.")))
                && !클립보드.Contains("고르") && _CAKE들.Count > 27 && !클립보드.Contains("㈋")) && (_시작할때열파일 != "0")) // 이거슨 정답지 ㄷㄷㄷ
            {
                MessageBox.Show("EBS Test 정답지입니다.");

                클립보드 = 클립보드.Replace(".", "");
                클립보드 = 클립보드.Replace("0", "");
                클립보드 = 클립보드.Replace("1", "");
                클립보드 = 클립보드.Replace("2", "");
                클립보드 = 클립보드.Replace("3", "");
                클립보드 = 클립보드.Replace("4", "");
                클립보드 = 클립보드.Replace("5", "");
                클립보드 = 클립보드.Replace("6", "");
                클립보드 = 클립보드.Replace("7", "");
                클립보드 = 클립보드.Replace("8", "");
                클립보드 = 클립보드.Replace("9", "");
                클립보드 = 클립보드.Replace(" ", "");
                클립보드 = 클립보드.Replace("\r", "");
                클립보드 = 클립보드.Replace("\n", "");
                클립보드 = 클립보드.Replace("\t", "");

                클립보드 = 클립보드.Replace("①", "①|");
                클립보드 = 클립보드.Replace("②", "②|");
                클립보드 = 클립보드.Replace("③", "③|");
                클립보드 = 클립보드.Replace("④", "④|");
                클립보드 = 클립보드.Replace("⑤", "⑤|");

                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[3];
                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[4];

                string[] 구분자들 = new string[] { "|" };
                string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                if (구분자로나뉜것들.Count() == 28)
                {
                    for (int i = 0; i < 28; i++)
                    {
                        treeView1.SelectedNode = treeView1.Nodes[0].Nodes[i];

                        if (구분자로나뉜것들[i] == "①") { 보기1.Checked = true; 보기2.Checked = false; 보기3.Checked = false; 보기4.Checked = false; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "②") { 보기1.Checked = false; 보기2.Checked = true; 보기3.Checked = false; 보기4.Checked = false; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "③") { 보기1.Checked = false; 보기2.Checked = false; 보기3.Checked = true; 보기4.Checked = false; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "④") { 보기1.Checked = false; 보기2.Checked = false; 보기3.Checked = false; 보기4.Checked = true; 보기5.Checked = false; }
                        if (구분자로나뉜것들[i] == "⑤") { 보기1.Checked = false; 보기2.Checked = false; 보기3.Checked = false; 보기4.Checked = false; 보기5.Checked = true; }

                    }
                }

                return;
            }

            #endregion
            if (제목.Focused && (_시작할때열파일 != "0")) { 제목.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " "); }
			#region 질문
			else if (질문.Focused && (_시작할때열파일 != "0"))
			{
				질문에붙여넣기(클립보드);
			}
			#endregion
			#region 본문
			else if (본문.Focused || _시작할때열파일 == "0")
			{

				화면업데이트중지();

				현재내용을_실행취소용_클립보드에_저장();

				if (String.IsNullOrEmpty(클립보드)) return;

				if (클립보드.Contains("㈋")) 클립보드 = 변환.문자열.복호화(클립보드);
                
                
                클립보드 = 클립보드.Replace("✽", "*");
                클립보드 = 클립보드.Replace("\ue10b", "*"); // 지원하지 않는 체크표시를 별표로 바꿔줌
				클립보드 = 클립보드.Replace("\u2013", "-"); // 지원하지 않는 - 표시를 - 로 바꿔줌
				클립보드 = 클립보드.Replace("\u2014", "-"); // 지원하지 않는 - 표시를 - 로 바꿔줌

				if (_최근복사한클립보드내용 == 클립보드)
				{
					본문.SelectedText = 클립보드;
				}
				else if (클립보드.Contains("<CAKE>"))
				{
					if (_CAKE들.Count() == 1 && 본문.Text.Trim() == "")
						열기_파일내용으로(클립보드, "", true);
				}
				else if (클립보드.Contains("[어구]") && 클립보드.Contains("[해설]"))
				{
					if (클립보드.IndexOf("[어구]") < 클립보드.IndexOf("[해설]"))
					{
						string[] 구분자들 = new string[] { "[어구]", "[해설]" };
						string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

						if (구분자로나뉜것들.Count() == 3)
						{
							if (구분자로나뉜것들[0].Contains("W:") || 구분자로나뉜것들[0].Contains("M:"))
							{
								본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[0]).Trim();
								힌트.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[2]).Trim();
								중요어휘.Text = 구분자로나뉜것들[1].Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
							}
						}

					}
				}
                else if ((클립보드.Contains("낱말의 쓰임이") || 클립보드.Contains("나머지 넷과") || 클립보드.Contains("관계 없는") || 클립보드.Contains("주어진 문장이")) && 클립보드.Contains("은?") 
                    && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
                {
                    if (클립보드.Q에서번호추출() == "" && 질문.Text.Q에서번호추출() == "")
                    {
                        string 이전Q = 앞부분Q내용가져오기().Q에서번호추출();
                        int 새시대의Q = 0;
                        if (이전Q != "")
                        {
                            새시대의Q = Convert.ToInt32(이전Q) + 1;
                            클립보드 = 새시대의Q.ToString() + ". " + 클립보드;
                        }
                        else if (_CAKE_인덱스 == 0){ 클립보드 = "1. " + 클립보드; }
                    }


                    bool 삼점슛가능 = false;

                    if (클립보드.Contains("[3점]")) { 삼점슛가능 = true; 클립보드 = 클립보드.Replace("[3점]", ""); }

                    string[] 구분자들 = new string[] { "은?" };
                    string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                    if (구분자로나뉜것들.Count() == 2)
                    {
                        if (삼점슛가능) 질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기() + " [3점]";
                        else 질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기();

                        본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim();
                    }
                }
                else if (클립보드.Contains("은?") && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
                {
                    if (클립보드.Q에서번호추출() == "" && 질문.Text.Q에서번호추출() == "")
                    {
                        string 이전Q = 앞부분Q내용가져오기().Q에서번호추출();
                        int 새시대의Q = 0;
                        if (이전Q != "")
                        {
                            새시대의Q = Convert.ToInt32(이전Q) + 1;
                            클립보드 = 새시대의Q.ToString() + ". " + 클립보드;
                        }
                        else if (_CAKE_인덱스 == 0)
                        {
                            클립보드 = "1. " + 클립보드;
                        }

                    }


                    bool 삼점슛가능 = false;

                    if (클립보드.Contains("[3점]")) { 삼점슛가능 = true; 클립보드 = 클립보드.Replace("[3점]", ""); }

                    string[] 구분자들 = new string[] { "은?", "①" };
                    string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                    if (구분자로나뉜것들.Count() == 3)
                    {
                        if (삼점슛가능) 질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기() + " [3점]";
                        else 질문.SelectedText = (구분자로나뉜것들[0] + "은?").질문띄어쓰기고치기();

                        본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim();

                        string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

                        if (A태그원형복원.복원결과("①" + 구분자로나뉜것들[2], 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
                        { ABC.Text = A0; 보기1Text.Text = A1; 보기2Text.Text = A2; 보기3Text.Text = A3; 보기4Text.Text = A4; 보기5Text.Text = A5; }
                    }
                }
                else if (클립보드.Contains("오.") && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
                {
                    if (클립보드.Q에서번호추출() == "" && 질문.Text.Q에서번호추출() == "")
                    {
                        string 이전Q = 앞부분Q내용가져오기().Q에서번호추출();
                        int 새시대의Q = 0;
                        if (이전Q != "")
                        {
                            새시대의Q = Convert.ToInt32(이전Q) + 1;
                            클립보드 = 새시대의Q.ToString() + ". " + 클립보드;
                        }
                        else if (_CAKE_인덱스 == 0)
                        {
                            클립보드 = "1. " + 클립보드;
                        }

                    }

                    bool 삼점슛가능 = false;

                    if (클립보드.Contains("[3점]")) { 삼점슛가능 = true; 클립보드 = 클립보드.Replace("[3점]", ""); }

                    string[] 구분자들 = new string[] { "오.", "①" };
                    string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

                    if (구분자로나뉜것들.Count() == 3)
                    {
                        if (삼점슛가능) 질문.SelectedText = (구분자로나뉜것들[0] + "오.").질문띄어쓰기고치기() + " [3점]";
                        else 질문.SelectedText = (구분자로나뉜것들[0] + "오.").질문띄어쓰기고치기();

                        본문.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim();

                        string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

                        if (A태그원형복원.복원결과("①" + 구분자로나뉜것들[2], 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
                        { ABC.Text = A0; 보기1Text.Text = A1; 보기2Text.Text = A2; 보기3Text.Text = A3; 보기4Text.Text = A4; 보기5Text.Text = A5; }
                    }
                }

                else
                {
					if (클립보드.Trim().부분문자열분리(0, 3) == "(A)" && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
					{
						string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

						if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
						{
							ABC.Text = A0;
							보기1Text.Text = A1;
							보기2Text.Text = A2;
							보기3Text.Text = A3;
							보기4Text.Text = A4;
							보기5Text.Text = A5;

							본문.SelectedText = "";
						}
					}

					// ①이①두 번 나오는 경우로, 대개는 처음의 ①은 영어, 그다음 ①은 한국어이다.
					else if (클립보드.Trim().StartsWith("①") && 클립보드.Trim().Substring(1).Contains("①"))
					{
						클립보드 = 클립보드.Trim();
						string 영문 = "", 국문 = "";

						int 처음원문자위치 = -2;		// ①이 처음 출현할 때,
						int 반복되는원문자위치 = -2;	// ①이 두번째로 출현할 때,
						int 그다음원문자위치 = -2;      // ②이 출현할 때,

						int 마지막처리위치 = 0;

						for (int i = 0; i <원문자.Length; i++)
						{


							처음원문자위치 = 클립보드.Trim().IndexOf(원문자[i]);

							반복되는원문자위치 = 클립보드.Substring(처음원문자위치 + 1).IndexOf(원문자[i]) + 처음원문자위치;

							if (i != 원문자.Length - 1)
								그다음원문자위치 = 클립보드.Trim().IndexOf(원문자[i + 1]);
							else
								그다음원문자위치 = -1;

							if (처음원문자위치 != -1 && 반복되는원문자위치 != 처음원문자위치 - 1 && 그다음원문자위치 != -1)
							{
								영문 += 클립보드.Substring(처음원문자위치 + 1, 반복되는원문자위치 - 처음원문자위치);

								국문 += 클립보드.Substring(반복되는원문자위치 + 3, 그다음원문자위치 - 반복되는원문자위치 - 3);

								마지막처리위치 = 그다음원문자위치;
							}
							else if (처음원문자위치 != -1 && 반복되는원문자위치 != 처음원문자위치 - 1)
							{
								영문 += 클립보드.Substring(처음원문자위치 + 1, 반복되는원문자위치 - 처음원문자위치);

								국문 += 클립보드.Substring(반복되는원문자위치 + 3);

								마지막처리위치 = -1; // 정상종료됨
							}


						}
						if(마지막처리위치 != -1)
						{
							영문 += 클립보드.Substring(마지막처리위치);
						}

						본문.SelectedText = 영문;
						해석.SelectedText = 국문;
					}

					// "①"번이 두번 나오면, 문제의 보기는 아닙니다. 즉, 맨처음 "①"로 시작한 이후에는, "①"이 나와서는 안됩니다.
					else if (클립보드.Trim().부분문자열분리(0, 1) == "①" && !클립보드.Trim().Substring(1).Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤") && !클립보드.Contains("⑥"))
					{
						string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

						if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
						{
							ABC.Text = A0;
							보기1Text.Text = A1;
							보기2Text.Text = A2;
							보기3Text.Text = A3;
							보기4Text.Text = A4;
							보기5Text.Text = A5;

							본문.SelectedText = "";
						}
					}
					else
					{
						if (질문.Text.Contains("빈칸") || 질문.Text.Contains("41."))
							클립보드 = 클립보드.Replace(" .", " ______.");

						if (클립보드.Contains("[3점]"))
						{
							클립보드 = 클립보드.Replace("[3점]", "");

							if (!질문.Text.Contains("[3점]"))
							{
								질문.Text += " [3점]";
								질문.Text = 질문.Text.Replace("  ", " ");
							}
						}


						클립보드 = 클립보드.Replace("，", ", ");

						클립보드 = 클립보드.Replace("①`", "① ");
						클립보드 = 클립보드.Replace("②`", "② ");
						클립보드 = 클립보드.Replace("③`", "③ ");
						클립보드 = 클립보드.Replace("④`", "④ ");
						클립보드 = 클립보드.Replace("⑤`", "⑤ ");

						if (질문.Text.Contains("안내문"))
						{
							;
						}
						else
						{
							클립보드 = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드);
						}
						if (클립보드.Contains("https://www.youtube.com") && 클립보드.Contains("?list="))
						{
							int i1 = 클립보드.IndexOf("https://www.youtube.com");
							int i2 = 클립보드.IndexOf("?list=");

							클립보드 = "\t" + 클립보드.Substring(i1, i2 - i1);
						}

						if (질문.Text.Contains("빈칸") && 질문.Text.Contains("(A)") && 질문.Text.Contains("(B)"))
						{
							클립보드 = 클립보드.빈칸AB밑줄넣기();
						}

						while (클립보드.Contains("_______"))
						{
							클립보드 = 클립보드.Replace("_______", "______");
						}

						선택위치에바꿀말넣고키워드색상입히기(클립보드);
					}
				}

				화면업데이트재개();

				//_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();
			}
			#endregion
			#region ABC
			else if (ABC.Focused)
			{
				if (클립보드.Trim().부분문자열분리(0, 3) == "(A)" && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else if (클립보드.Trim().부분문자열분리(0, 1) == "①" && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else
					ABC.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ");
			}
			#endregion
			#region 보기1
			else if (보기1Text.Focused)
			{
				if (클립보드.Trim().부분문자열분리(0, 3) == "(A)" && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else if (클립보드.Trim().부분문자열분리(0, 1) == "①" && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1.Replace("\r", " ").Replace("  ", " ");
						보기2Text.Text = A2.Replace("\r", " ").Replace("  ", " ");
						보기3Text.Text = A3.Replace("\r", " ").Replace("  ", " ");
						보기4Text.Text = A4.Replace("\r", " ").Replace("  ", " ");
						보기5Text.Text = A5.Replace("\r", " ").Replace("  ", " ");

						본문.SelectedText = "";
					}
				}
				else
					보기1Text.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ");

			}
			#endregion
			#region 보기2
			else if (보기2Text.Focused)
			{
				if (클립보드.Trim().부분문자열분리(0, 3) == "(A)" && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else if (클립보드.Trim().부분문자열분리(0, 1) == "①" && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else
					보기2Text.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ");
			}
			#endregion
			#region 보기3
			else if (보기3Text.Focused)
			{
				if (클립보드.Trim().부분문자열분리(0, 3) == "(A)" && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else if (클립보드.Trim().부분문자열분리(0, 1) == "①" && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else
					보기3Text.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ");
			}
			#endregion
			#region 보기4
			else if (보기4Text.Focused)
			{
				if (클립보드.Trim().부분문자열분리(0, 3) == "(A)" && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else if (클립보드.Trim().부분문자열분리(0, 1) == "①" && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else
					보기4Text.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ");
			}
			#endregion
			#region 보기5
			else if (보기5Text.Focused)
			{
				if (클립보드.Trim().부분문자열분리(0, 3) == "(A)" && 클립보드.Contains("①") && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else if (클립보드.Trim().부분문자열분리(0, 1) == "①" && 클립보드.Contains("②") && 클립보드.Contains("③") && 클립보드.Contains("④") && 클립보드.Contains("⑤"))
				{
					string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

					if (A태그원형복원.복원결과(클립보드, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
					{
						ABC.Text = A0;
						보기1Text.Text = A1;
						보기2Text.Text = A2;
						보기3Text.Text = A3;
						보기4Text.Text = A4;
						보기5Text.Text = A5;

						본문.SelectedText = "";
					}
				}
				else
					보기5Text.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ");
			}
			#endregion
			else if (주관식정답.Focused) { 주관식정답.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " "); }
			else if (해석.Focused)
			{
				if (_최근복사한클립보드내용 == 클립보드)
				{
					해석.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드);
				}
				else if (클립보드.Contains("[해석]") && 클립보드.Contains("[어구]") && 클립보드.Contains("[해설]"))
				{
					if ((클립보드.IndexOf("[해석]") < 클립보드.IndexOf("[어구]")) && (클립보드.IndexOf("[어구]") < 클립보드.IndexOf("[해설]")))
					{
						string[] 구분자들 = new string[] { "[해석]", "[어구]", "[해설]" };
						string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

						if (구분자로나뉜것들.Count() == 3)
						{
							해석.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[0]).Trim();
							힌트.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[2]).Trim();
							중요어휘.Text = 구분자로나뉜것들[1].Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
						}

					}
					else if ((클립보드.IndexOf("[해석]") < 클립보드.IndexOf("[해설]")) && (클립보드.IndexOf("[해설]") < 클립보드.IndexOf("[어구]")))
					{
						string[] 구분자들 = new string[] { "[해석]", "[해설]", "[어구]" };
						string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

						if (구분자로나뉜것들.Count() == 3)
						{
							해석.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[0]).Trim();
							힌트.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]).Trim();
							중요어휘.Text = 구분자로나뉜것들[2].Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
						}
					}
				}
				else if (클립보드.Contains("[어구]") && 클립보드.Contains("[해설]"))
				{
					if (클립보드.IndexOf("[어구]") < 클립보드.IndexOf("[해설]"))
					{
						string[] 구분자들 = new string[] { "[어구]", "[해설]" };
						string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

						if (구분자로나뉜것들.Count() == 3)
						{
							if (구분자로나뉜것들[0].Contains("W:") || 구분자로나뉜것들[0].Contains("M:"))
							{
								본문.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[0]).Trim();
								힌트.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[2]).Trim();
								중요어휘.Text = 구분자로나뉜것들[1].Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
							}
						}

					}
				}

				else if (클립보드.Contains("[해석]") && 클립보드.Contains("[풀이]") && 클립보드.Contains("[Words and Phrases]"))
				{
					if ((클립보드.IndexOf("[해석]") < 클립보드.IndexOf("[풀이]")) && (클립보드.IndexOf("[풀이]") < 클립보드.IndexOf("[Words and Phrases]")))
					{
						string[] 구분자들 = new string[] { "[해석]", "[풀이]", "[Words and Phrases]" };
						string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

						if (구분자로나뉜것들.Count() == 3)
						{
							해석.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[0]);
							힌트.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]);
							중요어휘.Text = 구분자로나뉜것들[2].Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
						}
					}
				}
				
				else if (클립보드.Contains("[해석]") && 클립보드.Contains("[풀이]") && 클립보드.Contains("[Words & Phrases]"))
				{
					if ((클립보드.IndexOf("[해석]") < 클립보드.IndexOf("[풀이]")) && (클립보드.IndexOf("[풀이]") < 클립보드.IndexOf("[Words & Phrases]")))
					{
						string[] 구분자들 = new string[] { "[해석]", "[풀이]", "[Words & Phrases]" };
						string[] 구분자로나뉜것들 = 클립보드.Split(구분자들, StringSplitOptions.RemoveEmptyEntries);

						if (구분자로나뉜것들.Count() == 3)
						{
							해석.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[0]);
							힌트.Text = 강력하지만무거운변환.문자열.지능형개행문자제거(구분자로나뉜것들[1]);
							중요어휘.Text = 구분자로나뉜것들[2].Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim();
						}
					}
				}
				else
				{
					해석.SelectedText = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드);
                }
			}
			else if (힌트.Focused)
			{
				if (클립보드.Contains("https://www.youtube.com") && 클립보드.Contains("\" frameborder=\"0\""))
				{
					int i1 = 클립보드.IndexOf("https://www.youtube.com");
					int i2 = 클립보드.IndexOf("?list=");
					int i3 = 클립보드.IndexOf("\" frameborder=\"0\"");
					if (i2 != -1 && i1 < i2)
					{
						힌트.SelectedText = 클립보드.Substring(i1, i2 - i1);
					}
					else if (i1 < i3)
					{
						힌트.SelectedText = 클립보드.Substring(i1, i3 - i1);
					}
				}
				else
				{
					클립보드 = 강력하지만무거운변환.문자열.지능형개행문자제거(클립보드);
					힌트.SelectedText = 클립보드;
				}

			}
			else if (중요어휘.Focused) { 중요어휘.SelectedText = 클립보드.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Trim(); }
			else if (사전페이지_사전표제어.Focused) { 사전페이지_사전표제어.SelectedText = 클립보드.Trim(); }
			else if (사전페이지_사전발음기호.Focused) { 사전페이지_사전발음기호.SelectedText = 클립보드.Trim(); }
			else if (사전페이지_사전의미.Focused) { 사전페이지_사전의미.SelectedText = 클립보드.Trim(); }
			else if (보기1_해설.Focused) { 보기1_해설.SelectedText = 클립보드.Trim(); }
			else if (보기2_해설.Focused) { 보기2_해설.SelectedText = 클립보드.Trim(); }
			else if (보기3_해설.Focused) { 보기3_해설.SelectedText = 클립보드.Trim(); }
			else if (보기4_해설.Focused) { 보기4_해설.SelectedText = 클립보드.Trim(); }
			else if (보기5_해설.Focused) { 보기5_해설.SelectedText = 클립보드.Trim(); }

			else if (주제.Focused) { 주제.SelectedText = 클립보드.Trim(); }
			else if (변형지문.Focused) { 변형지문.SelectedText = 클립보드.Trim(); }
			else if (변형지문해석.Focused) { 변형지문해석.SelectedText = 클립보드.Trim(); }
		}

		private void 붙여넣기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            붙여넣기();
		}
		private void 삭제ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.Focused)
			{
				if (treeView1.SelectedNode.Level == 1)
				{
					_편집시작 = true; 

					_CAKE_인덱스 = treeView1.SelectedNode.Index;

					_CAKE들.RemoveAt(_CAKE_인덱스);
					treeView1.Nodes[0].Nodes.RemoveAt(_CAKE_인덱스);

					// _CAKE_인덱스--; 이걸 쓰면 안되는 이유가, 삭제가 되면 선택이 바뀌고, 그부분에서 업데이트가 됨, 그래서 2번 빠지는 효과

                    if (_CAKE들.Count() <= _CAKE_인덱스)
						_CAKE_인덱스 = _CAKE들.Count() - 1;

					if(_CAKE_인덱스 >= 0)
	                    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스];

					if(_CAKE들.Count() == 0)
						케이크표시하기("");
				}
				else if (treeView1.SelectedNode.Level == 2)
				{
					_편집시작 = true;

					_CAKE_인덱스 = treeView1.SelectedNode.Parent.Index;
					_SUGAR_인덱스 = treeView1.SelectedNode.Index;

					string 상위노드케이크 = _CAKE들[treeView1.SelectedNode.Parent.Index];

					string 순수현재CAKE = "", 예상문제 = "";

					변환.문자열.파일내용과_예상문제_분리(상위노드케이크, ref 순수현재CAKE, ref 예상문제);

					List<string> SUGAR들 = new List<string>();
					변환.문자열.CAKE들로(예상문제, ref SUGAR들);

					SUGAR들.RemoveAt(treeView1.SelectedNode.Index);



					string 바꿀CAKE = 순수현재CAKE.Replace("</CAKE>", "").Trim();
					바꿀CAKE += "\r\n<SUGAR>\r\n";
					
					for(int i = 0; i < SUGAR들.Count(); i++)
						바꿀CAKE += SUGAR들[i] + "\r\n";

					바꿀CAKE += "\r\n</SUGAR>" + "\r\n</CAKE>\r\n";

					_CAKE들[treeView1.SelectedNode.Parent.Index] = 바꿀CAKE;

					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes.RemoveAt(_SUGAR_인덱스);

					if (SUGAR들.Count() <= _SUGAR_인덱스)
						_SUGAR_인덱스 = SUGAR들.Count() - 1;

					if (_SUGAR_인덱스 >= 0)
						treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes[_SUGAR_인덱스];

					if (SUGAR들.Count() == 0)
						treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스];

				}
			}
			#region 오른쪽 창
			if (제목.Focused)
			{
				if (제목.SelectedText != "") 제목.SelectedText = "";
				else if (제목.TextLength != 제목.SelectionStart)
				{
					제목.SelectionLength++;
					제목.SelectedText = "";
				}
			}
			else if (질문.Focused)
			{
				if (질문.SelectedText != "") 질문.SelectedText = "";
				else if (질문.TextLength != 질문.SelectionStart)
				{
					질문.SelectionLength++;
					질문.SelectedText = "";
				}
			}
			else if (본문.Focused)
			{
				현재내용을_실행취소용_클립보드에_저장();
				_텍스트변경사항자동저장불필요 = true;

				if (본문.SelectedText != "")
				{
					본문.SelectedText = "";

					현재커서근방의키워드색상업데이트();
				}
				else
				{
					if (본문.TextLength != 본문.SelectionStart)
					{
						본문.SelectionLength++;
						본문.SelectedText = "";
						현재커서근방의키워드색상업데이트();
					}
				}

				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();
			}
			else if (ABC.Focused)
			{
				if (ABC.SelectedText != "") ABC.SelectedText = "";
				else if (ABC.TextLength != ABC.SelectionStart)
				{
					ABC.SelectionLength++;
					ABC.SelectedText = "";
				}
			}
			else if (보기1Text.Focused)
			{
				if (보기1Text.SelectedText != "") 보기1Text.SelectedText = "";
				else if (보기1Text.TextLength != 보기1Text.SelectionStart)
				{
					보기1Text.SelectionLength++;
					보기1Text.SelectedText = "";
				}
			}
			else if (보기2Text.Focused)
			{
				if (보기2Text.SelectedText != "") 보기2Text.SelectedText = "";
				else if (보기2Text.TextLength != 보기2Text.SelectionStart)
				{
					보기2Text.SelectionLength++;
					보기2Text.SelectedText = "";
				}
			}
			else if (보기3Text.Focused)
			{
				if (보기3Text.SelectedText != "") 보기3Text.SelectedText = "";
				else if (보기3Text.TextLength != 보기3Text.SelectionStart)
				{
					보기3Text.SelectionLength++;
					보기3Text.SelectedText = "";
				}
			}
			else if (보기4Text.Focused)
			{
				if (보기4Text.SelectedText != "") 보기4Text.SelectedText = "";
				else if (보기4Text.TextLength != 보기4Text.SelectionStart)
				{
					보기4Text.SelectionLength++;
					보기4Text.SelectedText = "";
				}
			}
			else if (보기5Text.Focused)
			{
				if (보기5Text.SelectedText != "") 보기5Text.SelectedText = "";
				else if (보기5Text.TextLength != 보기5Text.SelectionStart)
				{
					보기5Text.SelectionLength++;
					보기5Text.SelectedText = "";
				}
			}
			else if (보기1_해설.Focused)
			{
				if (보기1_해설.SelectedText != "") 보기1_해설.SelectedText = "";
				else if (보기1_해설.TextLength != 보기1_해설.SelectionStart)
				{
					보기1_해설.SelectionLength++;
					보기1_해설.SelectedText = "";
				}
			}
			else if (보기2_해설.Focused)
			{
				if (보기2_해설.SelectedText != "") 보기2_해설.SelectedText = "";
				else if (보기2_해설.TextLength != 보기2_해설.SelectionStart)
				{
					보기2_해설.SelectionLength++;
					보기2_해설.SelectedText = "";
				}
			}
			else if (보기3_해설.Focused)
			{
				if (보기3_해설.SelectedText != "") 보기3_해설.SelectedText = "";
				else if (보기3_해설.TextLength != 보기3_해설.SelectionStart)
				{
					보기3_해설.SelectionLength++;
					보기3_해설.SelectedText = "";
				}
			}
			else if (보기4_해설.Focused)
			{
				if (보기4_해설.SelectedText != "") 보기4_해설.SelectedText = "";
				else if (보기4_해설.TextLength != 보기4_해설.SelectionStart)
				{
					보기4_해설.SelectionLength++;
					보기4_해설.SelectedText = "";
				}
			}
			else if (보기5_해설.Focused)
			{
				if (보기5_해설.SelectedText != "") 보기5_해설.SelectedText = "";
				else if (보기5_해설.TextLength != 보기5_해설.SelectionStart)
				{
					보기5_해설.SelectionLength++;
					보기5_해설.SelectedText = "";
				}
			}
			else if (주관식정답.Focused)
			{
				if (주관식정답.SelectedText != "") 주관식정답.SelectedText = "";
				else if (주관식정답.TextLength != 주관식정답.SelectionStart)
				{
					주관식정답.SelectionLength++;
					주관식정답.SelectedText = "";
				}
			}
			else if (해석.Focused)
			{
				if (해석.SelectedText != "") 해석.SelectedText = "";
				else if (해석.TextLength != 해석.SelectionStart)
				{
					해석.SelectionLength++;
					해석.SelectedText = "";
				}
			}
			else if (힌트.Focused)
			{
				if (힌트.SelectedText != "") 힌트.SelectedText = "";
				else if (힌트.TextLength != 힌트.SelectionStart)
				{
					힌트.SelectionLength++;
					힌트.SelectedText = "";
				}
			}
			else if (중요어휘.Focused)
			{
				if (중요어휘.SelectedText != "") 중요어휘.SelectedText = "";
				else if (중요어휘.TextLength != 중요어휘.SelectionStart)
				{
					중요어휘.SelectionLength++;
					중요어휘.SelectedText = "";
				}
			}
			else if (사전페이지_사전표제어.Focused)
			{ 
				if (사전페이지_사전표제어.SelectedText != "") 사전페이지_사전표제어.SelectedText = "";

				else if (사전페이지_사전표제어.TextLength != 사전페이지_사전표제어.SelectionStart) // 선택영역을 하나 늘려주고, 지운다. 즉 선택영역이 없으면 뒷부분 한 글자를 선택해서 지우는 방식
				{
					사전페이지_사전표제어.SelectionLength++;
					사전페이지_사전표제어.SelectedText = "";
				}
			}
			else if (사전페이지_사전발음기호.Focused)
			{ 
				if (사전페이지_사전발음기호.SelectedText != "") 사전페이지_사전발음기호.SelectedText = "";

				else if (사전페이지_사전발음기호.TextLength != 사전페이지_사전발음기호.SelectionStart) // 선택영역을 하나 늘려주고, 지운다. 즉 선택영역이 없으면 뒷부분 한 글자를 선택해서 지우는 방식
				{
					사전페이지_사전발음기호.SelectionLength++;
					사전페이지_사전발음기호.SelectedText = "";
				}
			}
			else if (사전페이지_사전발음기호.Focused)
			{ 
				if (사전페이지_사전발음기호.SelectedText != "") 사전페이지_사전발음기호.SelectedText = "";

				else if (사전페이지_사전발음기호.TextLength != 사전페이지_사전발음기호.SelectionStart) // 선택영역을 하나 늘려주고, 지운다. 즉 선택영역이 없으면 뒷부분 한 글자를 선택해서 지우는 방식
				{
					사전페이지_사전발음기호.SelectionLength++;
					사전페이지_사전발음기호.SelectedText = "";
				}
			}
			else if (사전페이지_사전의미.Focused)
			{ 
				if (사전페이지_사전의미.SelectedText != "") 사전페이지_사전의미.SelectedText = "";

				else if (사전페이지_사전의미.TextLength != 사전페이지_사전의미.SelectionStart) // 선택영역을 하나 늘려주고, 지운다. 즉 선택영역이 없으면 뒷부분 한 글자를 선택해서 지우는 방식
				{
					사전페이지_사전의미.SelectionLength++;
					사전페이지_사전의미.SelectedText = "";
				}
			}

			else if (주제.Focused)
			{
				if (주제.SelectedText != "") 주제.SelectedText = "";

				else if (주제.TextLength != 주제.SelectionStart) // 선택영역을 하나 늘려주고, 지운다. 즉 선택영역이 없으면 뒷부분 한 글자를 선택해서 지우는 방식
				{
					주제.SelectionLength++;
					주제.SelectedText = "";
				}
			}

			else if (변형지문.Focused)
			{
				if (변형지문.SelectedText != "") 변형지문.SelectedText = "";

				else if (변형지문.TextLength != 변형지문.SelectionStart) // 선택영역을 하나 늘려주고, 지운다. 즉 선택영역이 없으면 뒷부분 한 글자를 선택해서 지우는 방식
				{
					변형지문.SelectionLength++;
					변형지문.SelectedText = "";
				}
			}

			else if (변형지문해석.Focused)
			{
				if (변형지문해석.SelectedText != "") 변형지문해석.SelectedText = "";

				else if (변형지문해석.TextLength != 변형지문해석.SelectionStart) // 선택영역을 하나 늘려주고, 지운다. 즉 선택영역이 없으면 뒷부분 한 글자를 선택해서 지우는 방식
				{
					변형지문해석.SelectionLength++;
					변형지문해석.SelectedText = "";
				}
			}

			#endregion
		}
		private void 찾기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Point 선택내용의위치 = 본문.GetPositionFromCharIndex(본문.SelectionStart + 본문.SelectedText.Length);

			찾기다이얼로그 찾기다이얼로그 = new 찾기다이얼로그(본문.SelectedText);
			찾기다이얼로그.StartPosition = FormStartPosition.Manual;

			선택내용의위치.X += this.Location.X + 11;
			선택내용의위치.Y += this.Location.Y + 54;

			찾기다이얼로그.Location = 선택내용의위치;

			찾기다이얼로그.Owner = this;
			찾기다이얼로그.Show();
		}
		public bool 다음찾기(string 찾을말, bool 정방향인가) // 찾기다이얼로그의 다음찾기
		{
			_찾은내용 = 찾을말;

			if (정방향인가)
			{
				if (본문.SelectionStart == 본문.TextLength)
					return false;

				int 다음위치 = 본문.Text.IndexOf(찾을말, 본문.SelectionStart);

				if (다음위치 == 본문.SelectionStart)
					다음위치 = 본문.Text.IndexOf(찾을말, 본문.SelectionStart + 1);

				if (다음위치 != -1)
				{
					본문.SelectionStart = 다음위치;
					본문.SelectionLength = 찾을말.Length;
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return true;
			}

		}
		public bool 바꾸기(string 찾을말, string 바꿀말)
		{
			_찾은내용 = 찾을말;


			if (본문.SelectedText == 찾을말)
			{
				현재내용을_실행취소용_클립보드에_저장();
				_텍스트변경사항자동저장불필요 = true;

				본문.SelectedText = 바꿀말;
				현재커서근방의키워드색상업데이트();
				본문.SelectionStart -= 바꿀말.Length;
				현재커서근방의키워드색상업데이트();
				본문.SelectionStart += 바꿀말.Length;

				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();

				return 다음찾기(찾을말, true);
			}
			else
			{
				return 다음찾기(찾을말, true);
			}
		}
		public int 모두바꾸기(string 찾을말, string 바꿀말)
		{

			본문.Text = 본문.Text.Replace(찾을말, 바꿀말);

			return 0;
		}
		private void 다음찾기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_찾은내용 == "")
				return;

			if (본문.SelectionStart == 본문.TextLength)
				return;

			int 다음위치 = 본문.Text.IndexOf(_찾은내용, 본문.SelectionStart);

			if (다음위치 == 본문.SelectionStart)
				다음위치 = 본문.Text.IndexOf(_찾은내용, 본문.SelectionStart + 1);

			if (다음위치 != -1)
			{
				본문.SelectionStart = 다음위치;
				본문.SelectionLength = _찾은내용.Length;
				return;
			}
			else
			{
				Point 선택내용의위치 = 본문.GetPositionFromCharIndex(본문.SelectionStart + 본문.SelectedText.Length);

				선택내용의위치.X += this.Location.X + 11;
				선택내용의위치.Y += this.Location.Y + 54;

				메시지를스태틱으로만들기위한클래스 메시지박스 = new 메시지를스태틱으로만들기위한클래스("찾는 내용이 없습니다.");
				메시지박스.Owner = this;
				메시지박스.StartPosition = FormStartPosition.Manual;
				메시지박스.Location = new Point(선택내용의위치.X, 선택내용의위치.Y);

				if (메시지박스.ShowDialog() == DialogResult.OK)
				{
					this.Focus();
				}

				return;
			}
		}
		private void 바꾸기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Point 선택내용의위치 = 본문.GetPositionFromCharIndex(본문.SelectionStart + 본문.SelectedText.Length);

			바꾸기다이얼로그 바꾸기다이얼로그 = new 바꾸기다이얼로그(본문.SelectedText);

			바꾸기다이얼로그.StartPosition = FormStartPosition.Manual;

			선택내용의위치.X += this.Location.X + 11;
			선택내용의위치.Y += this.Location.Y + 54;

			바꾸기다이얼로그.Location = 선택내용의위치;
			바꾸기다이얼로그.Owner = this;
			바꾸기다이얼로그.Show();
		}

		private void 메뉴_도구_테스트(object sender, EventArgs e)
		{


			/*
			List<string> 영한사전전체 = new List<string>();
			변환.UTF8파일.문자열들로("D:\\word01t.txt", ref 영한사전전체);

			List<string> 바꾼영한사전전체 = new List<string>();


			foreach (string 영한사전항목 in 영한사전전체)
            {
                string[] 영한사전_현재항목_배열 = 영한사전항목.Split('|');

				if (영한사전_현재항목_배열.Length > 4)
				{
					MessageBox.Show(영한사전_현재항목_배열[0]);
				}
				else if(영한사전_현재항목_배열.Length > 3)
				{
					바꾼영한사전전체.Add(영한사전_현재항목_배열[0] + "|" + 영한사전_현재항목_배열[1] + "|" + 영한사전_현재항목_배열[2].Replace("á", "'a") + "|"+ 영한사전_현재항목_배열[3]);
				}
				else if (영한사전_현재항목_배열.Length > 2)
                {
					바꾼영한사전전체.Add(영한사전_현재항목_배열[0] + "|" + 영한사전_현재항목_배열[1] + "|" + 영한사전_현재항목_배열[2].Replace("á", "'a"));
                }
				//á, é, í, ó, ú
            }

			변환.문자열들.UTF8파일로(바꾼영한사전전체, "D:\\word02t.txt");
			*/
		}
	

		private string 사전의미에서동사아닌것만남기기(string 사전의미)
		{
			사전의미 = 사전의미.Replace("pron.", "|pro.");
			사전의미 = 사전의미.Replace("n.", "|n.");
			사전의미 = 사전의미.Replace("v.", "|v.");
			사전의미 = 사전의미.Replace("vi.", "|vi.");
			사전의미 = 사전의미.Replace("vt.", "|vt.");
			사전의미 = 사전의미.Replace("a.", "|a.");
			사전의미 = 사전의미.Replace("ad.", "|ad.");
			사전의미 = 사전의미.Replace("prep.", "|prep.");
			사전의미 = 사전의미.Replace("int.", "|int.");
			사전의미 = 사전의미.Replace("conj.", "|conj.");



			string[] 품사별구분 = 사전의미.Split('|');

			string 동사아닌것만 = "";

			foreach (string 현재문자열 in 품사별구분)
			{
				if (!현재문자열.Contains("v.") && !현재문자열.Contains("vi.") && !현재문자열.Contains("vt.")) 동사아닌것만 += 현재문자열;
			}

			return 동사아닌것만.Trim();
		}
		private string 사전의미에서명사랑동사만남기기(string 사전의미)
		{
			사전의미 = 사전의미.Replace("pron.", "|pro.");
			사전의미 = 사전의미.Replace("n.", "|n.");
			사전의미 = 사전의미.Replace("vi.", "|vi.");
			사전의미 = 사전의미.Replace("vt.", "|vt.");
			사전의미 = 사전의미.Replace("a.", "|a.");
			사전의미 = 사전의미.Replace("ad.", "|ad.");
			사전의미 = 사전의미.Replace("prep.", "|prep.");
			사전의미 = 사전의미.Replace("int.", "|int.");
			사전의미 = 사전의미.Replace("conj.", "|conj.");



			string[] 품사별구분 = 사전의미.Split('|');

			string 명사랑동사뜻만 = "";

			foreach (string 현재문자열 in 품사별구분)
			{
				if (현재문자열.Contains("n.")) 명사랑동사뜻만 += 현재문자열;
				if (현재문자열.Contains("vi.")) 명사랑동사뜻만 += 현재문자열;
				if (현재문자열.Contains("vt.")) 명사랑동사뜻만 += 현재문자열;
			}

			return 명사랑동사뜻만.Trim();
		}
		private string 사전의미에서동사만남기기(string 사전의미)
		{
			사전의미 = 사전의미.Replace("pron.", "|pro.");
			사전의미 = 사전의미.Replace("n.", "|n.");
			사전의미 = 사전의미.Replace("vi.", "|vi.");
			사전의미 = 사전의미.Replace("vt.", "|vt.");
			사전의미 = 사전의미.Replace("a.", "|a.");
			사전의미 = 사전의미.Replace("ad.", "|ad.");
			사전의미 = 사전의미.Replace("prep.", "|prep.");
			사전의미 = 사전의미.Replace("int.", "|int.");
			사전의미 = 사전의미.Replace("conj.", "|conj.");



			string[] 품사별구분 = 사전의미.Split('|');

			string 동사뜻만 = "";

			foreach (string 현재문자열 in 품사별구분)
			{
				if (현재문자열.Contains("vi.")) 동사뜻만 += 현재문자열;
				if (현재문자열.Contains("vt.")) 동사뜻만 += 현재문자열;
			}

			return 동사뜻만.Trim();
		}

		#endregion
		#region 도구
		private void 단어장만들기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			단어장.만들기(본문.SelectedText);
		}
		private void 단어를찾아라ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			단어를찾아라.만들기(본문.SelectedText);
		}
		private void 심플한버전ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string 현재선택된부분 = 본문.SelectedText;
			본문.SelectedText = 단어를찾아라.심플한버전(현재선택된부분);
		}

		private void 포켓사전ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
		public static string GetWord(string 화면상의텍스트, int 마우스포인터위치) //Extracts the whole word the mouse is currently focused on.
		{
			if (화면상의텍스트.Length <= 마우스포인터위치 || 마우스포인터위치 < 0)
				return "";

			char s = 화면상의텍스트[마우스포인터위치];
			int sp1 = 0, sp2 = 화면상의텍스트.Length;
			for (int i = 마우스포인터위치; i > 0; i--)
			{
				char ch = 화면상의텍스트[i];
				if (ch == ' ' || ch == '\n')
				{
					sp1 = i;
					break;
				}
			}

			for (int i = 마우스포인터위치; i < 화면상의텍스트.Length; i++)
			{
				char ch = 화면상의텍스트[i];
				if (ch == ' ' || ch == '\n')
				{
					sp2 = i;
					break;
				}
			}

			return 화면상의텍스트.Substring(sp1, sp2 - sp1).Replace("\n", "");
		}
		private void 본문_MouseMove(object sender, MouseEventArgs e)
		{

			현재마우스위치 = new Point(e.X, e.Y);
			if (이전마우스위치 != 현재마우스위치)
			{

				string 현재어절 = GetWord(본문.Text, 본문.GetCharIndexFromPosition(e.Location)).Trim().불필요제거();

                while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
                while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);


                string 표제어 = "";
				string 사전검색결과 = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);

				if (표제어 != "")
					사전의미.Text = 사전검색결과;
				else
					사전의미.Text = "";

				사전표제어.Text = 표제어;

				string 사전발음기호s = _검색.영한발음기호사전(표제어);

				if (사전발음기호s != "")
					사전발음기호.Text = "[" + 사전발음기호s + "]";
				else
					사전발음기호.Text = "";
/*
				if (!_포켓사전.IsDisposed)
				{
					사전표제어.Text = 표제어;

					_포켓사전.사전의미설정(_검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어));
					_포켓사전.사전표제어설정(표제어);
				}
*/
				이전마우스위치 = 현재마우스위치;
			}

		}



		#endregion
		#region 필터
		private void 메뉴_필터_선택부분의태그제거(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();

			string 선택원본 = 본문.SelectedText;
			string 태그제거된문자열 = 변환.문자열.태그제거(선택원본);

			본문.SelectedText = 태그제거된문자열;


		}

		private void 메뉴_필터_선택부분의문법표지제거(object sender, EventArgs e)
		{
			선택위치에바꿀말넣고키워드색상입히기(변환.문자열.문법문제표지제거(본문.SelectedText));
		}

		private void 메뉴_필터_선택부분의엔터제거(object sender, EventArgs e)
		{
			메뉴_필터_선택부분의엔터제거();
		}

		private void 메뉴_필터_선택부분의엔터제거()
		{
			if (본문.Focused)
			{
				현재내용을_실행취소용_클립보드에_저장();
				_텍스트변경사항자동저장불필요 = true;

				string 선택내용 = 본문.SelectedText;

				선택위치에바꿀말넣고키워드색상입히기(강력하지만무거운변환.문자열.개행문자제거(선택내용));

				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();
			}
			else if(해석.Focused)
			{
				해석.SelectedText = 강력하지만무거운변환.문자열.개행문자제거(해석.SelectedText);
            }
            else if (힌트.Focused)
            {
                힌트.SelectedText = 강력하지만무거운변환.문자열.개행문자제거(힌트.SelectedText);
            }
        }

		private void 본문의내용을본문과해석으로나누기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string 영어부분 = "";
			string 한글부분 = "";
			string 처리결과 = "";

			List<string> 문자열들 = new List<string>();

			변환.문자열.개행문자로_구분한_문자열들로(본문.SelectedText, ref 문자열들);

			foreach (string 현재문자열 in 문자열들)
			{
				if (변환.문자열.한글포함했는지확인(현재문자열))
					한글부분 += 현재문자열 + "\n";
				else
					영어부분 += 현재문자열 + "\n";
			}

			한글부분 = 한글부분.Replace("(", "");
			한글부분 = 한글부분.Replace(")", "");


			while (영어부분.Contains("\n\n"))
				영어부분 = 영어부분.Replace("\n\n", "\n");


			while (한글부분.Contains("\n\n"))
				한글부분 = 한글부분.Replace("\n\n", "\n");


			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지();
			_텍스트변경사항자동저장불필요 = true;


			선택위치에바꿀말넣고키워드색상입히기(영어부분);
			해석.SelectedText = 한글부분;

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();

			화면업데이트재개();

		}

        #endregion

		private string 문제출제공통부분(string 현재CAKE)
		{
			string 순수현재CAKE = "", 예상문제 = "";

			// 문제를 출제하기 전에, 잘못된 쌍따옴표를 원본까지 처리해준다.
			현재CAKE = 현재CAKE.Replace("“", "\"");
			현재CAKE = 현재CAKE.Replace("”", "\"");
			

			변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

			string 고유번호제거된예상문제 = 변환.문자열.예상문제들에서고유번호만제거(예상문제);

			string 제목 = 문자열.제목추출(순수현재CAKE);
			string t = 강력하지만무거운변환.문자열.T태그원형복원(순수현재CAKE);
			string t변형 = 문자열.변형지문추출(순수현재CAKE);
			string tr변형 = 문자열.변형지문해석추출(순수현재CAKE);

			string tr = "";
			if (순수현재CAKE.Contains("<해석>")) tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.해석추출(순수현재CAKE));
			else tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(순수현재CAKE));
			string t_문법표지있는 = 강력하지만무거운변환.문자열.T태그원형문법표지남기고복원(순수현재CAKE);
			string t_구조분석만있는 = t_문법표지있는.문제표지제거();


			string 현재CAKE에서만든문제 = "";

			문제._본문_해석_문장숫자확인여부 = false;

			문제._현재번호 = 1;

			string 임시문제 = "";

			임시문제 += 문제.질문("다음 글의 분위기로 알맞은 것은?", _DB루트 + "분위기.문제", t, tr, "");
			임시문제 += 문제.질문("다음 글의 분위기로 알맞은 것은?", _DB루트 + "분위기.문제", t변형, tr변형, "");

			임시문제 += 문제.일치_영문(_DB루트 + "correctEnglish.qst", t, t_구조분석만있는, tr);
			임시문제 += 문제.일치_영문(_DB루트 + "correctEnglish.qst", t변형, t변형, tr변형);

			임시문제 += 문제.일치_한글(t, tr, false);
			임시문제 += 문제.일치_한글(t변형, tr변형, false);

			임시문제 += 문제.질문("다음 글의 주제로 가장 적절한 것은?", _DB루트 + "subject.qst", t, tr, "");
			임시문제 += 문제.질문("다음 글의 주제로 가장 적절한 것은?", _DB루트 + "subject.qst", t변형, tr변형, "");

			임시문제 += 문제.질문("다음 글 바로 뒤에 올 내용으로 가장 적절한 것은?", _DB루트 + "after.qst", t, tr, "");
			임시문제 += 문제.질문("다음 글 바로 뒤에 올 내용으로 가장 적절한 것은?", _DB루트 + "after.qst", t변형, tr변형, "");

			임시문제 += 문제.질문("다음 글의 제목으로 가장 적절한 것은?", _DB루트 + "title.qst", t, tr, "");
			임시문제 += 문제.질문("다음 글의 제목으로 가장 적절한 것은?", _DB루트 + "title.qst", t변형, tr변형, "");

			임시문제 += 문제.질문("다음 글이 주는 교훈을 속담으로 가장 잘 나타낸 것은?", _DB루트 + "proverb.qst", t, tr, "");
			임시문제 += 문제.질문("다음 글이 주는 교훈을 속담으로 가장 잘 나타낸 것은?", _DB루트 + "proverb.qst", t변형, tr변형, "");

			임시문제 += 문제.질문("다음 글의 목적으로 가장 적절한 것은?", _DB루트 + "목적.문제", t, tr, "");
			임시문제 += 문제.질문("다음 글의 목적으로 가장 적절한 것은?", _DB루트 + "목적.문제", t변형, tr변형, "");

			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_문장.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_문장.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_서술어로시작하는문구.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_서술어로시작하는문구.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_서술어s로시작하는문구.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_서술어s로시작하는문구.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_명령주제.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_명령주제.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_명사구.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_명사구.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_명사.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_명사.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_속담.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_속담.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_동사.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_동사.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_동사es.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_동사es.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_동사ed.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_동사ed.문제");
			임시문제 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_형용사.문제"); 임시문제 += 문제.빈칸(t변형, tr변형, _DB루트 + "빈칸_형용사.문제");

			임시문제 += 문제.중의어(t, tr); 임시문제 += 문제.중의어(t변형, tr변형);
			임시문제 += 문제.접속어(t, tr); 임시문제 += 문제.접속어(t변형, tr변형);
			임시문제 += 문제.문장삽입(t, tr); 임시문제 += 문제.문장삽입(t변형, tr변형);
			임시문제 += 문제.주관식_빈칸(t, tr, _헤더); 임시문제 += 문제.주관식_빈칸(t변형, tr변형, _헤더);
			임시문제 += 문제.동의어반의어(t, tr); 임시문제 += 문제.동의어반의어(t변형, tr변형);
			임시문제 += 문제.빈칸주제영작(t_문법표지있는, tr, _헤더); 임시문제 += 문제.빈칸주제영작(t변형, tr변형, _헤더);
			임시문제 += 문제.영작문제내기(t_문법표지있는, tr, _헤더); 임시문제 += 문제.영작문제내기(t변형, tr변형, _헤더);
			임시문제 += 문제.어형을_바꾸어_보기에서_알맞은말_골라쓰기(t, tr); 임시문제 += 문제.어형을_바꾸어_보기에서_알맞은말_골라쓰기(t변형, tr변형);

			임시문제 += 문제.어휘(t, tr); 임시문제 += 문제.어휘(t변형, tr변형);
			임시문제 += 문제.어법(t, tr); 임시문제 += 문제.어법(t변형, tr변형);
			임시문제 += 문제.분위기(t_문법표지있는, tr); 임시문제 += 문제.분위기(t변형, tr변형);
			임시문제 += 문제.글의순서(제목, t_문법표지있는, tr); 임시문제 += 문제.글의순서(제목, t변형, tr변형);

			현재CAKE에서만든문제 += 예상문제;
			현재CAKE에서만든문제 += 변환.문자열.기존에_없는_문제만_산출(고유번호제거된예상문제, 임시문제);

			현재CAKE에서만든문제 = 변환.문자열.자연스럽게문제순서섞기(현재CAKE에서만든문제);
			현재CAKE에서만든문제 = 변환.문자열.문제번호다시매기기(현재CAKE에서만든문제.Trim());

			return 순수현재CAKE.Replace("</CAKE>", "").Trim() + "\r\n<SUGAR>\r\n" + 현재CAKE에서만든문제.Trim() + "\r\n</SUGAR>" + "\r\n</CAKE>\r\n";
		}

		#region 문제출제
		private void 커서위치의_CAKE만_예상문제_업데이트하기_클릭(object sender, EventArgs e)
		{
			if(!문서_종류와_난이도를_클릭했는지_확인())  
			{
				메시지박스.보여주기("문서 정보 탭에서, 문서 종류와 난이도를 선택하세요!", this);
				return; 
			}


			if (treeView1.Focused)
			{
				if (treeView1.SelectedNode.Level == 1)
				{
					_CAKE_인덱스 = treeView1.SelectedNode.Index;

					_CAKE들[_CAKE_인덱스] = 문제출제공통부분(_CAKE들[_CAKE_인덱스]);
				}

				string 저장할문자열 = "";
				저장할문자열 += 헤더정보();

				for (int i = 0; i < _CAKE들.Count; i++) 저장할문자열 += _CAKE들[i] + "\r\n";

				저장할문자열 = 저장할문자열.Replace("</CAKE>\r\n\r\n", "</CAKE>\r\n");

				저장할문자열 = 문제고유번호매기기(저장할문자열);

				열기_파일내용으로(저장할문자열, _파일이름, false);

				treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스];
			}
		}


        private void 지원하는모든문제출제하기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region 전처리
			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 현재CAKE에서만든문제 = "";
			string 임시문제 = "";

			#endregion
			#region CAKE마다 루프를 돌림

			for (int i = 0; i < _CAKE들.Count(); i++)
			{
				_CAKE들[i] = 문제출제공통부분(_CAKE들[i]);
			}
			#endregion
			#region 후처리
			string 저장할문자열 = "";
			저장할문자열 += 헤더정보();

			for (int i = 0; i < _CAKE들.Count; i++) 저장할문자열 += _CAKE들[i] + "\r\n";

			저장할문자열 = 저장할문자열.Replace("</CAKE>\r\n\r\n", "</CAKE>\r\n");

			저장할문자열 = 문제고유번호매기기(저장할문자열);

			열기_파일내용으로(저장할문자열, _파일이름, false);

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
			#endregion
		}
		private void 문제DB폴더열기_클릭(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("explorer.exe", "\"" + _DB루트 + "\"");
		}

		private void 텍스트원형복원확인ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(_CAKE들.Count == 0) { MessageBox.Show("복원할 텍스트가 없습니다."); return; }

			if (treeView1.SelectedNode != null)
				_CAKE_인덱스 = treeView1.SelectedNode.Index;
			else
				_CAKE_인덱스 = 0;


			string 현재CAKE = _CAKE들[_CAKE_인덱스];

            string 순수현재CAKE = "", 예상문제 = "";

            변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

            string 고유번호제거된예상문제 = 변환.문자열.예상문제들에서고유번호만제거(예상문제);


            string t = 강력하지만무거운변환.문자열.T태그원형복원(순수현재CAKE);


            string tr = "";
            if (순수현재CAKE.Contains("<해석>"))
                tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.해석추출(순수현재CAKE));
            else
                tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(순수현재CAKE));


            MessageBox.Show(t +  "\r\n" +  tr);

            //            string t_문법표지있는 = 변환.문자열.T태그원형문법표지남기고복원(순수현재CAKE);

        }
        // 어형을 바꾸어 알맞은 말 보기에서 골라쓰기
        // 가장 어려운 단어만을 골라서 보기에서 골라쓰도록 하는 모듈입니다.
        private void 메뉴_문제출제_어휘골라쓰기(object sender, EventArgs e)
		{
			#region 전처리
			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);
			#endregion
			#region CAKE마다 루프돌림
			foreach (string 현재CAKE in CAKE들)
			{
				string t = 강력하지만무거운변환.문자열.T태그원형복원(현재CAKE);
				string tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE));

				문제전체 += 문제.어형을_바꾸어_보기에서_알맞은말_골라쓰기(t, tr);
			}
			#endregion
			#region 후처리
			본문.Text = 문제전체;

			화면업데이트중지();

			전체화면하이라이트표시();

			화면업데이트재개();
			#endregion
		}
		private void 메뉴_문제출제_내용일치한글(object sender, EventArgs e)
		{
			if (treeView1.Focused)
			{
				if (treeView1.SelectedNode.Level == 1)
				{
					_CAKE_인덱스 = treeView1.SelectedNode.Index;

					string 현재CAKE = _CAKE들[_CAKE_인덱스];

					string 순수현재CAKE = "", 예상문제 = "";

					변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

					string t = 강력하지만무거운변환.문자열.T태그원형복원(순수현재CAKE);

					string tr = "";
					if (순수현재CAKE.Contains("<해석>"))
						tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.해석추출(순수현재CAKE));
					else
						tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(순수현재CAKE));

					if(!String.IsNullOrEmpty(문제.일치_한글(t, tr, true)))
						MessageBox.Show("내용 일치 문제를 한글로 출제할 수 있습니다.");
				}
			}
		}
		private void 메뉴_문제출제_내용일치영문(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.질문("다음 글의 내용과 일치하는 것은?", _DB루트 + "correctEnglish.qst",
                강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), "일치_영문");
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}
		private void 메뉴_문제출제_어법문제내기(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);
			문제._현재번호 = 1;

			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.어법(강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)));
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}
		private void 어휘문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);
			문제._현재번호 = 1;

			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.어휘(강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)));
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}
		private void 지시문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
		private void 글의목적문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.질문("다음 글의 목적으로 가장 적절한 것은?", _DB루트 + "목적.문제",
                강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), "");
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}
		private void 빈칸주제문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);
			문제._현재번호 = 1;

			foreach (string 현재CAKE in CAKE들)
			{

				string t = 강력하지만무거운변환.문자열.T태그원형복원(현재CAKE);
				string tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE));
				string t_문법표지있는 = 강력하지만무거운변환.문자열.T태그원형문법표지남기고복원(현재CAKE);

				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_문장.문제");
				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_서술어로시작하는문구.문제");
				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_서술어로시작하는문구s.문제");
				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_명령주제.문제");
				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_명사구.문제");
				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_속담.문제");

				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_명사.문제");
				문제전체 += 문제.빈칸(t_문법표지있는, tr, _DB루트 + "빈칸_형용사.문제");

			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}
		private void 빈칸접속어문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

            string 문제전체 = "";
            List<string> CAKE들 = new List<string>();

            변환.문자열.CAKE들로(본문.Text, ref CAKE들);


            foreach (string 현재CAKE in CAKE들)
            {
                문제전체 += 문제.접속어(강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)));
            }

            본문.Text = 문제전체;

            전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
        }

        private void 주제문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.질문("다음 글의 주제로 가장 적절한 것은?", _DB루트 + "subject.qst",
                강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), "");
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}

		private void 뒤에올문장내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.질문("다음 글 바로 뒤에 올 내용으로 가장 적절한 것은?", _DB루트 + "after.qst",
                강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), "");
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}

		private void 글의순서문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 요약문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 분위기심경문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 요지문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 문장삽입문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region 전처리
			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 현재CAKE에서만든문제 = "";
			string 임시문제 = "";

			#endregion
			#region CAKE마다 루프를 돌림

			for (int i = 0; i < _CAKE들.Count(); i++)
			{
				string 현재CAKE = _CAKE들[i];

				string 순수현재CAKE = "", 예상문제 = "";

				변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

				string 고유번호제거된예상문제 = 변환.문자열.예상문제들에서고유번호만제거(예상문제);


				string t = 강력하지만무거운변환.문자열.T태그원형복원(순수현재CAKE);



				string tr = "";
				if (순수현재CAKE.Contains("<해석>"))
					tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.해석추출(순수현재CAKE));
				else
					tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(순수현재CAKE));
				string t_문법표지있는 = 강력하지만무거운변환.문자열.T태그원형문법표지남기고복원(순수현재CAKE);
				string t_구조분석만있는 = t_문법표지있는.문제표지제거();


				현재CAKE에서만든문제 = "";
				문제._현재번호 = 1;

				임시문제 = "";

				임시문제 += 문제.문장삽입(t, tr);

				현재CAKE에서만든문제 += 예상문제;
				현재CAKE에서만든문제 += 변환.문자열.기존에_없는_문제만_산출(고유번호제거된예상문제, 임시문제);

				현재CAKE에서만든문제 = 변환.문자열.자연스럽게문제순서섞기(현재CAKE에서만든문제);
				현재CAKE에서만든문제 = 변환.문자열.문제번호다시매기기(현재CAKE에서만든문제.Trim());

				_CAKE들[i] = 순수현재CAKE.Replace("</CAKE>", "").Trim() + "\r\n<SUGAR>\r\n" + 현재CAKE에서만든문제.Trim() + "\r\n</SUGAR>" + "\r\n</CAKE>\r\n";
			}
			#endregion
			#region 후처리
			string 저장할문자열 = "";
			저장할문자열 += 헤더정보();

			for (int i = 0; i < _CAKE들.Count; i++) 저장할문자열 += _CAKE들[i] + "\r\n";

			저장할문자열 = 저장할문자열.Replace("</CAKE>\r\n\r\n", "</CAKE>\r\n");

			저장할문자열 = 문제고유번호매기기(저장할문자열);

			열기_파일내용으로(저장할문자열, _파일이름, false);

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
			#endregion
		}

		private void 제목문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.질문("다음 글의 제목으로 가장 적절한 것은?", _DB루트 + "title.qst",
                강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), "");
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();

		}

		private void 관계없는문장내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 속담문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.질문("다음 글이 주는 교훈을 속담으로 가장 잘 나타낸 것은?", _DB루트 + "proverb.qst",
                강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), "");
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}

		private void 영영사전형문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 영작문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);

			string 파일헤더 = 변환.문자열.헤더추출(본문.Text);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.영작문제내기(강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), 파일헤더);
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}

		private void 다의어문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);


			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.중의어(강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)));
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();

		}


		private void 핵심어구빈칸채우기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 문제전체 = "";
			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);

			문제._현재번호 = 1;

			foreach (string 현재CAKE in CAKE들)
			{
				문제전체 += 문제.주관식_빈칸(강력하지만무거운변환.문자열.T태그원형복원(현재CAKE), 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(현재CAKE)), _파일경로);
			}

			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
		}

		private void 전체지문영작문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 영화문제출제ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 다이얼로그흐름문제내기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}



		#endregion

		#region 외부프로그램실행
		private void 메뉴_외부프로그램실행_선택영역을구글로검색(object sender, EventArgs e)
		{
            string 선택한스트링 = "";

            if(제목.Focused) 선택한스트링 = 제목.SelectedText;
            else if (질문.Focused) 선택한스트링 = 질문.SelectedText;
            else if (본문.Focused) 선택한스트링 = 본문.SelectedText;
            else if (ABC.Focused) 선택한스트링 = ABC.SelectedText;
            else if (보기1Text.Focused) 선택한스트링 = 보기1Text.SelectedText;
            else if (보기2Text.Focused) 선택한스트링 = 보기2Text.SelectedText;
            else if (보기3Text.Focused) 선택한스트링 = 보기3Text.SelectedText;
            else if (보기4Text.Focused) 선택한스트링 = 보기4Text.SelectedText;
            else if (보기5Text.Focused) 선택한스트링 = 보기5Text.SelectedText;
            else if (주관식정답.Focused) 선택한스트링 = 주관식정답.SelectedText;

            else if (해석.Focused) 선택한스트링 = 해석.SelectedText;
            else if (힌트.Focused) 선택한스트링 = 힌트.SelectedText;
            else if (중요어휘.Focused) 선택한스트링 = 중요어휘.SelectedText;
			else if (주제.Focused) 선택한스트링 = 주제.SelectedText;
			else if (변형지문.Focused) 선택한스트링 = 변형지문.SelectedText;
			else if (변형지문해석.Focused) 선택한스트링 = 변형지문해석.SelectedText;

			선택한스트링 = 변환.문자열.문법문제표지제거(선택한스트링);

			선택한스트링 = 선택한스트링.Replace(" ", "%20");

			Process.Start("Chrome.exe", string.Format("http://www.google.co.kr/search?hl=ko&newwindow=1&q=%22{0}%22&lr=", 선택한스트링));
		}

		private void 메뉴_외부프로그램실행_DictionaryCom에서검색(object sender, EventArgs e)
		{
			string 선택한스트링 = 본문.SelectedText;
			선택한스트링 = 선택한스트링.Replace(" ", "+");

			Process.Start("Chrome.exe", string.Format("http://dictionary.reference.com/browse/{0}?s=t", 선택한스트링));
		}

		private void 메뉴_외부프로그램실행_urbanDictionary에서검색(object sender, EventArgs e)
		{
			string 선택한스트링 = 본문.SelectedText;
			선택한스트링 = 선택한스트링.Replace(" ", "+");

			Process.Start("Chrome.exe", string.Format("http://www.urbandictionary.com/define.php?term={0}", 선택한스트링));
		}

		private void 메뉴_외부프로그램실행_naver사전검색(object sender, EventArgs e)
		{
			string 선택한스트링 = 본문.SelectedText;
			선택한스트링 = 선택한스트링.Replace(" ", "+");

			Process.Start("Chrome.exe", string.Format("http://dic.naver.com/search.nhn?dicQuery={0}&query={0}&target=dic&ie=utf8&query_utf=&isOnlyViewEE=", 선택한스트링));
		}


		#endregion
		#region 자동입력
		#region 예상 문제 유형 입력

		private string 빈칸_주관식_표시(string 현재지문)
		{
			List<string> 빈칸_주관식_목록 = new List<string>();

			Ansi파일.문자열들로(_DB루트 + "빈칸_주관식_고.문제", ref 빈칸_주관식_목록);

			string 현재지문_문법문제표지제거된 = 문자열.문법문제표지제거(현재지문);

			List<string> 문법문제표지포함한_어절들 = new List<string>();
			문자열.문법문제표지포함한_어절들로(현재지문, ref 문법문제표지포함한_어절들);
			

			List<string> 문법문제표지제거한_어절들 = new List<string>();
			foreach (string 임시_문법문제표지포함한_어절 in 문법문제표지포함한_어절들)
			{
				문법문제표지제거한_어절들.Add(문자열.문법문제표지제거(임시_문법문제표지포함한_어절));
			}

			foreach (string 현재_빈칸_주관식_항목 in 빈칸_주관식_목록)
			{

				if (변환.문자열.Contains강력(현재지문_문법문제표지제거된, 현재_빈칸_주관식_항목))
				{
					#region 현재 핵심어구 데이터보다 더 내용이 긴 것은 빈칸으로 내면 안된다. 이 내용은 문제.cs 파일에도 있다.

					bool 현재_빈칸_주관식_항목보다내용긴게있는지 = false;

					foreach (string 현재핵심어구보다긴것 in 빈칸_주관식_목록)
					{
						if (변환.문자열.Contains강력(현재지문_문법문제표지제거된, 현재핵심어구보다긴것))
						{
							if (현재_빈칸_주관식_항목 != 현재핵심어구보다긴것 && 현재핵심어구보다긴것.Contains(현재_빈칸_주관식_항목))
							{
								현재_빈칸_주관식_항목보다내용긴게있는지 = true;
							}
						}
					}

					if (현재_빈칸_주관식_항목보다내용긴게있는지 == false)
					{
						List<string> 현재_빈칸_주관식_항목들 = new List<string>();
						문자열.어절들로(현재_빈칸_주관식_항목, ref 현재_빈칸_주관식_항목들);

						for (int i = 0; i < 문법문제표지포함한_어절들.Count - 현재_빈칸_주관식_항목들.Count + 1; i++)
						{
							bool 완전일치 = true;

							for (int j = 0; j < 현재_빈칸_주관식_항목들.Count; j++)
							{
								if (문법문제표지제거한_어절들[i + j] != 현재_빈칸_주관식_항목들[j])
								{
									완전일치 = false;
								}
							}

							if (완전일치)
							{
								문법문제표지포함한_어절들[i] = "{빈칸}" + 문법문제표지포함한_어절들[i];

								문법문제표지포함한_어절들[i + 현재_빈칸_주관식_항목들.Count - 1] = 문법문제표지포함한_어절들[i + 현재_빈칸_주관식_항목들.Count - 1] + "{/빈칸}";
							}
						}
					}

					#endregion
				}
			}

			현재지문 = 문자열들.단락으로(문법문제표지포함한_어절들);

			return 현재지문;
		}

		private string 영작_주관식_강조_표시(string 현재지문)
		{
			List<string> 영작_주관식_목록 = new List<string>();

			Ansi파일.문자열들로(_DB루트 + "영작_주관식_고.문제", ref 영작_주관식_목록);

			string 현재지문_문법문제표지제거된 = 문자열.문법문제표지제거(현재지문);

			List<string> 문법문제표지포함한_어절들 = new List<string>();
			문자열.문법문제표지포함한_어절들로(현재지문, ref 문법문제표지포함한_어절들);

			List<string> 문법문제표지제거한_어절들 = new List<string>();
			foreach (string 임시_문법문제표지포함한_어절 in 문법문제표지포함한_어절들)
			{
				문법문제표지제거한_어절들.Add(문자열.문법문제표지제거(임시_문법문제표지포함한_어절));
			}

			foreach (string 현재_영작_주관식_항목 in 영작_주관식_목록)
			{

				if (변환.문자열.Contains강력(현재지문_문법문제표지제거된, 현재_영작_주관식_항목))
				{
					#region 현재 핵심어구 데이터보다 더 내용이 긴 것은 빈칸으로 내면 안된다. 이 내용은 문제.cs 파일에도 있다.

					bool 현재_영작_주관식_항목보다내용긴게있는지 = false;

					foreach (string 현재핵심어구보다긴것 in 영작_주관식_목록)
					{
						if (변환.문자열.Contains강력(현재지문_문법문제표지제거된, 현재핵심어구보다긴것))
						{
							if (현재_영작_주관식_항목 != 현재핵심어구보다긴것 && 현재핵심어구보다긴것.Contains(현재_영작_주관식_항목))
							{
								현재_영작_주관식_항목보다내용긴게있는지 = true;
							}
						}
					}

					if (현재_영작_주관식_항목보다내용긴게있는지 == false)
					{
						List<string> 현재_영작_주관식_항목들 = new List<string>();
						문자열.어절들로(현재_영작_주관식_항목, ref 현재_영작_주관식_항목들);

						for (int i = 0; i < 문법문제표지포함한_어절들.Count - 현재_영작_주관식_항목들.Count + 1; i++)
						{
							bool 완전일치 = true;

							for (int j = 0; j < 현재_영작_주관식_항목들.Count; j++)
							{
								if (문법문제표지제거한_어절들[i + j] != 현재_영작_주관식_항목들[j])
								{
									완전일치 = false;
								}
							}

							if (완전일치)
							{
								문법문제표지포함한_어절들[i] = "{중요}" + 문법문제표지포함한_어절들[i];
							}
						}
					}
					#endregion
				}
			}

			현재지문 = 문자열들.단락으로(문법문제표지포함한_어절들);

			return 현재지문;
		}

		private void 메뉴_자동입력_예상문제유형입력_모든가능성(object sender, EventArgs e)
		{
			화면업데이트중지();
			//int 앞의T위치 = 0; int 뒤의T위치 = 0;
			string 현재지문 = 본문.Text;
			//string 현재지문 = 현재문제의T내용추출(ref 앞의T위치, ref 뒤의T위치);
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;
			//본문.Select(앞의T위치, 뒤의T위치 - 앞의T위치);

			//★★★★★★★★★★★★★★★★★★★★★★★★★
			현재지문 = 문제.출제가능한모든내용표시(현재지문);

			#region 지시 흐름

			if(현재지문.Contains("This time,"))
			{ 
				// 못냄
			}
			else
			{
				현재지문 = 현재지문.Replace("This ", "{지시}{흐름}This ");
				현재지문 = 현재지문.Replace("ⓢ{This}", "{지시}{흐름}ⓢ{This}");
			}

			현재지문 = 현재지문.Replace("Those ", "{지시}{흐름}Those ");
			현재지문 = 현재지문.Replace("ⓢ{Those}", "{지시}{흐름}ⓢ{Those}");

            while (현재지문.Contains("{흐름}{흐름}"))
            {
                현재지문 = 현재지문.Replace("{흐름}{흐름}", "{흐름}");
            }


            while (현재지문.Contains("{지시}{흐름}{지시}{흐름}"))
            {
                현재지문 = 현재지문.Replace("{지시}{흐름}{지시}{흐름}", "{지시}{흐름}");
            }

            현재지문 = 현재지문.Replace("ⓢ{{지시}{흐름}", "{지시}{흐름}ⓢ{");


            while (현재지문.Contains("{지시}{흐름}{지시}{흐름}"))
            {
                현재지문 = 현재지문.Replace("{지시}{흐름}{지시}{흐름}", "{지시}{흐름}");
            }

			#endregion

			#region 주제 - 인공지능의 서막

			List<string> 문장들 = new List<string>();

			강력하지만무거운변환.문자열.문장단위의_문자열들로_탭과개행문자살림(현재지문, ref 문장들);

			List<string> 주제문후보 = new List<string>();

			주제문후보.Add("you should");
			주제문후보.Add("You should");
			주제문후보.Add("ⓢ{you} ⓥ{ⓧ{should}");
			주제문후보.Add("ⓢ{You} ⓥ{ⓧ{should}");
			주제문후보.Add("we should");
			주제문후보.Add("We should");
			주제문후보.Add("ⓢ{we} ⓥ{ⓧ{should}");
			주제문후보.Add("ⓢ{We} ⓥ{ⓧ{should}");
			주제문후보.Add("why don't you");
			주제문후보.Add("Why don't you");
			주제문후보.Add("why don't we");
			주제문후보.Add("Why don't we");

			주제문후보.Add("ⓧ{why} ⓧ{don’t} ⓢ{you}");
			주제문후보.Add("ⓧ{Why} ⓧ{don’t} ⓢ{you}");
			주제문후보.Add("ⓧ{why} ⓧ{don’t} ⓢ{we}");
			주제문후보.Add("ⓧ{Why} ⓧ{don’t} ⓢ{we}");

			string 주제문표시된지문 = "";

			foreach (string 현재문장 in 문장들)
			{
				bool 주제문표시여부 = false;

				foreach(string 현재주제문후보 in 주제문후보)
				{
					if(현재문장.Contains(현재주제문후보))
					{
						주제문표시된지문 += "{주제}" + 현재문장 + "{/주제} ";
						주제문표시여부 = true;

						break;
					}
				}
				
				if(주제문표시여부 == false)
					주제문표시된지문 += 현재문장 + " ";

			}

			현재지문 = 주제문표시된지문.Trim();
			현재지문 = 현재지문.Replace("{주제}{주제}", "{주제}");
			현재지문 = 현재지문.Replace("{/주제}{/주제}", "{/주제}");



			#endregion

			

			#region 빈칸 흐름 어법
			현재지문 = 현재지문.Replace("This means that ", "{흐름}This means that ");
			현재지문 = 현재지문.Replace("{흐름}{흐름}This means that ", "{흐름}This means that ");
			현재지문 = 현재지문.Replace("ⓢ{This} ⓥ{means} ⓞ{㉨{that} ", "{흐름}ⓢ{This} ⓥ{means} ⓞ{㉨{that} ");
			현재지문 = 현재지문.Replace("{흐름}{흐름}ⓢ{This} ⓥ{means} ⓞ{㉨{that} ", "{흐름}ⓢ{This} ⓥ{means} ⓞ{㉨{that} ");

			현재지문 = 현재지문.Replace("But ", "{흐름}{빈칸}But{/빈칸} ");
			현재지문 = 현재지문.Replace("㉨{But} ", "{흐름}{빈칸}㉨{But}{/빈칸} ");
			현재지문 = 현재지문.Replace("㉨{But,} ", "{흐름}{빈칸}㉨{But},{/빈칸} ");
			현재지문 = 현재지문.Replace("㉨{But}, ", "{흐름}{빈칸}㉨{But},{/빈칸} ");

			현재지문 = 현재지문.Replace("Then ", "{흐름}{빈칸}Then{/빈칸} ");
			현재지문 = 현재지문.Replace("㉨{Then} ", "{흐름}{빈칸}㉨{Then}{/빈칸} ");
			현재지문 = 현재지문.Replace("㉨{Then,} ", "{흐름}{빈칸}㉨{Then},{/빈칸} ");
			현재지문 = 현재지문.Replace("㉨{Then}, ", "{흐름}{빈칸}㉨{Then},{/빈칸} ");

			현재지문 = 현재지문.Replace("Instead of ", "Instead_of ");
			현재지문 = 현재지문.Replace(" instead of ", " {어법:of 생략불가:}instead of ");
			현재지문 = 현재지문.Replace(" {어법:of 생략불가:}{어법:of 생략불가:}instead of ", " {어법:of 생략불가:}instead of ");

			현재지문 = 현재지문.Replace("Instead ", "{빈칸}{흐름}{어법:≠Instead of:}Instead{/빈칸} ");
			현재지문 = 현재지문.Replace("Instead,", "{빈칸}{흐름}{어법:≠Instead of:}Instead{/빈칸},");

			현재지문 = 현재지문.Replace("㉨{Instead}", "{빈칸}{흐름}{어법:≠Instead of:}㉨{Instead}{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{instead of ", "{어법:of 생략불가:}ⓧ{instead of ");
			현재지문 = 현재지문.Replace("ⓧ{Instead_of", "{어법:of 생략불가:}ⓧ{Instead of");
			현재지문 = 현재지문.Replace("Instead_of", "{어법:of 생략불가:}Instead of");
			현재지문 = 현재지문.Replace("{어법:of 생략불가:}{어법:of 생략불가:}", "{어법:of 생략불가:}");

			현재지문 = 현재지문.Replace("{빈칸}{흐름}{어법:≠{빈칸}{흐름}{어법:≠Instead of:}Instead{/빈칸} of:}Instead{/빈칸}", "{빈칸}{흐름}{어법:≠Instead of:}Instead{/빈칸}");
			현재지문 = 현재지문.Replace("{빈칸}{흐름}{어법:≠{빈칸}{흐름}{어법:≠Instead of:}Instead{/빈칸} of:}{빈칸}{흐름}{어법:≠Instead of:}㉨{Instead}{/빈칸}{/빈칸}", "{빈칸}{흐름}{어법:≠Instead of:}㉨{Instead}{/빈칸}");

            현재지문 = 현재지문.Replace("For example", "{빈칸}{흐름}For example{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}For example{/빈칸}}", "{빈칸}{흐름}ⓧ{For example}{/빈칸}");
			현재지문 = 현재지문.Replace("for example", "{빈칸}{흐름}for example{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}for example{/빈칸}}", "{빈칸}{흐름}ⓧ{for example}{/빈칸}");

			현재지문 = 현재지문.Replace("For instance", "{빈칸}{흐름}For instance{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}For instance{/빈칸}}", "{빈칸}{흐름}ⓧ{For instance}{/빈칸}");
			현재지문 = 현재지문.Replace("for instance", "{빈칸}{흐름}for instance{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}for instance{/빈칸}}", "{빈칸}{흐름}ⓧ{for instance}{/빈칸}");

			현재지문 = 현재지문.Replace("Therefore", "{빈칸}{흐름}Therefore{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}Therefore{/빈칸}}", "{빈칸}{흐름}㉨{Therefore}{/빈칸}");
			현재지문 = 현재지문.Replace("therefore", "{빈칸}{흐름}therefore{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}therefore{/빈칸}}", "{빈칸}{흐름}㉨{therefore}{/빈칸}");



            현재지문 = 현재지문.Replace("{빈칸}{흐름}{어법:≠In addition to:}In addition{/빈칸}", "{빈칸}{흐름}{어법:≠In_addition_to:}In_addition{/빈칸}");
            현재지문 = 현재지문.Replace("{빈칸}{흐름}{어법:≠In addition to:}ⓧ{In addition}{/빈칸}", "{빈칸}{흐름}{어법:≠In_addition_to:}ⓧ{In_addition}{/빈칸}");

            현재지문 = 현재지문.Replace("In addition to", "{어법:to 생략불가:}In_addition_to");
			현재지문 = 현재지문.Replace("In addition", "{빈칸}{흐름}{어법:≠In addition to:}In addition{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}{어법:≠In addition to:}In addition{/빈칸}}", "{빈칸}{흐름}{어법:≠In addition to:}ⓧ{In addition}{/빈칸}");
			현재지문 = 현재지문.Replace("{어법:to 생략불가:}{어법:to 생략불가:}", "{어법:to 생략불가:}");

            현재지문 = 현재지문.Replace("{빈칸}{흐름}{어법:≠In_addition_to:}In_addition{/빈칸}", "{빈칸}{흐름}{어법:≠In addition to:}In addition{/빈칸}");
            현재지문 = 현재지문.Replace("{빈칸}{흐름}{어법:≠In_addition_to:}ⓧ{In_addition}{/빈칸}", "{빈칸}{흐름}{어법:≠In addition to:}ⓧ{In addition}{/빈칸}");


            현재지문 = 현재지문.Replace("In_addition_to", "In addition to");


            현재지문 = 현재지문.Replace("Moreover", "{빈칸}{흐름}Moreover{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}Moreover{/빈칸}}", "{빈칸}{흐름}ⓧ{Moreover}{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}Moreover{/빈칸}}", "{빈칸}{흐름}ⓧ{Moreover}{/빈칸}");

			현재지문 = 현재지문.Replace("As a result", "{빈칸}{흐름}As a result{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}As a result{/빈칸}}", "{빈칸}{흐름}ⓧ{As a result}{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}As a result{/빈칸}}", "{빈칸}{흐름}ⓧ{As a result}{/빈칸}");

			현재지문 = 현재지문.Replace("At last", "{빈칸}{흐름}At last{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}At last{/빈칸}}", "{빈칸}{흐름}ⓧ{At last}{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}At last{/빈칸}}", "{빈칸}{흐름}ⓧ{At last}{/빈칸}");

			현재지문 = 현재지문.Replace("Besides", "{빈칸}{흐름}Besides{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}Besides{/빈칸}}", "{빈칸}{흐름}ⓧ{Besides}{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}Besides{/빈칸}}", "{빈칸}{흐름}ⓧ{Besides}{/빈칸}");

			현재지문 = 현재지문.Replace("By the way", "{빈칸}{흐름}By the way{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}By the way{/빈칸}}", "{빈칸}{흐름}ⓧ{By the way}{/빈칸}");

			현재지문 = 현재지문.Replace("However", "{빈칸}{흐름}However{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}However{/빈칸}}", "{빈칸}{흐름}㉨{However}{/빈칸}");
			현재지문 = 현재지문.Replace("however", "{빈칸}{흐름}however{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}however{/빈칸}}", "{빈칸}{흐름}㉨{however}{/빈칸}");

			현재지문 = 현재지문.Replace("In a word", "{빈칸}{흐름}In a word{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}In a word{/빈칸}}", "{빈칸}{흐름}ⓧ{In a word}{/빈칸}");

			현재지문 = 현재지문.Replace("In conclusion", "{빈칸}{흐름}In conclusion{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}In conclusion{/빈칸}}", "{빈칸}{흐름}ⓧ{In conclusion}{/빈칸}");

			현재지문 = 현재지문.Replace("In fact", "{빈칸}{흐름}In fact{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}In fact{/빈칸}}", "{빈칸}{흐름}ⓧ{In fact}{/빈칸}");

			현재지문 = 현재지문.Replace("{빈칸}{흐름}{빈칸}{흐름}", "{빈칸}{흐름}");
			현재지문 = 현재지문.Replace("{/빈칸}{/빈칸}", "{/빈칸}");



			현재지문 = 현재지문.Replace("In the end", "{빈칸}{흐름}In the end{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}In the end{/빈칸}}", "{빈칸}{흐름}ⓧ{In the end}{/빈칸}");

			현재지문 = 현재지문.Replace("In the long run", "{빈칸}{흐름}In the long run{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}In the long run{/빈칸}}", "{빈칸}{흐름}ⓧ{In the long run}{/빈칸}");

			현재지문 = 현재지문.Replace("Nonetheless", "{빈칸}{흐름}Nonetheless{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}Nonetheless{/빈칸}}", "{빈칸}{흐름}ⓧ{Nonetheless}{/빈칸}");

			현재지문 = 현재지문.Replace("On the contrary", "{빈칸}{흐름}On the contrary{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}On the contrary{/빈칸}}", "{빈칸}{흐름}ⓧ{On the contrary}{/빈칸}");

			현재지문 = 현재지문.Replace("On the other hand", "{빈칸}{흐름}On the other hand{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}On the other hand{/빈칸}}", "{빈칸}{흐름}ⓧ{On the other hand}{/빈칸}");

			현재지문 = 현재지문.Replace("Thus", "{빈칸}{흐름}Thus{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}Thus{/빈칸}}", "{빈칸}{흐름}ⓧ{Thus}{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}Thus{/빈칸}}", "{빈칸}{흐름}ⓧ{Thus}{/빈칸}");

			현재지문 = 현재지문.Replace("Furthermore", "{빈칸}{흐름}Furthermore{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}Furthermore{/빈칸}}", "{빈칸}{흐름}ⓧ{Furthermore}{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}Furthermore{/빈칸}}", "{빈칸}{흐름}ⓧ{Furthermore}{/빈칸}");


			현재지문 = 현재지문.Replace("Nevertheless", "{빈칸}{흐름}Nevertheless{/빈칸}");
			현재지문 = 현재지문.Replace("ⓧ{{빈칸}{흐름}Nevertheless{/빈칸}}", "{빈칸}{흐름}ⓧ{Nevertheless}{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}Nevertheless{/빈칸}}", "{빈칸}{흐름}ⓧ{Nevertheless}{/빈칸}");

			현재지문 = 현재지문.Replace("Though", "{빈칸}{흐름}{어법:≠Despite:}Though{/빈칸}");
			현재지문 = 현재지문.Replace("㉨{{빈칸}{흐름}{어법:≠Despite:}Though{/빈칸}}", "{빈칸}{흐름}{어법:≠Despite:}㉨{Though}{/빈칸}");

			현재지문 = 현재지문.Replace("While", "{어법:≠During:}While");
			현재지문 = 현재지문.Replace("㉨{{어법:≠During:}While", "{어법:≠During:}㉨{While");
			현재지문 = 현재지문.Replace(" while", " {어법:≠during:}while");
			현재지문 = 현재지문.Replace("㉨{while", "{어법:≠during:}㉨{while");

			현재지문 = 현재지문.Replace("During", "{어법:≠While:}During");
			현재지문 = 현재지문.Replace("ⓧ{{어법:≠While:}During", "{어법:≠While:}ⓧ{During");
			현재지문 = 현재지문.Replace(" during", " {어법:≠while:}during");
			현재지문 = 현재지문.Replace("ⓧ{during", "{어법:≠while:}ⓧ{during");

			현재지문 = 현재지문.Replace("{어법:≠{어법:≠while:}during:}㉨{while}", "{어법:≠during:}㉨{while}");
			현재지문 = 현재지문.Replace("{어법:≠{어법:≠While:}During:}㉨{While}", "{어법:≠During:}㉨{While}");
			현재지문 = 현재지문.Replace("{어법:≠{어법:≠while:}during:}{어법:≠during:}㉨{while}", "{어법:≠during:}㉨{while}");
			현재지문 = 현재지문.Replace("{어법:≠{어법:≠while:}during:}{어법:≠{어법:≠while:}during:}", "{어법:≠during:}");

			현재지문 = 현재지문.Replace("In other words", "{빈칸}In other words{/빈칸}");
			현재지문 = 현재지문.Replace("{빈칸}{빈칸}In other words{/빈칸}{/빈칸}", "{빈칸}In other words{/빈칸}");

			현재지문 = 현재지문.Replace("ⓧ{In other words}", "{빈칸}ⓧ{In other words}{/빈칸}");
			현재지문 = 현재지문.Replace("{빈칸}{빈칸}ⓧ{In other words}{/빈칸}{/빈칸}", "{빈칸}ⓧ{In other words}{/빈칸}");


			#endregion


			#region 영작_주관식_문제_강조
			


			#endregion
			현재지문 = 빈칸_주관식_표시(현재지문);


			#region 어휘가 어법보다 앞에 나와야 한다.
			#region have to do with : 관련이 있다.
			현재지문 = 현재지문.Replace(" have to do with ", " {빈칸}have to do with{/빈칸} ");
            현재지문 = 현재지문.Replace(" had to do with ", " {빈칸}had to do with{/빈칸} ");
            현재지문 = 현재지문.Replace(" has to do with ", " {빈칸}has to do with{/빈칸} ");

            현재지문 = 현재지문.Replace("ⓥ{ⓧ{have to} do} ⓧ{with", "{빈칸}ⓥ{ⓧ{have to} do} ⓧ{with{/빈칸}");
            현재지문 = 현재지문.Replace("ⓥ{ⓧ{had to} do} ⓧ{with", "{빈칸}ⓥ{ⓧ{had to} do} ⓧ{with{/빈칸}");
            현재지문 = 현재지문.Replace("ⓥ{ⓧ{has to} do} ⓧ{with", "{빈칸}ⓥ{ⓧ{has to} do} ⓧ{with{/빈칸}");
            #endregion

            현재지문 = 문제.어휘문제_후보표시(현재지문);
            #endregion

            #region Every Each 단수 취급
            현재지문 = 현재지문.Replace("Every ", "{어법:단수 취급:}Every ");
			현재지문 = 현재지문.Replace("ⓧ{{어법:단수 취급:}Every ", "{어법:단수 취급:}ⓧ{Every ");
			현재지문 = 현재지문.Replace("ⓢ{{어법:단수 취급:}Every ", "{어법:단수 취급:}ⓢ{Every ");

			현재지문 = 현재지문.Replace(" every ", " {어법:단수 취급:}every ");
			현재지문 = 현재지문.Replace(" ⓧ{every ", " {어법:단수 취급:}ⓧ{every ");
			현재지문 = 현재지문.Replace(" ⓞ{every ", " {어법:단수 취급:}ⓞ{every ");
			현재지문 = 현재지문.Replace(" ⓘ{every ", " {어법:단수 취급:}ⓘ{every ");
			현재지문 = 현재지문.Replace(" ⓓ{every ", " {어법:단수 취급:}ⓓ{every ");
			현재지문 = 현재지문.Replace(" ⓒ{every ", " {어법:단수 취급:}ⓒ{every ");

			현재지문 = 현재지문.Replace("{어법:단수 취급:}{어법:단수 취급:}", "{어법:단수 취급:}");
			#endregion
			#region 전치사를 안쓰는 타동사
			#region marry
			현재지문 = 현재지문.Replace(" marry ", " {어법:뒤에 with 안씀:}marry ");
			현재지문 = 현재지문.Replace(" marries ", " {어법:뒤에 with 안씀:}marries ");
			현재지문 = 현재지문.Replace(" married ", " {어법:뒤에 with 안씀:}married ");

			현재지문 = 현재지문.Replace(" marry}", " {어법:뒤에 with 안씀:}marry}");
			현재지문 = 현재지문.Replace(" marries}", " {어법:뒤에 with 안씀:}marries}");
			현재지문 = 현재지문.Replace(" married}", " {어법:뒤에 with 안씀:}married}");

			현재지문 = 현재지문.Replace("ⓥ{marry}", "{어법:뒤에 with 안씀:}ⓥ{marry}");
			현재지문 = 현재지문.Replace("ⓥ{marries}", "{어법:뒤에 with 안씀:}ⓥ{marries}");
			현재지문 = 현재지문.Replace("ⓥ{married}", "{어법:뒤에 with 안씀:}ⓥ{married}");
			#endregion
			#region mention
			현재지문 = 현재지문.Replace(" mention ", " {어법:뒤에 about 안씀:}mention ");
			현재지문 = 현재지문.Replace(" mentions ", " {어법:뒤에 about 안씀:}mentions ");
			현재지문 = 현재지문.Replace(" mentioned ", " {어법:뒤에 about 안씀:}mentioned ");
			현재지문 = 현재지문.Replace(" mentioning ", " {어법:뒤에 about 안씀:}mentioning ");

			현재지문 = 현재지문.Replace(" mention}", " {어법:뒤에 about 안씀:}mention}");
			현재지문 = 현재지문.Replace(" mentions}", " {어법:뒤에 about 안씀:}mentions}");
			현재지문 = 현재지문.Replace(" mentioned}", " {어법:뒤에 about 안씀:}mentioned}");
			현재지문 = 현재지문.Replace(" mentioning}", " {어법:뒤에 about 안씀:}mentioning}");

			현재지문 = 현재지문.Replace("ⓥ{mention}", "{어법:뒤에 about 안씀:}ⓥ{mention}");
			현재지문 = 현재지문.Replace("ⓥ{mentions}", "{어법:뒤에 about 안씀:}ⓥ{mentions}");
			현재지문 = 현재지문.Replace("ⓥ{mentioned}", "{어법:뒤에 about 안씀:}ⓥ{mentioned}");
			현재지문 = 현재지문.Replace("ⓥ{mentioning}", "{어법:뒤에 about 안씀:}ⓥ{mentioning}");
			#endregion
			#region discuss
			현재지문 = 현재지문.Replace(" discuss ", " {어법:뒤에 about 안씀:}discuss ");
			현재지문 = 현재지문.Replace(" discusses ", " {어법:뒤에 about 안씀:}discusses ");
			현재지문 = 현재지문.Replace(" discussed ", " {어법:뒤에 about 안씀:}discussed ");
			현재지문 = 현재지문.Replace(" discussing ", " {어법:뒤에 about 안씀:}discussing ");

			현재지문 = 현재지문.Replace(" discuss}", " {어법:뒤에 about 안씀:}discuss}");
			현재지문 = 현재지문.Replace(" discusses}", " {어법:뒤에 about 안씀:}discusses}");
			현재지문 = 현재지문.Replace(" discussed}", " {어법:뒤에 about 안씀:}discussed}");
			현재지문 = 현재지문.Replace(" discussing}", " {어법:뒤에 about 안씀:}discussing}");

			현재지문 = 현재지문.Replace("ⓥ{discuss}", "{어법:뒤에 about 안씀:}ⓥ{discuss}");
			현재지문 = 현재지문.Replace("ⓥ{discusses}", "{어법:뒤에 about 안씀:}ⓥ{discusses}");
			현재지문 = 현재지문.Replace("ⓥ{discussed}", "{어법:뒤에 about 안씀:}ⓥ{discussed}");
			현재지문 = 현재지문.Replace("ⓥ{discussing}", "{어법:뒤에 about 안씀:}ⓥ{discussing}");
			#endregion
			#region resemble
			현재지문 = 현재지문.Replace(" resemble ", " {어법:뒤에 with 안씀:}resemble ");
			현재지문 = 현재지문.Replace(" resembles ", " {어법:뒤에 with 안씀:}resembles ");
			현재지문 = 현재지문.Replace(" resembled ", " {어법:뒤에 with 안씀:}resembled ");
			현재지문 = 현재지문.Replace(" resembling ", " {어법:뒤에 with 안씀:}resembling ");

			현재지문 = 현재지문.Replace(" resemble}", " {어법:뒤에 with 안씀:}resemble}");
			현재지문 = 현재지문.Replace(" resembles}", " {어법:뒤에 with 안씀:}resembles}");
			현재지문 = 현재지문.Replace(" resembled}", " {어법:뒤에 with 안씀:}resembled}");
			현재지문 = 현재지문.Replace(" resembling}", " {어법:뒤에 with 안씀:}resembling}");

			현재지문 = 현재지문.Replace("ⓥ{resemble}", "{어법:뒤에 with 안씀:}ⓥ{resemble}");
			현재지문 = 현재지문.Replace("ⓥ{resembles}", "{어법:뒤에 with 안씀:}ⓥ{resembles}");
			현재지문 = 현재지문.Replace("ⓥ{resembled}", "{어법:뒤에 with 안씀:}ⓥ{resembled}");
			현재지문 = 현재지문.Replace("ⓥ{resembling}", "{어법:뒤에 with 안씀:}ⓥ{resembling}");

            현재지문 = 현재지문.Replace("{어법:뒤에 with 안씀:}{어법:뒤에 with 안씀:}", "{어법:뒤에 with 안씀:}");

            #endregion
            #region inhabit
            현재지문 = 현재지문.Replace(" inhabit ", " {어법:뒤에 in 안씀:}inhabit ");
			현재지문 = 현재지문.Replace(" inhabits ", " {어법:뒤에 in 안씀:}inhabits ");
			현재지문 = 현재지문.Replace(" inhabited ", " {어법:뒤에 in 안씀:}inhabited ");
			현재지문 = 현재지문.Replace(" inhabiting ", " {어법:뒤에 in 안씀:}inhabiting ");

			현재지문 = 현재지문.Replace(" inhabit}", " {어법:뒤에 in 안씀:}inhabit}");
			현재지문 = 현재지문.Replace(" inhabits}", " {어법:뒤에 in 안씀:}inhabits}");
			현재지문 = 현재지문.Replace(" inhabited}", " {어법:뒤에 in 안씀:}inhabited}");
			현재지문 = 현재지문.Replace(" inhabiting}", " {어법:뒤에 in 안씀:}inhabiting}");

			현재지문 = 현재지문.Replace("ⓥ{inhabit}", "{어법:뒤에 in 안씀:}ⓥ{inhabit}");
			현재지문 = 현재지문.Replace("ⓥ{inhabits}", "{어법:뒤에 in 안씀:}ⓥ{inhabits}");
			현재지문 = 현재지문.Replace("ⓥ{inhabited}", "{어법:뒤에 in 안씀:}ⓥ{inhabited}");
			현재지문 = 현재지문.Replace("ⓥ{inhabiting}", "{어법:뒤에 in 안씀:}ⓥ{inhabiting}");

			현재지문 = 현재지문.Replace("{어법:뒤에 in 안씀:}{어법:뒤에 in 안씀:}", "{어법:뒤에 in 안씀:}");

			#endregion
			#region reach
			현재지문 = 현재지문.Replace(" reach ", " {어법:뒤에 to 안씀:}reach ");
			현재지문 = 현재지문.Replace(" reaches ", " {어법:뒤에 to 안씀:}reaches ");
			현재지문 = 현재지문.Replace(" reached ", " {어법:뒤에 to 안씀:}reached ");
			현재지문 = 현재지문.Replace(" reaching ", " {어법:뒤에 to 안씀:}reaching ");

			현재지문 = 현재지문.Replace(" reach}", " {어법:뒤에 to 안씀:}reach}");
			현재지문 = 현재지문.Replace(" reaches}", " {어법:뒤에 to 안씀:}reaches}");
			현재지문 = 현재지문.Replace(" reached}", " {어법:뒤에 to 안씀:}reached}");
			현재지문 = 현재지문.Replace(" reaching}", " {어법:뒤에 to 안씀:}reaching}");

			현재지문 = 현재지문.Replace("ⓥ{reach}", " {어법:뒤에 to 안씀:}ⓥ{reach}");
			현재지문 = 현재지문.Replace("ⓥ{reaches}", " {어법:뒤에 to 안씀:}ⓥ{reaches}");
			현재지문 = 현재지문.Replace("ⓥ{reached}", " {어법:뒤에 to 안씀:}ⓥ{reached}");
			현재지문 = 현재지문.Replace("ⓥ{reaching}", " {어법:뒤에 to 안씀:}ⓥ{reaching}");
			#endregion
			#region approach
			현재지문 = 현재지문.Replace(" approach ", " {어법:뒤에 to 안씀:}approach ");
			현재지문 = 현재지문.Replace(" approaches ", " {어법:뒤에 to 안씀:}approaches ");
			현재지문 = 현재지문.Replace(" approached ", " {어법:뒤에 to 안씀:}approached ");
			현재지문 = 현재지문.Replace(" approaching ", " {어법:뒤에 to 안씀:}approaching ");

			현재지문 = 현재지문.Replace(" approach}", " {어법:뒤에 to 안씀:}approach}");
			현재지문 = 현재지문.Replace(" approaches}", " {어법:뒤에 to 안씀:}approaches}");
			현재지문 = 현재지문.Replace(" approached}", " {어법:뒤에 to 안씀:}approached}");
			현재지문 = 현재지문.Replace(" approaching}", " {어법:뒤에 to 안씀:}approaching}");

			현재지문 = 현재지문.Replace("ⓥ{approach}", "{어법:뒤에 to 안씀:}ⓥ{approach}");
			현재지문 = 현재지문.Replace("ⓥ{approaches}", "{어법:뒤에 to 안씀:}ⓥ{approaches}");
			현재지문 = 현재지문.Replace("ⓥ{approached}", "{어법:뒤에 to 안씀:}ⓥ{approached}");
			현재지문 = 현재지문.Replace("ⓥ{approaching}", "{어법:뒤에 to 안씀:}ⓥ{approaching}");
			#endregion

			#endregion

			#region some, others
			현재지문 = 현재지문.Replace("Another", "{어법:≠(The) other(s):}Another");
			현재지문 = 현재지문.Replace("ⓢ{{어법:≠(The) other(s):}Another", "{어법:≠(The) other(s):}ⓢ{Another");
			현재지문 = 현재지문.Replace("ⓞ{{어법:≠(The) other(s):}Another", "{어법:≠(The) other(s):}ⓞ{Another");
			현재지문 = 현재지문.Replace("another", "{어법:≠(the) other(s):}another");
			현재지문 = 현재지문.Replace("ⓢ{{어법:≠(the) other(s):}another", "{어법:≠(the) other(s):}ⓢ{another");
			현재지문 = 현재지문.Replace("ⓞ{{어법:≠(the) other(s):}another", "{어법:≠(the) other(s):}ⓞ{another");
			현재지문 = 현재지문.Replace("ⓘ{{어법:≠(the) other(s):}another", "{어법:≠(the) other(s):}ⓘ{another");
			현재지문 = 현재지문.Replace("ⓓ{{어법:≠(the) other(s):}another", "{어법:≠(the) other(s):}ⓓ{another");
			현재지문 = 현재지문.Replace("ⓒ{{어법:≠(the) other(s):}another", "{어법:≠(the) other(s):}ⓒ{another");



			현재지문 = 현재지문.Replace("Other", "{어법:≠Another:}Other");
			현재지문 = 현재지문.Replace("ⓢ{{어법:≠Another:}Other", "{어법:≠Another:}ⓢ{Other");
			현재지문 = 현재지문.Replace("ⓞ{{어법:≠Another:}Other", "{어법:≠Another:}ⓞ{Other");

			현재지문 = 변환.문자열.상황봐서바꾸기(현재지문, " other", " {어법:≠another:}other", "on the other hand", "", "");
			현재지문 = 현재지문.Replace("ⓢ{{어법}other", "{어법:≠another:}ⓢ{other");
			현재지문 = 현재지문.Replace("ⓞ{{어법}other", "{어법:≠another:}ⓞ{other");
			현재지문 = 현재지문.Replace("ⓘ{{어법}other", "{어법:≠another:}ⓘ{other");
			현재지문 = 현재지문.Replace("ⓓ{{어법}other", "{어법:≠another:}ⓓ{other");
			현재지문 = 현재지문.Replace("ⓒ{{어법}other", "{어법:≠another:}ⓒ{other");

			현재지문 = 현재지문.Replace("{어법:≠another:}{어법:≠another:}", "{어법:≠another:}");


			#endregion
			#region 전치사 + 관계대명사
			현재지문 = 현재지문.Replace(" by which ", " {중요}by which ");
			현재지문 = 현재지문.Replace("ⓧ{by which}", "{중요}ⓧ{by which}");

			현재지문 = 현재지문.Replace(" from which ", " {중요}from which ");
			현재지문 = 현재지문.Replace("ⓧ{from which}", "{중요}ⓧ{from which}");

			현재지문 = 현재지문.Replace(" in which ", " {중요}in which ");
			현재지문 = 현재지문.Replace("ⓧ{in which}", "{중요}ⓧ{in which}");

			현재지문 = 현재지문.Replace(" on which ", " {중요}on which ");
			현재지문 = 현재지문.Replace("ⓧ{on which}", "{중요}ⓧ{on which}");

			현재지문 = 현재지문.Replace(" through which ", " {중요}through which ");
			현재지문 = 현재지문.Replace("ⓧ{through which}", "{중요}ⓧ{through which}");

			현재지문 = 현재지문.Replace(" with which ", " {중요}with which ");
			현재지문 = 현재지문.Replace("ⓧ{with which}", "{중요}ⓧ{with which}");

			현재지문 = 현재지문.Replace(" without which ", " {중요}without which ");
			현재지문 = 현재지문.Replace("ⓧ{without which}", "{중요}ⓧ{without which}");

			#endregion

			#region used to
			현재지문 = 현재지문.Replace(" used to ", " {중요}used to ");
			현재지문 = 현재지문.Replace(" ⓒ{used} ⓧ{to ", " {중요}ⓒ{used} ⓧ{to ");
			#endregion
			#region be made
			현재지문 = 현재지문.Replace(" be made of ", " be made {어법:≠from:}of ");
			현재지문 = 현재지문.Replace(" been made of ", " been made {어법:≠from:}of ");
			현재지문 = 현재지문.Replace(" being made of ", " being made {어법:≠from:}of ");
			현재지문 = 현재지문.Replace(" is made of ", " is made {어법:≠from:}of ");
			현재지문 = 현재지문.Replace(" are made of ", " are made {어법:≠from:}of ");
			현재지문 = 현재지문.Replace(" was made of ", " was made {어법:≠from:}of ");
			현재지문 = 현재지문.Replace(" were made of ", " were made {어법:≠from:}of ");

			현재지문 = 현재지문.Replace(" be made from ", " be made {어법:≠of:}from ");
			현재지문 = 현재지문.Replace(" been made from ", " been made {어법:≠of:}from ");
			현재지문 = 현재지문.Replace(" being made from ", " being made {어법:≠of:}from ");
			현재지문 = 현재지문.Replace(" is made from ", " is made {어법:≠of:}from ");
			현재지문 = 현재지문.Replace(" are made from ", " are made {어법:≠of:}from ");
			현재지문 = 현재지문.Replace(" was made from ", " was made {어법:≠of:}from ");
			현재지문 = 현재지문.Replace(" were made from ", " were made {어법:≠of:}from ");

			현재지문 = 현재지문.Replace(" be} ⓒ{made} ⓧ{of ", " be} ⓒ{made} {어법:≠from:}ⓧ{of ");
			현재지문 = 현재지문.Replace(" been} ⓒ{made} ⓧ{of ", " been} ⓒ{made} {어법:≠from:}ⓧ{of ");
			현재지문 = 현재지문.Replace(" being} ⓒ{made} ⓧ{of ", " being} ⓒ{made} {어법:≠from:}ⓧ{of ");
			현재지문 = 현재지문.Replace("ⓥ{is} ⓒ{made} ⓧ{of ", "ⓥ{is} ⓒ{made} {어법:≠from:}ⓧ{of ");
			현재지문 = 현재지문.Replace("ⓥ{are} ⓒ{made} ⓧ{of ", "ⓥ{are} ⓒ{made} {어법:≠from:}ⓧ{of ");
			현재지문 = 현재지문.Replace("ⓥ{was} ⓒ{made} ⓧ{of ", "ⓥ{was} ⓒ{made} {어법:≠from:}ⓧ{of ");
			현재지문 = 현재지문.Replace("ⓥ{were} ⓒ{made} ⓧ{of ", "ⓥ{were} ⓒ{made} {어법:≠from:}ⓧ{of ");

			현재지문 = 현재지문.Replace(" be} ⓒ{made} ⓧ{from ", " be} ⓒ{made} {어법:≠of:}ⓧ{from ");
			현재지문 = 현재지문.Replace(" been} ⓒ{made} ⓧ{from ", " been} ⓒ{made} {어법:≠of:}ⓧ{from ");
			현재지문 = 현재지문.Replace(" being} ⓒ{made} ⓧ{from ", " being} ⓒ{made} {어법:≠of:}ⓧ{from ");
			현재지문 = 현재지문.Replace("ⓥ{is} ⓒ{made} ⓧ{from ", "ⓥ{is} ⓒ{made} {어법:≠of:}ⓧ{from ");
			현재지문 = 현재지문.Replace("ⓥ{are} ⓒ{made} ⓧ{from ", "ⓥ{are} ⓒ{made} {어법:≠of:}ⓧ{from ");
			현재지문 = 현재지문.Replace("ⓥ{was} ⓒ{made} ⓧ{from ", "ⓥ{was} ⓒ{made} {어법:≠of:}ⓧ{from ");
			현재지문 = 현재지문.Replace("ⓥ{were} ⓒ{made} ⓧ{from ", "ⓥ{were} ⓒ{made} {어법:≠of:}ⓧ{from ");

			#endregion
			#region 간접의문문의 어순
			현재지문 = 현재지문.Replace(" how ", " {중요}how ");
			현재지문 = 현재지문.Replace(" ⓞ{ⓧ{how} ", " {중요}ⓞ{ⓧ{how} ");
			현재지문 = 현재지문.Replace(" ⓓ{ⓧ{how} ", " {중요}ⓓ{ⓧ{how} ");
			현재지문 = 현재지문.Replace(" ⓒ{ⓧ{how} ", " {중요}ⓒ{ⓧ{how} ");

			현재지문 = 현재지문.Replace(" what ", " {중요}what ");
			현재지문 = 현재지문.Replace(" ⓒ{ⓒ{what} ", " {중요}ⓒ{ⓒ{what} ");
			현재지문 = 현재지문.Replace(" ⓞ{ⓒ{what} ", " {중요}ⓞ{ⓒ{what} ");
			현재지문 = 현재지문.Replace(" ⓒ{ⓞ{what} ", " {중요}ⓒ{ⓞ{what} ");
			현재지문 = 현재지문.Replace(" ⓞ{ⓞ{what} ", " {중요}ⓞ{ⓞ{what} ");

			현재지문 = 현재지문.Replace(" who ", " {중요}who ");
			현재지문 = 현재지문.Replace(" ⓒ{ⓒ{who} ", " {중요}ⓒ{ⓒ{who} ");
			현재지문 = 현재지문.Replace(" ⓞ{ⓒ{who} ", " {중요}ⓞ{ⓒ{who} ");
			현재지문 = 현재지문.Replace(" ⓒ{ⓞ{who} ", " {중요}ⓒ{ⓞ{who} ");
			현재지문 = 현재지문.Replace(" ⓞ{ⓞ{who} ", " {중요}ⓞ{ⓞ{who} ");

			현재지문 = 현재지문.Replace(" whom ", " {중요}whom ");
			현재지문 = 현재지문.Replace(" ⓒ{ⓒ{whom} ", " {중요}ⓒ{ⓒ{whom} ");
			현재지문 = 현재지문.Replace(" ⓞ{ⓒ{whom} ", " {중요}ⓞ{ⓒ{whom} ");
			현재지문 = 현재지문.Replace(" ⓒ{ⓞ{whom} ", " {중요}ⓒ{ⓞ{whom} ");
			현재지문 = 현재지문.Replace(" ⓞ{ⓞ{whom} ", " {중요}ⓞ{ⓞ{whom} ");
			#endregion

			현재지문 = 현재지문.Replace("similar to", "similar {어법:≠with:}to");
			현재지문 = 현재지문.Replace("ⓒ{similar} ⓧ{to", "ⓒ{similar} {어법:≠with:}ⓧ{to");

			현재지문 = 현재지문.Replace(" suitable for ", " suitable {어법}for ");
			현재지문 = 현재지문.Replace(" ⓒ{suitable} ⓧ{for ", " suitable {어법}for ");

			현재지문 = 현재지문.Replace(" proper for ", " proper {어법}for ");
			현재지문 = 현재지문.Replace(" ⓒ{proper} ⓧ{for ", " proper {어법}for ");

			현재지문 = 문제.어법문제출제가능유형찾아표시하기(현재지문, "worth", "동명사");



			#region 일치
			현재지문 = 현재지문.Replace(" 1", " {일치}1");
			현재지문 = 현재지문.Replace(" 2", " {일치}2");
			현재지문 = 현재지문.Replace(" 3", " {일치}3");
			현재지문 = 현재지문.Replace(" 4", " {일치}4");
			현재지문 = 현재지문.Replace(" 5", " {일치}5");
			현재지문 = 현재지문.Replace(" 6", " {일치}6");
			현재지문 = 현재지문.Replace(" 7", " {일치}7");
			현재지문 = 현재지문.Replace(" 8", " {일치}8");
			현재지문 = 현재지문.Replace(" 9", " {일치}9");

			현재지문 = 현재지문.Replace(" hundreds", " {일치}hundreds");
			현재지문 = 현재지문.Replace(" thousands", " {일치}thousands");
			#endregion

			현재지문 = 현재지문.Replace("{중요}{중요}", "{중요}");
			현재지문 = 현재지문.Replace("{빈칸}{빈칸}", "{빈칸}");
			현재지문 = 현재지문.Replace("{/빈칸}{/빈칸}", "{/빈칸}");


			본문.SelectAll();

			선택위치에바꿀말넣고키워드색상입히기(현재지문);


			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();

			화면업데이트재개();
		}

		private void 메뉴_자동입력_예상문제유형입력_지시(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("지시");
		}

		private void 메뉴_자동입력_예상문제유형입력_주제(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치를주어진문제유형으로감싸기("주제");
		}

		private void 메뉴_자동입력_예상문제유형입력_제목(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("제목");
		}

		private void 메뉴_자동입력_예상문제유형입력_속담(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("속담");
		}

		private void 메뉴_자동입력_예상문제유형입력_빈칸(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치를주어진문제유형으로감싸기("빈칸");
		}

		private void 메뉴_자동입력_예상문제유형입력_요약(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("요약");
		}

		private void 메뉴_자동입력_예상문제유형입력_어법(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("어법");
		}

		private void 메뉴_자동입력_예상문제유형입력_어휘(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("어휘");
		}

		private void 메뉴_자동입력_예상문제유형입력_분위기(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("분위기");
		}

		private void 메뉴_자동입력_예상문제유형입력_일치(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("일치");
		}

		private void 메뉴_자동입력_예상문제유형입력_흐름(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("흐름");
		}

		private void 유틸_자동입력_예상문제유형입력_선택위치를주어진문제유형으로감싸기(string 문제유형)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;
			string 바꿀말;

			바꿀말 = string.Format("{{{0}}}{1}{{/{2}}}", 문제유형, 본문.SelectedText, 문제유형);

			선택위치에바꿀말넣고키워드색상입히기(바꿀말);

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();

		}


		private void 유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기(string 문제유형)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;
			string 바꿀말;

			바꿀말 = string.Format("{{{0}}}", 문제유형);

			선택위치에바꿀말넣고키워드색상입히기(바꿀말);

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}

		#endregion
		#region 구문분석
		private void 서술어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 다음구문분석위치로이동할필요가있는지 = true;

			_최근구문분석 = "서술어";

			
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			string 바꿀말;



			if (본문.SelectedText != "")
			{
				if (본문.SelectedText[0] == ' ') { 본문.SelectionStart++; 본문.SelectionLength--; }

				
			}
			else
			{
				선택위치에바꿀말넣고키워드색상입히기("ⓥ{");

				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();
				다음구문분석위치로이동();

				return;
			}


			if(본문.SelectedText.EndsWith(" "))
			{ 
				바꿀말 = string.Format("ⓥ{{{0}}} ", 본문.SelectedText.TrimEnd());
				다음구문분석위치로이동할필요가있는지 = false;
			}
			else
				바꿀말 = string.Format("ⓥ{{{0}}}", 본문.SelectedText);

			바꿀말 = 바꿀말.Replace("’", "'");
			바꿀말 = 바꿀말.Replace("'d ", "ⓧ{'d} ");
			바꿀말 = 바꿀말.Replace("am able to ", "ⓧ{am able to} ");
			바꿀말 = 바꿀말.Replace("is able to ", "ⓧ{is able to} ");
			바꿀말 = 바꿀말.Replace("are able to ", "ⓧ{are able to} ");
			바꿀말 = 바꿀말.Replace("was able to ", "ⓧ{was able to} ");
			바꿀말 = 바꿀말.Replace("were able to ", "ⓧ{were able to} ");
			바꿀말 = 바꿀말.Replace("be able to ", "ⓧ{be able to} ");
			바꿀말 = 바꿀말.Replace("has been able to ", "ⓧ{has been able to} ");
			바꿀말 = 바꿀말.Replace("have been able to ", "ⓧ{have been able to} ");
			바꿀말 = 바꿀말.Replace("had been able to ", "ⓧ{had been able to} ");

			바꿀말 = 바꿀말.Replace("am about to ", "ⓧ{am about to} ");
			바꿀말 = 바꿀말.Replace("is about to ", "ⓧ{is about to} ");
			바꿀말 = 바꿀말.Replace("are about to ", "ⓧ{are about to} ");
			바꿀말 = 바꿀말.Replace("was about to ", "ⓧ{was about to} ");
			바꿀말 = 바꿀말.Replace("were about to ", "ⓧ{were about to} ");

			바꿀말 = 바꿀말.Replace("'m going to ", "ⓧ{'m going to} ");
			바꿀말 = 바꿀말.Replace("is going to ", "ⓧ{is going to} ");
			바꿀말 = 바꿀말.Replace("am going to ", "ⓧ{am going to} ");
			바꿀말 = 바꿀말.Replace("are going to ", "ⓧ{are going to} ");
			바꿀말 = 바꿀말.Replace("was going to ", "ⓧ{was going to} ");
			바꿀말 = 바꿀말.Replace("were going to ", "ⓧ{were going to} ");


			#region 조동사
			바꿀말 = 바꿀말.Replace("can ", "ⓧ{can} ");
			바꿀말 = 바꿀말.Replace("can't ", "ⓧ{can't} ");
			바꿀말 = 바꿀말.Replace("cannot ", "ⓧ{cannot} ");

			바꿀말 = 바꿀말.Replace("don't ", "ⓧ{don't} ");
			바꿀말 = 바꿀말.Replace("Don't ", "ⓧ{Don't} ");
			바꿀말 = 바꿀말.Replace("doesn't ", "ⓧ{doesn't} ");


			바꿀말 = 바꿀말.Replace("could ", "ⓧ{could} ");
			바꿀말 = 바꿀말.Replace("couldn't ", "ⓧ{couldn't} ");

			바꿀말 = 바꿀말.Replace("might ", "ⓧ{might} ");


			바꿀말 = 바꿀말.Replace("should ", "ⓧ{should} ");
			바꿀말 = 바꿀말.Replace("shouldn't ", "ⓧ{shouldn't} ");

			if(바꿀말.Contains("would like to "))
			{
				바꿀말 = 바꿀말.Replace("would like to ", "ⓧ{would like to} ");
			}
			else if(바꿀말.Contains("would"))
			{
				바꿀말 = 바꿀말.Replace("would ", "ⓧ{would} ");
			}

			바꿀말 = 바꿀말.Replace("wouldn't ", "ⓧ{wouldn't} ");

			바꿀말 = 바꿀말.Replace("will ", "ⓧ{will} ");

			바꿀말 = 바꿀말.Replace("has to ", "ⓧ{has to} ");
			바꿀말 = 바꿀말.Replace("had to ", "ⓧ{had to} ");
			바꿀말 = 바꿀말.Replace("have to ", "ⓧ{have to} ");

			if (바꿀말.IndexOf("may ") == 2)
				바꿀말 = 바꿀말.Replace("may ", "ⓧ{may} ");
			바꿀말 = 바꿀말.Replace("must ", "ⓧ{must} ");
			바꿀말 = 바꿀말.Replace("mustn't ", "ⓧ{mustn't} ");
			바꿀말 = 바꿀말.Replace("shall ", "ⓧ{shall} ");
			#endregion

			#region 부사들
			바꿀말 = 바꿀말.Replace(" not ", " ⓧ{not} ");
			바꿀말 = 바꿀말.Replace(" first ", " ⓧ{first} ");
			바꿀말 = 바꿀말.Replace(" often ", " ⓧ{often} ");
			바꿀말 = 바꿀말.Replace(" usually ", " ⓧ{usually} ");
			바꿀말 = 바꿀말.Replace(" truly ", " ⓧ{truly} ");
			바꿀말 = 바꿀말.Replace(" always ", " ⓧ{always} ");

			바꿀말 = 바꿀말.Replace(" also ", " ⓧ{also} ");


			List<string> 어절들 = new List<string>();

			string 부사처리결과 = "";

			foreach(string 현재어절 in 어절들)
			{
				string 부사의미 = Form1._검색.부사의미추출(현재어절);

				if (부사의미 != "")
					부사처리결과 += "ⓧ{" + 현재어절 + "}";
				else
					부사처리결과 += 현재어절 + " ";
			}
			부사처리결과 = 부사처리결과.Trim();


			문자열.어절들로(바꿀말, ref 어절들);
			#endregion

			선택위치에바꿀말넣고키워드색상입히기(바꿀말);

			_텍스트변경사항자동저장불필요 = false;

			if(다음구문분석위치로이동할필요가있는지)
				다음구문분석위치로이동();
		}

		private void 주어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 다음구문분석위치로옮길지여부 = false;
			if (본문.SelectedText != "") 다음구문분석위치로옮길지여부 = true;

			선택위치에주어진문법표지입히기("ⓢ", ref 다음구문분석위치로옮길지여부);

			if (다음구문분석위치로옮길지여부) 다음구문분석위치로이동();

            _최근구문분석 = "주어";

        }

        private void 다음구문분석위치로이동()
		{
			int 현재커서위치 = 본문.SelectionStart;

			int 다음어포스트로피위치 = 본문.Text.IndexOf("'", 현재커서위치);
			int 다음어포스트로피위치_ = 본문.Text.IndexOf("’", 현재커서위치);

			int 다음빈칸위치 = 본문.Text.IndexOf(" ", 현재커서위치);
			int 다음개행문자위치 = 본문.Text.IndexOf("\n", 현재커서위치);

			if ((다음어포스트로피위치 == 현재커서위치) || (다음어포스트로피위치_ == 현재커서위치))
			{
				// You'll
			}
			else if ((다음빈칸위치 == -1) && (다음개행문자위치 == -1))
			{

			}
			else if ((다음빈칸위치 == -1) && (다음개행문자위치 != -1))
			{

			}
			else if ((다음빈칸위치 != -1) && (다음개행문자위치 == -1))
				본문.SelectionStart = 다음빈칸위치 + 1;
			else if (다음빈칸위치 < 다음개행문자위치)
				본문.SelectionStart = 다음빈칸위치 + 1;
			else if (다음개행문자위치 < 다음빈칸위치)
			{

			}
		}

		private void 목적어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 다음구문분석위치로옮길지여부 = false;
			if (본문.SelectedText != "") 다음구문분석위치로옮길지여부 = true;

			선택위치에주어진문법표지입히기("ⓞ", ref 다음구문분석위치로옮길지여부);

			if (다음구문분석위치로옮길지여부) 다음구문분석위치로이동();

            _최근구문분석 = "목적어";
        }

        private void 보어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 다음구문분석위치로옮길지여부 = false;
			if (본문.SelectedText != "") 다음구문분석위치로옮길지여부 = true;

            if(_최근구문분석 == "목적어")
			    선택위치에주어진문법표지입히기("{중요}ⓒ", ref 다음구문분석위치로옮길지여부);
            else
                선택위치에주어진문법표지입히기("ⓒ", ref 다음구문분석위치로옮길지여부);

            if (다음구문분석위치로옮길지여부) 다음구문분석위치로이동();

            _최근구문분석 = "보어";
		}

		private void 수식어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 다음구문분석위치로옮길지여부 = false;
			if (본문.SelectedText != "") 다음구문분석위치로옮길지여부 = true;

			선택위치에주어진문법표지입히기("ⓧ", ref 다음구문분석위치로옮길지여부);

			if (다음구문분석위치로옮길지여부) 다음구문분석위치로이동();

            _최근구문분석 = "수식어";
        }

        private void 간접목적어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 다음구문분석위치로옮길지여부 = false;
			if (본문.SelectedText != "") 다음구문분석위치로옮길지여부 = true;

			선택위치에주어진문법표지입히기("ⓘ", ref 다음구문분석위치로옮길지여부);

			if (다음구문분석위치로옮길지여부) 다음구문분석위치로이동();

            _최근구문분석 = "간접목적어";
		}

		private void 직접목적어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 다음구문분석위치로옮길지여부 = false;
			if (본문.SelectedText != "") 다음구문분석위치로옮길지여부 = true;

			선택위치에주어진문법표지입히기("ⓓ", ref 다음구문분석위치로옮길지여부);

			if (다음구문분석위치로옮길지여부) 다음구문분석위치로이동();

            _최근구문분석 = "직접목적어";
		}

		private void 접속어ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool 안쓰는변수 = true;

			if (본문.SelectedText == "As a result") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "as a result") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "For example") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "for example") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "For instance") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "for instance") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "Moreover") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "moreover") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "At last") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "at last") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "Besides") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "besides") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "By the way") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "by the way") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "In a word") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "in a word") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "In fact") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "in fact") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "In the end") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "in the end") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "In the long run") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "in the long run") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }

			else if (본문.SelectedText == "Instead") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "instead") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }

			else if (본문.SelectedText == "Nonetheless") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "nonetheless") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "On the contrary") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "on the contrary") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "On the other hand") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "on the other hand") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "Thus") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "thus") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "Furthermore") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "furthermore") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "Nevertheless") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "nevertheless") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }

			else if (본문.SelectedText == "Therefore") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "therefore") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }

			else if (본문.SelectedText == "Then") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if (본문.SelectedText == "then") { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }
			else if(_검색.접속어의미추출(본문.SelectedText) == "" && _검색.부사의미추출(본문.SelectedText) != "" ) { 선택위치에주어진문법표지입히기("ⓧ", ref 안쓰는변수); }

			else { 선택위치에주어진문법표지입히기("㉨", ref 안쓰는변수); }

			다음구문분석위치로이동();

            _최근구문분석 = "접속어";
		}

		private void 선택위치에주어진문법표지입히기(string 문법표지, ref bool 다음구문분석위치로이동여부)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;
			string 바꿀말;

			
			if (본문.SelectedText != "")
			{
				// 앞에 한 칸 정도 빈칸이 있는 경우 이를 무시하고 다음 칸부터 처리하는 부분
				if (본문.SelectedText[0] == ' ') { 본문.SelectionStart++; 본문.SelectionLength--; }

				// 맨 뒤에 빈칸이 있는 경우 이를 무시하는 부분,

				if(본문.SelectedText.EndsWith(" "))
				{
					바꿀말 = string.Format("{0}{{{1}}} ", 문법표지, 본문.SelectedText.TrimEnd());
					다음구문분석위치로이동여부 = false;
				}
				else
				{
					바꿀말 = string.Format("{0}{{{1}}}", 문법표지, 본문.SelectedText);
					다음구문분석위치로이동여부 = true;
				}
			}
			else
			{
				바꿀말 = string.Format("{0}{{", 문법표지);
			}



			선택위치에바꿀말넣고키워드색상입히기(바꿀말);

			_텍스트변경사항자동저장불필요 = false;
			//현재내용을_실행취소용_클립보드에_저장();
		}

		#endregion
		#region 태그입력

		private string 현재문제의Q번호추측()
		{

			string q = 앞부분Q내용가져오기();

			if (q.Contains("["))
			{
				q = q.Replace(" ", "");

				string 포함되었는지확인할스트링 = "";
				string 포함되었는지확인할스트링2 = "";
				string 결과값 = "";

				for (int i = 1; i < 100; i++)
				{
					포함되었는지확인할스트링 = string.Format("[{0}~", i);
					포함되었는지확인할스트링2 = string.Format("[{0}-", i);
					결과값 = string.Format("{0}", i);

					if (q.Contains(포함되었는지확인할스트링)) return 결과값;
					if (q.Contains(포함되었는지확인할스트링2)) return 결과값;
				}
			}

			if (q.Contains("."))
			{
				int 숫자값 = 0;
				string 숫자여야하는스트링 = q.Substring(0, q.IndexOf("."));
				bool 숫자인지확인 = int.TryParse(숫자여야하는스트링, out 숫자값);

				if (숫자인지확인)
				{
					return string.Format("{0}", 숫자값 + 1);
				}
				else
					return "";
			}


			return "";
		}

		private string 앞부분Q내용가져오기()
		{
			if (_CAKE_인덱스 == 0)
				return "";

			string 케이크전체 = _CAKE들[_CAKE_인덱스 - 1];
			return 변환.문자열.Q태그내용(케이크전체);
		}

		private string T태그매기기(string 현재선택, string Q내용)
		{
			현재선택 = 현재선택.Replace("：", ":");
			현재선택 = 현재선택.Replace("??", "*");

			if (강력하지만무거운변환.문자열.개행문자제거할지판단(현재선택, Q내용))
			{
				if ((현재선택.Contains("(A)") && 현재선택.Contains("(B)") && 현재선택.Contains("(C)")) || Q내용.Contains("도표"))
					현재선택 = 강력하지만무거운변환.문자열.개행문자제거(현재선택);
				else
					현재선택 = 강력하지만무거운변환.문자열.지능형개행문자제거(현재선택);


				현재선택 = 현재선택.Replace("\n<TBAR></TBAR> ", "\n\t<TBAR><_TBAR>\n");
				현재선택 = 현재선택.Replace(" <TBAR></TBAR> ", "\n\t<TBAR><_TBAR>\n");
				현재선택 = 현재선택.Replace("\n<TBAR></TBAR>", "\n\t<TBAR><_TBAR>\n");
				현재선택 = 현재선택.Replace(" <TBAR></TBAR>", "\n\t<TBAR><_TBAR>\n");
				현재선택 = 현재선택.Replace("<TBAR></TBAR>", "\n\t<TBAR><_TBAR>\n");

				// 
				if (!현재선택.Contains("/") && !현재선택.Contains("___(A)___"))
				{
					현재선택 = 현재선택.Replace("(A)", "\t(A)");
					현재선택 = 현재선택.Replace("(B)", "\n\t(B)");
					현재선택 = 현재선택.Replace("(C)", "\n\t(C)");
					현재선택 = 현재선택.Replace("(D)", "\n\t(D)");

				}

				현재선택 = 현재선택.Replace("<_TBAR>", "</TBAR>");
			}

			현재선택 = 현재선택.Replace("`─`", " - ");
			현재선택 = 현재선택.Replace("`", " ");

			while (현재선택.Contains("  "))
				현재선택 = 현재선택.Replace("  ", " ");

			현재선택 = 현재선택.Replace("\n", "\n\t");
			현재선택 = 현재선택.Replace("\t\t", "\t");
			return string.Format("\t<T>\n\t{0}\n\t</T>", 현재선택.Trim());
		}

		private void 메뉴_자동입력_태그입력_A태그(object sender, EventArgs e)
		{
			string 현재선택 = 본문.SelectedText;
			string A0 = "", A1 = "", A2 = "", A3 = "", A4 = "", A5 = "";

			if (A태그원형복원.복원결과(현재선택, 본문.Text, ref A0, ref A1, ref A2, ref A3, ref A4, ref A5))
			{
				ABC.Text = A0;
				보기1Text.Text = A1;
				보기2Text.Text = A2;
				보기3Text.Text = A3;
				보기4Text.Text = A4;
				보기5Text.Text = A5;

				본문.SelectedText = "";
			}
		}
		private void 메뉴_자동입력_태그입력_TBAR태그(object sender, EventArgs e)
		{
            if (본문.Focused)
            {
                현재내용을_실행취소용_클립보드에_저장();
                _텍스트변경사항자동저장불필요 = true;

                //MessageBox.Show("test");
                int 현재커서위치 = 본문.SelectionStart;
                int 앞의개행문자위치 = 0;
                if (현재커서위치 != 0) 앞의개행문자위치 = 본문.Text.LastIndexOf("\n", 현재커서위치 - 1, 현재커서위치);

                if (현재커서위치 == 0)
                    본문.SelectedText = "<TBAR></TBAR>";
                else if (현재커서위치 != 앞의개행문자위치 + 1)
                    본문.SelectedText = "<TBAR></TBAR>";
                else
                    본문.SelectedText = "<TBAR></TBAR>";

                현재커서근방의키워드색상업데이트();

                _텍스트변경사항자동저장불필요 = false;
                현재내용을_실행취소용_클립보드에_저장();
            }
            else if(해석.Focused)
            {
                해석.SelectedText = "<TBAR></TBAR>";
            }
        }
		private void 대괄호입히기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			선택위치에대괄호입히기();
		}
		private void 선택위치에대괄호입히기()
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			string 현재선택 = 본문.SelectedText;
			string 바꾼내용 = string.Format("[{0}]", 현재선택);
			선택위치에바꿀말넣고키워드색상입히기(바꾼내용);

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();

		}

		#endregion
		#region 전각문자입력
		private void 메뉴_자동입력_전각문자입력_화살표(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

            if (제목.Focused) 제목.SelectedText = "→";
            if (질문.Focused) 질문.SelectedText = "→";
            if (본문.Focused) 본문.SelectedText = "→";

            if (ABC.Focused) ABC.SelectedText = "→";
            if (보기1Text.Focused) 보기1Text.SelectedText = "→";
            if (보기2Text.Focused) 보기2Text.SelectedText = "→";
            if (보기3Text.Focused) 보기3Text.SelectedText = "→";
            if (보기4Text.Focused) 보기4Text.SelectedText = "→";
            if (보기5Text.Focused) 보기5Text.SelectedText = "→";
            if (주관식정답.Focused) 주관식정답.SelectedText = "→";
            if (해석.Focused) 해석.SelectedText = "→";
            if (힌트.Focused) 힌트.SelectedText = "→";
            if (중요어휘.Focused) 중요어휘.SelectedText = "→";

			if (보기1_해설.Focused) 보기1_해설.SelectedText = "→";
			if (보기2_해설.Focused) 보기2_해설.SelectedText = "→";
			if (보기3_해설.Focused) 보기3_해설.SelectedText = "→";
			if (보기4_해설.Focused) 보기4_해설.SelectedText = "→";
			if (보기5_해설.Focused) 보기5_해설.SelectedText = "→";



			//			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 메뉴_자동입력_전각문자입력_점동그라미(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

            if (제목.Focused) 제목.SelectedText = "⊙";
            if (질문.Focused) 질문.SelectedText = "⊙";
            if (본문.Focused) 본문.SelectedText = "⊙";

            if (ABC.Focused) ABC.SelectedText = "⊙";
            if (보기1Text.Focused) 보기1Text.SelectedText = "⊙";
            if (보기2Text.Focused) 보기2Text.SelectedText = "⊙";
            if (보기3Text.Focused) 보기3Text.SelectedText = "⊙";
            if (보기4Text.Focused) 보기4Text.SelectedText = "⊙";
            if (보기5Text.Focused) 보기5Text.SelectedText = "⊙";
            if (주관식정답.Focused) 주관식정답.SelectedText = "⊙";
            if (해석.Focused) 해석.SelectedText = "⊙";
            if (힌트.Focused) 힌트.SelectedText = "⊙";
            if (중요어휘.Focused) 중요어휘.SelectedText = "⊙";

            //본문.SelectedText = "⊙";

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		#endregion
		#region 정답입력
		private void 메뉴_자동입력_정답입력_정답1번(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			if (앞부분Q내용가져오기().Contains("위 글") || 앞부분Q내용가져오기().Contains("(a)~(e)") || 앞부분Q내용가져오기().Contains("(a) ~ (e)") || 앞부분Q내용가져오기().Contains("(A)에 이어질"))
				선택위치에바꿀말넣고키워드색상입히기("\t<TR>\n\t정답 ①번\n\t</TR>");
			else
				선택위치에바꿀말넣고키워드색상입히기("정답 ①번\n<TBAR></TBAR>");



			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 메뉴_자동입력_정답입력_정답2번(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			if (앞부분Q내용가져오기().Contains("위 글") || 앞부분Q내용가져오기().Contains("(a)~(e)") || 앞부분Q내용가져오기().Contains("(a) ~ (e)") || 앞부분Q내용가져오기().Contains("(A)에 이어질"))
				선택위치에바꿀말넣고키워드색상입히기("\t<TR>\n\t정답 ②번\n\t</TR>");
			else
				선택위치에바꿀말넣고키워드색상입히기("정답 ②번\n<TBAR></TBAR>");

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 메뉴_자동입력_정답입력_정답3번(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			if (앞부분Q내용가져오기().Contains("위 글") || 앞부분Q내용가져오기().Contains("(a)~(e)") || 앞부분Q내용가져오기().Contains("(a) ~ (e)") || 앞부분Q내용가져오기().Contains("(A)에 이어질"))
				선택위치에바꿀말넣고키워드색상입히기("\t<TR>\n\t정답 ③번\n\t</TR>");
			else
				선택위치에바꿀말넣고키워드색상입히기("정답 ③번\n<TBAR></TBAR>");

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 메뉴_자동입력_정답입력_정답4번(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			if (앞부분Q내용가져오기().Contains("위 글") || 앞부분Q내용가져오기().Contains("(a)~(e)") || 앞부분Q내용가져오기().Contains("(a) ~ (e)") || 앞부분Q내용가져오기().Contains("(A)에 이어질"))
				선택위치에바꿀말넣고키워드색상입히기("\t<TR>\n\t정답 ④번\n\t</TR>");
			else
				선택위치에바꿀말넣고키워드색상입히기("정답 ④번\n<TBAR></TBAR>");

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 메뉴_자동입력_정답입력_정답5번(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			if (앞부분Q내용가져오기().Contains("위 글") || 앞부분Q내용가져오기().Contains("(a)~(e)") || 앞부분Q내용가져오기().Contains("(a) ~ (e)") || 앞부분Q내용가져오기().Contains("(A)에 이어질"))
				선택위치에바꿀말넣고키워드색상입히기("\t<TR>\n\t정답 ⑤번\n\t</TR>");
			else
				선택위치에바꿀말넣고키워드색상입히기("정답 ⑤번\n<TBAR></TBAR>");

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		#endregion
		#region 밑줄, 하이픈 입력
		private void 빈칸입력toolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (본문.Focused)
            {
                현재내용을_실행취소용_클립보드에_저장();
                _텍스트변경사항자동저장불필요 = true;

                본문.SelectedText = "______";

                현재커서근방의키워드색상업데이트();

                _텍스트변경사항자동저장불필요 = false;
                현재내용을_실행취소용_클립보드에_저장();
            }
            else if(해석.Focused)
            {
                해석.SelectedText = "______";
            }
            else if (힌트.Focused)
            {
                힌트.SelectedText = "______";
            }
        }

        private void 구분선입력toolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (ABC.Focused) { ABC.SelectedText = "……"; }
            if (보기1Text.Focused) { 보기1Text.SelectedText = "……"; }
            if (보기2Text.Focused) { 보기2Text.SelectedText = "……"; }
            if (보기3Text.Focused) { 보기3Text.SelectedText = "……"; }
            if (보기4Text.Focused) { 보기4Text.SelectedText = "……"; }
            if (보기5Text.Focused) { 보기5Text.SelectedText = "……"; }
		}
		#endregion

		#endregion
		#region 위자드
		private void 위자드1_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			string 선택텍스트 = 본문.SelectedText;
			본문.SelectedText = "① [" + 선택텍스트 + "]";

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 위자드2_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			string 선택텍스트 = 본문.SelectedText;
			본문.SelectedText = "② [" + 선택텍스트 + "]";

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 위자드3_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			string 선택텍스트 = 본문.SelectedText;
			본문.SelectedText = "③ [" + 선택텍스트 + "]";

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 위자드4_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			string 선택텍스트 = 본문.SelectedText;
			본문.SelectedText = "④ [" + 선택텍스트 + "]";

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void 위자드5_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			_텍스트변경사항자동저장불필요 = true;

			string 선택텍스트 = 본문.SelectedText;
			본문.SelectedText = "⑤ [" + 선택텍스트 + "]";

			현재커서근방의키워드색상업데이트();

			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();
		}
		#endregion
		#region 우클릭
		private string 선택()
		{
			string 현재선택 = "";
			if (본문.Focused) 현재선택 = 본문.SelectedText.Trim().불필요제거_사전용();
			else if (보기1Text.Focused) 현재선택 = 보기1Text.SelectedText.Trim().불필요제거_사전용();
			else if (보기2Text.Focused) 현재선택 = 보기2Text.SelectedText.Trim().불필요제거_사전용();
			else if (보기3Text.Focused) 현재선택 = 보기3Text.SelectedText.Trim().불필요제거_사전용();
			else if (보기4Text.Focused) 현재선택 = 보기4Text.SelectedText.Trim().불필요제거_사전용();
			else if (보기5Text.Focused) 현재선택 = 보기5Text.SelectedText.Trim().불필요제거_사전용();

			else if (변형지문.Focused) 현재선택 = 변형지문.SelectedText.Trim().불필요제거_사전용();
			else if (변형지문해석.Focused) 현재선택 = 변형지문해석.SelectedText.Trim().불필요제거_사전용();

			return 현재선택;
		}

		public void 사전편집창띄우기()
		{
			string 표제어 = "";
			string 검색결과 = _검색.영한사전_어떻게든_결과를_내는(선택(), ref 표제어);

			if (선택() != "")
			{
				string 발음기호 = _검색.영한발음기호사전(표제어);

				if (표제어 != "")
				{
					사전페이지_사전표제어.Text = 표제어;
					사전페이지_사전발음기호.Text = 발음기호;
					사전페이지_사전의미.Text = 검색결과;
				}
				else
				{
					사전페이지_사전표제어.Text = 선택();
					사전페이지_사전발음기호.Text = "";
					사전페이지_사전의미.Text = "검색결과가 없습니다.";
				}
				탭선택(2);
			}
		}

		List<string> l = new List<string>(); // 임시 리스트 변수로 쓸 것입니다.

		public void 우_명사(){			 if(!Ansi파일.목록내존재확인(_DB루트+"빈칸_명사.문제", 선택()))  Ansi파일.목록추가(_DB루트+"빈칸_명사.문제",   선택());}
        public void 우_명사구(){		 if(!Ansi파일.목록내존재확인(_DB루트+"빈칸_명사구.문제",선택())) Ansi파일.목록추가(_DB루트+"빈칸_명사구.문제", 선택());}
		public void 우_형용사(){		 if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_형용사.문제",    ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_형용사.문제");}
        public void 우_동사(){			 if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_동사.문제",      ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_동사.문제");}
        public void 우_동사es(){	     if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_동사es.문제",    ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_동사es.문제");}
        public void 우_동사ed(){		 if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_동사ed.문제",    ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_동사ed.문제");}
		public void 우_서술어시작문구(){ if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_서술어로시작하는문구.문제", ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_서술어로시작하는문구.문제");}
		public void 우_서술어s시작문구(){if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_서술어s로시작하는문구.문제",ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_서술어s로시작하는문구.문제");}
		public void 우_속담빈칸(){       if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_속담.문제",	     ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_속담.문제");}
		public void 우_문장빈칸(){       if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_문장.문제",	     ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_문장.문제");}
		public void 우_영작고교(){       if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"영작_주관식_고.문제", ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"영작_주관식_고.문제");}
		public void 우_영작중3(){        if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"영작_주관식_중3.문제",ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"영작_주관식_중3.문제");}
		public void 우_영작중2(){        if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"영작_주관식_중2.문제",ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"영작_주관식_중2.문제");}
		public void 우_영작중1(){        if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"영작_주관식_중1.문제",ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"영작_주관식_중1.문제");}
		public void 우_빈칸고교(){       if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_주관식_고.문제", ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_주관식_고.문제");}
		public void 우_빈칸중3(){        if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_주관식_중3.문제",ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_주관식_중3.문제");}
		public void 우_빈칸중2(){        if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_주관식_중2.문제",ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_주관식_중2.문제");}
		public void 우_빈칸중1(){        if(선택()=="")return;Ansi파일.문자열들로(_DB루트+"빈칸_주관식_중1.문제",ref l);foreach(string c in l){if(c==선택())return;}l.Add(선택());문자열들.Ansi파일로(l,_DB루트+"빈칸_주관식_중1.문제");}

		private void 우클릭메뉴공통()
		{
			_오른쪽클릭메뉴.Owner = this;
			_오른쪽클릭메뉴.StartPosition = FormStartPosition.Manual;
			_오른쪽클릭메뉴.Location = new Point(Control.MousePosition.X, Control.MousePosition.Y);
			_오른쪽클릭메뉴.Show();
		}
		// 마우스 우클릭으로 사전편집창 띄우는 부분
		private void richTextBox1_MouseUp(object sender, MouseEventArgs e){		if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 보기1Text_MouseUp(object sender, MouseEventArgs e) {		if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 보기2Text_MouseUp(object sender, MouseEventArgs e) {		if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 보기3Text_MouseUp(object sender, MouseEventArgs e) {		if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 보기4Text_MouseUp(object sender, MouseEventArgs e) {		if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 보기5Text_MouseUp(object sender, MouseEventArgs e) {		if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 변형지문_MouseUp( object sender, MouseEventArgs e) {		if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 변형지문해석_MouseUp(object sender, MouseEventArgs e)	{	if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}
		private void 주제_MouseUp(object sender, MouseEventArgs e){				if (e.Button == System.Windows.Forms.MouseButtons.Right) 우클릭메뉴공통();}


        private void 해석_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _해석오른쪽클릭메뉴.Owner = this;
                _해석오른쪽클릭메뉴.StartPosition = FormStartPosition.Manual;
                _해석오른쪽클릭메뉴.Location = new Point(Control.MousePosition.X, Control.MousePosition.Y);

                _해석오른쪽클릭메뉴._선택된스트링 = 해석.SelectedText.Trim();
                _해석오른쪽클릭메뉴._문제DB루트폴더 = _검색._문제DB루트폴더;
                _해석오른쪽클릭메뉴.코퍼스에넣기버튼텍스트업데이트();
                _해석오른쪽클릭메뉴.Show();
                
            }
        }

        void CutAction(object sender, EventArgs e)
		{
			//richTextBox1.Cut();
		}
		public void 사전업데이트(string 표제어, string 발음기호, string 의미)
		{
			_검색.영한사전업데이트(표제어, 발음기호, 의미);
		}
		#endregion

		private void 선택부분자동구문분석_클릭(object sender, EventArgs e)
		{
			if (본문.SelectedText == "")
			{
				현재내용을_실행취소용_클립보드에_저장();
				화면업데이트중지();
				_텍스트변경사항자동저장불필요 = true;


				선택위치에바꿀말넣고키워드색상입히기("___(A)___");


				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();

				화면업데이트재개();
			}
			else
			{
				현재내용을_실행취소용_클립보드에_저장();
				화면업데이트중지();
				_텍스트변경사항자동저장불필요 = true;

				string 문법문제표지제거한선택된본문 = 본문.SelectedText.문법문제표지제거().Replace("’", "'").Trim();

				string 구문분석로그 = "";
				string 구문분석결과 = _구문분석.구문분석(본문.SelectedText, ref 구문분석로그);

				string 문법문제표지제거된구문분석결과 = 구문분석결과.문법문제표지제거().Trim();

				if(문법문제표지제거한선택된본문 == 문법문제표지제거된구문분석결과)
				{
					선택위치에바꿀말넣고키워드색상입히기(구문분석결과);
				}
				else
				{
					MessageBox.Show("구문분석결과에 오류가 있습니다.\n" + 문법문제표지제거한선택된본문 + "\n" + 구문분석결과);
				}
				


				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();

				화면업데이트재개();
			}
		}
		private void 빈칸A_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지();
			_텍스트변경사항자동저장불필요 = true;


			선택위치에바꿀말넣고키워드색상입히기("___(A)___");


			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();

			화면업데이트재개();
		}
		private void 빈칸b_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지();
			_텍스트변경사항자동저장불필요 = true;


			선택위치에바꿀말넣고키워드색상입히기("___(B)___");


			_텍스트변경사항자동저장불필요 = false;
			현재내용을_실행취소용_클립보드에_저장();

			화면업데이트재개();
		}
		#region 동그라미
		private void 동그라미1번toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(제목.Focused){ 제목.SelectedText = "①"; }
            else if (질문.Focused){ 질문.SelectedText = "①"; }
            else if (본문.Focused)
            {
                현재내용을_실행취소용_클립보드에_저장();
                _텍스트변경사항자동저장불필요 = true;

                본문.SelectedText = "①";

                현재커서근방의키워드색상업데이트();

                _텍스트변경사항자동저장불필요 = false;
                현재내용을_실행취소용_클립보드에_저장();
            }
            else if(ABC.Focused)       { ABC.SelectedText = "①"; }
            else if (보기1Text.Focused) { 보기1Text.SelectedText = "①"; }
            else if (보기2Text.Focused) { 보기2Text.SelectedText = "①"; }
            else if (보기3Text.Focused) { 보기3Text.SelectedText = "①"; }
            else if (보기4Text.Focused) { 보기4Text.SelectedText = "①"; }
            else if (보기5Text.Focused) { 보기5Text.SelectedText = "①"; }
            else if (주관식정답.Focused) { 주관식정답.SelectedText = "①"; }
            else if (해석.Focused) { 해석.SelectedText = "①"; }
            else if (힌트.Focused) { 힌트.SelectedText = "①"; }
            else if (중요어휘.Focused) { 중요어휘.SelectedText = "①"; }
        }
        private void 동그라미2번toolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (제목.Focused) { 제목.SelectedText = "②"; }
            else if (질문.Focused) { 질문.SelectedText = "②"; }
            else if (본문.Focused)
            {
                현재내용을_실행취소용_클립보드에_저장();
                _텍스트변경사항자동저장불필요 = true;

                본문.SelectedText = "②";

                현재커서근방의키워드색상업데이트();

                _텍스트변경사항자동저장불필요 = false;
                현재내용을_실행취소용_클립보드에_저장();
            }
            else if (ABC.Focused) { ABC.SelectedText = "②"; }
            else if (보기1Text.Focused) { 보기1Text.SelectedText = "②"; }
            else if (보기2Text.Focused) { 보기2Text.SelectedText = "②"; }
            else if (보기3Text.Focused) { 보기3Text.SelectedText = "②"; }
            else if (보기4Text.Focused) { 보기4Text.SelectedText = "②"; }
            else if (보기5Text.Focused) { 보기5Text.SelectedText = "②"; }
            else if (주관식정답.Focused) { 주관식정답.SelectedText = "②"; }
            else if (해석.Focused) { 해석.SelectedText = "②"; }
            else if (힌트.Focused) { 힌트.SelectedText = "②"; }
            else if (중요어휘.Focused) { 중요어휘.SelectedText = "②"; }
        }
        private void 동그라미3번toolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (제목.Focused) { 제목.SelectedText = "③"; }
            else if (질문.Focused) { 질문.SelectedText = "③"; }
            else if (본문.Focused)
            {
                현재내용을_실행취소용_클립보드에_저장();
                _텍스트변경사항자동저장불필요 = true;

                본문.SelectedText = "③";

                현재커서근방의키워드색상업데이트();

                _텍스트변경사항자동저장불필요 = false;
                현재내용을_실행취소용_클립보드에_저장();
            }
            else if (ABC.Focused) { ABC.SelectedText = "③"; }
            else if (보기1Text.Focused) { 보기1Text.SelectedText = "③"; }
            else if (보기2Text.Focused) { 보기2Text.SelectedText = "③"; }
            else if (보기3Text.Focused) { 보기3Text.SelectedText = "③"; }
            else if (보기4Text.Focused) { 보기4Text.SelectedText = "③"; }
            else if (보기5Text.Focused) { 보기5Text.SelectedText = "③"; }
            else if (주관식정답.Focused) { 주관식정답.SelectedText = "③"; }
            else if (해석.Focused) { 해석.SelectedText = "③"; }
            else if (힌트.Focused) { 힌트.SelectedText = "③"; }
            else if (중요어휘.Focused) { 중요어휘.SelectedText = "③"; }
        }
		private void 동그라미4번toolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (제목.Focused) { 제목.SelectedText = "④"; }
            else if (질문.Focused) { 질문.SelectedText = "④"; }
            else if (본문.Focused)
            {
                현재내용을_실행취소용_클립보드에_저장();
                _텍스트변경사항자동저장불필요 = true;

                본문.SelectedText = "④";

                현재커서근방의키워드색상업데이트();

                _텍스트변경사항자동저장불필요 = false;
                현재내용을_실행취소용_클립보드에_저장();
            }
            else if (ABC.Focused) { ABC.SelectedText = "④"; }
            else if (보기1Text.Focused) { 보기1Text.SelectedText = "④"; }
            else if (보기2Text.Focused) { 보기2Text.SelectedText = "④"; }
            else if (보기3Text.Focused) { 보기3Text.SelectedText = "④"; }
            else if (보기4Text.Focused) { 보기4Text.SelectedText = "④"; }
            else if (보기5Text.Focused) { 보기5Text.SelectedText = "④"; }
            else if (주관식정답.Focused) { 주관식정답.SelectedText = "④"; }
            else if (해석.Focused) { 해석.SelectedText = "④"; }
            else if (힌트.Focused) { 힌트.SelectedText = "④"; }
            else if (중요어휘.Focused) { 중요어휘.SelectedText = "④"; }
        }
		private void 동그라미5번toolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (제목.Focused) { 제목.SelectedText = "⑤"; }
            else if (질문.Focused) { 질문.SelectedText = "⑤"; }
            else if (본문.Focused)
            {
                현재내용을_실행취소용_클립보드에_저장();
                _텍스트변경사항자동저장불필요 = true;

                본문.SelectedText = "⑤";

                현재커서근방의키워드색상업데이트();

                _텍스트변경사항자동저장불필요 = false;
                현재내용을_실행취소용_클립보드에_저장();
            }
            else if (ABC.Focused) { ABC.SelectedText = "⑤"; }
            else if (보기1Text.Focused) { 보기1Text.SelectedText = "⑤"; }
            else if (보기2Text.Focused) { 보기2Text.SelectedText = "⑤"; }
            else if (보기3Text.Focused) { 보기3Text.SelectedText = "⑤"; }
            else if (보기4Text.Focused) { 보기4Text.SelectedText = "⑤"; }
            else if (보기5Text.Focused) { 보기5Text.SelectedText = "⑤"; }
            else if (주관식정답.Focused) { 주관식정답.SelectedText = "⑤"; }
            else if (해석.Focused) { 해석.SelectedText = "⑤"; }
            else if (힌트.Focused) { 힌트.SelectedText = "⑤"; }
            else if (중요어휘.Focused) { 중요어휘.SelectedText = "⑤"; }
        }
		#endregion
		#region 분류할 함수들
		private void 모의고사파일을Xml로ToolStripMenuItem_Click(object sender, EventArgs e)
		{
/*
			if (본문.Text.Contains("<Q>"))
			{
				List<string> Q들 = new List<string>();

				변환.문자열.분석안된TXT를Q들로(본문.Text, ref Q들);

				string Q골라내기 = "";
				string 지문과보기 = "";
				string 지문골라내기 = "";
				string 보기골라내기 = "";

				string 최종결과 = "";

				foreach (string 현재Q in Q들)
				{


					Q골라내기 = 현재Q.Substring(0, 현재Q.IndexOf("</Q>") + 4).Trim();
					지문과보기 = 현재Q.Substring(현재Q.IndexOf("</Q>") + 4).Trim();

					최종결과 += "<CAKE>\n";
					최종결과 += "\t" + Q골라내기 + "\n";

					if (지문과보기.Contains("(A) (B)"))
					{
						지문골라내기 = 지문과보기.Substring(0, 지문과보기.IndexOf("(A) (B)")).Trim();
						보기골라내기 = 지문과보기.Substring(지문과보기.IndexOf("(A) (B)")).Trim();

						if (지문골라내기 != "") 최종결과 += T태그매기기(지문골라내기, Q골라내기) + "\n";

						최종결과 += A태그원형복원.복원결과(보기골라내기, 지문골라내기) + "\n";
					}
					else if (지문과보기.Contains("①") && !지문과보기.Contains("① [") && !Q골라내기.Contains("흐름") && !Q골라내기.Contains("도표"))
					{
						지문골라내기 = 지문과보기.Substring(0, 지문과보기.IndexOf("①")).Trim();
						보기골라내기 = 지문과보기.Substring(지문과보기.IndexOf("①")).Trim();

						if (지문골라내기 != "") 최종결과 += T태그매기기(지문골라내기, Q골라내기) + "\n";

						최종결과 += A태그원형복원.복원결과(보기골라내기, 지문골라내기) + "\n";
					}
					else
					{
						지문골라내기 = 지문과보기.Trim();

						if (지문골라내기 != "") 최종결과 += T태그매기기(지문골라내기, Q골라내기) + "\n";
					}

					최종결과 += "</CAKE>\n";
				}

				본문.Text = 최종결과;

				전체화면하이라이트표시();
			}
			*/
		}
		private void 배경이미지폴더열기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("explorer.exe", " \"" + _IMG루트폴더.Replace("/", "\\") + "\"");
		}
		private void 현재폴더열기_클릭(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("explorer.exe", "\"" + _현재폴더 + "\"");
		}
		private void Form1_Activated(object sender, EventArgs e)
		{
            if(_시작할때열파일 == "0")
            {
                //MessageBox.Show("붙여넣습니다.");
                붙여넣기();

                _시작할때열파일 = "";
            }

            _오른쪽클릭메뉴.Hide();
            _해석오른쪽클릭메뉴.Hide();
		}

		private void 선택부분의문제표지제거ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			선택위치에바꿀말넣고키워드색상입히기(변환.문자열.문제표지제거(본문.SelectedText));
		}

		private void 정렬ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region 전처리
			화면업데이트중지();

			List<string> 개행문자로_구분된_문자열들 = new List<string>();
			변환.문자열.개행문자로_구분한_문자열들로(본문.Text, ref 개행문자로_구분된_문자열들);

			개행문자로_구분된_문자열들 = 개행문자로_구분된_문자열들.Distinct().ToList();
			#endregion
			개행문자로_구분된_문자열들.Sort();
			#region 후처리
			본문.Text = 변환.문자열들.문자열로(개행문자로_구분된_문자열들);

			전체화면하이라이트표시();

			화면업데이트재개();
			#endregion
		}
		private void 암호화_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지();


			본문.Text = 본문.Text.암호화();

			전체화면하이라이트표시();
			//전체화면하이라이트표시();


			화면업데이트재개();
			현재내용을_실행취소용_클립보드에_저장();

		}
		private void 복호화_클릭(object sender, EventArgs e)
		{
			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지();

			본문.Text = 변환.문자열.복호화(본문.Text);
			전체화면하이라이트표시();
			//전체화면하이라이트표시();

			화면업데이트재개();
			현재내용을_실행취소용_클립보드에_저장();
		}
		private void RTF보기_클릭(object sender, EventArgs e)
		{
			본문.Text = 본문.Rtf;
		}

		private string 문제고유번호매기기(string 문제전체)
		{
			#region 전처리

			현재내용을_실행취소용_클립보드에_저장();
			string 처리완료텍스트 = "";

			List<string> 문자열들 = new List<string>();

			변환.문자열.개행문자로_구분한_문자열들로(문제전체, ref 문자열들);

			DateTime 현재 = DateTime.Now;

			string 연도 = "";
			string 달 = "";
			string 일 = "";
			string 시 = "";
			string 분 = "";
			int 초 = 0;

			연도 = 현재.Year.ToString();
			달 = 현재.Month.ToString();
			일 = 현재.Day.ToString();
			시 = 현재.Hour.ToString();
			#endregion
			#region 년도
			if (연도 == "2015") 연도 = "A";
			else if (연도 == "2016") 연도 = "B";
			else if (연도 == "2017") 연도 = "C";
			else if (연도 == "2018") 연도 = "D";
			else if (연도 == "2019") 연도 = "E";
			else if (연도 == "2020") 연도 = "F";
			else if (연도 == "2021") 연도 = "G";
			else if (연도 == "2022") 연도 = "H";
			else if (연도 == "2023") 연도 = "I";
			else if (연도 == "2024") 연도 = "J";
			else if (연도 == "2025") 연도 = "K";
			else if (연도 == "2026") 연도 = "L";
			else if (연도 == "2027") 연도 = "M";
			else if (연도 == "2028") 연도 = "N";
			else if (연도 == "2029") 연도 = "O";
			else if (연도 == "2030") 연도 = "P";
			else if (연도 == "2031") 연도 = "Q";
			else if (연도 == "2032") 연도 = "R";
			else if (연도 == "2033") 연도 = "S";
			else if (연도 == "2034") 연도 = "T";
			else if (연도 == "2035") 연도 = "U";
			else if (연도 == "2036") 연도 = "V";
			else if (연도 == "2037") 연도 = "W";
			else if (연도 == "2038") 연도 = "X";
			else if (연도 == "2039") 연도 = "Y";
			else if (연도 == "2040") 연도 = "Z";
			#endregion
			#region 달
			if (달 == "1") 달 = "A";
			else if (달 == "2") 달 = "B";
			else if (달 == "3") 달 = "C";
			else if (달 == "4") 달 = "D";
			else if (달 == "5") 달 = "E";
			else if (달 == "6") 달 = "F";
			else if (달 == "7") 달 = "G";
			else if (달 == "8") 달 = "H";
			else if (달 == "9") 달 = "I";
			else if (달 == "10") 달 = "J";
			else if (달 == "11") 달 = "K";
			else if (달 == "12") 달 = "L";
			#endregion
			#region 일
			if (일 == "1") 일 = "1";
			else if (일 == "2") 일 = "2";
			else if (일 == "3") 일 = "3";
			else if (일 == "4") 일 = "4";
			else if (일 == "5") 일 = "5";
			else if (일 == "6") 일 = "6";
			else if (일 == "7") 일 = "7";
			else if (일 == "8") 일 = "8";
			else if (일 == "9") 일 = "9";
			else if (일 == "10") 일 = "A";
			else if (일 == "11") 일 = "B";
			else if (일 == "12") 일 = "C";
			else if (일 == "13") 일 = "D";
			else if (일 == "14") 일 = "E";
			else if (일 == "15") 일 = "F";
			else if (일 == "16") 일 = "G";
			else if (일 == "17") 일 = "H";
			else if (일 == "18") 일 = "I";
			else if (일 == "19") 일 = "J";
			else if (일 == "20") 일 = "K";
			else if (일 == "21") 일 = "L";
			else if (일 == "22") 일 = "M";
			else if (일 == "23") 일 = "N";
			else if (일 == "24") 일 = "O";
			else if (일 == "25") 일 = "P";
			else if (일 == "26") 일 = "Q";
			else if (일 == "27") 일 = "R";
			else if (일 == "28") 일 = "S";
			else if (일 == "29") 일 = "T";
			else if (일 == "30") 일 = "U";
			else if (일 == "31") 일 = "V";
			#endregion
			#region 시
			if (시 == "1") 시 = "1";
			else if (시 == "2") 시 = "2";
			else if (시 == "3") 시 = "3";
			else if (시 == "4") 시 = "4";
			else if (시 == "5") 시 = "5";
			else if (시 == "6") 시 = "6";
			else if (시 == "7") 시 = "7";
			else if (시 == "8") 시 = "8";
			else if (시 == "9") 시 = "9";
			else if (시 == "10") 시 = "A";
			else if (시 == "11") 시 = "B";
			else if (시 == "12") 시 = "C";
			else if (시 == "13") 시 = "D";
			else if (시 == "14") 시 = "E";
			else if (시 == "15") 시 = "F";
			else if (시 == "16") 시 = "G";
			else if (시 == "17") 시 = "H";
			else if (시 == "18") 시 = "I";
			else if (시 == "19") 시 = "J";
			else if (시 == "20") 시 = "K";
			else if (시 == "21") 시 = "L";
			else if (시 == "22") 시 = "M";
			else if (시 == "23") 시 = "N";
			else if (시 == "24") 시 = "O";
			#endregion
			분 = 현재.Minute.ToString("00");
			초 = 현재.Second;
			#region Q 나올 때마다 번호매기는 부분
			int 순차번호 = 0;
			foreach (string 현재문자열 in 문자열들)
			{
				if (현재문자열.Contains(" </Q>") && !현재문자열.Contains("QN")) // 이미 고유번호가 매겨져있으면 매기지 않습니다.
				{
					처리완료텍스트 += 현재문자열.Substring(0, 현재문자열.IndexOf(" </Q>"));

					처리완료텍스트 += " QN";
					#region 문제유형별코드넣기
					if (현재문자열.Contains("목적")) 처리완료텍스트 += "O";
					else if (현재문자열.Contains("재배열")) 처리완료텍스트 += "E";
					else if (현재문자열.Contains("어형")) 처리완료텍스트 += "E";
					else if (현재문자열.Contains("주제")) 처리완료텍스트 += "S";
					else if (현재문자열.Contains("빈칸")) 처리완료텍스트 += "B";
					else if (현재문자열.Contains("흐름")) 처리완료텍스트 += "F";
					else if (현재문자열.Contains("어법")) 처리완료텍스트 += "G";
					else if (현재문자열.Contains("어휘")) 처리완료텍스트 += "W";
					else
						처리완료텍스트 += "Z";
					#endregion
					처리완료텍스트 += 연도 + 달 + 일 + 시 + 분 + 초.ToString("00") + String.Format("{0:00}", 순차번호);
					#region 겹칠것을 대비해서 순차번호 매기기
					순차번호++;
					if (순차번호 == 100)
					{
						순차번호 = 0;
						초++;

						if (초 == 60) 초 = 0;
					}
					#endregion
					처리완료텍스트 += 현재문자열.Substring(현재문자열.IndexOf(" </Q>"));
					처리완료텍스트 += "\n";
				}
				else
				{
					처리완료텍스트 += 현재문자열 + "\n";
				}
			}
			#endregion
			현재내용을_실행취소용_클립보드에_저장();

			return 처리완료텍스트;
		}
		private void 문제고유번호매기기_클릭(object sender, EventArgs e)
		{
			//문제고유번호매기기();
		}

		private void 모두선택_클릭(object sender, EventArgs e)
		{
            if(제목.Focused)                제목.SelectAll();
            else if (질문.Focused)          질문.SelectAll();
            else if (본문.Focused)			본문.SelectAll();
			else if (해석.Focused)			해석.SelectAll();
			else if (힌트.Focused)			힌트.SelectAll();
            else if (중요어휘.Focused)      중요어휘.SelectAll();
        }
		private void 예상문제일괄삭제하기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			#region 전처리
			현재내용을_실행취소용_클립보드에_저장();
			화면업데이트중지(); _텍스트변경사항자동저장불필요 = true;

			string 파일헤더 = 변환.문자열.헤더추출(본문.Text);

			string 문제전체 = "<HEAD>\n";
			문제전체 += "\t" + 파일헤더 + "\n";
			문제전체 += "</HEAD>\n";

			List<string> CAKE들 = new List<string>();

			변환.문자열.CAKE들로(본문.Text, ref CAKE들);
			#endregion
			#region CAKE마다 루프를 돌림
			foreach (string 현재CAKE in CAKE들)
			{

				string 순수현재CAKE = "", 예상문제 = "";

				변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

				문제전체 += 순수현재CAKE.Replace("\n\n</CAKE>", "\n</CAKE>") + "\n";
			}
			#endregion
			#region 후처리
			본문.Text = 문제전체;

			전체화면하이라이트표시(); _텍스트변경사항자동저장불필요 = false; 현재내용을_실행취소용_클립보드에_저장(); 화면업데이트재개();
			#endregion

		}
		private void 문제순서바꾸기_클릭(object sender, EventArgs e)
		{

		}
		private void 문제번호순서대로다시매기기_클릭(object sender, EventArgs e)
		{

		}
		private void 해석하기_클릭(object sender, EventArgs e)
		{
			// 맨처음이 주어인 경우
			해석 해석 = new 해석();

			string 해석결과 = 해석.해석하기(본문.SelectedText.문제표지제거(), 본문.Text);
			
			/*
			// 해석결과가 없으면 텔레키네시스시스템 가동한다.
			if (해석결과 == "")
			{
				string 주소 = "https://translate.google.co.kr/#en/ko/" + 본문.SelectedText.문법문제표지제거().Replace(" ", "%20").Replace("\"", "%22").Replace(",", "%2C");
				텔레키네시스시스템 텔레 = new 텔레키네시스시스템(주소);
				텔레.StartPosition = FormStartPosition.Manual;
				텔레.Location = new Point(0, 0);
				텔레.ShowDialog();

			}
			*/
			if(해석결과.Trim() != "")
			{
				Clipboard.SetText(해석결과.Trim());
			
				해석결과창 내해석결과 = new 해석결과창(해석결과);
				내해석결과.ShowDialog();
			}


		}

		// 함장님, 우리의 기체는 실전 응용단계에 이르렀습니다. 결단만 내리시면!!!
		private void 선택부분신경망해석하기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string 주소 = "https://translate.google.co.kr/#en/ko/" + 본문.SelectedText.문법문제표지제거().Replace(" ", "%20").Replace("\"", "%22").Replace(",", "%2C");
			텔레키네시스시스템 텔레 = new 텔레키네시스시스템(주소);
			텔레.StartPosition = FormStartPosition.Manual;
			텔레.Location = new Point(0, 0);
			텔레.ShowDialog();
		}

		private string 숫자읽기전처리(string 문자열)
		{
			bool 숫자시작 = false;

			string 처리된문자열 = "";
			string 아라비아숫자문자열 = "";

			string 현재문자열 = "";
			for(int i = 0; i < 문자열.Length; i++)
			{
				현재문자열 = 문자열.Substring(i, 1);
				if(한글자짜리숫자인지여부(현재문자열) && !숫자시작)
				{
					숫자시작 = true;
					아라비아숫자문자열 += 현재문자열;
				}
				else if(한글자짜리숫자인지여부(현재문자열) && 숫자시작)
				{
					아라비아숫자문자열 += 현재문자열;
				}
				else if(!한글자짜리숫자인지여부(현재문자열) && !숫자시작)
				{
					처리된문자열 += 현재문자열;
				}
				else if (!한글자짜리숫자인지여부(현재문자열) && 숫자시작)
				{
					숫자시작 = false;
					처리된문자열 += 숫자만읽는부분(아라비아숫자문자열);
					처리된문자열 += 현재문자열;

					아라비아숫자문자열 = "";
				}

			}

			if(아라비아숫자문자열 != "")
				처리된문자열 += 숫자만읽는부분(아라비아숫자문자열);

			return 처리된문자열;

			//return 문자열;
		}
		private string 숫자만읽는부분(string 숫자문자열)
		{
			if (숫자문자열 == "0")
				return "영";

			bool 숫자0인지_여부 = false;
			string 한글숫자로바꾼것 = "";

			for (int i = 0; i < 숫자문자열.Length; i++)
			{
				if(숫자문자열[i] == '1' ){
					if (i == 숫자문자열.Length - 1 || i == 숫자문자열.Length - 9 || i == 숫자문자열.Length - 13)
						한글숫자로바꾼것 += "일";
				}
				else if (숫자문자열[i] == '2'){ 한글숫자로바꾼것 += "이"; }
				else if (숫자문자열[i] == '3') { 한글숫자로바꾼것 += "삼"; }
				else if (숫자문자열[i] == '4') { 한글숫자로바꾼것 += "사"; }
				else if (숫자문자열[i] == '5') { 한글숫자로바꾼것 += "오"; }
				else if (숫자문자열[i] == '6') { 한글숫자로바꾼것 += "육"; }
				else if (숫자문자열[i] == '7') { 한글숫자로바꾼것 += "칠"; }
				else if (숫자문자열[i] == '8') { 한글숫자로바꾼것 += "팔"; }
				else if (숫자문자열[i] == '9') { 한글숫자로바꾼것 += "구"; }
				else if (숫자문자열[i] == '0') { 
					숫자0인지_여부 = true;
                }

				if (i == 숫자문자열.Length - 1) {; }
				else if (i == 숫자문자열.Length - 2) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "십"; } }
				else if (i == 숫자문자열.Length - 3) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "백"; } }
				else if (i == 숫자문자열.Length - 4) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "천"; } }
				else if (i == 숫자문자열.Length - 5)
				{
					if(숫자문자열.TTS용_자리에_숫자있는지확인(4) ||
					숫자문자열.TTS용_자리에_숫자있는지확인(5) ||
					숫자문자열.TTS용_자리에_숫자있는지확인(6) ||
					숫자문자열.TTS용_자리에_숫자있는지확인(7))
						한글숫자로바꾼것 += "만";
				}
				else if (i == 숫자문자열.Length - 6) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "십"; } }
				else if (i == 숫자문자열.Length - 7) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "백"; } }
				else if (i == 숫자문자열.Length - 8) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "천"; } }
				else if (i == 숫자문자열.Length - 9) {

					if (숫자문자열.TTS용_자리에_숫자있는지확인(8) ||
					숫자문자열.TTS용_자리에_숫자있는지확인(9) ||
					숫자문자열.TTS용_자리에_숫자있는지확인(10) ||
					숫자문자열.TTS용_자리에_숫자있는지확인(11))
						한글숫자로바꾼것 += "억"; 
				 }
				else if (i == 숫자문자열.Length - 10) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "십"; } }
				else if (i == 숫자문자열.Length - 11) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "백"; } }
				else if (i == 숫자문자열.Length - 12) { if (!숫자0인지_여부) { 한글숫자로바꾼것 += "천"; } }

				숫자0인지_여부 = false;
			}

			return 한글숫자로바꾼것;
		}
		private bool 한글자짜리숫자인지여부(string 문자열)
		{
			if (문자열 == "0") return true;
			else if (문자열 == "1") return true;
			else if (문자열 == "2") return true;
			else if (문자열 == "3") return true;
			else if (문자열 == "4") return true;
			else if (문자열 == "5") return true;
			else if (문자열 == "6") return true;
			else if (문자열 == "7") return true;
			else if (문자열 == "8") return true;
			else if (문자열 == "9") return true;

			return false;
		}
		private void 읽어주기_클릭(object sender, EventArgs e)
		{
			#region 변수
			List<string> 플레이리스트 = new List<string>();
            string 플레이리스트_결과보기;
			#endregion
			#region 1. 우선 wav 파일을 모두 로딩한다.
			if (!System.IO.Directory.Exists("C:\\TTS\\")) return;
			System.IO.DirectoryInfo 폴더정보 = new System.IO.DirectoryInfo("C:\\TTS\\");

			List<string> 파일이름들 = new List<string>();

			foreach (var 폴더내파일들 in 폴더정보.GetFiles())
			{
				파일이름들.Add(폴더내파일들.Name.Substring(0, 폴더내파일들.Name.Length - 4));
			}
			#endregion

			// 2. 최적의 wav를 검색한다.
			// 2.1 리스트 중에 현재의 조합을 모두 가지고 있는 것으로 한정한다.
			//  (속도를 빠르게 하기 위해서, 초성에 따라서 리스트를 나눈다.)
			//	가장 긴 것을 내보낸다.
			string 선택한_지문 = "";

            if (제목.Focused)               선택한_지문 = 제목.SelectedText;
            else if (질문.Focused)          선택한_지문 = 질문.SelectedText;
            else if (본문.Focused)          선택한_지문 = 본문.SelectedText;
            else if( ABC.Focused)           선택한_지문 = ABC.SelectedText;
            else if (보기1Text.Focused) 선택한_지문 = 보기1Text.SelectedText;
            else if (보기2Text.Focused) 선택한_지문 = 보기2Text.SelectedText;
            else if (보기3Text.Focused) 선택한_지문 = 보기3Text.SelectedText;
            else if (보기4Text.Focused) 선택한_지문 = 보기4Text.SelectedText;
            else if (보기5Text.Focused) 선택한_지문 = 보기5Text.SelectedText;

            else if (해석.Focused)            선택한_지문 = 해석.SelectedText;
            else if (힌트.Focused)			선택한_지문 = 힌트.SelectedText;
            

/*
			선택한_지문 = 선택한_지문.Replace(" 2000년", " 이천 년");
			선택한_지문 = 선택한_지문.Replace(" 3번", " 삼번");
			선택한_지문 = 선택한_지문.Replace(" 4번", " 사번");
			선택한_지문 = 선택한_지문.Replace(" 고3 ", " 고삼 ");
			선택한_지문 = 선택한_지문.Replace(" 2015년", " 이천십오 년");
			선택한_지문 = 선택한_지문.Replace(" 1학기", " 일학기");
			*/

			선택한_지문 = 선택한_지문.Replace(",", "");
			선택한_지문 = 선택한_지문.Replace(".", "");
			선택한_지문 = 선택한_지문.Replace("?", "");

			선택한_지문 = 숫자읽기전처리(선택한_지문);
			MessageBox.Show(선택한_지문);


			int i = 0;
			bool 바로전에_플러스로_끝났는지 = false;
            플레이리스트_결과보기 = "";


            while (i < 선택한_지문.Length)
			{
				float 현재최고길이 = 0.0f;
				string 현재최고길이지문 = "";
				bool 현재최고길이_왼쪽_플러스 = false;
				bool 현재최고길이_오른쪽_플러스 = false;
				bool 왼쪽_플러스 = false;
				bool 오른쪽_플러스 = false;

				for (int j = 0; j < 파일이름들.Count(); j++)
				{

					if (변환.문자열.Left(파일이름들[j], 1) == "+") 왼쪽_플러스 = true;
					else 왼쪽_플러스 = false;

					if (변환.문자열.Right(파일이름들[j], 1) == "+") 오른쪽_플러스 = true;
					else 오른쪽_플러스 = false;

					// i가 0인데 왜 바로전에 플러스로 끝난는지를 확인하느냐 하면,
					// "동+"와 같은 경우가 있기 때문이다. 이 경우 모든 처리를 끝내고 2를 빼준다.
					// 그렇다면 인덱스는 0이 된다.
					if (((i == 0) && (왼쪽_플러스 == false) && !바로전에_플러스로_끝났는지) || (바로전에_플러스로_끝났는지 && 왼쪽_플러스) || (!바로전에_플러스로_끝났는지 && !왼쪽_플러스))
					{
						if (파일이름들[j].Replace("+", "").Length + i <= 선택한_지문.Length)
						{
							if (선택한_지문.Substring(i, 파일이름들[j].Replace("+", "").Length) == 파일이름들[j].Replace("+", ""))
							{
								// 만약에 +로 끝나지 않고, 길이가 딱떨어지면, 그게 정답이다. 그러니까 "우차차+"가 있고 "우차차"가 있는데, 원문이 "이런 우차차"이면, "우차차"가 마지막이다.
								if(i + 파일이름들[j].Replace("+", "").Length == 선택한_지문.Length && !파일이름들[j].EndsWith("+"))
								{
									현재최고길이지문 = 파일이름들[j];
									현재최고길이 = 10000.0f;

									현재최고길이_왼쪽_플러스 = 왼쪽_플러스;
									현재최고길이_오른쪽_플러스 = 오른쪽_플러스;
								}
								else if (파일이름들[j].TTS용_길이확인() > 현재최고길이)
								{
									현재최고길이지문 = 파일이름들[j];
									현재최고길이 = 현재최고길이지문.TTS용_길이확인();

									현재최고길이_왼쪽_플러스 = 왼쪽_플러스;
									현재최고길이_오른쪽_플러스 = 오른쪽_플러스;
								}
							}
						}
					}


				}
				if (현재최고길이지문 != "")
				{
					플레이리스트.Add(String.Format("C:\\TTS\\{0}.wav", 현재최고길이지문));
                    플레이리스트_결과보기 += 현재최고길이지문 + "/";

					i += 현재최고길이지문.Length;
					if (현재최고길이_오른쪽_플러스) { i -= 2; 바로전에_플러스로_끝났는지 = true; }
					else 바로전에_플러스로_끝났는지 = false;
					if (현재최고길이_왼쪽_플러스) i -= 1;
				}
				else
					i++;
			}
			// 3. List에 wav를 쟁여 놓는다

			// 4. 틀어준다.


			List<string> 어절들 = new List<string>();
			변환.문자열.어절들로(선택한_지문, ref 어절들);

			WaveIO 웨이브합치기용클래스 = new WaveIO();
			웨이브합치기용클래스.웨이브파일합치기(ref 플레이리스트, "C:\\TTS\\temp.wav");

			SoundPlayer wp1 = new SoundPlayer("C:\\TTS\\temp.wav");

            MessageBox.Show(플레이리스트_결과보기);
            wp1.Play();

		}
		private void 본문해석문장숫자비교하기_Click(object sender, EventArgs e)
		{
			int 선택시작위치 = 0, 선택끝위치 = 0;
			string 현재CAKE = 현재지문의CAKE내용추출(ref 선택시작위치, ref 선택끝위치);

			if (선택끝위치 > 선택시작위치 && 선택끝위치 != 0)
				본문.Select(선택시작위치, 선택끝위치 - 선택시작위치);

			string 순수현재CAKE = "", 예상문제 = "";

			변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

			string 고유번호제거된예상문제 = 변환.문자열.예상문제들에서고유번호만제거(예상문제);


			string t = 강력하지만무거운변환.문자열.T태그원형복원(순수현재CAKE);
			string tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(순수현재CAKE));
			string t_문법표지있는 = 강력하지만무거운변환.문자열.T태그원형문법표지남기고복원(순수현재CAKE);

			문제.본문_해석_문장숫자확인(t, tr);
		}
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			//탭.SelectedIndex = 1;
			//MainTabControl.SelectedIndex = 0;
			//MessageBox.Show(treeView1.SelectedNode.Index.ToString());
			//MessageBox.Show(treeView1.SelectedNode.Level.ToString());
			
			// 그래픽 탭에서의 배경 강조할 내용을 초기화합니다.
			_현재i = -1;
			_현재j = -1;

			_y그림위치 = 0; // 스크롤 위치를 초기화합니다.

			if (treeView1.SelectedNode.Level == 1)
			{
				_CAKE_인덱스 = treeView1.SelectedNode.Index;
				_SUGAR_인덱스 = -1;

				화면업데이트중지();
				본문.SelectAll();

				string 케이크전체 = _CAKE들[treeView1.SelectedNode.Index];
                케이크전체 = 케이크전체.Replace("’", "\'");     // 이런 건 얼른 얼른 좀 바꾸자.

                string 순수현재CAKE = "", 예상문제 = "";

				변환.문자열.파일내용과_예상문제_분리(케이크전체, ref 순수현재CAKE, ref 예상문제);

				케이크표시하기(순수현재CAKE);
				//선택위치에바꿀말넣고키워드색상입히기(순수현재CAKE);

				int nPos = 0;
				uint wParam = (uint)ScrollBarCommands.SB_THUMBPOSITION | (uint)nPos;
				SendMessage(본문.Handle, (int)스크롤업데이트메시지.WM_VSCROLL, new IntPtr(wParam), new IntPtr(0));


				화면업데이트재개();

				if(_현재_선택한_탭 == "그래픽탭")
				{
					_JPG경로 = 동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 
													보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
													ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
					키워드.Text = _JPG경로;
					전체화면다시그리기();
				}
			}

			if (treeView1.SelectedNode.Level == 2)
			{
				_CAKE_인덱스 = treeView1.SelectedNode.Parent.Index;
				_SUGAR_인덱스 = treeView1.SelectedNode.Index;

				화면업데이트중지();

				string 상위노드케이크 = _CAKE들[treeView1.SelectedNode.Parent.Index];
                상위노드케이크 = 상위노드케이크.Replace("’", "\'");     // 이런 건 얼른 얼른 좀 바꾸자.

                string 순수현재CAKE = "", 예상문제 = "";

				변환.문자열.파일내용과_예상문제_분리(상위노드케이크, ref 순수현재CAKE, ref 예상문제);

				List<string> SUGAR들 = new List<string>();
				변환.문자열.CAKE들로(예상문제, ref SUGAR들);

				본문.SelectAll();
				케이크표시하기(SUGAR들[treeView1.SelectedNode.Index]);

//				MessageBox.Show(SUGAR들[treeView1.SelectedNode.Index]);

				//선택위치에바꿀말넣고키워드색상입히기(SUGAR들[treeView1.SelectedNode.Index]);

				화면업데이트재개();

				if (_현재_선택한_탭 == "그래픽탭")
				{
					_JPG경로 = 동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 
													보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
													ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
					키워드.Text = _JPG경로;
					전체화면다시그리기();
				}
			}
		}
		private void 구문분석ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
		private void 보기1_CheckedChanged(object sender, EventArgs e)
		{

		}
        private void 선택한항목뒤에새로운항목만들기()
        {
            if (treeView1.Focused)
            {
                메모리_저장CAKE();

                if (treeView1.SelectedNode.Level == 0)
                {
                    _CAKE_인덱스 = 0;

                    _CAKE들.Insert(_CAKE_인덱스, "");

                    string Tree제목 = "항목 " + (_CAKE_인덱스 + 1).ToString() + "번";
                    TreeNode CAKE노드 = new TreeNode(Tree제목);

                    treeView1.Nodes[0].Nodes.Insert(_CAKE_인덱스, CAKE노드);
                    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스];
                }
                else if (treeView1.SelectedNode.Level == 1)
                {

                    _CAKE_인덱스 = treeView1.SelectedNode.Index;
                    if (_CAKE_인덱스 == _CAKE들.Count() - 1)
                    {
                        _CAKE들.Add("");
                        _CAKE_인덱스++;

                        string Tree제목 = "항목 " + (_CAKE_인덱스 + 1).ToString() + "번";

                        TreeNode CAKE노드 = new TreeNode(Tree제목);

                        treeView1.Nodes[0].Nodes.Add(CAKE노드);
                        treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스];
                        //treeView1.Nodes[0].Nodes[_CAKE_인덱스].Selected
                        // treeView1.Nodes[_CAKE_인덱스]
                    }
                    else if (_CAKE_인덱스 >= 0)
                    {
                        _CAKE_인덱스++;

                        _CAKE들.Insert(_CAKE_인덱스, "");

                        string Tree제목 = "항목 " + (_CAKE_인덱스 + 1).ToString() + "번";
                        TreeNode CAKE노드 = new TreeNode(Tree제목);

                        treeView1.Nodes[0].Nodes.Insert(_CAKE_인덱스, CAKE노드);
                        treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스];
                    }
                }
                else if (treeView1.SelectedNode.Level == 2)
                {


                    _CAKE_인덱스 = treeView1.SelectedNode.Parent.Index;
                    _SUGAR_인덱스 = treeView1.SelectedNode.Index;

                    string 상위노드케이크 = _CAKE들[treeView1.SelectedNode.Parent.Index];

                    string 순수현재CAKE = "", 예상문제 = "";

                    변환.문자열.파일내용과_예상문제_분리(상위노드케이크, ref 순수현재CAKE, ref 예상문제);

                    List<string> SUGAR들 = new List<string>();
                    변환.문자열.CAKE들로(예상문제, ref SUGAR들);

                    _SUGAR_인덱스++;

                    SUGAR들.Insert(_SUGAR_인덱스, "<CAKE>\r\n</CAKE>\r\n");



                    string 바꿀CAKE = 순수현재CAKE.Replace("</CAKE>", "").Trim();
                    바꿀CAKE += "\r\n<SUGAR>\r\n";

                    for (int i = 0; i < SUGAR들.Count(); i++)
                        바꿀CAKE += SUGAR들[i] + "\r\n";

                    바꿀CAKE += "\r\n</SUGAR>" + "\r\n</CAKE>\r\n";

                    _CAKE들[treeView1.SelectedNode.Parent.Index] = 바꿀CAKE;

                    string Tree제목 = "항목 " + (_SUGAR_인덱스 + 1).ToString() + "번";
                    TreeNode CAKE노드 = new TreeNode(Tree제목);
                    treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes.Insert(_SUGAR_인덱스, CAKE노드);

                    treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes[_SUGAR_인덱스];

                }

                케이크표시하기("");
            }
        }
        private void 선택한항목뒤에새로운항목만들기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            선택한항목뒤에새로운항목만들기();
        }
		private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			메모리_저장CAKE();
		}
		private void 암호화하여클립보드에저장하기_Click(object sender, EventArgs e)
		{
			string 저장할문자열 = "";
			저장할문자열 += 헤더정보();
			for (int i = 0; i < _CAKE들.Count; i++) 저장할문자열 += _CAKE들[i] + "\r\n";

			저장할문자열 = 저장할문자열.Replace("</CAKE>\r\n\r\n", "</CAKE>\r\n");

			Clipboard.SetText(저장할문자열.암호화().Replace("\n","\r\n").Replace("\r\r","\r"));

			MessageBox.Show("클립보드에 암호화 하여 저장하였습니다.");
		}
		private void 좌측영역한글영문문장숫자비교하기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.Focused)
			{
				if (treeView1.SelectedNode.Level == 1)
				{
					_CAKE_인덱스 = treeView1.SelectedNode.Index;

					string 현재CAKE = _CAKE들[_CAKE_인덱스];

					string 순수현재CAKE = "", 예상문제 = "";

					변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

					string t = 강력하지만무거운변환.문자열.T태그원형복원(순수현재CAKE);

					string tr = "";
					if (순수현재CAKE.Contains("<해석>"))
						tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.해석추출(순수현재CAKE));
					else
						tr = 강력하지만무거운변환.문자열.TR태그원형복원(변환.문자열.TR태그내용(순수현재CAKE));

                    문제.본문_해석_문장숫자_무조건_확인(t, tr);
				}
			}
		}
		private void 왼쪽에선택한항목의하위항목으로새로운항목만들기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.Focused)
			{
				if (treeView1.SelectedNode.Level == 0)
				{
					_CAKE_인덱스 = 0;

					_CAKE들.Insert(_CAKE_인덱스, "");

					string Tree제목 = "항목 " + (_CAKE_인덱스 + 1).ToString() + "번";
					TreeNode CAKE노드 = new TreeNode(Tree제목);

					treeView1.Nodes[0].Nodes.Insert(_CAKE_인덱스, CAKE노드);
					treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스];
				}
				else if (treeView1.SelectedNode.Level == 1)
				{
					_CAKE_인덱스 = treeView1.SelectedNode.Index;

					string 현재CAKE = _CAKE들[_CAKE_인덱스];

					string 순수현재CAKE = "", 예상문제 = "";

					변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

					string 고유번호제거된예상문제 = 변환.문자열.예상문제들에서고유번호만제거(예상문제);


					if (예상문제 == "")
						_CAKE들[_CAKE_인덱스] = 순수현재CAKE.Replace("</CAKE>", "").Trim() + "\r\n<SUGAR>\r\n" + "\r\n<CAKE>\r\n" + "\r\n</CAKE>\r\n" + "</SUGAR>" + "\r\n</CAKE>\r\n";
					else
					{
						_CAKE들[_CAKE_인덱스] = 순수현재CAKE.Replace("</CAKE>", "").Trim() + "\r\n<SUGAR>\r\n" + "\r\n<CAKE>\r\n" + "\r\n</CAKE>\r\n" + 예상문제 + "\r\n</SUGAR>" + "\r\n</CAKE>\r\n";
					}

					TreeNode CAKE노드 = new TreeNode("항목 1번");
					treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes.Insert(0, CAKE노드);
					treeView1.SelectedNode = treeView1.Nodes[0].Nodes[_CAKE_인덱스].Nodes[0];
				}
				else if (treeView1.SelectedNode.Level == 2)
				{
					MessageBox.Show("문제에 대한 변형 문제에 대한 변형 문제는 만들지 않습니다. 그냥 변형문제의 숫자를 늘리는 것을 권유합니다.");
				}
			}
		}
		private void 같지않음기호입력_Click(object sender, EventArgs e)
		{
			if (제목.Focused)
				제목.SelectedText = "≠";
			else if (질문.Focused)
				질문.SelectedText = "≠";
			else if (본문.Focused)
				본문.SelectedText = "≠";
			else if (ABC.Focused)
				ABC.SelectedText = "≠";
			else if (보기1Text.Focused)
				보기1Text.SelectedText = "≠";
			else if (보기2Text.Focused)
				보기2Text.SelectedText = "≠";
			else if (보기3Text.Focused)
				보기3Text.SelectedText = "≠";
			else if (보기4Text.Focused)
				보기4Text.SelectedText = "≠";
			else if (보기5Text.Focused)
				보기5Text.SelectedText = "≠";
			else if (주관식정답.Focused)
				주관식정답.SelectedText = "≠";

			else if (힌트.Focused)
				힌트.SelectedText = "≠";
			else if (해석.Focused)
				해석.SelectedText = "≠";
			else if (중요어휘.Focused)
				중요어휘.SelectedText = "≠";

		}
		private void 중요ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			유틸_자동입력_예상문제유형입력_선택위치에주어진문제유형입력하기("중요");
		}
        private void 형식구분3000제용보기입력ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            보기1Text.Text = "1형식";
            보기2Text.Text = "2형식";
            보기3Text.Text = "3형식";
            보기4Text.Text = "4형식";
            보기5Text.Text = "5형식";

        }
        private void 이전질문붙여넣기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 1 && _CAKE_인덱스 != 0)
            {
                //string 이전질문 = 변환.문자열.Q태그내용(_CAKE들[_CAKE_인덱스 - 1]);
				if(질문.Focused)
				{
					if (String.IsNullOrEmpty(질문.Text))
					{
						질문.Text = 변환.문자열.Q태그내용(_CAKE들[_CAKE_인덱스 - 1]).번호늘리기();
					}
					else
					{
						if (MessageBox.Show("Ctrl + Alt + V 버튼을 누르면, 이전 문항의 질문을 복사해서 그대로 사용합니다. 현재 질문란이 비어 있지 않은데, 덮어 쓰시겠습니까? 무슨 말인지 모르겠으면 '아니오'를 선택하면 별 일 안 일어납니다.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
						{
							질문.Text = 변환.문자열.Q태그내용(_CAKE들[_CAKE_인덱스 - 1]).번호늘리기();
						}
					}
				}
				else if(본문.Focused)
				{
					if(String.IsNullOrEmpty(본문.Text))
					{
						본문.Text = 변환.문자열.T태그내용(_CAKE들[_CAKE_인덱스 - 1]);
					}
					else
					{
						if (MessageBox.Show("Ctrl + Alt + V 버튼을 누르면, 이전 문항의 본문을 복사해서 그대로 사용합니다. 현재 본문란이 비어 있지 않은데, 덮어 쓰시겠습니까? 무슨 말인지 모르겠으면 '아니오'를 선택하면 별 일 안 일어납니다.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
						{
							본문.Text = 변환.문자열.T태그내용(_CAKE들[_CAKE_인덱스 - 1]);
						}
					}
				}
				else if(해석.Focused)
				{
					if(String.IsNullOrEmpty(해석.Text))
					{
						해석.Text = 변환.문자열.해석추출(_CAKE들[_CAKE_인덱스 - 1]);
					}
					else
					{
						if (MessageBox.Show("Ctrl + Alt + V 버튼을 누르면, 이전 문항의 해석을 복사해서 그대로 사용합니다. 현재 해석란이 비어 있지 않은데, 덮어 쓰시겠습니까? 무슨 말인지 모르겠으면 '아니오'를 선택하면 별 일 안 일어납니다.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
						{
							해석.Text = 변환.문자열.해석추출(_CAKE들[_CAKE_인덱스 - 1]);
						}
					}
				}
            }
        }
        private void 문제에들어갈Db파일폴더위치열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
			//MessageBox.Show(_문제DB루트폴더);
            System.Diagnostics.Process.Start("explorer.exe", "\"" + _DB루트.Replace("/", "\\") + "\"");
        }
		private void 보기1Text_MouseMove(object sender, MouseEventArgs e)
		{
			현재마우스위치 = new Point(e.X, e.Y);
			if (이전마우스위치 != 현재마우스위치)
			{

				string 현재어절 = GetWord(보기1Text.Text, 보기1Text.GetCharIndexFromPosition(e.Location)).Trim().불필요제거();

                while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
                while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

                string 표제어 = "";

				사전의미.Text = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);
				사전표제어.Text = 표제어;

				string 사전발음기호s = _검색.영한발음기호사전(표제어);

				if (사전발음기호s != "")
					사전발음기호.Text = "[" + 사전발음기호s + "]";
				else
					사전발음기호.Text = "";


				이전마우스위치 = 현재마우스위치;
			}
		}
		private void 보기2Text_MouseMove(object sender, MouseEventArgs e)
		{
			현재마우스위치 = new Point(e.X, e.Y);
			if (이전마우스위치 != 현재마우스위치)
			{

				string 현재어절 = GetWord(보기2Text.Text, 보기2Text.GetCharIndexFromPosition(e.Location)).Trim().불필요제거();

                while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
                while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

                string 표제어 = "";

				사전의미.Text = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);
				사전표제어.Text = 표제어;

				string 사전발음기호s = _검색.영한발음기호사전(표제어);

				if (사전발음기호s != "")
					사전발음기호.Text = "[" + 사전발음기호s + "]";
				else
					사전발음기호.Text = "";

				이전마우스위치 = 현재마우스위치;
			}
		}
		private void 보기3Text_MouseMove(object sender, MouseEventArgs e)
		{
			현재마우스위치 = new Point(e.X, e.Y);
			if (이전마우스위치 != 현재마우스위치)
			{

				string 현재어절 = GetWord(보기3Text.Text, 보기3Text.GetCharIndexFromPosition(e.Location)).Trim().불필요제거();

                while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
                while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

                string 표제어 = "";

				사전의미.Text = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);
				사전표제어.Text = 표제어;

				string 사전발음기호s = _검색.영한발음기호사전(표제어);

				if (사전발음기호s != "")
					사전발음기호.Text = "[" + 사전발음기호s + "]";
				else
					사전발음기호.Text = "";


				이전마우스위치 = 현재마우스위치;
			}
		}
		private void 보기4Text_MouseMove(object sender, MouseEventArgs e)
		{
			현재마우스위치 = new Point(e.X, e.Y);
			if (이전마우스위치 != 현재마우스위치)
			{

				string 현재어절 = GetWord(보기4Text.Text, 보기4Text.GetCharIndexFromPosition(e.Location)).Trim().불필요제거();

                while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
                while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

                string 표제어 = "";

				사전의미.Text = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);
				사전표제어.Text = 표제어;

				string 사전발음기호s = _검색.영한발음기호사전(표제어);

				if (사전발음기호s != "")
					사전발음기호.Text = "[" + 사전발음기호s + "]";
				else
					사전발음기호.Text = "";


				이전마우스위치 = 현재마우스위치;
			}
		}
		private void 보기5Text_MouseMove(object sender, MouseEventArgs e)
		{
			현재마우스위치 = new Point(e.X, e.Y);
			if (이전마우스위치 != 현재마우스위치)
			{

				string 현재어절 = GetWord(보기5Text.Text, 보기5Text.GetCharIndexFromPosition(e.Location)).Trim().불필요제거();

                while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
                while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

                string 표제어 = "";

				사전의미.Text = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);
				사전표제어.Text = 표제어;

				string 사전발음기호s = _검색.영한발음기호사전(표제어);

				if (사전발음기호s != "")
					사전발음기호.Text = "[" + 사전발음기호s + "]";
				else
					사전발음기호.Text = "";

				이전마우스위치 = 현재마우스위치;
			}
		}
		private void 현재문제복사하기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Clipboard.SetText("<현재문제복사>" + 현재양식내용() + "</현재문제복사>");

			
		}
		private void 인쇄버전Txt로만들기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.SaveFileDialog 인쇄경로다이얼로그 = new System.Windows.Forms.SaveFileDialog();

			인쇄경로다이얼로그.Filter = "XML 파일(*.xml)|*.xml|txt 파일(*.txt)|*.txt|smi 파일(*.smi)|*.smi";
			인쇄경로다이얼로그.Title = "인쇄할 경로 설정";

			if (인쇄경로다이얼로그.ShowDialog() == DialogResult.OK)
			{
				string 인쇄경로 = 인쇄경로다이얼로그.FileName;

				string 예상문제모음 = "";
				//_현재폴더 = Path.GetDirectoryName(_파일경로);

				for (int i = 0; i < _CAKE들.Count(); i++)
				{
					string 현재CAKE = _CAKE들[i];

					string 순수현재CAKE = "", 예상문제 = "";

					변환.문자열.파일내용과_예상문제_분리(현재CAKE, ref 순수현재CAKE, ref 예상문제);

					예상문제모음 += 예상문제;


				}

				List<string> 예상문제들 = new List<string>();

				예상문제모음 = 변환.문자열.문제번호다시매기기(예상문제모음);

				변환.문자열.CAKE들로(예상문제모음, ref 예상문제들);

				string 전체문제 = "";
				string 전체정답 = "";

				for(int i = 0 ; i < 예상문제들.Count; i++)
				{
					string 현재예상문제 = 예상문제들[i];

					if(!String.IsNullOrWhiteSpace(현재예상문제))
					{
						현재예상문제 = 현재예상문제.Replace("<TBAR></TBAR>", "\r\n-----------------------------------------------------------------------------------------------------------------\r\n");
						현재예상문제 = 현재예상문제.Replace("\t", " ");

						전체문제 += 변환.문자열.Q태그내용(현재예상문제) + "\r\n\r\n";
						전체문제 += 변환.문자열.T태그내용(현재예상문제) + "\r\n\r\n";

						if(!String.IsNullOrWhiteSpace(변환.문자열.A1태그내용(현재예상문제)))
						{ 
							전체문제 += "① " + 변환.문자열.A1태그내용(현재예상문제) + "\r\n";
							전체문제 += "② " + 변환.문자열.A2태그내용(현재예상문제) + "\r\n";
							전체문제 += "③ " + 변환.문자열.A3태그내용(현재예상문제) + "\r\n";
							전체문제 += "④ " + 변환.문자열.A4태그내용(현재예상문제) + "\r\n";
							전체문제 += "⑤ " + 변환.문자열.A5태그내용(현재예상문제) + "\r\n\r\n";
						}

						//----------
						전체정답 += 변환.문자열.Q태그내용(현재예상문제).Q에서번호추출() + ". ";
						전체정답 += 변환.문자열.정답인쇄용추출(현재예상문제) + "\r\n";
					}
				}

				변환.문자열.Ansi파일로(전체문제 + 전체정답, 인쇄경로);
			}
		}
		private void 다음글의제목으로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "다음 글의 제목으로 가장 적절한 것은?";
		}
		private void 다음글의요지로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "다음 글의 요지로 가장 적절한 것은?";
		}
		private void 다음글에서필자가주장하는바로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "다음 글에서 필자가 주장하는 바로 가장 적절한 것은?";
		}
		private void 다음도표의내용과일치하지않는것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "다음 도표의 내용과 일치하지 않는 것은?";
		}
		private void 밑줄친부분이가리키는대상이나머지넷과다른것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "밑줄 친 부분이 가리키는 대상이 나머지 넷과 다른 것은?";
		}
		private void 다음글의빈칸에들어갈말로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "다음 글의 빈칸에 들어갈 말로 가장 적절한 것은?";
		}
		private void 다음글의빈칸AB에들어갈말로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "다음 글의 빈칸 (A), (B)에 들어갈 말로 가장 적절한 것은?";
		}
		private void ABC의각네모안에서문맥에맞는낱말로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "(A), (B), (C)의 각 네모 안에서 문맥에 맞는 낱말로 가장 적절한 것은?";
		}
		private void 다음글의상황에나타난분위기로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			질문.Text += "다음 글의 상황에 나타난 분위기로 가장 적절한 것은?";
		}
		private void 다음글에드러난필자의심경으로가장적절한것은ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			변환.문자열.Q태그내용(_CAKE들[_CAKE_인덱스 - 1]).번호늘리기();
		}

		#endregion
		#region 그래픽화면
		#region 그래픽화면_화면세팅
		private void 탭_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (탭.SelectedTab == 탭.TabPages["텍스트탭"])
			{
				_현재_선택한_탭 = "텍스트탭";
				사전웹브라우저.Navigate("about:blank");
				string 사전페이지내용 = String.Format("<body leftmargin=0 rightmargin=0 topmargin=0 bottommargin=0 marginwidth=0 marginheight=0><table width=100% background=\"{0}\" height=100% valign=center><tr><td align=center><font face=youth color=grey size=5>에피메테우스는 귀퉁이의 여기저기가 뜯겨져나간 두루마리를 주며 말했다.<br><br><br><br></font><font face=youth color=white size=5>\"여기저기 빼먹은 이야기들을 아주 조금만 꾸며내 주게...<br><br>정말이지 금방이면 멋진 이야기가 완성될걸세.\"</font></td></tr></table>", _IMG루트폴더 + "프로그램스킨\\StoryOfEpimetheus.png");
				사전웹브라우저.Document.Write(사전페이지내용);
				사전웹브라우저.Refresh();
			}
			else if (탭.SelectedTab == 탭.TabPages["사전탭"])
			{
				#region 화면 좌측 하단 사전 표시 기능을 끈다.
				사전표제어.Text = "";
				사전의미.Text = "";
				사전발음기호.Text = "";
				#endregion

				_현재_선택한_탭 = "사전탭";

				사전웹브라우저.Navigate("about:blank");
				string 사전페이지내용 = String.Format("<body leftmargin=0 rightmargin=0 topmargin=0 bottommargin=0 marginwidth=0 marginheight=0><table width=100% background=\"{0}\" height=100% valign=center><tr><td align=center><font face=youth color=grey size=5>에피메테우스는 귀퉁이의 여기저기가 뜯겨져나간 두루마리를 주며 말했다.<br><br><br><br></font><font face=youth color=white size=5>\"여기저기 빼먹은 이야기들을 아주 조금만 꾸며내 주게...<br><br>정말이지 금방이면 멋진 이야기가 완성될걸세.\"</font></td></tr></table>", _IMG루트폴더 + "프로그램스킨\\StoryOfEpimetheus.png");
				사전웹브라우저.Document.Write(사전페이지내용);
				사전웹브라우저.Refresh();
			}
			else if (탭.SelectedTab == 탭.TabPages["그래픽탭"])
			{
				_현재_선택한_탭 = "그래픽탭"; // 위의 표시는 문법이 너무 어렵다.
				사전웹브라우저.Navigate("about:blank");
				string 사전페이지내용 = String.Format("<body leftmargin=0 rightmargin=0 topmargin=0 bottommargin=0 marginwidth=0 marginheight=0><table width=100% background=\"{0}\" height=100% valign=center><tr><td align=center><font face=youth color=grey size=5>에피메테우스는 귀퉁이의 여기저기가 뜯겨져나간 두루마리를 주며 말했다.<br><br><br><br></font><font face=youth color=white size=5>\"여기저기 빼먹은 이야기들을 아주 조금만 꾸며내 주게...<br><br>정말이지 금방이면 멋진 이야기가 완성될걸세.\"</font></td></tr></table>", _IMG루트폴더 + "프로그램스킨\\StoryOfEpimetheus.png");
				사전웹브라우저.Document.Write(사전페이지내용);
				사전웹브라우저.Refresh();


				#region 화면 좌측 하단 사전 표시 기능을 끈다.
				사전표제어.Text = "";
				사전의미.Text = "";
				사전발음기호.Text = "";
				#endregion



				_JPG경로 = 동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
                    ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
				키워드.Text = _JPG경로;
				전체화면다시그리기();

				//_화면뷰어.그림위치초기화();
	
				if (_동영상배경사용이처음인지) _동영상배경사용이처음인지 = false;
			}
			else if (탭.SelectedTab == 탭.TabPages["문서정보탭"])
			{
				#region 화면 좌측 하단 사전 표시 기능을 끈다.
				사전표제어.Text = "";
				사전의미.Text = "";
				사전발음기호.Text = "";
				#endregion

				_현재_선택한_탭 = "문서정보탭";
				사전웹브라우저.Navigate("about:blank");
				string 사전페이지내용 = String.Format("<body leftmargin=0 rightmargin=0 topmargin=0 bottommargin=0 marginwidth=0 marginheight=0><table width=100% background=\"{0}\" height=100% valign=center><tr><td align=center><font face=youth color=grey size=5>에피메테우스는 귀퉁이의 여기저기가 뜯겨져나간 두루마리를 주며 말했다.<br><br><br><br></font><font face=youth color=white size=5>\"여기저기 빼먹은 이야기들을 아주 조금만 꾸며내 주게...<br><br>정말이지 금방이면 멋진 이야기가 완성될걸세.\"</font></td></tr></table>", _IMG루트폴더 + "프로그램스킨\\StoryOfEpimetheus.png");
				사전웹브라우저.Document.Write(사전페이지내용);
				사전웹브라우저.Refresh();

			}
		}

		public void 동영상용화면만들기()
		{
			동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
                ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
		}

		public void 전체화면다시그리기()
        {
		
            _이미지높이 = _배경과본문_비트맵.Height;
            
			if(_동영상배경사용이처음인지)	this.MouseWheel += new MouseEventHandler(스크롤마우스휠처리);

			if (_처음그리거나바뀌어야할때 == true)
			{
				_현재화면비트맵 = new Bitmap(픽쳐박스1.Width, 픽쳐박스1.Height);
				_처음그리거나바뀌어야할때 = false;
			}

			Graphics _현재화면그래픽 = Graphics.FromImage(_현재화면비트맵);

            _현재화면그래픽.DrawImage(_배경과본문_비트맵, new Point(0, 0));

			문제유형만들기(본문.Text);
			동영상용사전만들기();
			동영상용사전그리기("");

			해석만들기(해석.Text);
			해석보여주기(-1000);
			_현재화면그래픽.DrawImage(_비트맵_문제유형, 0, 256);
			_현재화면그래픽.DrawImage(_동영상용사전_비트맵, 0, 픽쳐박스1.Height - 250);
			_현재화면그래픽.DrawImage(_해석보여주는로봇_비트맵, 1280 - 300, 30); // 1280-300의 수치는 좌표일 뿐이다.
			픽쳐박스1.Image = _현재화면비트맵;

            _현재화면그래픽.Dispose();
        }
		private void 스크롤마우스휠처리(object sender, MouseEventArgs e)
        {
			mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, 0);
			그림위치변경(e.Delta);
        }

		public void 그림위치변경_최근()
		{
			if (_처음그리거나바뀌어야할때 == true)
			{
				_현재화면비트맵 = new Bitmap(픽쳐박스1.Width, 픽쳐박스1.Height);
				_처음그리거나바뀌어야할때 = false;
			}

			Graphics _현재화면그래픽 = Graphics.FromImage(_현재화면비트맵);

			if (_y그림위치 > _이미지높이 - 픽쳐박스1.Height) _y그림위치 = _이미지높이 - 픽쳐박스1.Height;

			if (_y그림위치 < 0) _y그림위치 = 0;

			_현재화면그래픽.DrawImage(_배경과본문_비트맵, new Rectangle(0, 0, 픽쳐박스1.Width, 픽쳐박스1.Height), new Rectangle(0, _y그림위치, 픽쳐박스1.Width, 픽쳐박스1.Height), GraphicsUnit.Pixel);
			_현재화면그래픽.DrawImage(_비트맵_문제유형, 0, 256);
			_현재화면그래픽.DrawImage(_동영상용사전_비트맵, 0, 픽쳐박스1.Height - 250);
			_현재화면그래픽.DrawImage(_해석보여주는로봇_비트맵, 1280 - 300, 30);

			픽쳐박스1.Image = _현재화면비트맵;

			_현재화면그래픽.Dispose();
		}

		private int _Delta = 0;

		Bitmap _현재화면비트맵;
		bool _처음그리거나바뀌어야할때 = true;

		public void 그림위치변경(int Delta)
        {
			if (_처음그리거나바뀌어야할때 == true)
			{
				_현재화면비트맵 = new Bitmap(픽쳐박스1.Width, 픽쳐박스1.Height);
				_처음그리거나바뀌어야할때 = false;
			}
			
			Graphics _현재화면그래픽 = Graphics.FromImage(_현재화면비트맵);

			_Delta = Delta;
			_y그림위치 -= Delta / 10; // 10은 그냥 수치임

			if (_y그림위치 > _이미지높이 - 픽쳐박스1.Height)        _y그림위치 = _이미지높이 - 픽쳐박스1.Height;

            if (_y그림위치 < 0)										_y그림위치 = 0;

			_현재화면그래픽.DrawImage(_배경과본문_비트맵, new Rectangle(0,0, 픽쳐박스1.Width, 픽쳐박스1.Height), new Rectangle(0, _y그림위치, 픽쳐박스1.Width, 픽쳐박스1.Height), GraphicsUnit.Pixel);
			_현재화면그래픽.DrawImage(_비트맵_문제유형, 0, 256);
			_현재화면그래픽.DrawImage(_동영상용사전_비트맵, 0, 픽쳐박스1.Height - 250);
			_현재화면그래픽.DrawImage(_해석보여주는로봇_비트맵, 1280 - 300, 30);

			픽쳐박스1.Image = _현재화면비트맵;

			_현재화면그래픽.Dispose();

        }

		int _현재i = -1;
		int _현재j = -1;


		private int 화면업데이트여부확인하고사전화면갱신()
		{
			Point 픽쳐박스1위치 = 픽쳐박스1.PointToScreen(Point.Empty);
			int X = Cursor.Position.X - 픽쳐박스1위치.X;
			int Y = Cursor.Position.Y - 픽쳐박스1위치.Y;

			int 업데이트여부 = 0;
            bool 동영상사전에써야하는지여부 = false;
			for (int i = 0; i < _본문_여러줄.Count; i++)
			{
				for (int j = 0; j < _본문_여러줄[i]._어절들.Count; j++)
				{
					if (_본문_여러줄[i]._어절들[j].x글자시작좌표 < (X - 312) 
					&& (X - 312) < _본문_여러줄[i]._어절들[j].x글자끝좌표 
					&& _본문_여러줄[i]._어절들[j].y글자시작좌표 - 5 < (Y + _y그림위치 + 15)  // -5는 약간 윗쪽으로 더 여유를 준 값이다.
					&& (Y + _y그림위치 + 15) < _본문_여러줄[i]._어절들[j].y밑줄좌표 + 15) // 15은 약간 아래쪽으로 더 여유를 준 값이다.
					{
						_현재i = i; _현재j = j;
                        // 여기에 한번이라도 들어왔는지 확인하여, 동영상 사전의 내용을 지워야하는지 확인한다.
                        동영상사전에써야하는지여부 = true;

                        if (_본문_여러줄[i]._어절들[j].내용 != _현재어절)
						{
							_현재어절 = _본문_여러줄[i]._어절들[j].내용;

							동영상용사전그리기(_본문_여러줄[i]._어절들[j].내용);
							업데이트여부 = 1;
						}

						if (_본문_여러줄[i]._어절들[j].몇번째문장인지 != _현재본문번호)
						{
							_현재본문번호 = _본문_여러줄[i]._어절들[j].몇번째문장인지;

							해석보여주기(_현재본문번호);
							업데이트여부 = 1;

						}


					}
				}
			}

            for (int i = 0; i < _A1_여러줄.Count; i++)
            {
                for (int j = 0; j < _A1_여러줄[i]._어절들.Count; j++)
                {
                    if (_A1_여러줄[i]._어절들[j].x글자시작좌표 < (X - 312)
                    && (X - 312) < _A1_여러줄[i]._어절들[j].x글자끝좌표
                    && _A1_여러줄[i]._어절들[j].y글자시작좌표 - 5 < (Y + _y그림위치 + 15)  // -5는 약간 윗쪽으로 더 여유를 준 값이다.
                    && (Y + _y그림위치 + 15) < _A1_여러줄[i]._어절들[j].y밑줄좌표 + 15) // 15은 약간 아래쪽으로 더 여유를 준 값이다.
                    {
                        if (_A1_여러줄[i]._어절들[j].내용 != _현재어절)
                        {
                            _현재어절 = _A1_여러줄[i]._어절들[j].내용;

                            동영상용사전그리기(_A1_여러줄[i]._어절들[j].내용);
                            업데이트여부 = 1;
                        }
                    }
                }
            }


            if (동영상사전에써야하는지여부 == false && 업데이트여부 == 0) 업데이트여부 = -1;

            return 업데이트여부;
		}
		private void 휠처리_화면업데이트(MouseEventArgs e)
		{
			for (int i = 0; i < _본문_여러줄.Count; i++)
			{
				for (int j = 0; j < _본문_여러줄[i]._어절들.Count; j++)
				{
					if (_본문_여러줄[i]._어절들[j].x글자시작좌표 < (e.X - 562) && 
					(e.X - 562) < _본문_여러줄[i]._어절들[j].x글자끝좌표 && 
					_본문_여러줄[i]._어절들[j].y글자시작좌표 < (e.Y + _y그림위치 - 27) && 
					(e.Y + _y그림위치 - 27) < _본문_여러줄[i]._어절들[j].y밑줄좌표)
					{
						if (_본문_여러줄[i]._어절들[j].내용 != _현재어절)
						{
							_현재어절 = _본문_여러줄[i]._어절들[j].내용;

							동영상용사전그리기(_본문_여러줄[i]._어절들[j].내용);
						}

						if (_본문_여러줄[i]._어절들[j].몇번째문장인지 != _현재본문번호)
						{
							_현재본문번호 = _본문_여러줄[i]._어절들[j].몇번째문장인지;

							//_동영상용해석뷰어.해석과_강조할문장_번호(_해석, _현재본문번호);
						}
					}

				}
			}
		}
		#endregion
		public void 문제유형만들기(string CAKE)
        {
			_비트맵_문제유형 = new Bitmap(256, 128);
            _그래픽_문제유형 = Graphics.FromImage(_비트맵_문제유형);

            if (CAKE.Contains("{주제}") || CAKE.Contains("{제목}")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.주제_활성화, new Rectangle(0, 0, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.주제_비활성화, new Rectangle(0, 0, 64, 64));

			if (CAKE.Contains("{빈칸}")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.빈칸_활성화, new Rectangle(64, 0, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.빈칸_비활성화, new Rectangle(64, 0, 64, 64));

            if (CAKE.Contains("{흐름}")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.흐름_활성화, new Rectangle(128, 0, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.흐름_비활성화, new Rectangle(128, 0, 64, 64));

            if (CAKE.Contains("{어법")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.어법_활성화, new Rectangle(192, 0, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.어법_비활성화, new Rectangle(192, 0, 64, 64));

            if (CAKE.Contains("{일치}")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.일치_활성화, new Rectangle(0, 64, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.일치_비활성화, new Rectangle(0, 64, 64, 64));

            if (CAKE.Contains("{분위기}")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.느낌_활성화, new Rectangle(64, 64, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.느낌_비활성화, new Rectangle(64, 64, 64, 64));

            if (CAKE.Contains("{지시}")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.지시_활성화, new Rectangle(128, 64, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.지시_비활성화, new Rectangle(128, 64, 64, 64));

            if (CAKE.Contains("{어휘")) _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.어휘_활성화, new Rectangle(192, 64, 64, 64));
            else _그래픽_문제유형.DrawImage(편집기의_제왕.Properties.Resources.어휘_비활성화, new Rectangle(192, 64, 64, 64));


            //_문제유형뷰어.Owner = pictureBox1;
            //_문제유형뷰어.문제유형배경입히기(CAKE);

        }
		public void 동영상용사전만들기()
		{
            _동영상용사전_기본글꼴 = new System.Drawing.Font("Youth", 10f);
            _동영상용사전_기본붓 = new SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255));

			_동영상용사전발음기호_기본글꼴 = new System.Drawing.Font("Charis SIL", 12f);

			_동영상용사전_해설글꼴 = new System.Drawing.Font("Youth", 10f);
            _동영상용사전_해설붓 = new SolidBrush(System.Drawing.Color.FromArgb(245, 245, 245));

            _동영상용사전_배경이미지 = Image.FromFile(_IMG루트폴더 + "프로그램스킨/동영상해설사전배경_darkblue.png");

			_동영상용사전_비트맵 = new Bitmap(300, 250);

            _동영상용사전_그래픽 = Graphics.FromImage(_동영상용사전_비트맵);

			_동영상용사전_그래픽.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;




		}
		public void 동영상용사전그리기(string text)
		{
			_동영상용사전_그래픽.Clear(System.Drawing.Color.Transparent);

            _동영상용사전_그래픽.DrawImage(_동영상용사전_배경이미지, new Rectangle(0, 0, 300, 250));

            string 현재어절 = 변환.문자열.불필요문자제거(text);

            while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
            while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

            string 표제어 = "";

            string 검색결과 = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);
			string 사전발음기호s = _검색.영한발음기호사전(표제어);

			if (사전발음기호s != "")
				사전발음기호s = "[" + 사전발음기호s + "]";
			else
				사전발음기호s = "";



			_동영상용사전_그래픽.DrawString(표제어.ToUpper(), _동영상용사전_기본글꼴, _동영상용사전_기본붓, new PointF(20, 31));
			_동영상용사전_그래픽.DrawString(사전발음기호s, _동영상용사전발음기호_기본글꼴, _동영상용사전_기본붓, new PointF(16, 55));

			_동영상용사전_그래픽.DrawString(검색결과, _동영상용사전_해설글꼴, _동영상용사전_해설붓, new RectangleF(20, 100, 235, 155));

			if (_어휘문제목록해시.ContainsKey(표제어.ToLower()))
			{
				string 반의어 = _어휘문제목록해시[표제어.ToLower()];

				string tmp = "";
				string 반의어뜻 = _검색.영한사전_어떻게든_결과를_내는(반의어, ref tmp);
				string 반의어뜻줄이기 = 반의어뜻.너비(160);
				if (반의어뜻 != 반의어뜻줄이기)
					반의어뜻 = 반의어뜻줄이기 + "...";
				_동영상용사전_그래픽.DrawString(반의어 + " : " + 반의어뜻, _동영상용사전_해설글꼴, _동영상용사전_해설붓, new RectangleF(20, 227, 235, 15));
			}
		}

		#region 그래픽화면_해석화면
		public int 문장길이확인(string 문자열)
		{
			int 문장길이 = 0;

			for (int i = 0; i < 문자열.Length; i++)
			{
				문장길이 += 문자길이확인(문자열[i]);
			}

			return (int)문장길이;
		}
		private int 문자길이확인(char 문자)
		{
            if (문자 == 'A') return 12;
            else if (문자 == 'B') return 12;
            else if (문자 == 'C') return 12;
            else if (문자 == 'D') return 12;
            else if (문자 == 'E') return 12;
            else if (문자 == 'F') return 12;
            else if (문자 == 'G') return 12;
            else if (문자 == 'H') return 12;
            else if (문자 == 'I') return 5;
            else if (문자 == 'J') return 8;
            else if (문자 == 'K') return 11;
            else if (문자 == 'L') return 11;
            else if (문자 == 'M') return 17;
            else if (문자 == 'N') return 14;
            else if (문자 == 'O') return 14;
            else if (문자 == 'P') return 11;
            else if (문자 == 'Q') return 11;
            else if (문자 == 'R') return 11;
            else if (문자 == 'S') return 11;
            else if (문자 == 'T') return 11;
            else if (문자 == 'U') return 11;
            else if (문자 == 'V') return 11;
            else if (문자 == 'W') return 14;
            else if (문자 == 'X') return 11;
            else if (문자 == 'Y') return 11;
            else if (문자 == 'Z') return 11;
            else if (문자 == 'a') return 9;
            else if (문자 == 'b') return 12;
            else if (문자 == 'c') return 9;
            else if (문자 == 'd') return 11;
            else if (문자 == 'e') return 11;
            else if (문자 == 'f') return 8;
            else if (문자 == 'g') return 11;
            else if (문자 == 'h') return 11;
            else if (문자 == 'i') return 5;
            else if (문자 == 'j') return 6;
            else if (문자 == 'k') return 9;
            else if (문자 == 'l') return 5;
            else if (문자 == 'm') return 17;
            else if (문자 == 'n') return 11;
            else if (문자 == 'o') return 11;
            else if (문자 == 'p') return 11;
            else if (문자 == 'q') return 9;
            else if (문자 == 'r') return 9;
            else if (문자 == 's') return 9;
            else if (문자 == 't') return 6;
            else if (문자 == 'u') return 11;
            else if (문자 == 'v') return 9;
            else if (문자 == 'w') return 14;
            else if (문자 == 'x') return 9;
            else if (문자 == 'y') return 9;
            else if (문자 == 'z') return 9;

            else if (문자 == '1') return 9;
            else if (문자 == '2') return 9;
            else if (문자 == '3') return 9;
            else if (문자 == '4') return 9;
            else if (문자 == '5') return 9;
            else if (문자 == '6') return 9;
            else if (문자 == '7') return 9;
            else if (문자 == '8') return 9;
            else if (문자 == '9') return 9;
            else if (문자 == '0') return 9;
            else if (문자 == '?') return 9;
            else if (문자 == '!') return 5;
            else if (문자 == '.') return 5;
            else if (문자 == ',') return 5;
            else if (문자 == '“') return 6;
            else if (문자 == '‘') return 5;
            else if (문자 == '’') return 5;

            else if (문자 == '”') return 6;
            else if (문자 == ':') return 5;
            else if (문자 == '/') return 8;

            else if (문자 == '(') return 8;
            else if (문자 == ')') return 8;


            else if (문자 == '\"') return 8;
            else if (문자 == '\'') return 5;
            else if (문자 == ' ') return 11;
            else if (문자 == '-') return 8;


            else return 18;
		}
		private void 한라인_딱_맞춰그리기(ref Graphics 그래픽객체, string 그릴텍스트, int x좌표, int y좌표, bool 붉은색인지)
		{
			// 1. 어절별로 모두 나눈다. 
			// 2. 각각의 어절의 길이를 확인하고, 전체너비에서 어절 길의의 합을 뺀다.
			// 3. 뺀 값을 빈칸 갯수로 나눈다.
			// 4. 어절을 하나씩 그리고 구한 빈칸 크기만큼 더한다.
			// 5. 마지막 것은 맨 뒤에 딱 맞도록 붙인다.

			// 1. 어절별로 모두 나눈다. 
			List<string> 어절들 = new List<string>();
			변환.문자열.어절들로(그릴텍스트, ref 어절들);

			// 2. 각각의 어절의 길이를 확인하고, 전체너비에서 어절 길의의 합을 뺀다.
			int 어절길이의합 = 0;

			for (int i = 0; i < 어절들.Count; i++)
			{
				어절길이의합 += 문장길이확인(어절들[i]);
			}

			int 맨끝너비 = 410;
			int 전체너비 = 맨끝너비 - x좌표;

			// 3. 뺀 값을 빈칸 갯수로 나눈다.
			int 공백간격;

			if (어절들.Count > 1 && 전체너비 > 어절길이의합)
				공백간격 = (전체너비 - 어절길이의합) / (어절들.Count - 1);
			else
				공백간격 = 0;

			int 현재x좌표 = x좌표;
			for (int i = 0; i < 어절들.Count; i++)
			{
				if (i != 어절들.Count - 1)
				{
					//if (붉은색인지) 그래픽객체.DrawString(어절들[i], _해석기본글꼴, _해석해설붓_r, _해석_왼쪽여백 + 현재x좌표, y좌표);
					//else 그래픽객체.DrawString(어절들[i], _해석기본글꼴, _해석해설붓_w, _해석_왼쪽여백 + 현재x좌표, y좌표);

                    if (붉은색인지) 어절그리기(ref 그래픽객체, 어절들[i], 현재x좌표, y좌표, true);
                    else 어절그리기(ref 그래픽객체, 어절들[i], 현재x좌표, y좌표, false);

                    현재x좌표 += 문장길이확인(어절들[i]) + 공백간격;
				}
				else
				{
                    // if (붉은색인지) 그래픽객체.DrawString(어절들[i], _해석기본글꼴, _해석해설붓_r, _해석_왼쪽여백 + 맨끝너비 - 문장길이확인(어절들[i]), y좌표);
                    // else 그래픽객체.DrawString(어절들[i], _해석기본글꼴, _해석해설붓_w, _해석_왼쪽여백 + 맨끝너비 - 문장길이확인(어절들[i]), y좌표);

                    if (붉은색인지) 어절그리기(ref 그래픽객체, 어절들[i], 맨끝너비 - 문장길이확인(어절들[i]), y좌표, true);
                    else 어절그리기(ref 그래픽객체, 어절들[i], 맨끝너비 - 문장길이확인(어절들[i]), y좌표, false);
                }
            }
		}

        private void 어절그리기(ref Graphics 그래픽객체, string 어절, int x좌표, int y좌표, bool 붉은색인지)
        {
            int 다음글자를넣을x좌표 = 0;

            if (붉은색인지)
            {
                foreach (char 현재글자 in 어절)
                {
                    그래픽객체.DrawString(현재글자.ToString(), _해석기본글꼴, _해석해설붓_r, x좌표 + 다음글자를넣을x좌표, y좌표);
                    다음글자를넣을x좌표 += (int)문자길이확인(현재글자);
                }
            }
            else
            {
                foreach (char 현재글자 in 어절)
                {
                    그래픽객체.DrawString(현재글자.ToString(), _해석기본글꼴, _해석해설붓_w, x좌표 + 다음글자를넣을x좌표, y좌표);
                    다음글자를넣을x좌표 += (int)문자길이확인(현재글자);
                }
            }
        }

        public void 해석만들기(string text)
		{
			_해석보여주는로봇_비트맵 = new Bitmap(480, 990);

			_해석보여주는로봇_그래픽 = Graphics.FromImage(_해석보여주는로봇_비트맵);


			_해석로봇 = Image.FromFile(_IMG루트폴더 + "프로그램스킨/동영상해설배경_darkblue.png");

			_문장단위_해석들 = new List<string>();
			강력하지만무거운변환.문자열.문장단위의_문자열들로_탭과개행문자살림(text, ref _문장단위_해석들);
		}
		public void 해석보여주기(int 붉게_표시할_문장인덱스) //
		{
            #region 초기화

            _해석보여주는로봇_그래픽.Clear(System.Drawing.Color.Transparent);
			_해석보여주는로봇_그래픽.DrawImage(_해석로봇, new Rectangle(0, 0, 480, 990));



			Bitmap _해석판_비트맵;
			Graphics _해석판_그래픽;

			
			_해석판_비트맵 = new Bitmap(450, 960); 
			
			_해석판_그래픽 = Graphics.FromImage(_해석판_비트맵);
            _해석판_그래픽.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			int x위치 = 0;
            int 현재까지그려진x위치 = 0;
			int y위치 = 20;
			int 탭과개행문자계산 = 0;

			string 흰줄 = "";
			string 붉은줄 = "";
            bool 붉은줄여부;

			#endregion
			for (int i = 0; i < _문장단위_해석들.Count; i++)
			{
				string 현재문장단위해석들 = _문장단위_해석들[i];
                if (i == 붉게_표시할_문장인덱스 + 탭과개행문자계산) 붉은줄여부 = true;
                else 붉은줄여부 = false;

                // 글자를 하나씩 하나씩 읽는다. 그리고 하얀줄이면, 하얀줄 버퍼에, 붉은 줄이면 붉은 줄 버퍼에 하나하나 채워넣는다.
                // 그 다음 그것을 언제 그리냐 하면, 하얀줄은, 붉은 줄이 나오기 시작해야 그리고, 붉은 줄은 반대로 하얀줄이 나오기 시작해야 그린다.
                // 이런식으로 하나하나 채우기 때문에, 줄이 넘쳐버리면, 그 때 또 한 줄을 그린다.
                
                // 한 줄의 끝까지 가기전에 문장이 끝나면,
                if (현재문장단위해석들 == "\r")
                {
                    탭과개행문자계산++;

                    if (붉은줄.Trim() != "") { 어절그리기(ref _해석판_그래픽, 붉은줄.Trim(), _해석_왼쪽여백 + 현재까지그려진x위치, y위치, true); 붉은줄 = ""; }
                    else if (흰줄.Trim() != "") {어절그리기(ref _해석판_그래픽, 흰줄.Trim(), _해석_왼쪽여백 + 현재까지그려진x위치, y위치, false); 흰줄 = ""; }

                    y위치 += _행간; x위치 = 0;
                    현재까지그려진x위치 = 0;
                }
                else
                {
                    for (int j = 0; j < 현재문장단위해석들.Length; j++)
                    {
                        bool jump = false, jump2 = false; ;

                        // 여기서부터는 좀 다르게 짜는 건데,
                        // 앞문장의 내용이 남아 있으면 그려준다.

                        #region x위치 계산

                        if (현재문장단위해석들[j].ToString() == "\t") { 탭과개행문자계산++; if (x위치 == 6) { x위치 = 12; } else { x위치 += 12; } }
                        else if (현재문장단위해석들[j].ToString() == " ") { if (x위치 != 0) { x위치 += 6; } }
                        else if (현재문장단위해석들[j].ToString() == "A") x위치 += 8;
                        else if (현재문장단위해석들[j].ToString() == "B") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "C") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "D") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "E") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "F") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "G") x위치 += 8;
                        else if (현재문장단위해석들[j].ToString() == "H") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "I") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == "J") x위치 += 5;
                        else if (현재문장단위해석들[j].ToString() == "K") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "L") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "M") x위치 += 11;
                        else if (현재문장단위해석들[j].ToString() == "N") x위치 += 9;
                        else if (현재문장단위해석들[j].ToString() == "O") x위치 += 9;
                        else if (현재문장단위해석들[j].ToString() == "P") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "Q") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "R") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "S") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "T") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "U") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "V") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "W") x위치 += 9;
                        else if (현재문장단위해석들[j].ToString() == "X") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "Y") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "Z") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "a") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "b") x위치 += 8;
                        else if (현재문장단위해석들[j].ToString() == "c") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "d") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "e") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "f") x위치 += 5;
                        else if (현재문장단위해석들[j].ToString() == "g") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "h") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "i") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == "j") x위치 += 4;
                        else if (현재문장단위해석들[j].ToString() == "k") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "l") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == "m") x위치 += 11;
                        else if (현재문장단위해석들[j].ToString() == "n") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "o") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "p") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "q") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "r") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "s") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "t") x위치 += 4;
                        else if (현재문장단위해석들[j].ToString() == "u") x위치 += 7;
                        else if (현재문장단위해석들[j].ToString() == "v") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "w") x위치 += 9;
                        else if (현재문장단위해석들[j].ToString() == "x") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "y") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "z") x위치 += 6;

                        else if (현재문장단위해석들[j].ToString() == "1") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "2") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "3") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "4") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "5") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "6") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "7") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "8") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "9") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "0") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "?") x위치 += 6;
                        else if (현재문장단위해석들[j].ToString() == "!") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == ".") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == ",") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == "“") x위치 += 4;
                        else if (현재문장단위해석들[j].ToString() == "‘") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == "’") x위치 += 3;

                        else if (현재문장단위해석들[j].ToString() == "”") x위치 += 4;
                        else if (현재문장단위해석들[j].ToString() == ":") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == "/") x위치 += 5;

                        else if (현재문장단위해석들[j].ToString() == "(") x위치 += 5;
                        else if (현재문장단위해석들[j].ToString() == ")") x위치 += 5;


                        else if (현재문장단위해석들[j].ToString() == "\"") x위치 += 5;
                        else if (현재문장단위해석들[j].ToString() == "\'") x위치 += 3;
                        else if (현재문장단위해석들[j].ToString() == "-") x위치 += 5;

                        else x위치 += 12;
                        #endregion
                        // 현재의 문장이 하얀 문장이고, 앞의 문장도 하얀 문장이면, 앞의 문장을 일단 그려준다.
                        if (j == 0 && 흰줄 != "" && 붉은줄여부 == false)
                        {
                            어절그리기(ref _해석판_그래픽, 흰줄.Trim(), _해석_왼쪽여백 + 현재까지그려진x위치, y위치, false);

                            현재까지그려진x위치 += 문장길이확인(흰줄.Trim() + " ");

                            흰줄 = "";
                        }
                        // 현재의 문장이 붉은 문장이고, 앞의 문장이 하얀 문장이면, 역시 앞의 문장을 일단 그려준다.
                        else if (j == 0 && 흰줄 != "" && 붉은줄여부 == true)
                        {
                            어절그리기(ref _해석판_그래픽, 흰줄.Trim(), _해석_왼쪽여백 + 현재까지그려진x위치, y위치, false);

                            현재까지그려진x위치 += 문장길이확인(흰줄.Trim() + " ");

                            흰줄 = "";
                        }
                        // 현재의 문장이 하얀 문장이고, 앞의 문장이 붉은 문장이면, 역시 앞의 문장을 일단 그려준다.
                        else if (j == 0 && 붉은줄 != "" && 붉은줄여부 == false)
                        {
                            어절그리기(ref _해석판_그래픽, 붉은줄.Trim(), _해석_왼쪽여백 + 현재까지그려진x위치, y위치, true);

                            현재까지그려진x위치 += 문장길이확인(붉은줄.Trim() + " ");

                            붉은줄 = "";
                        }



                        // 현재 줄이 오른쪽 끝까지 갔다는 이야기다. 마무리하고 그려버려야 한다.
                        //else if (x위치 > 220)
                        else if (x위치 > 215)
                        {
                            #region 바로 다음 줄 마침표처리
                            // 여기서 우선, 현재 줄의 바로 다음 줄이 .같은 것들이면, 이거는 알아서 붙여줘야 한다.
                            if (현재문장단위해석들[j].ToString() == "." || 현재문장단위해석들[j].ToString() == "," || 현재문장단위해석들[j].ToString() == "’" || 현재문장단위해석들[j].ToString() == "\'" || 현재문장단위해석들[j].ToString() == "\"" || 현재문장단위해석들[j].ToString() == "!" && 현재문장단위해석들[j].ToString() == "?")
                            {
                                if (붉은줄.Trim() != "") 붉은줄 = 붉은줄.Trim() + 현재문장단위해석들[j].ToString();
                                else if (흰줄.Trim() != "") 흰줄 = 흰줄.Trim() + 현재문장단위해석들[j].ToString();

                                jump = true;
                                if (현재문장단위해석들.Length - 1 > j)
                                {
                                    if (현재문장단위해석들[j+1].ToString() == "\"")
                                    {
                                        
                                        if (붉은줄.Trim() != "") 붉은줄 = 붉은줄.Trim() + 현재문장단위해석들[j+1].ToString();
                                        else if (흰줄.Trim() != "") 흰줄 = 흰줄.Trim() + 현재문장단위해석들[j+1].ToString();
                                        jump2 = true;
                                    }
                                }

                            }

                            if (현재문장단위해석들.Length - 1 > j)
                            {

                                if (문자열.알파벳(현재문장단위해석들[j]) && 문자열.알파벳(현재문장단위해석들[j + 1]))
                                {
                                    if (붉은줄.Trim() != "") 붉은줄 = 붉은줄.Trim() + "-";
                                    else if (흰줄.Trim() != "") 흰줄 = 흰줄.Trim() + "-";
                                }
                            }
                            #endregion
                            if (붉은줄여부)
                            {
                                한라인_딱_맞춰그리기(ref _해석판_그래픽, 붉은줄.Trim(), _해석_왼쪽여백 + 현재까지그려진x위치, y위치, true);
                                붉은줄 = "";
                            }
                            else
                            {
                                한라인_딱_맞춰그리기(ref _해석판_그래픽, 흰줄.Trim(), _해석_왼쪽여백 + 현재까지그려진x위치, y위치, false);
                                흰줄 = "";
                            }

                            x위치 = 0;
                            y위치 += _행간;

                            현재까지그려진x위치 = 0;
                        }

                        if (jump == false)
                        {
                            if (붉은줄여부) 붉은줄 += 현재문장단위해석들[j].ToString();
                            else 흰줄 += 현재문장단위해석들[j].ToString();
                        }

                        if(jump2 == true)
                        {
                            j++; // 더블 점프인경우 그냥 다음 것까지 건너 뛴다.
                        }
                    }
                }
			}

            //  그리지 못한 남은 내용이 있으면 여기에서 그려준다.
            if (붉은줄.Trim() != "")
            {
                //_해석판_그래픽.DrawString(붉은줄.Trim(), _해석기본글꼴, _해석해설붓_r, _해석_왼쪽여백, y위치);
                _해석판_그래픽.DrawString(붉은줄.Trim(), _해석기본글꼴, _해석해설붓_r, _해석_왼쪽여백 + 현재까지그려진x위치, y위치);

            }
            else if (흰줄.Trim() != "")
            {
                //_해석판_그래픽.DrawString(흰줄.Trim(), _해석기본글꼴, _해석해설붓_w, _해석_왼쪽여백, y위치);
                _해석판_그래픽.DrawString(흰줄.Trim(), _해석기본글꼴, _해석해설붓_w, _해석_왼쪽여백 + 현재까지그려진x위치, y위치); // 고쳐본 내용
            }


            Point[] 점들 = { new Point(18, 30), new Point(458, 30), new Point(18, 990) };
			

			_해석보여주는로봇_그래픽.DrawImage(_해석판_비트맵, 점들); 



			_해석판_비트맵.Dispose();
			_해석판_그래픽.Dispose();


			//_해석보여주는로봇_그래픽.Dispose();
		}
		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
            int 갱신여부확인 = 화면업데이트여부확인하고사전화면갱신();

            if (갱신여부확인 == 1)
			{
				//타이틀바텍스트애니메이션(현재마우스포인터테스트용);

				동영상용화면.마우스따라_다시만들기(_현재i, _현재j, _현재본문번호, ref _모르는단어리스트, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text,
										보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, ref _배경과본문_그래픽);


				그림위치변경_최근();
			}
            // 여기 고치던 중임// 여기 고치던 중임// 여기 고치던 중임
            /*
            else if(갱신여부확인 == -1)
            {
                // 여기 고치던 중임
                동영상용화면.마우스따라_다시만들기(0, 0, _현재본문번호, ref _모르는단어리스트, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text,
                                        보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, ref _본문_여러줄, ref _배경과본문_그래픽);


                그림위치변경_최근();
            }
            */
        }
        #endregion

        #endregion

        #region 사전편집창

        public void 탭선택(int index)
		{
			탭.SelectedIndex = index;
		}

		private void 검색_Click(object sender, EventArgs e)
		{
		    사전페이지_사전의미.Text = _검색.영한사전_문장부호제거(사전페이지_사전표제어.Text);
            사전페이지_사전발음기호.Text = _검색.영한발음기호사전(사전페이지_사전표제어.Text);
		}

		private void 항목추가_Click(object sender, EventArgs e)
		{
			사전업데이트(사전페이지_사전표제어.Text, 사전페이지_사전발음기호.Text, 사전페이지_사전의미.Text);

			메시지박스.보여주기("추가완료", this);
		}

		private void 명사로잘쓰이지않음_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "명사로잘쓰이지않음";
		}

		private void 분사수식가능명사_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "분사수식가능명사";
		}

		private void 분사로잘쓰이지않음_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "분사로잘쓰이지않음";
		}

		private void ing형_Click(object sender, EventArgs e)
		{
			string 원래사전표제어 = 사전페이지_사전표제어.Text;
            string 원래사전표제어에서e뺀것 = "";

            if(_검색.Ing형인지(원래사전표제어))
            {
                string 원형동사 = _검색.Ing형을원형으로(원래사전표제어);
                string 현재사전의미 = 사전페이지_사전의미.Text;

                사전페이지_사전의미.Text = 현재사전의미 + " " + "'" + 원형동사 + " " +  사전의미에서동사만남기기(Form1._검색.영한사전_문장부호제거(원형동사)) + "'의 ing형";
                return;
            }


            string Ing형 = _검색.원형을Ing로(원래사전표제어);

            if(Ing형 == "")
            {
                if(변환.문자열.Right(원래사전표제어, 1) == "e")
                {
                    원래사전표제어에서e뺀것 = 원래사전표제어.Substring(0, 원래사전표제어.Length -1);
                    사전페이지_사전표제어.Text = 원래사전표제어에서e뺀것 + "ing";
                }
                else
                    사전페이지_사전표제어.Text = 원래사전표제어 + "ing";
            }
            else
            {
                사전페이지_사전표제어.Text = Ing형;
            }


            사전페이지_사전발음기호.Text = "";
            사전페이지_사전의미.Text = "'" + 원래사전표제어 + " " + 사전의미에서동사만남기기(사전페이지_사전의미.Text) + "'의 ing형";
		}

		private void ed형_Click(object sender, EventArgs e)
		{
            string 원래사전표제어 = 사전페이지_사전표제어.Text;

            if(_검색.Ed형인지(원래사전표제어))
            {
                string 원형동사 = _검색.Ed형을원형으로(원래사전표제어);
                string 현재사전의미 = 사전페이지_사전의미.Text;

                사전페이지_사전의미.Text = 현재사전의미 + " " + "'" + 원형동사 + " " +  사전의미에서동사만남기기(Form1._검색.영한사전_문장부호제거(원형동사)) + "'의 ed형";
                return;
            }

            string Ed형 = _검색.원형을Ed형으로(원래사전표제어);

            if(Ed형 == "")
            {
                if(변환.문자열.Right(원래사전표제어, 1) == "e")
                    사전페이지_사전표제어.Text += "d";
                else
                    사전페이지_사전표제어.Text += "ed";
            }
            else
            {
                사전페이지_사전표제어.Text = Ed형;
            }

            사전페이지_사전발음기호.Text = "";
            사전페이지_사전의미.Text = "'" + 원래사전표제어 + " " + 사전의미에서동사만남기기(사전페이지_사전의미.Text) + "'의 ed형";
		}

		private void s형_Click(object sender, EventArgs e)
		{
            string 원래사전표제어 = 사전페이지_사전표제어.Text;

//            if(변환.문자열.Right(원래사전표제어, 1) == "e")
                사전페이지_사전표제어.Text += "s";
            //else
              //  사전표제어.Text += "es";

            사전페이지_사전발음기호.Text = "";
            사전페이지_사전의미.Text = "'" + 원래사전표제어 + " " + 사전페이지_사전의미.Text + "'의 s형";
		}

		private void 서술적으로쓰임_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "서술적으로쓰임";
		}

		private void 한정적으로쓰임_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "한정적으로쓰임";
		}

		private void 사람이름_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "{사람이름}";
		}

		private void 지명_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "{지명}";
		}

		private void form1_입력_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "form1.";
		}

		private void 입력_form2_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "form2.";
		}

		private void 입력_2형식동사_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "2형식동사";
		}
		#endregion

		private void 사전페이지_외부사전_Click(object sender, EventArgs e)
		{
			사전웹브라우저.Navigate(string.Format("http://dic.naver.com/search.nhn?dicQuery={0}&query={0}&target=dic&ie=utf8&query_utf=&isOnlyViewEE=", 사전페이지_사전표제어.Text));
		}

		private void 제1강세_Click(object sender, EventArgs e)
		{
			Clipboard.SetText("́"); _최근복사한클립보드내용 = "́"; // 여기에서는 사각형으로 보일 수 있겠지만, 앞에 문자가 있으면, 위에 제 1 강세 표시를 해 줍니다.
		}

		private void 제2강세_Click(object sender, EventArgs e)
		{
			Clipboard.SetText("̀"); _최근복사한클립보드내용 = "̀"; // 여기에서는 사각형으로 보일 수 있겠지만, 앞에 문자가 있으면, 위에 제 1 강세 표시를 해 줍니다.
		}

		private void IPATurnedV_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "ʌ";
		}

		private void IPAschwa_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "ə";
		}

		private void IPA_HighLevelAccent_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "́";
		}

		private void IPA_LowLevelAccent_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "̀";
		}

		private void IPA_ETH_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "ð";
		}

		private void IPA_ʧ_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "ʧ";
		}

		private void IPA_ʃ_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "ʃ";
		}

		private void IPA_ŋ_Click(object sender, EventArgs e)
		{
			사전페이지_사전발음기호.SelectedText = "ŋ";
		}

		private void 입력_2형식으로잘쓰이지않음_Click(object sender, EventArgs e)
		{
			사전페이지_사전의미.SelectedText = "2형식으로잘쓰이지않음";
		}

		private void 새로만들기ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			타이틀바텍스트애니메이션();
		}

		private void menuStrip1_MouseMove(object sender, MouseEventArgs e)
		{
			타이틀바텍스트애니메이션();
		}


		// 화면 크기를 바꿨을 때, (최대화 크기 누를 때는 호출되지 않는다. 윈도우를 움직일 때도 호출된다.)
		private void Form1_ResizeEnd(object sender, EventArgs e)
		{
			if (_현재_선택한_탭 == "그래픽탭")
			{
				_처음그리거나바뀌어야할때 = true;
				동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
                    ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
				그림위치변경_최근();
			}
		}
		FormWindowState LastWindowState = FormWindowState.Minimized;
		private void Form1_Resize(object sender, EventArgs e)
		{
			if (_현재_선택한_탭 == "그래픽탭")
			{
				// When window state changes
				if (WindowState != LastWindowState)
				{
					LastWindowState = WindowState;

					if (WindowState == FormWindowState.Maximized)
					{
						_처음그리거나바뀌어야할때 = true;
						동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
                            ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
						그림위치변경_최근();
					}
					if (WindowState == FormWindowState.Normal)
					{
						_처음그리거나바뀌어야할때 = true;
						동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
                            ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
						그림위치변경_최근();
					}
				}
			}
		}

		private void 변형지문_MouseMove(object sender, MouseEventArgs e)
		{
			현재마우스위치 = new Point(e.X, e.Y);
			if (이전마우스위치 != 현재마우스위치)
			{

				string 현재어절 = GetWord(변형지문.Text, 변형지문.GetCharIndexFromPosition(e.Location)).Trim().불필요제거();
				string 표제어 = "";

				사전의미.Text = _검색.영한사전_어떻게든_결과를_내는(현재어절, ref 표제어);
				사전표제어.Text = 표제어;

				string 사전발음기호s = _검색.영한발음기호사전(표제어);

				if (사전발음기호s != "")
					사전발음기호.Text = "[" + 사전발음기호s + "]";
				else
					사전발음기호.Text = "";


				이전마우스위치 = 현재마우스위치;
			}
		}

		private List<string> _모르는단어리스트 = new List<string>();
		private List<string> _모르는단어리스트_3번이하로_나온단어들 = new List<string>(); // 


		private string _현재학생정보파일경로 = "";
		public string _학생정보파일들폴더경로 = "";

		private void 독심술ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_모르는단어리스트.Clear();
			_모르는단어리스트_3번이하로_나온단어들.Clear();

			System.Windows.Forms.OpenFileDialog 파일열기다이얼로그 = new System.Windows.Forms.OpenFileDialog();
			파일열기다이얼로그.Filter = "nf 파일(*.nf)|*.nf;";

			// 학생 정보파일이 저장 위치에 있는지 확인해보고, 없으면, 학생 정보 파일을 저장합니다.
			// 


			if(System.IO.File.Exists(_DB루트 + "학생정보파일위치.txt"))
			{
				파일열기다이얼로그.InitialDirectory = UTF8파일.문자열로(_DB루트 + "학생정보파일위치.txt");
			}
			

			파일열기다이얼로그.Title = "독심술의 제물이 될 학생을 고르세요.";

			if (파일열기다이얼로그.ShowDialog() == DialogResult.OK)
			{

				_학생정보파일들폴더경로 = Path.GetDirectoryName(파일열기다이얼로그.FileName);
				_현재학생정보파일경로 = 파일열기다이얼로그.FileName;

				문자열.UTF8파일로(_학생정보파일들폴더경로, _DB루트 + "학생정보파일위치.txt");

				독심술(파일열기다이얼로그.FileName, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들);

				if (_현재_선택한_탭 == "그래픽탭")
				{
					_처음그리거나바뀌어야할때 = true;
					동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
                        ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
					그림위치변경_최근();
				}
			}
		}

		private List<string> _단어제외한_사용자파일문자열들 = new List<string>();
		public List<string> _사용자단어파일_문자열들 = new List<string>();


		public void 독심술()
		{
			독심술(_현재학생정보파일경로, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들);
		}

		private void 독심술(string 파일경로, ref List<string> 모르는단어리스트, ref List<string> 모르는단어리스트_3번이하로_나온단어들)
		{
			모르는단어리스트.Clear();
			모르는단어리스트_3번이하로_나온단어들.Clear();

			_단어제외한_사용자파일문자열들.Clear();
			_사용자단어파일_문자열들.Clear();

			List<string> 텍스트파일의_문자열들 = new List<string>();

			string 현재문자열 = "";
			// 2. 워드부분이 있는지 확인한다.
			bool 워드존재여부 = false;
			bool 워드종료여부 = false;

			변환.텍스트파일.문자열들로(파일경로, ref 텍스트파일의_문자열들);

			for (int i = 0; i < 텍스트파일의_문자열들.Count; i++)
			{
				현재문자열 = 텍스트파일의_문자열들[i];

				if (현재문자열.Contains("<word>"))
				{
					워드존재여부 = true;
				}
				else if (현재문자열.Contains("</word>"))
				{
					워드종료여부 = true;
				}
				else if (워드존재여부 == false || 워드종료여부 == true)
				{
					_단어제외한_사용자파일문자열들.Add(현재문자열);
				}
				else if (워드존재여부 && !워드종료여부)
				{
					_사용자단어파일_문자열들.Add(현재문자열);

					if (현재문자열.Contains("@"))
					{
						현재문자열에서모르는단어추출하여추가(현재문자열, ref 모르는단어리스트);
						if (현재문자열.StartsWith("@0000)") || 현재문자열.StartsWith("@0001)") || 현재문자열.StartsWith("@0002)"))
						{
							현재문자열에서모르는단어추출하여추가(현재문자열, ref 모르는단어리스트_3번이하로_나온단어들);
						}
					}
				}
			}
		}

		private void 현재문자열에서모르는단어추출하여추가(string 현재문자열, ref List<string> 모르는단어리스트)
		{
			bool 기록시작 = false;
			string 현재모르는단어후보 = "";

			// 현재 문자열의 숫자 확인

			// 한 글자씩 확인
			for(int i = 0; i < 현재문자열.Length; i++)
			{
				if (현재문자열[i] == ')') 기록시작 = true;
				else if (현재문자열[i] == '(')
				{
					기록시작 = false;
					모르는단어리스트.Add(현재모르는단어후보.Replace(":", ""));

					현재모르는단어후보 = "";

				}
				else if (기록시작 == true) { 현재모르는단어후보 += 현재문자열[i]; }
			}
		}

		private void 동그라미지우기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (본문.Focused)
			{
				현재내용을_실행취소용_클립보드에_저장();
				_텍스트변경사항자동저장불필요 = true;

				string 선택내용 = 본문.SelectedText;

				선택내용 = 선택내용.Replace("①", "");
				선택내용 = 선택내용.Replace("②", "");
				선택내용 = 선택내용.Replace("③", "");
				선택내용 = 선택내용.Replace("④", "");
				선택내용 = 선택내용.Replace("⑤", "");
				선택내용 = 선택내용.Replace("⑥", "");
				선택내용 = 선택내용.Replace("⑦", "");
				선택내용 = 선택내용.Replace("⑧", "");
				선택내용 = 선택내용.Replace("⑨", "");
				선택내용 = 선택내용.Replace("⑩", "");


				본문.SelectedText = 선택내용;

				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();
			}
			else if (해석.Focused)
			{
				현재내용을_실행취소용_클립보드에_저장();
				_텍스트변경사항자동저장불필요 = true;

				string 선택내용 = 해석.SelectedText;

				선택내용 = 선택내용.Replace("①", "");
				선택내용 = 선택내용.Replace("②", "");
				선택내용 = 선택내용.Replace("③", "");
				선택내용 = 선택내용.Replace("④", "");
				선택내용 = 선택내용.Replace("⑤", "");
				선택내용 = 선택내용.Replace("⑥", "");
				선택내용 = 선택내용.Replace("⑦", "");
				선택내용 = 선택내용.Replace("⑧", "");
				선택내용 = 선택내용.Replace("⑨", "");
				선택내용 = 선택내용.Replace("⑩", "");

				해석.SelectedText = 선택내용;

				_텍스트변경사항자동저장불필요 = false;
				현재내용을_실행취소용_클립보드에_저장();
			}
		}

		// 그래픽 화면에서의 오른쪽 클릭, 모르는 단어를 진하게 해주는 역할을 한다.
		private void 픽쳐박스1_MouseUp(object sender, MouseEventArgs e)
		{
			bool 처리여부 = false;
			if(e.Button == System.Windows.Forms.MouseButtons.Right)
			{

				if (_현재학생정보파일경로 == "")
				{
					//메시지박스.보여주기("학생 정보 파일을 추가하세요!", this);

					_모르는단어리스트.Clear();

					System.Windows.Forms.OpenFileDialog 파일열기다이얼로그 = new System.Windows.Forms.OpenFileDialog();
					파일열기다이얼로그.Filter = "nf 파일(*.nf)|*.nf;";

					// 학생 정보파일이 저장 위치에 있는지 확인해보고, 없으면, 학생 정보 파일을 저장합니다.
					// 

					if (System.IO.File.Exists(_DB루트 + "학생정보파일위치.txt"))
					{
						파일열기다이얼로그.InitialDirectory = UTF8파일.문자열로(_DB루트 + "학생정보파일위치.txt");
					}

					파일열기다이얼로그.Title = "먼저 독심술의 제물이 될 학생을 고르세요.";

					if (파일열기다이얼로그.ShowDialog() == DialogResult.OK)
					{

						_학생정보파일들폴더경로 = Path.GetDirectoryName(파일열기다이얼로그.FileName);
						_현재학생정보파일경로 = 파일열기다이얼로그.FileName;

						문자열.UTF8파일로(_학생정보파일들폴더경로, _DB루트 + "학생정보파일위치.txt");

						독심술(파일열기다이얼로그.FileName, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들);

						if (_현재_선택한_탭 == "그래픽탭")
						{
							_처음그리거나바뀌어야할때 = true;
							동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text, 보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text, 
                                ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);
							그림위치변경_최근();
						}
					}
				}
				else
				{
                    string 현재어절 = _현재어절.불필요제거().ToLower();//.Replace("'ll", "").Replace("'s", "").Replace("'d", "").ToLower();

                    if (!현재어절.StartsWith("'ll")) 현재어절 = 현재어절.Replace("'ll", "");
                    if (!현재어절.StartsWith("'s")) 현재어절 = 현재어절.Replace("'s", "");
                    if (!현재어절.StartsWith("'d")) 현재어절 = 현재어절.Replace("'d", "");

                    if (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1); // 따옴표로 둘러 쌓여 있는 경우,
					if (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1); // 따옴표로 둘러 쌓여 있는 경우,

					for (int i = 0; i < _사용자단어파일_문자열들.Count; i++)
					{
						if(_사용자단어파일_문자열들[i].Contains(")" + 현재어절 + "(") || _사용자단어파일_문자열들[i].Contains(":" + 현재어절 + "("))
						{
							if (_사용자단어파일_문자열들[i].Contains("#")) { _사용자단어파일_문자열들[i] = "@" + _사용자단어파일_문자열들[i].Replace("#", ""); }
							else if (_사용자단어파일_문자열들[i].Contains("@")) { _사용자단어파일_문자열들[i] = _사용자단어파일_문자열들[i].Replace("@", ""); }
							else { _사용자단어파일_문자열들[i] = "@" + _사용자단어파일_문자열들[i]; }

							처리여부 = true;
						}
						
					}

					if (처리여부)
					{
						//메시지박스.보여주기(_현재본문어절, this);
						학생정보저장();

						// 다시 독심술 사용하기
						독심술(_현재학생정보파일경로, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들);

						화면업데이트중지();


						동영상용화면.만들기(ref _사용자단어파일_문자열들, ref _모르는단어리스트, ref _모르는단어리스트_3번이하로_나온단어들, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text,
															보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, 주관식정답.Text, 해석.Text, 힌트.Text, 중요어휘.Text,
															ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, 픽쳐박스1.Width, 픽쳐박스1.Height, ref _배경과본문_비트맵, ref _배경과본문_그래픽);

						동영상용화면.마우스따라_다시만들기(_현재i, _현재j, _현재본문번호, ref _모르는단어리스트, 제목.Text, 질문.Text, 본문.Text, ABC.Text, 보기1Text.Text, 보기2Text.Text, 보기3Text.Text, 보기4Text.Text, 보기5Text.Text,
												보기1.Checked, 보기2.Checked, 보기3.Checked, 보기4.Checked, 보기5.Checked, ref _본문_여러줄, ref _A1_여러줄, ref _A2_여러줄, ref _A3_여러줄, ref _A4_여러줄, ref _A5_여러줄, ref _배경과본문_그래픽);

						그림위치변경_최근();

						화면업데이트재개();
					}
					else
					{
						// 사전에 추가할 내용을 찾는다.
						// 원형을 찾는다.

						// 아예 원형도 없을 것이 확실한 경우이거나, 그 자체가 원형인 것이 거의 확실한 경우

						WordStat단어통계추가 단어통계추가다이얼로그 = new WordStat단어통계추가();

						string 줄여가며찾을단어;

						줄여가며찾을단어 = 현재어절;

						do
						{
							줄여가며찾을단어 = 줄여가며찾을단어.Substring(0, 줄여가며찾을단어.Length - 1);
						}
						while (!찾기(줄여가며찾을단어, 현재어절));

						단어통계추가다이얼로그.찾을단어세팅(줄여가며찾을단어, 현재어절);

						//단어통계추가다이얼로그.모르는단어목록세팅(ref _단어_사용자파일문자열들);

						단어통계추가다이얼로그.ShowDialog(this);


						

						
						// 우선은 수동 검색 창을 만들고,

						//메시지박스.보여주기("업그레이드가 필요합니다.", this);


					}

				}
			}
		}

		private bool 찾기(string 찾을단어, string 원래찾을단어)
		{
			for (int i = 0; i < _사용자단어파일_문자열들.Count; i++)
			{
				if (_사용자단어파일_문자열들[i].Contains(")" + 찾을단어) || _사용자단어파일_문자열들[i].Contains(":" + 찾을단어)) return true;
			}

			return false;
		}

		List<string> _새롭게저장할_사용자파일_문자열들 = new List<string>();


		private void 학생정보저장()
		{
			_새롭게저장할_사용자파일_문자열들.Clear();

			for (int i = 0; i < _단어제외한_사용자파일문자열들.Count; i++)
			{	
				if(!string.IsNullOrEmpty(_단어제외한_사용자파일문자열들[i]))
					_새롭게저장할_사용자파일_문자열들.Add(_단어제외한_사용자파일문자열들[i]);
			}

			_새롭게저장할_사용자파일_문자열들.Add("<word>");

			for (int i = 0; i < _사용자단어파일_문자열들.Count; i++)
			{
				if (!string.IsNullOrEmpty(_사용자단어파일_문자열들[i]))
					_새롭게저장할_사용자파일_문자열들.Add(_사용자단어파일_문자열들[i]);
			}

			_새롭게저장할_사용자파일_문자열들.Add("</word>");

			문자열들.UTF8파일로(_새롭게저장할_사용자파일_문자열들, _현재학생정보파일경로);

		}

		private void 픽쳐박스1_Click(object sender, EventArgs e)
		{

		}

		private void 단어장클립보드에추가하기ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(_현재학생정보파일경로 == "")
			{
				메시지박스.보여주기("학생 정보 파일을 추가하세요!", this);

				return;
			}



			string 클립보드 = "";
			string 현재단어, 현재발음기호;

			List<string> 어절들 = new List<string>();
			List<string> 현재지문모르는단어리스트 = new List<string>();

			변환.문자열.어절들로(본문.Text.불필요제거_사전용().ToLower().Replace("'ll", "").Replace("'s", "").Replace("'d", ""), ref 어절들);

			for (int i = 0; i < _모르는단어리스트.Count; i++)
			{
				for (int j = 0; j < 어절들.Count; j++)
				{
					if (_모르는단어리스트[i] == 어절들[j])
					{
						if (!현재지문모르는단어리스트.Contains(_모르는단어리스트[i]))
							현재지문모르는단어리스트.Add(_모르는단어리스트[i]);
					}
				}
			}

			if (현재지문모르는단어리스트.Count == 0)
			{
				메시지박스.보여주기("현재 지문에는 모르는 단어가 없습니다!", this);

				return;
			}

			for (int i = 0; i < 현재지문모르는단어리스트.Count; i++)
			{
				현재단어 = 현재지문모르는단어리스트[i];
				클립보드 += 현재단어 + "\n";

				현재발음기호 = _검색.영한발음기호사전(현재단어);
				if(현재발음기호 != "")
					클립보드 += "[" + _검색.영한발음기호사전(현재단어) + "]" + "\n";
					
				클립보드 += _검색.영한사전(현재단어) +"\n";

				클립보드 += "\n";
			}

			Clipboard.SetText(클립보드);
		}

        private void 수능단어통계업데이트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 0. 관리자용이므로 클릭하지 말라는 메시지를 띄웁니다.
            if(MessageBox.Show("이는 관리자용 메뉴이므로 절대로 클릭해서는 안됩니다.", "관리자용 메뉴 경고 메시지", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                return;
            }

            if(!수능라디오버튼.Checked)
            {
                MessageBox.Show("당신은 아직 이 버튼을 누를 준비가 되지 않았습니다.");
                return;
            }


            if(_학생정보파일들폴더경로 == "")
            {
                MessageBox.Show("학생정보파일폴더의 경로를 지정하셔요. 독심술을 한 번만 사용하면 이 메시지가 뜨지 않습니다.");
                return;
            }

            // 1. 모든 WordStat 데이터와, 학생의 단어 정보 데이터를 로드합니다.

            string[] files = Directory.GetFiles(_학생정보파일들폴더경로);

            List<string> 업데이트어절들 = new List<string>();

            for (int i = 0; i < _CAKE들.Count(); i++)
            {
                List<string> 현재CAKE어절들 = new List<string>();

                string 본문텍스트 = 변환.문자열.T태그내용(_CAKE들[i]).불필요제거();

                변환.문자열.어절들로(본문텍스트, ref 현재CAKE어절들);

                for(int j = 0; j < 현재CAKE어절들.Count(); j++)
                {
                    업데이트어절들.Add(현재CAKE어절들[j].ToLower());
                }
            }
            업데이트어절들.Sort();

            List<string> 업데이트어절중복제거 = new List<string>();
            List<int> 업데이트어절중복숫자 = new List<int>();

            string 이전업데이트어절 = "";
            int 중복숫자 = 0;

            for (int i = 0; i < 업데이트어절들.Count; i++)
            {
                string 현재업데이트어절 = 업데이트어절들[i];


                if(i == 업데이트어절들.Count - 1)
                {
                    if(이전업데이트어절 != 현재업데이트어절)
                    {
                        업데이트어절중복제거.Add(이전업데이트어절);
                        업데이트어절중복숫자.Add(중복숫자);

                        업데이트어절중복제거.Add(현재업데이트어절);
                        업데이트어절중복숫자.Add(1);
                    }
                    else
                    {
                        업데이트어절중복제거.Add(현재업데이트어절);
                        업데이트어절중복숫자.Add(중복숫자 + 1);
                    }
                }
                else if(이전업데이트어절 == "")
                {
                    중복숫자 = 1;
                }
                else if(이전업데이트어절 != 현재업데이트어절 && 이전업데이트어절 != "")
                {
                    업데이트어절중복제거.Add(이전업데이트어절);
                    업데이트어절중복숫자.Add(중복숫자);
                    중복숫자 = 1;
                }
                else if(이전업데이트어절 == 현재업데이트어절)
                {
                    중복숫자++;
                }

                이전업데이트어절 = 현재업데이트어절;
            }


            foreach (String 현재학생단어파일 in files)
            {
                if (현재학생단어파일.EndsWith(".nf"))
                {
                    단어정보파일에빈도수업데이트하기(현재학생단어파일, ref 업데이트어절중복제거, ref 업데이트어절중복숫자);
                }
            }

            단어정보파일에빈도수업데이트하기(_학생정보파일들폴더경로 + "\\txt\\WordStat.txt", ref 업데이트어절중복제거, ref 업데이트어절중복숫자);

            // 1. 현재 문제의 모든 지문을 모두 로드하고, 각각의 단어를 모두 로드합니다. 

            // 1.1 각각의 지문에서 어절들을 모두 불러옵니다.

            // 1.2 불러온 어절에 대해서, 숫자를 하나씩 업데이트 합니다.
            MessageBox.Show("Done");
        }

        private void 단어정보파일에빈도수업데이트하기(string 파일경로, ref List<string> 업데이트어절중복제거, ref List<int> 업데이트어절중복숫자)
        {
            // 우선 파일경로에 있는 문서를 메모리에 올립니다.
            List<string> 학생정보파일의문자열들 = new List<string>();
            변환.텍스트파일.문자열들로(파일경로, ref 학생정보파일의문자열들);

            for(int i = 0; i < 학생정보파일의문자열들.Count(); i++)
            {
                string 현재라인 = 학생정보파일의문자열들[i];

                for(int j = 0; j < 업데이트어절중복제거.Count(); j++)
                {


                    string 현재업데이트어절 = 업데이트어절중복제거[j].불필요제거(); //.Replace("'ll", "").Replace("'s", "").Replace("'d", "").ToLower();
                    if (!현재업데이트어절.StartsWith("'ll")) 현재업데이트어절 = 현재업데이트어절.Replace("'ll", "");
                    if (!현재업데이트어절.StartsWith("'s")) 현재업데이트어절 = 현재업데이트어절.Replace("'s", "");
                    if (!현재업데이트어절.StartsWith("'d")) 현재업데이트어절 = 현재업데이트어절.Replace("'d", "");

                    if (현재업데이트어절.EndsWith("\'")) 현재업데이트어절 = 현재업데이트어절.Left(현재업데이트어절.Length - 1); // 따옴표로 둘러 쌓여 있는 경우,
                    if (현재업데이트어절.StartsWith("\'")) 현재업데이트어절 = 현재업데이트어절.Right(현재업데이트어절.Length - 1); // 따옴표로 둘러 쌓여 있는 경우,

                    // 현재라인에서 업데이트할 어절들이 있는지를 찾는다.
                    // 만약 업데이트할 어절이 있다면, 그 뒤의 숫자를 하나 올려주고, 맨 앞의 숫자도 하나 올려준다.

                    //#0001)dispute(0)::disputes(1)
                    bool 샵이나앳이있음 = false;

                    if ((현재라인.Contains(")" + 현재업데이트어절 + "(") || 현재라인.Contains(":" + 현재업데이트어절 + "(")) && 현재업데이트어절.Length > 1 && !현재라인.StartsWith("Q"))
                    {
                        string 고친문자열 = "";
                        if(현재라인.StartsWith("#") || 현재라인.StartsWith("@"))
                        {
                            string s현재라인숫자 = 현재라인.Substring(1, 4);
                            int 현재라인숫자 = int.Parse(s현재라인숫자);
                            현재라인숫자 += 업데이트어절중복숫자[j];

                            if (현재라인숫자 > 9999) 현재라인숫자 = 9999;

                            if(현재라인.StartsWith("#"))
                            {
                                고친문자열 += "#";
                            }
                            else if(현재라인.StartsWith("@"))
                            {
                                고친문자열 += "@";
                            }

                            고친문자열 += String.Format("{0:D4}", 현재라인숫자);

                            샵이나앳이있음 = true;

                        }
                        else
                        {
                            string s현재라인숫자 = 현재라인.Substring(0, 4);
                            int 현재라인숫자 = int.Parse(s현재라인숫자);
                            현재라인숫자 +=  업데이트어절중복숫자[j];

                            if (현재라인숫자 > 9999) 현재라인숫자 = 9999;

                            고친문자열 += String.Format("{0:D4}", 현재라인숫자);

                            샵이나앳이있음 = false;
                        }

                        int 숫자시작위치 = 0;
                        if(현재라인.Contains(")" + 현재업데이트어절 + "("))
                        {
                            숫자시작위치 = 현재라인.IndexOf(")" + 현재업데이트어절 + "(") + 2 + 현재업데이트어절.Length;
                        }
                        else if(현재라인.Contains(":" + 현재업데이트어절 + "("))
                        {
                            숫자시작위치 = 현재라인.IndexOf(":" + 현재업데이트어절 + "(") + 2 + 현재업데이트어절.Length;
                        }

                        string s숫자 = 현재라인.Substring(숫자시작위치);

                        int 숫자끝위치 = s숫자.IndexOf(")");
                        s숫자 = s숫자.Substring(0, 숫자끝위치);
                        int 숫자 = int.Parse(s숫자);
                        숫자 += 업데이트어절중복숫자[j];
                        if (숫자 > 9999) 숫자 = 9999;


                        if (샵이나앳이있음)
                            고친문자열 += 현재라인.Substring(5, 숫자시작위치 - 5) + String.Format("{0}", 숫자) + 현재라인.Substring(숫자시작위치 + s숫자.Length) ;
                        else
                            고친문자열 += 현재라인.Substring(4, 숫자시작위치 - 4) + String.Format("{0}", 숫자) + 현재라인.Substring(숫자시작위치 + s숫자.Length);

                        //MessageBox.Show(고친문자열);

                        현재라인 = 고친문자열;

                        학생정보파일의문자열들[i] = 현재라인;
                    }
                }
            }

            변환.문자열들.UTF8파일로(학생정보파일의문자열들, 파일경로.Replace(".nf", "up.nf").Replace(".txt", "up.txt"));
        }

        private void 수능핵심_Click(object sender, EventArgs e)
        {
            사전페이지_사전의미.SelectedText = "{수능핵심}";
        }
    }



    [StructLayout(LayoutKind.Sequential)]
	public struct PARAFORMAT
	{
		public int cbSize;
		public uint dwMask;
		public short wNumbering;
		public short wReserved;
		public int dxStartIndent;
		public int dxRightIndent;
		public int dxOffset;
		public short wAlignment;
		public short cTabCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public int[] rgxTabs;
		// PARAFORMAT2 from here onwards
		public int dySpaceBefore;
		public int dySpaceAfter;
		public int dyLineSpacing;
		public short sStyle;
		public byte bLineSpacingRule;
		public byte bOutlineLevel;
		public short wShadingWeight;
		public short wShadingStyle;
		public short wNumberingStart;
		public short wNumberingStyle;
		public short wNumberingTab;
		public short wBorderSpace;
		public short wBorderWidth;
		public short wBorders;
	}
}