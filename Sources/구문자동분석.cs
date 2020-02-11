using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 변환;
using 편집기의_제왕;

namespace 구문분석
{
    public static class ExClass
    {
        public static string 중괄호붙이기(this String 현재내용, int 중괄호갯수)
        {
            if (현재내용 == "") return "";

            if (현재내용.EndsWith("."))
            {
                현재내용 = 현재내용.Substring(0, 현재내용.Length - 1);
                for (int i = 0; i < 중괄호갯수; i++) { 현재내용 += "}"; }
                현재내용 += ".";

                return 현재내용;
            }
            else if (현재내용.EndsWith(","))
            {
                현재내용 = 현재내용.Substring(0, 현재내용.Length - 1);
                for (int i = 0; i < 중괄호갯수; i++) { 현재내용 += "}"; }
                현재내용 += ",";

                return 현재내용;
            }
            else if (현재내용.EndsWith("!"))
            {
                현재내용 = 현재내용.Substring(0, 현재내용.Length - 1);
                for (int i = 0; i < 중괄호갯수; i++) { 현재내용 += "}"; }
                현재내용 += "!";

                return 현재내용;
            }
            else if (현재내용.EndsWith("?"))
            {
                현재내용 = 현재내용.Substring(0, 현재내용.Length - 1);
                for (int i = 0; i < 중괄호갯수; i++) { 현재내용 += "}"; }
                현재내용 += "?";

                return 현재내용;
            }
            else
            {
                for (int i = 0; i < 중괄호갯수; i++) { 현재내용 += "}"; }
                return 현재내용;
            }
        }
        public static string 현재내용에중괄호두개붙이기(this String 현재내용)
        {
            if (현재내용 == "") return "";

            if (변환.문자열.Right(현재내용, 1) == ".") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}."; }
            else if (변환.문자열.Right(현재내용, 1) == ",") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}},"; }
            else if (변환.문자열.Right(현재내용, 1) == "!") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}!"; }
            else if (변환.문자열.Right(현재내용, 1) == "?") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}?"; }
            else return 현재내용 + "}}";
        }
        private static string 현재내용에중괄호세개붙이기(this String 현재내용)
        {
            if (현재내용 == "") return "";

            if (변환.문자열.Right(현재내용, 1) == ".") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}}."; }
            else if (변환.문자열.Right(현재내용, 1) == ",") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}},"; }
            else if (변환.문자열.Right(현재내용, 1) == "!") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}}!"; }
            else if (변환.문자열.Right(현재내용, 1) == "?") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}}?"; }
            else return 현재내용 + "}}";
        }

    }

    public class 구문자동분석
    {
        #region 변수
        private bool _구문분석내_구문분석;
        public bool _구문분석내_to부정사;
        public string _내부첫접속사부사구분 = "";

        private int _i;

		private bool _RatherThan시작;
		private string _RatherThan저장소;
		private string _RatherThan만_저장하는_저장소;

        private bool _There;
        private bool _There주어시작;
        private bool _There서술어;

        private bool _ThereBe;
        private bool _ThereBe주어시작;

        private bool _조동사나옴;
        private bool _의문문;
        private bool _주어나옴;
        private bool _서술어나옴;
        private bool _1형식동사;
        private bool _2형식동사;
        private bool _자동사나옴;
        private bool _타동사나옴;
        private bool _목적어나옴;

        private bool _목적어구시작;
        private bool _보어구시작;
        private bool _조동사서술어시작;
        private bool _수여동사나옴;
        private bool _간접목적어나옴;
        private bool _직접목적어시작;
        private bool _HowAbout_WhatAbout시작;


        private bool _부사구_전치사구시작;
        private bool _havePP시작;

        private bool _to부정사부사적용법시작;
        private bool _to부정사형용사적용법시작;

        private bool _to부정사서술어나옴;
        private bool _to부정사자동사나옴;
        private bool _to부정사타동사나옴;
        private bool _to부정사목적어나옴;

        private bool _to부정사내조동사서술어시작;
        private bool _to부정사내보어구시작;
        private bool _to부정사내목적어나옴;
        private bool _to부정사내목적어구시작;
        private bool _to부정사내부사구_전치사구시작;

        private string _ToV내목적어임시저장소;

        private bool _현재진행형시작;

        private bool _to부정사보어구시작;
        private bool _to부정사목적어구시작;

        private bool _that절시작;
        private bool _that절이아님을확인;
        private int _that절시작추측지점i;

        private bool _전치사구동반한명사구시작;

        private int _중첩형용사구갯수 = 0;
        private int _전치사구동반한명사구단수여부;


        private string _주어임시저장소;

        private string _보어임시저장소;
        private string _목적어임시저장소;
        private string _처리결과기호;
        private string _that절안의내용;
        private string _관계사절내부;
        private bool _관계사절시작;

        private bool _분사구문시작;
        private bool _처리완료;
        private bool _처음;
        private bool _마지막;
        private bool _2마지막;
        private string _이전 = "";
		private string _이전소문자 = "";
		private string _2이전 = "";
        private string _현재 = "";
		private string _현재소문자 = "";
		private string _다음 = "";
		private string _다음소문자 = "";
        private string _2다음 = "";
        private string _3다음 = "";
        private string _4다음 = "";

        private string _이전어절_원본 = "";
        private string _현재B = "";
        private string _다음B = "";
        private string _다음2B = "";
        private string _다음3B = "";
        private string _다음4B = "";

		private string _2이전값 = "";
        private string _이전값 = "";
        private string _현재값 = "";
        private string _다음값 = "";
        private string _2다음검색값 = "";
        private string _3다음검색값 = "";
        private string _4다음검색값 = "";

        private string _이전어절모두 = "";
        private string _다음어절모두 = "";
        private string _다다음어절모두 = "";


        private string _결과;

        #endregion

        public 구문자동분석()
        {
            #region 변수 초기화
            _i = 0;
            _There = false;
            _There서술어 = false;
            _There주어시작 = false;

            _ThereBe = false;
            _ThereBe주어시작 = false;
            _의문문 = false;
            _주어임시저장소 = "";
            _보어임시저장소 = "";

            _목적어임시저장소 = "";
            _ToV내목적어임시저장소 = "";

            _결과 = "";
            _처리결과기호 = "";

            _분사구문시작 = false;
            _HowAbout_WhatAbout시작 = false;
            _현재진행형시작 = false;
            _주어나옴 = false;
            _서술어나옴 = false;
            _자동사나옴 = false;
            _타동사나옴 = false;
            _수여동사나옴 = false;
            _간접목적어나옴 = false;
            _직접목적어시작 = false;

            _목적어나옴 = false;
            _목적어구시작 = false;
            _보어구시작 = false;
            _조동사서술어시작 = false;
            _부사구_전치사구시작 = false;
            _havePP시작 = false;
            _to부정사부사적용법시작 = false; _to부정사서술어나옴 = false; _to부정사자동사나옴 = false; _to부정사타동사나옴 = false; _to부정사목적어나옴 = false;
            _to부정사보어구시작 = false; _to부정사목적어구시작 = false;
            _to부정사형용사적용법시작 = false; 
            _to부정사내조동사서술어시작 = false;            _to부정사내보어구시작 = false;            _to부정사내목적어나옴 = false;            _to부정사내목적어구시작 = false;
            _to부정사내부사구_전치사구시작 = false;

            _전치사구동반한명사구시작 = false;            _전치사구동반한명사구단수여부 = 0;




            _관계사절시작 = false;
            _that절시작 = false;
            _that절시작추측지점i = 0;
            _that절이아님을확인 = false;

            _처리완료 = false;
            _구문분석내_to부정사 = false;

            _내부첫접속사부사구분 = "";
            #endregion
        }

        public string 구문분석(string 문자열, ref string 구문분석로그)
        {
            #region 초기화

            string 초반처리결과 = 초반처리(문자열);				if (초반처리결과 != "") return 초반처리결과;

			string 초반사전확인 = Form1._검색.영한사전_문장부호제거(문자열);
			if (초반사전확인 != "" && !초반사전확인.Contains("int."))	return 문자열;		
			
			// 사전에 있는 경우 문장 분석을 하지 않는다. 예컨데 ⓢ{Niagara} ⓥ{Falls} 라고 하면 안된다.


            _i = 0;							_There = false;					_There서술어 = false;			_There주어시작 = false;				_ThereBe = false;					_ThereBe주어시작 = false;            _의문문 = false;
            _결과 = "";						_주어임시저장소 = "";			_보어임시저장소 = "";			_목적어임시저장소 = "";				_ToV내목적어임시저장소 = "";
            _분사구문시작 = false;			_HowAbout_WhatAbout시작=false;	_현재진행형시작 = false;		_주어나옴 = false;
            _서술어나옴 = false;			_자동사나옴 = false;			_타동사나옴 = false;			_수여동사나옴 = false;				_간접목적어나옴 = false;			_직접목적어시작 = false;
            _목적어나옴 = false;			_목적어구시작 = false;			_보어구시작 = false;            _부사구_전치사구시작 = false;		_havePP시작 = false;
            _to부정사부사적용법시작=false;	_to부정사서술어나옴 = false;	_to부정사자동사나옴 = false;	_to부정사타동사나옴 = false;		_to부정사목적어나옴 = false;		_to부정사보어구시작 = false;	_to부정사목적어구시작 = false;  _to부정사형용사적용법시작 = false;
            _to부정사내조동사서술어시작 = false;	_to부정사내보어구시작 = false;		_to부정사내목적어나옴 = false;	_to부정사내목적어구시작 = false;	_to부정사내부사구_전치사구시작 = false;
            _전치사구동반한명사구시작 = false;		_전치사구동반한명사구단수여부 = 0;	_조동사서술어시작 = false;		_관계사절시작 = false;				_that절시작 = false;	_중첩형용사구갯수 = 0;

            문자열 = 하나로_묶을_관용어구_전처리(문자열);	문자열 = 문자열.Replace("’", "'");		문자열 = 문자열.Trim();
            if (변환.문자열.Right(문자열, 1) == "?")		_의문문 = true;
            List<string> W = new List<string>();			변환.문자열.어절들로(문자열, ref W);   _처음 = false;	_마지막 = false;	_2마지막 = false;	_결과 = "";		_처리결과기호 = "시작";		_처리완료 = false;
            #endregion

            for (_i = 0; _i < W.Count; _i++)
            {
                if (_처리완료) break; // 내부의 함수에서 이 부분을 한 번 더 부르는데, _i를 멤버로 쓰기 때문에 여러번 루프가 돌 수 있다.
                #region 어절 세팅

                _현재 = W[_i];	        _현재B = W[_i];
				_현재소문자 = _현재.ToLower().불필요제거(); 

                _이전어절모두 = "";		for (int k = 0; k < _i; k++) _이전어절모두 += W[k] + " ";               _이전어절모두 = _이전어절모두.Trim();
                _다음어절모두 = "";		for (int j = _i + 1; j < W.Count; j++) _다음어절모두 += W[j] + " ";		_다음어절모두 = _다음어절모두.Trim();
                _다다음어절모두 = "";	for (int j = _i + 2; j < W.Count; j++) _다다음어절모두 += W[j] + " ";	_다다음어절모두 = _다다음어절모두.Trim();

                if (_i == 0)
                {
                    if (_구문분석내_구문분석 == false)	_처음 = true;	else _처음 = false;

                    if (_i == W.Count - 1){		_다음 = "";						_2다음 = "";					_3다음 = "";					_4다음 = "";					_다음B = "";		_다음2B = "";		_다음3B = "";		_다음4B = ""; _마지막 = true;	}
                    else if (_i == W.Count - 2){_다음 = W[_i + 1].불필요제거(); _2다음 = "";					_3다음 = "";					_4다음 = "";					_다음B = W[_i + 1]; _다음2B = "";		_다음3B = "";		_다음4B = "";					}
                    else if (_i == W.Count - 3){_다음 = W[_i + 1].불필요제거(); _2다음 = W[_i + 2].불필요제거();_3다음 = "";					_4다음 = "";					_다음B = W[_i + 1]; _다음2B = W[_i + 2];_다음3B = "";		_다음4B = "";					}
                    else if (_i == W.Count - 4){_다음 = W[_i + 1].불필요제거(); _2다음 = W[_i + 2].불필요제거();_3다음 = W[_i + 3].불필요제거();_4다음 = "";					_다음B = W[_i + 1]; _다음2B = W[_i + 2];_다음3B = W[_i + 3];_다음4B = "";					}
                    else {						_다음 = W[_i + 1].불필요제거(); _2다음 = W[_i + 2].불필요제거();_3다음 = W[_i + 3].불필요제거();_4다음 = W[_i + 4].불필요제거();_다음B = W[_i + 1]; _다음2B = W[_i + 2];_다음3B = W[_i + 3];_다음4B = W[_i + 4];			}
                }
				else if(_i == 1)
				{
					if (_i == W.Count - 1) {		_처음=false;	_마지막=true;	_이전=W[_i-1];	_다음="";					_2다음="";					_3다음="";						_4다음="";						_다음B="";			_다음2B="";			_다음3B = "";		_다음4B = "";		}
					else if (_i == W.Count - 2) {	_2마지막=true;	_처음=false;	_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음="";					_3다음="";						_4다음="";						_다음B=W[_i + 1];	_다음2B="";			_다음3B = "";		_다음4B = "";		}
					else if (_i == W.Count - 3) {	_처음=false;					_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음=W[_i+2].불필요제거();_3다음="";						_4다음="";						_다음B=W[_i + 1];	_다음2B=W[_i + 2];	_다음3B = "";		_다음4B = "";		}
					else if (_i == W.Count - 4) {	_처음=false;					_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음=W[_i+2].불필요제거();_3다음=W[_i + 3].불필요제거();	_4다음="";						_다음B=W[_i + 1];	_다음2B=W[_i + 2];	_다음3B = W[_i + 3];_다음4B = "";		}
					else {							_처음=false;					_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음=W[_i+2].불필요제거();_3다음=W[_i + 3].불필요제거();	_4다음=W[_i + 4].불필요제거();	_다음B=W[_i + 1];	_다음2B=W[_i + 2];	_다음3B = W[_i + 3];_다음4B = W[_i + 4];}
				}
                else if (_i == W.Count - 1) {	_처음=false;	_마지막=true;	_2이전 = W[_i-2];		_이전 =W[_i-1];		_다음="";					_2다음="";					_3다음="";						_4다음="";						_다음B="";			_다음2B="";			_다음3B = "";		_다음4B = "";		}
                else if (_i == W.Count - 2) {	_2마지막=true;	_처음=false;	_2이전 = W[_i-2];		_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음="";					_3다음="";						_4다음="";						_다음B=W[_i + 1];	_다음2B="";			_다음3B = "";		_다음4B = "";		}
                else if (_i == W.Count - 3) {	_처음=false;					_2이전 = W[_i-2];		_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음=W[_i+2].불필요제거();_3다음="";						_4다음="";						_다음B=W[_i + 1];	_다음2B=W[_i + 2];	_다음3B = "";		_다음4B = "";		}
                else if (_i == W.Count - 4) {	_처음=false;					_2이전 = W[_i-2];		_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음=W[_i+2].불필요제거();_3다음=W[_i + 3].불필요제거();	_4다음="";						_다음B=W[_i + 1];	_다음2B=W[_i + 2];	_다음3B = W[_i + 3];_다음4B = "";		}
                else {							_처음=false;					_2이전 = W[_i-2];		_이전=W[_i-1];	_다음=W[_i+1].불필요제거();	_2다음=W[_i+2].불필요제거();_3다음=W[_i + 3].불필요제거();	_4다음=W[_i + 4].불필요제거();	_다음B=W[_i + 1];	_다음2B=W[_i + 2];	_다음3B = W[_i + 3];_다음4B = W[_i + 4];}
                #endregion
				
                _2이전값 = Form1._검색.영한사전_문장부호제거(_2이전);		_이전값 = Form1._검색.영한사전_문장부호제거(_이전);		_현재값 = Form1._검색.영한사전_문장부호제거(_현재);		_다음값 = Form1._검색.영한사전_문장부호제거(_다음);		_2다음검색값 = Form1._검색.영한사전_문장부호제거(_2다음);		_3다음검색값 = Form1._검색.영한사전_문장부호제거(_3다음);	_4다음검색값 = Form1._검색.영한사전_문장부호제거(_4다음);
				_다음소문자 = _다음.ToLower().불필요제거();
				_이전소문자 = _이전.ToLower().불필요제거();

				// 이 부분이 목적어 임시 저장소보다 뒤로 가면 안됨
				if (_ToV내목적어임시저장소 != "")	_ToV내목적어임시저장소 += " ";		else if (_목적어임시저장소 != "")	_목적어임시저장소 += " ";			else if (_보어임시저장소 != "")		_보어임시저장소 += " ";			else _결과 += " ";

				if(RatherThan시작()) {; }			else if(RatherThan종료()) {; }		else if(RatherThan중간()) {; }		
				else if (ThereBe(_현재)) {}			else if (ThereBe주어확정()) {; }	else if (ThereBe주어시작()) {; }	else if (ThereBe주어종료()) {; }	else if (ThereBe주어중간()) {; }	else if (There()) {; }			else if (There서술어()) {; }
				else if (접속사that절시작()) {}		else if (접속사that절종료()) {; }	else if (접속사that절내부()) {; }
                else if (의문문Be동사(_현재)) {}	else if (의문문조동사(_현재)) {; }	else if (문두독립부사()) {; }		else if (의문부사()) {; }			else if (의문형용사()) {; }			else if (의문대명사()) {; }
                else if (감탄사_단일부사_어구()) {}	else if (Today_Yesterday()) {; }	else if (분사구문시작()) {; }		else if (분사구문종료()) {; }		else if (NiceToMeetYou구문()) {; }	else if (WH_About시작()) {; }
                else if (관계사절시작()) {}			else if (관계사절_내or종료()) {; }

                else if (접속사확정가능성()) {}		else if (전치사구시작가능성()) { }	/* 목적어, 보어 시작 앞*/
                else if (주어확정가능성()) {}			/* 확정되면 바로 주어처리.*/		else if (S_Be동사()) {; }			else if (S_ll확정()) {; }			else if (주어VE확정가능성()) {;}
                else if (명령어수여동사출현()) { }	else if (명령어서술어확정()) {; }
                else if (ShouldHavePP형태()) { }	/* 조동사 서술어 시작보다 먼저 */	else if (조동사서술어시작()) { }	else if (조동사내beAbleTo()) { }/* 동사확정 가능성보다 먼저*/		else if (조동사서술어끝()) { }		else if (조동사서술어내_부사(_현재)) { }
                else if (주어있고_havePP시작()) {}	else if (S있고_현재진행형시작()){;}	else if (현재진행형종료()) {;}		else if (S있고_감각동사확정()) {; }	else if (S있고_수여동사()) {; }		else if (주어있고_서술어확정()) {;}
                else if (ToV목적어구시작()) {}		else if (ToV보어구시작()) {; }		else if (ToV형용사시작()) {; }		else if (ToV내서술어()) {; }		/* ToV3목적어끝보다 먼저 */
                else if (ToV부사적용법시작()) {}	else if (ToV내목적어()){;}			/* 주어의 확정과 같음 */			else if (ToV부사끝()) {; }			else if (ToV내목적어시작()) { }
                else if (S미정_havePP시작()) {}		else if (havePP종료(_현재)) {; }	else if (S미정_BeGoingTo()) {; }	else if (S미정_현재진행()) {; }		else if (S미정_Ved()) {; }			else if (S미정_Vs()) {; }		else if (S미정_Be동사()) {; }		else if (S미정복수_기본동사()) {; }

                else if (S미정_부사()) { }          else if (간접목적어확정가능성()) {} else if (직접목적어확정가능성()) {} else if (직접목적어시작가능성()) {} else if (직접목적어종료가능성()) {} else if (목적어확정가능성()) {; }
                else if (보어확정가능성()) {; }
                else if (부사종료확정가능성()) { }

                else if (서술어부근의_부사()) { } // 목적어 시작, 보어 시작 가능성 뒤에 나와야 합니다.

                else if (목적어시작가능성()) { }    else if (목적어종료가능성()) { }

                else if (보어시작가능성()) { } // 목적어 시작 가능성 뒤에 나와야 합니다.
                else if (보어종료가능성()) { }

                else if (부사구_전치사구종료가능성()) { }
                else if (이름종료가능성()) { }

                else if (to부정사내목적어종료가능성()) { }
                else if (to부정사목적어구종료()) {; }
                else if (to부정사보어구종료()) {; }

                else if (주어가아직안나왔을때주어후보로만들기()) { }// 함수 내에서 주어후보에 내용을 추가하였습니다.
                else if (주어가아직안나왔는데끝까지간경우()) { }
                else if (수식어시작_확정가능성()) { }
                else if (_ToV내목적어임시저장소 != "") { _ToV내목적어임시저장소 += _현재; } // _목적어임시저장소보다 앞에 나와야 합니다.
                else if (_보어임시저장소 != "") { _보어임시저장소 += _현재; }
                else if (_목적어임시저장소 != "") { _목적어임시저장소 += _현재; }
				else if(부사종료확정가능성_마지막()) { }
                else
                {
                    _결과 += _현재;
                    _처리결과기호 = "처리안함";

                }
            }

            while (_결과.Contains("  ")) _결과 = _결과.Replace("  ", " ");

            _결과 = _결과.Trim();

            _결과 = 하나로_묶은_관용어구_후처리(_결과);

            return _결과;

        }

        public string 동명사구_구문분석(string 문자열, ref string 구문분석로그)
		{
            구문자동분석 동명사구 = new 구문자동분석();

			string 문법표지제거문자열 = 문자열.문법표지제거();
			string 문법표지제거문자열_첫번째어절 =  문법표지제거문자열.첫번째어절();

			if(!Form1._검색.영한사전_문장부호제거(문법표지제거문자열_첫번째어절).Contains("ing형"))
			{
				return 문자열;
			}

            string 동명사구분석결과 = 동명사구.구문분석(문법표지제거문자열, ref 구문분석로그);

			if(동명사구분석결과.StartsWith("ⓧ{") && 동명사구분석결과.EndsWith("}"))
			{
				return 동명사구분석결과.Substring(2, 동명사구분석결과.Length - 3);
			}
			else
				return 동명사구분석결과;
		}

        public string 초반처리(string 문자열)
        {
            string 문자열원본 = 문자열;
            문자열 = 문자열.불필요제거();

            if (문자열.ToLower() == "yes") { _결과 = "ⓧ{" + 문자열원본.중괄호붙이기(1); return _결과; }
            if (문자열.ToLower() == "no") { _결과 = "ⓧ{" + 문자열원본.중괄호붙이기(1); return _결과; }
            if (문자열.ToLower() == "hi") { _결과 = "ⓧ{" + 문자열원본.중괄호붙이기(1); return _결과; }

            return "";
        }
        public string 구문분석(string 문자열, bool 구문분석안의_구문분석, ref string 구문분석로그)
        {
            _구문분석내_구문분석 = 구문분석안의_구문분석;
            return 구문분석(문자열, ref 구문분석로그);
        }

        public string 하나로_묶을_관용어구_전처리(string 문자열)
        {
            문자열 = 문자열.Replace("once in a life", "once_in_a_life");

            return 문자열;

        }

        public string 하나로_묶은_관용어구_후처리(string 문자열)
        {
            문자열 = 문자열.Replace("_", " ");

            return 문자열;

        }

		private bool RatherThan시작()
		{
			if(_RatherThan시작) return false; // 시작한 걸 또 시작할 수 없다.

			if(_현재소문자 == "rather" && _다음.ToLower() == "than")
			{
				_RatherThan시작 = true;

				_RatherThan만_저장하는_저장소 = _현재 + " " + _다음;
				return true;
			}

			return false;
		}

		private bool RatherThan중간()
		{
			if(_RatherThan시작)
			{
				if(_이전.ToLower() == "rather" && _현재소문자 == "than")
				{
					;
				}
				else
				{
					if(!String.IsNullOrEmpty(_RatherThan저장소))
						_RatherThan저장소 += " " + _현재;
					else
						_RatherThan저장소 += _현재;
				}

				return true;
			}

			return false;
		}

		private bool RatherThan종료()
		{
			string RatherThan종료_구문분석로그 = "";

			if(_RatherThan시작 && _현재.EndsWith(","))
			{
				_RatherThan저장소 += " " + _현재;

				구문자동분석 RatherThan구문분석 = new 구문자동분석();
				_결과 += "ⓧ{" + _RatherThan만_저장하는_저장소 + " " + RatherThan구문분석.구문분석(_RatherThan저장소.Substring(0, _RatherThan저장소.Length - 1), ref RatherThan종료_구문분석로그) + "},";

				_RatherThan저장소 = "";
				_RatherThan시작 = false;
				return true;
			}

			return false;
		}

        private bool 문두독립부사()
        {
            if (_처리결과기호 != "시작") return false;

            if (_현재.Contains(",") && 부사_다른품사는없는(_현재))
            {
                _결과 = "ⓧ{" + _현재.중괄호붙이기(1);
                //_처리결과기호 = "문두에독립적으로나온부사"; // "시작"으로 두는 게 오히려 좋다.
                return true;
            }
            else if (부사_다른품사는없는(_현재) && 관사(_다음))
            {
                _결과 = "ⓧ{" + _현재.중괄호붙이기(1);
                //_처리결과기호 = "문두에독립적으로나온부사"; // "시작"으로 두는 게 오히려 좋다.
                return true;
            }
            else if (_현재소문자 == "then")
            {
                _결과 = "ⓧ{" + _현재.중괄호붙이기(1);
                //_처리결과기호 = "문두에독립적으로나온부사"; // "시작"으로 두는 게 오히려 좋다.
                return true;
            }

            return false;
        }

        private bool 과거형동사가확실한것(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            현재어절 = 변환.문자열.문장부호제거(현재어절);

            if (현재어절 == "showed") return true;
            if (현재어절 == "taught") return true;

            return false;
        }
        private bool 소유대명사도되고_형용사도되는것(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            현재어절 = 변환.문자열.문장부호제거(현재어절);

            if (현재어절 == "these") return true;
            if (현재어절 == "this") return true;
            if (현재어절 == "that") return true;
            if (현재어절 == "those") return true;

            return false;
        }
        private bool 소유대명사(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            현재어절 = 변환.문자열.문장부호제거(현재어절);

            if (현재어절 == "my") return true;
            if (현재어절 == "your") return true;
            if (현재어절 == "his") return true;
            if (현재어절 == "her") return true;
            if (현재어절 == "their") return true;
            if (현재어절 == "our") return true;

            return false;
        }
        public bool 관사(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            현재어절 = 변환.문자열.문장부호제거(현재어절);

            if (현재어절 == "a") return true;
            if (현재어절 == "an") return true;
            if (현재어절 == "the") return true;

            return false;
        }
        private bool 관계사절시작()
        {

            // 주격 관계 대명사 시작
            if (!선행사있는주격관계사(_현재)) return false;

            if (_이전 == "") return false;



            if (서술어가능성(_다음) || 조동사(_다음))
            {
                _관계사절시작 = true;
                _관계사절내부 += _현재;
                _처리결과기호 = "관계사절시작";
                return true;
            }

            return false;
        }
        private bool 관계사절_내or종료()
        {
			string 관계사절_내or종료_구문분석로그 = "";

			if (!_관계사절시작) return false;

            _처리결과기호 = "관계사절내부_혹은_종료";
            if (_마지막 == true)
            {
                _관계사절내부 += " " + _현재;

                if (_주어임시저장소 != "")
                {
                    _결과 += _주어임시저장소 + " ";

                    _주어임시저장소 = "";
                }
                구문자동분석 임시 = new 구문자동분석();


                _결과 += "ⓧ{" + 현재내용에중괄호붙이기(임시.구문분석(_관계사절내부, ref 관계사절_내or종료_구문분석로그));

                return true;
            }
            else if (다음어절_서술어가능성(_이전어절모두, _현재, _다음, _2다음) || 조동사(_다음))
            {
                _관계사절내부 += " " + _현재;



                if (완결된문장(_관계사절내부))
                {
                    if (_주어임시저장소 != "")
                    {
                        _결과 += "ⓢ{" + _주어임시저장소 + " ";

                        _주어임시저장소 = "";

                        구문자동분석 임시 = new 구문자동분석();



                        if (!_전치사구동반한명사구시작)
                        {
                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;

                            _결과 += "ⓧ{" + 현재내용에중괄호두개붙이기(임시.구문분석(_관계사절내부, ref 관계사절_내or종료_구문분석로그));
                        }
                        else
                        {
                            _결과 += "ⓧ{" + 현재내용에중괄호세개붙이기(임시.구문분석(_관계사절내부, ref 관계사절_내or종료_구문분석로그));
                        }

                        _주어나옴 = true;

                    }
                    else if (_부사구_전치사구시작 == true)
                    {
                        구문자동분석 임시 = new 구문자동분석();

                        _결과 += "ⓧ{" + 현재내용에중괄호두개붙이기(임시.구문분석(_관계사절내부, ref 관계사절_내or종료_구문분석로그));
                        _부사구_전치사구시작 = false;

                    }
                    else
                    {
                        구문자동분석 임시 = new 구문자동분석();

                        _결과 += "ⓧ{" + 현재내용에중괄호붙이기(임시.구문분석(_관계사절내부, ref 관계사절_내or종료_구문분석로그));
                    }


                    _관계사절시작 = false;
                    _관계사절내부 = "";
                }


                return true;
            }
            else
            {
                _관계사절내부 += " " + _현재;
                return true;
            }

        }
        private bool 완결된문장(string 문장)
        {
			string 확인용으로_로그불필요함 = "";
            구문자동분석 임시 = new 구문자동분석();
            string 분석결과 = 임시.구문분석(문장, ref 확인용으로_로그불필요함);

            if (분석결과.Contains("ⓢ") && 분석결과.Contains("ⓥ")) return true;

            return false;
        }
        private bool 선행사있는주격관계사(string 현재어절)
        {
            string 현재어절소문자 = 현재어절.ToLower();

            if (현재어절 == "which") return true;
            if (현재어절 == "who") return true;
            if (현재어절 == "that") return true;

            return false;

        }
        private bool 접속사that절시작()
        {
            if (_that절시작) return false; // 일단 하나만 중첩되게 하자.
            if (_that절이아님을확인) return false;
            if (_마지막) return false;

            if (접속사that앞에쓰이는말들(_이전) && (_현재 == "that"))
            {
                _that절시작 = true;

                if (_주어임시저장소 != "")
                {
                    _결과 += _주어임시저장소 + " ⓞ{㉨{" + _현재 + "}";
                    _주어임시저장소 = "";
                }
                else
                    _결과 += "ⓞ{㉨{" + _현재 + "}";

                _that절안의내용 = "";

                return true;
            }
            // that이 없지만 일단 that절이라고 봅니다.
            else if (접속사that앞에쓰이는말들(_이전))
            {
                _that절시작추측지점i = _i;

                _that절시작 = true;

                if (_주어임시저장소 != "")
                {
                    _결과 += _주어임시저장소 + " ⓞ{";
                    _주어임시저장소 = "";
                }
                else
                    _결과 += "ⓞ{";

                _that절안의내용 = _현재;

                return true;
            }
            return false;
        }
        private bool 접속사that앞에쓰이는말들(string 현재어절)
        {

			현재어절 = 현재어절.ToLower();
            //if (현재어절 == "") return true;

            if (현재어절 == "argue") return true;
            if (현재어절 == "argues") return true;
            if (현재어절 == "argued") return true;

            if (현재어절 == "believe") return true;
            if (현재어절 == "believes") return true;
            if (현재어절 == "believed") return true;

            if (현재어절 == "decide") return true;
            if (현재어절 == "decides") return true;
            if (현재어절 == "decided") return true;

            if (현재어절 == "discover") return true;
            if (현재어절 == "discovers") return true;
            if (현재어절 == "discovered") return true;

            if (현재어절 == "find") return true;
            if (현재어절 == "finds") return true;
            if (현재어절 == "found") return true;

			if (현재어절 == "imagine") return true;

			if (현재어절 == "imply") return true;
            if (현재어절 == "implied") return true;
            if (현재어절 == "implies") return true;



            if (현재어절 == "know") return true;
            if (현재어절 == "knew") return true;
            if (현재어절 == "knows") return true;
            if (현재어절 == "known") return true;

            if (현재어절 == "presume") return true;
            if (현재어절 == "presumes") return true;
            if (현재어절 == "presumed") return true;


            if (현재어절 == "say") return true;
            if (현재어절 == "says") return true;
            if (현재어절 == "said") return true;

            if (현재어절 == "show") return true;
            if (현재어절 == "shows") return true;
            if (현재어절 == "shown") return true;

            if (현재어절 == "speculate") return true;
            if (현재어절 == "speculates") return true;
            if (현재어절 == "speculated") return true;

			if (현재어절 == "suppose") return true;

			return false;
        }
        private bool 접속사that절내부()
        {
            if (!_that절시작) return false;

			if(_that절안의내용 == "")
	            _that절안의내용 = _현재;
			else
	            _that절안의내용 += " " + _현재;


            return true;

        }
        // 고치던 중
        private bool 접속사that절종료()
        {
			string that절내부로그 = "";

            if (!_that절시작) return false;
            if (!_마지막) return false;

			if(_that절안의내용 == "")
	            _that절안의내용 = _현재;
			else
	            _that절안의내용 += " " + _현재;

            string 현재까지처리결과 = _결과;

            구문자동분석 임시 = new 구문자동분석();

            string that절내부처리결과 = 임시.구문분석(_that절안의내용.Trim(), ref that절내부로그);

            if (!완결된문장(that절내부처리결과))
            {
                _i = _that절시작추측지점i;

                _that절이아님을확인 = true;
            }

            _결과 = _결과.Trim() + that절내부처리결과.중괄호붙이기(1);

            _처리완료 = true;
            return true;
        }
        private bool 이름종료가능성()
        {
            if (!_마지막) return false;

            string 현재검색결과 = Form1._검색.영한사전_문장부호제거(_현재);
            _처리결과기호 = "이름종료가능성";
            return false;
        }



        #region There
        private bool ThereBe주어중간()
        {
            if (!_ThereBe && !_There서술어) return false;
            if (!_ThereBe주어시작) return false; // 시작을 해야 중간이 있음

            _결과 += _현재;
            _처리결과기호 = "ThereBe의주어중간";

            return true;
        }

		// 마지막인 경우 그냥 종료
        private bool ThereBe주어종료()
        {
            if (!_ThereBe && !_There서술어) return false;

			if(전치사_to와of빼고(_다음))
			{
				_결과 += 현재내용에중괄호붙이기(_현재);
				_처리결과기호 = "ThereBe의주어종료";

				_ThereBe = false; // 초기화함 there 구문은 끝난 것이나 다름없다.
				_There서술어 = false;
				return true;
			}

			if (!_마지막) return false;

            _결과 += 현재내용에중괄호붙이기(_현재);
            _처리결과기호 = "ThereBe의주어종료";

            return true;
        }
        private bool ThereBe주어시작()
        {
            if (!_ThereBe && !_There서술어) return false;
            if (_ThereBe주어시작) return false; // 이미 시작했으면 또 시작할 수 없음

			
			_결과 += "ⓢ{" + _현재;

            _ThereBe주어시작 = true;
            _처리결과기호 = "ThereBe주어시작";

            return true;
        }

		// 단어 하나만 달랑 남은 경우
        private bool ThereBe주어확정()
        {
            if (!_ThereBe && !_There서술어) return false;
            if (_ThereBe주어시작) return false; // 이미 시작했으면 확정이 말이 안됨.
            if (!_마지막) return false;

            _결과 += "ⓢ{" + _현재 + "}";
            _처리결과기호 = "ThereBe의주어확정";

            return true;
        }
        private bool There서술어()
        {
            if (!_There) return false;

            if (Be동사(_현재))
            {
                _There서술어 = true;

                _결과 += "ⓥ{" + _현재 + "}";
                _처리결과기호 = "There서술어";

                return true;
            }

            return false;
        }
        private bool There()
        {

            if (_처리결과기호 != "시작") return false;

            if (_현재 == "There")
            {
                _결과 += "ⓧ{There}";

                _There = true;
                _처리결과기호 = "There";

                return true;
            }

            if (_현재 == "there")
            {
                _결과 += "ⓧ{there}";

                _There = true;
                _처리결과기호 = "There";

                return true;
            }

            if (_현재 == "Here")
            {
                _결과 += "ⓧ{Here}";

                _There = true;
                _처리결과기호 = "There";

                return true;
            }

            if (_현재 == "here")
            {
                _결과 += "ⓧ{here}";

                _There = true;
                _처리결과기호 = "There";

                return true;
            }

            return false;
        }
        private bool ThereBe(string 현재어절)
        {
            if (_처리결과기호 != "시작") return false;

            if (현재어절 == "There's")
            {
                _결과 += "ⓧ{There}ⓥ{'s}";

                _ThereBe = true;
                _처리결과기호 = "ThereBe";

                return true;
            }

            if (현재어절 == "Here's")
            {
                _결과 += "ⓧ{Here}ⓥ{'s}";

                _ThereBe = true;
                _처리결과기호 = "ThereBe";

                return true;
            }

            return false;
        }
        #endregion
        private bool 의문문Be동사(string 현재어절)
        {
            if (Be동사(현재어절) && _의문문)
            {
                _결과 += "ⓥ{" + 현재어절 + "}";

                _서술어나옴 = true;
                _자동사나옴 = true;

                _처리결과기호 = "의문문Be동사";

                return true;
            }

            return false;
        }
        private bool 의문문조동사(string 현재어절)
        {
            if (조동사(현재어절) && _의문문 && _처리결과기호 == "시작")
            {
                _결과 += "ⓧ{" + 현재어절 + "}";

                _처리결과기호 = "의문문조동사";

                return true;
            }
            return false;
        }
        public bool 조동사(string 현재어절)
        {
            현재어절 = 변환.문자열.문장부호제거(현재어절);


            현재어절 = 현재어절.ToLower();

            if ((현재어절 == "do")
                || (현재어절 == "don't")
                || (현재어절 == "doesn't")
                || (현재어절 == "did")
                || (현재어절 == "didn't")
                || (현재어절 == "can")
                || (현재어절 == "cannot")
                || (현재어절 == "can't")
                || (현재어절 == "could")
                || (현재어절 == "must")
                || (현재어절 == "shall")
                || (현재어절 == "should")
                || (현재어절 == "will")

                || (현재어절 == "would")
                || (현재어절 == "may")
                || (현재어절 == "might"))
                return true;

            return false;
        }
        private bool 분사구문시작()
        {
			if (관사(_이전)) return false; // the following is... 여기에서 following이 분사구문일 수가 없다.

            if (_분사구문시작 == true) return false; // 시작했는데 또 시작할 순 없지

			string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

			if (!현재어절검색결과.Contains("ing형")) return false; // 일단 ing 형이어야 분사구문 흉내라도 낸다. 분사구문의 부정은 아직 안 다룸

			if ((_현재 == "closing") && (_다음 == "time")) return false;

			if (_처리결과기호 == "시작")
			{

				// 다음어절 모두에 s형은 없어야 합니다.
				string[] 다음어절들 = _다음어절모두.Split(' ');

				for (int i = 0; i < 다음어절들.Count(); i++)
				{
					string 현재어절_다음어절들 = 다음어절들[i];
					string 다음어절_다음어절들 = "";
					string 다다음어절_다음어절들 = "";

					if (i < 다음어절들.Count() - 1) 다음어절_다음어절들 = 다음어절들[i + 1];
					if (i < 다음어절들.Count() - 2) 다다음어절_다음어절들 = 다음어절들[i + 2];



					string 현재어절_다음어절_검색결과 = Form1._검색.영한사전_문장부호제거(현재어절_다음어절들);
					string 다음어절_다음어절_검색결과 = Form1._검색.영한사전_문장부호제거(다음어절_다음어절들);



					if (현재어절_다음어절_검색결과.Contains("s형"))
						return false;

					if (조동사(현재어절_다음어절들) && 동사원형(다음어절_다음어절들))
						return false;

					if (조동사(현재어절_다음어절들) && 접속어AndOr(다음어절_다음어절들) && 조동사(다다음어절_다음어절들))
						return false;

				}

				if (이어동사용부사(_다음))
				{
					_결과 += "ⓧ{ⓥ{" + _현재 + " " + _다음 + "}";
					_i++; // 다음어절 처리를 건너 뜁니다.
				}
				else
					_결과 += "ⓧ{ⓥ{" + _현재 + "}";

				_분사구문시작 = true;
				_처리결과기호 = "분사구문시작";

				if (현재어절검색결과.Contains("vi.")) _자동사나옴 = true;
				if (현재어절검색결과.Contains("vt.")) _타동사나옴 = true;

				_서술어나옴 = true;


				return true;
			}
			// 자! 중간에 ing가 나왔다. 그렇다면, 동명사거나, 분사구문이거나, 분사가 앞의 명사를 수식하는 것이거나, 5형식에서 목적보어가 ing인 경우이다.
			// be + ing도 있다.
			// 동명사라고 치면, 앞이 단어가 동명사를 목적어로 하는 타동사일 것이다.
			else
			{
				// 1. 동명사이거나
				if(Form1._검색.목적어를동명사를쓰는동사인지확인(_이전)) return false;
				// 2. 분사가 앞의 명사를 수식하거나
				if(Form1._검색.분사수식가능명사인지확인(_이전)) return false;

				if(Form1._검색.분사로잘쓰이지않는지확인(_현재)) return false;

				// be + ing
				if(_이전.Contains("'s")) return false;
				if(Be동사(_이전)) return false;

				// 이 부분이 문제다.
				if(_주어임시저장소 != "") return false;

				// 아니면 분사구문이라는 건데,
				구문자동분석 중간이후분사구문 = new 구문자동분석();
				string 중간이후분사구문로그 = "";
				string 중간이후분사구문분석결과 = 중간이후분사구문.구문분석(_현재 + " " + _다음어절모두, ref 중간이후분사구문로그);
				if(_목적어임시저장소 != "")
				{
					_결과 += "ⓞ{" + _목적어임시저장소.Trim() + "} " + 중간이후분사구문분석결과; _처리완료 = true;
					_목적어임시저장소 = "";
				}
				else if(_보어임시저장소 != "")
				{
					_결과 += "ⓒ{" + _보어임시저장소.Trim() + "} " + 중간이후분사구문분석결과; _처리완료 = true;
					_보어임시저장소 = "";
				}
				else if(_부사구_전치사구시작)
				{
					_부사구_전치사구시작 = false;

					_결과 = _결과.Trim();

					for (int i = 0; i < _중첩형용사구갯수; i++) { _결과 += "}"; _중첩형용사구갯수 = 0; }

					_결과 += "} " + 중간이후분사구문분석결과; _처리완료 = true;
				}

				else
				{
					_결과 += " " + 중간이후분사구문분석결과; _처리완료 = true;
				}
				return true;
			}
        }



        private bool 이어동사용부사(string 현재어절)
        {
            if (현재어절 == "back") return true;
            if (현재어절 == "up") return true;

            return false;
        }
        private bool 접속어AndOr(string 현재어절)
        {
            현재어절 = 변환.문자열.문장부호제거(현재어절);

            if (현재어절 == "and") return true;
            if (현재어절 == "or") return true;


            if (현재어절 == "And") return true;
            if (현재어절 == "Or") return true;

            return false;
        }
        private bool 분사구문종료()
        {
            if (_분사구문시작 == false) return false;
            if (_마지막 == false) return false;

			if(_목적어임시저장소 != "")
			{
				_결과 += "ⓞ{" + _목적어임시저장소.Trim() + " " + _현재.중괄호붙이기(2); _목적어임시저장소 = "";
			}
			else if(_보어임시저장소 != "")
			{
				_결과 += "ⓒ{" + _보어임시저장소.Trim() + " " + _현재.중괄호붙이기(2); _보어임시저장소 = "";
			}
			else
				_결과 += _현재.중괄호붙이기(2);


            _분사구문시작 = false;
            _처리결과기호 = "분사구문종료";

            return true;
        }
        private bool to부정사3형식명사적용법목적어_내부구문분석시작()
        {
            if (_처리결과기호 != "시작") return false;
            if (_구문분석내_to부정사 == false) return false;
            if (_현재소문자 != "to") return false;
            _결과 = _현재;

            _처리결과기호 = "to부정사3형식명사적용법목적어_내부구문분석시작";

            return true;
        }
        private bool to부정사3형식명사적용법목적어_내부구문분석_서술어()
        {
            if (_구문분석내_to부정사 == false) return false;
            if (_처리결과기호 != "to부정사3형식명사적용법목적어_내부구문분석시작") return false;
            if (_서술어나옴 == true) return false;


            if (이어동사_자동사_현재형가능성(_현재, _다음, _2다음))
            {
                _서술어나옴 = true;
                _자동사나옴 = true;

                _결과 += "ⓥ{" + _현재 + _다음.중괄호붙이기(1);

                _처리결과기호 = "to부정사3형식명사적용법목적어_내부구문분석_서술어";

                return true;
            }

            if (이어동사_타동사_현재형가능성(_현재, _다음, _2다음))
            {
                _서술어나옴 = true;
                _타동사나옴 = true;

                _결과 += "ⓥ{" + _현재 + _다음.중괄호붙이기(1);

                _처리결과기호 = "to부정사3형식명사적용법목적어_내부구문분석_서술어";

                return true;
            }

            if (동사원형(_현재))
            {
                _서술어나옴 = true;
                if (_현재값.Contains("vi.")) _자동사나옴 = true;
                if (_현재값.Contains("vt.")) _타동사나옴 = true;


                _결과 += "ⓥ{" + _현재.중괄호붙이기(1);

                _처리결과기호 = "to부정사3형식명사적용법목적어_내부구문분석_서술어";

                return true;
            }

            return false;
        }
        private bool ToV형용사시작()
        {
            if (!명사가능성(_이전)) return false;
            if (_현재 != "to") return false;
            if (!동사원형(_다음)) return false;

            if(_주어임시저장소 != "")
                _주어임시저장소 += " ⓧ{to";
            else if (_보어임시저장소 != "")
                _보어임시저장소 += "ⓧ{to";
            else if (_목적어임시저장소 != "")
                _목적어임시저장소 += "ⓧ{to";
            else
                _결과 += "ⓧ{to";

            _to부정사형용사적용법시작 = true;
            _처리결과기호 = "to부정사형용사적용법시작";

            return true;
        }
        private bool ToV목적어구시작()
        {
            if (!Form1._검색.목적어를to부정사를쓰는동사인지확인(_이전)) return false;
            if (_현재 != "to") return false;
            if (!동사원형(_다음)) return false;

            _결과 += "ⓞ{to";

            _to부정사목적어구시작 = true;
            _처리결과기호 = "to부정사목적어구시작";

            return true;
        }
        private bool ToV보어구시작()
        {
            if (!_이전값.Contains("2형식동사") && !Be동사(_이전)) return false;
            if (_현재 != "to") return false;
            if (!동사원형(_다음)) return false;

            _결과 += "ⓒ{to";

            _to부정사보어구시작 = true;
            _처리결과기호 = "to부정사보어구시작";

            return true;
        }

        private bool ToV부사적용법시작()
        {
			if (_처리결과기호 == "NiceToMeetYou")
            {
                if (_현재소문자 == "to")
                {
                    if (동사원형(_다음))
                    {
                        _결과 += "ⓧ{" + _현재;

                        _to부정사부사적용법시작 = true;
                        _처리결과기호 = "to부정사부사적용법시작";

                        return true;
                    }
                }
            }

			if (_처리결과기호 == "보어확정가능성")
			{
				if (_현재소문자 == "to")
				{
					if (동사원형(_다음))
					{
						_결과 += "ⓧ{" + _현재;

						_to부정사부사적용법시작 = true;
						_처리결과기호 = "to부정사부사적용법시작";

						return true;
					}
				}
			}

			return false;
        }
        private bool ToV내서술어()
        {
            if (_to부정사부사적용법시작 == false && _to부정사목적어구시작 == false && _to부정사보어구시작 == false && _to부정사형용사적용법시작 == false) return false;

            if (_to부정사서술어나옴 == true) return false;


            if (이어동사_자동사_현재형가능성(_현재, _다음, _2다음))
            {
                _to부정사서술어나옴 = true;
                _to부정사자동사나옴 = true;

                if(_주어임시저장소 != "")
                {
                    if (_2마지막 == true) { _주어임시저장소 += " ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _주어임시저장소 += " ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }
                else if (_보어임시저장소 != "")
                {
                    if (_2마지막 == true) { _보어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _보어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }
                else if (_목적어임시저장소 != "")
                {
                    if (_2마지막 == true) { _목적어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _목적어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }
                else
                {
                    if (_2마지막 == true) { _결과 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _결과 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }

                _처리결과기호 = "to부정사내서술어";
                _i++;
                return true;
            }

            if (이어동사_타동사_현재형가능성(_현재, _다음, _2다음))
            {
                _to부정사서술어나옴 = true;
                _to부정사타동사나옴 = true;


                if(_주어임시저장소 != "")
                {
                    if (_2마지막 == true) { _주어임시저장소 += " ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _주어임시저장소 += " ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }
                else if (_보어임시저장소 != "")
                {
                    if (_2마지막 == true) { _보어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _보어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }
                else if (_목적어임시저장소 != "")
                {
                    if (_2마지막 == true) { _목적어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _목적어임시저장소 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }
                else
                {
                    if (_2마지막 == true) { _결과 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(2); }
                    else { _결과 += "ⓥ{" + _현재 + " " + _다음B.중괄호붙이기(1); }
                }

                _처리결과기호 = "to부정사내서술어";
                _i++;
                return true;
            }
            if (동사원형(_현재))
            {
                _to부정사서술어나옴 = true;
                if (_현재값.Contains("vi.")) _to부정사자동사나옴 = true;
                if (_현재값.Contains("vt.")) _to부정사타동사나옴 = true;
                _처리결과기호 = "to부정사내서술어";
                if(_주어임시저장소 != "")
                {
                    if (_마지막 == true) { _주어임시저장소 += " ⓥ{" + _현재.중괄호붙이기(2); }
                    else { _주어임시저장소 += " ⓥ{" + _현재.중괄호붙이기(1); }
                }
                else if (_보어임시저장소 != "")
                {
                    if (_마지막 == true) { _보어임시저장소 += "ⓥ{" + _현재.중괄호붙이기(2); }
                    else { _보어임시저장소 += "ⓥ{" + _현재.중괄호붙이기(1); }
                }
                else if (_목적어임시저장소 != "")
                {
                    if (_마지막 == true) { _목적어임시저장소 += "ⓥ{" + _현재.중괄호붙이기(2); }
                    else { _목적어임시저장소 += "ⓥ{" + _현재.중괄호붙이기(1); }
                }
                else
                {
                    if (_마지막 == true) { _결과 += "ⓥ{" + _현재.중괄호붙이기(2); }
                    else { _결과 += "ⓥ{" + _현재.중괄호붙이기(1); }
                }
                return true;
            }

            return false;
        }
        private bool ToV내목적어() // 목적어 구가 아니라 그냥 목적어다.
        {
            if (_ToV내목적어임시저장소 != "") return false; // 뭐라도 있으면 안된다.
            if (_to부정사부사적용법시작 == false && _to부정사목적어구시작 == false && _to부정사형용사적용법시작 == false) return false; // to 부정사 용법만 나오면 일단 통과

            if (_현재값 != "" && !_현재값.Contains("n.")) // 사전의 검색결과가 없으면 고유명사일 가능성이 있는데, 고유명사도 아니고, 그냥 명사도 아니면 목적어일리가 없다.
                return false;

            //           if(_부사구_전치사구시작 == true) return false; // 부사구가 시작되면 목적어 가능성은 없다.

            if ((_to부정사타동사나옴 == true) && (_to부정사보어구시작 == false) && (_마지막 == true) && (_to부정사목적어구시작 == false))
            {
                _결과 += "ⓞ{" + _현재.중괄호붙이기(2);
                _처리결과기호 = "to부정사내목적어확정";
                return true;
            }

            if ((_to부정사타동사나옴 == true) && (_to부정사보어구시작 == false) && (_to부정사목적어구시작 == false) && (전치사_to와of빼고(_다음) || 접속어(_다음)))
            {
                _결과 += "ⓞ{" + _현재.중괄호붙이기(1);
                _처리결과기호 = "to부정사내목적어확정";

                _to부정사타동사나옴 = false;
                _to부정사서술어나옴 = false;
                return true;
            }
            // the only way to fix it is to eat. 
            if((_to부정사타동사나옴 == true) &&  명사가능성(_현재) && 서술어가능성(_다음) && (_주어임시저장소 != "") && _to부정사형용사적용법시작 == true)
            {
                _결과 += "ⓢ{" + _주어임시저장소 + " ⓞ{" + _현재.중괄호붙이기(3);
                _주어나옴 = true;
                _처리결과기호 = "to부정사내목적어확정";
                _주어임시저장소 = "";
                _to부정사타동사나옴 = false;
                _to부정사서술어나옴 = false;
                _to부정사형용사적용법시작 = false;
                return true;
            }

            return false;
        }
        public bool ToV내목적어시작()
        {
            if (_to부정사부사적용법시작 == false && _to부정사목적어구시작 == false && _to부정사형용사적용법시작 == false) return false;

            string 이전어절검색결과 = Form1._검색.영한사전_문장부호제거(_이전);


            if (이전어절검색결과.Contains("2형식동사") && _현재 == "like") //The actors sounded like they were reading from a book.
            {
                return false;
            }
            if (_to부정사내조동사서술어시작 == true) return false;    // 조동사 서술어가 안끝났는데 목적어가 도대체 왜 나옴. push back같은 경우, back이 명사도 있지만 이건 아니지.

            if (_to부정사내보어구시작 == true) return false;
            if (_to부정사내목적어나옴 == true) return false;    // 이미 목적어가 나온적이 있으면 목적어가 또 시작될 가능성이 없습니다.
            if (_to부정사내목적어구시작 == true) return false; // 이미 시작했으면 목적어 시작가능성은 없습니다.
            if (_to부정사내부사구_전치사구시작 == true) return false; // 부사구가 시작되면 보어 가능성은 없습니다.

            if (_이전 == "tend" && _현재 == "to") return false; // tend는 뒤에 to 부정사 나오면 자동사임
            if (_이전 == "tends" && _현재 == "to") return false; // tend는 뒤에 to 부정사 나오면 자동사임
            if (_이전 == "tended" && _현재 == "to") return false; // tend는 뒤에 to 부정사 나오면 자동사임


            if (_to부정사타동사나옴 == true && Form1._검색.전치사_나오기_전에_명사_있는지_확인(_다음어절모두)) // they eat lunch in the morning. 바로 다음 것이 전치사인 경우 그냥 목적어 확정을 해야 한다.
            {
                _to부정사내목적어구시작 = true;

                //_처리결과 += "ⓞ{" + 현재어절;

                _ToV내목적어임시저장소 += _현재;
                _처리결과기호 = "to부정사내목적어시작";

                return true;
            }

			// to tell the truth
			if (_to부정사타동사나옴 == true && (_이전값.Contains("2형식으로잘쓰이지않음") || _to부정사자동사나옴 == false) && 관사(_현재))
			{
				_ToV내목적어임시저장소 += _현재;
				_처리결과기호 = "to부정사내목적어시작";

				return true;
			}

			return false;
        }
        public bool to부정사내목적어종료가능성()
		{
			string to부정사내목적어구문분석로그 = "";

			// 명사일 가능성이 없으면 목적어일 가능성이 없다. 하지만 목적어 임시 저장소가 있다면 일단 그것은 그냥 뿌려줘야 한다.
			if (!_현재값.Contains("n.") && !_현재값.Contains("ing형") && _현재값 != "" && _목적어임시저장소 == "")
            {
                return false;
            }


            if (_to부정사내목적어구시작 == false) return false; // 시작도 안한 걸 종료할 수 없다.

            if ((변환.문자열.Right(_현재, 1) == ".") || _마지막 == true ||
            ((변환.문자열.Right(this._현재, 1) == ",") && !_현재값.Contains("사람이름") && _다음값.Contains("사람이름"))
                )
            {
                _to부정사내목적어구시작 = false;

                _ToV내목적어임시저장소 += _현재;

                구문자동분석 목적어절분석 = new 구문자동분석();

                //                if(_목적어임시저장소.StartsWith("to "))
                //              {
                //                목적어절분석._구문분석내_to부정사 = true;
                //              _처리결과 += "ⓞ{" + 목적어절분석.구문분석(_목적어임시저장소).중괄호붙이기(1);
                //        }
                //      else
                if (_목적어임시저장소 != "")
                {
                    _결과 += "ⓞ{" + _목적어임시저장소 + "ⓞ{" + 목적어절분석.구문분석(_ToV내목적어임시저장소, ref to부정사내목적어구문분석로그).중괄호붙이기(3);
                    _목적어임시저장소 = "";
                }
                else if (_보어임시저장소 != "")
                {
                    _결과 += "ⓞ{" + _보어임시저장소 + "ⓞ{" + 목적어절분석.구문분석(_ToV내목적어임시저장소, ref to부정사내목적어구문분석로그).중괄호붙이기(3);
                    _보어임시저장소 = "";
                }
                else
                {
                    _결과 += "ⓞ{" + 목적어절분석.구문분석(_ToV내목적어임시저장소, ref to부정사내목적어구문분석로그).중괄호붙이기(2); // to 부정사 내부이므로 2개를 붙여주어야 한다.
                }

                _ToV내목적어임시저장소 = "";



                _처리결과기호 = "to부정사내목적어종료가능성";
                _to부정사내목적어나옴 = true;
                return true;
            }

            return false;
        }
        private bool ToV부사끝()
        {
            if (_to부정사부사적용법시작 == false) return false;

            if (_마지막 == true)
            {
				if (_ToV내목적어임시저장소 != "")
				{
					_결과 += "ⓞ{" + _ToV내목적어임시저장소 + _현재.중괄호붙이기(2);

					_ToV내목적어임시저장소 = "";
				}
				else
					_결과 += 현재내용에중괄호붙이기(_현재);

				if(_부사구_전치사구시작)
				{
					_결과 = _결과.중괄호붙이기(1);
				}



                _처리결과기호 = "to부정사부사적용법종료";
                return true;
            }

            return false;
        }
        private bool to부정사보어구종료()
        {
            if (_to부정사보어구시작 == false) return false;


            if (_마지막 == true)
            {
                _결과 += 현재내용에중괄호붙이기(_현재);
                _처리결과기호 = "to부정사보어구종료";
                return true;
            }

            return false;
        }
        private bool to부정사목적어구종료()
        {
            if (_to부정사목적어구시작 == false) return false;


            if (_마지막 == true)
            {
                _결과 += 현재내용에중괄호붙이기(_현재);
                _처리결과기호 = "to부정사목적어구종료";
                return true;
            }

            return false;
        }
        private string 현재내용에중괄호붙이기(string 현재내용)
        {
            if (현재내용 == "") return "";

            if (변환.문자열.Right(현재내용, 1) == ".") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}."; }
            else if (변환.문자열.Right(현재내용, 1) == ",") { return 현재내용.Substring(0, 현재내용.Length - 1) + "},"; }
            else if (변환.문자열.Right(현재내용, 1) == "!") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}!"; }
            else if (변환.문자열.Right(현재내용, 1) == "?") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}?"; }
            else return 현재내용 + "}";
        }
        private string 현재내용에중괄호두개붙이기(string 현재내용)
        {
            if (현재내용 == "") return "";

            if (변환.문자열.Right(현재내용, 1) == ".") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}."; }
            else if (변환.문자열.Right(현재내용, 1) == ",") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}},"; }
            else if (변환.문자열.Right(현재내용, 1) == "!") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}!"; }
            else if (변환.문자열.Right(현재내용, 1) == "?") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}?"; }
            else return 현재내용 + "}}";
        }
        private string 현재내용에중괄호세개붙이기(string 현재내용)
        {
            if (현재내용 == "") return "";

            if (변환.문자열.Right(현재내용, 1) == ".") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}}."; }
            else if (변환.문자열.Right(현재내용, 1) == ",") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}},"; }
            else if (변환.문자열.Right(현재내용, 1) == "!") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}}!"; }
            else if (변환.문자열.Right(현재내용, 1) == "?") { return 현재내용.Substring(0, 현재내용.Length - 1) + "}}}?"; }
            else return 현재내용 + "}}";
        }
        private bool 동사원형(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            if (현재어절검색결과.Contains("현재형")) return true;

            if (현재어절검색결과.Contains("vt.") || 현재어절검색결과.Contains("vi.") || 현재어절검색결과.Contains("v."))
            {
                if (!현재어절검색결과.Contains("ed형") && !현재어절검색결과.Contains("pp형") && !현재어절검색결과.Contains("과거형"))
                {
                    return true;
                }

            }
            return false;
        }
        private bool 명사가능성(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);
            if (현재어절검색결과.Contains("n.")) return true;
            if (현재어절검색결과.Contains("ing형")) return true;


            return false;
        }
        private bool 동사가능성(string 현재어절)
        {
            return 서술어가능성(현재어절);


            //            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            //            if(현재어절검색결과.Contains("vt.") || 현재어절검색결과.Contains("vi.") || 현재어절검색결과.Contains("v."))
            //            {
            //                if(현재어절검색결과.Contains("ed형")) return true;

            //                if(현재어절검색결과.Contains("과거형")) return true;

            //                if(현재어절검색결과.Contains("pp형")) return false; // 과거형이 아니고 pp형만 있는 경우 동사일 수가 없습니다.

            //                if(현재어절검색결과.Contains("ing형")) return false;

            //                return true;
            //            }

            //            return false;
        }

		// 동사밖에 없는 경우, 동사인 것이 확실하다.
		private bool 동사확실(string 현재어절)
		{
			string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

			if(현재어절검색결과.Contains("n.")) return false;
			if(현재어절검색결과.Contains("a.")) return false;
			if(현재어절검색결과.Contains("ad.")) return false;
			if(현재어절검색결과.Contains("conj.")) return false;
			if(현재어절검색결과.Contains("prep.")) return false;
			if(현재어절검색결과.Contains("pron.")) return false;
			if(현재어절검색결과.Contains("int.")) return false;

			if(현재어절검색결과.Contains("vt.")) return true;
			if(현재어절검색결과.Contains("vi.")) return true;

			return false;
		}

        private bool 동사_S안붙거나과거형(string 현재어절)
        {
            if (현재어절 == "are") return true;

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);


            if (현재어절검색결과.Contains("동사로 잘 안씀")) return false;

            if (현재어절검색결과.Contains("vt.") || 현재어절검색결과.Contains("vi.") || 현재어절검색결과.Contains("v."))
            {
                if (현재어절검색결과.Contains("ed형")) return true;

                if (현재어절검색결과.Contains("과거형")) return true;

                if (현재어절검색결과.Contains("s형")) return false;

                if (현재어절검색결과.Contains("pp형")) return false; // 과거형이 아니고 pp형만 있는 경우 동사일 수가 없습니다.

                if (현재어절검색결과.Contains("ing형")) return false;

                return true;
            }

            return false;
        }
        private bool 동사_S붙거나과거형(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            if (현재어절검색결과.Contains("vt.") || 현재어절검색결과.Contains("vi.") || 현재어절검색결과.Contains("v."))
            {
                if (현재어절검색결과.Contains("ed형")) return true;

                if (현재어절검색결과.Contains("과거형")) return true;

                if (현재어절검색결과.Contains("s형")) return true;

                if (현재어절검색결과.Contains("pp형")) return false; // 과거형이 아니고 pp형만 있는 경우 동사일 수가 없습니다.

                if (현재어절검색결과.Contains("ing형")) return false;

                return true;
            }

            return false;
        }
        private bool NiceToMeetYou구문()
        {
            if (_처리결과기호 != "시작") return false;

            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);
            string 다다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_2다음);


            if (_현재값.Contains("a.") && (_다음 == "to") && (다다음어절검색결과.Contains("vi.") || 다다음어절검색결과.Contains("vt.") || 다다음어절검색결과.Contains("v."))
                && !다다음어절검색결과.Contains("ing형") && !다다음어절검색결과.Contains("ed형") && !다다음어절검색결과.Contains("과거형") && !다다음어절검색결과.Contains("pp형"))
            {
                _결과 += "ⓒ{" + _현재 + "}";

                _처리결과기호 = "NiceToMeetYou";
                return true;
            }

            return false;
        }
        private bool 의문부사()
        {
            if (_처리결과기호 != "시작") return false;

            if (_현재 == "How") { _결과 += "ⓧ{How}"; _처리결과기호 = "의문부사"; return true; }
            if (_현재 == "how") { _결과 += "ⓧ{how}"; _처리결과기호 = "의문부사"; return true; }
            if (_현재 == "Where") { _결과 += "ⓧ{Where}"; _처리결과기호 = "의문부사"; return true; }
            if (_현재 == "where") { _결과 += "ⓧ{where}"; _처리결과기호 = "의문부사"; return true; }
            if (_현재 == "When") {
                if (_내부첫접속사부사구분 == "접속사")
                {
                    _결과 += "㉨{When}"; _처리결과기호 = "시작"; return true; // 접속사는 시작으로 해서 한 번 더 돌 수 있도록 한다.
                }
                else if(!Be동사(_다음) && !조동사(_다음)) // When your blood sugar is low 
                {
                    _결과 += "㉨{When}"; _처리결과기호 = "시작"; return true;
                }
                else
                {
                    _결과 += "ⓧ{When}"; _처리결과기호 = "의문부사"; return true;
                }
            }
            if (_현재 == "when")
            {
                if (_내부첫접속사부사구분 == "접속사")
                {
                    _결과 += "㉨{when}"; _처리결과기호 = "시작"; return true;
                }
                else if(!Be동사(_다음) && !조동사(_다음)) // So when your blood sugar is low 
                {
                    _결과 += "㉨{when}"; _처리결과기호 = "시작"; return true;
                }
                else
                {
                    _결과 += "ⓧ{when}"; _처리결과기호 = "의문부사"; return true;
                }
            }

            return false;
        }
        private bool 의문형용사()
        {
            if (_현재 == "Which" && 명사가능성(_다음) && 조동사(_2다음))
            {
                _결과 += "ⓞ{Which " + _다음 + "} ⓧ{" + _2다음 + "}";

                _i += 2;
                return true;

            }
            return false;
        }
        private bool 의문대명사()
        {
            if (_현재소문자 == "what" && _처리결과기호 == "시작")
            {
                if (조동사(_다음))
                {
                    _결과 += "ⓞ{" + _현재 + "} ⓧ{" + _다음 + "}";
                    _i++;
                    _조동사나옴 = true;
                    _목적어나옴 = true;
                }
                else if (_다음 == "about")
                {
                    _결과 += "ⓞ{" + _현재 + "} ⓧ{" + _다음;

                    _i++;
                    _부사구_전치사구시작 = true;
                    _전치사구동반한명사구시작 = true;
                }
                else if (Be동사(_다음))
                {
                    if (형용사가능성(_2다음) && (_3다음 == ""))
                    {
                        _결과 += "ⓢ{" + _현재 + "} ⓥ{" + _다음 + "} ⓒ{" + _다음2B.중괄호붙이기(1);

                        _처리완료 = true;
                        return true;
                    }
                    else
                    {
                        _결과 += "ⓒ{" + _현재 + "} ⓥ{" + _다음 + "} ⓢ{" + _다다음어절모두.중괄호붙이기(1);

                        _처리완료 = true;
                        return true;
                    }
                }
                else
                {
                    _결과 += "ⓢ{" + _현재 + "} ";
                    _주어나옴 = true;
                }

                _처리결과기호 = "의문대명사";

                return true;
            }
            else if (_현재소문자 == "what's")
            {
                if (_다음값.Contains("명사로잘쓰이지않음") && 형용사가능성(_다음))
                {
                    _결과 += "ⓢ{" + _현재.Substring(0, _현재.Length - 2) + "}ⓥ{'s}" + " ⓒ{" + _다음B.중괄호붙이기(1);

                    _i++;

                    return true;
                }
				else if(_다음값.Contains("ing형"))
				{
					if(동사확실(_다음))
					{
	                    _결과 += "ⓢ{" + _현재.Substring(0, _현재.Length - 2) + "}ⓥ{'s " + _다음B.중괄호붙이기(1);

						_i++;


						if(_다음값.Contains("vi.")) _자동사나옴 = true;
						if(_다음값.Contains("vt.")) _타동사나옴 = true; 

						return true;
					}
					else if(_다음값.Contains("n."))
					{
						_결과 += "ⓒ{" + _현재.Substring(0, _현재.Length - 2) + "}ⓥ{'s} ⓢ{" + _다음어절모두.중괄호붙이기(1);
						// 명사

						_처리완료 = true;
						return true;
					}
					else if(_다음값.Contains("a."))
					{
						_결과 += "ⓢ{" + _현재.Substring(0, _현재.Length - 2) + "}ⓥ{'s} ⓒ{" + _다음어절모두.중괄호붙이기(1);

						// 형용사
						_처리완료 = true;
						return true;
					}
				}
				else if(관사(_다음) || 소유대명사(_다음))
				{
                    _결과 += "ⓒ{" + _현재.Substring(0, _현재.Length - 2) + "}ⓥ{'s} ⓢ{" + _다음어절모두.중괄호붙이기(1);

                    _처리완료 = true;
                    return true;
				}

            }
            return false;
        }
        private bool Today_Yesterday()
        {
            if ((_현재소문자 == "yesterday" || _현재소문자 == "today") && _부사구_전치사구시작)
            {
                _결과 = _결과.Trim() + "} ⓧ{" + _현재.중괄호붙이기(1);

                _부사구_전치사구시작 = false;
                return true;
            }

            return false;
        }
        private bool 감탄사_단일부사_어구()
        {

            if (_현재소문자 == "yes")																{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
			if (_현재소문자 == "hello")																{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
			if (_현재소문자 == "no" && _처리결과기호 == "시작" && _구문분석내_구문분석 == false)	{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
            if (_현재소문자 == "ok" && _처리결과기호 == "시작" && _구문분석내_구문분석 == false)	{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
            if (_현재소문자 == "hi")																{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
            if (_현재소문자 == "now")																{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
            if (_현재소문자 == "oh")																{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
            if (_현재소문자 == "really")															{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
            if (_현재소문자 == "thanks")															{ _결과 += "ⓧ{" + _현재B.중괄호붙이기(1); return true; }
            if (_현재소문자 == "of" && _다음.ToLower() == "course")									{ _결과 += "ⓧ{" + _현재B + " " + _다음B.중괄호붙이기(1); _i++; _처리결과기호 = "감탄사_단일부사_기타단일어구"; return true; }

            return false;
        }
        private bool WH_About시작()
        {
            if ((_현재 == "How" || _현재 == "What") && _다음 == "about")
            {
                _결과 += "ⓧ{" + _현재 + "}";

                _HowAbout_WhatAbout시작 = true;
                _처리결과기호 = "HowAboutWhatAbout";
                return true;
            }


            return false;
        }
        private bool 접속사확정가능성()
        {
			if (_처리결과기호 == "시작")
			{
				string 쉼표앞부분 = "";

				if (_다음어절모두.Contains(","))
					쉼표앞부분 = _다음어절모두.Substring(0, _다음어절모두.IndexOf(","));
				else
					쉼표앞부분 = _다음어절모두;


				if (접속어(_현재) && _현재.Contains(","))
				{
					_결과 += "㉨{" + 현재내용에중괄호붙이기(_현재);

					_처리결과기호 = "시작"; // 이걸 시작으로 해놔야 접속사를 처리하고 그 다음 부분을 맨 처음으로 인식해서 제대로 처리한다.
					return true;
				}
				else if(_현재소문자 == "that" && _다음값.Contains("s형")) { return false; } // That sounds great.에서의 That을 접속사로 보고 있다.
				else if (접속어(_현재))
				{
					구문자동분석 _임시 = new 구문자동분석();

					string 접속사구문분석로그 = "";
					if (!_임시.구문분석(쉼표앞부분, true, ref 접속사구문분석로그).Contains("ⓥ")) return false;

					_결과 += "㉨{" + _현재 + "}";

					_처리결과기호 = "시작"; // 이걸 시작으로 해놔야 접속사를 처리하고 그 다음 부분을 맨 처음으로 인식해서 제대로 처리한다.
					return true;
				}
			}
            return false;
        }
        private bool 서술어부근의_부사()
        {
            if (전치사가능성(_현재)) return false;

            // 짜야 함
            // 일반동사 Be동사 다음의 부사
            if (((_처리결과기호 == "주어Be동사") || (_처리결과기호 == "명령어수여동사") || (_처리결과기호 == "명령어서술어") || (_처리결과기호 == "과거형동사")
                || (_처리결과기호 == "감각동사") || (_처리결과기호 == "수여동사") || (_처리결과기호 == "서술어") || (_처리결과기호 == "to부정사내서술어")
                || (_처리결과기호 == "havePP종료") || (_처리결과기호 == "서술어s형동사") || (_처리결과기호 == "조동사서술어종료"))
                && 부사_다른품사는없는(_현재))
            {
                _결과 += "ⓧ{" + _현재.중괄호붙이기(1);
                _처리결과기호 = "서술어부근의부사";
                return true;
            }
            else if (Be동사(_이전) && 부사가능성(_현재) && 형용사가능성(_다음) && _2마지막) // The music was pretty good.
            {
                _결과 += "ⓧ{" + _현재.중괄호붙이기(1);
                _처리결과기호 = "서술어부근의부사";
                return true;
            }
            else if(!_마지막 && (_처리결과기호 == "목적어확정가능성" || _처리결과기호 == "목적어종료가능성") && 부사가능성(_현재))
            {
                _결과 += "ⓧ{" + _현재.중괄호붙이기(1);
                _처리결과기호 = "서술어부근의부사";
                return true;
            }

            return false;
        }
        private bool 부사_다른품사는없는(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            if (현재어절 == "very") return true; // 아니 very에 형용사 뜻이 있다. the very man 바로 그 사람

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            if (현재어절검색결과.Contains("ad."))
            {
                if (!현재어절검색결과.Contains("a.") && !현재어절검색결과.Contains("vt.") && !현재어절검색결과.Contains("vi.") && !현재어절검색결과.Contains("n.") && !현재어절검색결과.Contains("def.") && !현재어절검색결과.Contains("conj."))
                {
                    return true;
                }
            }

            return false;
        }
        private bool 부사가능성(string 현재어절)
        {
			if(현재어절 == "no") return false; // he had no loose change, 문두에 no가 나타나는 경우에는 부사로 쓰일 수 있지만, 중간에 애매하게 나오는 경우는 절대 아니다.

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);
            if (현재어절검색결과.Contains("ad.")) return true;

            return false;
        }
        private bool 직접목적어종료가능성()
        {
            string 검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

            // 명사일 가능성이 없으면 직접목적어일 가능성이 없다.
            if (!검색결과.Contains("n.") && !검색결과.Contains("ing형") && 검색결과 != "")
            {
                return false;
            }


            if (_직접목적어시작 == false) return false; // 시작도 안한 걸 종료할 수 없다.

            if ((변환.문자열.Right(_현재, 1) == ".") || _마지막 == true)
            {
                _직접목적어시작 = false;

                _결과 += _현재 + "}";
                _결과 = _결과.Replace(".}", "}.");

                _처리결과기호 = "직접목적어종료가능성";
                return true;
            }

            return false;

        }
        private bool 직접목적어시작가능성()
        {
            if (_직접목적어시작 == true) return false; // 이미 시작한 것을 또 시작할 수는 없습니다.

            if (_간접목적어나옴 == true)
            {
                _결과 += "ⓓ{" + _현재;

                _직접목적어시작 = true;
                _처리결과기호 = "직접목적어시작가능성";
                return true;
            }

            return false;
        }
        private bool 직접목적어확정가능성()
        {

            if (_간접목적어나옴 == false) return false; // 간접목적어가 있어야 직접목적어가 시작됩니다.
            if (_직접목적어시작 == true) return false; // 직접목적어구가 시작된 상태라면, 자기 혼자서 확정할 수 없습니다.


            if ((_마지막 == true) || (전치사_to와of빼고(_다음) || 접속어(_다음)))
            {
                _결과 += string.Format("ⓓ{{{0}}}", _현재);
                _처리결과기호 = "직접목적어확정가능성";
                return true;
            }

            return false;

        }
        private bool S있고_수여동사()
        {
            if (!_주어나옴) return false;

            if (_수여동사나옴 == true) return false; // 나왔는데 또나오면 에러

            string 현재어절소문자 = _현재.ToLower();



            if (현재어절소문자 == "give") _수여동사나옴 = true;
            if (현재어절소문자 == "gives") _수여동사나옴 = true;
            if (현재어절소문자 == "gave") _수여동사나옴 = true;

            if (_수여동사나옴 == true)
            {
                _결과 += "ⓥ{" + _현재 + "}";
                _처리결과기호 = "수여동사";

                return true;
            }

            return false;
        }
        private bool 주어있고_havePP시작()
        {
            if (!_주어나옴) return false;

            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);

            if (PP형(_다음) || (다음어절검색결과.Contains("ad.") && PP형(_2다음)))
            {
                if (_현재 == "had" || _현재 == "have" || _현재 == "has")
                {
                    _결과 += "ⓥ{" + _현재;

                    _서술어나옴 = true;
                    _havePP시작 = true;
                    _처리결과기호 = "주어가있는상태에서_havePP시작";
                    return true;
                }

            }

            return false;
        }
        private bool S미정_havePP시작()
        {
            if (_주어나옴) return false;
            if (_주어임시저장소 == "") return false;

            string 이전어절검색결과 = Form1._검색.영한사전_문장부호제거(_이전);
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);
            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);
            string 다다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_2다음);

            if (_현재 == "have")
            {
                if ((_다음 == "fried") && (_2다음 == "food")) return false;

                if (이전어절검색결과.Contains("ad.") || 이전어절검색결과.Contains("복수") || 이전어절검색결과.Contains("pl."))
                {
                    if (PP형(_다음) || (다음어절검색결과.Contains("ad.") && PP형(_2다음)))
                    {

                        _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{have";
                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _주어임시저장소 = "";
                        _havePP시작 = true;
                        _처리결과기호 = "주어는미정인데_havePP시작";
                        return true;
                    }
                }
            }
            if (_현재 == "has")
            {
                if ((_다음 == "fried") && (_2다음 == "food")) return false;

                if (이전어절검색결과.Contains("ad.") || (이전어절검색결과.Contains("n.") && !이전어절검색결과.Contains("복수") && !이전어절검색결과.Contains("pl.")))
                {
                    if (PP형(_다음) || (다음어절검색결과.Contains("ad.") && PP형(_2다음)))
                    {

                        _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{has";
                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _주어임시저장소 = "";
                        _havePP시작 = true;
                        _처리결과기호 = "주어는미정인데_havePP시작";
                        return true;
                    }
                }
            }
            if (_현재 == "had")
            {
                if ((_다음 == "fried") && (_2다음 == "food")) return false;

                if (PP형(_다음) || (다음어절검색결과.Contains("ad.") && PP형(_2다음)))
                {
                    _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{had";
                    _주어나옴 = true;
                    _서술어나옴 = true;
                    _주어임시저장소 = "";
                    _havePP시작 = true;
                    _처리결과기호 = "주어는미정인데_havePP시작";
                    return true;
                }
            }
            return false;
        }
        private bool PP형(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            if (현재어절검색결과.Contains("ed형")) return true;
            if (현재어절검색결과.Contains("pp형")) return true;
            if (현재어절검색결과.Contains("p.p형")) return true;
            if (현재어절검색결과.Contains("p.p.형")) return true;

            return false;
        }
        private bool ING형(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            if (현재어절검색결과.Contains("ing형")) return true;

            return false;
        }
        private bool havePP종료(string 현재어절)
        {
            if (_havePP시작 == false) return false;
            else
            {
                string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

                if (현재어절검색결과.Contains("vt.")) { _타동사나옴 = true; }

                if (현재어절검색결과.Contains("vi.")) { _자동사나옴 = true; }

                if ((_자동사나옴 == false) && (_타동사나옴 == false))
                {
                    if (현재어절검색결과.Contains("ad."))
                    {
                        _결과 += "ⓧ{" + 현재어절 + "}";
                        _처리결과기호 = "havePP종료";
                        return true;
                    }
                    else
                    {
                        _결과 += 현재어절;
                        _처리결과기호 = "havePP종료";
                        return true;
                    }
                }


                _havePP시작 = false;

                _결과 += 현재어절 + "}";




                _처리결과기호 = "havePP종료";
                return true;
            }
        }
        private bool S미정_Ved()
        {
            if (_주어나옴) return false;
            if (_주어임시저장소 == "") return false;

            if (부사_다른품사는없는(_주어임시저장소)) return false;


            if (관사(_이전) || 소유대명사(_이전)) return false;


            if (소유대명사도되고_형용사도되는것(_이전) && 과거형동사가확실한것(_현재)) return true;

            // these나 this나 those 같은 것들이 문제다.
            // These interconnected webs are intricately involved in our memories.
            if (소유대명사도되고_형용사도되는것(_이전)) return false;


            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);
            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);

            if (이어동사_자동사_과거형가능성(_현재, _다음, _2다음))
            {
                _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + " " + _다음 + "}";

                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                _주어임시저장소 = "";
                _처리결과기호 = "과거형동사";
                _i++;
                return true;
            }
            else if (이어동사_타동사_과거형가능성(_현재, _다음, _2다음))
            {
                _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + " " + _다음 + "}";

                _주어나옴 = true;
                _서술어나옴 = true;
                _타동사나옴 = true;

                _주어임시저장소 = "";
                _처리결과기호 = "과거형동사";
                _i++;
                return true;
            }
            else if ((현재어절검색결과.Contains("ed형") || 현재어절검색결과.Contains("과거형")))
            {
                if ((_다음 == "the") || (_다음 == "a"))
                {
                    if (현재어절검색결과.Contains("vi.") && 현재어절검색결과.Contains("vt."))
                    {
                        if (_부사구_전치사구시작)
                        {
                            _결과 += "ⓢ{" + 현재내용에중괄호두개붙이기(_주어임시저장소) + " " + "ⓥ{" + _현재 + "}";
                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;
                        }
                        else
                        {
                            _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                        }

                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _자동사나옴 = true;
                        _타동사나옴 = true;

                        _주어임시저장소 = "";
                        _처리결과기호 = "과거형동사";
                        return true;
                    }
                    if (현재어절검색결과.Contains("vi."))
                    {
                        if (_부사구_전치사구시작)
                        {
                            _결과 += "ⓢ{" + 현재내용에중괄호두개붙이기(_주어임시저장소) + " " + "ⓥ{" + _현재 + "}";
                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;
                        }
                        else
                        {
                            _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                        }
                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _자동사나옴 = true;

                        _주어임시저장소 = "";
                        _처리결과기호 = "과거형동사";
                        return true;
                    }
                    if (현재어절검색결과.Contains("vt."))
                    {
                        if (_부사구_전치사구시작)
                        {
                            _결과 += "ⓢ{" + 현재내용에중괄호두개붙이기(_주어임시저장소) + " " + "ⓥ{" + _현재 + "}";
                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;
                        }
                        else
                        {
                            _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                        }
                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _타동사나옴 = true;

                        _주어임시저장소 = "";
                        _처리결과기호 = "과거형동사";
                        return true;
                    }
                }
                else if (명사가능성(_현재) && 동사가능성(_다음))
                {
                    return false;
                }
                else
                {
                    // 여기에 pp형이 주어를 꾸며주는 긴 주어구문을 처리할 수 있도록 해야한다.
                    // 일단은, 그냥 모두 true

                    if (현재어절검색결과.Contains("vi.") && 현재어절검색결과.Contains("vt."))
                    {
                        if (_부사구_전치사구시작)
                        {
                            _결과 += "ⓢ{" + 현재내용에중괄호두개붙이기(_주어임시저장소) + " " + "ⓥ{" + _현재 + "}";
                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;
                        }
                        else
                        {
                            _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                        }
                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _자동사나옴 = true;
                        _타동사나옴 = true;

                        _주어임시저장소 = "";
                        _처리결과기호 = "과거형동사";
                        return true;
                    }
                    if (현재어절검색결과.Contains("vi."))
                    {
                        if (_부사구_전치사구시작)
                        {
                            _결과 += "ⓢ{" + 현재내용에중괄호두개붙이기(_주어임시저장소) + " " + "ⓥ{" + _현재 + "}";
                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;
                        }
                        else
                        {
                            _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                        }
                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _자동사나옴 = true;

                        _주어임시저장소 = "";
                        _처리결과기호 = "과거형동사";
                        return true;
                    }
                    if (현재어절검색결과.Contains("vt."))
                    {
                        if (_부사구_전치사구시작)
                        {
                            _결과 += "ⓢ{" + 현재내용에중괄호두개붙이기(_주어임시저장소) + " " + "ⓥ{" + _현재 + "}";
                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;
                        }
                        else
                        {
                            _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                        }
                        _주어나옴 = true;
                        _서술어나옴 = true;
                        _타동사나옴 = true;

                        _주어임시저장소 = "";
                        _처리결과기호 = "과거형동사";
                        return true;
                    }

                }
            }
            return false;
        }
        // 이거 다음 어절이 서술어여야 되는 거 아닌가 싶은데 애매하다. 왜 만들었을까.
        private bool S미정_부사()
        {
            if (_주어나옴) return false;
            if (_주어임시저장소 == "") return false;
            if (_이전 == "a") return false; // ⓢ{a} ⓧ{truly} good speaker ⓥ{is}
            if (_이전 == "A") return false; // ⓢ{a} ⓧ{truly} good speaker ⓥ{is}
            if (_이전 == "the") return false; // ⓢ{a} ⓧ{truly} good speaker ⓥ{is}
            if (_이전 == "The") return false; // ⓢ{a} ⓧ{truly} good speaker ⓥ{is}

            if (_현재 == "there") return false; // The best scientists out there aren’t emotionless beings
            if (_현재 == "There") return false;

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

            if (현재어절검색결과.Contains("prep.")) return false; // 전치사도 있는 경우에는 전치사구를 부사로 착각하여 날려버리는 경우가 있음.
            if (현재어절검색결과.Contains("n.")) return false; // 사실 다른 품사도 있어보니, 완전 이상하다. 그냥 딱 부사만 있어야 될듯.
            if (현재어절검색결과.Contains("a.")) return false; // 
            if (현재어절검색결과.Contains("vi.")) return false; // 
            if (현재어절검색결과.Contains("vt.")) return false; // 

            if (현재어절검색결과.Contains("ad."))
            {
                _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓧ{" + _현재 + "}";
                _주어나옴 = true;
				_주어임시저장소 = "";
				_처리결과기호 = "부사";
                return true;
            }

            return false;
        }
        private bool S미정_Be동사()
        {
            if (_주어나옴) return false;
            if (_주어임시저장소 == "") return false;

            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);

            if (Be동사(_현재) && !다음어절검색결과.Contains("ing형"))
            {
                if (_전치사구동반한명사구시작)
                {
                    _결과 += "ⓢ{" + _주어임시저장소 + "}} " + "ⓥ{" + _현재 + "}";

                    _부사구_전치사구시작 = false;
                    _전치사구동반한명사구시작 = false;
                }
                else
                    _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";


                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;
                _주어임시저장소 = "";
                _처리결과기호 = "서술어";
                return true;
            }
            return false;
        }
        private bool Be동사(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            if (현재어절 == "am") return true;
            if (현재어절 == "is") return true;
            if (현재어절 == "are") return true;
            if (현재어절 == "was") return true;
            if (현재어절 == "were") return true;

            if (현재어절 == "isn't") return true;
            if (현재어절 == "aren't") return true;
            if (현재어절 == "wasn't") return true;
            if (현재어절 == "weren't") return true;


            return false;
        }
        private bool Have동사(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            if (현재어절 == "have") return true;
            if (현재어절 == "has") return true;
            if (현재어절 == "had") return true;

            return false;
        }
        private bool S미정_Vs()
        {
            if (_주어나옴) return false;
            if (_주어임시저장소 == "") return false;
            if (_서술어나옴 == true) return false;

            if (관사(_주어임시저장소.Trim())) return false;
            if (소유대명사(_주어임시저장소.Trim())) return false;

			if (_이전소문자 == "2") return false;
			if (_이전소문자 == "two") return false;

            if (!동사확실(_현재) && 동사가능성(_다음) && (_다음 != "like")) return false;
            if (전치사구이후동사나오는지()) return false;

            if (_현재값.Contains("s형") && _현재값.Contains("vi.") && _현재값.Contains("vt."))
            {
                _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;
                _타동사나옴 = true;
                _주어임시저장소 = "";
                _처리결과기호 = "서술어s형동사";

                if (_현재값.Contains("2형식동사")) _2형식동사 = true;

                return true;
            }
            else if (_현재값.Contains("s형") && _현재값.Contains("vi."))
            {
                _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;
                _주어임시저장소 = "";
                _처리결과기호 = "서술어s형동사";

                if (_현재값.Contains("2형식동사")) _2형식동사 = true;

                return true;
            }
            else if (_현재값.Contains("s형") && _현재값.Contains("vt.") && !_마지막)
            {
                _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";
                _주어나옴 = true;
                _서술어나옴 = true;
                _타동사나옴 = true;
                _주어임시저장소 = "";
                _처리결과기호 = "서술어s형동사";

                return true;
            }

            return false;
        }
        private bool 전치사구이후동사나오는지()
        {
			bool 현재어절복수여부 = false;
            if (!전치사가능성(_다음)) return false;

            // to부정사여기서 헷갈리라고 만든 거 아님
            if (_다음 == "to" && 동사원형(_2다음)) return false;

            if (_현재값.Contains("s형") || _현재값.Contains("의 복수") || _현재값.Contains("pl.")) // || 이전어절모두.Contains(" and ") || 이전어절모두.Contains(" and, "))
            {
                현재어절복수여부 = true;
            }


            string[] 다음어절들 = _다음어절모두.Split(' ');

            foreach (string 지금다음어절 in 다음어절들)
            {
                if (현재어절복수여부)
                {
                    if (동사_S안붙거나과거형(지금다음어절) && (지금다음어절 != "like")) return true; // like는 이런 경우 대개 전치사임
                }
                else
                {
                    if (동사_S붙거나과거형(지금다음어절)) return true;
                    // 짜고 있는 중.
                }
            }


            return false;
        }
        private bool S미정복수_기본동사()
        {
            if (_주어나옴) return false;
            if (_주어임시저장소 == "") return false;
            if (_서술어나옴) return false;

            string 이전어절검색결과 = Form1._검색.영한사전_문장부호제거(_이전);
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

            bool 이전어절복수여부 = false;

            if (이전어절검색결과.Contains("s형") || 이전어절검색결과.Contains("의 복수") || 이전어절검색결과.Contains("pl.") || _이전어절모두.Contains(" and ") || _이전어절모두.Contains(" and, ") || 복수형_구_가능성(_이전어절모두))
            {
                이전어절복수여부 = true;
            }

            if (_현재 == "like") // like가 전치사인지를 확인하려는 것
            {
                string[] 다음어절들 = _다음어절모두.Split(' ');

                foreach (string 지금다음어절 in 다음어절들)
                {
                    if (이전어절복수여부)
                    {
                        if (동사_S안붙거나과거형(지금다음어절)) return false;
                    }
                    else
                    {
                        if (동사_S붙거나과거형(지금다음어절)) return false;
                    }
                }
            }


            if (이전어절복수여부)
            {
                if (!동사_S안붙거나과거형(_현재)) return false;

                if (동사_S안붙거나과거형(_다음)) return false; // both the fee and the completed registration form are received. - form이 동사가 있어서 걸러내야 됨


                if (_전치사구동반한명사구시작 == true)
                {
                    string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);

                    if ((다음어절검색결과.Contains("vi.") || 다음어절검색결과.Contains("vt.")) && !다음어절검색결과.Contains("ing형"))
                    {
                        if ((_전치사구동반한명사구단수여부 == 1 && 다음어절검색결과.Contains("s형")) || (_전치사구동반한명사구단수여부 == 0 && !다음어절검색결과.Contains("s형")) || (_전치사구동반한명사구단수여부 == 2))
                        {
                            _결과 += "ⓢ{" + _주어임시저장소.Trim() + " " + _현재 + "}}";

                            _부사구_전치사구시작 = false;
                            _전치사구동반한명사구시작 = false;
                            _주어임시저장소 = "";
                            _주어나옴 = true;
                            _처리결과기호 = "이거는 주어뿐인 거 같은데 뭔지 모르겠음";
                            return true;
                        }

                    }
                }



                if (현재어절검색결과.Contains("vi.") && 현재어절검색결과.Contains("vt."))
                {
                    _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";

                    _주어나옴 = true;
                    _서술어나옴 = true;
                    _타동사나옴 = true;
                    _주어임시저장소 = "";

                    _처리결과기호 = "서술어";
                    return true;
                }
                else if (현재어절검색결과.Contains("vi."))
                {
                    _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";

                    _주어나옴 = true;
                    _서술어나옴 = true;
                    _자동사나옴 = true;
                    _주어임시저장소 = "";

                    _처리결과기호 = "서술어";
                    return true;

                }

                else if (현재어절검색결과.Contains("vt."))
                {
                    _결과 += "ⓢ{" + _주어임시저장소 + "} " + "ⓥ{" + _현재 + "}";

                    _주어나옴 = true;
                    _서술어나옴 = true;
                    _자동사나옴 = true;
                    _타동사나옴 = true;
                    _주어임시저장소 = "";

                    _처리결과기호 = "서술어";
                    return true;
                }
            }

            return false;
        }
        private bool 복수형_구_가능성(string 이전어절모두)
        {
            List<string> 어절들 = new List<string>();

            변환.문자열.어절들로(이전어절모두, ref 어절들);

            string 이전어절 = "";
            string 현재어절 = "";

            for (int i = 0; i < 어절들.Count; i++)
            {
                if (i != 0) 이전어절 = 어절들[i - 1];
                현재어절 = 어절들[i];


                if (복수형명사가능성(이전어절) && 전치사가능성(현재어절))
                    return true;
            }

            return false;
        }
        public bool 복수형명사가능성(string 현재어절)
        {
            if (현재어절.ToLower() == "you") return true;
            if (현재어절.ToLower() == "we") return true;

            if (현재어절.ToLower() == "they") return true;
            if (현재어절.ToLower() == "these") return true;

            if (현재어절.ToLower() == "some") return true;
            if (현재어절.ToLower() == "many") return true;

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            if (현재어절검색결과.Contains("의 복수")) return true;
            if (현재어절검색결과.Contains("s형")) return true;
            if (현재어절검색결과.Contains("pl.")) return true;


            return false;
        }
        private bool S있고_감각동사확정()
        {
            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);

            // if(_조동사서술어시작 == true) return false; // 조동사가 시작된 다음에는 자체 확장 가능성 함수를 씁니다.

            if (_주어나옴 != true) return false; // 주어가 나오지 않은 상태에서 서술어가 먼저 나올 수 없다. (일반적으로)

            if (_서술어나옴 == true) return false; // 문장의 서술어가 둘 일 수 없다.

            if (((_현재 == "feel") || (_현재 == "sound")) && 다음어절검색결과.Contains("a.") &&
                ((_2다음 == "") || 접속어(_2다음) || 전치사가능성(_2다음)))
            {
                // They sound identical to the original. 에서, 전치사 to도 포함해야 한다.

                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_조동사서술어시작 == true)
                {
                    _결과 += _현재 + "}";
                    _조동사서술어시작 = false;
                }
                else
                    _결과 += "ⓥ{" + _현재 + "}";


                _처리결과기호 = "감각동사";
                return true;
            }

            return false;
        }
        public bool 접속어(string 접속어후보)
        {
            접속어후보 = 접속어후보.ToLower();
            접속어후보 = 접속어후보.불필요제거();

            if (접속어후보 == "as") return true;
            if (접속어후보 == "before") return true;

            if (접속어후보 == "and") return true;
            if (접속어후보 == "if") return true;

            if (접속어후보 == "for") return true;

            if (접속어후보 == "or") return true;
            if (접속어후보 == "since") return true;
            if (접속어후보 == "so") return true;

            if (접속어후보 == "but") return true;

            if (접속어후보 == "until") return true;
            if (접속어후보 == "when") return true;

            if (접속어후보 == "however") return true;

            string 접속어후보검색결과 = Form1._검색.영한사전_문장부호제거(접속어후보);
            if (접속어후보검색결과.Contains("conj.")) return true;


            return false;
        }

        private bool 전치사가능성(string 전치사후보)
        {
            // 검색에도 똑같은 클래스 있음
            전치사후보 = 전치사후보.ToLower();

            string[] 전치사목록 = { "beyond", "above", "over", "up", "down", "underneath", "beneath", "under", "below",     //위치를 나타내는 전치사
                "as", "than", "like",                                                                                       //비교, 동등을 나타내는 전치사
                "at", "in", "inside", "within", "between", "among", "amid", "outside", "around", "about", "on",             //공간의 소속과 관련된 전치사
                "with", "of",  "without",                                                                                   //관계를 나타내는 전치사
                "off",                                                                                                      //분리와 관련된 전치사
                "for", "toward", "to", "onto", "into", "from", "out of", "away from",                                       //방향과 관련된 전치사
                "before", "in front of", "ahead of", "after", "behind", "in back of",                                       //앞뒤와 관련된 전치사
                "by",
                "during",                                                                                                   //기간을 나타내는 전치사
                "against",                                                                                                  //대항을 나타내는 전치사
                "through", "throughout",                                                                                    //관통을 나타내는 전치사
				"despite",																									//양보
                };

            foreach (string 현재전치사 in 전치사목록)
            {
                if (전치사후보 == 현재전치사)
                    return true;
            }


            return false;
        }

		private bool 전치사구가능성(string 전치사구후보)
		{
			if (전치사구후보.ToLower().IndexOf("due to") == 0)
				return true;

			if (전치사구후보.ToLower().IndexOf("because of") == 0)
				return true;

			return false;
		}
			private bool 부사구아닌전치사(string 전치사후보)
        {
            전치사후보 = 전치사후보.ToLower();

            if (전치사후보 == "to") return true;
            if (전치사후보 == "of") return true;

            return false;
        }
        private bool 전치사_to와of빼고(string 전치사후보)
        {
            //검색 클래스에도 똑같은 함수 있음.

            전치사후보 = 전치사후보.ToLower();
            if (전치사후보 == "to") return false;
            if (전치사후보 == "of") return false;


            if (전치사가능성(전치사후보)) return true;

            return false;
        }
        private bool 부사구_전치사구종료가능성()
        {
            if (_부사구_전치사구시작 == false) return false; // 시작도 안한 걸 종료할 수 없다.


            if ((변환.문자열.Right(_현재, 1) == ",") && (_주어임시저장소 == "")) // 밑에서는 주어임시저장소에 관련된 언급이 있길래 써봤다.
            {
                // 예외가 많이 있겠지만, 그 때 그 때 고쳐나갈 것이다.

                _부사구_전치사구시작 = false;
                _결과 += _현재.중괄호붙이기(1);
                _처리결과기호 = "부사구_전치사구종료가능성";
                return true;
            }

            if ((변환.문자열.Right(this._현재, 1) == ",") && !_현재값.Contains("사람이름") && _다음값.Contains("사람이름")) // 밑에서는 주어임시저장소에 관련된 언급이 있길래 써봤다.
            {
                // 예외가 많이 있겠지만, 그 때 그 때 고쳐나갈 것이다.

                _부사구_전치사구시작 = false;


                _결과 += this._현재.중괄호붙이기(1);


                _처리결과기호 = "부사구_전치사구종료가능성";
                return true;
            }

            if ((변환.문자열.Right(_현재, 1) == ".") || _마지막 == true)
            {
                _부사구_전치사구시작 = false;

                _결과 += _주어임시저장소 + " " + 현재내용에중괄호붙이기(_현재);
				for (int i = 0; i < _중첩형용사구갯수; i++) { _결과 = _결과.중괄호붙이기(1);  }

				if(_to부정사부사적용법시작) // To 부정사의 부사적 용법이 끝났는지 확인하는 과정이 먼저 있으므로, 이리로 안 올 수 있다. 하지만 넣어둔다.
					_결과 = _결과.중괄호붙이기(1);

				_중첩형용사구갯수 = 0;

                return true;
            }
            if (_전치사구동반한명사구시작 == true)
            {
                string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);

                if ((다음어절검색결과.Contains("vi.") || 다음어절검색결과.Contains("vt.")) && !다음어절검색결과.Contains("ing형") && (_주어임시저장소 != ""))
                {

                    if ((_전치사구동반한명사구단수여부 == 1 && 다음어절검색결과.Contains("s형")) || (_전치사구동반한명사구단수여부 == 0 && !다음어절검색결과.Contains("s형")) || (_전치사구동반한명사구단수여부 == 2))
                    {
                        _결과 += "ⓢ{" + _주어임시저장소.Trim() + " " + _현재 + "}}";

                        _부사구_전치사구시작 = false;
                        _전치사구동반한명사구시작 = false;
                        _주어임시저장소 = "";
                        _주어나옴 = true;

                        return true;
                    }

                }

            }
            return false;
        }

        private bool 부사종료확정가능성()
        {
            if (_이전 == "the") return false;  // 당연히 안되지 무슨 관사 뒤에 부사가 오냐고 ㅋ
            if (_이전 == "a") return false;
            if (_이전 == "an") return false;
            if (전치사가능성(_이전)) return false;


            if (_목적어나옴 != true) // 목적어가 나왔다면 거의 수식어 나온다고 봐야 함.
            {
                if (_현재값.Contains("n.")) return false;
                if (_현재값.Contains("a.")) return false;
                if (_현재값.Contains("vi.")) return false;
                if (_현재값.Contains("vt.")) return false;
                if (_현재값.Contains("conj.")) return false;
            }

            if (_마지막 == true && _현재값.Contains("ad."))
            {
                if (_to부정사목적어구시작)
                {
                    if (_목적어구시작 == true || _보어구시작 == true)
                    {
                        _결과 = _결과.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(2);
                    }
                    else if (_부사구_전치사구시작 == true)
                    {
                        if (전치사가능성(_이전)) return false; // 전치사 구가 시작되고도 물론, 독립적인 부사가 올 수는 있는데, 전치사 바로 다음에 부사는 좀 아니다. 예컨대 at home.

                        _결과 = _결과.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(2);
                    }
                    else
                    {
                        _결과 = _결과.Trim();

                        if (_주어임시저장소 != "")
                            _결과 += _주어임시저장소;

                        _결과 += " ⓧ{" + _현재B.중괄호붙이기(2);
                    }
                }
                else if (_목적어구시작 == true || _보어구시작 == true)
                {
                    if (_목적어임시저장소 != "")
                        _결과 += "ⓞ{" + _목적어임시저장소.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(1);
                    else if (_보어임시저장소 != "")
                        _결과 += "ⓒ{" + _보어임시저장소.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(1);
                }
                else if (_부사구_전치사구시작 == true)
                {
                    if (전치사가능성(_이전)) return false; // 전치사 구가 시작되고도 물론, 독립적인 부사가 올 수는 있는데, 전치사 바로 다음에 부사는 좀 아니다. 예컨대 at home.

                    _결과 = _결과.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(1);
                }
                else
                {
                    _결과 = _결과.Trim();

                    if (_주어임시저장소 != "")
                        _결과 += _주어임시저장소;

                    _결과 += " ⓧ{" + _현재B.중괄호붙이기(1);
                }
                _처리결과기호 = "부사종료확정가능성";
                return true;
            }

            return false;
        }

		// 근데 아마 이 부분도 나중에 엄청 말썽 일으킬 거다. 문제 생길 때마다 고치면 된다.
		// 기운내라 미래의 용남아 ㅋㅋㅋㅋ 니가 나보다 나이 많아봐야 어쩔거임?
		// He can jump high. 이 문장만 해결하려고 만든거다.
        private bool 부사종료확정가능성_마지막() // 이판사판 왠만하면 부사로 집어 넣어야 하는 경우.
        {
            if (_이전 == "the") return false;  // 당연히 안되지 무슨 관사 뒤에 부사가 오냐고, 근데 여기까지 와서는 이런 경우가 없을 것이다.
            if (_이전 == "a") return false;
            if (_이전 == "an") return false;
            if (전치사가능성(_이전)) return false;


			// 왠만하면 낑궈 넣어준다. 왠만하면,
            if (_마지막 == true && _현재값.Contains("ad."))
            {
                if (_to부정사목적어구시작)
                {
                    if (_목적어구시작 == true || _보어구시작 == true)
                    {
                        _결과 = _결과.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(2);
                    }
                    else if (_부사구_전치사구시작 == true)
                    {
                        if (전치사가능성(_이전)) return false; // 전치사 구가 시작되고도 물론, 독립적인 부사가 올 수는 있는데, 전치사 바로 다음에 부사는 좀 아니다. 예컨대 at home.

                        _결과 = _결과.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(2);
                    }
                    else
                    {
                        _결과 = _결과.Trim();

                        if (_주어임시저장소 != "")
                            _결과 += _주어임시저장소;

                        _결과 += " ⓧ{" + _현재B.중괄호붙이기(2);
                    }
                }
                else if (_목적어구시작 == true || _보어구시작 == true)
                {
                    if (_목적어임시저장소 != "")
                        _결과 += "ⓞ{" + _목적어임시저장소.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(1);
                    else if (_보어임시저장소 != "")
                        _결과 += "ⓒ{" + _보어임시저장소.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(1);
                }
                else if (_부사구_전치사구시작 == true)
                {
                    if (전치사가능성(_이전)) return false; // 전치사 구가 시작되고도 물론, 독립적인 부사가 올 수는 있는데, 전치사 바로 다음에 부사는 좀 아니다. 예컨대 at home.

                    _결과 = _결과.Trim().중괄호붙이기(1) + " " + "ⓧ{" + _현재B.중괄호붙이기(1);
                }
                else
                {
                    _결과 = _결과.Trim();

                    if (_주어임시저장소 != "")
                        _결과 += _주어임시저장소;

                    _결과 += " ⓧ{" + _현재B.중괄호붙이기(1);
                }
                _처리결과기호 = "부사종료확정가능성";
                return true;
            }

            return false;
        }

		private void 전치사구시작가능성_내부처리()
		{
			if (_목적어구시작 == true)
			{
				if (_현재 == "for")
				{
					_목적어임시저장소 += "for "; // 이게 이렇게만 써줘도 목적어구를 내부에서 분석하므로 알아서 for를 형용사구 처리를 한다.
				}
				else
				{
					_결과 += "ⓞ{" + _목적어임시저장소.Trim() + "} ⓧ{" + _현재;
					_목적어임시저장소 = "";

					_목적어구시작 = false;
					_부사구_전치사구시작 = true;
				}
			}
			else if (_보어구시작 == true && _보어임시저장소 != "")
			{
				if (_현재 == "for")
				{
					_보어임시저장소 += "for"; // 이것도 이렇게만 써줘도 보어구를 내부에서 알아서 분석한다.
				}
				else
				{
					_결과 += "ⓒ{" + _보어임시저장소.Trim() + "} ⓧ{" + _현재;
					_보어임시저장소 = "";

					_보어구시작 = false;
					_부사구_전치사구시작 = true;
				}
			}
			else if (_직접목적어시작 == true)
			{
				_결과 = _결과.Trim() + "} ⓧ{" + _현재;

				_직접목적어시작 = false;
				_부사구_전치사구시작 = true;
			}
			else if (_부사구_전치사구시작 == true)
			{
				if (_이전.ToLower() == "journey" && _현재.ToLower() == "through")
				{
					_중첩형용사구갯수++;

					_결과 = _결과.Trim();
					_결과 += " ⓧ{" + _현재;
				}
				else
				{
					_결과 = _결과.Trim();
					for (int i = 0; i < _중첩형용사구갯수; i++) { _결과 += "}"; _중첩형용사구갯수 = 0; }

					_결과 += "} ⓧ{" + _현재;

					_부사구_전치사구시작 = true;
				}
			}
			else
			{
				_결과 = _결과.Trim();
				_결과 += " ⓧ{" + _현재;
				_부사구_전치사구시작 = true;
			}
			_처리결과기호 = "부사구형용사구_전치사구시작가능성";
		}


		private bool 전치사구시작가능성()
        {
            // if (_부사구_전치사구시작) return false; // 부사구가 나와도 또 부사구가 나올 수 있으므로 이 코드는 넣어서는 안된다.

            _2다음 = 변환.문자열.문장부호제거(_2다음);

		
			if(전치사구가능성(_현재 + " " + _다음어절모두)) // because of와 같은 것들
			{
				/*
					_결과 += "ⓧ{" + _현재;
					_부사구_전치사구시작 = true;
					_처리결과기호 = "부사구형용사구_전치사구시작가능성";
					return true;
					*/
				전치사구시작가능성_내부처리();
				return true;
			}

			if(_다음소문자 == "to")	// 부사이기도 하지만 형용사인 것들도 분명히 있다. 그 뒤에 to 부정사가 나온다면 당연히 전치사구가 시작되는 것은 아니다.
			{
				return false;
			}

            if ((_이전값.Contains("2형식동사") && _현재 == "like") //The actors sounded like they were reading from a book.
			|| (_이전값.Contains("2형식동사") && _현재 == "out" && _다음 == "of" )
			)
            {
                return false;
            }

			if(_이전 == "hours" && _현재 == "of") // hours 다음에 of 나왔을 때, of 뒤를 지우면 안된다.
			{
				return false;
			}

			if (_이전 == "due" && _현재 == "to") // due to는 이미 due에서 전치사구로 시작을 했다.
			{
				return false;
			}


			if (_처리결과기호 == "시작" || _처리결과기호 == "ThereBe의주어종료")
            {
                if (전치사_to와of빼고(_현재))
                {
                    _결과 += "ⓧ{" + _현재;
                    _부사구_전치사구시작 = true;
                    _전치사구동반한명사구시작 = true;
                    _처리결과기호 = "부사구형용사구_전치사구시작가능성";
                    return true;
                }
            }

            if (_서술어나옴 == true || _HowAbout_WhatAbout시작 == true)
            {
                if ((전치사_to와of빼고(_현재) && !부사구아닌전치사(_현재))
                    || ((_현재 == "all") && (_다음소문자 == "morning"))
                    || ((_현재 == "every") && (_다음소문자 == "day"))
                    || ((_현재 == "every") && (_다음소문자 == "night"))
                    || ((_현재 == "every") && (_다음소문자 == "morning"))
                    || ((_현재 == "every") && (_다음소문자 == "spring"))
                    || ((_현재 == "every") && (_다음소문자 == "summer"))
                    || ((_현재 == "every") && (_다음소문자 == "fall"))
                    || ((_현재 == "every") && (_다음소문자 == "autumn"))
                    || ((_현재 == "every") && (_다음소문자 == "winter"))

                    || ((_현재 == "out") && (_다음소문자 == "of"))
                    || ((_현재 == "the") && (_다음소문자 == "next") && (_2다음 == "morning"))

                    || ((_현재 == "this") && (_다음소문자 == "night"))
                    || ((_현재 == "this") && (_다음소문자 == "morning"))
                    || ((_현재 == "this") && (_다음소문자 == "spring"))
                    || ((_현재 == "this") && (_다음소문자 == "summer"))
                    || ((_현재 == "this") && (_다음소문자 == "fall"))
                    || ((_현재 == "this") && (_다음소문자 == "autumn"))
                    || ((_현재 == "this") && (_다음소문자 == "winter"))
                    || ((_현재 == "this") && (_다음소문자 == "weekend"))

                    || ((_현재 == "last") && (_다음소문자 == "weekend"))

                    || ((_현재 == "next") && (_다음소문자 == "time"))             )
                {

					전치사구시작가능성_내부처리();



                    return true;
                }
                else if (_현재 == "to")
                {
                    //string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(다음어절);

                    // to 부정사구는 해당되지 않습니다.
                    if (_다음값.Contains("vi.") || _다음값.Contains("vt.") &&
                        !_다음값.Contains("과거형") && _다음값.Contains("ed형") && _다음값.Contains("pp형") && _다음값.Contains("ing형"))
                        return false;

                    if (_이전 == "trip" || _이전 == "travel" || _이전 == "visit")
                    {

                        if (_목적어임시저장소 == "")
                        {
                            _중첩형용사구갯수++;

                            _결과 = _결과.Trim();
                            _결과 += " ⓧ{" + _현재;
                        }
                        else
                        {
                            _목적어임시저장소 = _목적어임시저장소.Trim();
                            _목적어임시저장소 += " " + _현재;
                        }
                    }
                    else if (_목적어구시작 == true)
                    {
                        _결과 += "ⓞ{" + _목적어임시저장소.Trim() + "} ⓧ{" + _현재;
                        _목적어임시저장소 = "";

                        _목적어구시작 = false;
                        _부사구_전치사구시작 = true;
                    }
                    else if (_보어구시작 == true)
                    {
                        _결과 += "ⓒ{" + _보어임시저장소.Trim() + "} ⓧ{" + _현재;
                        _보어임시저장소 = "";

                        _보어구시작 = false;
                        _부사구_전치사구시작 = true;
                    }
                    else if (_직접목적어시작 == true)
                    {
                        _결과 = _결과.Trim();
                        _결과 += "} ⓧ{" + _현재;

                        _직접목적어시작 = false;
                        _부사구_전치사구시작 = true;
                    }
                    else if (_부사구_전치사구시작 == true)
                    {
                        _결과 = _결과.Trim();
                        for (int i = 0; i < _중첩형용사구갯수; i++) { _결과 += "}"; _중첩형용사구갯수 = 0; }

                        _결과 += "} ⓧ{" + _현재;

                        _부사구_전치사구시작 = true;
                    }

                    else
                    {
                        _결과 = _결과.Trim();
                        _결과 += " ⓧ{" + _현재;
                        _부사구_전치사구시작 = true;
                    }
                    _처리결과기호 = "부사구형용사구_전치사구시작가능성";

                    return true;
                }
            }
            else
            {
                if ((전치사_to와of빼고(_현재) && (_주어임시저장소 != "") && _이전값.Contains("n."))
                    || (_현재 == "to" && !_다음값.Contains("vi.") && !_다음값.Contains("vt.")))
                {
                    _주어임시저장소 = _주어임시저장소.Trim();
                    _주어임시저장소 += " ⓧ{" + _현재;

                    _부사구_전치사구시작 = true;
                    _전치사구동반한명사구시작 = true;
                    if (_이전값.Contains("복수형") || _이전값.Contains("s형") || _이전값.Contains("pl."))
                        _전치사구동반한명사구단수여부 = 0;
                    else if (단복수공통명사(_이전))
                        _전치사구동반한명사구단수여부 = 2;
                    else
                        _전치사구동반한명사구단수여부 = 1;

                    _처리결과기호 = "부사구형용사구_전치사구시작가능성";

                    return true;
                }
            }

            return false;
        }
        private bool 단복수공통명사(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            if (현재어절 == "all") return true;
            if (현재어절 == "some") return true;

            return false;
        }
        private bool 수식어시작_확정가능성()
        {
            if (_부사구_전치사구시작) return false;
            if (_목적어임시저장소 != "") return false;

            // 수식어 확정
            if ((_1형식동사 && 부사가능성(this._현재) && _마지막) ||
                (_현재값.Contains("사람이름") && _마지막))
            {
                _결과 += "ⓧ{" + _현재.중괄호붙이기(1);
                return true;
            }

            return false;
        }
        private bool 주어가아직안나왔는데끝까지간경우()
        {
            // 딱히 파싱할 것은 없고, 그냥 토해냅니다.

            if (_주어나옴 == false && _마지막 == true)
            {
                _결과 += _주어임시저장소 + " " + _현재;

                _처리결과기호 = "주어가아직안나왔는데끝까지간경우";
                return true;
            }

            return false;
        }
        private bool 주어가아직안나왔을때주어후보로만들기()
        {
            //(_부사구_전치사구시작 == true && _주어임시저장소 == "") : "For example," "these kinds {of money"
            if (_주어나옴 == true || _서술어나옴 == true || _마지막 == true || _to부정사부사적용법시작 == true || (_부사구_전치사구시작 == true && _주어임시저장소 == "") || _처리결과기호 == "ThereBe의주어종료")
                return false;

            _주어임시저장소 += " " + _현재B;

            _주어임시저장소 = _주어임시저장소.Trim();

            _처리결과기호 = "주어임시저장";

            return true;
        }
        private bool 주어확정가능성()
        {
            if (_주어나옴 == true) return false; // 문장의 주어는 2개일 수가 없다.
            if (_서술어나옴 == true && !_의문문) return false; // 서술어나왔으면 주어는 안나온다. (도치구문을 제외하고는)
            if (_to부정사부사적용법시작 == true) return false; // to부정사 나왔다니까! 무슨 주어야 주어가.
            if (_to부정사서술어나옴 == true) return false;// to부정사 나왔다니까! 무슨 주어야 주어가.



            string 현재어절소문자 = _현재.ToLower(); // 대소문자 모두 확인합니다.

            if (현재어절소문자 == "i" || 현재어절소문자 == "you" || 현재어절소문자 == "he" || 현재어절소문자 == "she" || 현재어절소문자 == "it" ||
                현재어절소문자 == "we" || 현재어절소문자 == "they" || 현재어절소문자 == "someone"

                )
            {
                _주어나옴 = true;
                if (_주어임시저장소 != "")
                {
                    if (변환.문자열.Right(_주어임시저장소, 1) == ".") { _결과 += "ⓧ{" + _주어임시저장소.Substring(0, _주어임시저장소.Length - 1) + "}. ⓢ{" + _현재 + "}"; }
                    else if (변환.문자열.Right(_주어임시저장소, 1) == ",") { _결과 += "ⓧ{" + _주어임시저장소.Substring(0, _주어임시저장소.Length - 1) + "}, ⓢ{" + _현재 + "}"; }
					else if((_이전.ToLower() == "of" && _현재.ToLower() == "it") || (_2이전.ToLower() == "some" && _이전.ToLower() == "of" && _현재.ToLower() == "you"))
					{
						_결과 += "ⓢ{" + _주어임시저장소 + " " + _현재 + "}";
					}
                    else _결과 += "ⓧ{" + _주어임시저장소 + "} ⓢ{" + _현재 + "}";

                    _주어임시저장소 = "";
                }
                else
                    _결과 += "ⓢ{" + _현재 + "}";


                _처리결과기호 = "주어확정가능성";
                return true;
            }
			else if(현재어절소문자 == "that" && _다음값.Contains("2형식동사") && _다음값.Contains("s형")) // 이 부분이 없으면 That sounds fun에서, That sounds를 주어로 묶는 경우가 생긴다.
			{ 
				_주어나옴 = true;

				_결과 += "ⓢ{" + _현재 + "}";

				_처리결과기호 = "주어확정가능성";
				return true;
			}

			return false;
        }
        private bool S_Be동사()
        {
            _현재 = _현재.Replace("’", "'");

            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);

            if (_주어나옴 == true) return false; // 이미 주어가 나왔으므로, 또 주어가 나올 이유가 없다.

            // I'm -------------------------------------------------------------------------------
            if ((_현재 == "I'm") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;	_서술어나옴 = true;	_자동사나옴 = true;	if (_주어임시저장소 == "")	_결과 += "ⓢ{I}ⓥ{'m}";	else	_결과 += "ⓧ{" + _주어임시저장소.중괄호붙이기(1) + " ⓢ{I}ⓥ{'m}";
                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }
            else if ((_현재 == "I'm") && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{I}ⓥ{'m}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{I}ⓥ{'m}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // You're -------------------------------------------------------------------------------
            if ((_현재 == "You're") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{You}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{You}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "You're")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{You}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{You}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // you're -------------------------------------------------------------------------------
            if ((_현재 == "you're") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{you}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{you}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "you're")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{you}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{you}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // He's -------------------------------------------------------------------------------
            if ((_현재 == "He's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{He}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{He}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }



            if ((_현재 == "He's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{He}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{He}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // he's -------------------------------------------------------------------------------
            if ((_현재 == "he's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{he}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{he}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "he's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{he}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{he}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // She's -------------------------------------------------------------------------------
            if ((_현재 == "She's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{She}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{She}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "She's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{She}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{She}ⓥ{'s}";

                _주어임시저장소 = "";


                _처리결과기호 = "주어Be동사";
                return true;
            }

            // she's -------------------------------------------------------------------------------
            if ((_현재 == "she's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{she}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{she}ⓥ{'s}";

                _주어임시저장소 = "";


                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "she's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{she}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{she}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }



            // That's -------------------------------------------------------------------------------
            if ((_현재 == "That's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{That}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{That}ⓥ{'s}";

                _주어임시저장소 = "";


                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "That's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{That}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{That}ⓥ{'s}";

                _주어임시저장소 = "";


                _처리결과기호 = "주어Be동사";
                return true;
            }

            // that's -------------------------------------------------------------------------------
            if ((_현재 == "that's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{that}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{that}ⓥ{'s}";

                _주어임시저장소 = "";


                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "that's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{that}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{that}ⓥ{'s}";

                _주어임시저장소 = "";


                _처리결과기호 = "주어Be동사";
                return true;
            }

            // We're -------------------------------------------------------------------------------
            if ((_현재 == "We're") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{We}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{We}ⓥ{'re}";

                _주어임시저장소 = "";


                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "We're")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{We}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{We}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }
            // we're -------------------------------------------------------------------------------
            if ((_현재 == "we're") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{we}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{we}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "we're")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{we}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{we}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // They're -------------------------------------------------------------------------------
            if ((_현재 == "They're") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{They}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{They}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "They're")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{They}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{They}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }
            // they're -------------------------------------------------------------------------------
            if ((_현재 == "they're") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{they}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{they}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "they're")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{they}ⓥ{'re}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{they}ⓥ{'re}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // It's -------------------------------------------------------------------------------
            if ((_현재 == "It's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{It}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{It}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "It's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{It}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{It}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            // It's -------------------------------------------------------------------------------
            if ((_현재 == "it's") && ((_다음 == "the") || 형용사가능성(_다음)))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{it}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{it}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }

            if ((_현재 == "it's")
                && !다음어절검색결과.Contains("ing형"))
            {
                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (_주어임시저장소 == "")
                    _결과 += "ⓢ{it}ⓥ{'s}";
                else
                    _결과 += "ⓧ{" + 현재내용에중괄호붙이기(_주어임시저장소) + " ⓢ{it}ⓥ{'s}";

                _주어임시저장소 = "";

                _처리결과기호 = "주어Be동사";
                return true;
            }
            return false;
        }
        public bool S_ll확정()
        {

            _현재 = _현재.Replace("’", "'");
            string 현재어절소문자 = _현재.ToLower();

            if ((현재어절소문자 == "i'll") || (현재어절소문자 == "you'll") || (현재어절소문자 == "he'll") || (현재어절소문자 == "she'll")
                 || (현재어절소문자 == "we'll") || (현재어절소문자 == "they'll"))
            {
                _주어나옴 = true;
                _조동사서술어시작 = true;
                _처리결과기호 = "주어LL확정가능성";
                _결과 += 주어LL표시(_현재);
                return true;
            }
            return false;
        }
        public bool 주어VE확정가능성()
        {
            if (!PP형(_다음)) return false;

            _현재 = _현재.Replace("’", "'");
            string 현재어절소문자 = _현재.ToLower();
            if ((현재어절소문자 == "i've") || (현재어절소문자 == "you've") || (현재어절소문자 == "he's") || (현재어절소문자 == "she's")
                 || (현재어절소문자 == "we've") || (현재어절소문자 == "they've"))
            {

                _주어나옴 = true;
                _havePP시작 = true;
                _조동사서술어시작 = true;
                _처리결과기호 = "주어VE확정가능성";
                _결과 += 주어VE표시(_현재);
                return true;
            }
            return false;

        }
        public string 주어LL표시(string 주어LL후보)
        {
            // 
            string 주어 = 변환.문자열.Left(주어LL후보, 주어LL후보.Length - 3);

            string 결과 = "ⓢ{" + 주어 + "}ⓥ{ⓧ{'ll}";
            return 결과;
        }
        public string 주어VE표시(string 주어VE후보)
        {
            // 
            string 주어 = 변환.문자열.Left(주어VE후보, 주어VE후보.Length - 3);

            string 결과 = "ⓢ{" + 주어 + "}ⓥ{ⓧ{'ve}";

            결과 = 결과.Replace("Sh}ⓥ{ⓧ{'ve", "She}ⓥ{ⓧ{'s");
            결과 = 결과.Replace("sh}ⓥ{ⓧ{'ve", "she}ⓥ{ⓧ{'s");

            결과 = 결과.Replace("H}ⓥ{ⓧ{'ve", "He}ⓥ{ⓧ{'s");
            결과 = 결과.Replace("h}ⓥ{ⓧ{'ve", "he}ⓥ{ⓧ{'s");



            return 결과;
        }
        public bool 형용사가능성(string 형용사후보)
        {
            string 검색결과 = Form1._검색.영한사전_문장부호제거(형용사후보);

            if (검색결과.Contains("a."))
            {
                return true;
            }

            return false;
        }
        private bool 명령어수여동사출현()
        {
            if (_처리결과기호 != "시작") return false;
			if(_구문분석내_구문분석 == true) return false; 

            string 현재어절소문자 = _현재.ToLower();


            if (현재어절소문자 == "give")
            {
                _결과 += string.Format("ⓥ{{{0}}}", _현재);
                _수여동사나옴 = true;

                _처리결과기호 = "명령어수여동사";

                return true;
            }

            return false;
        }
        private bool 간접목적어확정가능성()
        {
            string 현재어절소문자 = "";

            if (_현재 != "US")
                현재어절소문자 = _현재.ToLower();

            if (_수여동사나옴 == true &&
                (현재어절소문자 == "me" || 현재어절소문자 == "you" || 현재어절소문자 == "him" || 현재어절소문자 == "her"
                || 현재어절소문자 == "us" || 현재어절소문자 == "them")
                && (!전치사_to와of빼고(_다음) || !접속어(_다음))
                )
            {
                _간접목적어나옴 = true;

                _결과 += string.Format("ⓘ{{{0}}}", _현재);
                _처리결과기호 = "간접목적어확정가능성";
                return true;
            }

            return false;
        }
        private bool ShouldHavePP형태()
        {
            if (_현재 != "should") return false;
            if (_다음 != "have") return false;
            if (!PP형(_2다음)) return false;

            int 형식 = 0;

            if (이어동사_자동사_PP형가능성(_2다음, _3다음, _4다음, ref 형식))
            {
                _결과 += "ⓥ{ⓧ{should} have " + _다음2B + " " + _3다음.중괄호붙이기(1);
                _자동사나옴 = true;

                _i += 3;
                if (형식 == 1) _1형식동사 = true;
                if (형식 == 2) _2형식동사 = true;

                return true;
            }
            if (이어동사_타동사_PP형가능성(_2다음, _3다음, _4다음))
            {
                _결과 += "ⓥ{ⓧ{should} have " + _다음2B + " " + _3다음.중괄호붙이기(1);
                _타동사나옴 = true;
                _i += 3;

                return true;
            }


            if (_2다음검색값.Contains("vi.")) _자동사나옴 = true;
            if (_2다음검색값.Contains("vt.")) _타동사나옴 = true;

            if (_3다음검색값.Contains("n.")) _자동사나옴 = false; // 뒤에 명사나 명사구 나오면 거의 
            if (관사(_3다음)) _자동사나옴 = false;

            _서술어나옴 = true;

            _결과 += "ⓥ{ⓧ{should} have " + 현재내용에중괄호붙이기(_2다음);

            _처리결과기호 = "ShouldHavePP";
            _i += 2;




            return true;
        }
        private bool 명령어서술어확정()
        {
            if (_주어나옴 == true) return false;
            if (_서술어나옴 == true) return false;
            if (_주어임시저장소 != "") return false; // 뭔가가 있는 상태면 일단 그게 주어가 아닌지 의심할 필요는 있다.
            if (Be동사(_다음)) return false; // 다음 어절이 Be동사이면 당연히 이게 명령문은 아니다. ⓥ{English} ⓞ{is a door} ⓧ{to a big world}. English에 동사가 있다니!
			if(_구문분석내_구문분석 == true) return false; //he had no loose change에서 loose change를 다시 분석하는 문제가 발생했다.

			if(_처리결과기호 == "시작" && _현재값.Contains("vt.") && !_현재값.Contains("vi.") && (_다음값.Contains("vi.") || _다음값.Contains("vt.")) && !_다음값.Contains("n.") && !_다음값.Contains("ing형"))
			{
				return false;
			}

			// 첫 단어가 대문자이지만, 그 다음 단어도 대문자이고, 둘 다 이름과 관계되어 있으면, 동사가 아닐 가능성이 매우 높습니다.
			// Sue Jin's toes became out of shape ...
			else if(_현재.대문자인지확인_첫글자만() && _다음.대문자인지확인_첫글자만() && _현재값.Contains("이름") && _다음값.Contains("이름") ||
			_현재.대문자인지확인_첫글자만() && _다음.대문자인지확인_첫글자만() && _현재값.Contains("이름") && _다음값 == "")
			{
				return false;
			}

			else if (_처리결과기호 == "시작" && (_현재값.Contains("vt.") || _현재값.Contains("vi.")) && !_현재값.Contains("s형") && !_현재값.Contains("ing형")
                && !_다음값.Contains("과거형") && !_다음값.Contains("s형"))
            {
                // 사전에서 맨처음 나오는 것이 자동사이거나 타동사이면 평소에 동사로 많이 쓰이는 표현이라는 뜻이므로,
                // 명령형일 가능성이 높습니다.

                // 그 다음 어절이 과거형이나, s형이면 어느쪽이 서술어인지 가늠하기 어렵습니다.


                if (_현재값.Contains("vi.")) _자동사나옴 = true;
                if (_현재값.Contains("vt.")) _타동사나옴 = true;

                _서술어나옴 = true;

                _결과 += "ⓥ{" + 현재내용에중괄호붙이기(_현재);

                _처리결과기호 = "명령어서술어";

                return true;
            }
            else if (_처리결과기호 == "시작" && (_현재값.Contains("vt.") || _현재값.Contains("vi.")) && (_현재값.Contains("명사로잘쓰이지않음") || !명사가능성(_현재)))
            {
                if (_현재값.Contains("vi.")) _자동사나옴 = true;
                if (_현재값.Contains("vt.")) _타동사나옴 = true;

                if (_다음값.Contains("n.")) _자동사나옴 = false; // 뒤에 명사나 명사구 나오면 거의 
                if (관사(_다음)) _자동사나옴 = false;

                _서술어나옴 = true;

                _결과 += "ⓥ{" + 현재내용에중괄호붙이기(_현재);

                _처리결과기호 = "명령어서술어";

                return true;
            }
            else if ((_처리결과기호 != "시작") && (_주어임시저장소 == "") && (_처리결과기호 == "감탄사_단일부사_기타단일어구")
                && (_현재값.Contains("vt.") || _현재값.Contains("vi.")) && !_현재값.Contains("s형") && !_현재값.Contains("ing형")
                && !_다음값.Contains("과거형") && !_다음값.Contains("s형"))
            {
                // 사전에서 맨처음 나오는 것이 자동사이거나 타동사이면 평소에 동사로 많이 쓰이는 표현이라는 뜻이므로,
                // 명령형일 가능성이 높습니다.

                // 그 다음 어절이 과거형이나, s형이면 어느쪽이 서술어인지 가늠하기 어렵습니다.


                if (_현재값.Contains("vi.")) _자동사나옴 = true;
                if (_현재값.Contains("vt.")) _타동사나옴 = true;

                _서술어나옴 = true;

                _결과 += "ⓥ{" + 현재내용에중괄호붙이기(_현재);

                _처리결과기호 = "명령어서술어";

                return true;
            }
            else if ((_처리결과기호 != "시작") && 접속어(_이전)
                && (_현재값.Contains("vt.") || _현재값.Contains("vi.")) && !_현재값.Contains("s형") && !_현재값.Contains("ing형")
                && !_다음값.Contains("과거형") && !_다음값.Contains("s형"))
            {
                if (_현재값.Contains("vi.")) _자동사나옴 = true;
                if (_현재값.Contains("vt.")) _타동사나옴 = true;

                _서술어나옴 = true;

                _결과 += "ⓥ{" + 현재내용에중괄호붙이기(_현재);

                _처리결과기호 = "명령어서술어";

                return true;
            }

            return false;
        }
        public bool 다음어절_서술어가능성(string 이전어절모두, string 현재어절, string 다음어절, string 다다음어절)
        {
            if (서술어가능성(다다음어절)) return false;

            if (이전어절맨뒤가_부사빼고_BE동사인지(이전어절모두) && 부사_다른품사는없는(현재어절) && PP형(다음어절)) return false;

            if (Have동사(현재어절) && PP형(다음어절)) return false;

            if (Be동사(현재어절) && PP형(다음어절)) return false;

            if (Be동사(현재어절) && ING형(다음어절)) return false;

            if (자동사가능성(다음어절)) return true;

            if (타동사가능성(다음어절)) return true;

            return false;
        }
        public bool 이전어절맨뒤가_부사빼고_BE동사인지(string 이전어절모두)
        {
            List<string> 역순어절들 = new List<string>();

            변환.문자열.어절들역순으로(이전어절모두, ref 역순어절들);

            foreach (string 현재어절 in 역순어절들)
            {
                if (Be동사(현재어절)) return true;
                else if (동사가능성(현재어절)) return false;
            }


            return false;
        }
        public bool 서술어가능성(string 현재어절)
        {
            if (ING형(현재어절)) return false;

            if (자동사가능성(현재어절)) return true;

            if (타동사가능성(현재어절)) return true;

            return false;
        }
        public bool 자동사가능성(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);
            if (현재어절검색결과.Contains("vi."))
                return true;

            return false;
        }
        public bool 타동사가능성(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);
            if (현재어절검색결과.Contains("vt."))
                return true;

            return false;
        }
        public bool 자타동사가능성(string 현재어절)
        {
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);
            if (현재어절검색결과.Contains("v."))
                return true;

            return false;
        }

        public bool S미정_BeGoingTo()
        {
            if (_주어임시저장소 == "") return false; // 주어꺼리가 있어야 주어를 만들고 진행형을 만든다.

            if (_서술어나옴 == true) return false; // 문장의 서술어가 둘 일 수 없다.

            if(Be동사(_현재) && _다음.ToLower() == "going" && _2다음 == "to" && 동사원형(_3다음))
            {
                _결과 += "ⓢ{" + _주어임시저장소 + "} ⓥ{ⓧ{" + _현재B + " "  + _다음B + " " + _다음2B + "} " + _다음3B + "}";
                _주어임시저장소 = "";

                _처리결과기호 = "BeGoingTo";

                
                if (_3다음검색값.Contains("vi.")) { _자동사나옴 = true; }
                if (_3다음검색값.Contains("vt.")) { _타동사나옴 = true; }

                if (_4다음검색값.Contains("n.")) _자동사나옴 = false; // 뒤에 명사나 명사구 나오면 거의 
                if (관사(_4다음)) _자동사나옴 = false;


                if (_3다음 == "be") _자동사나옴 = true;

                _서술어나옴 = true;

                _i += 3;
                return true;
            }

            return false;
        }
        public bool S미정_현재진행()
        {
            if (_주어임시저장소 == "") return false; // 주어꺼리가 있어야 주어를 만들고 진행형을 만든다.


            if (_서술어나옴 == true) return false; // 문장의 서술어가 둘 일 수 없다.

            if ((_현재 == "am") || (_현재 == "is") || (_현재 == "are") || (_현재 == "was") || (_현재 == "were"))
            {
                if (_다음값.Contains("ing형"))
                {
                    _현재진행형시작 = true;

                    _결과 += "ⓢ{" + _주어임시저장소 + "} ⓥ{" + _현재;

                    _주어임시저장소 = "";

                    _처리결과기호 = "현재진행형시작";

                    return true;
                }
            }

            return false;
        }
        public bool S있고_현재진행형시작()
        {
            if (_주어나옴 != true) return false; // 주어가 나오지 않은 상태에서 서술어가 먼저 나올 수 없다. (일반적으로)

            if (_서술어나옴 == true) return false; // 문장의 서술어가 둘 일 수 없다.

            if (_다음 == "exciting") return false;
            if (_다음 == "interesting") return false;
            if (_다음 == "amazing") return false;


            string 다음어절검색결과 = Form1._검색.영한사전_문장부호제거(_다음);
            if ((_현재 == "am") || (_현재 == "is") || (_현재 == "are") || (_현재 == "was") || (_현재 == "were"))
            {
                if (다음어절검색결과.Contains("ing형"))
                {

                    _현재진행형시작 = true;

                    _결과 += "ⓥ{" + _현재;


                    _처리결과기호 = "현재진행형시작";

                    return true;
                }
            }

            return false;
        }
        public bool 현재진행형종료()
        {

            if (_현재진행형시작 == false) return false;

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

            if (현재어절검색결과.Contains("vi.")) { _자동사나옴 = true; }
            if (현재어절검색결과.Contains("vt.")) { _타동사나옴 = true; }

            _서술어나옴 = true;

            _결과 += _현재 + "}";
            _현재진행형시작 = false;
            _처리결과기호 = "현재진행형종료";

            return true;
        }
        public bool 주어있고_서술어확정()
        {
            if (_조동사서술어시작 == true) return false; // 조동사가 시작된 다음에는 자체 확장 가능성 함수를 씁니다.

            if (_주어나옴 != true) return false; // 주어가 나오지 않은 상태에서 서술어가 먼저 나올 수 없다. (일반적으로)

            if (_현재진행형시작 == true) return false;

            if (_서술어나옴 == true) return false; // 문장의 서술어가 둘 일 수 없다.

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

            if (현재어절검색결과.Contains("ing형")) return false;


            if (이어동사_자동사_과거형가능성(_현재, _다음, _2다음))
            {
                _결과 += "ⓥ{" + _현재 + " " + _다음 + "}";

                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                _처리결과기호 = "과거형동사";
                _i++;
                return true;
            }
            else if (이어동사_타동사_과거형가능성(_현재, _다음, _2다음))
            {
                _결과 += "ⓥ{" + _현재 + " " + _다음 + "}";

                _주어나옴 = true;
                _서술어나옴 = true;
                _타동사나옴 = true;

                _처리결과기호 = "과거형동사";
                _i++;
                return true;
            }
            else if (이어동사_자동사_현재형가능성(_현재, _다음, _2다음))
            {
                _결과 += "ⓥ{" + _현재 + " " + _다음 + "}";

                _주어나옴 = true;
                _서술어나옴 = true;
                _자동사나옴 = true;

                _처리결과기호 = "서술어";
                _i++;
                return true;
            }
            else if (이어동사_타동사_현재형가능성(_현재, _다음, _2다음))
            {
                _결과 += "ⓥ{" + _현재 + " " + _다음 + "}";

                _주어나옴 = true;
                _서술어나옴 = true;
                _타동사나옴 = true;

                _처리결과기호 = "서술어";
                _i++;
                return true;
            }


            if (자동사가능성(_현재))
            {
                _서술어나옴 = true;
                _자동사나옴 = true;

                if (현재어절검색결과.Contains("2형식동사")) _2형식동사 = true;

            }

            if (타동사가능성(_현재))
            {
                _서술어나옴 = true;
                _타동사나옴 = true;

            }

            if (자타동사가능성(_현재))
            {
                _서술어나옴 = true;
                _자동사나옴 = true;
                _타동사나옴 = true;

            }

            if (_서술어나옴 == true)
            {
                _결과 += "ⓥ{" + 현재내용에중괄호붙이기(_현재);
            }

            _처리결과기호 = "서술어";

            return _서술어나옴;
        }

        public bool 보어확정가능성()
        {

			if (_현재 == "out" && _다음 == "of") // 가끔 out만을 형용사로 잘못 보는 경우가 있다. 
				return false;


            if (_처음 && _마지막 && 형용사가능성(_현재)) // 처음이자 마지막이면, 그러니까 하나뿐이면...
            {
                _결과 += "ⓒ{" + _현재.중괄호붙이기(1);
                _처리완료 = true;
				_처리결과기호 = "보어확정가능성";
				return true;
            }

            if (_1형식동사) return false;

            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

            if (현재어절검색결과 != "" && !현재어절검색결과.Contains("n.") && !현재어절검색결과.Contains("a.") && !현재어절검색결과.Contains("ed형") && !현재어절검색결과.Contains("pp형") && !현재어절검색결과.Contains("p.p형") && !현재어절검색결과.Contains("p.p.형")) // 사전의 검색결과가 없으면 고유명사일 가능성이 있는데, 고유명사도 아니고, 그냥 명사도 형용사도 아니면 보어일리가 없다.
                return false;

            if (_부사구_전치사구시작 == true) return false; // 부사구가 시작되면 보어 가능성은 없다.

            if ((_자동사나옴 == true) && (_타동사나옴 == false) && (_보어구시작 == false) && (_마지막 == true))
            {
                _결과 += "ⓒ{" + _현재.중괄호붙이기(1);
                _처리결과기호 = "보어확정가능성";
                return true;
            }

            if ((_2형식동사 == true) && 형용사가능성(_현재) && (_보어구시작 == false) && (_마지막 == true))
            {
                _결과 += "ⓒ{" + _현재.중괄호붙이기(1);
                _처리결과기호 = "보어확정가능성";
                return true;
            }


            if ((_자동사나옴 == true) && (_타동사나옴 == false) && (_보어구시작 == false) && (전치사_to와of빼고(_다음) || 접속어(_다음) && 완결된문장(_다다음어절모두)))
            {
                _결과 += "ⓒ{" + _현재.중괄호붙이기(1);
                _처리결과기호 = "보어확정가능성";
                return true;
            }

			


			// We are prone to listen to
			if ((_자동사나옴 == true) && (_타동사나옴 == false) && ((전치사가능성(_다음) && _다음소문자 != "for") || 전치사구가능성(_다음어절모두))) 
			{
				if(_보어임시저장소 != "")
					_결과 += "ⓒ{" + _보어임시저장소 + " " + _현재.중괄호붙이기(1);
				else
					_결과 += "ⓒ{" + _현재.중괄호붙이기(1);

				_처리결과기호 = "보어확정가능성";
				_보어임시저장소 = "";
				_보어구시작 = false;
				return true;
			}


			return false;
        }

        public string 문장가능성(string 문장)
        {
            string 미니문장분석결과;

            구문자동분석 미니문장 = new 구문자동분석();

			string 문장가능성확인용불필요로그 = "";
            미니문장분석결과 = 미니문장.구문분석(문장, ref 문장가능성확인용불필요로그);

            if (미니문장분석결과.Contains("ⓢ") && 미니문장분석결과.Contains("ⓥ"))
            {
                return 미니문장분석결과;
            }
            else
                return "";
        }

        public bool 보어시작가능성()
        {
            //
            if (_to부정사부사적용법시작 || _to부정사목적어구시작 || _to부정사형용사적용법시작) return false;

            if (_조동사서술어시작 == true) return false;    // 조동사 서술어가 안끝났는데 보어가 나올 수가 없다.
            if (_1형식동사) return false;
            // 아래의 어구들이 진행되고 있다면, 보어 시작가능성은 없다.
            if (_보어구시작 ==  true) return false;
            if (_부사구_전치사구시작 == true) return false;
            if (_목적어구시작 == true) return false;
            if (_목적어나옴) return false;

            if (!_이전값.Contains("2형식동사") && _이전값.Contains("1형식동사")) // 일부러 보어는 절대로 아니라고 말하고 있다. 이 경우 목적어로 돌아가는 것이 좋다.
                return false;

			if (_처리결과기호 == "보어확정가능성")	return false; // 보어가 끝나자마자 또 보어가 시작될 수 없다.

			string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(_현재);
            string 이전어절검색결과 = Form1._검색.영한사전_문장부호제거(_이전);

            if (이전어절검색결과.Contains("2형식동사") && _현재 == "like")
            {
                string 미니문장분석결과 = 문장가능성(_다음어절모두);
                if (미니문장분석결과 != "")
                {
                    _결과 += "ⓒ{㉨{like} " + 미니문장분석결과.중괄호붙이기(1);
                        
                    _처리완료 = true;
                }
                else
                {
                    _보어임시저장소 += "like";
                    _처리결과기호 = "보어시작";
                }

                return true;
            }

            if (!Form1._검색.전치사_나오기_전에_명사_있는지_확인(_다음어절모두) && 부사가능성(_현재))
				return false;

            if (_자동사나옴 == true)
            {
                _보어구시작 = true;

                _보어임시저장소 += _현재;

                if (_마지막)
                    _결과 = _결과.중괄호붙이기(1);

                _처리결과기호 = "보어시작";

                return true;

            }
            if (_이전 == "tend" && _현재 == "to") { _보어구시작 = true; _보어임시저장소 += _현재; _처리결과기호 = "보어시작"; return true; } // tend는 뒤에 to 부정사 나오면 자동사임
            if (_이전 == "tends" && _현재 == "to") { _보어구시작 = true; _보어임시저장소 += _현재; _처리결과기호 = "보어시작"; return true; } // tend는 뒤에 to 부정사 나오면 자동사임
            if (_이전 == "tended" && _현재 == "to") { _보어구시작 = true; _보어임시저장소 += _현재; _처리결과기호 = "보어시작"; return true; } // tend는 뒤에 to 부정사 나오면 자동사임

            return false;
        }
        public  bool 보어종료가능성()
        {
			string 보어문장구문분석로그 = "";

            if(_보어구시작 == false)   return false; // 시작도 안한 걸 종료할 수 없다.
            if (!_이전값.Contains("2형식동사") && _이전값.Contains("1형식동사")) // 일부러 보어는 절대로 아니라고 말하고 있다. 이 경우 목적어로 돌아가는 것이 좋다.
                return false;


            if ((변환.문자열.Right(_현재, 1) == ".") || _마지막 == true)
            {
                _보어구시작 = false;

                구문자동분석 보어절분석 = new 구문자동분석();
                _보어임시저장소 += _현재;
                보어절분석._내부첫접속사부사구분 = "접속사";
                _결과 += "ⓒ{" + 보어절분석.구문분석(_보어임시저장소, ref 보어문장구문분석로그).중괄호붙이기(1);
                _처리결과기호 = "보어종료가능성";
                _보어임시저장소 = "";
                return true;
            }

			return false;
        }
        public  bool 목적어확정가능성()
        {
            if (_목적어나옴 == true) return false;
            if (_to부정사서술어나옴) return false;

           

            if(_현재값 != "" && !_현재값.Contains("n.")) // 사전의 검색결과가 없으면 고유명사일 가능성이 있는데, 고유명사도 아니고, 그냥 명사도 아니면 목적어일리가 없다.
                return false;

			if (_이전값.Contains("2형식동사") && 형용사가능성(_현재)) // That sounds fun.과 같은 형태로, 이 경우 2형식 문장일 가능성이 훨씬 높다.
				return false;

           if(_부사구_전치사구시작 == true) return false; // 부사구가 시작되면 목적어 가능성은 없다.

           if((_타동사나옴 == true) && (_보어구시작 == false) && (_마지막 == true) && (_목적어구시작 == false) && !_현재값.Contains("명사로잘쓰이지않음"))
           {
               _목적어나옴 = true;

               _결과 +=  "ⓞ{" + 현재내용에중괄호붙이기(_현재);
                _처리결과기호 = "목적어확정가능성";
                return true;
           }

           if((_타동사나옴 == true) && (_보어구시작 == false) && (_목적어구시작 == false) && (전치사_to와of빼고(_다음) || 접속어(_다음) || 부사가능성(_다음)))
           {
				if(부사가능성(_다음) && _현재 == "no")  return false;
               _목적어나옴 = true;

               _결과 +=  "ⓞ{" + 현재내용에중괄호붙이기(_현재);
                _처리결과기호 = "목적어확정가능성";
                return true;
           }

           return false;
        }
        public  bool 목적어시작가능성()
        {
            if (_to부정사부사적용법시작 || _to부정사목적어구시작 || _to부정사형용사적용법시작) return false;

            string 이전어절검색결과 = Form1._검색.영한사전_문장부호제거(_이전);


            if (이전어절검색결과.Contains("2형식동사") && _현재 == "like") //The actors sounded like they were reading from a book.
            {
                return false;
            }
            if (_조동사서술어시작 == true) return false;    // 조동사 서술어가 안끝났는데 목적어가 도대체 왜 나옴. push back같은 경우, back이 명사도 있지만 이건 아니지.

            if (_보어구시작 == true) return false;
            if(_목적어나옴 == true) return false;    // 이미 목적어가 나온적이 있으면 목적어가 또 시작될 가능성이 없습니다.
            if(_목적어구시작 ==  true) return false; // 이미 시작했으면 목적어 시작가능성은 없습니다.
            if(_부사구_전치사구시작 == true) return false; // 부사구가 시작되면 보어 가능성은 없습니다.

            if (_이전 == "tend" && _현재 == "to") return false; // tend는 뒤에 to 부정사 나오면 자동사임
            if (_이전 == "tends" && _현재 == "to") return false; // tend는 뒤에 to 부정사 나오면 자동사임
            if (_이전 == "tended" && _현재 == "to") return false; // tend는 뒤에 to 부정사 나오면 자동사임


			if ((_타동사나옴 == true && Form1._검색.전치사_나오기_전에_명사_있는지_확인(_다음어절모두))// they eat lunch in the morning. 바로 다음 것이 전치사인 경우 그냥 목적어 확정을 해야 한다.
			|| (_타동사나옴 == true && (소유대명사(_현재) || 관사(_현재)))
			) 
            {
                _목적어구시작 = true;

                _목적어임시저장소 += _현재;
                _처리결과기호 = "목적어시작";

                return true;
            }        

            return false;
        }
        public  bool 목적어종료가능성()
        {
            if (_to부정사목적어구시작 || _to부정사형용사적용법시작 || _to부정사부사적용법시작 || _to부정사내목적어구시작) return false; // 여기서 처리할 문제가 아니다.

            string 검색결과 = Form1._검색.영한사전_문장부호제거(_현재);

            // 명사일 가능성이 없으면 목적어일 가능성이 없다. 하지만 목적어 임시 저장소가 있다면 일단 그것은 그냥 뿌려줘야 한다.
            if(!검색결과.Contains("n.") && !검색결과.Contains("ing형") && 검색결과 != "" && _목적어임시저장소 == "")
            {
                return false;
            }


            if(_목적어구시작 == false)   return false; // 시작도 안한 걸 종료할 수 없다.

            if((변환.문자열.Right(_현재, 1) == ".") || _마지막 == true ||
            ((변환.문자열.Right(_현재, 1) == ",") && !_현재값.Contains("사람이름") && _다음값.Contains("사람이름")) 
                )
            {
                _목적어구시작 = false;

                _목적어임시저장소 += _현재;

                구문자동분석 목적어절분석 = new 구문자동분석();

				//                if(_목적어임시저장소.StartsWith("to "))
				//              {
				//                목적어절분석._구문분석내_to부정사 = true;
				//              _처리결과 += "ⓞ{" + 목적어절분석.구문분석(_목적어임시저장소).중괄호붙이기(1);
				//        }
				//      else

				string 목적어절구문분석로그 = "";
                _결과 += "ⓞ{" + 목적어절분석.구문분석(_목적어임시저장소, true, ref 목적어절구문분석로그).중괄호붙이기(1); // ⓢ{Korea} ⓥ{has} ⓞ{ⓢ{a lot of delivery} ⓥ{services}}

				

                _목적어임시저장소 = "";



                _처리결과기호 = "목적어종료가능성";
                _목적어나옴 = true;
                return true;
            }

            return false;
        }
        public  bool 조동사서술어시작()
        {
            if (_목적어임시저장소 != "") return false;
            if(_조동사서술어시작) return false;
            if (_조동사나옴) return false;

            if(((_주어나옴 == true) || (_주어임시저장소 != "")) && 조동사(_현재))
            {
                _조동사서술어시작 = true;

                if(_주어임시저장소 != "")
                {
					bool 동명사구 = false;
					if(Form1._검색.영한사전_문장부호제거(_주어임시저장소.첫번째어절()).Contains("ing형"))
					{
						string 동명사구_구문분석_로그 = "";

						_결과 += "ⓢ{" + 동명사구_구문분석(_주어임시저장소, ref 동명사구_구문분석_로그); 
						동명사구 = true;
					}
					else 
						_결과 += "ⓢ{" + _주어임시저장소; 

                    if (_부사구_전치사구시작 == true && _전치사구동반한명사구시작 == true)
                    {
                        _부사구_전치사구시작 = false;
                        _전치사구동반한명사구시작 = false;

						if(!동명사구) // 동명사구일 때는 자동으로 지워서 결과를 내보냄.
							_결과 += "}";

                    }

                    if (_마지막)
                        _결과 += "} ⓥ{ⓧ{" + _현재.중괄호붙이기(2);
                    else
                        _결과 += "} ⓥ{ⓧ{" + _현재.중괄호붙이기(1);

                    _주어임시저장소 = "";
                    _주어나옴 = true;
                }
                else
                {
                    if (_마지막)
                        _결과 += "ⓥ{ⓧ{" + _현재.중괄호붙이기(2);
                    else
                        _결과 += "ⓥ{ⓧ{" + _현재.중괄호붙이기(1);
                    

                }
                _처리결과기호 = "조동사서술어시작가능성";
                return true;
            }
            // 주어후보가 있을 때를 추가해야 합니다.
            return false;
        }
		public bool 조동사내beAbleTo()
		{
			if (_조동사서술어시작 == false) return false; // 시작을 해야 조동사 서술어 내부이다.

			if (_현재.ToLower() == "be" && _다음.ToLower() == "able" && _2다음.ToLower() == "to")
			{
				_결과 += "ⓧ{" + _현재 + " " + _다음 + " " + _2다음 + "}";
				_i += 2;

				return true;
			}

			return false;
		}

        public bool 조동사서술어내_부사(string 현재어절)
        {
            if (_조동사서술어시작 == false) return false; // 시작을 해야 조동사 서술어 내부이다.

            if (부사_다른품사는없는(현재어절)) // 바로 밑의 코드 때문에 쓸데 없긴 한데, 일단 둘로 나눠둔다.
            {
                _결과 += "ⓧ{" + 현재어절 + "}";
                _처리결과기호 = "조동사서술어내부부사";
                return true;
            }
            else if (부사가능성(현재어절)) // 사실 동사만 아니면 뭐가 나와도 부사이긴 하다
            {
                _결과 += "ⓧ{" + 현재어절 + "}";
                _처리결과기호 = "조동사서술어내부부사";
                return true;
            }

            return false;
        }
        public  bool 조동사서술어끝()
        {
            if(_조동사서술어시작 == false) return false; // 시작을 해야 종료가능성이 있다

            if(_현재값.Contains("vi."))
            {
                _서술어나옴 = true;
                _자동사나옴 = true;
            }

            if(_현재값.Contains("vt."))
            {
                _서술어나옴 = true;
                _타동사나옴 = true;

            }

            if(_현재값.Contains("v."))
            {
                _서술어나옴 = true;
                _자동사나옴 = true;
                _타동사나옴 = true;

            }

            if(이어동사용부사(_다음))
            {
                _결과 += _현재;
                _처리결과기호 = "조동사서술어종료";

				if (_현재값.Contains("2형식동사")) _2형식동사 = true;

				return true; // 다음 back에서 자동으로 서술어 종료가 됨
            }

            if(_현재 == "be" && _다음값.Contains("ing형"))
            {
                _결과 += _현재;
                _처리결과기호 = "조동사서술어종료";
                return true; // 다음 back에서 자동으로 서술어 종료가 됨
            }


            if(_서술어나옴 == true)
            {
                _조동사서술어시작 = false;
                _결과 += _현재.중괄호붙이기(1);
                _처리결과기호 = "조동사서술어종료";

				if (_현재값.Contains("2형식동사")) _2형식동사 = true;

				return true;
            }
            return false;
        }

        public bool 목적어가능성_타동사확인용(string 현재어절)
        {
            // 파싱에 사용하는 것이 아니고, 현재어절이 관사나, 명사 같은 것인지 확인하는 용도로만 사용한다.

            if (string.IsNullOrEmpty(현재어절)) return false;

            if (관사(현재어절)) return true;

            if (소유대명사(현재어절)) return true;

            if (명사가능성(현재어절)) return true;

            return false;
        }



        public bool 이어동사_타동사_과거형가능성(string 어절1, string 어절2, string 어절3)
        {
            if ((어절1 == "cut") && (어절2 == "out")) return true;
            if ((어절1 == "gave") && (어절2 == "up")) return true;

            return false;
        }


        public bool 이어동사_타동사_PP형가능성(string 어절1, string 어절2, string 어절3)
        {
            if ((어절1 == "cut") && (어절2 == "out")) return true;
            if ((어절1 == "given") && (어절2 == "up")) return true;

            return false;
        }

        public bool 이어동사_자동사_현재형가능성(string 어절1, string 어절2, string 어절3)
        {
            if (목적어가능성_타동사확인용(어절3)) return false;

            if ((어절1 == "come") && (어절2 == "back")) return true;
            if ((어절1 == "give") && (어절2 == "up")) return true;
            if ((어절1 == "go") && (어절2 == "on")) return true;
            if ((어절1 == "grow") && (어절2 == "up")) return true;
			if ((어절1 == "wake") && (어절2 == "up")) return true;

			return false;
        }

		public bool 이어동사_자동사_과거형가능성(string 어절1, string 어절2, string 어절3)
		{
			if (목적어가능성_타동사확인용(어절3)) return false;

			if ((어절1 == "gave") && (어절2 == "up")) return true;
			if ((어절1 == "grew") && (어절2 == "up")) return true;
			if ((어절1 == "went") && (어절2 == "on")) return true;
			if ((어절1 == "woke") && (어절2 == "up")) return true;

			return false;
		}

		public bool 이어동사_자동사_PP형가능성(string 어절1, string 어절2, string 어절3, ref int 형식)
		{
			형식 = 0;
			if (목적어가능성_타동사확인용(어절3)) return false;

			if ((어절1 == "given") && (어절2 == "up")) { 형식 = 1; return true; }
			if ((어절1 == "grown") && (어절2 == "up")) { return true; }
			if ((어절1 == "gotten") && (어절2 == "up")) { 형식 = 1; return true; }
			if ((어절1 == "gone") && (어절2 == "on")) { 형식 = 1; return true; }
			if ((어절1 == "waken") && (어절2 == "up")) return true;

			return false;
		}

		public bool 이어동사_타동사_현재형가능성(string 어절1, string 어절2, string 어절3)
        {
            if ((어절1 == "cut") && (어절2 == "out")) return true;
            if ((어절1 == "give") && (어절2 == "up")) return true;
            if ((어절1 == "link") && (어절2 == "together")) return true;

            return false;
        }
    }
}
