// 당연히 그 펜은 로봇이 집어갔지.
// 이 프로그램의 전신은 2005년으로 거슬러 올라간다.
/////////////////////////////////////////////////////////////////

// 2005 / 4 / 20 김용남
// KingOfWord.h: 수능에 중요한 단어를 자동으로 추출하는 클래스.

// 2017 / 11 / 23 : 12년 전에는 결코 할 수 없었던 것을 이제야 만든다.
/////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using 변환;

namespace 편집기의_제왕
{
	class 여기_있던_펜이_어디_갔지
	{
		#region 멤버변수들
		private bool _상태_보기 = false; private bool _상태_A태그; private bool _상태_A태그에서페이지나뉨 = false; private bool _상태_A0사용한_보기 = false;  // 칸이 딱딱 매겨짐
		private bool _상태_지문;
		private bool _상태_해설;
		private bool _상태_CAKE;              // CAKE 태그의 시작과 끝을 나타냄

		private bool _상태_개행추가;          // Count함수에서, 태그만을 처리한 경우, 한 줄로의 가치가 없기 때문에, 띄어쓰기가 불필요하다.

		private bool _한줄전_상태_지문;


		public bool _옵션_정답뒤로;
		public bool _옵션_모니터에서보기;
		public bool _옵션_우등생 = false;
		public bool _옵션_단어난이도표시 = false;
		public bool _옵션_정답표시;
		public bool _옵션_의미자동보기;           // html에서 자동으로 뜻이 표시되도록 하는 옵션임



		private int LineLimit;


		private int pageNum;
		private int _점수_총합;
		private int _점수_총합_Google;
		private int _점수_총합_Empas;
		private int _점수_총합_Words;
		private int _점수_총합_DifficultWords;
		private int _nWordPosition;                 // 단어가 문장의 처음인지 아닌지 확인하게 해줌.
													// 문장의 처음이 아니고 대문자이면, 고유명사 처리하는데 사용

		private string m_PathFolder;
		private string m_fileName = "";
		private string m_studentName = "";
		//CString m_wordbookView;
		//CStringArray _saWord_Book_Manual_Edit;			//클릭하면 단어를 수정할 수 있는 왼쪽 창에 나타나는 리스트이다.
		//CStringArray _saWord_Book_Manual_Edit_tmp;		// CAKE 단위로 페이지를 나누는 기법을 쓰기 때문에 한줄을 두번이상 처리할 경우가 있다. 이 때문에 첫번째로 처리한 것만 단어장에 넣기 위해서 사용한다.

		public static List<string> _1순위단어장;
		public static List<string> _2순위단어장;

		public static List<string> m_saWordBookNotInDict;
		public static List<string> m_saCurrentWordBook;
		private bool bTagBegin;

		#endregion

		public 여기_있던_펜이_어디_갔지()
		{
			_1순위단어장 = new List<string>();
			_2순위단어장 = new List<string>();

			m_saWordBookNotInDict = new List<string>();
			m_saCurrentWordBook = new List<string>();
		}

		private void 변수초기화()
		{
			_1순위단어장.Clear();
			_2순위단어장.Clear();

			m_saWordBookNotInDict.Clear();
			m_saCurrentWordBook.Clear();

			_상태_지문 = false;
			_상태_해설 = false;
			_한줄전_상태_지문 = false;


			_점수_총합_Google = 0;
			_점수_총합_Empas = 0;
			_점수_총합_Words = 0;
			_점수_총합_DifficultWords = 0;

			pageNum = 0;

			bTagBegin = false;

			if (_옵션_모니터에서보기 == true) LineLimit = 24;
			else LineLimit = 47;

		}


		// return wordbook page의 줄 수.
		// wordbook = 만들어진 워드북의 내용.
		private int MakeWordBookPage(ref string wordbook)
		{
			int nRet;
			string wordbookTmp = "";

			// 1. 적어야 할 내용의 단어가 있어야 단어장을 시작함.
			if ((_1순위단어장.Count != 0) || (_2순위단어장.Count != 0))
			{
				wordbookTmp += "----------------------\r\n";
			}

			// 2. 수능빈출 단어
			if (_1순위단어장.Count != 0)
			{
				for (int i = 0; i < _1순위단어장.Count; i++) wordbookTmp += _1순위단어장[i] + "\r\n";
			}

			if ((_1순위단어장.Count != 0) && (_2순위단어장.Count != 0))
			{
				wordbookTmp += "----------------------\r\n";
			}

			// 3. 기타 단어.
			if (_2순위단어장.Count != 0)
			{
				for (int i = 0; i < _2순위단어장.Count; i++) wordbookTmp += _2순위단어장[i] + "\r\n";
			}

			List<string> 단어장문자열들 = new List<string>();

			문자열.개행문자로_구분한_문자열들로(wordbookTmp, ref 단어장문자열들);

			nRet = 단어장문자열들.Count;

			wordbook = wordbookTmp;

			return nRet;
		}

		public string Pdf페이지만들기(string src, bool wordbook)
		{
			string HTML결과물;
			string 한줄전_만들어진내용 = "", 한줄전_만들어진_단어장 = ""; int 한줄전_만들어진_단어장줄수 = 0;
			string 현재까지만들어진CAKE단위로딱떨어지는페이지 = ""; int 현재까지만들어진CAKE단위로딱떨어지는페이지_그때의줄번호 = 0;
			string 현재까지만들어본_컨텐츠 = "", 현재까지만들어본_단어장 = "";


			_상태_A태그에서페이지나뉨 = false;

			변수초기화();                                        // 1. 출력 파일의 헤더를 만든다.

			src = 문자열.문법문제표지제거(src);

			src = 문자열.적정길이에서줄바꿈한문자열로(src, 55);

			List<string> 너비가_일정한_문자열들 = new List<string>();
			문자열.개행문자로_구분한_문자열들로(src, ref 너비가_일정한_문자열들);

			for (int i = 0; i < 너비가_일정한_문자열들.Count; i++)
			{
				string CurLine = 너비가_일정한_문자열들[i];           // 전처리가 필요하면 여기서 해도 된다.

				한줄전_만들어진내용 = 현재까지만들어본_컨텐츠;
				한줄전_만들어진_단어장줄수 = MakeWordBookPage(ref 한줄전_만들어진_단어장);
				_한줄전_상태_지문 = _상태_지문;

				if (CurLine == "") 현재까지만들어본_컨텐츠 += "\r\n";
				//		else if(CurLine.Find("그림입력|"		) != -1)								현재까지만들어본_컨텐츠 += LINE_PIC(CurLine, src);
				else if (CurLine.Contains("<TBAR></TBAR>")) 현재까지만들어본_컨텐츠 += "----------------------\r\n";

				// else if(CurLine.Find("<Q>"				) != -1)							현재까지만들어본_컨텐츠 += LINE_Q(CurLine);
				// else if(_상태_해설)															현재까지만들어본_컨텐츠 += LINE_TR(CurLine);	// 해석이나 한글 텍스트는 처리하지 않는다.
				else 현재까지만들어본_컨텐츠 += LINE_WORD_핵심(CurLine);


			}
			//

			return "미완성";
		}


		private string LINE_WORD_핵심(string CurLine)
		{
			string HTML결과 = "";

			List<string> wordArray = new List<string>(); 문자열.어절들로(CurLine, ref wordArray);

			for (int j = 0; j < wordArray.Count; j++)
			{
				HTML결과 += WORD_(wordArray[j], false);
			}

			return HTML결과;
		}

		//…………………………………………………………………………………
		// 입력
		// 출력
		//…………………………………………………………………………………
		private string WORD_(string src, bool bFixedWidth)
		{
			//	if(		WORD_CheckTags(src)){					return WORD_Tag(src, bFixedWidth);	}
			//	else if(WORD_CheckMath(src)){					return WORD_Pass(src);				}
			//	else if(WORD_Check1Ltr(src)){					return WORD_Pass(src);				}
			//	else if(WORD_CheckCaps(src)){					return WORD_Pass(src);				}
			//	else if(WORD_CheckDbls(src)){					return WORD_Dbls(src);				}
			//	else						{					return WORD_Txt(src);				}

			//	return WORD_Txt(src);
			return "미완성";
		}

		#region 멤버변수들
		public static string _IMG루트폴더 = "C:/Users/Administrator/Google 드라이브/편집기의제왕img/";

		public static int _가로픽셀;
		public static int _세로픽셀;

		public static Pen _기본펜;
		public static Pen _보기펜;
		public static Pen _밑줄펜;
		public static Pen _취소펜;

		public static Font _Q글꼴;

		public static Font _기본글꼴;
		public static Font _보기글꼴;
		public static Font _정답글꼴;

		public static Font _격자글꼴;
		public static Font _문법표지글꼴;
		public static Font _어휘문제글꼴;
		public static Font _어법문제글꼴;

		public static SolidBrush _Q붓;
		public static SolidBrush _보기붓;

		public static SolidBrush _기본붓;
		public static SolidBrush _격자붓;
		public static SolidBrush _정답붓;

		public static SolidBrush _문법표지붓;
		public static SolidBrush _어휘문제붓;
		public static SolidBrush _어휘문제배경;

		public static SolidBrush _어법문제붓;
		public static SolidBrush _어법문제배경;

		public static PointF _기본좌표;

		public static List<한줄> _Q_여러줄;


		public static List<한줄> _A0_여러줄;
		public static List<한줄> _A1_여러줄;
		public static List<한줄> _A2_여러줄;
		public static List<한줄> _A3_여러줄;
		public static List<한줄> _A4_여러줄;
		public static List<한줄> _A5_여러줄;

		public static List<string> _어휘문제들;
		public static List<float> _x어휘문제좌표;
		public static List<float> _y어휘문제좌표;

		public static List<string> _어법문제들;
		public static List<float> _x어법문제좌표;
		public static List<float> _y어법문제좌표;



		public static List<문법표지> _문법표지들 = null;
		public static int _문법표지수;
		public static string _선택된배경파일이름;

		public static int _좌여백 = 316;
		public static int _우한계 = 916;
		public static int _좌한계 = 365;
		public static int _우문법기호한계 = 906;
		#endregion

		public static string 만들기(string 제목, string 질문, string 본문, string ABC, string 보기1Text, string 보기2Text, string 보기3Text, string 보기4Text, string 보기5Text,
									bool 보기1, bool 보기2, bool 보기3, bool 보기4, bool 보기5,
									string 주관식정답, string 해석, string 해설, string 중요어휘, ref List<한줄> 본문_여러줄, ref Bitmap 비트맵, ref Graphics 그래픽)
		{
			_어휘문제들 = new List<string>();
			_x어휘문제좌표 = new List<float>();
			_y어휘문제좌표 = new List<float>();

			_어법문제들 = new List<string>();
			_x어법문제좌표 = new List<float>();
			_y어법문제좌표 = new List<float>();


			_Q_여러줄 = new List<한줄>();
			_A0_여러줄 = new List<한줄>();
			_A1_여러줄 = new List<한줄>();
			_A2_여러줄 = new List<한줄>();
			_A3_여러줄 = new List<한줄>();
			_A4_여러줄 = new List<한줄>();
			_A5_여러줄 = new List<한줄>();

			if (_문법표지들 != null) _문법표지들.Clear();
			else _문법표지들 = new List<문법표지>();

			_문법표지수 = 0;
			본문_여러줄.Clear();


			_기본펜 = new Pen(Color.FromArgb(255, 0, 0, 255), 2.5f);
			_보기펜 = new Pen(Color.FromArgb(255, 0, 0, 255), 2.5f);
			_밑줄펜 = new Pen(Color.FromArgb(100, 159, 191, 159), 1f);
			_취소펜 = new Pen(Color.FromArgb(100, 159, 191, 159), 1f);

			/*
			_Q글꼴 = new Font("Youth", 20f);
			_보기글꼴 = new Font("Malgun Gothic", 15f);
			_정답글꼴 = new Font("Malgun Gothic", 15f);


			_기본글꼴 = new Font("Malgun Gothic", 15f);
			_격자글꼴 = new Font("Malgun Gothic", 22.5f);
			_문법표지글꼴 = new Font("Malgun Gothic", 10f);
			_어휘문제글꼴 = new Font("Malgun Gothic", 10f);
			_어법문제글꼴 = new Font("Malgun Gothic", 10f);
			*/
			_Q글꼴 = new Font("Youth", 4.0f);
			_보기글꼴 = new Font("Malgun Gothic", 3.0f);
			_정답글꼴 = new Font("Malgun Gothic", 3.0f);


			_기본글꼴 = new Font("Malgun Gothic", 3.0f);
			_격자글꼴 = new Font("Malgun Gothic", 4.5f);
			_문법표지글꼴 = new Font("Malgun Gothic", 2.0f);
			_어휘문제글꼴 = new Font("Malgun Gothic", 2.0f);
			_어법문제글꼴 = new Font("Malgun Gothic", 2.0f);

			//new SolidBrush( Color.Black);
			//_Q붓 = new SolidBrush(Color.FromArgb(22, 43, 72));
			//_Q붓 = new SolidBrush(Color.FromArgb(254, 72, 25));
			//_Q붓 = new SolidBrush(Color.FromArgb(255, 24, 74));
			_Q붓 = new SolidBrush(Color.FromArgb(0, 14, 28));

			_정답붓 = new SolidBrush(Color.FromArgb(255, 24, 74));

			_보기붓 = new SolidBrush(Color.FromArgb(181, 183, 180));


			_기본붓 = new SolidBrush(Color.FromArgb(0, 0, 0));

			_격자붓 = new SolidBrush(Color.FromArgb(100, 125, 125, 125));
			_문법표지붓 = new SolidBrush(Color.FromArgb(100, 200, 200, 200));
			_어휘문제붓 = new SolidBrush(Color.FromArgb(255, 250, 250, 255));
			_어휘문제배경 = new SolidBrush(Color.FromArgb(125, 163, 172, 198));
			_어법문제붓 = new SolidBrush(Color.FromArgb(255, 255, 250, 250));
			_어법문제배경 = new SolidBrush(Color.FromArgb(125, 237, 208, 221));

			_기본좌표 = new PointF(-6.0f, -15.0f);

			_세로픽셀 = 215;
			//_세로픽셀 = 40;


			if (질문 != "")
				변환_문자열을_여러줄로(질문.Replace("▼", ""), _세로픽셀, ref _Q_여러줄);
			else if (제목 != "")
				변환_문자열을_여러줄로(제목, _세로픽셀, ref _Q_여러줄);


			int Q태그높이 = _Q_여러줄.Count * 35;

			_세로픽셀 += Q태그높이 + 35;

			int 본문시작높이 = _세로픽셀;



			변환_문자열을_여러줄로(본문, _세로픽셀, ref 본문_여러줄);

			_세로픽셀 += 본문_여러줄.Count * 35 + 35;

			변환_문자열을_여러줄로(ABC, _세로픽셀, ref _A0_여러줄); _세로픽셀 += _A0_여러줄.Count * 35;
			변환_문자열을_여러줄로("① " + 보기1Text, _세로픽셀, ref _A1_여러줄); _세로픽셀 += _A1_여러줄.Count * 35;
			변환_문자열을_여러줄로("② " + 보기2Text, _세로픽셀, ref _A2_여러줄); _세로픽셀 += _A2_여러줄.Count * 35;
			변환_문자열을_여러줄로("③ " + 보기3Text, _세로픽셀, ref _A3_여러줄);
			_세로픽셀 += _A3_여러줄.Count * 35;
			변환_문자열을_여러줄로("④ " + 보기4Text, _세로픽셀, ref _A4_여러줄);
			_세로픽셀 += _A4_여러줄.Count * 35;
			변환_문자열을_여러줄로("⑤ " + 보기5Text, _세로픽셀, ref _A5_여러줄);
			_세로픽셀 += _A5_여러줄.Count * 35;

			_세로픽셀 += 35; // 한줄 정도 더 띄어 준다.

			if (본문.Contains("http")) { 본문 = 본문.Substring(본문.IndexOf("\n")); 본문 = 본문.Trim(); }
			// 문법 표지 세팅 때문에 한 번 더 해 줌
			변환_문자열을_여러줄로(본문, 본문시작높이, ref 본문_여러줄);

			_가로픽셀 = 1280; if (_세로픽셀 < 720) _세로픽셀 = 720;

			//비트맵 = new Bitmap(1280, _세로픽셀);
			비트맵 = new Bitmap(2480, 3508); // 이게 A4 사이즈다. 프린트 버전과 화면 버전의 중요한 차이점.
			비트맵.SetResolution(600, 600);
	

			그래픽 = Graphics.FromImage(비트맵);

			배경그림넣기(본문, "", ref 그래픽);

			// 사진들넣기(ref _그래픽);


			글자사각배경넣기(ref 그래픽);
			// 더러운문제경고하기(ref _그래픽, 질문);

			//            외곽테두리넣기(ref _그래픽);


			//엠블럼넣기(ref _그래픽);

			그래픽.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;



			문법표지를그림(ref 그래픽);

			for (int i = 0; i < 본문_여러줄.Count; i++)
			{
				for (int j = 0; j < 본문_여러줄[i]._어절들.Count; j++) { 본문쓰기(본문_여러줄[i]._어절들[j].내용, 본문_여러줄[i]._어절들[j].x글자시작좌표, 본문_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽); }
			}


			취소선을그림(ref 그래픽);

			어휘문제를그림(ref 그래픽);
			어법문제를그림(ref 그래픽);

			중요표지를그림(ref 그래픽);

			if (ABC.Trim() != "")
			{
				for (int i = 0; i < _A0_여러줄.Count; i++)
				{
					for (int j = 0; j < _A0_여러줄[i]._어절들.Count; j++)
					{
						보기쓰기(_A0_여러줄[i]._어절들[j].내용, _A0_여러줄[i]._어절들[j].x글자시작좌표, _A0_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
					}
				}
			}
			#region 보기1
			if (보기1Text.Trim() != "")
			{
				for (int i = 0; i < _A1_여러줄.Count; i++)
				{
					for (int j = 0; j < _A1_여러줄[i]._어절들.Count; j++)
					{
						if (보기1) 정답쓰기(_A1_여러줄[i]._어절들[j].내용, _A1_여러줄[i]._어절들[j].x글자시작좌표, _A1_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
						else 보기쓰기(_A1_여러줄[i]._어절들[j].내용, _A1_여러줄[i]._어절들[j].x글자시작좌표, _A1_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
					}
				}
			}
			#endregion
			#region 보기2
			if (보기2Text.Trim() != "")
			{
				for (int i = 0; i < _A2_여러줄.Count; i++)
				{
					for (int j = 0; j < _A2_여러줄[i]._어절들.Count; j++)
					{
						if (보기2)
							정답쓰기(_A2_여러줄[i]._어절들[j].내용, _A2_여러줄[i]._어절들[j].x글자시작좌표, _A2_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
						else
							보기쓰기(_A2_여러줄[i]._어절들[j].내용, _A2_여러줄[i]._어절들[j].x글자시작좌표, _A2_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
					}
				}
			}
			#endregion
			#region 보기3
			if (보기3Text.Trim() != "")
			{
				for (int i = 0; i < _A3_여러줄.Count; i++)
				{
					for (int j = 0; j < _A3_여러줄[i]._어절들.Count; j++)
					{
						if (보기3)
							정답쓰기(_A3_여러줄[i]._어절들[j].내용, _A3_여러줄[i]._어절들[j].x글자시작좌표, _A3_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
						else
							보기쓰기(_A3_여러줄[i]._어절들[j].내용, _A3_여러줄[i]._어절들[j].x글자시작좌표, _A3_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
					}
				}
			}
			#endregion
			#region 보기4
			if (보기4Text.Trim() != "")
			{
				for (int i = 0; i < _A4_여러줄.Count; i++)
				{
					for (int j = 0; j < _A4_여러줄[i]._어절들.Count; j++)
					{
						if (보기4)
							정답쓰기(_A4_여러줄[i]._어절들[j].내용, _A4_여러줄[i]._어절들[j].x글자시작좌표, _A4_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
						else
							보기쓰기(_A4_여러줄[i]._어절들[j].내용, _A4_여러줄[i]._어절들[j].x글자시작좌표, _A4_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
					}
				}
			}
			#endregion
			#region 보기5
			if (보기5Text.Trim() != "")
			{
				for (int i = 0; i < _A5_여러줄.Count; i++)
				{
					for (int j = 0; j < _A5_여러줄[i]._어절들.Count; j++)
					{
						if (보기5)
							정답쓰기(_A5_여러줄[i]._어절들[j].내용, _A5_여러줄[i]._어절들[j].x글자시작좌표, _A5_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
						else
							보기쓰기(_A5_여러줄[i]._어절들[j].내용, _A5_여러줄[i]._어절들[j].x글자시작좌표, _A5_여러줄[i]._어절들[j].y글자시작좌표, ref 그래픽);
					}
				}
			}
			#endregion


			그래픽.TextRenderingHint = TextRenderingHint.AntiAlias;

			if (질문.Trim() != "") Q쓰기(질문.Trim(), ref 그래픽);
			else if (제목.Trim() != "") Q쓰기(제목.Trim(), ref 그래픽);

			그래픽.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			return _선택된배경파일이름;
		}

		// Image format에 대한 Codec 정보를 가져온다.
		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{

			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}
		private static void 본문쓰기(string 내용, float 가로좌표, float 세로좌표, ref Graphics _그래픽)
		{
			float 더해질_가로좌표 = 0;

			if (내용.Contains("<TBAR></TBAR>"))
			{
				_그래픽.DrawString("--------------------------------------------------------------------", _기본글꼴, _기본붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
			}
			else
			{
				for (int i = 0; i < 내용.Length; i++)
				{

					_그래픽.DrawString(내용.Substring(i, 1), _기본글꼴, _기본붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));


					더해질_가로좌표 += 개별코드에대한너비(내용[i]);
				}
			}
		}
		private static void 정답쓰기(string 내용, float 가로좌표, float 세로좌표, ref Graphics _그래픽)
		{
			float 더해질_가로좌표 = 0;

			for (int i = 0; i < 내용.Length; i++)
			{
				_그래픽.DrawString(내용.Substring(i, 1), _정답글꼴, _정답붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));

				더해질_가로좌표 += 개별코드에대한너비(내용[i]);
			}

		}
		private static void 보기쓰기(string 내용, float 가로좌표, float 세로좌표, ref Graphics _그래픽)
		{
			float 더해질_가로좌표 = 0;

			for (int i = 0; i < 내용.Length; i++)
			{
				_그래픽.DrawString(내용.Substring(i, 1), _보기글꼴, _보기붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));

				더해질_가로좌표 += 개별코드에대한너비(내용[i]);
			}

		}
		private static void Q쓰기(string 내용, ref Graphics _그래픽)
		{
			if (내용.Contains("1형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌")) { 내용 = "1형식 고르기"; goto 마지막부분; }
			if (내용.Contains("1형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌")) { 내용 = "1형식 아닌 것 고르기"; goto 마지막부분; }

			if (내용.Contains("2형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌")) { 내용 = "2형식 고르기"; goto 마지막부분; }
			if (내용.Contains("2형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌")) { 내용 = "2형식 아닌 것 고르기"; goto 마지막부분; }

			if (내용.Contains("3형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌")) { 내용 = "3형식 고르기"; goto 마지막부분; }
			if (내용.Contains("3형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌")) { 내용 = "3형식 아닌 것 고르기"; goto 마지막부분; }

			if (내용.Contains("4형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌")) { 내용 = "4형식 고르기"; goto 마지막부분; }
			if (내용.Contains("4형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌")) { 내용 = "4형식 아닌 것 고르기"; goto 마지막부분; }

			if (내용.Contains("5형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌")) { 내용 = "5형식 고르기"; goto 마지막부분; }
			if (내용.Contains("5형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌")) { 내용 = "5형식 아닌 것 고르기"; goto 마지막부분; }

			if (내용.Contains("빈칸에 적절한 말을") && 내용.Contains("주제를 완성")) { 내용 = "빈칸에 들어갈 주제 완성"; goto 마지막부분; }
			if (내용.Contains("빈칸") && 내용.Contains("어형을 바꾸시오")) { 내용 = "빈칸에 들어갈 어휘"; goto 마지막부분; }


			if (내용.Contains("영작하세요")) { 내용 = "영작(을 가장한 어법)"; goto 마지막부분; }

			if (내용.Contains("일치하지 않는 것")) { 내용 = "일치하지 않는 것"; goto 마지막부분; }
			if (내용.Contains("일치 하지 않는 것")) { 내용 = "일치하지 않는 것"; goto 마지막부분; }

			if (내용.Contains("일치하는 것")) { 내용 = "일치하는 것"; goto 마지막부분; }
			if (내용.Contains("어법상 어색한 것")) { 내용 = "어법상 틀린 것"; goto 마지막부분; }

			if (내용.Contains("어법상 틀린 것")) { 내용 = "어법상 틀린 것"; goto 마지막부분; }
			if (내용.Contains("어법에 맞는 표현")) { 내용 = "어법에 맞는 표현"; goto 마지막부분; }
			if (내용.Contains("어법에 알맞는 표현")) { 내용 = "어법에 맞는 표현"; goto 마지막부분; }

			if (내용.Contains("문맥에 맞는 낱말")) { 내용 = "문맥에 맞는 낱말"; goto 마지막부분; }

			if (내용.Contains("낱말의 쓰임이 적절하지 않은 것은")) { 내용 = "적절하지 않은 낱말"; goto 마지막부분; }
			if (내용.Contains("순서에 맞게 배열한 것")) { 내용 = "순서에 맞게 배열한 것"; goto 마지막부분; }

			if (내용.Contains("나머지 넷과 다른 것")) { 내용 = "나머지 넷과 다른 것"; goto 마지막부분; }
			if (내용.Contains("빈칸에 들어갈 말")) { 내용 = "빈칸에 들어갈 말"; goto 마지막부분; }

			if (내용.Contains("언급하지 않은 것")) { 내용 = "언급하지 않은 것"; goto 마지막부분; }
			if (내용.Contains("언급되지 않은 것")) { 내용 = "언급되지 않은 것"; goto 마지막부분; }

			if (내용.Contains("남자가") && 내용.Contains("선택한")) { 내용 = "남자가 선택한 것"; goto 마지막부분; }
			if (내용.Contains("여자가") && 내용.Contains("선택한")) { 내용 = "여자가 선택한 것"; goto 마지막부분; }
			if (내용.Contains("주어진 문장") && 내용.Contains("들어")) { 내용 = "문장 삽입"; goto 마지막부분; }
			if (내용.Contains("관한 내용으로") && 내용.Contains("적절하지 않은")) { 내용 = "일치하지 않는 것"; goto 마지막부분; }

			if (내용.Contains("요약")) { 내용 = "문장 요약"; goto 마지막부분; }
			if (내용.Contains("본문 정리")) { 내용 = "본문"; goto 마지막부분; }
			if (내용.Contains("본문정리")) { 내용 = "본문"; goto 마지막부분; }

			내용 = 내용.Replace("10. ", ""); 내용 = 내용.Replace("11. ", ""); 내용 = 내용.Replace("12. ", ""); 내용 = 내용.Replace("13. ", ""); 내용 = 내용.Replace("14. ", "");
			내용 = 내용.Replace("15. ", ""); 내용 = 내용.Replace("16. ", ""); 내용 = 내용.Replace("17. ", ""); 내용 = 내용.Replace("18. ", ""); 내용 = 내용.Replace("19. ", "");
			내용 = 내용.Replace("20. ", ""); 내용 = 내용.Replace("21. ", ""); 내용 = 내용.Replace("22. ", ""); 내용 = 내용.Replace("23. ", ""); 내용 = 내용.Replace("24. ", "");
			내용 = 내용.Replace("25. ", ""); 내용 = 내용.Replace("26. ", ""); 내용 = 내용.Replace("27. ", ""); 내용 = 내용.Replace("28. ", ""); 내용 = 내용.Replace("29. ", "");
			내용 = 내용.Replace("30. ", ""); 내용 = 내용.Replace("31. ", ""); 내용 = 내용.Replace("32. ", ""); 내용 = 내용.Replace("33. ", ""); 내용 = 내용.Replace("34. ", "");
			내용 = 내용.Replace("35. ", ""); 내용 = 내용.Replace("36. ", ""); 내용 = 내용.Replace("37. ", ""); 내용 = 내용.Replace("38. ", ""); 내용 = 내용.Replace("39. ", "");
			내용 = 내용.Replace("40. ", ""); 내용 = 내용.Replace("41. ", ""); 내용 = 내용.Replace("42. ", ""); 내용 = 내용.Replace("43. ", ""); 내용 = 내용.Replace("44. ", "");
			내용 = 내용.Replace("45. ", ""); 내용 = 내용.Replace("46. ", ""); 내용 = 내용.Replace("47. ", ""); 내용 = 내용.Replace("48. ", ""); 내용 = 내용.Replace("49. ", "");
			내용 = 내용.Replace("50. ", "");

			내용 = 내용.Replace("1. ", ""); 내용 = 내용.Replace("2. ", ""); 내용 = 내용.Replace("3. ", ""); 내용 = 내용.Replace("4. ", ""); 내용 = 내용.Replace("5. ", "");
			내용 = 내용.Replace("6. ", ""); 내용 = 내용.Replace("7. ", ""); 내용 = 내용.Replace("8. ", ""); 내용 = 내용.Replace("9. ", "");

			내용 = 내용.Replace("주어진 글 다음에 ", "");
			내용 = 내용.Replace("여자의 마지막 말에 대한 ", "");
			내용 = 내용.Replace("남자의 마지막 말에 대한 ", "");
			내용 = 내용.Replace("다음 글에 드러난", "");
			내용 = 내용.Replace("대화를 듣고, ", "");
			내용 = 내용.Replace("다음을 듣고, ", "");
			내용 = 내용.Replace("다음 표를 보면서 ", "");
			내용 = 내용.Replace("다음 상황 설명을 듣고, ", "");

			내용 = 내용.Replace("를 가장 잘 나타낸 것을 고르시오.", "");
			내용 = 내용.Replace("으로 가장 적절한 것은?", "");
			내용 = 내용.Replace("로 가장 적절한 것은?", "");
			내용 = 내용.Replace("으로 가장 적절한 것을 고르시오.", "");
			내용 = 내용.Replace("로 가장 적절한 것을 고르시오.", "");
			내용 = 내용.Replace("을 고르시오.", "");
			내용 = 내용.Replace("를 고르시오.", "");

			내용 = 내용.Replace("다음 글의 밑줄 친 부분 중,", "");
			내용 = 내용.Replace("다음 중 ", "");
			내용 = 내용.Replace("다음 ", "");
			내용 = 내용.Replace("[3점]", "");

			마지막부분:

			_그래픽.DrawString(내용, _Q글꼴, _Q붓, 357, 80);
		}
		private static void 어휘문제를그림(ref Graphics _그래픽)
		{
			int 좌여백 = 316;

			for (int i = 0; i < _어휘문제들.Count; i++)
			{
				var size = _그래픽.MeasureString(_어휘문제들[i], _어휘문제글꼴);
				var rect = new RectangleF(_x어휘문제좌표[i] + 좌여백, _y어휘문제좌표[i] - 6f, size.Width, size.Height - 5);
				_그래픽.FillRectangle(_어휘문제배경, rect);
				_그래픽.DrawString(_어휘문제들[i], _어휘문제글꼴, _어휘문제붓, new PointF(_x어휘문제좌표[i] + 좌여백, _y어휘문제좌표[i] - 10f));
			}
		}
		private static void 어법문제를그림(ref Graphics _그래픽)
		{
			int 좌여백 = 316;

			for (int i = 0; i < _어법문제들.Count; i++)
			{
				var size = _그래픽.MeasureString(_어법문제들[i], _어법문제글꼴);
				var rect = new RectangleF(_x어법문제좌표[i] + 좌여백, _y어법문제좌표[i] - 6f, size.Width, size.Height - 5);
				_그래픽.FillRectangle(_어법문제배경, rect);
				_그래픽.DrawString(_어법문제들[i], _어법문제글꼴, _어법문제붓, new PointF(_x어법문제좌표[i] + 좌여백, _y어법문제좌표[i] - 10f));
			}
		}
		private static void 문법표지를그림(ref Graphics _그래픽)
		{
			List<int> 주제시작x좌표 = new List<int>();
			List<int> 주제시작y좌표 = new List<int>();

			List<int> 주제끝x좌표 = new List<int>();
			List<int> 주제끝y좌표 = new List<int>();


			List<int> 빈칸시작x좌표 = new List<int>();
			List<int> 빈칸시작y좌표 = new List<int>();

			List<int> 빈칸끝x좌표 = new List<int>();
			List<int> 빈칸끝y좌표 = new List<int>();





			Image 지시 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/지시.png");
			Image 주제 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/주제_좌측.png");
			Image 주제중간 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/주제_중간.png");
			Image 주제끝 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/주제_우측.png");

			Image 제목 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/제목.png");
			Image 속담 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/속담.png");
			Image 빈칸 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/빈칸_좌측.png");
			Image 빈칸중간 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/빈칸_중간.png");
			Image 빈칸끝 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/빈칸_우측.png");

			Image 중요 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/중요.png");
			Image 요약 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/요약.png");
			Image 어법 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/어법.png");
			Image 어휘 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/어휘.png");
			Image 분위기 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/분위기.png");
			Image 일치 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/일치.png");
			Image 흐름 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/흐름.png");

			Image 주어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/주어.png");
			Image 가주어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/가주어.png");
			Image 서술어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/서술어.png");

			Image 목적어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/목적어.png");
			Image 간접목적어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/간접목적어.png");
			Image 직접목적어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/직접목적어.png");
			Image 괄호직접목적어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/괄호직접목적어.png");
			Image 보어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/보어.png");
			Image 괄호보어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/괄호보어.png");
			Image 접속어 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/접속어.png");



			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "주제") { _그래픽.DrawImage(주제, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 7.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 40f), 25, 50)); 주제시작x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 주제시작y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); }
					else if (_문법표지들[i].문법표지문자열 == "/주제") { _그래픽.DrawImage(주제끝, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 27.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 40f), 25, 50)); 주제끝x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 주제끝y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); }
					else if (_문법표지들[i].문법표지문자열 == "제목") { _그래픽.DrawImage(제목, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
					else if (_문법표지들[i].문법표지문자열 == "속담") { _그래픽.DrawImage(속담, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }

					else if (_문법표지들[i].문법표지문자열 == "빈칸") { 빈칸시작x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 빈칸시작y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); }
					else if (_문법표지들[i].문법표지문자열 == "/빈칸") { 빈칸끝x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 빈칸끝y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); }

					else if (_문법표지들[i].문법표지문자열 == "요약") { _그래픽.DrawImage(요약, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
				}
			}

			for (int i = 0; i < 주제끝y좌표.Count; i++)
			{
				if (주제끝y좌표[i] - 주제시작y좌표[i] == 0)
				{
					_그래픽.DrawImage(주제중간, 주제시작x좌표[i] - 7.5f + 25 + _좌여백, 주제시작y좌표[i] - 40f, 주제끝x좌표[i] - 주제시작x좌표[i] - 20f, 50);
				}

				if ((주제끝y좌표[i] - 주제시작y좌표[i]) % 35 == 0 && (주제끝y좌표[i] != 주제시작y좌표[i]))
				{
					_그래픽.DrawImage(주제중간, new Rectangle((int)(주제시작x좌표[i] - 7.5f + 25 + _좌여백), (int)(주제시작y좌표[i] - 40f), _우한계 - (int)(주제시작x좌표[i] - 7.5f + 25 + _좌여백), 50));

					for (int j = 0; j < (주제끝y좌표[i] - 주제시작y좌표[i]) / 35 - 1; j++)
					{
						_그래픽.DrawImage(주제중간, new Rectangle(_좌한계, (int)(주제시작y좌표[i] - 5f + 35f * j), _우한계 - _좌한계, 50));
					}

					_그래픽.DrawImage(주제중간, new Rectangle(_좌한계, (int)(주제끝y좌표[i] - 40f), (int)(주제끝x좌표[i] - 70), 50));
				}
			}

			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "흐름")
					{
						//_그래픽.DrawImage(흐름, new Rectangle((int)(좌한계), (int)(_문법표지들[i].y문법표지시작좌표 - 47.0f), 550, 100), new Rectangle(611 - (int)_문법표지들[i].x문법표지시작좌표, 0, 550, 100), System.Drawing.GraphicsUnit.Pixel);
						_그래픽.DrawImage(흐름, new Rectangle((int)(_좌한계) - 44, (int)(_문법표지들[i].y문법표지시작좌표 - 47.0f), 638, 100), new Rectangle(570 - (int)_문법표지들[i].x문법표지시작좌표, 0, 638, 100), System.Drawing.GraphicsUnit.Pixel);
					}
				}
			}

			/*
            for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					//if (_문법표지들[i].문법표지문자열 == "중요") { _그래픽.DrawImage(중요, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + 좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 45f), 18, 18)); }
					if (_문법표지들[i].문법표지문자열 == "중요") { _그래픽.DrawImage(중요, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 30f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 + - 70f), 64, 64)); }
				}

                if (_문법표지들[i].문법표지문자열 == "지시") { _그래픽.DrawImage(지시, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
                else if (_문법표지들[i].문법표지문자열 == "분위기") { _그래픽.DrawImage(분위기, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }

            }
			*/

			#region 빈칸
			for (int i = 0; i < 빈칸끝y좌표.Count; i++)
			{
				//_그래픽.DrawImage(빈칸, new Rectangle((int)(빈칸시작x좌표[i] - 7.5f + 좌여백), (int)(빈칸시작y좌표[i] - 40f), 25, 50));
				_그래픽.DrawImage(빈칸, new Rectangle((int)(빈칸시작x좌표[i] - 3.5f + _좌여백), (int)(빈칸시작y좌표[i] - 40f), 25, 50));


				//_그래픽.DrawImage(빈칸끝, new Rectangle((int)(빈칸끝x좌표[i] - 22f + 좌여백), (int)(빈칸끝y좌표[i] - 40f), 25, 50));
				_그래픽.DrawImage(빈칸끝, new Rectangle((int)(빈칸끝x좌표[i] - 20f + _좌여백), (int)(빈칸끝y좌표[i] - 40f), 25, 50));
				if (빈칸끝y좌표[i] - 빈칸시작y좌표[i] == 0)
				{
					//if (빈칸끝x좌표[i] - 빈칸시작x좌표[i] - 55.0f > 0)
					//_그래픽.DrawImage(빈칸중간, 빈칸시작x좌표[i] + 25 + 좌여백, 빈칸시작y좌표[i] - 40f, 빈칸끝x좌표[i] - 빈칸시작x좌표[i] - 55.0f, 50);
					_그래픽.DrawImage(빈칸중간, 빈칸시작x좌표[i] + 15 + _좌여백, 빈칸시작y좌표[i] - 40.0f, 빈칸끝x좌표[i] - 빈칸시작x좌표[i] - 20.0f, 50);


				}

				if (((빈칸끝y좌표[i] - 빈칸시작y좌표[i]) % 35 == 0) && (빈칸끝y좌표[i] != 빈칸시작y좌표[i]))
				{
					//_그래픽.DrawImage(빈칸중간, new Rectangle((int)(빈칸시작x좌표[i] + 25 + 좌여백), (int)(빈칸시작y좌표[i] - 40f), 우한계 - (int)(빈칸시작x좌표[i] - 7.5f + 25 + 좌여백), 50));
					_그래픽.DrawImage(빈칸중간, new Rectangle((int)(빈칸시작x좌표[i] + 10 + _좌여백), (int)(빈칸시작y좌표[i] - 40f), _우한계 - (int)(빈칸시작x좌표[i] - 7.5f + 18 + _좌여백), 50));

					for (int j = 0; j < (빈칸끝y좌표[i] - 빈칸시작y좌표[i]) / 35 - 1; j++)
					{
						_그래픽.DrawImage(빈칸중간, new Rectangle(_좌한계, (int)(빈칸시작y좌표[i] - 5f + 35f * j), _우한계 - _좌한계, 50));
					}

					//_그래픽.DrawImage(빈칸중간, new Rectangle(좌한계, (int)(빈칸끝y좌표[i] - 40f), (int)(빈칸끝x좌표[i] - 70 - 7.5f), 50));
					_그래픽.DrawImage(빈칸중간, new Rectangle(_좌한계, (int)(빈칸끝y좌표[i] - 40f), (int)(빈칸끝x좌표[i] - 60 - 7.5f), 50));
				}
			}
			#endregion

			for (int i = 0; i < _문법표지수; i++)
			{
				// 표지좌표확인.Format("%s : %d,%d - %d,%d",(_문법표지들 + i)->문법표지문자열, (_문법표지들 + i)->x문법표지시작좌표, (_문법표지들 + i)->y문법표지시작좌표, (_문법표지들 + i)->x문법표지끝좌표, (_문법표지들 + i)->y문법표지끝좌표);
				// AfxMessageBox(표지좌표확인);
				if ((_문법표지들[i].문법표지종류 == "격자") && (_문법표지들[i].문법표지문자열 != "ⓧ"))
				{
					//_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0101, _해설형식_좌);


					_그래픽.DrawString("[", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백, _문법표지들[i].y문법표지시작좌표 - 35f - 7f));
					_그래픽.DrawString("]", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지끝좌표 - 10f + _좌여백, _문법표지들[i].y문법표지끝좌표 - 35f - 7f));

					if (_문법표지들[i].문법표지문자열 == "ⓢ") _그래픽.DrawImage(주어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "(ⓢ)") _그래픽.DrawImage(가주어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓥ") _그래픽.DrawImage(서술어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓞ") _그래픽.DrawImage(목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓒ") _그래픽.DrawImage(보어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "(ⓒ)") _그래픽.DrawImage(괄호보어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓘ") _그래픽.DrawImage(간접목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓓ") _그래픽.DrawImage(직접목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "(ⓓ)") _그래픽.DrawImage(괄호직접목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "㉨") _그래픽.DrawImage(접속어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
				}
				else if ((_문법표지들[i].문법표지종류 == "격자") && (_문법표지들[i].문법표지문자열 == "ⓧ"))
				{
					_그래픽.DrawString("(", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백, _문법표지들[i].y문법표지시작좌표 - 35 - 7f));
					_그래픽.DrawString(")", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지끝좌표 - 10f + _좌여백, _문법표지들[i].y문법표지끝좌표 - 35 - 7f));

				}
				else if ((_문법표지들[i].문법표지종류 == "밑줄") && (_문법표지들[i].문법표지문자열 != "ⓧ"))
				{
					if (_문법표지들[i].y문법표지시작좌표 == _문법표지들[i].y문법표지끝좌표)
					{
						_그래픽.DrawLine(_밑줄펜, _문법표지들[i].x문법표지시작좌표 + _좌여백, _문법표지들[i].y문법표지시작좌표 - 7f, _문법표지들[i].x문법표지끝좌표 + _좌여백, _문법표지들[i].y문법표지끝좌표 - 7f);

						//if (_문법표지들[i].문법표지문자열 == "ⓢ") _그래픽.DrawImage(주어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 6f + 좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 14f), 14, 14));

						if (_문법표지들[i].문법표지문자열 == "ⓢ") _그래픽.DrawImage(주어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓢ)") _그래픽.DrawImage(가주어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓥ") _그래픽.DrawImage(서술어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓞ") _그래픽.DrawImage(목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓒ") _그래픽.DrawImage(보어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓒ)") _그래픽.DrawImage(괄호보어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓘ") _그래픽.DrawImage(간접목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓓ") _그래픽.DrawImage(직접목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓓ)") _그래픽.DrawImage(괄호직접목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "㉨") _그래픽.DrawImage(접속어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
					}
					else
					{

						_그래픽.DrawLine(_밑줄펜, _문법표지들[i].x문법표지시작좌표 + _좌여백, _문법표지들[i].y문법표지시작좌표 - 7f, _우한계, _문법표지들[i].y문법표지시작좌표 - 7f);
						_그래픽.DrawLine(_밑줄펜, 368, _문법표지들[i].y문법표지끝좌표 - 7f, _문법표지들[i].x문법표지끝좌표 + _좌여백, _문법표지들[i].y문법표지끝좌표 - 7f);

						// 만약 두 줄 이상이면,
						if (_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표 >= 70)
						{
							int 시작과끝의줄간격 = (int)(_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표);
							시작과끝의줄간격 /= 35;

							for (int i더할줄 = 0; i더할줄 < 시작과끝의줄간격 - 1; i더할줄++)
							{
								_그래픽.DrawLine(_밑줄펜, 368, _문법표지들[i].y문법표지시작좌표 - 7f + 35f * (i더할줄 + 1), _우한계, _문법표지들[i].y문법표지시작좌표 - 7f + 35f * (i더할줄 + 1));
							}
						}

						//if (_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표 == 70){ _그래픽.DrawLine(_밑줄펜, 368, _문법표지들[i].y문법표지시작좌표 - 7f + 35f, 우한계, _문법표지들[i].y문법표지시작좌표 - 7f + 35f);}


						//if (_문법표지들[i].문법표지문자열 == "ⓢ") _그래픽.DrawString("S", _문법표지글꼴, _문법표지붓, new PointF(우문법기호한계, _문법표지들[i].y문법표지시작좌표 - 7f));

						if (_문법표지들[i].문법표지문자열 == "ⓢ") _그래픽.DrawImage(주어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓢ)") _그래픽.DrawImage(가주어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓥ") _그래픽.DrawImage(서술어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓞ") _그래픽.DrawImage(목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓒ") _그래픽.DrawImage(보어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓒ)") _그래픽.DrawImage(괄호보어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓘ") _그래픽.DrawImage(간접목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓓ") _그래픽.DrawImage(직접목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓓ)") _그래픽.DrawImage(괄호직접목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "㉨") _그래픽.DrawImage(접속어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
					}
				}


			}


			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "일치") { _그래픽.DrawImage(일치, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
				}
			}


		}

		private static void 중요표지를그림(ref Graphics _그래픽)
		{
			Image 중요 = Image.FromFile(_IMG루트폴더 + "문제유형아이콘/중요.png");

			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					//if (_문법표지들[i].문법표지문자열 == "중요") { _그래픽.DrawImage(중요, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + 좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 45f), 18, 18)); }
					if (_문법표지들[i].문법표지문자열 == "중요") { _그래픽.DrawImage(중요, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 30f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 + -70f), 64, 64)); }
				}
			}

		}

		private static void 취소선을그림(ref Graphics _그래픽)
		{
			int 좌여백 = 316;
			int 우한계 = 916;


			for (int i = 0; i < _문법표지수; i++)
			{
				if ((_문법표지들[i].문법표지종류 == "밑줄") && (_문법표지들[i].문법표지문자열 == "ⓧ"))
				{
					if (_문법표지들[i].y문법표지시작좌표 == _문법표지들[i].y문법표지끝좌표)
					{
						//_그래픽.DrawLine(_밑줄펜, _문법표지들[i].x문법표지시작좌표 + 좌여백, _문법표지들[i].y문법표지시작좌표 - 17f, _문법표지들[i].x문법표지끝좌표 + 좌여백, _문법표지들[i].y문법표지끝좌표 - 17f);
						_그래픽.DrawLine(_취소펜, _문법표지들[i].x문법표지시작좌표 + 좌여백, _문법표지들[i].y문법표지시작좌표 - 17f, _문법표지들[i].x문법표지끝좌표 + 좌여백, _문법표지들[i].y문법표지끝좌표 - 17f);
					}
					else
					{
						//_그래픽.DrawLine(_밑줄펜, _문법표지들[i].x문법표지시작좌표 + 좌여백, _문법표지들[i].y문법표지시작좌표 - 17f, 우한계, _문법표지들[i].y문법표지시작좌표 - 17f);
						//_그래픽.DrawLine(_밑줄펜, 368, _문법표지들[i].y문법표지끝좌표 - 17f, _문법표지들[i].x문법표지끝좌표 + 좌여백, _문법표지들[i].y문법표지끝좌표 - 17f);
						_그래픽.DrawLine(_취소펜, _문법표지들[i].x문법표지시작좌표 + 좌여백, _문법표지들[i].y문법표지시작좌표 - 17f, 우한계, _문법표지들[i].y문법표지시작좌표 - 17f);
						_그래픽.DrawLine(_취소펜, 368, _문법표지들[i].y문법표지끝좌표 - 17f, _문법표지들[i].x문법표지끝좌표 + 좌여백, _문법표지들[i].y문법표지끝좌표 - 17f);


						// 만약 두 줄 이상이면,

						//if (_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표 == 70) { _그래픽.DrawLine(_밑줄펜, 368, _문법표지들[i].y문법표지시작좌표 - 17f + 35f, 우한계, _문법표지들[i].y문법표지시작좌표 - 17f + 35f); }
						if (_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표 == 70) { _그래픽.DrawLine(_취소펜, 368, _문법표지들[i].y문법표지시작좌표 - 17f + 35f, 우한계, _문법표지들[i].y문법표지시작좌표 - 17f + 35f); }
					}
				}
			}
		}
		private static void 변환_문자열을_여러줄로(string 문자열, int y좌표시작위치, ref List<한줄> 여러줄_처리결과)
		{
			문자열 = 문자열.Trim();   // 꼭 이 자리여야 함
			if (문자열 == "") return;

			List<string> 여러줄 = new List<string>();


			여러줄_처리결과.Clear();

			string 현재줄;
			string 현재어절;
			List<float> 줄의빈칸너비들 = new List<float>();
			List<string> 개행문자단위_문자열들 = new List<string>();

			string 현재의개행문자단위문자열;
			string 문법표지를포함한문자열 = 문자열;
			string 문법표지를제거한문자열;

			문법표지를제거한문자열 = 변환.문자열.문법문제표지제거(문법표지를포함한문자열);

			변환.문자열.개행문자로_구분한_문자열들로(문법표지를제거한문자열, ref 개행문자단위_문자열들);

			List<int> 어절이_몇번째문장인지_매긴_리스트 = new List<int>();

			강력하지만무거운변환.문자열.어절이_몇번째문장인지_매김(문법표지를제거한문자열, ref 어절이_몇번째문장인지_매긴_리스트);



			for (int i = 0; i < 개행문자단위_문자열들.Count(); i++)
			{
				List<string> 어절들 = new List<string>();

				현재의개행문자단위문자열 = 개행문자단위_문자열들[i];
				변환.문자열.어절들로(현재의개행문자단위문자열, ref 어절들);

				float 현재까지줄의너비중빈칸을뗀것 = 0;
				float 현재어절의너비 = 0;
				float 새로운어절을더해봤을때의너비 = 0;
				float 현재줄에적절한빈칸의너비 = 0;
				int 현재줄의어절숫자 = 0;

				현재줄 = "";

				for (int j = 0; j < 어절들.Count(); j++)
				{
					현재어절 = 어절들[j];
					현재어절의너비 = 현재어절의너비확인(현재어절);

					새로운어절을더해봤을때의너비 += 현재어절의너비;

					if (새로운어절을더해봤을때의너비 > 550)
					{
						여러줄.Add(현재줄);
						if (현재줄의어절숫자 != 1)
							현재줄에적절한빈칸의너비 = (550 - 현재까지줄의너비중빈칸을뗀것) / (현재줄의어절숫자 - 1);
						else
							현재줄에적절한빈칸의너비 = 10; // 필요는 없는 부분이지만,

						줄의빈칸너비들.Add(현재줄에적절한빈칸의너비);

						// 처리 이후의 초기화-------------------------------------------------------------------------
						현재줄 = 현재어절;
						새로운어절을더해봤을때의너비 = 현재어절의너비;
						현재까지줄의너비중빈칸을뗀것 = 현재어절의너비;

						현재줄의어절숫자 = 1;
					}
					else
					{
						현재까지줄의너비중빈칸을뗀것 += 현재어절의너비;
						현재줄의어절숫자++;
						현재줄 += " ";
						현재줄 += 현재어절;
						새로운어절을더해봤을때의너비 += 10;
					}
				}

				여러줄.Add(현재줄);
				줄의빈칸너비들.Add(10);
			}


			int 현재어절수 = 0;

			for (int i = 0; i < 여러줄.Count(); i++)
			{
				List<string> 어절들 = new List<string>();


				현재줄 = 여러줄[i];
				변환.문자열.어절들로(현재줄, ref 어절들);

				한줄 추가할한줄 = new 한줄();

				float x바로전글자끝좌표 = 0;

				for (int j = 0; j < 어절들.Count(); j++)
				{

					어절 추가할어절 = new 어절();
					추가할어절.내용 = 어절들[j];

					if (j == 0)
					{
						추가할어절.x글자시작좌표 = 50;
						추가할어절.x글자앞빈칸중간좌표 = 추가할어절.x글자시작좌표 - 5;
						추가할어절.x글자끝좌표 = 추가할어절.x글자시작좌표 + 현재어절의너비확인(어절들[j]);
						추가할어절.x글자뒷빈칸중간좌표 = 추가할어절.x글자시작좌표 + 현재어절의너비확인(어절들[j]) + 줄의빈칸너비들[i] / 2;
						추가할어절.y글자시작좌표 = y좌표시작위치 + (35 * i);
						추가할어절.y밑줄좌표 = y좌표시작위치 + (35 * i) + 20;
					}
					else
					{
						추가할어절.x글자시작좌표 = x바로전글자끝좌표 + 줄의빈칸너비들[i];
						추가할어절.x글자앞빈칸중간좌표 = x바로전글자끝좌표 + 줄의빈칸너비들[i] / 2;
						추가할어절.x글자끝좌표 = 추가할어절.x글자시작좌표 + 현재어절의너비확인(어절들[j]);
						추가할어절.x글자뒷빈칸중간좌표 = 추가할어절.x글자시작좌표 + 현재어절의너비확인(어절들[j]) + 줄의빈칸너비들[i] / 2;
						추가할어절.y글자시작좌표 = y좌표시작위치 + (35 * i);
						추가할어절.y밑줄좌표 = y좌표시작위치 + (35 * i) + 20;
					}

					x바로전글자끝좌표 = 추가할어절.x글자끝좌표;
					추가할어절.몇번째문장인지 = 어절이_몇번째문장인지_매긴_리스트[현재어절수];
					추가할한줄._어절들.Add(추가할어절);
					현재어절수++;

				}

				여러줄_처리결과.Add(추가할한줄);
			}


			문법표지의좌표를설정해줌(문법표지를포함한문자열, ref 여러줄_처리결과);
		}

		// CConverter::문법표지제거 함수와 쌍을 이룹니다.
		private static void 문법표지의좌표를설정해줌(string 문법표지를포함한문자열, ref List<한줄> 여러줄)
		{
			//            _문법표지들.Clear();

			//	        _문법표지수 = 0;

			List<string> 문법표지들 = new List<string>();
			List<string> 문법표지종류들 = new List<string>();

			//	        CStringArray	문법표지들;
			//	        CStringArray	문법표지종류들;

			List<float> x문법표지시작좌표들 = new List<float>();
			List<float> x문법표지끝좌표들 = new List<float>();
			List<float> y문법표지시작좌표들 = new List<float>();
			List<float> y문법표지끝좌표들 = new List<float>();
			Stack<int> 문법표지스택 = new Stack<int>();

			int 문법표지갯수 = -1;
			float x문법표지시작좌표 = 0;
			float y문법표지시작좌표 = 0;
			float x문법표지끝좌표 = 0;
			float y문법표지끝좌표 = 0;
			//int 현재괄호중첩갯수		= 0;



			for (int i = 0; i < 문법표지를포함한문자열.Length; i++)
			{
				//		AfxMessageBox(문법표지를포함한문자열.Mid(i,2));
				if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "ⓢ{") { 문법표지들.Add("ⓢ"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "(ⓢ){") { 문법표지들.Add("(ⓢ)"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "ⓥ{") { 문법표지들.Add("ⓥ"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "ⓒ{") { 문법표지들.Add("ⓒ"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "(ⓒ){") { 문법표지들.Add("(ⓒ)"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "ⓞ{") { 문법표지들.Add("ⓞ"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "ⓘ{") { 문법표지들.Add("ⓘ"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "ⓓ{") { 문법표지들.Add("ⓓ"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "(ⓓ){") { 문법표지들.Add("(ⓓ)"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "㉨{") { 문법표지들.Add("㉨"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if (문법표지스택.Count != 0) { 문법표지종류들[문법표지스택.Peek()] = "격자"; } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == "ⓧ{") { 문법표지들.Add("ⓧ"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 1; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄"); }

				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{지시}") { 문법표지들.Add("지시"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{주제}") { 문법표지들.Add("주제"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 5) == "{/주제}") { 문법표지들.Add("/주제"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 4; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }


				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{제목}") { 문법표지들.Add("제목"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{속담}") { 문법표지들.Add("속담"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }

				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{빈칸}") { 문법표지들.Add("빈칸"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 5) == "{/빈칸}") { 문법표지들.Add("/빈칸"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 4; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }

				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{중요}") { 문법표지들.Add("중요"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{요약}") { 문법표지들.Add("요약"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{어법}") { 문법표지들.Add("어법"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{어휘}") { 문법표지들.Add("어휘"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{어휘:") {

					string 어휘문제내용 = 변환.문자열.Mid(문법표지를포함한문자열, i + 4);
					어휘문제내용 = 어휘문제내용.Substring(0, 어휘문제내용.IndexOf(':'));

					문법표지들.Add("어휘"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제");

					_어휘문제들.Add(어휘문제내용);
					_x어휘문제좌표.Add(x문법표지시작좌표);
					_y어휘문제좌표.Add(y문법표지시작좌표);

				}
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{어법:")
				{

					string 어법문제내용 = 변환.문자열.Mid(문법표지를포함한문자열, i + 4);
					어법문제내용 = 어법문제내용.Substring(0, 어법문제내용.IndexOf(':'));

					문법표지들.Add("어법"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제");

					_어법문제들.Add(어법문제내용);
					_x어법문제좌표.Add(x문법표지시작좌표);
					_y어법문제좌표.Add(y문법표지시작좌표);

				}



				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 5) == "{분위기}") { 문법표지들.Add("분위기"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 4; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{일치}") { 문법표지들.Add("일치"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 4) == "{흐름}") { 문법표지들.Add("흐름"); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 3; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add("문제"); }

				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 3) == ":} ") { i += 2; }
				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == ":}") { i += 1; }

				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 1) == "{") { 문법표지들.Add(""); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 0; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add(""); 문법표지스택.Push(문법표지갯수); }

				else if (변환.문자열.Mid(문법표지를포함한문자열, i, 1) == "}") { 문법표지의끝좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), 문법표지를포함한문자열.Substring(i + 1), ref x문법표지끝좌표, ref y문법표지끝좌표, ref 여러줄); x문법표지끝좌표들[문법표지스택.Peek()] = x문법표지끝좌표; y문법표지끝좌표들[문법표지스택.Peek()] = y문법표지끝좌표; 문법표지스택.Pop(); }

			}

			if (문법표지갯수 == -1)
				return;

			//_문법표지수 += 문법표지갯수 + 1;

			for (int i = 0; i < 문법표지갯수 + 1; i++)
			{
				문법표지 현재문법표지 = new 문법표지();

				현재문법표지.문법표지문자열 = 문법표지들[i];
				현재문법표지.문법표지종류 = 문법표지종류들[i];
				현재문법표지.x문법표지시작좌표 = x문법표지시작좌표들[i];
				현재문법표지.y문법표지시작좌표 = y문법표지시작좌표들[i];
				현재문법표지.x문법표지끝좌표 = x문법표지끝좌표들[i];
				현재문법표지.y문법표지끝좌표 = y문법표지끝좌표들[i];

				_문법표지들.Add(현재문법표지);
			}

			// 문법표지 끝좌표의 격자가 겹치는 경우 조금 띄어주는 기능
			for (int i = _문법표지수; i < _문법표지수 + 문법표지갯수 + 1; i++)
			{
				for (int j = _문법표지수; j < i; j++)
				{
					if (_문법표지들[i].문법표지종류 == "격자")
					{
						if ((_문법표지들[i].x문법표지끝좌표 == _문법표지들[j].x문법표지끝좌표) && (_문법표지들[i].y문법표지끝좌표 == _문법표지들[j].y문법표지끝좌표))
							_문법표지들[i].x문법표지끝좌표 += 3.5f;
					}
					//if(_문법표지들[i].문법표지종류 == "문제")
					//{
					//    if ((_문법표지들[i].x문법표지끝좌표 == _문법표지들[j].x문법표지끝좌표) && (_문법표지들[i].y문법표지끝좌표 == _문법표지들[j].y문법표지끝좌표))
					//        _문법표지들[i].x문법표지시작좌표 += 25f;
					//}
				}
			}

			// 문법표지 시작좌표의 격자가 겹치는 경우 조금 띄어주는 기능
			for (int i = _문법표지수; i < _문법표지수 + 문법표지갯수 + 1; i++)
			{
				for (int j = _문법표지수; j < i; j++)
				{
					if (_문법표지들[i].문법표지종류 == "격자")
					{
						if ((_문법표지들[i].x문법표지시작좌표 == _문법표지들[j].x문법표지시작좌표) && (_문법표지들[i].y문법표지시작좌표 == _문법표지들[j].y문법표지시작좌표))
							_문법표지들[i].x문법표지시작좌표 -= 3.5f;
					}
				}
			}

			// 괄호와 관련한 기능
			for (int i = _문법표지수; i < _문법표지수 + 문법표지갯수 + 1; i++)
			{
				for (int j = _문법표지수; j < i; j++)
				{
					if ((_문법표지들[j].문법표지종류 == "밑줄") && (_문법표지들[i].문법표지종류 == "격자") && (_문법표지들[i].문법표지문자열 == "ⓧ"))
					{
						if ((_문법표지들[j].y문법표지시작좌표 < _문법표지들[i].y문법표지시작좌표) || ((_문법표지들[j].y문법표지시작좌표 == _문법표지들[i].y문법표지시작좌표) && (_문법표지들[j].x문법표지시작좌표 < _문법표지들[i].x문법표지시작좌표)))
						{
							if ((_문법표지들[i].y문법표지시작좌표 < _문법표지들[j].y문법표지끝좌표) || ((_문법표지들[i].y문법표지시작좌표 == _문법표지들[j].y문법표지끝좌표) && (_문법표지들[i].x문법표지시작좌표 < _문법표지들[j].x문법표지끝좌표)))
							{
								_문법표지들[j].x문법표지끝좌표 = _문법표지들[i].x문법표지시작좌표;
								_문법표지들[j].y문법표지끝좌표 = _문법표지들[i].y문법표지시작좌표;
							}
						}
					}
				}
			}
			_문법표지수 += 문법표지갯수 + 1;
		}

		// 앞의 어절이 몇 개나 있는지 확인해서
		// 현재 문법표지의 시작좌표가 어디인지를 확인함
		// 앞에 어절이 아니라 어포스트로피 앞의 주어인 경우 정교한 작업이 필요함.
		private static void 문법표지의시작좌표확인(string 문법표지앞의_모든문자열, ref float x문법표지시작좌표, ref float y문법표지시작좌표, ref List<한줄> _여러줄)
		{
			if (_여러줄.Count == 0) return;

			List<string> 문법표지앞부분의어절들 = new List<string>();

			string 문법표지앞부분의문자열중표지제거한것 = 변환.문자열.문법문제표지제거(문법표지앞의_모든문자열);
			변환.문자열.어절들로(문법표지앞부분의문자열중표지제거한것, ref 문법표지앞부분의어절들);

			int 현재어절수 = 0;
			if (문법표지앞부분의문자열중표지제거한것 == "") // 여기는 맨 앞줄이라는 거임
			{
				if (_여러줄[0]._어절들.Count == 0)
					return;

				x문법표지시작좌표 = _여러줄[0]._어절들[0].x글자시작좌표;
				y문법표지시작좌표 = _여러줄[0]._어절들[0].y밑줄좌표;

			}
			else if (변환.문자열.Right(문법표지앞부분의문자열중표지제거한것, 1) == " "
			|| 변환.문자열.Right(문법표지앞부분의문자열중표지제거한것, 1) == "\r"
			|| 변환.문자열.Right(문법표지앞부분의문자열중표지제거한것, 1) == "\n"
			|| 변환.문자열.Right(문법표지앞부분의문자열중표지제거한것, 1) == "\t"
			) // 대개는 앞부분이 공백임, 생각을 해봐 "문법표지1{내용1} 문법표지2{내용2}" 이렇게 되면 문법표지2 앞은 공백이잖아.
			{


				for (int i = 0; i < _여러줄.Count; i++)
				{
					for (int j = 0; j < _여러줄[i]._어절들.Count; j++)
					{
						현재어절수++;

						if (문법표지앞부분의어절들.Count() == 현재어절수)
						{
							if (j < _여러줄[i]._어절들.Count - 1) // 맨 마지막 어절이 아니라면
							{
								x문법표지시작좌표 = _여러줄[i]._어절들[j + 1].x글자시작좌표;
								y문법표지시작좌표 = _여러줄[i]._어절들[j + 1].y밑줄좌표;
							}
							// 맨 마지막 어절인 경우에는 여기로 들어가는데, 문제는 다음줄의 첫부분을 찾는데,
							// 칸을 두 번 띄우면 다음줄의 첫부분이 아니라, 다다음줄의 첫부분을 찾아야 함.
							else if (i < _여러줄.Count - 1 && _여러줄[i + 1]._어절들.Count != 0)
							{
								x문법표지시작좌표 = _여러줄[i + 1]._어절들[0].x글자시작좌표;
								y문법표지시작좌표 = _여러줄[i + 1]._어절들[0].y밑줄좌표;
							}
							else if (i < _여러줄.Count - 1 && _여러줄[i + 1]._어절들.Count == 0)
							{
								int 임시 = 1;
								while (_여러줄[i + 1 + 임시]._어절들.Count == 0)
								{
									임시++;
								}
								x문법표지시작좌표 = _여러줄[i + 1 + 임시]._어절들[0].x글자시작좌표;
								y문법표지시작좌표 = _여러줄[i + 1 + 임시]._어절들[0].y밑줄좌표;
							}
							else
							{
								x문법표지시작좌표 = 0;
								y문법표지시작좌표 = 0;
							}

							return;
						}
					}
				}
			}
			else
			{
				string 어포스트로피앞부분의글자들;

				if (문법표지앞부분의문자열중표지제거한것.LastIndexOf(' ') != -1)
					어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것.Substring(문법표지앞부분의문자열중표지제거한것.LastIndexOf(' ') + 1);
				else
					어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것;



				for (int i = 0; i < _여러줄.Count; i++)
				{
					for (int j = 0; j < _여러줄[i]._어절들.Count; j++)
					{
						현재어절수++;

						if (문법표지앞부분의어절들.Count() == 현재어절수)
						{

							x문법표지시작좌표 = _여러줄[i]._어절들[j].x글자시작좌표 + 현재어절의너비확인(어포스트로피앞부분의글자들) + 2; // +2은 살짝만 띈다는 의미임
							y문법표지시작좌표 = _여러줄[i]._어절들[j].y밑줄좌표;

							return;
						}
					}
				}
			}
		}
		private static void 문법표지의끝좌표확인(string 문법표지앞부분의문자열, string 문법표지뒷부분의문자열, ref float x문법표지끝좌표, ref float y문법표지끝좌표, ref List<한줄> _여러줄)
		{
			List<string> 문법표지앞부분의어절들 = new List<string>();


			string 문법표지앞부분의문자열중표지제거한것 = 변환.문자열.문법문제표지제거(문법표지앞부분의문자열);
			string 문법표지뒷부분의문자열중표지제거한것 = 변환.문자열.문법문제표지제거(문법표지뒷부분의문자열);

			변환.문자열.어절들로(문법표지앞부분의문자열중표지제거한것, ref 문법표지앞부분의어절들);

			int 현재어절수 = 0;


			// 일반적인 경우
			//	        if((문법표지뒷부분의문자열중표지제거한것.Substring(0, 1) != " ") && (문법표지뒷부분의문자열중표지제거한것 != "") 
			//                && (문법표지뒷부분의문자열중표지제거한것.Substring(0, 1) != ".") && (문법표지뒷부분의문자열중표지제거한것.Substring(0, 1) != ",") 
			//                && (문법표지뒷부분의문자열중표지제거한것.Substring(0, 2) != "，") && (문법표지뒷부분의문자열중표지제거한것.Substring(0, 2) != "．"))

			if ((변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 1) != " ") && (문법표지뒷부분의문자열중표지제거한것 != "")
				&& (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 1) != ".") && (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 1) != ",")
				&& (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 2) != ", ") && (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 2) != ". "))
			{
				string 어포스트로피앞부분의글자들;

				if (문법표지앞부분의문자열.LastIndexOf(' ') != -1)
					어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것.Substring(문법표지앞부분의문자열중표지제거한것.LastIndexOf(' ') + 1);
				else
					어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것;



				for (int i = 0; i < _여러줄.Count; i++)
				{
					for (int j = 0; j < _여러줄[i]._어절들.Count; j++)
					{
						현재어절수++;

						if (문법표지앞부분의어절들.Count() == 현재어절수)
						{
							x문법표지끝좌표 = _여러줄[i]._어절들[j].x글자시작좌표 + 현재어절의너비확인(어포스트로피앞부분의글자들) - 2; // -2은 살짝만 띈다는 의미임
							y문법표지끝좌표 = _여러줄[i]._어절들[j].y밑줄좌표;

							return;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < _여러줄.Count; i++)
				{
					for (int j = 0; j < _여러줄[i]._어절들.Count; j++)
					{
						현재어절수++;

						if (문법표지앞부분의어절들.Count() == 현재어절수)
						{
							x문법표지끝좌표 = _여러줄[i]._어절들[j].x글자끝좌표;
							y문법표지끝좌표 = _여러줄[i]._어절들[j].y밑줄좌표;

							return;
						}
					}
				}

			}
			// 뒤에 어포스트로피가 있는 경우

		}
		private static float 현재어절의너비확인(string 현재어절)
		{
			float 현재어절너비 = 0;

			for (int i = 0; i < 현재어절.Length; i++)
				현재어절너비 += 개별코드에대한너비(현재어절[i]);



			return 현재어절너비;
		}
		private static float 개별코드에대한너비(char 코드)
		{
			if (코드 == 'A') return 13f;
			else if (코드 == 'B') return 12f;
			else if (코드 == 'C') return 12.5f;
			else if (코드 == 'D') return 13f;
			else if (코드 == 'E') return 10f;
			else if (코드 == 'F') return 9.5f;
			else if (코드 == 'G') return 13f;
			else if (코드 == 'H') return 13.5f;
			else if (코드 == 'I') return 5.5f;
			else if (코드 == 'J') return 7.5f;
			else if (코드 == 'K') return 11.5f;
			else if (코드 == 'L') return 9.5f;
			else if (코드 == 'M') return 17.5f;
			else if (코드 == 'N') return 14.5f;
			else if (코드 == 'O') return 15.5f;
			else if (코드 == 'P') return 10.5f;
			else if (코드 == 'Q') return 16f;
			else if (코드 == 'R') return 12f;
			else if (코드 == 'S') return 10.5f;
			else if (코드 == 'T') return 11.5f;
			else if (코드 == 'U') return 13.5f;
			else if (코드 == 'V') return 14f;
			else if (코드 == 'W') return 20.5f;
			else if (코드 == 'X') return 13f;
			else if (코드 == 'Y') return 12.5f;
			else if (코드 == 'Z') return 12.5f;

			else if (코드 == 'a') return 11f;
			else if (코드 == 'b') return 11.5f;
			else if (코드 == 'c') return 9.5f;
			else if (코드 == 'd') return 12.5f;
			else if (코드 == 'e') return 10.5f;
			else if (코드 == 'f') return 8.5f;
			else if (코드 == 'g') return 11.5f;
			else if (코드 == 'h') return 11.5f;
			else if (코드 == 'i') return 5.5f;
			else if (코드 == 'j') return 6.5f;
			else if (코드 == 'k') return 10.5f;
			else if (코드 == 'l') return 5.5f;
			else if (코드 == 'm') return 17f;
			else if (코드 == 'n') return 11.5f;
			else if (코드 == 'o') return 13f;
			else if (코드 == 'p') return 12f;
			else if (코드 == 'q') return 12f;
			else if (코드 == 'r') return 8.5f;
			else if (코드 == 's') return 9.5f;
			else if (코드 == 't') return 8.5f;
			else if (코드 == 'u') return 11.5f;
			else if (코드 == 'v') return 11.5f;
			else if (코드 == 'w') return 17f;
			else if (코드 == 'x') return 11f;
			else if (코드 == 'y') return 11f;
			else if (코드 == 'z') return 12f;

			else if (코드 == '1') return 12f;
			else if (코드 == '2') return 10.5f;
			else if (코드 == '3') return 11f;
			else if (코드 == '4') return 11f;
			else if (코드 == '5') return 10f;
			else if (코드 == '6') return 12f;
			else if (코드 == '7') return 11.5f;
			else if (코드 == '8') return 12f;
			else if (코드 == '9') return 12f;
			else if (코드 == '0') return 11.5f;
			else if (코드 == ' ') return 10f;
			else if (코드 == '~') return 12f;
			else if (코드 == '`') return 7f;
			else if (코드 == '!') return 4.5f;
			else if (코드 == '@') return 20f;
			else if (코드 == '#') return 14f;
			else if (코드 == '$') return 10f;
			else if (코드 == '%') return 18.5f;
			else if (코드 == '^') return 12f;
			else if (코드 == '&') return 17f;
			else if (코드 == '*') return 8.5f;
			else if (코드 == '(') return 5.5f;
			else if (코드 == ')') return 5.5f;
			else if (코드 == '_') return 8.5f;
			else if (코드 == '-') return 6.5f;
			else if (코드 == '+') return 12f;
			else if (코드 == '=') return 12f;
			else if (코드 == '[') return 5f;
			else if (코드 == ']') return 5f;
			else if (코드 == '{') return 5.5f;
			else if (코드 == '}') return 5.5f;
			else if (코드 == '|') return 3.5f;
			else if (코드 == '\\') return 17.5f;
			else if (코드 == ':') return 5f;
			else if (코드 == ';') return 5f;
			else if (코드 == '\"') return 8f;
			else if (코드 == '\'') return 5f;
			else if (코드 == '’') return 5f;
			else if (코드 == '<') return 11.5f;
			else if (코드 == '>') return 11.5f;
			else if (코드 == ',') return 5f;
			else if (코드 == '.') return 5f;
			else if (코드 == '.') return 5f;
			else if (코드 == '?') return 10f;
			else if (코드 == '/') return 10f;
			else if (코드 == '“') return 6.5f;
			else if (코드 == '‘') return 6.5f;

			return 20f;
		}
		private static void 사진들넣기(ref Graphics _그래픽)
		{
			Image 그림파일 = Image.FromFile(_IMG루트폴더 + "프로그램스킨/사진들.png");

			_그래픽.DrawImage(그림파일, new Rectangle(320, 0, 640, 640));

			그림파일.Dispose();

		}
		private static void 더러운문제경고하기(ref Graphics _그래픽, string Q)
		{
			if (Q.Contains("▼"))
			{
				Image 그림파일 = Image.FromFile(_IMG루트폴더 + "프로그램스킨/DirtyDrumCan.png");

				_그래픽.DrawImage(그림파일, new Rectangle(720, 170, 200, 200));

				그림파일.Dispose();

			}
		}
		/*
				private static void 외곽테두리넣기(ref Graphics _그래픽)
				{

					Image 그림파일 = Image.FromFile(_IMG루트폴더 + "프로그램스킨/외곽테두리.png");

					_그래픽.DrawImage(그림파일, new Rectangle(0, 0, 1280, 1280));

					그림파일.Dispose();

				}
				*/
		private static void 엠블럼넣기(ref Graphics _그래픽)
		{
			Image 그림파일 = Image.FromFile(_IMG루트폴더 + "프로그램스킨/엠블럼.png");

			_그래픽.DrawImage(그림파일, new Rectangle(320, 125, 300, 300));

			그림파일.Dispose();
		}
		private static void 글자사각배경넣기(ref Graphics _그래픽)
		{
			Image 그림파일 = Image.FromFile(_IMG루트폴더 + "프로그램스킨/글자사각배경.png");


			//_그래픽.DrawImage(그림파일, new Rectangle(290, 360, 700, _세로픽셀), new Rectangle(0, 0, 700, _세로픽셀), GraphicsUnit.Pixel);
			//_그래픽.DrawImage(그림파일, new Rectangle(290, 50, 700, _세로픽셀), new Rectangle(0, 0, 700, _세로픽셀), GraphicsUnit.Pixel);
			_그래픽.DrawImage(그림파일, new Rectangle(290, 0, 700, _세로픽셀), new Rectangle(0, 0, 700, _세로픽셀), GraphicsUnit.Pixel);
			그림파일.Dispose();
		}
		public static void 배경그림넣기(string 지문, string 전체경로, ref Graphics _그래픽)
		{
			지문 = " " + 지문 + " ";
			지문 = 지문.불필요제거().ToLower();
			지문 = 지문.Replace("\r", " ");
			지문 = 지문.Replace("\n", " ");
			// 최적의 배경을 찾는 부분

			string 선택된배경파일이름 = "";
			string 찾을말;

			// 키워드 검색하기
			// 인간이 말을 하는 지문에는 핵심이 되는 주제가 있다. 핵심 주제 천 개가 있다고 치면, 단어가 나온 것들을 통해서 얼마나 그 주제에 가까운 내용인지를 검토한 다음 그 점수에 따라서 배경을 설정한다.


			// 우선은 많이 하는 말을 찾아보자.
			List<string> 어절들 = new List<string>();

			변환.문자열.어절들로(지문, ref 어절들);
			어절들.Sort();


			if (Directory.Exists(_IMG루트폴더))
			{
				DirectoryInfo dir = new DirectoryInfo(_IMG루트폴더);
				System.IO.FileInfo[] files = dir.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);

				int 현재그림이름의_적합도점수 = 0;
				int 여태까지그림중_최고적합도 = -1;

				foreach (System.IO.FileInfo file in files)
				{



					현재그림이름의_적합도점수 = 0;

					string 확장자를포함한_현재그림파일이름, 현재그림파일이름;
					확장자를포함한_현재그림파일이름 = file.Name;

					현재그림파일이름 = Path.GetFileNameWithoutExtension(확장자를포함한_현재그림파일이름);

					현재그림파일이름 = 현재그림파일이름.ToLower();

					if (현재그림파일이름.Contains(","))
					{
						string[] 현재그림파일이름을쉼표로나눈것들 = 현재그림파일이름.Split(',');

						bool 현재쉼표로나뉜것이모두있는지여부 = true;

						foreach (string 현재쉼표로나뉜것 in 현재그림파일이름을쉼표로나눈것들)
						{

							찾을말 = String.Format(" {0} ", 현재쉼표로나뉜것);

							if (!지문.Contains(찾을말))
							{
								현재쉼표로나뉜것이모두있는지여부 = false;
							}
						}

						if (현재쉼표로나뉜것이모두있는지여부 == true)
						{
							현재그림이름의_적합도점수 += (현재그림파일이름을쉼표로나눈것들.Count() * 100);
							현재그림이름의_적합도점수 += 현재그림파일이름.Length;
						}
					}
					else
					{
						찾을말 = String.Format(" {0} ", 현재그림파일이름);

						if (지문.Contains(찾을말))
						{
							현재그림이름의_적합도점수 += 1;
							현재그림이름의_적합도점수 += 현재그림파일이름.Length;

							if (현재그림파일이름.Contains(" "))
							{
								현재그림이름의_적합도점수 += 50;
							}
							else if (현재그림파일이름 == "beautiful") 현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "building") 현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "finish") 현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "general") 현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "website") 현재그림이름의_적합도점수 = 3;

						}
					}

					if (여태까지그림중_최고적합도 < 현재그림이름의_적합도점수)
					{
						여태까지그림중_최고적합도 = 현재그림이름의_적합도점수;
						선택된배경파일이름 = 현재그림파일이름;
						_선택된배경파일이름 = 선택된배경파일이름;
					}


				}
			}
			else
				Directory.CreateDirectory(_IMG루트폴더);






			string 그림경로 = String.Format("{0}{1}.jpg", _IMG루트폴더, 선택된배경파일이름);


			Image 그림파일 = Image.FromFile(그림경로);


			int 그림변환된높이 = (1280 * 그림파일.Height) / 그림파일.Width;


			//--------------------------------
			//_그래픽.CompositingMode = CompositingMode.SourceCopy;
			//_그래픽.PixelOffsetMode = PixelOffsetMode.Half;
			//_그래픽.InterpolationMode = InterpolationMode.NearestNeighbor;

			// Draw your image here.



			//_그래픽.InterpolationMode = InterpolationMode.HighQualityBicubic;
			//----------------------------------

			_그래픽.PixelOffsetMode = PixelOffsetMode.Half;
			//_그래픽.DrawImage(그림파일, new Rectangle(0, 0, 1280, 그림변환된높이));

			_그래픽.DrawImage(그림파일, new Rectangle(0, 0, 1280, 그림변환된높이), new Rectangle(1, 1, 그림파일.Width - 2, 그림파일.Height - 2), GraphicsUnit.Pixel);



			for (int i = 1; 그림변환된높이 * i < _세로픽셀; i++)
			{
				그림파일.RotateFlip(RotateFlipType.RotateNoneFlipY);

				_그래픽.DrawImage(그림파일, new Rectangle(0, (그림변환된높이 - 1) * i, 1280, 그림변환된높이), new Rectangle(1, 1, 그림파일.Width - 2, 그림파일.Height - 2), GraphicsUnit.Pixel); // -1은 약간 줄여준다는 의미




			}

			그림파일.Dispose();
		}
	}
}
/*
CString CKingOfWord::WORD_Pass(CString src)
{
	CString src_space;				// 한 단어씩 받아들였기 때문에 출력시에는 한 칸식 떼어야 한다.
	src_space.Format("%s ", src);

	return src_space;
}
*/
/*
// 하이픈으로 연결되어 있는 단어를 처리한다.
CString CKingOfWord::WORD_Dbls(CString src)
{
	CString ret, sLeft, sRight;

	while(src.Find("-") != -1)
	{
		sLeft = src.Left(src.Find("-") );
		ret += WORD_Txt(sLeft);

		sRight = src.Mid(src.Find("-") + 1);
		src = sRight;
	}

	if(sRight != "")
		ret += WORD_Txt(sRight);


//	AfxMessageBox(ret);

	ret.Trim();
	ret.Replace(" ", "-");
	ret += " ";				// 띄어쓰기는 있어야 한다.
//	AfxMessageBox(ret);
	return ret;
}
*/
/*
CString CKingOfWord::WORD_Score(CString basicForm, CString popularForm)
{
	CString sScore;

	// 1. 한글이 아니어야 함.
	// 2. 사전에 있는 단어여야 함.
	// 3. 3글자보다 큰 단어여야 함.
	if(_fd.FindWordBook(popularForm) != "")
	{

		int Score_Empas		= Difficulty(_fd.Find_Score_Empas(popularForm));
		int Score_Google	= Difficulty(_fd.Find_Score_Google(popularForm));

		if((Score_Empas > 20) && (Score_Google > 20)) // 상.
		{
			_점수_총합_DifficultWords++;
		}

		int CurScore_Empas = 0;
		int CurScore_Google = 0;

		CurScore_Empas = Score_Empas;
		CurScore_Google = Score_Google;

		_점수_총합_Empas += Score_Empas;
		_점수_총합_Google += Score_Google;

		_점수_총합_Words++;



		CString scoreBasicForm;
		int score = _fd.ScoreVariation(basicForm, scoreBasicForm);
		_점수_총합 += score;
		sScore.Format("(%d-%s)", score, scoreBasicForm);

		if(_옵션_단어난이도표시)
			return sScore;
	}

	return "";
}
*/

/*
// 이 부분이 기호 전처리의 핵심이다.
CString CKingOfWord::CORE_PRE(CString src)
{
	CString srcF = src;
	srcF.MakeLower();

	CString DeleteList[31] = {
		"?",	".",	",",	"\"",	"!",	"[",	"]",	"(",	")",	">",
		"<",	"_",	"1",	"2",	"3",	"4",	"5",	"6",	"7",	"8",
		"9",	"0",	":",	";",	"*",	"/",	"%",	"$",	"~",	"|",
		"^"};

	for(int i=0 ; i < 31 ; i++)		srcF.Replace(DeleteList[i], "");


	for(int i = 0 ; i < 3 ; i++)
	{
		//어포스트로피와 인용문을 구별하자
		// "\'"
		// "`"
		// "’"
		if(srcF.GetLength() > 0){		if(srcF.Left(1) == "\'")		srcF = srcF.Mid(1);	}
		if(srcF.GetLength() > 0){		if(srcF.Left(1) == "`")			srcF = srcF.Mid(1);	}
		if(srcF.GetLength() > 1){		if(srcF.Left(2) == "’")		srcF = srcF.Mid(2);	}

		if(srcF.GetLength() > 0){		if(srcF.Right(1) == "\'")		srcF = srcF.Mid(srcF.GetLength() - 1 );	}
		if(srcF.GetLength() > 0){		if(srcF.Right(1) == "`")		srcF = srcF.Mid(srcF.GetLength() - 1 );	}
		if(srcF.GetLength() > 1){		if(srcF.Right(2) == "’")		srcF = srcF.Mid(srcF.GetLength() - 2 );	}
	}

	return srcF;
}
*/
/*
		string WORD_Txt(string src)
		{
			string ret;

			//string srcF = CORE_PRE(src);
			string findWord, dont;																		// 실제로 어떤 단어를 찾았는지 확인함.

			int num					= _fd.Find_Stat_Sunung_Variation(srcF, dont, findWord);				// 주어진 단어의 수능 출현 빈도

			CString basicForm		= _fd.FindBasicForm(findWord);										// 기본형 검색
			CString popularForm		= _fd.FindPopularForm(findWord);									// 변이형 중 출현빈도가 높은 단어를 가져온다.
			CString W				= _fd.FindWordBookVariation(srcF, basicForm);						// 현재단어에 대한 임시단어장



			CString tooltip_first, tooltip_last;
			CString meaning;
			meaning = _fd.FindMeaning(basicForm);



			if(num == -2)					{			ret = CORE_WORD_LV01(src); return ret;}			// 수능 빈도수 함수에서 걸러짐
			else if(W.Find("∴") != -1)		{			ret = CORE_WORD_LV02(src); return ret;}			// 사전에 불필요하다고 쓰인 단어
			else if(W == "")				{			ret = CORE_WORD_LV03(src); return ret;}			// 사전에 없는 단어

			else if(num == -1 || num == 0)				ret = tooltip_first + CORE_WORD_LV04(src, W, basicForm)	+ tooltip_last;		// 중요하지 않은 단어
			else if((num < 3) && (_옵션_우등생 == FALSE))	ret = tooltip_first + CORE_WORD_LV05(src, W, basicForm)	+ tooltip_last;		// 적당히 중요한 단어
			else if(dont == "warning")  				ret = tooltip_first + CORE_WORD_LV06(src, W, basicForm)	+ tooltip_last;		// 상당히 중요한 단어
			else										ret = tooltip_first + CORE_WORD_LV07(src)				+ tooltip_last;		// 일반 단어

			ret += WORD_Score(basicForm, popularForm);

			return ret;


		}

	}
}
*/


/*

CString CKingOfWord::All(CString src, bool wordbook)
{


		HTML결과물 += LINE_MAKE_PAGE(i,현재까지만들어본_컨텐츠, 현재까지만들어본_단어장, 현재까지만들어진CAKE단위로딱떨어지는페이지, 현재까지만들어진CAKE단위로딱떨어지는페이지_그때의줄번호, 한줄전_만들어진내용, 한줄전_만들어진_단어장);
	}



	HTML결과물 += LINE_MAKE_LAST_PAGE(현재까지만들어본_컨텐츠, 현재까지만들어본_단어장);

	if(_옵션_정답뒤로){ HTML결과물 +=  Html형식정답만들기(src);}

	HTML결과물 += "</body>\r\n";
	HTML결과물 += "</html>\r\n";

	LINE_SCORE();

	return HTML결과물;
}


// 뒤에 정답페이지를 만드는 옵션이다.
// str
// AnswerHtml;
CString CKingOfWord::Html형식정답만들기(CString str)
{
	int nQ = 0;

	CStringArray ar;
	_cv.변환_CString을_개행문자단위_문자열들로(ar, str);
	CString question, answer, curQ, line, curAnswerNum, AnswerNumLines;
	CString CheckQ; // 해설에 정답이라는 말이 나오면, 실수로 정답 번호인 줄 알고, 또 정답을 표시한다.
					// 이를 막기 위해서, 문항당 정답은 한 번만 표시하도록 한다.

	bool bTR = false;
	bool b정답찾음 = false;


	AnswerNumLines +=	"<table cellpadding=0 cellspacing=0 border=1 bordercolor=eeeeee>\r\n";	// 정답표
	AnswerNumLines +=	"\t<tr>\r\n";

	for(int i = 0 ; i < ar.GetCount(); i++)
	{
		line = ar.GetAt(i);

		// CAKE로 시작해서
		// Q 나오면 번호 확인하고
		// TR 나올 때부터 저장해서
		// /TR 나올 때까지 저장하고
		if(line.Find("<Q>") != -1)
		{
			curQ = GetQ(line);
		}
		else if(line.Find("<TR>") != -1)
		{
			bTR = true;
			b정답찾음 = false;
			answer += curQ + "\r\n";
		}
		// 해설에 정답이라는 말이 나오면 처리를 못하는 문제점이 있다.
		 // 정답이 단순 숫자인 경우
		else if((line.Find("정답") != -1) && (GetAnswerNum(line) != "") && (CheckQ != curQ))
		{
			b정답찾음 = true;

			answer += line + "\r\n";
			curAnswerNum = GetAnswerNum(line);

			CString tmp;
			tmp.Format("\t\t<td width=13 align=center bgcolor=dddddd><span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:10pt;font-color:black;\">%s</span></td>\r\n\t\t<td width=13 align=center><span style=\"font-family:바탕;font-size:7pt;line-height:10pt;font-color:black;\">%s</span></td>\r\n", curQ, curAnswerNum);
			nQ++;

			CheckQ = curQ;

			AnswerNumLines += tmp;
			if(nQ % 7 == 0)	AnswerNumLines += "\t</tr>\r\n\t<tr>\r\n";
		}
		// 정답이 단순 숫자가 아닌 경우.
		else if((line.Find("정답") != -1) && (GetAnswerNum(line) == "") && (CheckQ != curQ))
		{
			b정답찾음 = true;

			answer += line + "\r\n";

			CString tmp;
			tmp.Format("\t\t<td width=13 align=center bgcolor=dddddd><span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:10pt;font-color:black;\">%s</span></td>\r\n\t\t<td width=13 align=center><span style=\"font-family:바탕;font-size:7pt;line-height:10pt;font-color:black;\"> </span></td>\r\n", curQ);
			nQ++;

			CheckQ = curQ;

			AnswerNumLines += tmp;
			if(nQ % 7 == 0) AnswerNumLines += "</tr>\r\n\t<tr>";
		}

		else if(line.Find("</TR>") != -1)
		{
			// 정답이 없는 경우에는 정답을 공란으로 만듭니다. // 46- 49- bug
			if(b정답찾음 == false && (curQ.Find("~") != -1))
			{
				CString tmp;
				tmp.Format("\t\t<td width=13 align=center bgcolor=dddddd><span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:10pt;font-color:black;\">%s</span></td>\r\n\t\t<td width=13 align=center><span style=\"font-family:바탕;font-size:7pt;line-height:10pt;font-color:black;\"> </span></td>\r\n", curQ);
				nQ++;
				AnswerNumLines += tmp;

				if(nQ % 7 == 0)					AnswerNumLines += "</tr>\r\n\t<tr>";

			}

			bTR = false;
			answer += "\r\n";



		}
		else if(bTR)
		{
			if(line.Find("<TBAR></TBAR>") == -1)
				answer += line + "\r\n";
		}
		else
		{
//			question += line + "\r\n";
		}

	}
	AnswerNumLines += "\t</tr>\r\n";
	AnswerNumLines += "</table>\r\n";

	return MakeAnswerHtml(AnswerNumLines, answer);

}


// 73칸을 세칸으로 나누어야 한다.
// 정답은 초기에 8칸을 차지한다.
CString CKingOfWord::MakeAnswerHtml(CString AnswerNumLines, CString answer)
{
	int nLineLimit = 100;

	CString res;
	CStringArray 너비가_일정한_문자열들;
	_cv.변환_CString을_너비가_일정한_문자열들(너비가_일정한_문자열들, answer , 50);

	CString answerHtml;
	int nLineIdx  = 12;
	int nCurState = 0;

	answerHtml += "<table cellpadding=0 cellspacing=0  border=0 height=975>\r\n";				//975 이 수치 상당한 시행착오로 얻어낸 것이다.바꿀거면 각오하도록
	answerHtml += "\t<tr>\r\n";
	answerHtml += "\t<td valign=top>\r\n";
	answerHtml += AnswerNumLines + "\r\n";

	for(int i = 0 ; i < 너비가_일정한_문자열들.GetCount() ; i++)
	{

		if(nLineIdx == 0)
		{
			if(nCurState == 0)
			{
				answerHtml += "<table cellpadding=0 cellspacing=0  border=0 height=975>\r\n";
				answerHtml += "\t<tr>\r\n";
				answerHtml += "\t<td valign=top>\r\n";
			}
			else if(nCurState == 1)
			{
				answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
				answerHtml += "\t<td valign=top>\r\n";
			}
			else if(nCurState == 2)
			{
				answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
				answerHtml += "\t<td valign=top>\r\n";
			}
		}

		answerHtml += AnswerHtmlLine(너비가_일정한_문자열들.GetAt(i));

		if(nLineIdx == nLineLimit)
		{
			if(nCurState == 0)
			{
				answerHtml += "\t</td>\r\n";
			}
			else if(nCurState == 1)
			{
				answerHtml += "\t</td>\r\n";
			}
			else if(nCurState == 2)
			{
				answerHtml += "\t</td>\r\n";
				answerHtml += "\t</tr>\r\n";
				answerHtml += "</table>\r\n";
			}

			if(nCurState == 2)
				nCurState = 0;
			else
				nCurState++;
		}

		if(nLineIdx == nLineLimit)
			nLineIdx = 0;
		else
			nLineIdx++;
	
	}

	if(nCurState == 0 && nLineIdx != 0)
	{
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
		answerHtml += "\t<td>\r\n";
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
		answerHtml += "\t<td>\r\n";
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t</tr>\r\n";
		answerHtml += "</table>\r\n";
	}
	else if(nCurState == 1 && nLineIdx != 0)
	{
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
		answerHtml += "\t<td>\r\n";
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t</tr>\r\n";
		answerHtml += "</table>\r\n";
	}
	else if(nCurState == 1 && nLineIdx != 0)
	{
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t</tr>\r\n";
		answerHtml += "</table>\r\n";
	}
	else if(nCurState == 0 && nLineIdx == 0)
	{
	}
	else if(nCurState == 1 && nLineIdx == 0)
	{
		answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
		answerHtml += "\t<td>\r\n";
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
		answerHtml += "\t<td>\r\n";
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t</tr>\r\n";
		answerHtml += "</table>\r\n";
	}
	else if(nCurState == 2 && nLineIdx == 0)
	{
		answerHtml += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
		answerHtml += "\t<td>\r\n";
		answerHtml += "\t</td>\r\n";
		answerHtml += "\t</tr>\r\n";
		answerHtml += "</table>\r\n";
	}


//	res += AnswerNumLines + "\r\n" + answerHtml;

	return answerHtml;
}

CString CKingOfWord::AnswerHtmlLine(CString CurLine)
{

	// 2. 영어 지문인 경우.…………………………………………………………………………………………………
	CString			CandyMan;	// 두껍기도 하고 휘기도 한 글자들.
	CStringArray	wordArray;
	CString			html_source;

	_cv.CVT_LineToWordArray(wordArray, CurLine);

	if(CurLine != "")
	{
		for(int j = 0 ; j < wordArray.GetCount(); j++)
		{
			if(wordArray.GetAt(wordArray.GetCount() -1 ) == "¶")
			{
				if(j == 0)									
				{	
					CandyMan += "\t\t\t<td align=left>";
					CandyMan += "<span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:7pt;font-color:black;\">";
					CandyMan += wordArray.GetAt(j); 
					CandyMan += "</span>";
					CandyMan += "</td>";
				}
				else if(j == wordArray.GetCount() - 2)
				{
					CandyMan += "<td align=right>";
					CandyMan += "<span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:7pt;font-color:black;\">";
					CandyMan += wordArray.GetAt(j); 
					CandyMan += "</span>";
					CandyMan += "</td>";
				}
				else if(j == wordArray.GetCount() - 1)
				{	
				}
				else										
				{	
					CandyMan += "<td align=center>";
					CandyMan += "<span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:7pt;font-color:black;\">";
					CandyMan += wordArray.GetAt(j); 
					CandyMan += "</span>";
					CandyMan += "</td>";
				}

			}
			else
			{
					CandyMan += "<span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:7pt;font-color:black;\">" + wordArray.GetAt(j) + " </span>";
			}
		}


		html_source += "\r\n";
		html_source += "\t\t\t<table width=225 height=7 border=0 cellpadding=0 cellspacing=0>\r\n";
		html_source += "\t\t\t<tr>\r\n";
		if(wordArray.GetAt(wordArray.GetCount() -1 ) != "¶")	html_source += "\t\t\t<td width=225>";

		html_source += CandyMan;

		if(wordArray.GetAt(wordArray.GetCount() -1 ) != "¶")	html_source += "</td>";

		html_source += "\t\t\t</tr>\r\n";
		html_source += "\t\t\t</table>\r\n";
	}
	else
	{
		html_source += "\t\t\t<table width=225 height=7 border=0 cellpadding=0 cellspacing=0>\r\n";
		html_source += "\t\t\t<tr>\r\n";
		html_source += "<td><span style=\"font-family:Malgun Gothic;font-size:7pt;line-height:7pt;font-color:black;\">&nbsp;</span></td>";
		html_source += "\t\t\t</tr>\r\n";
		html_source += "\t\t\t</table>\r\n";
	}
		return html_source;
}

CString CKingOfWord::GetAnswerNum(CString line)
{
	if(line.Find("①") != -1) return "①";
	if(line.Find("②") != -1) return "②";
	if(line.Find("③") != -1) return "③";
	if(line.Find("④") != -1) return "④";
	if(line.Find("⑤") != -1) return "⑤";
	return "";
}


// 현재위치가 <CAKE>에 있을 때에 미리 현재 정답을 조사해 둡니다.
// 이후 <CAKE>...</CAKE> 태그 내에서 언제든지 호출해서 쓸 수 있는 함수입니다.
// 특별한 정답 표시가 없는 경우, 0이 되며,
// 정답이 1번인 경우에는 1
// 정답이 2번인 경우에는 2
// 정답이 3번인 경우에는 3
// 정답이 4번인 경우에는 4
// 정답이 5번인 경우에는 5를 각각 내보냅니다.
int CKingOfWord::GetCurCakeAnswer()
{
	return _nRightAnswer;
}

// 현재위치가 <CAKE>에 있을 때에 미리 <T> ... </T>를 조사해 둡니다.
// 이후 <CAKE>...</CAKE> 태그 내에서 언제든지 호출해서 쓸 수 있는 함수입니다.
CString CKingOfWord::GetCurCakeT()
{
	return _t;
}

// <CAKE>내에 있는 TEXT마다의 고유한 DNA값을 가져옵니다.
// 이 값은 <CAKE>가 시작할 때 미리 추출하여 둡니다.
int CKingOfWord::GetCurCakeDNA()
{
	return _dna;
}

// 현재 질문이 담긴 항목의 번호를 추출합니다.

// 입력 : "<Q> 45. 다음 글의 내용을 한 문장으로 요약하고자 한다. 빈칸 (A)와 (B)에 들어갈 말로 가장 적절한 것끼리 짝지은 것은? [3점] </Q>"
// 출력 : "45"

CString CKingOfWord::GetQ(CString q)
{
	CString Old, New;
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	for(int i = 100; i < 1000; i++)	{	Old.Format("%d.", i);		New.Format("%d.", i);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("%d.", i);		New.Format("%d.", i);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("%d.", i);		New.Format("%d.", i);		if(q.Find(Old)!= -1) return New;	}
	//……………………………………………………………………………………………………………………………………………………………………………………………………………
	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d - %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d - %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d - %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d - %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d - %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d - %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d - %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d - %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d - %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d - %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d - %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d - %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d - %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d - %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d - %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	//……………………………………………………………………………………………………………………………………………………………………………………………………………
	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d-%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d-%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d-%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d-%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d-%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d-%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d-%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d-%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d-%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d-%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d-%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d-%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d-%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d-%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d-%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	//……………………………………………………………………………………………………………………………………………………………………………………………………………
	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ～ %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ～ %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ～ %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ～ %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ～ %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ～ %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ～ %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ～ %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ～ %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ～ %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ～ %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ～ %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ～ %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ～ %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ～ %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	//……………………………………………………………………………………………………………………………………………………………………………………………………………
	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d～%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d～%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d～%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d～%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d～%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d～%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d～%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d～%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d～%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d～%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d～%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d～%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d～%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d～%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d～%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	//……………………………………………………………………………………………………………………………………………………………………………………………………………
	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ~ %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ~ %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ~ %d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ~ %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ~ %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ~ %d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ~ %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ~ %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ~ %d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ~ %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ~ %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ~ %d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d ~ %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d ~ %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d ~ %d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	//……………………………………………………………………………………………………………………………………………………………………………………………………………
	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d~%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d~%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d~%d]", i,i+1);		New.Format("[%d～%d]", i,i+1);		if(q.Find(Old)!= -1) return New;	}
	
	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d~%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d~%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d~%d]", i,i+2);		New.Format("[%d～%d]", i,i+2);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d~%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d~%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d~%d]", i,i+3);		New.Format("[%d～%d]", i,i+3);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d~%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d~%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d~%d]", i,i+4);		New.Format("[%d～%d]", i,i+4);		if(q.Find(Old)!= -1) return New;	}

	for(int i = 100; i < 1000; i++)	{	Old.Format("[%d~%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("[%d~%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("[%d~%d]", i,i+5);		New.Format("[%d～%d]", i,i+5);		if(q.Find(Old)!= -1) return New;	}
	//……………………………………………………………………………………………………………………………………………………………………………………………………………
	for(int i = 100; i < 1000; i++)	{	Old.Format("%d", i);		New.Format("%d.", i);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 10; i < 100; i++)	{	Old.Format("%d", i);		New.Format("%d.", i);		if(q.Find(Old)!= -1) return New;	}
	for(int i = 1; i < 10; i++)		{	Old.Format("%d", i);		New.Format("%d.", i);		if(q.Find(Old)!= -1) return New;	}

	return "?";
}



CString CKingOfWord::LINE_PIC(CString CurLine, CString src)
{
	CString HTML결과;
	int nRate;
	CString Rate;

	CString sImgHeight;
	CString	sImgPath;
	int		nImgHeight = 0;
	CurLine.Trim(); // 앞부분에서 탭을 일괄적으로 띄어쓰기로 바꾸는 부분이 있어서, 아래의 코드만으로는 불충분함
//	CurLine.Replace("\t", "");
	CurLine.Replace("그림입력|", "");

	// 수동으로 그림의 높이를 조절하는 부분입니다.
	// 그림입력|10|.\picced0.gif에서 10과 같은 숫자를 넣어서 수동으로 입력합니다. 
	if(CurLine.Find("|") != -1)
	{
		sImgHeight = CurLine.Left(src.Find("|"));
		nImgHeight = atoi((const char*)sImgHeight);
		sImgPath = CurLine.Mid(CurLine.Find("|") + 1) ;
	}
	else
	{
		sImgPath = CurLine;
		nImgHeight = GetImageHeightBR(sImgPath, nRate);
	}
	

	Rate.Format("%d%%", nRate);
	if(_상태_해설 == true && _옵션_정답표시 == TRUE)
	{
		if(_옵션_다단 == TRUE)
			HTML결과 += "<table width=330 height=15 border=0 cellpadding=0 cellspacing=0><tr><td width=1 bgcolor=black></td><td width=328 bgcolor=white>\r\n";
		else
			HTML결과 += "<table width=700 height=15 border=0 cellpadding=0 cellspacing=0><tr><td width=1 bgcolor=black></td><td width=698 bgcolor=white>\r\n";				
		
		HTML결과 += "\t\t\t<table style=\"background-image:url(\'";
		HTML결과 += sImgPath;

		if(_옵션_다단 == TRUE)
			HTML결과 += "'); background-repeat:no-repeat; background-position:top left;" + Rate + "\" width=328 cellpadding=0 cellspacing=0><tr><td>\r\n";
		else
			HTML결과 += "'); background-repeat:no-repeat; background-position:top left;" + Rate + "\" width=698 cellpadding=0 cellspacing=0><tr><td>\r\n";

		for(int iImgHeight = 0 ; iImgHeight < nImgHeight ; iImgHeight++)
		{
			HTML결과 += "\t\t\t<br>";
		}
		HTML결과 += "\t\t\t</td></tr></table><!--그림을 삽입하기 위한 테이블을 마침-->";

		HTML결과 += "<br></td><td width=1 bgcolor=black></td></tr></table>\r\n";


	}
	else if(_상태_해설 == true && _옵션_정답표시 == FALSE) // 정답을 보여주지 않는 옵션인 경우.
	{}
	else
	{
		CString html_height;

		html_height.Format("%d", nImgHeight * 20);

		if(nRate != 100)
			HTML결과 += "<table border=0 align=center><tr><td><img src=\'"+ sImgPath + "\' width=300 height=" + html_height + "></td></tr></table>\r\n";
		else
			HTML결과 += "<table border=0 align=center><tr><td><img src=\'"+ sImgPath + "\' height=" + html_height + "></td></tr></table>\r\n";

		for(int iImgHeight = 0 ; iImgHeight < nImgHeight ; iImgHeight++){		HTML결과 += "<!--<br>-->";	}
	}

	return HTML결과;
}


//
//
//
//
//
// nIndex값에는 현재 처리하고 있는 라인의 줄 숫자가 들어옵니다.
//………………………………………………………………………………………………
// 너비가_일정한_문자열들에는 다음과 같은 형태로 내용이 들어가게 됩니다.
// nIndex의 i값에 들어오는 것은 현재 <CAKE> 태그의 줄 번호 숫자 입니다.
//
// 0:	<CAKE>
// 1:		<Q> ... </Q>
// 2:		<T>
// 3:		...
// 4:		</T>
// 5:		<TR>
// 6:		정답 ②번
// 7:		...
// 8:		</TR>
// 9:	</CAKE>
// 10:	<CAKE>					<---------------- 현재 처리하는 곳 (예시)
// 11:		<Q> ... </Q>
// 12:		<T>
// 13:		...
// 14:		</T>
// 15:		<TR>
// 16:		정답 ②번
// 17:		...
// 18:		</TR>
// 19:	</CAKE>
// 20:	<CAKE>
// 21:		<Q> ... </Q>
// 22:		<T>
// 23:		...
// 24:		</T>
// 25:		<TR>
// 26:		정답 ②번
// 27:		...
// 28:		</TR>
// 29:	</CAKE>
//………………………………………………………………………………………………

//-------------------------------------------------------------------------------------------------------------------
// 현재 CAKE 내에 있는 <T></T>태그의 내용을 알아옵니다.
// 이 함수는 <CAKE>태그가 시작될 때 실행시킵니다.
// 이 함수의 결과를 알아오기 위해서는, GetCurCakeAnswer() 함수를 사용합니다.
//-------------------------------------------------------------------------------------------------------------------
void CKingOfWord::FindCurCakeAnswer(CStringArray& 너비가_일정한_문자열들, int nIndex)
{
	int nCurrentIndex;

	// 현재 CAKE 내에 있는 문제의 정답을 미리 알아냅니다.---------------------------------------------------------------
	_nRightAnswer = 0;

	nCurrentIndex = nIndex;
	while(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("</CAKE>") == -1) // </CAKE>문의 맨 끝에 도달하지만 않았다면,
	{
		if(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("정답 ①번") != -1){	_nRightAnswer = 1;	}
		if(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("정답 ②번") != -1){	_nRightAnswer = 2;	}
		if(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("정답 ③번") != -1){	_nRightAnswer = 3;	}
		if(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("정답 ④번") != -1){	_nRightAnswer = 4;	}
		if(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("정답 ⑤번") != -1){	_nRightAnswer = 5;	}

		nCurrentIndex++;
	}
}

//-------------------------------------------------------------------------------------------------------------------
// 현재 CAKE 내에 있는 <T></T>태그의 내용을 알아옵니다.
// 이 함수는 <CAKE>태그가 시작될 때 실행시킵니다.
// 이 함수의 결과를 알아오기 위해서는, GetCurCakeT() 함수를 사용합니다.
//-------------------------------------------------------------------------------------------------------------------
void CKingOfWord::FindCurCakeT(CStringArray& 너비가_일정한_문자열들, int nIndex)
{
	int nCurrentIndex;

	_t = "";

	bool bT; 	bT = false;

	nCurrentIndex = nIndex;
	while(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("</CAKE>") == -1) // </CAKE>문의 맨 끝에 도달하지만 않았다면,
	{
		if(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("</T>") != -1)		// <T>태그를 발견하였습니다.
			bT = false;

		if(bT == true)
		{
			_t += 너비가_일정한_문자열들.GetAt(nCurrentIndex);
			_t += " ";

			_t.Replace(" ¶","");
		}

		if(너비가_일정한_문자열들.GetAt(nCurrentIndex).Find("<T>") != -1)		// <T>태그를 발견하였습니다.
			bT = true;

		nCurrentIndex++;
	}
}

void CKingOfWord::FindCurCakeDNA()
{
	CStringArray wordArray;
	CString word;
	_cv.CVT_CStringToWordArray(wordArray, _t);

	if(wordArray.GetCount() > 2)
	{
		word = wordArray.GetAt(1);
	}

	CString key; 	
	if(word != "")	{key = word.Left(1);}
	else			{key = "";}

//	key.MakeUpper();를 사용하면 한글이 들어왔을 경우 에러납니다.

	if(		key == "A")	_dna = 2;
	else if(key == "B")	_dna = 0;
	else if(key == "C")	_dna = 2;
	else if(key == "D")	_dna = 0;
	else if(key == "E")	_dna = 3;
	else if(key == "F")	_dna = 3;
	else if(key == "G")	_dna = 3;
	else if(key == "H")	_dna = 4;
	else if(key == "I")	_dna = 2;
	else if(key == "J")	_dna = 2;
	else if(key == "K")	_dna = 4;
	else if(key == "L")	_dna = 2;
	else if(key == "M")	_dna = 2;
	else if(key == "N")	_dna = 2;
	else if(key == "O")	_dna = 0;
	else if(key == "P")	_dna = 1;
	else if(key == "Q")	_dna = 2;
	else if(key == "R")	_dna = 2;
	else if(key == "S")	_dna = 2;
	else if(key == "T")	_dna = 3;
	else if(key == "U")	_dna = 2;
	else if(key == "V")	_dna = 2;
	else if(key == "W")	_dna = 2;
	else if(key == "X")	_dna = 4;
	else if(key == "Y")	_dna = 3;
	else if(key == "Z")	_dna = 2;
	else if(key == "a")	_dna = 2;
	else if(key == "b")	_dna = 0;
	else if(key == "c")	_dna = 2;
	else if(key == "d")	_dna = 0;
	else if(key == "e")	_dna = 3;
	else if(key == "f")	_dna = 3;
	else if(key == "g")	_dna = 3;
	else if(key == "h")	_dna = 4;
	else if(key == "i")	_dna = 2;
	else if(key == "j")	_dna = 2;
	else if(key == "k")	_dna = 4;
	else if(key == "l")	_dna = 2;
	else if(key == "m")	_dna = 2;
	else if(key == "n")	_dna = 2;
	else if(key == "o")	_dna = 0;
	else if(key == "p")	_dna = 1;
	else if(key == "q")	_dna = 2;
	else if(key == "r")	_dna = 2;
	else if(key == "s")	_dna = 2;
	else if(key == "t")	_dna = 3;
	else if(key == "u")	_dna = 2;
	else if(key == "v")	_dna = 2;
	else if(key == "w")	_dna = 2;
	else if(key == "x")	_dna = 4;
	else if(key == "y")	_dna = 3;
	else if(key == "z")	_dna = 2;
	else _dna = -1;
}

CString CKingOfWord::LINE_CAKE_END(int SentenceIndex, int& 현재까지만들어진CAKE단위로딱떨어지는페이지_그때의줄번호, CString& 현재까지만들어진CAKE단위로딱떨어지는페이지, CString& 한줄전_만들어진내용, CString& 한줄전_만들어진_단어장)
{
	_상태_CAKE = false;
	// 가장 중요하고 어려운 부분이다.
	// 이곳 때문에 <CAKE>단위로 띄어쓰기가 가능한 것이며, 이곳 때문에 단어가 다시 한 번씩 나온다.

	현재까지만들어진CAKE단위로딱떨어지는페이지 = PostProcess(한줄전_만들어진내용);

	현재까지만들어진CAKE단위로딱떨어지는페이지 += 한줄전_만들어진_단어장;
	현재까지만들어진CAKE단위로딱떨어지는페이지_그때의줄번호 = SentenceIndex;
	
	// 그 당시의 격자 상태를 기억해 주어야 한다.
//	_페이지넘어가서되돌릴경우의예전_상태_지문 = _상태_지문;
//	_페이지넘어가서되돌릴경우의예전_상태_해설 = _상태_해설;

	// 왼쪽에 나오는 단어장 고치는 파일을 만들기 위한 코드이다.
	// CAKE 단위로 페이지가 나뉘어지기 때문에, 
	// 같은 곳을 두번 이상 처리할 수 있는데,
	// 이 때 CAKE 단위로 단어장을 기재하며, 남은 부분은 어차피 다시 한번
	// 돌게 되므로 지운다.
	// 지우는 부분은 맨 뒤에 줄 넘어가는 부분에 있다.

	for(int nManualEditTmp = 0 ; nManualEditTmp < _saWord_Book_Manual_Edit_tmp.GetSize() ; nManualEditTmp++)
		_saWord_Book_Manual_Edit.Add(_saWord_Book_Manual_Edit_tmp.GetAt(nManualEditTmp));

	_saWord_Book_Manual_Edit_tmp.RemoveAll();

	return "<br><!-- LINE CAKE END -->\r\n";
}

CString CKingOfWord::LINE_T_BGN()
{
	CString T1;

	if(_옵션_다단 == TRUE)
		T1 += "\t\t\t<img src=\"C:\\BluePill\\top_new_330.gif\" align=absbottom><br>\r\n";
	else
		T1 += "<img src=\"C:\\BluePill\\top_new.gif\" align=absbottom><br>\r\n";

	_상태_지문	= true;


	// 텍스트 난이도 체크 시작.
	_점수_총합_DifficultWords = 0;
	_점수_총합_Empas = 0;
	_점수_총합_Google = 0;
	_점수_총합_Words = 0;

	_점수_총합 = 0;

	return T1;
}

CString CKingOfWord::LINE_T_BAR()
{
	CString T1;

	if(_옵션_정답표시 == FALSE && _상태_해설 == true) 
	{
		// 정답을 보여주지 않으려 할 때, 
		// 정답부분의 바는 필요없음.
	}
	else if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == TRUE && _상태_해설 == true)
	{
		// 정답을 일괄적으로 뒤로 보내려고 하며, 현재가 정답 부분을 처리하는 구간이라면,
		// 정답부분의 바를 보여주어서는 안됩니다.
	}
	else
	{

		if(_옵션_다단 == TRUE)
			T1 += "<img src=\"C:\\BluePill\\tbar_new_330.gif\" align=absbottom><br>\r\n";
		else
			T1 += "<img src=\"C:\\BluePill\\tbar_new.gif\" align=absbottom><br>\r\n";


	}
	return T1;
}

CString CKingOfWord::LINE_T_END()
{
	CString sScore;

	int nScore = 0;
	int nScoreGoogle = 0;
	int nScoreEmpas = 0;
	int nDifficultPercentage = 0;

	if(_점수_총합_Words != 0)
	{
		nScore = (_점수_총합 * 20) / _점수_총합_Words;
	}
	else
	{
		nScore = 0;
	}


	sScore.Format("%d\r\n", nScore);

	
//----------------------------

	CString T2;

	if(_옵션_다단 == TRUE)
	{
		if(nScore > 100)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_10.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 90)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_9.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 80)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_8.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 70)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_7.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 60)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_6.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 50)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_5.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 40)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_4.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 30)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_3.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 20)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_2.gif\" align=absbottom><br>\r\n\r\n";
		else if(nScore > 10)
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_1.gif\" align=absbottom><br>\r\n\r\n";
		else
			T2 += "\r\n\t\t\t<img src=\"C:\\BluePill\\bottom_new_330_0.gif\" align=absbottom><br>\r\n\r\n";
	}
	else
	{
		if(nScore > 100)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_10.gif\" align=absbottom><br>\r\n";
		else if(nScore > 90)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_9.gif\" align=absbottom><br>\r\n";
		else if(nScore > 80)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_8.gif\" align=absbottom><br>\r\n";
		else if(nScore > 70)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_7.gif\" align=absbottom><br>\r\n";
		else if(nScore > 60)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_6.gif\" align=absbottom><br>\r\n";
		else if(nScore > 50)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_5.gif\" align=absbottom><br>\r\n";
		else if(nScore > 40)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_4.gif\" align=absbottom><br>\r\n";
		else if(nScore > 30)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_3.gif\" align=absbottom><br>\r\n";
		else if(nScore > 20)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_2.gif\" align=absbottom><br>\r\n";
		else if(nScore > 10)
			T2 += "<img src=\"C:\\BluePill\\bottom_new_1.gif\" align=absbottom><br>\r\n";
		else
			T2 += "<img src=\"C:\\BluePill\\bottom_new_0.gif\" align=absbottom><br>\r\n";

	}
	_상태_지문	= false;

	return T2;
}

CString CKingOfWord::LINE_TR_BGN()
{
	CString T1;

	if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == FALSE)
	{

		if(_옵션_다단 == TRUE)
			T1 += "<img src=\"C:\\BluePill\\top_new_330.gif\" align=absbottom><br>\r\n";
		else
			T1 += "<img src=\"C:\\BluePill\\top_new.gif\" align=absbottom><br>\r\n";

		_상태_해설	= true;

	}
	else if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == TRUE)
	{
		_상태_해설	= true;
	}
	else
	{
		_상태_해설	= true;
	}

	return T1;
}

CString CKingOfWord::LINE_TS_BGN()
{
	CString T1;

	if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == FALSE)
	{
		if(_옵션_다단 == TRUE)
			T1 += "<img src=\"C:\\BluePill\\top_new_330.gif\" align=absbottom><br>\r\n";
		else
			T1 += "<img src=\"C:\\BluePill\\top_new.gif\" align=absbottom><br>\r\n";

		_상태_해설	= true;

	}
	else if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == TRUE)
	{
		_상태_해설	= true;
	}
	else
	{
		_상태_해설	= true;
	}

	return T1;
}

CString CKingOfWord::LINE_TS_END()
{
	CString T2;

	if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == FALSE)
	{
		if(_옵션_다단 == TRUE)
			T2 += "<img src=\"C:\\BluePill\\bottom_TR_new_330.gif\" align=absbottom><br>\r\n";
		else
			T2 += "<img src=\"C:\\BluePill\\bottom_TR_new.gif\" align=absbottom><br>\r\n";

		_상태_해설	= false;

	}
	else if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == TRUE)
	{
		_상태_해설	= false;
	}
	else
	{
		_상태_해설	= false;
	}

	return T2;
}


CString CKingOfWord::LINE_TR_END()
{
	CString T2;

	if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == FALSE)
	{
		if(_옵션_다단 == TRUE)
			T2 += "<img src=\"C:\\BluePill\\bottom_TR_new_330.gif\" align=absbottom><br>\r\n";
		else
			T2 += "<img src=\"C:\\BluePill\\bottom_TR_new.gif\" align=absbottom><br>\r\n";

		_상태_해설	= false;
	}
	else if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == TRUE)
	{
		_상태_해설	= false;
	}
	else
	{
		_상태_해설	= false;
	}

	return T2;
}



CString CKingOfWord::LINE_TR(CString CurLine)
{
	CString HTML결과;

	if(_옵션_정답표시 == FALSE)							return "";	// 정답표시 버튼이 체크되어 있지 않은 경우에는 정답을 표시할 필요가 없다.

	if(_옵션_정답표시 == TRUE && _옵션_정답뒤로 == TRUE)	return ""; // 정답표시 버튼이 체크되어 있고, 정답을 뒤로 빼는 경우에는,
															// 지금 당장은 정답을 표시할 필요가 없습니다.


//	if(HTML결과 == "")
//		HTML결과 += "<br><!-- LINE_TR-->\r\n";

	HTML결과 += "\r\n";

		if(_옵션_다단 == TRUE)
			HTML결과 += "\t\t\t<table width=337 height=15 border=0 cellpadding=0 cellspacing=0>\r\n";
		else
			HTML결과 += "<table width=606 height=15 border=0 cellpadding=0 cellspacing=0>\r\n";

		HTML결과 += "\t\t\t<tr>\r\n";
		HTML결과 += "\t\t\t<td width=2 height=20><img src=C:\\BluePill/left_new.gif></td><td width=10></td>\r\n";

		HTML결과 += LINE_TR_핵심(CurLine);

		HTML결과 += "<td width=10></td><td width=6 height=20><img src=C:\\BluePill/right_new.gif></td>\r\n";
		HTML결과 += "</tr>\r\n";
		HTML결과 += "</table><!--<br>-->\r\n";


	return HTML결과;
}

CString CKingOfWord::LINE_TR_핵심(CString CurLine)
{
	CString			HTML결과;	// 두껍기도 하고 휘기도 한 글자들.
	CStringArray	wordArray;

	// 적절하게 띄어쓰기를 맞추어 주는 코드-------------------------------
	_cv.CVT_ParagraphToWord_T_R_Array(wordArray, CurLine);

	if(wordArray.GetAt(wordArray.GetCount() -2 ) != "¶")
	{
		if(_옵션_다단 == TRUE)
			HTML결과 += "<td width=309>";	
		else
			HTML결과 += "<td width=578>";	
	}


		for(int j = 0 ; j < wordArray.GetCount() -1 ; j++)
		{
			if(wordArray.GetAt(wordArray.GetCount() -2 ) == "¶")
			{
				if(j == 0)
					HTML결과 += "<td align=left valign=bottom>";
				else if(j == wordArray.GetCount() -3)
					HTML결과 += "<td align=right valign=bottom>";
				else
					HTML결과 += "<td align=center valign=bottom>";
			}

			HTML결과 += wordArray.GetAt(j); 
			HTML결과 += " ";

			if(wordArray.GetAt(wordArray.GetCount() -2 ) == "¶")
				HTML결과 += "</td>";
		}
	// 적절하게 띄어쓰기를 맞추어 주는 코드-------------------------------

	if(wordArray.GetAt(wordArray.GetCount() -2 ) != "¶")
		HTML결과 += "</td>";

	return HTML결과;
}

CString CKingOfWord::LINE_Q(CString CurLine)
{
	CurLine.Replace("<Q>", ""); 	CurLine.Replace("</Q>", ""); 	CurLine.Trim();

	CString 단어수정_리스트, 일련번호;
	단어수정_리스트 = CurLine;

	// 왼쪽에 있는 단어 수정용 리스트에 올리기 위한 자료임.
	if(단어수정_리스트.Find("1.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("2.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("3.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("4.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("5.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("6.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("7.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("8.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("9.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}
	if(단어수정_리스트.Find("0.") != -1)	{		일련번호 = 단어수정_리스트.Left(단어수정_리스트.Find(".") + 2);		_saWord_Book_Manual_Edit_tmp.Add(일련번호);	}



//	AfxMessageBox(CurLine + "k");
	// 2. 영어 지문인 경우.…………………………………………………………………………………………………
	CString			CandyMan;	// 두껍기도 하고 휘기도 한 글자들.
	CStringArray	wordArray;
	CString			html_source;

	_cv.CVT_LineToWordArray(wordArray, CurLine);
	
		for(int j = 0 ; j < wordArray.GetCount(); j++)
		{
//			AfxMessageBox(wordArray.GetAt(j));
			if(wordArray.GetAt(wordArray.GetCount() -1 ) == "¶")
			{
				if(j == 0)									CandyMan += "\t\t\t<td align=left>";
				else if(j == wordArray.GetCount() - 2)		CandyMan += "<td align=right>";
				else if(j == wordArray.GetCount() - 1)		CandyMan += "";
				else										CandyMan += "<td align=center>";

															CandyMan += "<font face=\"Malgun Gothic\">";
															CandyMan += wordArray.GetAt(j); 
															CandyMan += "</font>";

															CandyMan += "</td>";
			}
			else
			{

															CandyMan += "<font face=\"Malgun Gothic\">" + wordArray.GetAt(j) + "</font> ";
			}
		}


		if(_옵션_다단 == TRUE)
		{
			html_source += "\r\n";
			html_source += "\t\t\t<table width=337 height=15 border=0 cellpadding=0 cellspacing=0>\r\n";
			html_source += "\t\t\t<tr>\r\n";
//			html_source += "\t\t\t<td width=2 height=20><img src=C:\\BluePill/left_new.gif></td><td width=10></td>\r\n";
			if(wordArray.GetAt(wordArray.GetCount() -1 ) != "¶")	html_source += "\t\t\t<td width=327>";

			html_source += CandyMan;

			if(wordArray.GetAt(wordArray.GetCount() -1 ) != "¶")	html_source += "</td>";

			html_source += "<td width=10></td>\r\n";
//			html_source += "\t\t\t<td width=6 height=20><img src=C:\\BluePill/right_new.gif></td>\r\n";
			html_source += "\t\t\t</tr>\r\n";
			html_source += "\t\t\t</table><!--<br>-->\r\n";
		}
		else
		{
			html_source += "<table width=606 height=15 border=0 cellpadding=0 cellspacing=0>\r\n";
			html_source += "<tr>\r\n";
//			html_source += "\t<td width=2 height=20><img src=C:\\BluePill/left_new.gif></td><td width=10></td>\r\n";
			if(wordArray.GetAt(wordArray.GetCount() -1 ) != "¶")	html_source += "<td width=578>";

			html_source += CandyMan;

			if(wordArray.GetAt(wordArray.GetCount() -1 ) != "¶")	html_source += "</td>";

//			html_source += "<td width=10></td><td width=6 height=20><img src=C:\\BluePill/right_new.gif></td>\r\n";
			html_source += "</tr>\r\n";
			html_source += "</table><!--<br>-->\r\n";
		}

	// CString test;
	// test.Format("%s - %d - %s - %d",html_source, GetCurCakeAnswer(), GetCurCakeT(), GetCurCakeDNA());
	// AfxMessageBox(test);

	return html_source;
}


CString CKingOfWord::LINE_MAKE_PAGE(int& i, CString& 현재까지만들어본_컨텐츠, CString& 현재까지만들어본_단어장, CString &현재까지만들어진CAKE단위로딱떨어지는페이지, int &현재까지만들어진CAKE단위로딱떨어지는페이지_그때의줄번호, CString &한줄전_만들어진내용 , CString &한줄전_만들어진_단어장)
{
	CString HTML결과물;

	if((GetLineCnt( 현재까지만들어본_컨텐츠 ) + MakeWordBookPage(현재까지만들어본_단어장) ) > LineLimit)
	{
		if(현재까지만들어진CAKE단위로딱떨어지는페이지 !="")	// 여태까지 </CAKE>태그가 있었기 때문에 문제마다 딱딱 떨어지는, _현재까지만들어진CAKE단위로딱떨어지는페이지가 생겼다는 뜻이다.
		{
			if(_옵션_다단)
			{
				if(pageNum % 2 == 0) 
				{
					HTML결과물 += "<!-- CAKE가 깔끔한 새로운 페이지 시작 -->\r\n";
					HTML결과물 += "<table>\r\n";
					HTML결과물 += "\t<tr>\r\n";
					HTML결과물 += "\t<td valign=top width=337>\r\n";

					// 보기의 중간에 페이지가 잘리는 버그를 해결해야 할 부분
					if(_상태_A태그에서페이지나뉨){		HTML결과물 += _cur_A_Table_BGN;
											_상태_A태그에서페이지나뉨 = false;	}

					HTML결과물 += 현재까지만들어진CAKE단위로딱떨어지는페이지;

					HTML결과물 += "\t</td>\r\n";
				}
				else
				{
					HTML결과물 += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
					HTML결과물 += "\t<td valign=top width=337>\r\n";						

					// 보기의 중간에 페이지가 잘리는 버그를 해결해야 할 부분
					if(_상태_A태그에서페이지나뉨){		HTML결과물 += _cur_A_Table_BGN;
											_상태_A태그에서페이지나뉨 = false;	}

					HTML결과물 += 현재까지만들어진CAKE단위로딱떨어지는페이지;

					HTML결과물 += "\t</td>\r\n";
					HTML결과물 += "\t</tr>\r\n";
					HTML결과물 += "</table>\r\n";
				}
			}
			else
			{
				// 보기의 중간에 페이지가 잘리는 버그를 해결해야 할 부분
				if(_상태_A태그에서페이지나뉨){		HTML결과물 += _cur_A_Table_BGN;
										_상태_A태그에서페이지나뉨 = false;	}
				HTML결과물 += 현재까지만들어진CAKE단위로딱떨어지는페이지;
			}


			pageNum++;

			i = 현재까지만들어진CAKE단위로딱떨어지는페이지_그때의줄번호;
			_상태_지문 = false;
			_상태_해설 = false;
//			_상태_지문 = _페이지넘어가서되돌릴경우의예전_상태_지문;
//			_상태_해설 = _페이지넘어가서되돌릴경우의예전_상태_해설;
//			_페이지넘어가서되돌릴경우의예전_상태_지문 = false;
//			_페이지넘어가서되돌릴경우의예전_상태_해설 = false;

			현재까지만들어진CAKE단위로딱떨어지는페이지 = "";
			현재까지만들어진CAKE단위로딱떨어지는페이지_그때의줄번호 = 0;

			
			_saWord_Book_Manual_Edit_tmp.RemoveAll(); // 왼쪽에 있는 단어장을 처리하기 위해 만들어진 부분이다.

		}
		else	// </CAKE>태그가 없는 경우, : _현재까지만들어진CAKE단위로딱떨어지는페이지가 비어있다는 뜻으로, 대개 처리하기 시작한 CAKE페이지가 아직 끝나지 않고 CAKE의 중간에 머물렀을 때 들어오는 부분이다.
		{
			//AfxMessageBox(_현재까지만들어본_컨텐츠.Mid(_한줄전_만들어진내용.GetLength()));
			// 처리될뻔하다가 만 부분 중에서 <A> 태그가 페이지의 나뉨으로 인하여 특수한 처리를 해주어야 하는 부분이 있는 경우,
			// _상태_A태그이 true로 되었다가, 이전 줄을 처리하게 됨으로써 옵션이 바뀐다. 그 옵션을 재설정한다.
			if(현재까지만들어본_컨텐츠.Mid(한줄전_만들어진내용.GetLength()).Find(_cur_A_Table_BGN) != -1)
				_상태_A태그 = false;

			한줄전_만들어진내용 = PostProcess(한줄전_만들어진내용);

			if(_옵션_다단)
			{
				if(pageNum % 2 == 0) 
				{
					HTML결과물 += "<!-- CAKE가 도중에 잘리는 경우의 새로운 페이지 시작 -->\r\n";
					HTML결과물 += "<table>\r\n";
					HTML결과물 += "\t<tr>\r\n";
					HTML결과물 += "\t<td valign=top width=337>\r\n";

					// 보기의 중간에 페이지가 잘리는 버그를 해결해야 할 부분
					if(_상태_A태그에서페이지나뉨){		HTML결과물 += _cur_A_Table_BGN;
											_상태_A태그에서페이지나뉨 = false;	}

					HTML결과물 += 한줄전_만들어진내용;
					// CAKE가 <A> 태그 사이에서 잘리는 지를 확인하여 관련 코드를 추가함
					if(_상태_A태그){	HTML결과물 += "\t\t</table>\r\n\t\t<!-- CAKE가 A태그 사이에서 잘립니다.-->\r\n";_상태_A태그에서페이지나뉨 = true;}
			
					HTML결과물 += 한줄전_만들어진_단어장;
//					HTML결과물 += MakeBR(GetLineCnt(_한줄전_만들어진내용) + GetLineCnt(_한줄전_만들어진_단어장));//---- 모자란 줄을 채워넣는다. -------//

					HTML결과물 += "\t</td>\r\n";
				}
				else
				{
					HTML결과물 += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
					HTML결과물 += "\t<td valign=top width=337>\r\n";						

					// 보기의 중간에 페이지가 잘리는 버그를 해결해야 할 부분
					if(_상태_A태그에서페이지나뉨){		HTML결과물 += _cur_A_Table_BGN;
											_상태_A태그에서페이지나뉨 = false;	}

					HTML결과물 += 한줄전_만들어진내용;
					// CAKE가 <A> 태그 사이에서 잘리는 지를 확인하여 관련 코드를 추가함
					if(_상태_A태그){	HTML결과물 += "\t\t</table>\r\n\t\t<!-- CAKE가 A태그 사이에서 잘립니다.-->\r\n";_상태_A태그에서페이지나뉨 = true;}

					HTML결과물 += 한줄전_만들어진_단어장;
//					HTML결과물 += MakeBR(GetLineCnt(_한줄전_만들어진내용) + GetLineCnt(_한줄전_만들어진_단어장));//---- 모자란 줄을 채워넣는다. -------//

					HTML결과물 += "\t</td>\r\n";
					HTML결과물 += "\t</tr>\r\n";
					HTML결과물 += "</table>\r\n";
				}
			}
			else
			{
				HTML결과물 += 한줄전_만들어진내용;
				// CAKE가 <A> 태그 사이에서 잘리는 지를 확인하여 관련 코드를 추가함
					if(_상태_A태그){	HTML결과물 += "\t\t</table>\r\n\t\t<!-- CAKE가 A태그 사이에서 잘립니다.-->\r\n";_상태_A태그에서페이지나뉨 = true;}

				HTML결과물 += 한줄전_만들어진_단어장;
//				HTML결과물 += MakeBR(GetLineCnt(_한줄전_만들어진내용) + GetLineCnt(_한줄전_만들어진_단어장));//---- 모자란 줄을 채워넣는다. -------//
			}



			pageNum++;

			i--;	// 한줄 전부터 다시 처리한다.
			_상태_지문 = _한줄전_상태_지문;


			// 왼쪽에 나오는 단어장 고치는 파일을 만들기 위한 코드이다.
			// CAKE 태그가 있을 때만 의미가 있고 이 경우에는 별로 의미가 없지만,
			// 통일성을 위해서 똑같이 해준다.
			// </CAKE>를 참조할 것.

			for(int nManualEditTmp = 0 ; nManualEditTmp < _saWord_Book_Manual_Edit_tmp.GetSize() ; nManualEditTmp++)
				_saWord_Book_Manual_Edit.Add(_saWord_Book_Manual_Edit_tmp.GetAt(nManualEditTmp));

			_saWord_Book_Manual_Edit_tmp.RemoveAll();

		}


		// 초기화 한다.
		_fd._ld._1순위단어장.RemoveAll();
		_fd._ld._2순위단어장.RemoveAll();
		_fd._ld.m_saCurrentWordBook.RemoveAll();
		
		한줄전_만들어진내용 = "";
		한줄전_만들어진_단어장 = "";
		현재까지만들어본_컨텐츠 = "";

		//---------------------------------------------------
	}

	return HTML결과물;
}

CString CKingOfWord::LINE_MAKE_LAST_PAGE(CString& 현재까지만들어본_컨텐츠, CString& 현재까지만들어본_단어장)
{
	CString HTML결과물;
	// 막장

	현재까지만들어본_컨텐츠 = PostProcess(현재까지만들어본_컨텐츠);			//OutputDebugString(_현재까지만들어본_컨텐츠);
	MakeWordBookPage(현재까지만들어본_단어장);


	if(_옵션_다단)
	{
		if(pageNum % 2 == 0) 
		{
			HTML결과물 += "<table>\r\n";
			HTML결과물 += "\t<tr>\r\n";
			HTML결과물 += "\t<td valign=top width=337>\r\n";
		}
		else
		{
			HTML결과물 += "<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
			HTML결과물 += "<td valign=top width=337>\r\n";						
		}
	}

	if(_상태_A태그에서페이지나뉨)		HTML결과물 += _cur_A_Table_BGN;
	HTML결과물 += 현재까지만들어본_컨텐츠;
	HTML결과물 += 현재까지만들어본_단어장;

	if(_옵션_다단)
	{
		if(pageNum % 2 == 0) 
		{
			HTML결과물 += "\t<td width=7><img src=C:\\BluePill/center_bar.gif></td>\r\n";
			HTML결과물 += "\t<td valign=top width=337>\r\n";						
			HTML결과물 += "\t</td>\r\n";
			HTML결과물 += "\t</tr>\r\n";
			HTML결과물 += "</table>\r\n";
		}
		else
		{
			HTML결과물 += "\t</td>\r\n";
			HTML결과물 += "\t</tr>\r\n";
			HTML결과물 += "</table>\r\n";
		}
	}





	_fd._ld._1순위단어장.RemoveAll();
	_fd._ld._2순위단어장.RemoveAll();
	_fd._ld.m_saCurrentWordBook.RemoveAll();



	return HTML결과물;


}


void CKingOfWord::LINE_SCORE()
{
	CString sScore;

	int nScoreGoogle = 0;
	int nScoreEmpas = 0;
	int nDifficultPercentage = 0;

	if((_점수_총합_Words != 0) && (_점수_총합_DifficultWords != 0))
	{
		nScoreGoogle = _점수_총합_Google / _점수_총합_Words;
		nScoreEmpas	= _점수_총합_Empas / _점수_총합_Words;
		nDifficultPercentage = (_점수_총합_DifficultWords * 100) / _점수_총합_Words;

	}
	else
	{
		nScoreGoogle = 0;
		nScoreEmpas = 0;
	}


	sScore.Format("Google : %d, Empas : %d - 어려운단어 비율 = %d", nScoreGoogle, nScoreEmpas, nDifficultPercentage);
}

void CKingOfWord::LINE_S()
{
}

CString CKingOfWord::Get_Cur_ProgramName()
{
	char a[1024], b[1024], c[1024];

	GetModuleFileName( NULL, a, 1024);
	_splitpath(a, b, c, NULL, NULL);

	CString fullPath, ProgramName;
	fullPath = a;
	ProgramName = fullPath.Mid(fullPath.ReverseFind('\\') + 1);

	return ProgramName;
}


void CKingOfWord::Get_StudentList(CListBox& studentList)
{
	CString p;
	p.Format("%sstudents\\*.txt", _ut.Dir());

	CFileFind fileFind;
    BOOL bWorking=fileFind.FindFile(p);

    while(bWorking)
    {       
        bWorking=fileFind.FindNextFile();       
		studentList.AddString(fileFind.GetFileName());
    }
}


int Difficulty(int input)
{
	//return 100 - (100 / input);
	int nDifficulty = 0;
	nDifficulty = 100 - (int)sqrt(double(10000 - (input-100 ) * (input-100)));

	return nDifficulty;
}

// 난이도에 따른 가중치를 부여한다.
// 1. 난이도 '하'는 가중치를 거의 부여하지 않고, 
// 2. 난이도 '중'은 급격히 가중치를 올리고,
// 3. 난이도 '상'은 가중치를 적당한 정도로 일정하게 만든다.
int Weight(int input)
{
	if(input < 0 || input > 100)
		return -1;

	int Weight_table[100] = {	 0,  0,  0,  0,  0,	// 0 ~ 4
								 0,  0,  1,  1,  1,	// 5 ~ 9
								 1,  2,  2,  3,  3,	// 10 ~ 14
								 4,  5,  6,  8, 10,	// 15 ~ 19
								12, 13, 14, 15, 15, // 20 ~ 24
								15, 16, 16, 16, 17, // 25 ~ 29
								17, 17, 18, 18, 18, // 30 ~ 34
								19, 19, 19, 19, 19, // 35 ~ 39
								19, 19, 19, 19, 19, // 40 ~ 44
								19, 19, 19, 19, 19, // 45 ~ 49
								19, 19, 19, 19, 19, // 50 ~ 54
								19, 19, 19, 19, 19, // 55 ~ 59
								19, 19, 19, 19, 19, // 60 ~ 64
								19, 19, 19, 19, 19, // 65 ~ 69
								19, 19, 20, 20, 20, // 70 ~ 74
								20, 20, 20, 20, 20, // 75 ~ 79
								20, 20, 20, 20, 20, // 80 ~ 84
								20, 20, 20, 20, 20, // 85 ~ 89
								20, 20, 20, 20, 20, // 90 ~ 94
								20, 20, 20, 20, 20, // 95 ~ 99
	};

	return Weight_table[input - 1];
}



bool CKingOfWord::WORD_CheckTags(CString src)
{
	if(src == "<SEN>")		return true;
	if(src == "</SEN>")		return true;
	if(src == "<Q>")		return true;
	if(src == "</Q>")		return true;

	if(src == "<B>")		return true;
	if(src == "</B>")		return true;
	if(src == "<A>")		return true;
	if(src == "<A0>")		return true;
	if(src == "<A1>")		return true;
	if(src == "<A2>")		return true;
	if(src == "<A3>")		return true;
	if(src == "<A4>")		return true;
	if(src == "<A5>")		return true;

	if(src == "</A0>")		return true;
	if(src == "</A1>")		return true;
	if(src == "</A2>")		return true;
	if(src == "</A3>")		return true;
	if(src == "</A4>")		return true;
	if(src == "</A5>")		return true;


	if(src == "……")		return true;
	if(src == "</A>")		return true;


	return false;
}

bool CKingOfWord::WORD_CheckCaps(CString src)
{

	CString srcF = src;										srcF.MakeLower();

	CString DeleteList[34] = {
		"?",	".",	",",	"\"",	"!",	"[",	"]",	"(",	")",	">",
		"<",	"_",	"1",	"2",	"3",	"4",	"5",	"6",	"7",	"8",
		"9",	"0",	"\'",	":",	";",	"*",	"`",	"/",	"%",	"$",
		"’",	"~",	"|",	"^"};

	for(int i=0 ; i < 34 ; i++)		srcF.Replace(DeleteList[i], "");

	CString sCheckCapital = src;	sCheckCapital.MakeLower();

	//--------------------------------------------------------------------------------------
	// 대문자인지 아닌지 확인하여 고유명사인지를 알게하는 부분임.
	if(sCheckCapital != src && _nWordPosition != 0)
	{

		// 현재의 문장은 고유명사로 끝나더라도 다음 문장이 시작된다는 것은 이야기 해주어야 함
		if(		sCheckCapital.Find(".") != -1) _nWordPosition = 0; // 이 다음 단어는 문장의 처음이라는 뜻임
		else if(sCheckCapital.Find("!") != -1) _nWordPosition = 0; // 이 다음 단어는 문장의 처음이라는 뜻임
		else if(sCheckCapital.Find("?") != -1) _nWordPosition = 0; // 이 다음 단어는 문장의 처음이라는 뜻임

		return true; // 문장의 첫머리가 아니며, 대문자가 있으면 고유명사로 생각한다.
	}
	else
	{
		// 현재의 단어가 영단어가 아니었으면, 그냥 한 박자를 쉬어야 한다.
		if(_fd.Is2ByteWord(srcF) == true)
		{;} // 2바이트 문자가 문장의 맨 앞에 있으면 문장의 첫머리가 아니라고 보고,
			// 이전의 옵션을 그대로 따른다.
		else if(srcF != "")
			_nWordPosition = 1;
		else // 이전의 옵션을 그대로 따른다.
		{;}
	}
	
	if(sCheckCapital.Find(".") != -1) _nWordPosition = 0; // 이 다음 단어는 문장의 처음이라는 뜻임
	else if(sCheckCapital.Find("!") != -1) _nWordPosition = 0; // 이 다음 단어는 문장의 처음이라는 뜻임
	else if(sCheckCapital.Find("?") != -1) _nWordPosition = 0; // 이 다음 단어는 문장의 처음이라는 뜻임

	return false;
}

bool CKingOfWord::WORD_CheckDbls(CString src)
{
	src.MakeLower();

	if(src.Find("-") != -1 && src != "-")
	{
		// 만약 사전에 하이픈으로 만들어진 단어가 그대로 있으면, 이를 따로 나누어서 굳이 설명할 필요는 없다.
		if(_fd.FindMeaning(src) != "")
			return false;

		return true;
	}
	else
		return false;
}

bool CKingOfWord::WORD_CheckMath(CString src)
{
	CString srcF = src;										srcF.MakeLower();

	CString DeleteList[34] = {
		"?",	".",	",",	"\"",	"!",	"[",	"]",	"(",	")",	">",
		"<",	"_",	"1",	"2",	"3",	"4",	"5",	"6",	"7",	"8",
		"9",	"0",	"\'",	":",	";",	"*",	"`",	"/",	"%",	"$",
		"’",	"~",	"|",	"^"};

	for(int i=0 ; i < 34 ; i++)		srcF.Replace(DeleteList[i], "");

	// 처리할 필요 없는 일반 글자. - 수학 기호.
	if(srcF == "+")				return true;
	if(srcF.Find("+") != -1)	return true;
	if(srcF == "-")				return true;
	if(srcF == "=")				return true;

	return false;
}

bool CKingOfWord::WORD_Check1Ltr(CString src)
{
	CString srcF = src;										srcF.MakeLower();

	CString DeleteList[34] = {
		"?",	".",	",",	"\"",	"!",	"[",	"]",	"(",	")",	">",
		"<",	"_",	"1",	"2",	"3",	"4",	"5",	"6",	"7",	"8",
		"9",	"0",	"\'",	":",	";",	"*",	"`",	"/",	"%",	"$",
		"’",	"~",	"|",	"^"};

	for(int i=0 ; i < 34 ; i++)		srcF.Replace(DeleteList[i], "");


	// 한글자이면 처리할 필요 없다.
	if(srcF.GetLength() == 1)	return true;
	else						return false;
}

CString CKingOfWord::WORD_Tag(CString src, bool bFixedWidth)
{
	//0. 태그인 경우
	// 0.1 무시해야 할 태그들.
	// ……………………………………………………………………………
	if(src == "<SEN>")		{_nWordPosition = 0; return "";}
	if(src == "</SEN>")		{_nWordPosition = 0; return "";}

	// 0.2 무시할 수 없는 태그들.
	// ……………………………………………………………………………

	if(src == "<B>")		{_nWordPosition = 0; return "\t\t<font face=\"Malgun Gothic\">";}
	if(src == "<A>")		{  _상태_개행추가 = false; return "";}
	//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
	// bFixedWidth : true이면 문항을 정해진 길이로 만들고, false이면 문항을 정해지지 않은 길이로 만든다.
	// 문항에는 크게 세가지 종류가 있다.

	// 1) "① 아주 문항이 길어서 한 칸이
	//        넘어가는 문항"				- 문항을 <table>을 이용해서 폭을 정하지 않고, 만들면,
	//										  "한 칸이" 부분에 <br>이 매겨질 경우, 프린트 화면에서는 두 칸이 띄어지는
	//										  버그가 있다. 따라서 문항이 아주 긴 경우에는 폭을 정하고 만들어야 한다.

	// 2) "① As a result …… For example"	- 이 경우에는 테이블의 열을 여러개로 해주어야 하는데, 웹태그의 특성상
	//										  테이블의 열이 여러개일 때 폭을 정해주면 다음과 같이 보인다.
	//										
	//										2.1) 원래의 의도
	//											① 문항1 …… 문항2 …… 문항3
	//
	//										2.2) 실제 화면
	//											① 문항1			……			문항2			……			문항3
	//
	//										따라서 이런 경우에는 폭을 정해 주어서는 안된다.
	//										문항 내에 "……" 와 같은 문자열이 있거나, <A0>태그가 있으면
	//										폭을 정해서는 안되는 문항임을 알 수 있다.

	// 3) 일반적인 문항						- 일반적인 문항에는 폭을 정해주어도 버그가 생기지 않는다.
	//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
	if(src == "<A0>")		{	_상태_A태그				= true;
								_nWordPosition		= 0; _상태_A0사용한_보기 = true;	
								_cur_A_Table_BGN	= "<table cellspacing=0 cellpadding=0 border=0>\r\n\t\t\t<tr><td valign=top width=30>　　</td><td align=center>"; return _cur_A_Table_BGN;}

	if(src == "<A1>")		
	{
		if(_상태_A0사용한_보기 == true) // A0 태그가 이전에 사용되었었다.
		{
			_nWordPosition = 0; 	return "\r\n\t\t\t<!--A0태그 사용되었음-->\r\n\t\t\t<tr><td valign=top width=30>　①</td><td>";
		}
		else if(bFixedWidth == false)
		{
			_nWordPosition			= 0;
			_상태_A태그					= true;
			_cur_A_Table_BGN		= "<table cellspacing=0 cellpadding=0 border=0>\r\n";
			return "<table cellspacing=0 cellpadding=0 border=0><tr><td valign=top width=30>　①</td><td>";
		}
		else // A0 태그가 없다는 것은, 간격을 맞출 필요가 없다는 것이다.
		{
			if(_옵션_다단 == TRUE)
			{
				_nWordPosition		= 0;
				_상태_A태그				= true;
				_cur_A_Table_BGN	= "\t\t\t<table cellspacing=0 cellpadding=0 border=0 width=331>\r\n";
				return "\r\n\t\t\t<table cellspacing=0 cellpadding=0 border=0 width=331>\r\n\t\t\t<tr><td valign=top width=30>　①</td><td>";
			}
			else
			{
				_상태_A태그				= true;
				_cur_A_Table_BGN	= "\t\t\t<table cellspacing=0 cellpadding=0 border=0 width=600>\r\n";
				_nWordPosition		= 0; return "\t\t\t<table cellspacing=0 cellpadding=0 border=0 width=600>\r\n\t\t\t<tr><td valign=top width=30>　①</td><td>";
			}
		}
	}

	if(src == "<A2>")		{_nWordPosition = 0; return "\t\t<tr><td valign=top width=30>　②</td><td>";}
	if(src == "<A3>")		{_nWordPosition = 0; return "\t\t<tr><td valign=top width=30>　③</td><td>";}
	if(src == "<A4>")		{_nWordPosition = 0; return "\t\t<tr><td valign=top width=30>　④</td><td>";}
	if(src == "<A5>")		{_nWordPosition = 0; return "\t\t<tr><td valign=top width=30>　⑤</td><td>";}

	if(src == "</A0>")		{_nWordPosition = 0; _상태_보기 = true; return "</td></tr>";}
	if(src == "</A1>")		{_nWordPosition = 0; _상태_보기 = true; return "</td></tr>";}
	if(src == "</A2>")		{_nWordPosition = 0; _상태_보기 = true; return "</td></tr>";}
	if(src == "</A3>")		{_nWordPosition = 0; _상태_보기 = true; return "</td></tr>";}
	if(src == "</A4>")		{_nWordPosition = 0; _상태_보기 = true; return "</td></tr>";}
	if(src == "</A5>")		{_nWordPosition = 0; _상태_보기 = true; return "</td></tr>";}


	if(src == "……")		{_nWordPosition = 0; return "　</td><td> </td><td align=center>　";	}

	if(src == "</A>")		{  _상태_개행추가 = false; _상태_A태그 = false;	 _nWordPosition = 0; _상태_A0사용한_보기 = false; return "\t\t</table><!-- 보기가 정상적으로 끝남 -->\r\n";}
	//_상태_A0사용한_보기 = false; 이 코드가 A1자리에 있으면 backup페이지 되돌리기 때문에 옵션이 그냥 꺼질 수 있다. 적당히 뒤로 빼 놓았으니 위치를 바꾸지 말 것

	CString AnswerTags[2] = {"</Q>", "</B>"};
	for(int i=0; i < 2 ; i++)
	{
		if(src == AnswerTags[i])
		{					_nWordPosition = 0;  return "</font>";}
	}


	return "";
}


// 좌측 학생 이름 리스트를 선택할 때 불린다.
// 추출된 학생이름은, 교재에 이름을 적을 때 쓰인다.
bool CKingOfWord::SetStudentName(CString studentName)
{
	m_studentName = studentName;
	return true;
}

CString CKingOfWord::GetStudentName(void)
{
	return m_studentName;
}

bool CKingOfWord::MakeUserWordBook(CString StudentName)
{

	return true;
	
}

bool CKingOfWord::SetFileName(CString fileName)
{
	m_fileName = fileName;
	return true;
}

CString CKingOfWord::GetFileName(void)
{
	return m_fileName;
}


void CKingOfWord::SetPathFolder(CString pathFolder)
{
	m_PathFolder = pathFolder;
}

CString CKingOfWord::GetPathFolder(void)
{
	return m_PathFolder;
}


// WordStat에 단어에 관한 정보가 있을 경우, 알려진 변이형의 목록을 찾아온다.
// 없을 경우 false를 리턴한다.
bool CKingOfWord::ParseWordStatVariation(CString word, CStringArray& saWordBookVariation)
{
	CString		curLine;
	CString		tmp;
	ifstream	fpIN;

	char		c[1024];
	bool		bFound;

	//----------------------------------------------------------------------------
	CString		sPath;
	CString		sFolderPath;
	CString		sUpOneLevelPath;

	sFolderPath		= _ut.Dir();
	sUpOneLevelPath = sFolderPath.Left(sFolderPath.ReverseFind('\\'));
	sUpOneLevelPath = sUpOneLevelPath.Left(sUpOneLevelPath.ReverseFind('\\'));

	sPath.Format("%s/common/WordStat.txt", sUpOneLevelPath);
	//----------------------------------------------------------------------------
	
	// WordStat에서 알려진 변이형의 목록을 찾아온다.
	bFound = false;

	fpIN.open(sPath, ios::in|ios::out);

	while(fpIN.getline(c,999,'\n'))
	{
		curLine = c;
		tmp.Format(")%s(", word);

		if(curLine.Find(tmp) != -1)
		{
			bFound = true;
			break;
		}
	}
	fpIN.close();


	if(bFound == false)
		return false;

	saWordBookVariation.RemoveAll();

	//Ex. 0005)fruit(4)fruits(1)

	curLine.Replace("0", "|");curLine.Replace("1", "|");curLine.Replace("2", "|");
	curLine.Replace("3", "|");curLine.Replace("4", "|");curLine.Replace("5", "|");
	curLine.Replace("6", "|");curLine.Replace("7", "|");curLine.Replace("8", "|");
	curLine.Replace("9", "|");curLine.Replace("(", "|");curLine.Replace(")", "|");

	//Ex. |||||fruit|||fruits|||

	while(curLine.Find("||") != -1)
	{
		curLine.Replace("||", "|");
	}

	//Ex. |fruit|fruits|

	CString curWord;

	curLine = curLine.Mid(1);
		
	//Ex. fruit|fruits|
	

	while(curLine.Find("|") != -1)
	{
		curWord = curLine.Left(curLine.Find("|")); 
		curLine = curLine.Mid(curLine.Find("|") + 1);
		
		saWordBookVariation.Add(curWord);
	}


	return true;
}


CString CKingOfWord::MakeBR(int cntLine)
{
	int LineLimit;
	CString ret;

	if(_옵션_모니터에서보기 == TRUE)	LineLimit = 24;
	else									LineLimit = 48;
	
	ret += "\r\n\t\t\t<!-- for printing -->\r\n\t\t\t";
	for(int i = 0 ; i < (LineLimit - cntLine) ; i++)	ret += "<br>";
	ret += "\r\n\t\t\t<!-- for printing -->\r\n";

	return ret;
}

int CKingOfWord::GetImageHeightBR(CString path, int& rate)
{
	CString fullPath;
	fullPath.Format("%s%s", GetPathFolder(), path);

	fullPath.Replace("/", "\\");

	// GDI+를 위한 세팅을 해주어야 한다.
	USES_CONVERSION;
 	Bitmap bitmap(A2W(fullPath));

	// Max값이 넘어가지 않을 경우
	if(bitmap.GetWidth() < 310)
	{
		rate = 100;

		int n = bitmap.GetHeight() / 20;

///		CString t;
//		t.Format("%d -  %d", bitmap.GetWidth(), bitmap.GetHeight());
//		AfxMessageBox(t);

		// 47이 넘어가면 페이지 만들지 않는다. 따라서,
		// 이 코드를 생략하면 무한 루프에 빠지는 경우가 있다.

		if(n > LineLimit - 1)
			n = LineLimit - 1;

		return n ;
	}
	else
	{
		int n = (bitmap.GetHeight() * 300) / (bitmap.GetWidth() * 20);
		rate = 30000 / bitmap.GetWidth();

		return n;
	}

}

CString CKingOfWord::PostProcess(CString str)
{
	CString ret;
	str.Replace(" ［ ", " [");
	str.Replace("［ ", " [");
	str.Replace(" ［", " [");
	str.Replace("［", " [");

	str.Replace(" ］ ", "] ");
	str.Replace("］ ", "] ");
	str.Replace(" ］", "] ");
	str.Replace("］", "] ");

	str.Replace(" ） ", ")");
	str.Replace("） ", ")");
	str.Replace(" ）", ")");
	str.Replace("）", ")");

	str.Replace(" （ ", "(");
	str.Replace("（ ", "(");
	str.Replace(" （", "(");
	str.Replace("（", "(");


	str.Replace("%", "％");
	str.Replace("¶","");

	ret = str;
	return ret;
}

void CKingOfWord::LINE_XML()
{
	;
}

void CKingOfWord::LINE_ALL_END()
{
	;
}

void CKingOfWord::LINE_ALL_BGN()
{
	;
}



CString CKingOfWord::CORE_WORD_LV01(CString src)
{
	CString ret;
	ret.Format("%s ",src);
	return ret;	
}

CString CKingOfWord::CORE_WORD_LV02(CString src)
{
	CString ret;
	ret.Format("%s ",src);
	return ret;	
}

CString CKingOfWord::CORE_WORD_LV03(CString src)
{
	CString ret;
	ret.Format("<font color=#888888><span style=\"border-bottom:1px dashed;\"><i>%s</i></span></font> ",src);
	return ret;
}

CString CKingOfWord::CORE_WORD_LV04(CString src, CString W, CString basicForm)
{
	CString ret,s;

	// 1. 종이 본문
	ret.Format("<span style=\"border-bottom:1px dashed;\"><i>%s</i></span> ",src);

	if(W != "<중복>")
	{
		// 2. 종이 단어장
		if(_fd._ld._2순위단어장.GetCount() == 0) s.Format("&nbsp;&#9787&nbsp;%s", W);
		else							s.Format("　&nbsp;%s", W);
		_fd._ld._2순위단어장.Add(s);

		// 3. EngMaster 좌측 단어장
		CString sWordPrintList = _fd.Find_LineNum_Meaning(basicForm);
		_saWord_Book_Manual_Edit_tmp.Add(sWordPrintList);

	}

	return ret;
}

CString CKingOfWord::CORE_WORD_LV05(CString src, CString W, CString basicForm)
{
	CString ret, s;

	// 1. 종이 본문
	ret.Format("<i><b>%s</b></i> ",src);

	if(W != "<중복>")
	{
		// 2. 종이 단어장
		if(_fd._ld._2순위단어장.GetCount() == 0)	s.Format("&nbsp;&#9787&nbsp;%s", W);
		else							s.Format("　&nbsp;%s", W);
		_fd._ld._2순위단어장.Add(s);

		// 3. EngMaster 좌측 단어장
		CString sWordPrintList = _fd.Find_LineNum_Meaning(basicForm);
		_saWord_Book_Manual_Edit_tmp.Add(sWordPrintList);
	}

	return ret;
}

// 머리가 나쁜 경우에는 출현빈도가 3이상인 것 중 모른다고 한 것을 중요 표시한다.
// 머리가 좋은 경우에는 모른다고 한 것은 꼭 중요 표시한다.
CString CKingOfWord::CORE_WORD_LV06(CString src, CString W, CString basicForm)
{
	CString ret, s;
	// 1. 종이 본문
	ret.Format("<b>%s</b> ", src);

	if(W != "<중복>")
	{
		// 2. 종이 단어장
		if(_fd._ld._1순위단어장.GetCount() == 0)	s.Format("&nbsp;&#9786&nbsp;%s", W);
		else							s.Format("　&nbsp;%s", W);
		_fd._ld._1순위단어장.Add(s);

		// 3. EngMaster 좌측 단어장			
		CString sWordPrintList = _fd.Find_LineNum_Meaning(basicForm);
		_saWord_Book_Manual_Edit_tmp.Add(sWordPrintList);
	}

	return ret;
}

CString CKingOfWord::CORE_WORD_LV07(CString src)
{
	CString ret;
	ret.Format("%s ",src);
	return ret;	
}
*/