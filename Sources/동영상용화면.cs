using System;
using System.Collections.Generic;
using System.Linq;

using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using 변환;
using System.Drawing.Drawing2D;

namespace 편집기의_제왕
{
    class 동영상용화면
    {
		#region 멤버변수들
	
        public static string		_그림폴더;

		public static int           _가로픽셀;
        public static int           _세로픽셀;

        public static Pen			_기본펜;
        public static Pen			_보기펜;
        public static Pen			_밑줄펜;
        public static Pen			_취소펜;

        public static Font			_Q글꼴;

        public static Font			_기본글꼴;
		public static Font			_굵은글꼴;

		public static Font			_보기글꼴;
        public static Font			_정답글꼴;

        public static Font			_격자글꼴;
        public static Font			_문법표지글꼴;
        public static Font			_어휘문제글꼴;
        public static Font			_어법문제글꼴;

        public static SolidBrush	_Q붓;
        public static SolidBrush	_보기붓;

        public static SolidBrush	_기본붓;
		public static SolidBrush	_격자붓;
        public static SolidBrush	_정답붓;
		public static SolidBrush	_빨간붓;
		public static SolidBrush	_검은붓;
		public static SolidBrush	_푸른붓;
		public static SolidBrush	_하얀붓;
		public static SolidBrush	_회색붓;


		public static SolidBrush	_문법표지붓;
        public static SolidBrush	_어휘문제붓;
        public static SolidBrush	_어휘문제배경;

        public static SolidBrush	_어법문제붓;
        public static SolidBrush	_어법문제배경;

        public static SolidBrush	_주제배경;


        public static PointF		_기본좌표;

        public static List<한줄>	_Q_여러줄;


        public static List<한줄> _A0_여러줄;

        public static List<string> _어휘문제들;
        public static List<float> _x어휘문제좌표;
        public static List<float> _y어휘문제좌표;

        public static List<string> _어법문제들;
        public static List<float> _x어법문제좌표;
        public static List<float> _y어법문제좌표;

		public static List<string> _모르는단어리스트;
		public static List<string> _모르는단어리스트_3번이하로_나온단어들;
		public static List<string> _사용자단어파일에없는단어들;

		public static List<문법표지> _문법표지들 = null;
        public static int				_문법표지수;
        public static string _선택된배경파일이름;

        public static int _좌여백 = 316;
        public static int _우한계 = 916;
        public static int _좌한계 = 365;
        public static int _우문법기호한계 = 906;
		public static int _높이 = 0;


		private static Image _배경그림파일원본 = null;
		#endregion

		static 동영상용화면()
        {
			_그림폴더 = Application.StartupPath + "/img/";

			_모르는단어리스트 = new List<string>();
			_모르는단어리스트_3번이하로_나온단어들 = new List<string>();
			_사용자단어파일에없는단어들 = new List<string>();
		}

        public static string 만들기(ref List<string> 사용자파일문자열들, ref List<string> 모르는단어리스트, ref List<string> 모르는단어리스트_3번이하로_나온단어들, string 제목, string 질문, string 본문, string ABC, string 보기1Text, string 보기2Text, string 보기3Text, string 보기4Text, string 보기5Text,
									bool 보기1, bool 보기2, bool 보기3, bool 보기4, bool 보기5,
									string 주관식정답, string 해석, string 해설, string 중요어휘, 
                                    ref List<한줄> _본문_여러줄, 
                                    ref List<한줄> _A1_여러줄,
                                    ref List<한줄> _A2_여러줄,
                                    ref List<한줄> _A3_여러줄,
                                    ref List<한줄> _A4_여러줄,
                                    ref List<한줄> _A5_여러줄,
                                    int 너비, int 높이, ref Bitmap _배경과본문_비트맵, ref Graphics _배경과본문_그래픽)
        {
			#region 초기화
			#region 모르는단어리스트 추가부분
			_모르는단어리스트.Clear();
			_모르는단어리스트_3번이하로_나온단어들.Clear();
			_사용자단어파일에없는단어들.Clear();

			List<string> 어절들 = new List<string>();

			변환.문자열.어절들로(본문, ref 어절들);

			string 현재어절, 현재사용자단어파일문자열들;

			for (int i = 0; i < 어절들.Count; i++)
			{
                현재어절 = 어절들[i].불필요제거().ToLower();//.Replace("'ll", "").Replace("'s", "").Replace("'d", "");


                if (!현재어절.StartsWith("'ll")) 현재어절 = 현재어절.Replace("'ll", "");
                if (!현재어절.StartsWith("'s")) 현재어절 = 현재어절.Replace("'s", "");
                if (!현재어절.StartsWith("'d")) 현재어절 = 현재어절.Replace("'d", "");


                while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
				while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

				bool 발견여부 = false;

				if (현재어절.Length == 1) 발견여부 = true;
				if (현재어절.한글포함했는지확인()) 발견여부 = true;


				// 사용자 파일 문자열에서 현재 어절을 발견할 수 없으면, 사전에는 없는 단어가 된다.
				for (int j = 0; j < 사용자파일문자열들.Count && 발견여부 == false; j++)
				{
					현재사용자단어파일문자열들 = 사용자파일문자열들[j];

					if(현재사용자단어파일문자열들.Contains(")" + 현재어절 + "(") || 현재사용자단어파일문자열들.Contains(":" + 현재어절 + "("))
					{
						발견여부 = true;

						break;
					}
				}

				if (발견여부 == false && 사용자파일문자열들.Count != 0 
                    && !현재어절.Contains("0") && !현재어절.Contains("1") && !현재어절.Contains("2") && !현재어절.Contains("3") && !현재어절.Contains("4")
                    && !현재어절.Contains("5") && !현재어절.Contains("6") && !현재어절.Contains("7") && !현재어절.Contains("8") && !현재어절.Contains("9")
                    && 현재어절.Length > 1)
                    _사용자단어파일에없는단어들.Add(현재어절);
			}

			for (int i = 0; i< 모르는단어리스트.Count; i++)
			{
				for (int j = 0; j < 어절들.Count; j++)
				{
					현재어절 = 어절들[j].불필요제거().ToLower().Replace("'ll", "").Replace("'s", "").Replace("'d", "");
					while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
					while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);


					if (모르는단어리스트[i] == 현재어절)
					{
						if(!_모르는단어리스트.Contains(모르는단어리스트[i]))
							_모르는단어리스트.Add(모르는단어리스트[i]);
					}
				}
			}

			for (int i = 0; i < 모르는단어리스트_3번이하로_나온단어들.Count; i++)
			{
				for (int j = 0; j < 어절들.Count; j++)
				{
					현재어절 = 어절들[j].불필요제거().ToLower().Replace("'ll", "").Replace("'s", "").Replace("'d", "");
					while (현재어절.StartsWith("\'")) 현재어절 = 현재어절.Right(현재어절.Length - 1);
					while (현재어절.EndsWith("\'")) 현재어절 = 현재어절.Left(현재어절.Length - 1);

					if (모르는단어리스트_3번이하로_나온단어들[i] == 현재어절)
					{
						if (!_모르는단어리스트_3번이하로_나온단어들.Contains(모르는단어리스트_3번이하로_나온단어들[i]))
							_모르는단어리스트_3번이하로_나온단어들.Add(모르는단어리스트_3번이하로_나온단어들[i]);
					}
				}
			}
			#endregion

			_어휘문제들 = new List<string>();            _x어휘문제좌표 = new List<float>();            _y어휘문제좌표 = new List<float>();
            _어법문제들 = new List<string>();            _x어법문제좌표 = new List<float>();            _y어법문제좌표 = new List<float>();


            _Q_여러줄 = new List<한줄>();            
            _A0_여러줄 = new List<한줄>();            
            _A1_여러줄.Clear(); 
            _A2_여러줄.Clear();
            _A3_여러줄.Clear();
            _A4_여러줄.Clear();
            _A5_여러줄.Clear();

			if(_문법표지들 != null) _문법표지들.Clear();
			else _문법표지들	= new List<문법표지>();

	        _문법표지수	= 0;
			_본문_여러줄.Clear();


	        _기본펜			= new Pen(Color.FromArgb(255,0,0,255), 2.5f);
            _보기펜			= new Pen(Color.FromArgb(255, 0, 0, 255), 2.5f);
	        _밑줄펜			= new Pen(Color.FromArgb(100,159,191,159), 1f);
			_취소펜			= new Pen(Color.FromArgb(100,159,191,159), 1f);
            _Q글꼴			= new Font("Youth", 20f);
            _보기글꼴		= new Font("Malgun Gothic", 15f);
            _정답글꼴		= new Font("Malgun Gothic", 15f);


	        _기본글꼴		= new Font("Malgun Gothic", 15f) ;
			_굵은글꼴		= new Font("Malgun Gothic", 15f, FontStyle.Bold);


			_격자글꼴		= new Font("Malgun Gothic" , 22.5f) ;
            _문법표지글꼴 = new Font("Malgun Gothic", 10f);
            _어휘문제글꼴 = new Font("Malgun Gothic", 10f);
            _어법문제글꼴 = new Font("Malgun Gothic", 10f);


            //new SolidBrush( Color.Black);
            //_Q붓 = new SolidBrush(Color.FromArgb(22, 43, 72));
            //_Q붓 = new SolidBrush(Color.FromArgb(254, 72, 25));
            //_Q붓 = new SolidBrush(Color.FromArgb(255, 24, 74));
			_Q붓 = new SolidBrush(Color.FromArgb(0, 14, 28));

            _정답붓 = new SolidBrush(Color.FromArgb(255, 24, 74));
			_빨간붓 = new SolidBrush(Color.FromArgb(255, 24, 74));
			_검은붓 = new SolidBrush(Color.FromArgb(0, 0, 0));
			_하얀붓 = new SolidBrush(Color.FromArgb(255, 255, 255));
			_회색붓 = new SolidBrush(Color.FromArgb(125, 125, 125));
			//_오렌지붓 = new SolidBrush(Color.FromArgb(253, 106, 2));
			_푸른붓 = new SolidBrush(Color.FromArgb(38, 198, 218));
			

			_보기붓 = new SolidBrush(Color.FromArgb(181, 183, 180));

            
            _기본붓			= new SolidBrush(Color.FromArgb(0,0,0));
			_격자붓 = new SolidBrush(Color.FromArgb(100, 125, 125, 125));
			_문법표지붓 = new SolidBrush(Color.FromArgb(100, 200, 200, 200));
            _어휘문제붓 = new SolidBrush(Color.FromArgb(255, 250, 250, 255));
            _어휘문제배경 = new SolidBrush(Color.FromArgb(125, 125, 125, 125));
            _어법문제붓 = new SolidBrush(Color.FromArgb(255, 255, 250, 250));
            _어법문제배경 = new SolidBrush(Color.FromArgb(125, 125, 125, 125));
            _주제배경 = new SolidBrush(Color.FromArgb(125, 238, 135, 114));

            _기본좌표 = new PointF(-6.0f, -15.0f);

            _세로픽셀 = 215;
			//_세로픽셀 = 40;


			if(질문!= "")
				변환_문자열을_여러줄로(질문.Replace("▼", ""), _세로픽셀, ref _Q_여러줄);
			else if (제목 != "")
				변환_문자열을_여러줄로(제목, _세로픽셀, ref _Q_여러줄);


			int Q태그높이 = _Q_여러줄.Count * 35;

            _세로픽셀 += Q태그높이 + 35;

            int 본문시작높이 = _세로픽셀;



            변환_문자열을_여러줄로(본문, _세로픽셀, ref _본문_여러줄);

            _세로픽셀 += _본문_여러줄.Count * 35 + 35;

            변환_문자열을_여러줄로(ABC, _세로픽셀, ref _A0_여러줄);					_세로픽셀 += _A0_여러줄.Count * 35;
            변환_문자열을_여러줄로("① " + 보기1Text, _세로픽셀, ref _A1_여러줄);	_세로픽셀 += _A1_여러줄.Count * 35;
            변환_문자열을_여러줄로("② " + 보기2Text, _세로픽셀, ref _A2_여러줄);	_세로픽셀 += _A2_여러줄.Count * 35;
            변환_문자열을_여러줄로("③ " + 보기3Text, _세로픽셀, ref _A3_여러줄);
            _세로픽셀 += _A3_여러줄.Count * 35;
            변환_문자열을_여러줄로("④ " + 보기4Text, _세로픽셀, ref _A4_여러줄);
            _세로픽셀 += _A4_여러줄.Count * 35;
            변환_문자열을_여러줄로("⑤ " + 보기5Text, _세로픽셀, ref _A5_여러줄);
            _세로픽셀 += _A5_여러줄.Count * 35;

            _세로픽셀 += 35; // 한줄 정도 더 띄어 준다.

			if(본문.Contains("http")) { 본문 = 본문.Substring(본문.IndexOf("\n")); 본문 = 본문.Trim();}
            // 문법 표지 세팅 때문에 한 번 더 해 줌
            변환_문자열을_여러줄로(본문, 본문시작높이, ref _본문_여러줄);


			//	        _가로픽셀 = 1280;    if(_세로픽셀 < 720)	_세로픽셀 = 720;
			_높이 = 높이;
			_가로픽셀 = 너비; if (_세로픽셀 < 높이) _세로픽셀 = 높이;

			_배경과본문_비트맵 = new Bitmap(_가로픽셀, _세로픽셀);
			//_비트맵 = new Bitmap(1280, _세로픽셀);
			_배경과본문_그래픽 = Graphics.FromImage(_배경과본문_비트맵);

			#endregion

            배경그림_글자사각_넣기(본문, ref _배경과본문_그래픽);

           // 사진들넣기(ref _그래픽);
           // 더러운문제경고하기(ref _그래픽, 질문);
			// 외곽테두리넣기(ref _그래픽);


            //엠블럼넣기(ref _그래픽);

            _배경과본문_그래픽.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


			주제를그림(ref _배경과본문_그래픽);

			문법표지를그림(ref _배경과본문_그래픽);

			for (int i = 0; i < _본문_여러줄.Count; i++){
                for (int j = 0; j < _본문_여러줄[i]._어절들.Count; j++){ 본문쓰기(0, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽); }
	        }

			취소선을그림(ref _배경과본문_그래픽);

			어휘문제를그림(ref _배경과본문_그래픽);
			어법문제를그림(ref _배경과본문_그래픽);

			중요표지를그림(ref _배경과본문_그래픽);

            if(ABC.Trim() != "")
            {
                for (int i = 0; i < _A0_여러줄.Count; i++)
	            {
                    for (int j = 0; j < _A0_여러줄[i]._어절들.Count; j++)
		            {
                        보기쓰기(_A0_여러줄[i]._어절들[j].내용, _A0_여러줄[i]._어절들[j].x글자시작좌표, _A0_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
                        if(보기1)  정답쓰기(_A1_여러줄[i]._어절들[j].내용, _A1_여러줄[i]._어절들[j].x글자시작좌표, _A1_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
                        else       보기쓰기(_A1_여러줄[i]._어절들[j].내용, _A1_여러줄[i]._어절들[j].x글자시작좌표, _A1_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
		            }
	            }
            }
			#endregion
			#region 보기2
			if (보기2Text.Trim() != "")
            {
	            for(int i = 0 ; i < _A2_여러줄.Count; i++)
	            {
                    for (int j = 0; j < _A2_여러줄[i]._어절들.Count; j++)
		            {
                        if (보기2)
                            정답쓰기(_A2_여러줄[i]._어절들[j].내용, _A2_여러줄[i]._어절들[j].x글자시작좌표, _A2_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
                        else
                            보기쓰기(_A2_여러줄[i]._어절들[j].내용, _A2_여러줄[i]._어절들[j].x글자시작좌표, _A2_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
                            정답쓰기(_A3_여러줄[i]._어절들[j].내용, _A3_여러줄[i]._어절들[j].x글자시작좌표, _A3_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
                        else
                            보기쓰기(_A3_여러줄[i]._어절들[j].내용, _A3_여러줄[i]._어절들[j].x글자시작좌표, _A3_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
                            정답쓰기(_A4_여러줄[i]._어절들[j].내용, _A4_여러줄[i]._어절들[j].x글자시작좌표, _A4_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
                        else
                            보기쓰기(_A4_여러줄[i]._어절들[j].내용, _A4_여러줄[i]._어절들[j].x글자시작좌표, _A4_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
                    }
	            }
            }
			#endregion
			#region 보기5
			if (보기5Text.Trim() != "")
            {
	            for(int i = 0 ; i < _A5_여러줄.Count; i++)
	            {
                    for (int j = 0; j < _A5_여러줄[i]._어절들.Count; j++)
		            {
                        if (보기5)
                            정답쓰기(_A5_여러줄[i]._어절들[j].내용, _A5_여러줄[i]._어절들[j].x글자시작좌표, _A5_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
                        else
                            보기쓰기(_A5_여러줄[i]._어절들[j].내용, _A5_여러줄[i]._어절들[j].x글자시작좌표, _A5_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
                    }
	            }
            }
			#endregion

            _배경과본문_그래픽.TextRenderingHint = TextRenderingHint.AntiAlias;

			if(질문.Trim() != "")	제목그리기(질문.Trim(), ref _배경과본문_그래픽);
			else if(제목.Trim() != "") 제목그리기(제목.Trim(), ref _배경과본문_그래픽);

            _배경과본문_그래픽.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            return _선택된배경파일이름;
        }

		public static void 마우스따라_다시만들기(int 현재i, int 현재j, int 현재본문번호, ref List<string> 모르는단어리스트, string 제목, string 질문, string 본문, string ABC, string 보기1Text, string 보기2Text, string 보기3Text, string 보기4Text, string 보기5Text,
									bool 보기1, bool 보기2, bool 보기3, bool 보기4, bool 보기5, ref List<한줄> _본문_여러줄, ref List<한줄> _A1_여러줄, ref List<한줄> _A2_여러줄, ref List<한줄> _A3_여러줄, ref List<한줄> _A4_여러줄, ref List<한줄> _A5_여러줄, ref Graphics _배경과본문_그래픽)
		{
			if (현재i == -1) return;


			배경그림_글자사각_다시그리기(ref _배경과본문_그래픽);

			_배경과본문_그래픽.TextRenderingHint = TextRenderingHint.AntiAlias;
			
			if (질문.Trim() != "") 제목그리기(질문.Trim(), ref _배경과본문_그래픽);
			else if (제목.Trim() != "") 제목그리기(제목.Trim(), ref _배경과본문_그래픽);

			_배경과본문_그래픽.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			주제를그림(ref _배경과본문_그래픽); // 최적화되어 있어, 다시 그릴 필요가 없다.

			#region 마우스 올려진 배경의 사각형을 그림
			for (int i = 0; i < _본문_여러줄.Count; i++)
			{
				for (int j = 0; j < _본문_여러줄[i]._어절들.Count; j++)
				{
					// 마우스가 올려져 있는 어절인 경우
					if (현재i == i && 현재j == j)
					{
						// 그 줄의 맨 마지막 어절이 아닌 경우
						if (j != _본문_여러줄[i]._어절들.Count - 1)
						{
							// 현재 문장의 마지막 어절인 경우 (줄 단위가 아닌, 문장 단위의 구분)
							if (_본문_여러줄[i]._어절들[j + 1].몇번째문장인지 == 현재본문번호 + 1)
								현재문장강조하기(2, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, 0, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
							else
								현재문장강조하기(2, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j + 1].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
						}
						else
							// 그 줄의 맨 마지막 어절인 경우
							현재문장강조하기(2, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, 0, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
					}
					else if (_본문_여러줄[i]._어절들[j].몇번째문장인지 == 현재본문번호)
					{
						// 그 줄의 맨 처음 어절이고, 
						if(j == 0)
						{
							if (j == _본문_여러줄[i]._어절들.Count - 1) // 한 어절만 있는 경우
							{
								현재문장강조하기(3, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, 0, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
							}
							// 한 어절만 있지 않은 경우
							else 
							{
								// 현재 문장의 마지막 어절인 경우 (줄 단위가 아닌, 문장 단위의 구분)
								if (_본문_여러줄[i]._어절들[j + 1].몇번째문장인지 == 현재본문번호 + 1)
									현재문장강조하기(3, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, 0, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
								else
									현재문장강조하기(3, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j + 1].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
							}

						}
						// 그 줄의 맨 처음 어절도 아니고, 마지막 어절도 아닌 경우
						else if (j != _본문_여러줄[i]._어절들.Count - 1)
						{	
							// 현재 문장의 첫 어절인 경우
							if (_본문_여러줄[i]._어절들[j - 1].몇번째문장인지 == 현재본문번호 - 1)
								현재문장강조하기(3, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j + 1].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
							// 현재 문장의 마지막 어절인 경우 (줄 단위가 아닌, 문장 단위의 구분)
							else if (_본문_여러줄[i]._어절들[j+1].몇번째문장인지 == 현재본문번호 + 1)
								현재문장강조하기(1, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, 0, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
							else
								현재문장강조하기(1, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j + 1].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);


						}
						else // 그 줄의 맨 마지막 어절인 경우
							현재문장강조하기(1, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, 0, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
					}

				}
			}
			#endregion

			문법표지를다시그림(ref _배경과본문_그래픽);

			for (int i = 0; i < _본문_여러줄.Count; i++){
				for (int j = 0; j < _본문_여러줄[i]._어절들.Count; j++){
					if(현재i == i && 현재j == j) 본문쓰기(2, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
					else if(_본문_여러줄[i]._어절들[j].몇번째문장인지 == 현재본문번호)	본문쓰기(1, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽); 
					else															본문쓰기(0, _본문_여러줄[i]._어절들[j].내용, _본문_여러줄[i]._어절들[j].x글자시작좌표, _본문_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
				}
			}


			취소선을그림(ref _배경과본문_그래픽);		// 최적화되어 있어, 다시 그릴 필요가 없다.

			어휘문제를그림(ref _배경과본문_그래픽);		// 최적화되어 있어, 다시 그릴 필요가 없다.
			어법문제를그림(ref _배경과본문_그래픽);		// 최적화되어 있어, 다시 그릴 필요가 없다.

			중요표지를그림(ref _배경과본문_그래픽);		// 최적화되어 있어, 다시 그릴 필요가 없다.
			#region 보기
			if (ABC.Trim() != ""){
				for (int i = 0; i < _A0_여러줄.Count; i++){
					for (int j = 0; j < _A0_여러줄[i]._어절들.Count; j++){
						보기쓰기(_A0_여러줄[i]._어절들[j].내용, _A0_여러줄[i]._어절들[j].x글자시작좌표, _A0_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
						if (보기1) 정답쓰기(_A1_여러줄[i]._어절들[j].내용, _A1_여러줄[i]._어절들[j].x글자시작좌표, _A1_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
						else 보기쓰기(_A1_여러줄[i]._어절들[j].내용, _A1_여러줄[i]._어절들[j].x글자시작좌표, _A1_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
							정답쓰기(_A2_여러줄[i]._어절들[j].내용, _A2_여러줄[i]._어절들[j].x글자시작좌표, _A2_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
						else
							보기쓰기(_A2_여러줄[i]._어절들[j].내용, _A2_여러줄[i]._어절들[j].x글자시작좌표, _A2_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
							정답쓰기(_A3_여러줄[i]._어절들[j].내용, _A3_여러줄[i]._어절들[j].x글자시작좌표, _A3_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
						else
							보기쓰기(_A3_여러줄[i]._어절들[j].내용, _A3_여러줄[i]._어절들[j].x글자시작좌표, _A3_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
							정답쓰기(_A4_여러줄[i]._어절들[j].내용, _A4_여러줄[i]._어절들[j].x글자시작좌표, _A4_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
						else
							보기쓰기(_A4_여러줄[i]._어절들[j].내용, _A4_여러줄[i]._어절들[j].x글자시작좌표, _A4_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
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
							정답쓰기(_A5_여러줄[i]._어절들[j].내용, _A5_여러줄[i]._어절들[j].x글자시작좌표, _A5_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
						else
							보기쓰기(_A5_여러줄[i]._어절들[j].내용, _A5_여러줄[i]._어절들[j].x글자시작좌표, _A5_여러줄[i]._어절들[j].y글자시작좌표, ref _배경과본문_그래픽);
					}
				}
			}
			#endregion
			#endregion
		}

		private static void 현재문장강조하기(int 강조여부, string 내용, float 가로좌표, float 다음가로좌표, float 세로좌표, ref Graphics _그래픽)
		{
			float 더해질_가로좌표 = 0;
			float 너비 = 0;

			if (강조여부 == 3) // 문장의 맨 첫 부분, 앞 쪽의 너비가 약간 더 넓음
			{
				//						if (모르는단어여부) _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _빨간붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
				//						else _그래픽.DrawString(내용.Substring(i, 1), _기본글꼴, _빨간붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
				
				float 검은너비 = 현재어절의너비확인(내용);

				if (다음가로좌표 == 0) 너비 = 검은너비;
				else 너비 = 다음가로좌표 - 가로좌표 + 3;

				// 특정한 줄의 맨 첫번째 어절인데,
				if (다음가로좌표 != 0)
				{
					var rectF = new RectangleF(가로좌표 - 11 + 더해질_가로좌표 + 320, 세로좌표 - 11, 너비 + 12, 26);
					var rect = Rectangle.Round(rectF);

					둥근사각형을그림(ref _그래픽, _검은붓, rect, 3);
				}
				else
				{
					var rectF = new RectangleF(가로좌표 - 11 + 더해질_가로좌표 + 320, 세로좌표 - 11, 검은너비 + 8, 26);
					var rect = Rectangle.Round(rectF);

					둥근사각형을그림(ref _그래픽, _검은붓, rect, 3);
				}
			}
			if (강조여부 == 2) // 오렌지 색으로 강조하고, 너비가 약간 더 넓음
			{
				//						if (모르는단어여부) _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _빨간붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
				//						else _그래픽.DrawString(내용.Substring(i, 1), _기본글꼴, _빨간붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));

				float 오렌지너비 = 현재어절의너비확인(내용);

				if (다음가로좌표 == 0) 너비 = 오렌지너비;
				else 너비 = 다음가로좌표 - 가로좌표 + 3;

				var rectF = new RectangleF(가로좌표 - 6 + 더해질_가로좌표 + 320, 세로좌표 - 11, 너비, 26);
				var rect = Rectangle.Round(rectF);


				둥근사각형을그림(ref _그래픽, _검은붓, rect, 0);


				// 오렌지 컬러
				var 오렌지rectF = new RectangleF(가로좌표 - 11 + 더해질_가로좌표 + 320, 세로좌표 - 11, 오렌지너비 + 12, 26);
				var 오렌지rect = Rectangle.Round(오렌지rectF);

				둥근사각형을그림(ref _그래픽, _푸른붓, 오렌지rect, 3);

			}
			else if (강조여부 == 1)
			{
					if (다음가로좌표 == 0)
					{
						너비 = 현재어절의너비확인(내용);

						var rectF = new RectangleF(가로좌표 - 8 + 더해질_가로좌표 + 320, 세로좌표 - 11, 너비 + 9, 26);
						var rect = Rectangle.Round(rectF);


						둥근사각형을그림(ref _그래픽, _검은붓, rect, 3);
					}
					else
					{
						너비 = 다음가로좌표 - 가로좌표 + 3;

						var rectF = new RectangleF(가로좌표 - 6 + 더해질_가로좌표 + 320, 세로좌표 - 11, 너비, 26);
						var rect = Rectangle.Round(rectF);


						둥근사각형을그림(ref _그래픽, _검은붓, rect, 0);
					}
			}
			else if (강조여부 == 0)
			{
				//if (모르는단어여부) _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _기본붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
				//else _그래픽.DrawString(내용.Substring(i, 1), _기본글꼴, _기본붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
			}

		}

		private static void 본문쓰기(int 강조여부, string 내용, float 가로좌표, float 세로좌표, ref Graphics _그래픽)
        {
			bool 빈도사전에없는여부 = false;
			bool 모르는단어여부 = false;
			bool 모르는단어여부_3번이하 = false;

			float 더해질_가로좌표 = 0;

            string 불필요제거한내용 = 내용.불필요제거().ToLower();//.Replace("'ll", "").Replace("'s", "").Replace("'d", "");

            if (!불필요제거한내용.StartsWith("'ll")) 불필요제거한내용 = 불필요제거한내용.Replace("'ll", "");
            if (!불필요제거한내용.StartsWith("'s")) 불필요제거한내용 = 불필요제거한내용.Replace("'s", "");
            if (!불필요제거한내용.StartsWith("'d")) 불필요제거한내용 = 불필요제거한내용.Replace("'d", "");



            while (불필요제거한내용.StartsWith("\'")) 불필요제거한내용 = 불필요제거한내용.Right(불필요제거한내용.Length - 1);
			while (불필요제거한내용.EndsWith("\'")) 불필요제거한내용 = 불필요제거한내용.Left(불필요제거한내용.Length - 1);

			for (int i = 0; i < _모르는단어리스트_3번이하로_나온단어들.Count; i++)
			{
				if (_모르는단어리스트_3번이하로_나온단어들[i] == 불필요제거한내용)
				{
					모르는단어여부_3번이하 = true;
				}
			}


			for (int i = 0; i < _모르는단어리스트.Count; i++)
			{
				if(_모르는단어리스트[i] == 불필요제거한내용)
				{
					모르는단어여부 = true;
				}
			}

			for(int i = 0; i <  _사용자단어파일에없는단어들.Count; i++)
			{
				if(_사용자단어파일에없는단어들[i] == 불필요제거한내용)
				{
					빈도사전에없는여부 = true;
				}
			}

			if (내용.Contains("<TBAR></TBAR>"))
			{
				_그래픽.DrawString("--------------------------------------------------------------------", _기본글꼴, _기본붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
			}
			else
			{
				for(int i = 0 ; i < 내용.Length; i++)
				{
					if (강조여부 == 2)
					{
						if (빈도사전에없는여부) _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _회색붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
						else if (모르는단어여부_3번이하) _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _하얀붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
						else if (모르는단어여부) _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _빨간붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
						else _그래픽.DrawString(내용.Substring(i, 1), _기본글꼴, _하얀붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
					}
					else if (강조여부 == 1)
					{
						if (빈도사전에없는여부) { _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _회색붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15)); }
						else if (모르는단어여부_3번이하)	{_그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _하얀붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));}
						else if (모르는단어여부)	{_그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _빨간붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));}
						else
						{
							//var rectF = new RectangleF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15, 개별코드에대한너비(내용[i]), 30);
							//var rect = Rectangle.Round(rectF);

							//FillRoundedRectangle(ref _그래픽, _어법문제배경, rect, 4);
							_그래픽.DrawString(내용.Substring(i, 1), _기본글꼴, _하얀붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
						}
					}
					else if(강조여부 == 0)
					{
						if (빈도사전에없는여부) {	_그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _회색붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));}
						else if (모르는단어여부_3번이하){	_그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _기본붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));}
						else if (모르는단어여부) _그래픽.DrawString(내용.Substring(i, 1), _굵은글꼴, _빨간붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
						else _그래픽.DrawString(내용.Substring(i, 1), _기본글꼴, _기본붓, new PointF(가로좌표 - 6 + 더해질_가로좌표 + 318, 세로좌표 - 15));
					}

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
		private static void 제목그리기(string 내용, ref Graphics _그래픽)
        {
			if(내용.Contains("1형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌"))		{ 내용 = "1형식 고르기";			goto 마지막부분; }
			if(내용.Contains("1형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌"))		{ 내용 = "1형식 아닌 것 고르기";	goto 마지막부분; }

			if(내용.Contains("2형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌"))		{ 내용 = "2형식 고르기";			goto 마지막부분; }
			if(내용.Contains("2형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌"))		{ 내용 = "2형식 아닌 것 고르기";	goto 마지막부분; }

			if(내용.Contains("3형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌"))		{ 내용 = "3형식 고르기";			goto 마지막부분; }
			if(내용.Contains("3형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌"))		{ 내용 = "3형식 아닌 것 고르기";	goto 마지막부분; }

			if(내용.Contains("4형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌"))		{ 내용 = "4형식 고르기";			goto 마지막부분; }
			if(내용.Contains("4형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌"))		{ 내용 = "4형식 아닌 것 고르기";	goto 마지막부분; }

			if(내용.Contains("5형식 문장") && 내용.Contains("고르세요.") && !내용.Contains("아닌"))		{ 내용 = "5형식 고르기";			goto 마지막부분; }
			if(내용.Contains("5형식 문장") && 내용.Contains("고르세요.") && 내용.Contains("아닌"))		{ 내용 = "5형식 아닌 것 고르기";	goto 마지막부분; }

			if(내용.Contains("빈칸에 적절한 말을") && 내용.Contains("주제를 완성"))	{ 내용 = "빈칸에 들어갈 주제 완성";	goto 마지막부분; }
			if(내용.Contains("빈칸") && 내용.Contains("어형을 바꾸시오"))			{ 내용 = "빈칸에 들어갈 어휘";		goto 마지막부분; }


			if (내용.Contains("영작하세요"))										{ 내용 = "영작(을 가장한 어법)";	goto 마지막부분; }

			if(내용.Contains("일치하지 않는 것"))									{ 내용 = "일치하지 않는 것";		goto 마지막부분; }
			if(내용.Contains("일치 하지 않는 것"))									{ 내용 = "일치하지 않는 것";		goto 마지막부분; }

			if (내용.Contains("일치하는 것"))										{ 내용 = "일치하는 것";				goto 마지막부분; }
			if(내용.Contains("어법상 어색한 것"))									{ 내용 = "어법상 틀린 것";			goto 마지막부분; }

			if(내용.Contains("어법상 틀린 것"))										{ 내용 = "어법상 틀린 것";			goto 마지막부분; }
			if(내용.Contains("어법에 맞는 표현"))									{ 내용 = "어법에 맞는 표현";		goto 마지막부분; }
			if(내용.Contains("어법에 알맞는 표현"))									{ 내용 = "어법에 맞는 표현";		goto 마지막부분; }

			if(내용.Contains("문맥에 맞는 낱말"))									{ 내용 = "문맥에 맞는 낱말";		goto 마지막부분; }

			if(내용.Contains("낱말의 쓰임이 적절하지 않은 것은"))					{ 내용 = "적절하지 않은 낱말";		goto 마지막부분; }
			if(내용.Contains("순서에 맞게 배열한 것"))								{ 내용 = "순서에 맞게 배열한 것";	goto 마지막부분; }

			if (내용.Contains("나머지 넷과 다른 것"))								{ 내용 = "나머지 넷과 다른 것";		goto 마지막부분; }
			if (내용.Contains("빈칸에 들어갈 말"))									{ 내용 = "빈칸에 들어갈 말";		goto 마지막부분; }

			if (내용.Contains("언급하지 않은 것"))									{ 내용 = "언급하지 않은 것";		goto 마지막부분; }
			if (내용.Contains("언급되지 않은 것"))									{ 내용 = "언급되지 않은 것";		goto 마지막부분; }

			if (내용.Contains("남자가") && 내용.Contains("선택한"))					{ 내용 = "남자가 선택한 것";		goto 마지막부분; }
			if (내용.Contains("여자가") && 내용.Contains("선택한"))					{ 내용 = "여자가 선택한 것";		goto 마지막부분; }
			if (내용.Contains("주어진 문장") && 내용.Contains("들어"))				{ 내용 = "문장 삽입";				goto 마지막부분; }
			if (내용.Contains("관한 내용으로") && 내용.Contains("적절하지 않은"))	{ 내용 = "일치하지 않는 것";		goto 마지막부분; }

			if(내용.Contains("요약"))												{ 내용 = "문장 요약";				goto 마지막부분; }
			if(내용.Contains("본문 정리"))											{ 내용 = "본문";					goto 마지막부분; }
			if(내용.Contains("본문정리"))											{ 내용 = "본문";					goto 마지막부분; }

			내용 = 내용.Replace("10. ", ""); 내용 = 내용.Replace("11. ", ""); 내용 = 내용.Replace("12. ", ""); 내용 = 내용.Replace("13. ", ""); 내용 = 내용.Replace("14. ", "");
			내용 = 내용.Replace("15. ", ""); 내용 = 내용.Replace("16. ", ""); 내용 = 내용.Replace("17. ", ""); 내용 = 내용.Replace("18. ", ""); 내용 = 내용.Replace("19. ", "");
			내용 = 내용.Replace("20. ", ""); 내용 = 내용.Replace("21. ", ""); 내용 = 내용.Replace("22. ", ""); 내용 = 내용.Replace("23. ", ""); 내용 = 내용.Replace("24. ", "");
			내용 = 내용.Replace("25. ", ""); 내용 = 내용.Replace("26. ", ""); 내용 = 내용.Replace("27. ", ""); 내용 = 내용.Replace("28. ", ""); 내용 = 내용.Replace("29. ", "");
			내용 = 내용.Replace("30. ", ""); 내용 = 내용.Replace("31. ", ""); 내용 = 내용.Replace("32. ", ""); 내용 = 내용.Replace("33. ", ""); 내용 = 내용.Replace("34. ", "");
			내용 = 내용.Replace("35. ", ""); 내용 = 내용.Replace("36. ", ""); 내용 = 내용.Replace("37. ", ""); 내용 = 내용.Replace("38. ", ""); 내용 = 내용.Replace("39. ", "");
			내용 = 내용.Replace("40. ", ""); 내용 = 내용.Replace("41. ", ""); 내용 = 내용.Replace("42. ", ""); 내용 = 내용.Replace("43. ", ""); 내용 = 내용.Replace("44. ", "");
			내용 = 내용.Replace("45. ", ""); 내용 = 내용.Replace("46. ", ""); 내용 = 내용.Replace("47. ", ""); 내용 = 내용.Replace("48. ", ""); 내용 = 내용.Replace("49. ", "");
			내용 = 내용.Replace("50. ", ""); 

			내용 = 내용.Replace("1. ", ""); 내용 = 내용.Replace("2. ", ""); 내용 = 내용.Replace("3. ", "");  내용 = 내용.Replace("4. ", ""); 내용 = 내용.Replace("5. ", "");
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
			// 글자 크기를 확인한 후, 글자 크기가 너비를 넘어가면 어절단위로 다음줄로 넘겨준다.
			if(문장길이확인(내용) * 2 >_우한계 - _좌한계)
			{
				List<string> 제목어절들 = new List<string>();


				변환.문자열.어절들로(내용, ref 제목어절들);

				string 현재까지의제목어절들 = "";
				string 바로전까지의제목어절들 = "";

				for(int i = 0; i < 제목어절들.Count; i++)
				{
					현재까지의제목어절들 += 제목어절들[i] + " ";

					if (문장길이확인(현재까지의제목어절들) * 2 > _우한계 - _좌한계)
					{
						_그래픽.DrawString(바로전까지의제목어절들 + "...", _Q글꼴, _Q붓, 357, 80);

						break;
					}
					else
					{
						바로전까지의제목어절들 += 제목어절들[i] + " ";
					}
				}
			}
			else
				_그래픽.DrawString(내용, _Q글꼴, _Q붓, 357, 80);
        }

		// 원래 동영상용 화면에서 나오는 한글과 영어를 적기 위한 용도로 사용되었던 함수이다.
		// 필요에 맞도록 수치를 키워주면 좋다.
		public static int 문장길이확인(string 문자열)
		{
			float 문장길이 = 0;

			for (int i = 0; i < 문자열.Length; i++)
			{
				문장길이 += 문자길이확인(문자열[i]);
			}

			return (int)문장길이;
		}
		private static float 문자길이확인(char 문자)
		{
			if (문자 == 'A') return 8;
			else if (문자 == 'B') return 7;
			else if (문자 == 'C') return 7;
			else if (문자 == 'D') return 7;
			else if (문자 == 'E') return 7;
			else if (문자 == 'F') return 7;
			else if (문자 == 'G') return 8;
			else if (문자 == 'H') return 7;
			else if (문자 == 'I') return 3;
			else if (문자 == 'J') return 5;
			else if (문자 == 'K') return 7;
			else if (문자 == 'L') return 7;
			else if (문자 == 'M') return 11;
			else if (문자 == 'N') return 9;
			else if (문자 == 'O') return 9;
			else if (문자 == 'P') return 7;
			else if (문자 == 'Q') return 7;
			else if (문자 == 'R') return 7;
			else if (문자 == 'S') return 7;
			else if (문자 == 'T') return 7;
			else if (문자 == 'U') return 7;
			else if (문자 == 'V') return 7;
			else if (문자 == 'W') return 9;
			else if (문자 == 'X') return 7;
			else if (문자 == 'Y') return 7;
			else if (문자 == 'Z') return 7;
			else if (문자 == 'a') return 6;
			else if (문자 == 'b') return 8;
			else if (문자 == 'c') return 6;
			else if (문자 == 'd') return 7;
			else if (문자 == 'e') return 7;
			else if (문자 == 'f') return 5;
			else if (문자 == 'g') return 7;
			else if (문자 == 'h') return 7;
			else if (문자 == 'i') return 3;
			else if (문자 == 'j') return 4;
			else if (문자 == 'k') return 6;
			else if (문자 == 'l') return 3;
			else if (문자 == 'm') return 11;
			else if (문자 == 'n') return 7;
			else if (문자 == 'o') return 7;
			else if (문자 == 'p') return 7;
			else if (문자 == 'q') return 6;
			else if (문자 == 'r') return 6;
			else if (문자 == 's') return 6;
			else if (문자 == 't') return 4;
			else if (문자 == 'u') return 7;
			else if (문자 == 'v') return 6;
			else if (문자 == 'w') return 9;
			else if (문자 == 'x') return 6;
			else if (문자 == 'y') return 6;
			else if (문자 == 'z') return 6;

			else if (문자 == '1') return 6;
			else if (문자 == '2') return 6;
			else if (문자 == '3') return 6;
			else if (문자 == '4') return 6;
			else if (문자 == '5') return 6;
			else if (문자 == '6') return 6;
			else if (문자 == '7') return 6;
			else if (문자 == '8') return 6;
			else if (문자 == '9') return 6;
			else if (문자 == '0') return 6;
			else if (문자 == '?') return 6;
			else if (문자 == '!') return 3;
			else if (문자 == '.') return 3;
			else if (문자 == ',') return 3;
			else if (문자 == '“') return 4;
			else if (문자 == '‘') return 3;
			else if (문자 == '’') return 3;

			else if (문자 == '”') return 4;
			else if (문자 == ':') return 3;
			else if (문자 == '/') return 5;

			else if (문자 == '(') return 5;
			else if (문자 == ')') return 5;


			else if (문자 == '\"') return 3;
			else if (문자 == '\'') return 3;
			else if (문자 == ' ') return 6.8f;
			else if (문자 == '-') return 5;


			else return 12.5f;
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
				var rectF = new RectangleF(_x어법문제좌표[i] + 좌여백, _y어법문제좌표[i] - 6f, size.Width, size.Height - 5);
				var rect = Rectangle.Round(rectF);

				둥근사각형을그림(ref _그래픽, _어법문제배경, rect, 4);
				_그래픽.DrawString(_어법문제들[i], _어법문제글꼴, _어법문제붓, new PointF(_x어법문제좌표[i] + 좌여백, _y어법문제좌표[i] - 10f));
            }
        }

		public static void 둥근사각형을그림(ref Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");
			if (brush == null)
				throw new ArgumentNullException("brush");

			using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
			{
				graphics.FillPath(brush, path);
			}
		}

		public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
		{
			int diameter = radius * 2;
			Size size = new Size(diameter, diameter);
			Rectangle arc = new Rectangle(bounds.Location, size);
			GraphicsPath path = new GraphicsPath();

			if (radius == 0)
			{
				path.AddRectangle(bounds);
				return path;
			}

			// top left arc  
			path.AddArc(arc, 180, 90);

			// top right arc  
			arc.X = bounds.Right - diameter;
			path.AddArc(arc, 270, 90);

			// bottom right arc  
			arc.Y = bounds.Bottom - diameter;
			path.AddArc(arc, 0, 90);

			// bottom left arc 
			arc.X = bounds.Left;
			path.AddArc(arc, 90, 90);

			path.CloseFigure();
			return path;
		}

		private static void 주제를그림(ref Graphics _그래픽)
		{
			List<int> 주제시작x좌표 = new List<int>();
			List<int> 주제시작y좌표 = new List<int>();

			List<int> 주제끝x좌표 = new List<int>();
			List<int> 주제끝y좌표 = new List<int>();

			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "주제") 
					{
						주제시작x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 주제시작y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); 
					}
					else if (_문법표지들[i].문법표지문자열 == "/주제") 
					{
						주제끝x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 주제끝y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); 
					}
				}
			}

			for (int i = 0; i < 주제끝y좌표.Count; i++)
			{
				if (주제끝y좌표[i] - 주제시작y좌표[i] == 0)
				{
					var rectF = new RectangleF(주제시작x좌표[i] + _좌여백 - 5, 주제시작y좌표[i] - 30f, 주제끝x좌표[i] - 주제시작x좌표[i] + 10, 25);
					var rect = Rectangle.Round(rectF);

					둥근사각형을그림(ref _그래픽, _주제배경, rect, 4);
				}

				if ((주제끝y좌표[i] - 주제시작y좌표[i]) % 35 == 0 && (주제끝y좌표[i] != 주제시작y좌표[i]))
				{

					var rectF = new RectangleF(주제시작x좌표[i] + _좌여백 -5, 주제시작y좌표[i] - 30, _우한계 - 주제시작x좌표[i] - _좌여백 + 10, 25);
					var rect = Rectangle.Round(rectF);

					둥근사각형을그림(ref _그래픽, _주제배경, rect, 4);

					for (int j = 0; j < (주제끝y좌표[i] - 주제시작y좌표[i]) / 35 - 1; j++)
					{
						rectF = new RectangleF(_좌한계 - 5, (int)(주제시작y좌표[i] + 5 + 35f * j), _우한계 - _좌한계 +10, 25);
						rect = Rectangle.Round(rectF);

						둥근사각형을그림(ref _그래픽, _주제배경, rect, 4);
					}

					rectF = new RectangleF(_좌한계 - 5, (int)(주제끝y좌표[i] - 30f), (int)(주제끝x좌표[i] - 45), 25);
					rect = Rectangle.Round(rectF);

					둥근사각형을그림(ref _그래픽, _주제배경, rect, 4);
				}
			}
		}

		private static Bitmap		_문법표지_비트맵;
		private static Graphics		_문법표지_그래픽;
			
		private static Image		_지시				= null;
		private static Image		_제목				= null;
		private static Image		_속담				= null;
		private static Image		_빈칸				= null;
		private static Image		_빈칸중간			= null;
		private static Image		_빈칸끝				= null;
		private static Image		_요약				= null;
		private static Image		_분위기				= null;
		private static Image		_일치				= null;
		private static Image		_흐름				= null;
		private static Image		_주어				= null;
		private static Image		_가주어				= null;
		private static Image		_서술어				= null;
		private static Image		_목적어				= null;
		private static Image		_간접목적어			= null;
		private static Image		_직접목적어			= null;
		private static Image		_괄호직접목적어		= null;
		private static Image		_보어				= null;
		private static Image		_괄호보어			= null;
		private static Image		_접속어				= null;

		private static void 문법표지를그림(ref Graphics _그래픽)
		{
			#region 초기화
			_문법표지_비트맵 = new Bitmap(_가로픽셀, _세로픽셀);
			_문법표지_그래픽 = Graphics.FromImage(_문법표지_비트맵);

			_문법표지_그래픽.TextRenderingHint = TextRenderingHint.AntiAlias;

			List<int> 빈칸시작x좌표 = new List<int>();
			List<int> 빈칸시작y좌표 = new List<int>();

			List<int> 빈칸끝x좌표 = new List<int>();
			List<int> 빈칸끝y좌표 = new List<int>();

			if (_지시			== null)		{ _지시				= Image.FromFile(_그림폴더 + "문제유형아이콘/지시.png");				}
			if (_제목			== null)		{ _제목				= Image.FromFile(_그림폴더 + "문제유형아이콘/제목.png");				}
			if (_속담			== null)		{ _속담				= Image.FromFile(_그림폴더 + "문제유형아이콘/속담.png");				}
			if (_빈칸			== null)		{ _빈칸				= Image.FromFile(_그림폴더 + "문제유형아이콘/빈칸_좌측.png");		}
			if (_빈칸중간		== null)		{ _빈칸중간			= Image.FromFile(_그림폴더 + "문제유형아이콘/빈칸_중간.png");		}
			if (_빈칸끝			== null)		{ _빈칸끝			= Image.FromFile(_그림폴더 + "문제유형아이콘/빈칸_우측.png");		}
			if (_요약			== null)		{ _요약				= Image.FromFile(_그림폴더 + "문제유형아이콘/요약.png");				}
			if (_분위기			== null)		{ _분위기			= Image.FromFile(_그림폴더 + "문제유형아이콘/분위기.png");			}
			if (_일치			== null)		{ _일치				= Image.FromFile(_그림폴더 + "문제유형아이콘/일치.png");				}
			if (_흐름			== null)		{ _흐름				= Image.FromFile(_그림폴더 + "문제유형아이콘/흐름.png");				}

			if (_주어			== null)		{ _주어				= Image.FromFile(_그림폴더 + "문제유형아이콘/주어.png");				}
			if (_가주어			== null)		{ _가주어			= Image.FromFile(_그림폴더 + "문제유형아이콘/가주어.png");			}
			if (_서술어			== null)		{ _서술어			= Image.FromFile(_그림폴더 + "문제유형아이콘/서술어.png");			}
			if (_목적어			== null)		{ _목적어			= Image.FromFile(_그림폴더 + "문제유형아이콘/목적어.png");			}
			if (_간접목적어		== null)		{ _간접목적어		= Image.FromFile(_그림폴더 + "문제유형아이콘/간접목적어.png");		}
			if (_직접목적어		== null)		{ _직접목적어		= Image.FromFile(_그림폴더 + "문제유형아이콘/직접목적어.png");		}
			if (_괄호직접목적어 == null)		{ _괄호직접목적어	= Image.FromFile(_그림폴더 + "문제유형아이콘/괄호직접목적어.png");	}
			if (_보어			== null)		{ _보어				= Image.FromFile(_그림폴더 + "문제유형아이콘/보어.png");				}
			if (_괄호보어		== null)		{ _괄호보어			= Image.FromFile(_그림폴더 + "문제유형아이콘/괄호보어.png");			}
			if (_접속어			== null)		{ _접속어			= Image.FromFile(_그림폴더 + "문제유형아이콘/접속어.png");			}

			#endregion
			#region 구현부분
			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "제목") { _문법표지_그래픽.DrawImage(_제목, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
					else if (_문법표지들[i].문법표지문자열 == "속담") { _문법표지_그래픽.DrawImage(_속담, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }

					else if (_문법표지들[i].문법표지문자열 == "빈칸") { 빈칸시작x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 빈칸시작y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); }
					else if (_문법표지들[i].문법표지문자열 == "/빈칸") { 빈칸끝x좌표.Add((int)_문법표지들[i].x문법표지시작좌표); 빈칸끝y좌표.Add((int)_문법표지들[i].y문법표지시작좌표); }

					else if (_문법표지들[i].문법표지문자열 == "요약") { _문법표지_그래픽.DrawImage(_요약, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
				}
			}

			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "흐름")
						_문법표지_그래픽.DrawImage(_흐름, new Rectangle((int)(_좌한계) - 44, (int)(_문법표지들[i].y문법표지시작좌표 - 47.0f), 638, 100), new Rectangle(570 - (int)_문법표지들[i].x문법표지시작좌표, 0, 638, 100), System.Drawing.GraphicsUnit.Pixel);
				}
			}

			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지문자열 == "지시") { _문법표지_그래픽.DrawImage(_지시, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
				else if (_문법표지들[i].문법표지문자열 == "분위기") { _문법표지_그래픽.DrawImage(_분위기, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
			}

			#region 빈칸
			for (int i = 0; i < 빈칸끝y좌표.Count; i++)
			{
				_문법표지_그래픽.DrawImage(_빈칸, new Rectangle((int)(빈칸시작x좌표[i] - 6.5f + _좌여백), (int)(빈칸시작y좌표[i] - 40f), 25, 50));

				_문법표지_그래픽.DrawImage(_빈칸끝, new Rectangle((int)(빈칸끝x좌표[i] - 17f + _좌여백), (int)(빈칸끝y좌표[i] - 40f), 25, 50));


				if (빈칸끝y좌표[i] - 빈칸시작y좌표[i] == 0)
					_문법표지_그래픽.DrawImage(_빈칸중간, 빈칸시작x좌표[i] + 15 + _좌여백, 빈칸시작y좌표[i] - 40.0f, 빈칸끝x좌표[i] - 빈칸시작x좌표[i] - 20.0f, 50);

				if (((빈칸끝y좌표[i] - 빈칸시작y좌표[i]) % 35 == 0) && (빈칸끝y좌표[i] != 빈칸시작y좌표[i]))
				{
					_문법표지_그래픽.DrawImage(_빈칸중간, new Rectangle((int)(빈칸시작x좌표[i] + 10 + _좌여백), (int)(빈칸시작y좌표[i] - 40f), _우한계 - (int)(빈칸시작x좌표[i] - 7.5f + 18 + _좌여백), 50));

					for (int j = 0; j < (빈칸끝y좌표[i] - 빈칸시작y좌표[i]) / 35 - 1; j++)
						_문법표지_그래픽.DrawImage(_빈칸중간, new Rectangle(_좌한계, (int)(빈칸시작y좌표[i] - 5f + 35f * j), _우한계 - _좌한계, 50));

					_문법표지_그래픽.DrawImage(_빈칸중간, new Rectangle(_좌한계, (int)(빈칸끝y좌표[i] - 40f), (int)(빈칸끝x좌표[i] - 60 - 7.5f), 50));
				}
			}
			#endregion

			for (int i = 0; i < _문법표지수; i++)
			{
				if ((_문법표지들[i].문법표지종류 == "격자") && (_문법표지들[i].문법표지문자열 != "ⓧ"))
				{
					_문법표지_그래픽.DrawString("[", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백, _문법표지들[i].y문법표지시작좌표 - 35f - 7f));
					_문법표지_그래픽.DrawString("]", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지끝좌표 - 10f + _좌여백, _문법표지들[i].y문법표지끝좌표 - 35f - 7f));

					if (_문법표지들[i].문법표지문자열 == "ⓢ") _문법표지_그래픽.DrawImage(_주어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "(ⓢ)") _문법표지_그래픽.DrawImage(_가주어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓥ") _문법표지_그래픽.DrawImage(_서술어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓞ") _문법표지_그래픽.DrawImage(_목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓒ") _문법표지_그래픽.DrawImage(_보어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "(ⓒ)") _문법표지_그래픽.DrawImage(_괄호보어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓘ") _문법표지_그래픽.DrawImage(_간접목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "ⓓ") _문법표지_그래픽.DrawImage(_직접목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "(ⓓ)") _문법표지_그래픽.DrawImage(_괄호직접목적어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
					else if (_문법표지들[i].문법표지문자열 == "㉨") _문법표지_그래픽.DrawImage(_접속어, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 8f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 30f), 10, 7));
				}
				else if ((_문법표지들[i].문법표지종류 == "격자") && (_문법표지들[i].문법표지문자열 == "ⓧ"))
				{
					_문법표지_그래픽.DrawString("(", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백, _문법표지들[i].y문법표지시작좌표 - 35 - 7f));
					_문법표지_그래픽.DrawString(")", _격자글꼴, _격자붓, new PointF(_문법표지들[i].x문법표지끝좌표 - 10f + _좌여백, _문법표지들[i].y문법표지끝좌표 - 35 - 7f));

				}
				else if ((_문법표지들[i].문법표지종류 == "밑줄") && (_문법표지들[i].문법표지문자열 != "ⓧ"))
				{
					if (_문법표지들[i].y문법표지시작좌표 == _문법표지들[i].y문법표지끝좌표)
					{
						_문법표지_그래픽.DrawLine(_밑줄펜, _문법표지들[i].x문법표지시작좌표 + _좌여백, _문법표지들[i].y문법표지시작좌표 - 7f, _문법표지들[i].x문법표지끝좌표 + _좌여백, _문법표지들[i].y문법표지끝좌표 - 7f);

						if (_문법표지들[i].문법표지문자열 == "ⓢ") _문법표지_그래픽.DrawImage(_주어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓢ)") _문법표지_그래픽.DrawImage(_가주어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓥ") _문법표지_그래픽.DrawImage(_서술어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓞ") _문법표지_그래픽.DrawImage(_목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓒ") _문법표지_그래픽.DrawImage(_보어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓒ)") _문법표지_그래픽.DrawImage(_괄호보어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓘ") _문법표지_그래픽.DrawImage(_간접목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓓ") _문법표지_그래픽.DrawImage(_직접목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓓ)") _문법표지_그래픽.DrawImage(_괄호직접목적어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "㉨") _문법표지_그래픽.DrawImage(_접속어, new Rectangle((int)(((_문법표지들[i].x문법표지시작좌표 + _문법표지들[i].x문법표지끝좌표) / 2) - 4f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
					}
					else
					{

						_문법표지_그래픽.DrawLine(_밑줄펜, _문법표지들[i].x문법표지시작좌표 + _좌여백, _문법표지들[i].y문법표지시작좌표 - 7f, _우한계, _문법표지들[i].y문법표지시작좌표 - 7f);
						_문법표지_그래픽.DrawLine(_밑줄펜, 368, _문법표지들[i].y문법표지끝좌표 - 7f, _문법표지들[i].x문법표지끝좌표 + _좌여백, _문법표지들[i].y문법표지끝좌표 - 7f);

						// 만약 두 줄 이상이면,
						if (_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표 >= 70)
						{
							int 시작과끝의줄간격 = (int)(_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표);
							시작과끝의줄간격 /= 35;

							for (int i더할줄 = 0; i더할줄 < 시작과끝의줄간격 - 1; i더할줄++)
							{
								_문법표지_그래픽.DrawLine(_밑줄펜, 368, _문법표지들[i].y문법표지시작좌표 - 7f + 35f * (i더할줄 + 1), _우한계, _문법표지들[i].y문법표지시작좌표 - 7f + 35f * (i더할줄 + 1));
							}
						}

						if (_문법표지들[i].문법표지문자열 == "ⓢ") _문법표지_그래픽.DrawImage(_주어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓢ)") _문법표지_그래픽.DrawImage(_가주어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓥ") _문법표지_그래픽.DrawImage(_서술어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓞ") _문법표지_그래픽.DrawImage(_목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓒ") _문법표지_그래픽.DrawImage(_보어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓒ)") _문법표지_그래픽.DrawImage(_괄호보어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓘ") _문법표지_그래픽.DrawImage(_간접목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "ⓓ") _문법표지_그래픽.DrawImage(_직접목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "(ⓓ)") _문법표지_그래픽.DrawImage(_괄호직접목적어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
						else if (_문법표지들[i].문법표지문자열 == "㉨") _문법표지_그래픽.DrawImage(_접속어, new Rectangle(_우문법기호한계, (int)(_문법표지들[i].y문법표지시작좌표 - 10f), 10, 7));
					}
				}
			}


			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "일치") { _문법표지_그래픽.DrawImage(_일치, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 12.5f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 - 50f), 25, 50)); }
				}
			}

			#endregion

			_그래픽.DrawImage(_문법표지_비트맵, 0, 0);
		}

		private static void 문법표지를다시그림(ref Graphics _그래픽)
		{
			//_그래픽.DrawImage(_문법표지_비트맵, 0, 0);
			_그래픽.DrawImage(_문법표지_비트맵, _좌한계 - 20, 0, new Rectangle(_좌한계 - 20, 0, _우한계 - _좌한계 + 40, _세로픽셀), GraphicsUnit.Pixel);
		}

		private static Image _중요 = null;
		private static void 중요표지를그림(ref Graphics _그래픽)
		{
			if(_중요 == null)
				_중요 = Image.FromFile(_그림폴더 + "문제유형아이콘/중요.png");

			for (int i = 0; i < _문법표지수; i++)
			{
				if (_문법표지들[i].문법표지종류 == "문제")
				{
					if (_문법표지들[i].문법표지문자열 == "중요") { _그래픽.DrawImage(_중요, new Rectangle((int)(_문법표지들[i].x문법표지시작좌표 - 30f + _좌여백), (int)(_문법표지들[i].y문법표지시작좌표 + -70f), 64, 64)); }
				}
			}
		}

        private static void 취소선을그림(ref Graphics _그래픽)
        {
			for (int i = 0 ; i < _문법표지수 ; i++)
	        {
		        if((_문법표지들[i].문법표지종류 == "밑줄") && (_문법표지들[i].문법표지문자열 == "ⓧ"))
		        {
			        if(_문법표지들[i].y문법표지시작좌표 == _문법표지들[i].y문법표지끝좌표)
						_그래픽.DrawLine(_취소펜, _문법표지들[i].x문법표지시작좌표 + _좌여백, _문법표지들[i].y문법표지시작좌표 - 17f, _문법표지들[i].x문법표지끝좌표 + _좌여백, _문법표지들[i].y문법표지끝좌표 - 17f);
			        else
			        {
                        _그래픽.DrawLine(_취소펜, _문법표지들[i].x문법표지시작좌표 + _좌여백, _문법표지들[i].y문법표지시작좌표 - 17f, _우한계, _문법표지들[i].y문법표지시작좌표 - 17f);
                        _그래픽.DrawLine(_취소펜, 368, _문법표지들[i].y문법표지끝좌표 - 17f, _문법표지들[i].x문법표지끝좌표 + _좌여백, _문법표지들[i].y문법표지끝좌표 - 17f);

                        // 만약 두 줄 이상이면,

						if (_문법표지들[i].y문법표지끝좌표 - _문법표지들[i].y문법표지시작좌표 == 70) { _그래픽.DrawLine(_취소펜, 368, _문법표지들[i].y문법표지시작좌표 - 17f + 35f, _우한계, _문법표지들[i].y문법표지시작좌표 - 17f + 35f); }
			        }
		        }
	        }
        }
        private static void 변환_문자열을_여러줄로(string 문자열, int y좌표시작위치, ref List<한줄> 여러줄_처리결과)
        {
            문자열 = 문자열.Trim();	// 꼭 이 자리여야 함
            if(문자열 == "") return;

            List<string> 여러줄 = new List<string>();


            여러줄_처리결과.Clear();

	        string			현재줄;
	        string			현재어절;
            List<float> 줄의빈칸너비들 = new List<float>();
            List<string> 개행문자단위_문자열들 = new List<string>();

	        string			현재의개행문자단위문자열;
	        string			문법표지를포함한문자열 = 문자열;
	        string			문법표지를제거한문자열;
	
	        문법표지를제거한문자열 = 변환.문자열.문법문제표지제거(문법표지를포함한문자열);

            변환.문자열.개행문자로_구분한_문자열들로(문법표지를제거한문자열, ref 개행문자단위_문자열들);

            List<int> 어절이_몇번째문장인지_매긴_리스트 = new List<int>();

            강력하지만무거운변환.문자열.어절이_몇번째문장인지_매김(문법표지를제거한문자열, ref 어절이_몇번째문장인지_매긴_리스트);



	        for(int i = 0 ; i < 개행문자단위_문자열들.Count(); i++)
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

		        for(int j = 0 ; j < 어절들.Count(); j++)
		        {
			        현재어절 = 어절들[j];
			        현재어절의너비 = 현재어절의너비확인(현재어절);

			        새로운어절을더해봤을때의너비 += 현재어절의너비;

			        if(새로운어절을더해봤을때의너비 > 550)
			        {
				        여러줄.Add(현재줄);
				        if(현재줄의어절숫자 != 1)
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

	        for(int i = 0 ; i < 여러줄.Count() ; i++)
	        {
                List<string> 어절들 = new List<string>();


		        현재줄 = 여러줄[i];
                변환.문자열.어절들로(현재줄, ref 어절들);

                한줄 추가할한줄 = new 한줄();

                float x바로전글자끝좌표 = 0;

		        for(int j = 0 ; j < 어절들.Count(); j++)
		        {

                    어절 추가할어절 = new 어절();
                    추가할어절.내용 = 어절들[j];

                    if(j == 0)
			        {
                        추가할어절.x글자시작좌표 = 50;
                        추가할어절.x글자앞빈칸중간좌표 = 추가할어절.x글자시작좌표 - 5;
                        추가할어절.x글자끝좌표 = 추가할어절.x글자시작좌표 + 현재어절의너비확인(어절들[j]);
                        추가할어절.x글자뒷빈칸중간좌표 = 추가할어절.x글자시작좌표 + 현재어절의너비확인(어절들[j]) + 줄의빈칸너비들[i] / 2 ;
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
            Stack<int> 문법표지스택  = new Stack<int>();

            int	문법표지갯수 = -1;
	        float x문법표지시작좌표		= 0;
            float y문법표지시작좌표 = 0;
            float x문법표지끝좌표 = 0;
            float y문법표지끝좌표 = 0;
	        //int 현재괄호중첩갯수		= 0;

	

	        for(int i = 0 ; i < 문법표지를포함한문자열.Length; i++)
	        {
		        if(		변환.문자열.Mid(문법표지를포함한문자열,i,2) == "ⓢ{")	  {문법표지들.Add("ⓢ");  문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄);i += 1;	문법표지갯수++;	x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표);	x문법표지끝좌표들.Add(0); y문법표지끝좌표들.Add(0); if(문법표지스택.Count != 0){문법표지종류들[문법표지스택.Peek()] = "격자";  } 문법표지스택.Push(문법표지갯수); 문법표지종류들.Add("밑줄");}
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
                else if(변환.문자열.Mid(문법표지를포함한문자열,i,4) == "{어휘:")  {

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
                else if (변환.문자열.Mid(문법표지를포함한문자열, i, 2) == ":}")  { i += 1; }

                else if (변환.문자열.Mid(문법표지를포함한문자열, i, 1) == "{") { 문법표지들.Add(""); 문법표지의시작좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), ref x문법표지시작좌표, ref y문법표지시작좌표, ref 여러줄); i += 0; 문법표지갯수++; x문법표지시작좌표들.Add(x문법표지시작좌표); y문법표지시작좌표들.Add(y문법표지시작좌표); x문법표지끝좌표들.Add(x문법표지시작좌표); y문법표지끝좌표들.Add(y문법표지시작좌표); 문법표지종류들.Add(""); 문법표지스택.Push(문법표지갯수); }

                else if (변환.문자열.Mid(문법표지를포함한문자열, i, 1) == "}") {

					if (문법표지스택.Count != 0)
					{
						문법표지의끝좌표확인(변환.문자열.Left(문법표지를포함한문자열, i), 문법표지를포함한문자열.Substring(i + 1), ref x문법표지끝좌표, ref y문법표지끝좌표, ref 여러줄);

						x문법표지끝좌표들[문법표지스택.Peek()] = x문법표지끝좌표;
						y문법표지끝좌표들[문법표지스택.Peek()] = y문법표지끝좌표;
						문법표지스택.Pop();
					}
				}

	        }

	        if(문법표지갯수 == -1)
		        return;

	        //_문법표지수 += 문법표지갯수 + 1;

	        for(int i = 0 ; i <  문법표지갯수 + 1; i++)
	        {
                문법표지 현재문법표지 = new 문법표지();

                현재문법표지.문법표지문자열 =	문법표지들[i];
                현재문법표지.문법표지종류   =	문법표지종류들[i];
                현재문법표지.x문법표지시작좌표 = x문법표지시작좌표들[i];
                현재문법표지.y문법표지시작좌표 = y문법표지시작좌표들[i];
                현재문법표지.x문법표지끝좌표 = x문법표지끝좌표들[i];
                현재문법표지.y문법표지끝좌표 = y문법표지끝좌표들[i];

                _문법표지들.Add(현재문법표지);
	        }

	        // 문법표지 끝좌표의 격자가 겹치는 경우 조금 띄어주는 기능
	        for(int i = _문법표지수 ; i < _문법표지수 + 문법표지갯수 + 1; i++)
	        {
		        for(int j = _문법표지수 ; j < i; j++)
		        {
			        if(_문법표지들[i].문법표지종류 == "격자")
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
	        for(int i = _문법표지수 ; i < _문법표지수 + 문법표지갯수 + 1; i++)
	        {
		        for(int j = _문법표지수 ; j < i; j++)
		        {
			        if(_문법표지들[i].문법표지종류 == "격자")
			        {
                        if ((_문법표지들[i].x문법표지시작좌표 == _문법표지들[j].x문법표지시작좌표) && (_문법표지들[i].y문법표지시작좌표 == _문법표지들[j].y문법표지시작좌표))
					        _문법표지들[i].x문법표지시작좌표 -= 3.5f;
			        }
		        }
	        }

	        // 괄호와 관련한 기능
	        for(int i = _문법표지수 ; i < _문법표지수 + 문법표지갯수 + 1; i++)
	        {
		        for(int j = _문법표지수 ; j < i; j++)
		        {
			        if((_문법표지들[j].문법표지종류 == "밑줄") && (_문법표지들[i].문법표지종류 == "격자") && (_문법표지들[i].문법표지문자열 == "ⓧ"))
			        {
				        if((_문법표지들[j].y문법표지시작좌표 < _문법표지들[i].y문법표지시작좌표 ) || ((_문법표지들[j].y문법표지시작좌표 == _문법표지들[i].y문법표지시작좌표 ) && (_문법표지들[j].x문법표지시작좌표 < _문법표지들[i].x문법표지시작좌표) ))
				        {
					        if((_문법표지들[i].y문법표지시작좌표 < _문법표지들[j].y문법표지끝좌표 ) || ((_문법표지들[i].y문법표지시작좌표 == _문법표지들[j].y문법표지끝좌표 ) && (_문법표지들[i].x문법표지시작좌표 < _문법표지들[j].x문법표지끝좌표) ))
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
	        if(문법표지앞부분의문자열중표지제거한것 == "") // 여기는 맨 앞줄이라는 거임
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


		        for(int i = 0 ; i < _여러줄.Count; i++)
		        {
			        for(int j = 0 ; j < _여러줄[i]._어절들.Count; j++)
			        {
				        현재어절수++;

				        if(문법표지앞부분의어절들.Count() == 현재어절수)
				        {
					        if(j < _여러줄[i]._어절들.Count - 1) // 맨 마지막 어절이 아니라면
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
								while(_여러줄[i + 1 + 임시]._어절들.Count == 0)
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

		        if(문법표지앞부분의문자열중표지제거한것.LastIndexOf(' ') != -1)
			        어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것.Substring(문법표지앞부분의문자열중표지제거한것.LastIndexOf(' ') + 1);
		        else
			        어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것;

		

		        for(int i = 0 ; i < _여러줄.Count; i++)
		        {
			        for(int j = 0 ; j < _여러줄[i]._어절들.Count; j++)
			        {
				        현재어절수++;

				        if(문법표지앞부분의어절들.Count() == 현재어절수)
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

            if((변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 1) != " ") && (문법표지뒷부분의문자열중표지제거한것 != "") 
                && (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 1) != ".") && (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 1) != ",")
                && (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 2) != ", ") && (변환.문자열.Substring강력(문법표지뒷부분의문자열중표지제거한것, 0, 2) != ". "))
	        {
		        string 어포스트로피앞부분의글자들;

		        if(문법표지앞부분의문자열.LastIndexOf(' ') != -1)
			        어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것.Substring(문법표지앞부분의문자열중표지제거한것.LastIndexOf(' ') + 1);
		        else
			        어포스트로피앞부분의글자들 = 문법표지앞부분의문자열중표지제거한것;



		        for(int i = 0 ; i < _여러줄.Count; i++)
		        {
			        for(int j = 0 ; j < _여러줄[i]._어절들.Count; j++)
			        {
				        현재어절수++;

				        if(문법표지앞부분의어절들.Count() == 현재어절수)
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
		        for(int i = 0 ; i < _여러줄.Count; i++)
		        {
			        for(int j = 0 ; j < _여러줄[i]._어절들.Count; j++)
			        {
				        현재어절수++;

				        if(문법표지앞부분의어절들.Count() == 현재어절수)
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

	        for(int i = 0 ; i < 현재어절.Length; i++)
		        현재어절너비 += 개별코드에대한너비(현재어절[i]);

	        

	        return 현재어절너비;
        }
		private static float 개별코드에대한너비(char 코드)
        {
	        if(코드 == 'A')				return 13f;
	        else if(코드 == 'B')		return 12f;
	        else if(코드 == 'C')		return 12.5f;
	        else if(코드 == 'D') 		return 13f;
	        else if(코드 == 'E') 		return 10f;
	        else if(코드 == 'F') 		return 9.5f;
	        else if(코드 == 'G') 		return 13f;
	        else if(코드 == 'H') 		return 13.5f;
	        else if(코드 == 'I') 		return 5.5f;
	        else if(코드 == 'J') 		return 7.5f;
	        else if(코드 == 'K') 		return 11.5f;
	        else if(코드 == 'L') 		return 9.5f;
	        else if(코드 == 'M') 		return 17.5f;
	        else if(코드 == 'N') 		return 14.5f;
	        else if(코드 == 'O') 		return 15.5f;
	        else if(코드 == 'P') 		return 10.5f;
	        else if(코드 == 'Q') 		return 16f;
	        else if(코드 == 'R') 		return 12f;
	        else if(코드 == 'S') 		return 10.5f;
	        else if(코드 == 'T') 		return 11.5f;
	        else if(코드 == 'U') 		return 13.5f;
	        else if(코드 == 'V') 		return 14f;
	        else if(코드 == 'W') 		return 20.5f;
	        else if(코드 == 'X') 		return 13f;
	        else if(코드 == 'Y') 		return 12.5f;
	        else if(코드 == 'Z') 		return 12.5f;

	        else if(코드 == 'a') 		return 11f;
	        else if(코드 == 'b') 		return 11.5f;
	        else if(코드 == 'c') 		return 9.5f;
	        else if(코드 == 'd') 		return 12.5f;
	        else if(코드 == 'e') 		return 10.5f;
	        else if(코드 == 'f') 		return 8.5f;
	        else if(코드 == 'g') 		return 11.5f;
	        else if(코드 == 'h') 		return 11.5f;
	        else if(코드 == 'i') 		return 5.5f;
	        else if(코드 == 'j') 		return 6.5f;
	        else if(코드 == 'k') 		return 10.5f;
	        else if(코드 == 'l') 		return 5.5f;
	        else if(코드 == 'm') 		return 17f;
	        else if(코드 == 'n') 		return 11.5f;
	        else if(코드 == 'o') 		return 13f;
	        else if(코드 == 'p') 		return 12f;
	        else if(코드 == 'q') 		return 12f;
	        else if(코드 == 'r') 		return 8.5f;
	        else if(코드 == 's') 		return 9.5f;
	        else if(코드 == 't') 		return 8.5f;
	        else if(코드 == 'u') 		return 11.5f;
	        else if(코드 == 'v') 		return 11.5f;
	        else if(코드 == 'w') 		return 17f;
	        else if(코드 == 'x') 		return 11f;
	        else if(코드 == 'y') 		return 11f;
	        else if(코드 == 'z') 		return 12f;

	        else if(코드 == '1') 		return 12f;
	        else if(코드 == '2') 		return 10.5f;
	        else if(코드 == '3') 		return 11f;
	        else if(코드 == '4') 		return 11f;
	        else if(코드 == '5') 		return 10f;
	        else if(코드 == '6') 		return 12f;
	        else if(코드 == '7') 		return 11.5f;
	        else if(코드 == '8') 		return 12f;
	        else if(코드 == '9') 		return 12f;
	        else if(코드 == '0') 		return 11.5f;
	        else if(코드 == ' ') 		return 10f;
	        else if(코드 == '~') 		return 12f;
	        else if(코드 == '`') 		return 7f;
	        else if(코드 == '!') 		return 4.5f;
	        else if(코드 == '@') 		return 20f;
	        else if(코드 == '#') 		return 14f;
	        else if(코드 == '$') 		return 10f;
	        else if(코드 == '%') 		return 18.5f;
	        else if(코드 == '^') 		return 12f;
	        else if(코드 == '&') 		return 17f;
	        else if(코드 == '*') 		return 8.5f;
	        else if(코드 == '(') 		return 5.5f;
	        else if(코드 == ')') 		return 5.5f;
	        else if(코드 == '_') 		return 8.5f;
	        else if(코드 == '-') 		return 6.5f;
	        else if(코드 == '+') 		return 12f;
	        else if(코드 == '=') 		return 12f;
	        else if(코드 == '[') 		return 5f;
	        else if(코드 == ']') 		return 5f;
	        else if(코드 == '{') 		return 5.5f;
	        else if(코드 == '}') 		return 5.5f;
	        else if(코드 == '|') 		return 3.5f;
	        else if(코드 == '\\') 		return 17.5f;
	        else if(코드 == ':') 		return 5f;
	        else if(코드 == ';') 		return 5f;
	        else if(코드 == '\"') 		return 8f;
	        else if(코드 == '\'') 		return 5f;
	        else if(코드 == '’') 		return 5f;
	        else if(코드 == '<') 		return 11.5f;
	        else if(코드 == '>') 		return 11.5f;
	        else if(코드 == ',') 		return 5f;
	        else if(코드 == '.') 		return 5f;
	        else if(코드 == '.') 		return 5f;
	        else if(코드 == '?') 		return 10f;
	        else if(코드 == '/') 		return 10f;
	        else if (코드 == '“') 		return 6.5f;
	        else if(코드 == '‘') 		return 6.5f;

	        return 20f;
        }
        private static void 사진들넣기(ref Graphics _그래픽)
        {
            Image 그림파일 = Image.FromFile(_그림폴더 + "프로그램스킨/사진들.png");

            _그래픽.DrawImage(그림파일, new Rectangle(320, 0, 640, 640));

            그림파일.Dispose();

        }
        private static void 더러운문제경고하기(ref Graphics _그래픽, string Q)
        {
            if(Q.Contains("▼"))
            {
                Image 그림파일 = Image.FromFile(_그림폴더 + "프로그램스킨/DirtyDrumCan.png");

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
            Image 그림파일 = Image.FromFile(_그림폴더 + "프로그램스킨/엠블럼.png");

            _그래픽.DrawImage(그림파일, new Rectangle(320, 125, 300, 300));

            그림파일.Dispose();
        }

		private static Image _글자사각배경그림파일 = null;

		public static int _그림변환된높이 = 0;

		public static Bitmap _배경그림파일완성본_비트맵;
		public static Graphics _배경그림파일완성본_그래픽;
		public static void 배경그림_글자사각_넣기(string 지문, ref Graphics _그래픽)
        {
			#region 배경그림넣는부분
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


            if (Directory.Exists(_그림폴더))
            {
                DirectoryInfo dir = new DirectoryInfo(_그림폴더);
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

			            foreach(string 현재쉼표로나뉜것 in 현재그림파일이름을쉼표로나눈것들)
			            {

					        찾을말 = String.Format(" {0} ",현재쉼표로나뉜것);

				            if(!지문.Contains(찾을말))
				            {
					            현재쉼표로나뉜것이모두있는지여부 = false;
				            }
			            }

			            if(현재쉼표로나뉜것이모두있는지여부 == true)
			            {
				            현재그림이름의_적합도점수 += (현재그림파일이름을쉼표로나눈것들.Count() * 100);
				            현재그림이름의_적합도점수 += 현재그림파일이름.Length;
			            }
		            }
		            else
		            {
			            찾을말 = String.Format(" {0} ",현재그림파일이름);

                        int 포함한수 = 지문.포함갯수(찾을말);

                        if (포함한수 != 0)
			            {
				            현재그림이름의_적합도점수 += 1;
				            현재그림이름의_적합도점수 += 현재그림파일이름.Length;

				            if(현재그림파일이름.Contains(" "))
				            {
					            현재그림이름의_적합도점수 += 50;
				            }
							else if (현재그림파일이름 == "beautiful") 현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "building") 현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "finish")	현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "general") 현재그림이름의_적합도점수 = 3;
							else if (현재그림파일이름 == "website") 현재그림이름의_적합도점수 = 3;

                            현재그림이름의_적합도점수 *= 포함한수;

                        }
					}

		            if(여태까지그림중_최고적합도 < 현재그림이름의_적합도점수)
		            {
			            여태까지그림중_최고적합도 = 현재그림이름의_적합도점수;
			            선택된배경파일이름 = 현재그림파일이름;
                        _선택된배경파일이름 = 선택된배경파일이름;
		            }


                }
            }
            else
                Directory.CreateDirectory(_그림폴더);

	        string 그림경로 = String.Format("{0}{1}.jpg", _그림폴더, 선택된배경파일이름);

			if (_배경그림파일원본 != null) 	_배경그림파일원본.Dispose();
			//if (_배경그림파일완성본 != null) _배경그림파일완성본.Dispose();


			_배경그림파일원본 = Image.FromFile(그림경로);


			_배경그림파일완성본_비트맵 = new Bitmap(_가로픽셀, _세로픽셀);
			_배경그림파일완성본_그래픽 = Graphics.FromImage(_배경그림파일완성본_비트맵);

			//_배경그림파일완성본 = Image.FromHbitmap()

			//int 그림변환된높이 = (1280 * 그림파일.Height) / 그림파일.Width;
			_그림변환된높이 = (_가로픽셀 * _배경그림파일원본.Height) / _배경그림파일원본.Width;
			_배경그림파일완성본_그래픽.PixelOffsetMode = PixelOffsetMode.Half;

			_배경그림파일완성본_그래픽.DrawImage(_배경그림파일원본, new Rectangle(0, 0, _가로픽셀, _그림변환된높이), new Rectangle(1, 1, _배경그림파일원본.Width - 2, _배경그림파일원본.Height - 2), GraphicsUnit.Pixel);

			int 홀짝확인 = 0;

	        for(int i = 1; _그림변환된높이 * i < _세로픽셀 ; i++)
	        {
		        _배경그림파일원본.RotateFlip(RotateFlipType.RotateNoneFlipY);

				_배경그림파일완성본_그래픽.DrawImage(_배경그림파일원본, new Rectangle(0, (_그림변환된높이 - 1) * i , _가로픽셀, _그림변환된높이 ), new Rectangle(1, 1, _배경그림파일원본.Width - 2, _배경그림파일원본.Height - 2), GraphicsUnit.Pixel); // -1은 약간 줄여준다는 의미

				홀짝확인++;
	        }

			if(홀짝확인 % 2 ==1)
				_배경그림파일원본.RotateFlip(RotateFlipType.RotateNoneFlipY);
			#endregion

			#region 글자사각배경파일
			if (_글자사각배경그림파일 != null) _글자사각배경그림파일.Dispose();
			_글자사각배경그림파일 = Image.FromFile(_그림폴더 + "프로그램스킨/글자사각배경.png");

			_배경그림파일완성본_그래픽.DrawImage(_글자사각배경그림파일, new Rectangle(290, 0, 700, _세로픽셀), new Rectangle(0, 0, 700, _세로픽셀), GraphicsUnit.Pixel);
			#endregion

			_그래픽.DrawImage(_배경그림파일완성본_비트맵, 0, 0);
		}
		public static void 배경그림_글자사각_다시그리기(ref Graphics _그래픽)
		{
			_그래픽.PixelOffsetMode = PixelOffsetMode.Half;

			_그래픽.DrawImage(_배경그림파일완성본_비트맵, 0, 0);
		}


		~동영상용화면()
        {
			_배경그림파일원본.Dispose();
			_글자사각배경그림파일.Dispose();
		}
	}


}
