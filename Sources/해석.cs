//█▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█
//█  ╗╦╔ ╦═╗ ╔  ╔═╗ ╔═╗ ╔╗╗ ╦═╗  █
//█  ║║║ ╠═  ║  ║   ║ ║ ║║║ ╠═   █
//█  ╚╩╝ ╚═╝ ╚═ ╚═╝ ╚═╝ ╩╩╩ ╩═╝  █
//█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄█

#region 앞부분
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 변환;
using 구문분석;
using 검색_진화하는;

namespace 편집기의_제왕
{
    public static class ExClass
    {
		private static string _초성테이블 = "ㄱㄲㄴㄷㄸㄹㅁㅂㅃㅅㅆㅇㅈㅉㅊㅋㅌㅍㅎ";
		private static string _중성테이블 = "ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ";
		private static string _종성테이블 = " ㄱㄲㄳㄴㄵㄶㄷㄹㄺㄻㄼㄽㄾㄿㅀㅁㅂㅄㅅㅆㅇㅈㅊㅋㅌㅍㅎ";
		private static ushort _UniCode한글Base = 0xAC00;
		private static ushort _UniCode한글Last = 0xD79F;

		public static string 다(this String 원본문자열)
		{
			if (String.IsNullOrEmpty(원본문자열)) return "";
			if (원본문자열.Length < 2) return 원본문자열;

			string s = 원본문자열.다_빼기().Right(1); //마지막2번째글자
			if (!글자종성있는지여부(s)) // 다를 뺀 것의 종성이 없으면,
			{
				return 원본문자열.Substring(0, 원본문자열.Length - 2) + 문자열.초중종_자소합치기(s.초성추출(), s.중성추출(), "ㄴ") + "다";
			}

			return 원본문자열;
		}

		public static string 할수있다(this String 원본문자열)
		{
			return ㄹ_수_있다(원본문자열);
		}

		public static string ㄹ_수_있을_것이다(this String 원본문자열)
		{
			// 처리하기 애매한 것들은 수동으로 말끔하게! 히힛!
			if (원본문자열 == "들리다") return "들을 수 있을 것이다";
			if (원본문자열 == "묻다") return "물어볼 수 있을 것이다";

			// 1. "다"를 뺀다.
			// 2. 맨 뒷글자의 종성을 'ㄹ'로 교체한다.
			//	2-1. 맨 뒷글자를 세 개로 나눈다.
			//	2-2. 초성과, 중성을 이용해서 종성 'ㄹ'로 합친다.
			//	2-3. " 수 있을텐데."를 결합한다.

			원본문자열 = 다_빼기(원본문자열);

			string 원본문자열앞부분 = 원본문자열.Substring(0, 원본문자열.Length - 1);
			string 원본문자열뒷부분한글자 = 원본문자열.Substring(원본문자열.Length - 1);

			string 맨뒷글자초성 = 초성추출(원본문자열뒷부분한글자);
			string 맨뒷글자중성 = 중성추출(원본문자열뒷부분한글자);

			return 원본문자열앞부분 + 초중종_자소합치기(맨뒷글자초성, 맨뒷글자중성, "ㄹ") + " 수 있을 것이다";
		}


		public static string ㄹ_수_있을텐데(this String 원본문자열)
		{
			// 처리하기 애매한 것들은 수동으로 말끔하게! 히힛!
			if (원본문자열 == "들리다") return "들을 수 있을텐데";
			if (원본문자열 == "묻다") return "물어볼 수 있을텐데";

			// 1. "다"를 뺀다.
			// 2. 맨 뒷글자의 종성을 'ㄹ'로 교체한다.
			//	2-1. 맨 뒷글자를 세 개로 나눈다.
			//	2-2. 초성과, 중성을 이용해서 종성 'ㄹ'로 합친다.
			//	2-3. " 수 있을텐데."를 결합한다.

			원본문자열 = 다_빼기(원본문자열);

			string 원본문자열앞부분 = 원본문자열.Substring(0, 원본문자열.Length - 1);
			string 원본문자열뒷부분한글자 = 원본문자열.Substring(원본문자열.Length - 1);

			string 맨뒷글자초성 = 초성추출(원본문자열뒷부분한글자);
			string 맨뒷글자중성 = 중성추출(원본문자열뒷부분한글자);

			return 원본문자열앞부분 + 초중종_자소합치기(맨뒷글자초성, 맨뒷글자중성, "ㄹ") + " 수 있을텐데";
		}

		public static string ㄹ_수_있다(this String 원본문자열)
		{
			// 처리하기 애매한 것들은 수동으로 말끔하게! 히힛!
			if(원본문자열 == "들리다") return "들을 수 있다";
			// 1. "다"를 뺀다.
			// 2. 맨 뒷글자의 종성을 'ㄹ'로 교체한다.
			//	2-1. 맨 뒷글자를 세 개로 나눈다.
			//	2-2. 초성과, 중성을 이용해서 종성 'ㄹ'로 합친다.
			//	2-3. " 수 있다."를 결합한다.

			원본문자열 = 다_빼기(원본문자열);

			string 원본문자열앞부분 = 원본문자열.Substring(0, 원본문자열.Length - 1);
			string 원본문자열뒷부분한글자 = 원본문자열.Substring(원본문자열.Length - 1);

			string 맨뒷글자초성 = 초성추출(원본문자열뒷부분한글자);
			string 맨뒷글자중성 = 중성추출(원본문자열뒷부분한글자);

			return 원본문자열앞부분 + 초중종_자소합치기(맨뒷글자초성, 맨뒷글자중성, "ㄹ") + " 수 있다";
		}



		public static string ㄹ텐데(this String 원본문자열)
		{
			// 처리하기 애매한 것들은 수동으로 말끔하게! 히힛!
			if (원본문자열 == "묻다") return "물어볼텐데";


			// 1. "다"를 뺀다.
			// 2. 맨 뒷글자의 종성을 'ㄹ'로 교체한다.
			//	2-1. 맨 뒷글자를 세 개로 나눈다.
			//	2-2. 초성과, 중성을 이용해서 종성 'ㄹ'로 합친다.
			//	2-3. " 수 있다."를 결합한다.

			원본문자열 = 다_빼기(원본문자열);

			string 원본문자열앞부분 = 원본문자열.Substring(0, 원본문자열.Length - 1);
			string 원본문자열뒷부분한글자 = 원본문자열.Substring(원본문자열.Length - 1);

			string 맨뒷글자초성 = 초성추출(원본문자열뒷부분한글자);
			string 맨뒷글자중성 = 중성추출(원본문자열뒷부분한글자);



			return 원본문자열앞부분 + 초중종_자소합치기(맨뒷글자초성, 맨뒷글자중성, "ㄹ") + "텐데";
		}

		public static string ㄹ때(this String 원본문자열)
		{
			// 처리하기 애매한 것들은 수동으로 말끔하게! 히힛!
			if (원본문자열 == "묻다") return "물을 때";

			// 1. "다"를 뺀다.
			// 2. 맨 뒷글자의 종성을 'ㄹ'로 교체한다.
			//	2-1. 맨 뒷글자를 세 개로 나눈다.
			//	2-2. 초성과, 중성을 이용해서 종성 'ㄹ'로 합친다.
			//	2-3. " 수 있다."를 결합한다.

			원본문자열 = 다_빼기(원본문자열);

			string 원본문자열앞부분 = 원본문자열.Substring(0, 원본문자열.Length - 1);
			string 원본문자열뒷부분한글자 = 원본문자열.Substring(원본문자열.Length - 1);

			string 맨뒷글자초성 = 초성추출(원본문자열뒷부분한글자);
			string 맨뒷글자중성 = 중성추출(원본문자열뒷부분한글자);

			return 원본문자열앞부분 + 초중종_자소합치기(맨뒷글자초성, 맨뒷글자중성, "ㄹ") + " 때";
		}

		public static string 이다_빼기(this String 원본문자열)
        {
            원본문자열 += "※";
            원본문자열 = 원본문자열.Replace("이다※", "");
            원본문자열 = 원본문자열.Replace("이다.※", "");
            원본문자열 = 원본문자열.Replace("※", "");
            return 원본문자열;
        }
        public static string 다_빼기(this String 원본문자열)
        {
            원본문자열 += "※";
            원본문자열 = 원본문자열.Replace("다※", "");
            원본문자열 = 원본문자열.Replace("다.※", "");
            원본문자열 = 원본문자열.Replace("※", "");
            return 원본문자열;
        }

		public static string 면서(this String 원본문자열)
		{

			if (원본문자열.EndsWith("었다"))
				return 원본문자열.Left(원본문자열.Length - 2) + "면서";
			else if (원본문자열.EndsWith("졌다"))
				return 원본문자열.Left(원본문자열.Length - 2) + "지면서";
			else if (원본문자열.EndsWith("다"))
				return 원본문자열.Left(원본문자열.Length - 1) + "면서";

			return 원본문자열;
		}


		public static string 된_빼기(this String 원본문자열)
		{
			원본문자열 += "※";
			원본문자열 = 원본문자열.Replace("된※", "");
			원본문자열 = 원본문자열.Replace("※", "");
			return 원본문자열;
		}

		public static string 한_빼기(this String 원본문자열)
		{
			원본문자열 += "※";
			원본문자열 = 원본문자열.Replace("한※", "");
			원본문자열 = 원본문자열.Replace("※", "");
			return 원본문자열;
		}


		public static string ㄹ것이다(this String 원본문자열)
        {
			if (원본문자열 == "") return "";

			원본문자열 += "※";
            원본문자열 = 원본문자열.Replace("하다※", "할 것이다");

            원본문자열 = 원본문자열.Replace("다※", "을 것이다");
            원본문자열 = 원본문자열.Replace("※", "");

            return 원본문자열;
        }

		public static string 부정(this String 원본문자열, bool 부정여부)
		{
			if(부정여부)		return 원본문자열.지않았다();
			else				return 원본문자열;
		}

		public static string 지않았다(this String 원본문자열)
        {
			if(원본문자열.EndsWith("했다"))
			{
				원본문자열 = 원본문자열.Substring(0, 원본문자열.Length - 2) + "하지 않았다";

				return 원본문자열;
			}

            원본문자열 += "※";
            원본문자열 = 원본문자열.Replace("다※", "지 않았다");

            원본문자열 = 원본문자열.Replace("※", "");
            return 원본문자열;
        }
        public static string 하라(this String 원본문자열)
        {
			if (원본문자열 == "쓰다") return "써라";

			if (글자종성있는지여부(변환.문자열.Right(원본문자열.다_빼기(), 1)) && 글자중성의양음여부(변환.문자열.Right(원본문자열.다_빼기(), 1)))
			{
				원본문자열 += "※";
				원본문자열 = 원본문자열.Replace("다※", "아라");
			}
			else if (글자종성있는지여부(변환.문자열.Right(원본문자열.다_빼기(), 1)) && !글자중성의양음여부(변환.문자열.Right(원본문자열.다_빼기(), 1)))
			{
				원본문자열 += "※";
				원본문자열 = 원본문자열.Replace("다※", "어라");
			}
			else
			{
				원본문자열 += "※";
				원본문자열 = 원본문자열.Replace("끼다※", "껴라");
				원본문자열 = 원본문자열.Replace("다※", "라");
			}
            원본문자열 = 원본문자열.Replace("※", "");
            return 원본문자열;
        }
        public static string 는(this String 단어, string 은는이가)
        {
			if (단어 == "") return "";

			if (은는이가 == "이가")
            {
                if (글자종성있는지여부(변환.문자열.Right(단어, 1))) return 단어 + "이";
                else
                {
                    if (단어 == "나")
                        단어 = "내";

                    return 단어 + "가";
                }
            }
            else
            {
                if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                    return 단어 + "은";
                else
                    return 단어 + "는";
            }
        }
        public static string 이가(this String 단어)
        {
			if (단어 == "") return "";

			if (글자종성있는지여부(변환.문자열.Right(단어, 1))) return 단어 + "이";
            else
            {
                if (단어 == "나")
                    단어 = "내";

                return 단어 + "가";
            }
        }

		public static string 기로(this String 단어)
		{
			if (단어 == "") return "";

			return 단어.다_빼기() + "기로";
		}
		public static string 아야(this String 단어)
        {
			if (단어 == "") return "";

			if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "아";
            else
                return 단어 + "야";
        }
        public static string 은는(this String 단어)
        {
			if (단어 == "") return "";

			if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "은";
            else
                return 단어 + "는";
        }

		public static string 하는(this String 단어)
		{
			if (단어 == "") return "";

			단어 = 단어.다_빼기();

			//if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
			return 단어 + "는";
		}

		public static string 도(this String 단어, bool 이름여부)
        {
			if (단어 == "") return "";

			if(단어.EndsWith("들"))
				return 단어 + "도";
            else if (글자종성있는지여부(변환.문자열.Right(단어, 1)) && (이름여부 == true))
                return 단어 + "이도";
            else
                return 단어 + "도";
        }

        public static string 에게(this String 단어)
        {
            if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "이에게";
            else
                return 단어 + "에게";
        }
        public static string 을를(this String 단어, string 단어구절)
        {
			if (단어 == "잘") return "잘"; // She is doing a good job.

			if (단어.EndsWith("바다"))
                return 단어 + "를";
            else if(단어구절 == "구")
            {
                return 단어.다_빼기().은는() + " 것을";
            }
            else if (단어구절 == "절") // 해야한다'고' 말했다.
                return 단어 + "고";
            else if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "을";
            else
                return 단어 + "를";
        }
        public static string 을를(this String 단어, string 단어구절, string 을를이가)
        {
			if (단어 == "잘") return "잘"; // She is doing a good job.

            if (을를이가 == "이가")
                return 단어.이가();
            if (단어.EndsWith("바다"))
                return 단어 + "를";
            else if (단어구절 == "구")
            {
                return 단어.다_빼기().은는() + " 것을";
            }
            else if (단어구절 == "절") // 해야한다'고' 말했다.
                return 단어 + "고";
            else if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "을";
            else
                return 단어 + "를";
        }

		public static string 해서(this String 단어)
		{
			// 먹다 > 먹어서
			// 먹는다 > 먹어서
			// 감는다 > 감아서
			// 감다 > 감아서
			// 잔다 > 자서
			// 부른다 > 불러서
			// 기쁘다 > 기뻐서
			// 시원하다 > 시원해서
			// 덥다 > 더워서
			// 뜨겁다 > 뜨거워서
			// 누른다 > 눌러서
			// 눌린다 > 눌려서

			// 만나다 > 만나서
			// 만난다 > 만나서
			// 신난다 > 신나서
			// 하늘을 난다 > 날아서
			if(단어.EndsWith("난다") && 단어.Length != 2)
			{
				return 단어.꼬리자르기(2) + "나서"; 
			}

			string 결과 = "";
			결과 = 단어.다_빼기() + "서";

			return 결과;
		}


		public static string 해야한다(this String 단어)
		{
			if (단어 == "") return "";

			string 결과 = "";
			결과 = 단어.다_빼기() + "어야 한다";
			결과 = 결과.Replace("하어야", "해야");

			return 결과;
		}

		public static string 어야한다(this String 단어, bool 형용사여부)
		{
			if (단어 == "") return "";

			string 결과 = "";
			if (형용사여부 == true)
			{
				if (단어.EndsWith("다"))
					결과 = 단어.다_빼기() + "돼야 한다";
				else if (단어.EndsWith("된"))
					결과 = 단어.된_빼기() + "돼야 한다";
				else if (단어.EndsWith("한"))
					결과 = 단어.한_빼기() + "해야 한다";
			}
			else
			{
				if (글자종성있는지여부(단어.Right(1)))
					결과 = 단어 + "이어야 한다";
				else
					결과 = 단어 + "여야 한다";
			}
			return 결과;
		}


		public static string 해왔다(this String 단어)
		{
			string 결과 = "";
			결과 = 단어.다_빼기() + "왔다";
			결과 = 결과.Replace("했왔다", "해왔다");

			return 결과;
		}

		public static string 해야했다(this String 단어)
		{
			string 결과 = "";
			결과 = 단어.다_빼기() + "어야 했다";
			결과 = 결과.Replace("하어야", "해야");

			return 결과;
		}

		public static string 라는(this String 단어)
		{
			if (단어 == "") return "";

			if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
				return "\'" + 단어 + "\'" + "이라는";
			else
				return "\'" + 단어 + "\'" + "라는";
		}

		public static string 라고(this String 단어)
        {
			if (단어 == "") return "";

			if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "이라고";
            else
                return 단어 + "라고";
        }
        public static string 형용사화(this String 단어)
        {
			if(단어.EndsWith("왔다") || 단어.EndsWith("왔다."))
			{
				단어 = 단어.Substring(0, 단어.Length - 2) + "왔던"; return 단어;
			}

            단어 += "※";
            단어 = 단어.Replace("다※", "");
            단어 = 단어.Replace("다.※", "");
            단어 = 단어.Replace("※", "");



            if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "을";
            else
            {
                return 단어 + "는";
            }
        }
		public static string 형용사화_문장(this String 단어)
		{
			단어 = 단어.다_빼기();

			return 단어 + "다는";
		}

		public static string 로(this String 단어)
        {
            if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                return 단어 + "으로";
            else
                return 단어 + "로";
		}

		public static string 와과(this String 단어)
		{
			if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
				return 단어 + "과";
			else
				return 단어 + "와";
		}

        public static string 다면(this String 단어, string 접속어, bool 현재여부)
        {
            string 결과 = "";
			if (접속어.ToLower() == "if")
			{
				// 그랬었다 > 그랬다면, 근데, 먹었다 > 먹다면(?)
				if (단어.Contains("었다"))
				{
					단어 = 단어.Replace("었다", "다면,");
					결과 = 단어 + ",";

					return 결과;
				}
				if (단어.EndsWith("였다")) return 단어 + "면,"; // 내가 너였다면
				else
				{
					if (단어 == "가지다") return "가지고 있다면,";

					// 배우다 > 배우면
					// 하다 > 하면

					결과 = 단어.다_빼기() + "면,";

					return 결과;
				}
			}
			else if (접속어.ToLower() == "and")
			{
				결과 = "그리고 " + 단어.다(); return 결과;
			}
			else if (접속어.ToLower() == "when")
			{
				if (현재여부)
				{
					결과 = 단어.ㄹ때();	return 결과;
				}
				else
				{
					결과 = 단어.다_빼기() + "던 때,"; return 결과;
				}
			}
			else if (접속어.ToLower() == "as")
			{
				결과 = 단어.면서() + ","; return 결과;
			}
			else
			{
				결과 = 단어 + "."; return 결과;
			}
        }
        public static string 이었니(this String 단어)
        {
            string 결과 = "";
            결과 = 단어.다_빼기();
            if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                결과 += "이었니";
            else
                결과 += "였니";

            return 결과;
        }
        public static string 었니(this String 단어)
        {
            string 결과 = "";
            결과 = 단어.다_빼기();
            결과 += "었니";
            결과 = 결과.Replace("타었니", "탔니");
            결과 = 결과.Replace("하었니", "했니");
            return 결과;
        }
        public static string 니(this String 단어)
        {
			// 종성이 'ㄴ'으로 끝나면 

			if(단어.Length > 1)
			{
				if(단어.EndsWith("다"))
				{
					if(종성추출(단어.Substring(단어.Length - 2, 1)) == "ㄴ")
					{
						string 초성 = 초성추출(단어.Substring(단어.Length - 2, 1));
						string 중성 = 중성추출(단어.Substring(단어.Length - 2, 1));

						return 단어.Substring(0, 단어.Length - 2) + 초중종_자소합치기(초성, 중성, "") + "니";
					}
				}
			}

//			if(단어.EndsWith("한다"))
//				return 단어.Substring(0, 단어.Length - 2) + "하니";


            단어 = 단어.다_빼기();

            return 단어 + "니";
        }
        public static string 이다(this String 단어, bool 형용사여부)
        {
            string 결과 = "";
            if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                결과 = 단어 + "이다";
            else
                결과 = 단어 + "다";

            if (형용사여부)
            {
				if(단어.EndsWith("운"))
				{
					단어 = 단어.Substring(0, 단어.Length - 1);

					string 단어맨마지막글자 = 변환.문자열.Right(단어, 1);

					단어 = 단어.Substring(0, 단어.Length - 1);

					return 단어 + 초중종_자소합치기(초성추출(단어맨마지막글자), 중성추출(단어맨마지막글자), "ㅂ") + "다";
				// 반가운이다 > 반갑다
				// 흥겨운이다 > 흥겹다
				// 더운이다 > 덥다
				// 추운이다 > 춥다
				// *순전히 운이다> 순전히 운이다

				}
				else if(단어.EndsWith("되는"))
				{
					단어 = 단어.Substring(0, 단어.Length - 2);

					return 단어 + "된다";
				}

				결과 = 결과.Replace("큰이다", "크다"); // 큰이다 > 크다
				결과 = 결과.Replace("있는이다", "있다"); // 있는이다 > 있다
				결과 = 결과.Replace("나은이다", "낫다"); // 나은이다 > 낫다
				결과 = 결과.Replace("은이다", "다"); // 옳은이다 > 옳다
                결과 = 결과.Replace("한이다", "하다"); // 신기한이다 > 신기하다
				결과 = 결과.Replace("하는이다", "한다"); // 직면한다
				결과 = 결과.Replace("화난이다", "화나다"); // 화난이다 > 화나다
            }
            return 결과;
		}

		public static string 이_아니다(this String 단어, bool 형용사여부)
		{
			string 결과 = "";
			if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
				결과 = 단어 + "이다";
			else
				결과 = 단어 + "다";


			if (형용사여부)
			{
				if (단어.EndsWith("운"))
				{
					단어 = 단어.Substring(0, 단어.Length - 1);

					string 단어맨마지막글자 = 변환.문자열.Right(단어, 1);

					단어 = 단어.Substring(0, 단어.Length - 1);

					return 단어 + 초중종_자소합치기(초성추출(단어맨마지막글자), 중성추출(단어맨마지막글자), "ㅂ") + "지 않다";
				}
				else // 명확한 > 명확하지 않다.
				{
					string 단어맨마지막글자 = 변환.문자열.Right(단어, 1);

					단어 = 단어.Substring(0, 단어.Length - 1);

					return 단어 + 초중종_자소합치기(초성추출(단어맨마지막글자), 중성추출(단어맨마지막글자), "") + "지 않다";
				}
			}
			else
			{
				return 단어.이가() + " 아니다.";
			}
		}



        public static string 는가(this String 단어)
		{
			string 결과 = "";

			if(단어.EndsWith("이다"))
			{
				결과 = 단어.이다_빼기() + "인가";
			}
			else
				결과 = 단어.다_빼기() + "는가";

			return 결과;
		}
        public static string 인가(this String 단어, bool 형용사여부)
		{
			string 결과 = "";

            if (형용사여부)
            {
				결과 = 단어 + "가?";
			}
			else
			{
				결과 = 단어 + "인가?";
			}

			return 결과;
		}
        public static string 었다(this String 단어, bool 형용사여부)
        {
			if (단어.EndsWith("었다") || 단어.EndsWith("였다")) return 단어; // 원래 과거형으로 끝나면 그대로 배출한다.

			string 단어앞부분 = ""; string 맨뒷글자 = ""; string 뒤에서두번째글자 = "";

			if(단어.Length > 0)
			{
				단어앞부분 = 단어.Substring(0, 단어.Length - 1);
				맨뒷글자 = 단어.Substring(단어.Length - 1);
			}

			if(단어.Length > 1)
			{
				뒤에서두번째글자 =  단어.Substring(단어.Length - 2, 1);
			}

			


			string 맨뒷글자초성 = 초성추출(맨뒷글자);
			string 맨뒷글자중성 = 중성추출(맨뒷글자);
			string 뒤에서두번째글자중성 = 중성추출(뒤에서두번째글자);


            string 결과 = "";

            if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
                결과 = 단어 + "이었다";
            else
                결과 = 단어 + "였다";

			// 규칙이 명확히 보일 때까지 자료를 더 수집한다.
            if (형용사여부)
            {
				// 결국 '았다', '었다', '기타' - 이 세 가지로 분류된다.

				if((뒤에서두번째글자중성 != "ㅏ") && 단어.EndsWith("쁜")) return 단어앞부분 + "뻤다";  // 예쁜이었다 > 예뻤다, 기쁜이었다> 기뻤다
				if((뒤에서두번째글자중성 != "ㅏ") && 단어.EndsWith("픈")) return 단어앞부분 + "펐다";  // 슬픈이었다 > 슬펐다.

				if(단어.EndsWith("건")) return 단어앞부분 + "겠다"; // 허여멀건이었다 > 허여멀겠다
				if(단어.EndsWith("먼")) return 단어앞부분 + "멀었다"; // 먼이었다 > 멀었다

				if(단어.EndsWith("한")) return 단어앞부분 + "했다"; // 어색한이었다 > 어색했다
				if(단어.EndsWith("있는")) return 단어앞부분 + "었다";
				if (단어.EndsWith("진")) return 단어앞부분 + "졌다";


				return 단어앞부분 + 초중종_자소합치기(맨뒷글자초성, "ㅏ", "ㅆ") + "다";

				// 뒤에서 두번째 단어의 중성이 'ㅏ'이고, 받침이 없으며, 'ㅡㄴ'으로 끝나면, 'ㅏㅆ다'를 붙이는 것 같다.
                //결과 = 결과.Replace("가쁜이었다", "가빴다"); // 숨가쁜이었다 > 숨가빴다
                //결과 = 결과.Replace("나쁜이었다", "나빴다"); // 나쁜이었다 > 나빴다

                //결과 = 결과.Replace("짠이었다", "짰다"); // 짠이었다 > 짰다
                //결과 = 결과.Replace("난이었다", "났다"); // 화난이었다 > 화났다

            }
            return 결과;


        }
        public static string 되었다(this String 단어)
        {
			if (단어.EndsWith("워")) return 단어 + "졌다";

            return 단어 + " 되었다";
        }

		public static string 다듬기(this String 단어)
		{
			단어 = 단어.Trim();

			while(단어.Contains("  "))
			{
				단어 = 단어.Replace("  ", " ");
			}

			return 단어;
		}
		public static string 초성추출(this String 글자)
		{
			if(string.IsNullOrEmpty(글자)) return "";

			char 한글자 = 글자[0];

			ushort temp = 0x0000;            // 임시로 코드값을 담을 변수
			//Char을 16비트 부호없는 정수형 형태로 변환 - Unicode
			temp = Convert.ToUInt16(한글자);
 
			// 캐릭터가 한글이 아닐 경우 처리
			if ((temp < _UniCode한글Base) || (temp > _UniCode한글Last))
				return "";

			int nUniCode = temp - _UniCode한글Base;
			int 초성 = nUniCode / (21 * 28);

			return _초성테이블[초성].ToString();
		}
		public static string 중성추출(this String 글자)
		{
			if(string.IsNullOrEmpty(글자)) return "";

			char 한글자 = 글자[0];

			ushort temp = 0x0000;            // 임시로 코드값을 담을 변수
			//Char을 16비트 부호없는 정수형 형태로 변환 - Unicode
			temp = Convert.ToUInt16(한글자);
 
			// 캐릭터가 한글이 아닐 경우 처리
			if ((temp < _UniCode한글Base) || (temp > _UniCode한글Last))
				return "";

			int nUniCode = temp - _UniCode한글Base;

			nUniCode = nUniCode % (21 * 28);
			int 중성 = nUniCode / 28;

			return _중성테이블[중성].ToString();
		}
		public static string 초중종_자소합치기(string s초성, string s중성, string s종성)
		{
			int i초성위치, i중성위치, i종성위치;
			int iUniCode;
			i초성위치 = _초성테이블.IndexOf(s초성);   // 초성 위치
			i중성위치 = _중성테이블.IndexOf(s중성);   // 중성 위치
			i종성위치 = _종성테이블.IndexOf(s종성);   // 종성 위치
										   // 앞서 만들어 낸 계산식
			iUniCode = _UniCode한글Base + (i초성위치 * 21 + i중성위치) * 28 + i종성위치;
			// 코드값을 문자로 변환
			char temp = Convert.ToChar(iUniCode);
			return temp.ToString();
		}
		public static string 종성추출(this String 글자)
		{
			if(string.IsNullOrEmpty(글자)) return "";

			char 한글자 = 글자[0];

			ushort temp = 0x0000;            // 임시로 코드값을 담을 변수
			//Char을 16비트 부호없는 정수형 형태로 변환 - Unicode
			temp = Convert.ToUInt16(한글자);
 
			// 캐릭터가 한글이 아닐 경우 처리
			if ((temp < _UniCode한글Base) || (temp > _UniCode한글Last))
				return "";

			int nUniCode = temp - _UniCode한글Base;

			nUniCode = nUniCode % (21 * 28);
			int 종성 = nUniCode % 28;

			return _종성테이블[종성].ToString();
		}
        private static bool 글자종성있는지여부(string 글자)
        {
            if (string.IsNullOrEmpty(글자)) return false;

            int x = (int)글자[0];

            if (x >= 0xAC00 && x <= 0xD7A3)
            {
                //int a,b,c;
                int c;

                c = x - 0xAC00;
                //a = c / (21 * 28);
                c = c % (21 * 28);
                //b = c / 28;
                c = c % 28;

                if (c == 0)
                    return false;
                else
                    return true;
            }
            return false;
		}

		private static bool 글자중성의양음여부(string 글자)
		{
			// 살다 > 살아라, 보다 > 보아라, 먹다 > 먹어라, 짖다 > 짖어라
			//"ㅏㅐㅑㅒㅓㅔㅕㅖㅗㅘㅙㅚㅛㅜㅝㅞㅟㅠㅡㅢㅣ";
			string 중성 = 중성추출(글자);

			if (중성 == "ㅏ") return true;
			if (중성 == "ㅗ") return true;
			return false;
		}

		public static string 점빼기(this String 단어)
		{
			return 단어.Replace(".", "");
		}

	}
    class 해석
    {

        public 해석()
        {
            
        }

        public string 해석하기(string 문자열, string 전체문맥)
        {
            if (!문자열.Contains("㉨"))
            {
                단문복문해석 단문복문해석 = new 단문복문해석();

                return 단문복문해석.단문복문해석하기(문자열, 전체문맥);
            }
            else
            {
                List<string> 단문들 = new List<string>();
                단문으로나누기(문자열, ref 단문들);

                단문복문해석 [] 단문복문해석 = new 단문복문해석[단문들.Count];



                string 해석결과 = "";

                if (단문들.Count == 2)
                {
                    if(단문들[0].Contains("㉨{If}") && 단문들[1].Contains("would"))
                    {
                        단문복문해석[0] = new 단문복문해석();
                        단문복문해석[1] = new 단문복문해석();

                        해석결과 += 단문복문해석[0].단문복문해석하기(단문들[0], 전체문맥) + " " + 단문복문해석[1].가정법해석하기(단문들[1], 전체문맥);
                    }
                    else
                    {
                        for (int i = 0; i < 단문들.Count; i++)
                        {
                            단문복문해석[i] = new 단문복문해석();

                            해석결과 += 단문복문해석[i].단문복문해석하기(단문들[i], 전체문맥) + " ";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 단문들.Count; i++)
                    {
                        단문복문해석[i] = new 단문복문해석();

                        해석결과 += 단문복문해석[i].단문복문해석하기(단문들[i], 전체문맥) + " ";
                    }
                }


                return 해석결과;
            }
        }

        public void 단문으로나누기(string 문자열, ref List<string> 단문들)
        {


            int 단계 = 0;   // 문법표지 안에 문법 표지가 들어갈 수 있기 때문에 만들어 놓은 설정입니다.

            bool 주어나온적있음 = false;
            bool 접속사나온적있음 = false;

            string 현재까지모아진문장 = "";

            for (int i = 0; i < 문자열.Count(); i++)
            {

                if (문자열.Substring(i).Left(2) == "ⓢ{")
                {
                    if (단계 == 0)
                    {
                        if(주어나온적있음)
                        {
                            단문들.Add(현재까지모아진문장);
                            현재까지모아진문장 = "";
                        }
                        주어나온적있음 = true;
                    }

                    단계++;
                }
                else if (변환.문자열.Left(문자열.Substring(i), 2) == "ⓥ{"){ 단계++;}
                else if (변환.문자열.Left(문자열.Substring(i), 2) == "ⓧ{"){ 단계++;}
                else if (변환.문자열.Left(문자열.Substring(i), 2) == "ⓒ{"){ 단계++;}
                else if (변환.문자열.Left(문자열.Substring(i), 2) == "ⓞ{"){ 단계++;}
                else if (변환.문자열.Left(문자열.Substring(i), 2) == "㉨{")
                {
                    if (단계 == 0)
                    {
                        if (주어나온적있음)
                        {
                            단문들.Add(현재까지모아진문장);
                            현재까지모아진문장 = "";
                        }
                        접속사나온적있음 = true;
                        주어나온적있음 = false;
                    }
                    단계++;
                }
                else if (변환.문자열.Left(문자열.Substring(i), 1) == "}"){ 단계--;}
                현재까지모아진문장 += 문자열.Substring(i, 1);
            }

            if (현재까지모아진문장 != "")
                단문들.Add(현재까지모아진문장);
        }
    }
	class 단문복문해석
	{
		#region 멤버변수
		public string _전체문맥 = "";
		public string _전체문장 = "";

		public string _전체문맥_소문자 = "";
		public string _전체문장_소문자 = "";

		public string _문장구조기호;
		public string _주어;
		public string _서술어;
		public string _목적어;
		public string _간접목적어;
		public string _직접목적어;

		public string _수식어앞;
		public string _수식어S뒤;
		public string _수식어V뒤;
		public string _수식어O뒤;
		public string _수식어C뒤;

		public string _미분석0;
		public string _미분석1;
		public string _미분석2;
		public string _미분석3;

		public string _보어;
		public string _보어잔재; // (C)

		public string _접속어;
		public bool _내부 = false;
		public string _는 = "";

		private 구문자동분석 _구문분석;
		#endregion
		public 단문복문해석()
		{
			_주어 = "";
			_서술어 = "";
			_목적어 = "";
			_간접목적어 = "";
			_직접목적어 = "";

			_보어 = "";
			_접속어 = "";

			_수식어앞 = "";
			_수식어S뒤 = "";
			_수식어V뒤 = "";
			_수식어O뒤 = "";
			_수식어C뒤 = "";

			_미분석0 = "";
			_미분석1 = "";
			_미분석2 = "";
			_미분석3 = "";


			_구문분석 = new 구문자동분석();
		}

		public string 수동태로바꾸기(string 문자열)
		{
			string 바꾼결과 = "";
			string 문장구조기호 = 문자열분석(문자열.Trim());

			if (문장구조기호 == "SVO")
			{

			}

			return 바꾼결과;
		}
		public string 가정법해석하기(string 문자열, string 전체문맥)
		{
			_전체문맥 = 전체문맥;
			string 해석결과 = "";
			string 명사구해석_처리결과로그 = "";

			_문장구조기호 = 문자열분석(문자열.Trim());

			if (_문장구조기호 == "SV")
			{
				string 주어해석결과 = 명사구해석(_주어, ref 명사구해석_처리결과로그);
				string 가정법자동사해석결과 = 가정법자동사해석(_서술어, 1);

				해석결과 = 주어해석결과.은는() + " " + 가정법자동사해석결과 + ".";
			}

			return 해석결과;
		}
		public string 단문복문내부해석하기(string 문자열)
		{
			bool 쌍따옴표로묶임 = false;
			if(문자열.StartsWith("\"") && 문자열.EndsWith("\"")){ 쌍따옴표로묶임 = true; 문자열 = 문자열.Substring(1, 문자열.Length - 2); }

			if (문자열.ToLower() == "her") return "그녀"; // 목적어 하나만 달랑 넘어왔을 때, "그녀의"로 해석되는 것을 막는다.

			단문복문해석 단문복문내부 = new 단문복문해석();
			단문복문내부._내부 = true;
			if(쌍따옴표로묶임)		return "\"" + 단문복문내부.단문복문해석하기(문자열, _전체문맥) + "\"";
			else					return 단문복문내부.단문복문해석하기(문자열, _전체문맥).Replace(".", "");
		}
		public string 단문복문해석하기(string 문장문자열, string 전체문맥)
		{
			#region 초기화
			string LOG = "";
			문장문자열 = 문장문자열.Trim();
			문장문자열 = 문장문자열.Replace("’", "\'");

			string 해석결과 = "";

			_전체문장 = 문장문자열.불필요제거();
			_전체문맥 = 전체문맥.불필요제거();

			_전체문장 = _전체문장.Replace("\n", " ");
			_전체문맥 = _전체문맥.Replace("\n", " ");

			_전체문장_소문자 = _전체문장.ToLower();
			_전체문맥_소문자 = _전체문맥.ToLower();

			_문장구조기호 = 문자열분석(문장문자열);


			if (_내부) _는 = "이가";
			else _는 = "은는";

			#endregion

			#endregion // 맨 앞줄에서 시작한다.

			#region 접속어해석
			string 접속어해석결과 = 접속어해석(_접속어); string 접속어소문자 = _접속어.ToLower();

			string 최적의미 = 최적의미추출(접속어해석결과);
			if (최적의미 != "")
			{
				접속어해석결과 = 최적의미;
			}

			if (접속어소문자 == "and" || 접속어소문자 == "but" || 접속어소문자 == "however" || 접속어소문자 == "so" || 접속어소문자 == "yet")
			{
				해석결과 = 접속어해석결과 + " ";
			}
			else if (접속어소문자 == "if" || 접속어소문자 == "like")
			{
				해석결과 = 접속어해석결과 + " ";
				_는 = "이가";
			}
			else if (접속어소문자 == "when" || 접속어소문자 == "as")
			{
				_는 = "이가";
			}
			else
				해석결과 = "";
			#endregion

			if (_문장구조기호 == "") { 해석결과 += 명사구해석(문장문자열, ref LOG); }
			if (_문장구조기호 == "_") { 해석결과 += 명사구해석(_미분석0, ref LOG); }
			else if (_문장구조기호 == "_X") { 해석결과 += _X(_미분석0, _수식어앞); }
			else if (_문장구조기호 == "X") { 해석결과 += X(_수식어앞, 1, "전체문장"); }
			else if (_문장구조기호 == "C") { 해석결과 += C(); }

			else if (_문장구조기호 == "V") { 해석결과 += V(); }

			else if (_문장구조기호 == "VO" || _문장구조기호 == "_VO" || _문장구조기호 == "+VO" || _문장구조기호 == "++VO")	{ 해석결과 += SVO(); }
			else if (_문장구조기호 == "VOX") { 해석결과 += SVO(); }
			else if (_문장구조기호 == "VOXX") { 해석결과 += SVO(); }
			else if (_문장구조기호 == "XVO" || _문장구조기호 == "+XVO" || _문장구조기호 == "++XVO") { 해석결과 += SVO(); }

			else if (_문장구조기호 == "SV" || _문장구조기호 == "+SV") { 해석결과 += SV(); }

			else if (_문장구조기호 == "XVS") { 해석결과 += VS(); }
			else if (_문장구조기호 == "XVSX") { 해석결과 += VS(); }
			else if (_문장구조기호 == "VSX") { 해석결과 += VSX(_서술어, _주어, _수식어S뒤); }

			else if (_문장구조기호 == "SVC" || _문장구조기호 == "+SVC") { 해석결과 += SVC(); }

			else if (_문장구조기호 == "CVS") { 해석결과 += CVS(_보어, _서술어, _주어); }
			else if (_문장구조기호 == "VSC") { 해석결과 += VSC(_서술어, _주어, _보어); }
			// 3형식
			else if (_문장구조기호 == "SVO" || _문장구조기호 == "+SVO" || _문장구조기호 == "++SVO") { 해석결과 += SVO(); }

			else if (_문장구조기호 == "O") { 해석결과 += O(); }

			else if (_문장구조기호 == "OXSV") { 해석결과 += OXSV(_목적어, _수식어O뒤, _주어, _서술어); }
			else if (_문장구조기호 == "OXSVX") { 해석결과 += OXSVX(_목적어, _수식어O뒤, _주어, _서술어, _수식어V뒤); }// What did you see on your trip?

			else if (_문장구조기호 == "SVID") { 해석결과 += SVID(_주어, _서술어, _간접목적어, _직접목적어); }
			else if (_문장구조기호 == "OSVX") { 해석결과 += OSVX(_목적어, _주어, _서술어, _수식어V뒤); }



			if (접속어소문자 == "like")
				해석결과 = 해석결과.다_빼기() + "는 것처럼";

			해석결과 = 해석결과후처리(해석결과);

			return 해석결과;
		}


		private string VSX(string 서술어, string 주어, string 수식어)
		{
			string 번역결과 = ""; if (통문작번역(서술어 + " " + 주어 + " " + 수식어, ref 번역결과)) return 번역결과;

			string 명사구해석_처리결과로그 = "";
			string 주어해석결과 = 명사구해석(주어, ref 명사구해석_처리결과로그);

			if (Be동사(서술어))
			{
				return 주어해석결과.이가() + " " + X(수식어, 1, "맨끝").점빼기() + " 있니?";
			}

			return "";
		}

		private string SVID(string 주어, string 서술어, string 간접목적어, string 직접목적어)
		{
			string 번역결과 = ""; if (통문작번역(주어 + " " + 서술어 + " " + 간접목적어 + " " + 직접목적어, ref 번역결과)) return 번역결과;

			string 주어해석_처리결과로그 = "";
			string 간접목적어해석_처리결과로그 = "";

			string 주어해석결과 = 명사구해석(주어, ref 주어해석_처리결과로그);

			string 간접목적어해석결과 = 명사구해석(간접목적어, ref 간접목적어해석_처리결과로그);

			string 직접목적어해석결과 = 단문복문내부해석하기(직접목적어);
			string 단어구절 = 단어구절여부(직접목적어);

			if (서술어 == "told")
				return 주어해석결과.는(_는) + " " + 간접목적어해석결과.에게() + " " + 직접목적어해석결과.을를(단어구절) + " 말했다.";
			else
				return "";
		}
		private string OXSVX(string 목적어, string 수식어, string 주어, string 서술어, string 수식어_1)
		{
			string 번역결과 = ""; if (통문작번역(목적어 + " " + 수식어 + " " + 주어 + " " + 서술어 + " " + 수식어_1, ref 번역결과)) return 번역결과;

			string 주어해석_처리결과로그 = "";
			string 목적어해석_처리결과로그 = "";
			string 수식어부사구해석_처리결과로그 = "";

			string 주어해석결과 = 명사구해석(주어, ref 주어해석_처리결과로그);
			string 을를이가 = "";
			bool 현재여부 = true;
			string 타동사해석결과 = 타동사해석(서술어, ref 을를이가, ref 현재여부);
			string 수식어해석결과_1 = 수식어부사구해석(서술어, "", 수식어_1, 3, ref 수식어부사구해석_처리결과로그);
			string 목적어해석결과 = 명사구해석(목적어, ref 목적어해석_처리결과로그);
			string 단어구절 = 단어구절여부(목적어);

			if ((목적어 == "What") && (수식어 == "did"))
			{
				return 주어해석결과.은는() + " " + 수식어해석결과_1 + " " + 목적어해석결과.을를(단어구절) + " " + 타동사해석결과.었니() + "?";
			}
			else if ((목적어 == "What") && (수식어 == "didn't") && (서술어 == "like")) // What didn't you like about the acting?
			{
				return 주어해석결과.은는() + " " + 수식어해석결과_1 + " 뭐가 마음에 안들었니?";
			}
			return "";
		}

		private string OXSV(string 목적어, string 수식어, string 주어, string 서술어)
		{
			string 번역결과 = ""; if (통문작번역(목적어 + " " + 수식어 + " " + 주어 + " " + 서술어, ref 번역결과)) return 번역결과;

			string 주어해석_처리결과로그 = "";
			string 주어해석결과 = 명사구해석(주어, ref 주어해석_처리결과로그);
			string 목적어해석_처리결과로그 = "";
			string 을를이가 = "";
			bool 현재여부 = true;
			string 타동사해석결과 = 타동사해석(서술어, ref 을를이가, ref 현재여부);
			string 목적어해석결과 = 명사구해석(목적어, ref 목적어해석_처리결과로그);
			string 단어구절 = 단어구절여부(목적어);

			if (목적어.StartsWith("Which ") && (수식어 == "did"))
			{
				return 주어해석결과.은는() + " " + 목적어해석결과.을를(단어구절) + " " + 타동사해석결과.었니() + "?";
			}

			return "";
		}

		private string O()
		{
			string 번역결과 = "";
			if (통문작번역(_수식어앞 + " " + _목적어 + " " + _수식어O뒤, ref 번역결과)) return 번역결과;

			string 수식어부사구해석_처리결과로그 = "";

			if (_목적어.ToLower() == "what" && _수식어O뒤.StartsWith("about "))
			{
				string 수식어해석결과 = 수식어부사구해석("", "", _수식어앞, 3, ref 수식어부사구해석_처리결과로그);
				string 수식어해석결과1 = 수식어부사구해석("", "", _수식어O뒤, 3, ref 수식어부사구해석_처리결과로그);
				if (_수식어앞 == "Then")
					return 수식어해석결과 + " " + 수식어해석결과1 + "는 어떻게 생각하니?";
				else
					return 수식어해석결과 + " " + 수식어해석결과1 + " 어떻게 생각하니?";
			}

			return "";
		}

		private string SVC()
		{
			string 번역결과 = "";

			if (통문작번역((_수식어앞 + " " + _주어 + " " + _서술어 + " " + _수식어V뒤 + " " + _보어 + " " + _수식어C뒤).다듬기(), ref 번역결과)) return 번역결과;

			string 로그 = "";

			string 수식어_앞뜻 = 수식어부사구해석(_서술어, "", _수식어앞, 2, ref 로그);

			string 주어뜻 = 명사구해석(_주어, ref 로그); if (_주어.ToLower() == "it") 주어뜻 = "";
			bool 이름여부 = false; if (로그.Contains("이름")) 이름여부 = true;

			bool 형용사여부 = false;

			bool 현재여부 = true;
			string 자동사해석결과 = 자동사해석(_서술어, 2, ref 현재여부);

			string 수식어_서술어_뒤뜻 = 수식어부사구해석(_서술어, "", _수식어V뒤, 2, ref 로그);

			if (_수식어V뒤 == "too") 수식어_서술어_뒤뜻 = "너무";
			if (_수식어V뒤 == "so") 수식어_서술어_뒤뜻 = "너무";

			string 수식어_끝뜻 = 수식어부사구해석(_서술어, _보어, _수식어C뒤, 2, ref 로그);
			string 보어뜻 = 보어해석(_보어, ref 형용사여부);
			string be주격보어뜻 = be주격보어해석(_보어, ref 형용사여부);

			if (_주어 == "What" && (_서술어 == "'s" || _서술어 == "is") && _보어 == "wrong") { return "뭐가 문제니?"; }
			else if (_주어 == "You" && (_서술어 == "'re" || _서술어 == "are") && _보어 == "right") { return "맞아."; }
			else if ((_서술어 == "was" || _서술어 == "were" || _서술어 == "had been"))
			{
				if (_수식어V뒤 == "not")
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + 보어뜻.이가() + " 아니었다.").다듬기();
				else if (_보어.Contains("ⓢ"))
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + 단문복문내부해석하기(_보어).었다(false) + ".").다듬기();
				else if (_보어 == "used")
				{
					if (앞의전치사만(_수식어C뒤) == "as") 수식어_끝뜻 = 명사구해석(앞의전치사빼고남은수식어구(_수식어C뒤), ref 로그).로();
					else if (앞의전치사만(_수식어C뒤) == "by") 수식어_끝뜻 = 명사구해석(앞의전치사빼고남은수식어구(_수식어C뒤), ref 로그) + "에 의해";
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_끝뜻 + " " + 수식어_서술어_뒤뜻 + " " + " 사용되었다".다면(_접속어, 현재여부)).다듬기();
				}
				else
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + be주격보어뜻.었다(형용사여부).다면(_접속어, 현재여부)).다듬기();
			}
			else if ((_서술어 == "ⓧ{could} go") && (_보어 == "bad"))
				return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + " 상할 수 있었다".다면(_접속어, 현재여부)).다듬기();
			else if ((_서술어.ToLower().Replace("ⓧ{", "").Replace("}", "") == "should be"))
				return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + be주격보어뜻.어야한다(형용사여부) + ".").다듬기();
			else if ((_서술어.ToLower().Replace("ⓧ{", "").Replace("}", "") == "may feel") || (_서술어.ToLower().Replace("ⓧ{", "").Replace("}", "") == "may get"))
			{
				return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + be주격보어뜻.이다(형용사여부).ㄹ_수_있다() + ".").다듬기();
			}
			else if (_서술어 == "became" || _서술어 == "got" || _서술어 == "grew" || (_서술어.ToLower().Replace("ⓧ{", "").Replace("}", "") == "have become") || (_서술어 == "is" && _수식어V뒤 == "now")) // Be 동사 현재보다 앞에 와야 한다.
			{
				if (형용사여부) // "관심을 가지게 되었다"와 같은 시리즈는 자연스럽다. "춥게 되었다"와 같은 것은 추워졌다로 바꾼다.
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + 서술적용법_부사느낌_해석(_보어).되었다().다면(_접속어, 현재여부)).다듬기();
				else
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + 보어뜻.이가() + " 되었다".다면(_접속어, 현재여부)).다듬기();
			}
			else if (Be동사현재(_서술어))
			{
				if (_수식어C뒤.ToLower() == "too")
					return (수식어_앞뜻 + " " + 주어뜻.도(이름여부) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + be주격보어뜻.이다(형용사여부).다면(_접속어, 현재여부)).다듬기();
				else if (_수식어V뒤 == "not" || _서술어 == "isn't")
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + be주격보어뜻.이_아니다(형용사여부)).다듬기();
				else
					return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + be주격보어뜻.이다(형용사여부).다면(_접속어, 현재여부)).다듬기();
			}
			else if (_보어.Contains("ⓢ")) // ⓢ{The actors} ⓥ{sounded} ⓒ{㉨{like} ⓢ{they} ⓥ{were reading} ⓧ{from a book}}.
			{
				string 보어절해석 = 단문복문내부해석하기(_보어);

				return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + 보어절해석 + " " + 자동사해석결과 + ".").다듬기();
			}
			else
			{
				string 서술적용법_부사느낌_해석결과 = 서술적용법_부사느낌_해석(_보어);

				return (수식어_앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어_서술어_뒤뜻 + " " + 수식어_끝뜻 + " " + 서술적용법_부사느낌_해석결과 + " " + 자동사해석결과.다면(_접속어, 현재여부)).다듬기();
			}
		}

		private string VS()
		{
			string 번역결과 = "";
			if (통문작번역(_수식어앞 + " " + _서술어 + " " + _주어 + " " + _수식어S뒤, ref 번역결과)) return 번역결과;

			string 주어해석_처리결과로그 = "", 수식어부사구해석_처리결과로그 = "";
			string 주어해석결과 = 명사구해석(_주어, ref 주어해석_처리결과로그);
			string 수식어S뒤뜻 = 수식어부사구해석(_서술어, "", _수식어S뒤, 1, ref 수식어부사구해석_처리결과로그);

			if (_수식어앞.ToLower() == "there")
			{
				if (Be동사현재(_서술어)) { return 수식어S뒤뜻 + " " + 주어해석결과.이가() + " " + "있다."; }
				else if (Be동사과거(_서술어)) { return 수식어S뒤뜻 + " " + 주어해석결과.이가() + " " + "있었다."; }
				else return "";
			}
			else if (_수식어앞.ToLower() == "here")
			{
				if (Be동사현재(_서술어)) { return "여기에 " + 수식어S뒤뜻 + " " + 주어해석결과.이가() + " " + "있다."; }
				else if (Be동사과거(_서술어)) { return "여기에 " + 수식어S뒤뜻 + " " + 주어해석결과.이가() + " " + "있었다."; }
				else return "";
			}
			else
				return "";
		}

		private string SV()
		{
			string 번역결과 = "";
			if (통문작번역((_수식어앞 + " " + _주어 + " " + _서술어 + " " + _수식어V뒤).다듬기(), ref 번역결과)) return 번역결과;

			string 주어해석_처리결과로그 = "";
			string 수식어부사구해석_처리결과로그 = "";

			string 수식어_앞_뜻 = 수식어부사구해석(_서술어, "", _수식어앞, 1, ref 수식어부사구해석_처리결과로그);
			string 주어_뜻 = 명사구해석(_주어, ref 주어해석_처리결과로그);

			bool 현재여부 = true;
			string 자동사_뜻 = 자동사해석(_서술어, 1, ref 현재여부);
			string 수식어_끝_뜻 = 수식어부사구해석(_서술어, "", _수식어V뒤, 1, ref 수식어부사구해석_처리결과로그);
			string 서술어 = _서술어.ToLower().불필요제거();

			if ((_수식어앞.ToLower().문법표지제거() == "where did")) // Where did you go on your trip?
			{
				//해석결과 = 주어해석결과.은는() + " " + 수식어구의_명사구만.로() + " 어디에 " + 자동사해석결과_1형식.었니() + "?";
				return 주어_뜻.은는() + " " + 수식어_끝_뜻 + " 어디에 " + 자동사_뜻.었니() + "?";
			}
			else if (_수식어앞.ToLower().문법표지제거() == "did")
			{
				return 주어_뜻.은는() + " " + 수식어_끝_뜻 + " " + 자동사_뜻.었니() + "?";
			}
			else if (_수식어앞.ToLower().문법표지제거() == "do")
			{
				return 주어_뜻.은는() + " " + 수식어_끝_뜻 + " " + 자동사_뜻.니() + "?";
			}
			else if (_주어.ToLower() == "what")
			{
				return "무엇이 " + 자동사_뜻.는가() + ".";
			}
			else if (_수식어앞.ToLower().문법표지제거() == "No" && (_서술어.문법표지제거() == "didn't" || _서술어.문법표지제거() == "don't")) return "아니.";
			else if (_수식어앞.ToLower().문법표지제거() == "Yes" && (_서술어.문법표지제거() == "do" || _서술어.문법표지제거() == "did")) return "응.";
			else if (수식어부사구해석_처리결과로그 == "형용사구")
			{
				return SVC();
			}
			else if(서술어.StartsWith("'ll ") || 서술어.StartsWith("will "))
			{
				return (수식어_앞_뜻 + " " + 주어_뜻.는(_는) + " " + 수식어_끝_뜻 + " " + 자동사_뜻.다면(_접속어, 현재여부)).다듬기(); // 다()만 빠진 형태다. ㄴ다로 바꿔주는 특징이 있다.
			}
			else
				return (수식어_앞_뜻 + " " + 주어_뜻.는(_는) + " " + 수식어_끝_뜻 + " " + 자동사_뜻.다().다면(_접속어, 현재여부)).다듬기();
		}

		private string V()
		{
			string 번역결과 = "";
			if (통문작번역(_서술어 + " " + _수식어V뒤, ref 번역결과)) return 번역결과;

			string 수식어부사구해석_처리결과로그 = "";

			bool 현재여부 = true;
			string 자동사해석결과 = 자동사해석(_서술어, 1, ref 현재여부);

			#region 수식어V뒤뜻
			string 수식어V뒤뜻 = "";
			List<string> 수식어V뒤_수식어목록 = new List<string>();
			List<string> 수식어V뒤_수식어목록뜻 = new List<string>();

			수식어들나누기(_수식어V뒤, ref 수식어V뒤_수식어목록);

							for(int i = 0; i < 수식어V뒤_수식어목록.Count; i++){	수식어V뒤_수식어목록뜻.Add(수식어부사구해석(_서술어, "", 수식어V뒤_수식어목록[i], 1, ref 수식어부사구해석_처리결과로그));	}
			수식어V뒤뜻 = "";	for(int i = 수식어V뒤_수식어목록뜻.Count - 1; i > -1; i--){ 수식어V뒤뜻 += 수식어V뒤_수식어목록뜻[i] + " ";	}
			#endregion
			if (_서술어.ToLower() == "welcome" && _수식어V뒤.ToLower().StartsWith("to ")){			return (수식어V뒤뜻 + "오신 것을 환영합니다.").다듬기();		}
			else if (_내부)																			return (수식어V뒤뜻 + " " + 자동사해석결과 + ".").다듬기();
			else																					return (수식어V뒤뜻 + " " + 자동사해석결과.하라()).다듬기();
		}

		private string OSVX(string 목적어, string 주어, string 서술어, string 수식어)
		{
			string 주어해석_처리결과로그 = "";
			string 주어해석결과 = 명사구해석(_주어, ref 주어해석_처리결과로그);

			string 을를이가 = "";
			bool 현재여부 = true;
			 string 타동사해석결과 = 타동사해석(서술어, ref 을를이가, ref 현재여부);

			string 수식어부사구해석_처리결과로그 = "";
			string 수식어해석결과 = 수식어부사구해석(서술어, "", 수식어, 3, ref 수식어부사구해석_처리결과로그);


			if (목적어.ToLower() == "that" || 목적어.ToLower() == "which")
			{
				return 주어해석결과.이가() + " " + 수식어해석결과 + " " + 타동사해석결과.형용사화();
			}
			else
			{
				return "";
			}
		}

		private string CVS(string 보어, string 서술어, string 주어)
		{
			string 번역결과 = "";
			if (통문작번역(보어 + " " + 서술어 + " " + 주어, ref 번역결과)) return 번역결과;

			string 주어해석_처리결과로그 = "";
			string 주어해석결과 = 명사구해석(_주어, ref 주어해석_처리결과로그);


			if (보어 == "What" && Be동사과거(서술어)) { return "뭐가 " + 주어해석결과.Replace("너의 ", "").이었니() + "?"; }
			else if (보어 == "What" && Be동사현재(서술어)) { return 주어해석결과.이가() + " 뭐니?"; }

			return "";
		}

		private bool 통문작번역(string 원문, ref string 번역결과)
		{
			원문 = 원문.Replace(" 's ", "'s ");

			string 의미 = Form1._검색.문장의미추출(원문);

			if (의미 != "")
			{
				string 최적의미 = 최적의미추출(의미);

				if (최적의미 != "")
				{
					의미 = 최적의미;
				}

				번역결과 = 의미 + ".";
				return true;
			}
			else
				return false;
		}

		private string VSC(string 서술어, string 주어, string 보어)
		{
			string 번역결과 = "";
			if (통문작번역(서술어 + " " + 주어 + " " + 보어, ref 번역결과)) return 번역결과;

			bool 형용사여부 = false;

			string 주어해석_처리결과로그 = "";
			string 주어해석결과 = 명사구해석(_주어, ref 주어해석_처리결과로그);

			string be주격보어해석결과 = be주격보어해석(보어, ref 형용사여부);

			if (Be동사(서술어))
			{
				if (주어.ToLower() != "it")
					return 주어해석결과.이가() + " " + be주격보어해석결과.인가(형용사여부);
				else
					return be주격보어해석결과.인가(형용사여부);
			}

			return "";
		}


		private string SVO()
		{
			string 번역결과 = "";
			if (통문작번역((_수식어앞 + " " + _주어 + " " + _수식어S뒤 + " " + _서술어 + " " + _수식어V뒤 + " " + _목적어 + " " + _수식어O뒤).다듬기(), ref 번역결과)) return 번역결과;

			string LOG = "";
			string 주어 = _주어.ToLower();
			string 주어뜻 = 명사구해석(_주어, ref LOG);			bool 이름여부 = false; if (LOG.Contains("이름")) 이름여부 = true;

			string 서술어 = _서술어.ToLower().불필요제거();
			string 목적어 = _목적어.ToLower();
			string 목적어뜻 = 단문복문내부해석하기(_목적어);
			if (목적어뜻.Contains("없는 ") && _목적어.StartsWith("no ") && (서술어 == "have" || 서술어 == "has" || 서술어 == "had"))
			{
				목적어뜻 = 목적어뜻.Substring(3);
			}

			string 단어구절 = 단어구절여부(_목적어);

			string 을를이가 = "";

			bool 현재여부 = true;
			string 타동사뜻 = 타동사해석(_서술어, ref 을를이가, ref 현재여부);

			string 수식어앞 = _수식어앞.ToLower().불필요제거();
			#region 수식어앞뜻
			string 수식어앞뜻 = "";
			List<string> 수식어앞_수식어목록 = new List<string>();
			List<string> 수식어앞_수식어목록뜻 = new List<string>();

			수식어들나누기(_수식어앞, ref 수식어앞_수식어목록);

			for (int i = 0; i < 수식어앞_수식어목록.Count; i++) { 수식어앞_수식어목록뜻.Add(수식어부사구해석(_서술어, "", 수식어앞_수식어목록[i], 3, ref LOG)); }
			수식어앞뜻 = ""; for (int i = 0; i < 수식어앞_수식어목록뜻.Count; i++) 
			{
				if (i == 0)
				{
					if (_주어 == "" && (수식어앞_수식어목록[0].ToLower() == "then"))
						수식어앞뜻 += "그다음, ";
					else
						수식어앞뜻 += 수식어앞_수식어목록뜻[i] + " ";
				}
				else
				{
					수식어앞뜻 += 수식어앞_수식어목록뜻[i] + " ";
				}
			}
			#endregion

			#region 수식어V뒤뜻
			string 수식어V뒤뜻 = "";
			List<string> 수식어V뒤_수식어목록 = new List<string>();
			List<string> 수식어V뒤_수식어목록뜻 = new List<string>();

			수식어들나누기(_수식어V뒤, ref 수식어V뒤_수식어목록);

			for (int i = 0; i < 수식어V뒤_수식어목록.Count; i++) { 수식어V뒤_수식어목록뜻.Add(수식어부사구해석(_서술어, "", 수식어V뒤_수식어목록[i], 3, ref LOG)); }
			수식어V뒤뜻 = ""; for (int i = 수식어V뒤_수식어목록뜻.Count - 1; i > -1; i--) { 수식어V뒤뜻 += 수식어V뒤_수식어목록뜻[i] + " "; }
			#endregion

			#region 수식어S뒤뜻
			string 수식어S뒤뜻 = "";
			List<string> 수식어S뒤_수식어목록 = new List<string>();
			List<string> 수식어S뒤_수식어목록뜻 = new List<string>();

			수식어들나누기(_수식어S뒤, ref 수식어S뒤_수식어목록);
			for (int i = 0; i < 수식어S뒤_수식어목록.Count; i++) { 수식어S뒤_수식어목록뜻.Add(수식어부사구해석(_서술어, "", 수식어S뒤_수식어목록[i], 3, ref LOG)); }
			수식어S뒤뜻 = ""; for (int i = 수식어S뒤_수식어목록뜻.Count - 1; i > -1; i--) { 수식어S뒤뜻 += 수식어S뒤_수식어목록뜻[i] + " "; }
			#endregion

			bool 부정여부 = false; if (_수식어S뒤 == "never") { 수식어S뒤뜻 = "절대로"; 부정여부 = true; }

			#region 수식어O뒤뜻
			string 수식어O뒤뜻 = "";
			List<string> 수식어O뒤_수식어목록 = new List<string>();
			List<string> 수식어O뒤_수식어목록뜻 = new List<string>();

			수식어들나누기(_수식어O뒤, ref 수식어O뒤_수식어목록);
			for (int i = 0; i < 수식어O뒤_수식어목록.Count; i++) { 수식어O뒤_수식어목록뜻.Add(수식어부사구해석(_서술어, "", 수식어O뒤_수식어목록[i], 3, ref LOG)); }
			수식어O뒤뜻 = ""; 
			for (int ᚼ = 수식어O뒤_수식어목록뜻.Count - 1; ᚼ > -1; ᚼ--) 
			{ 
				if(ᚼ != 0)
				{
					수식어O뒤뜻 += 수식어O뒤_수식어목록뜻[ᚼ] + " ";
				}
				else
				{
					if (_목적어.EndsWith("book")) { 수식어O뒤뜻 += 단문복문내부해석하기(수식어O뒤_수식어목록뜻[ᚼ]).라는(); }
					else 수식어O뒤뜻 += 수식어O뒤_수식어목록뜻[ᚼ] + " ";
				}
			}
			#endregion
			if (_주어 == "") // 명령형일 때, ᖇᓮᖙᙓ Ꭹᗢ⋒ᖇ ᗯᗩᐯᙓᔕ.
			{
				_주어 = " ";
				if (_내부)												return SVO();
				else													return SVO().점빼기().하라() + ".";
			}
			else if ((서술어=="loved" || 서술어=="liked") && 목적어=="it" && 주어=="i") 
																		return (수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + "좋았다".부정(부정여부).다면(_접속어, 현재여부)).다듬기();
			else if (서술어 == "need" && 단어구절 == "구")				return (주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.해야한다().부정(부정여부).다면(_접속어, 현재여부)).다듬기();
			else if (서술어 == "needed" && 단어구절 == "구")			return (주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.해야했다().부정(부정여부).다면(_접속어, 현재여부)).다듬기();
			else if (서술어 == "have" && _목적어.StartsWith("no "))		return (주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.이가() + " " + "없다".부정(부정여부) + ".").다듬기();
			else if (서술어 == "has" && _목적어.StartsWith("no "))		return 주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.이가() + " " + "없다".부정(부정여부) + ".";
			else if (서술어 == "had" && _목적어.StartsWith("no "))		return 주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.이가() + " " + "없었다".부정(부정여부) + ".";
			else if (서술어 == "agreed")								return 주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.기로() + " " + "했다".부정(부정여부).다면(_접속어, 현재여부);
			else if (서술어.EndsWith(" ask"))							return (주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.에게() + " " + 타동사뜻.부정(부정여부).다면(_접속어, 현재여부)).다듬기();
			else if (_수식어S뒤.ToLower() == "too")						return 주어뜻.도(이름여부) + " " + 목적어뜻.을를(단어구절) + " " + 타동사뜻.부정(부정여부).다면(_접속어, 현재여부);
			else if ((수식어앞 == "how can") && (주어 == "i"))			return (목적어뜻.을를(단어구절) + " " + 수식어O뒤뜻 + " 어떻게 " + 수식어S뒤뜻 + " " +타동사뜻.할수있다().니() + "?").다듬기();
			else if ((수식어앞 == "how did") && (서술어 == "like"))		return 주어뜻.는(_는) + " " + 목적어뜻.이가() + " 어땠니?";
			else if (수식어앞.StartsWith("how ") && 수식어앞.Length > 6){ // how로 이외에도 뭔가가 있는 경우다.
				수식어O뒤뜻 = X(_수식어O뒤, 3, "맨끝").점빼기(); 수식어앞뜻 = X(_수식어앞, 3, "맨앞").점빼기(); _수식어앞 = ""; _수식어O뒤 = "";
																		return 수식어O뒤뜻 + " " + 수식어앞뜻 + " " + SVO();}
			else if (수식어앞 == "did")									return 주어뜻.는(_는) + " " + 수식어O뒤뜻 + " " + 목적어뜻.을를(단어구절) + " " + 타동사뜻.었니() + "?";
			else if (수식어앞 == "do"){
				if (서술어 == "have")									return 주어뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.이가() + " " + "있니?";
				else													return 주어뜻.는(_는) + " " + 수식어O뒤뜻 + " " + 목적어뜻.을를(단어구절) + " " + 타동사뜻.니() + "?";}
			else if (수식어앞 == "can"){
				if (_주어.ToLower() == "you"){
					if (서술어 == "say")								return 목적어뜻.을를(단어구절) + " " + 수식어O뒤뜻 + " " + "말해줄 수 있니?"; // can you say that again? 
					else												return 목적어뜻.을를(단어구절) + " " + 수식어O뒤뜻 + " " + 타동사뜻.할수있다().니() + "?";}
				else													return 주어뜻.는(_는) + " " + 수식어O뒤뜻 + " " + 목적어뜻.을를(단어구절) + " " + 타동사뜻.할수있다().니() + "?";}
			else if (말하기관련(서술어)) return (수식어앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.라고() + " " + 타동사뜻.부정(부정여부).다().다면(_접속어, 현재여부)).다듬기();
			else return (수식어앞뜻 + " " + 주어뜻.는(_는) + " " + 수식어S뒤뜻 + " " + 수식어O뒤뜻 + " " + 목적어뜻.을를(단어구절, 을를이가) + " " + 타동사뜻.부정(부정여부).다().다면(_접속어, 현재여부)).다듬기();
		}

	
		private string C()
		{
			string 번역결과 = "";
			if (통문작번역(_수식어앞 + " " + _보어 + " " + _수식어C뒤, ref 번역결과)) return 번역결과;

			#region 수식어앞뜻
			string 수식어앞뜻 = "", LOG = "";
			List<string> 수식어앞_수식어목록 = new List<string>();
			List<string> 수식어앞_수식어목록뜻 = new List<string>();

			수식어들나누기(_수식어앞, ref 수식어앞_수식어목록);

			for (int i = 0; i < 수식어앞_수식어목록.Count; i++) { 수식어앞_수식어목록뜻.Add(수식어부사구해석("", "", 수식어앞_수식어목록[i], 3, ref LOG)); }
			수식어앞뜻 = ""; for (int i = 0; i < 수식어앞_수식어목록뜻.Count; i++)
			{
				수식어앞뜻 += 수식어앞_수식어목록뜻[i] + " ";
			}
			#endregion
			#region 수식어C뒤뜻
			string 수식어C뒤뜻 = "";
			List<string> 수식어C뒤_수식어목록 = new List<string>();
			List<string> 수식어C뒤_수식어목록뜻 = new List<string>();

			수식어들나누기(_수식어C뒤, ref 수식어C뒤_수식어목록);

			for (int i = 0; i < 수식어C뒤_수식어목록.Count; i++) { 수식어C뒤_수식어목록뜻.Add(수식어부사구해석("", "", 수식어C뒤_수식어목록[i], 3, ref LOG)); }
			수식어C뒤뜻 = ""; 

			for (int i = 수식어C뒤_수식어목록뜻.Count - 1; i > -1; i--)
			{
				if (i == 수식어C뒤_수식어목록.Count - 1 && 수식어C뒤_수식어목록[수식어C뒤_수식어목록.Count - 1] == "too")
					수식어C뒤뜻 += "나도 ";
				else
					수식어C뒤뜻 += 수식어C뒤_수식어목록뜻[i] + " ";

			}

			#endregion

			bool 형용사여부 = false;

			string 감탄 = 감탄사뜻(_보어);
			if (감탄 != "")				return (수식어C뒤뜻 + 수식어앞뜻 + 감탄 + "!").다듬기();
			else
			{
				string be주격보어해석결과 = be주격보어해석(_보어, ref 형용사여부);
				return (수식어C뒤뜻 + 수식어앞뜻 + be주격보어해석결과.이다(형용사여부) + ".").다듬기();
			}

		}

		private string _X(string 미분석, string 수식어)
		{
			string 로그 = "";
			return 단문복문내부해석하기(수식어).하는() + " " + 명사구해석(미분석, ref 로그);
		}

		private string X(string 수식어, int 형식, string 위치)
		{
			List<string> 수식어목록 = new List<string>();			수식어들나누기(수식어, ref 수식어목록);

			if (수식어목록.Count > 1)
			{
				string 결과 = "";


				for (int i = 수식어목록.Count - 1; i > -1; i--)
				{
					if (위치 == "맨앞" && i == 0)
						결과 += X(수식어목록[i], 형식, "맨앞") + " ";
					else if (위치 == "맨끝" && i == 수식어목록.Count - 1)
						결과 += X(수식어목록[i], 형식, "맨끝") + " ";
					else
						결과 += X(수식어목록[i], 형식, "중간") + " ";
				}

				return 결과;
			}
			else if (수식어목록.Count == 1)
			{
				수식어 = 수식어목록[0];

				string 수식어부사구해석_처리결과로그 = "";

				if (수식어.ToLower().StartsWith("how to ⓥ{"))
				{
					string s = 수식어.Substring(6, 수식어.Length - 7);
					return To부정사_형용사적용법(s) + " 방법";
				}
				else if (수식어.ToLower() == "too")
				{
					if (위치 == "맨끝")
						return "또한";
					else
						return "너무";
				}
				else if (형식 == 2 && 수식어.Contains("ⓥ"))
				{
					return To부정사_부사적용법_이유(수식어);

				}
				else
				{
					string 이름해석사전전체 = Form1._검색.영한사전_문장부호제거(수식어);

					if (이름해석사전전체.Contains("사람이름"))
					{
						string 이름해석_처리결과로그 = "";

						string 이름해석결과 = 명사구해석(수식어, ref 이름해석_처리결과로그);

						return 이름해석결과.아야();
					}


					if (!수식어.Contains("ⓧ{"))
					{
						string 수식어해석결과 = 수식어부사구해석("", "", 수식어, 형식, ref 수식어부사구해석_처리결과로그);

						return 수식어해석결과 + ".";
					}
					else
					{
						string 앞부분 = 수식어.Substring(0, 수식어.IndexOf("ⓧ{"));
						string 뒷부분 = 수식어.Substring(수식어.IndexOf("ⓧ{"));

						뒷부분 = 뒷부분.Substring(2, 뒷부분.Length - 3);

						return 형용사구절해석(뒷부분) + " " + 단어뿐인_명사구해석(앞부분);
					}
				}
			}
			else return "";
		}

		private string To부정사_부사적용법_이유(string 수식어)
		{
			return 단문복문내부해석하기(수식어).해서();
		}

		private string To부정사_형용사적용법(string 수식어)
		{
			return 단문복문내부해석하기(수식어).하는();
		}



		// 수식어 항목들을 나누어 목록에 저장한다. ⓧ{항목 1} ⓧ{항목 2}
		private void 수식어들나누기(string s, ref List<string> 수식어목록)
		{
			string 결과 = "", 표지 = ""; /* 현재문법표지 */	int 단계 = 0 /*문법표지 안에 문법 표지가 들어갈 수 있기 때문에 만들어 놓은 설정입니다.*/, 위치 = 0;

			for (int i = 0; i < s.Count(); i++)
			{
				if (s.Substring(i).Left(2) == "ⓢ{") { if (단계 == 0) { 표지 = "주어"; 위치 = i + 2; } 단계++; i++; }
				else if (s.Substring(i).Left(2) == "ⓒ{") { if (단계 == 0) { 표지 = "보어"; 위치 = i + 2; } 단계++; i++; }
				else if (s.Substring(i).Left(2) == "ⓘ{") { if (단계 == 0) { 표지 = "간접목적어"; 위치 = i + 2; } 단계++; i++; }
				else if (s.Substring(i).Left(2) == "ⓓ{") { if (단계 == 0) { 표지 = "직접목적어"; 위치 = i + 2; } 단계++; i++; }
				else if (s.Substring(i).Left(4) == "(ⓒ){") { if (단계 == 0) { 표지 = "보어잔재"; 위치 = i + 4; } 단계++; i += 3; }
				else if (s.Substring(i).Left(2) == "ⓞ{") { if (단계 == 0) { 표지 = "목적어"; 위치 = i + 2; } 단계++; i++; }
				else if (s.Substring(i).Left(2) == "㉨{") { if (단계 == 0) { 표지 = "접속어"; 위치 = i + 2; } 단계++; i++; }
				
				else if (s.Substring(i).Left(2) == "ⓧ{") { if (단계 == 0) { if (표지 == "미분석") { if (_미분석0 == "") _미분석0 = s.Substring(위치, i - 위치); else if (_미분석1 == "") _미분석1 = s.Substring(위치, i - 위치); else if (_미분석2 == "") _미분석2 = s.Substring(위치, i - 위치); 결과 += "_"; } 표지 = "수식어"; 위치 = i; } 단계++; i++; }
				else if (s.Substring(i).Left(1) == "}")
				{
					단계--; if (단계 == 0)
					{
						if (표지 == "수식어")
						{
							수식어목록.Add(s.Substring(위치, i - 위치 + 1));
						}

						표지 = ""; /*초기화*/
					}
				}
			}
			_수식어앞 = _수식어앞.Trim();
			_수식어S뒤 = _수식어S뒤.Trim();
			_수식어V뒤 = _수식어V뒤.Trim();
			_수식어O뒤 = _수식어O뒤.Trim();
			_수식어C뒤 = _수식어C뒤.Trim();
		}

		public string 해석결과후처리(string 해석결과)
		{

			해석결과 = 해석결과.Replace(" 운이었다.", " 우니었다.");
            해석결과 = 해석결과.Replace("은이었다.", "았다."); // 좋은+이었다 = 좋았다
            해석결과 = 해석결과.Replace("나쁜이었다.", "나빴다."); // 나쁜+이었다 = 나빴다

            해석결과 = 해석결과.Replace("운이었다.", "웠다.");

			해석결과 = 해석결과.Replace("된이었다.", "되었다.");

			해석결과 = 해석결과.Replace(" 우니었다.", " 운이었다.");

            if(해석결과.Contains("너는"))
                해석결과 = 해석결과.Replace("너의 ", "");

            해석결과 = 해석결과.Replace("가었", "갔었"); //가다+었다 > 갔었다
            해석결과 = 해석결과.Replace("보었", "봤");

            해석결과 = 해석결과.Replace("었는 것", "었던 것"); // ~었던 것처럼 들렸다.

            해석결과 = 해석결과.Replace("으로부터 읽는", "을 읽는"); // 책으로부터 읽는 > 책을 읽는

            해석결과 = 해석결과.Replace("감기를 걸", "감기에 걸"); //
            해석결과 = 해석결과.Replace("는 것을 원하지", "고 싶지"); // 돌아가는 것을 원하지 않았다. > 돌아가고 싶지 않았다.
            return 해석결과;
		}

		public string 앞의전치사빼고남은수식어구(string 수식어구)
		{
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
                };

            foreach (string 현재전치사 in 전치사목록)
            {
                if (수식어구.ToLower().StartsWith(현재전치사 + " "))
                    return 수식어구.ToLower().Substring(현재전치사.Length + 1);
            }

            return 수식어구;
		}

        public string 앞의전치사만(string 수식어구)
        {
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
                };

            foreach (string 현재전치사 in 전치사목록)
            {
                if (수식어구.ToLower().StartsWith(현재전치사 + " "))
                    return 현재전치사;
            }

            return 수식어구;
        }
		public string 와과(string 단어)
		{
			if (글자종성있는지여부(변환.문자열.Right(단어, 1)))
				return "과";
			else
				return "와";
		}
        private string 가정법자동사해석(string 자동사, int 형식)
        {
			bool 현재여부 = true;

            List<string> 어절들 = new List<string>();
            변환.문자열.어절들로(자동사, ref 어절들);

            string 자동사의미 = "";

            if (어절들.Count == 2)
            {
                자동사 = 자동사.Replace("ⓧ{", "").Replace("}", "");

                if (자동사.StartsWith("would "))
                {
                    자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).다_빼기() + " 수 있을텐데";
                }

                return 자동사의미;

            }

            return 자동사;
        }
        private bool ING형(string 현재어절)
        {
            
            string 현재어절검색결과 = Form1._검색.영한사전_문장부호제거(현재어절);

            if (현재어절검색결과.Contains("ing형"))
            {
                return true;
            }
            else
                return false;
        }
        private bool Be동사현재(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();
            if (현재어절 == "\'m") return true;
            if (현재어절 == "am") return true;
            if (현재어절 == "\'s") return true;
            if (현재어절 == "is") return true;
            if (현재어절 == "are") return true;
			if (현재어절 == "isn't") return true;
			


			return false;
        }
        private bool Be동사과거(string 현재어절)
        {
            현재어절 = 현재어절.ToLower();

            if (현재어절 == "was") return true;
            if (현재어절 == "were") return true;

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
        private string 서술적용법_부사느낌_해석(string 서술적용법형용사)
        {
            string 서술적용법의미 = 서술적용법뜻(서술적용법형용사);

			//는으로 끝나면 게를 붙여준다. 흥미를 느끼는 > 흥미를 느끼게
			if (서술적용법의미.EndsWith("는")) return 서술적용법의미.Left(서술적용법의미.Length - 1) + "게";

			if (서술적용법의미.EndsWith("운"))
			{
				//서술적용법의미 = 서술적용법의미.Substring(0, 서술적용법의미.Length - 1);

				//string 단어맨마지막글자 = 변환.문자열.Right(서술적용법의미, 1);
				//서술적용법의미 = 서술적용법의미.Substring(0, 서술적용법의미.Length - 1);
				//return 서술적용법의미 + 문자열.초중종_자소합치기(단어맨마지막글자.초성추출(), 단어맨마지막글자.중성추출(), "ㅂ") + "게";
				return 서술적용법의미.Substring(0, 서술적용법의미.Length - 1) + "워";
			}

			서술적용법의미 += "※";

			서술적용법의미 = 서술적용법의미.Replace("은※", "아"); // 좋은 > 좋아
			서술적용법의미 = 서술적용법의미.Replace("쁜※", "쁘게"); 

            서술적용법의미 = 서술적용법의미.Replace("한※", "하게");
            서술적용법의미 = 서술적용법의미.Replace("려운※", "렵게"); // 두려운 > 두렵게


            서술적용법의미 = 서술적용법의미.Replace("※", "");



            return 서술적용법의미;
        }

        private string 단어구절여부(string 목적어문장)
        {
            if (목적어문장.StartsWith("to ⓥ{")) return "구";
            if(!목적어문장.StartsWith("ⓢ") && !목적어문장.StartsWith("ⓥ") && !목적어문장.StartsWith("ⓞ") && !목적어문장.StartsWith("ⓒ")){ return "단어"; }



            if (목적어문장.Contains("ⓢ")) return "절";
            else if (목적어문장.Contains("ⓥ")) return "구";
            else return "단어";
        }
		private string 자동사해석(string 자동사, int 형식, ref bool 현재여부)
		{
			현재여부 = true; // 과거로 검색될 때만 표시해준다.
			List<string> 어절들 = new List<string>();
			변환.문자열.어절들로(자동사, ref 어절들);

			string 자동사의미 = "";

			if (어절들.Count == 5)
			{
				자동사 = 자동사.Replace("ⓧ{", "").Replace("}", "");

				if (자동사.StartsWith("will be able to "))
				{
					자동사의미 += 자동사뜻(어절들[4], 형식, ref 현재여부).ㄹ_수_있을_것이다();
				}

				return 자동사의미;
			}
			else if (어절들.Count == 4)
			{
				자동사 = 자동사.Replace("ⓧ{", "").Replace("}", "");

				if (자동사.StartsWith("should have "))
				{
					자동사의미 += 자동사뜻(어절들[2] + " " + 어절들[3], 형식, ref 현재여부).다_빼기() + "었어야 했다";
				}

				return 자동사의미;
			}
			else if (어절들.Count == 3)
			{
				string 부사의미 = 부사뜻(어절들[1].Replace("ⓧ{", "").Replace("}", ""));

				if (자동사.StartsWith("ⓧ{may}") && 부사의미 != "")
				{
					자동사의미 += 부사의미 + " " + 자동사뜻(어절들[2], 형식, ref 현재여부).ㄹ_수_있다();

					return 자동사의미;
				}

				자동사 = 자동사.Replace("ⓧ{", "").Replace("}", "");

				if (자동사.StartsWith("should have "))
				{
					자동사의미 += 자동사뜻(어절들[2], 형식, ref 현재여부).다_빼기() + "었어야 했다";
				}

				return 자동사의미;
			}
			else if (어절들.Count == 2)
			{
				자동사 = 자동사.Replace("ⓧ{", "").Replace("}", "");

				if (자동사.StartsWith("would "))
				{
					자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).다_빼기() + "곤 했다";

					현재여부 = false;
					return 자동사의미;
				}
				else if (자동사.StartsWith("'ll ") || 자동사.StartsWith("will "))
				{
					자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).ㄹ것이다();
					현재여부 = false;
					return 자동사의미;
				}
				else if (자동사.StartsWith("should be"))
				{
					return "되어야 한다";
				}
				else if (자동사.StartsWith("can "))
				{
					자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).ㄹ_수_있다();
					return 자동사의미;
				}
				else if (Be동사현재(어절들[0]) && ING형(어절들[1]))
				{
					자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).다_빼기() + "고 있다";

					return 자동사의미;
				}

				else if (Be동사과거(어절들[0]) && ING형(어절들[1]))
				{
					자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).다_빼기() + "고 있었다";

					현재여부 = false;
					return 자동사의미;
				}
				else if (자동사.StartsWith("didn't "))
				{
					자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).다_빼기() + "지 않았다";

					현재여부 = false;
					return 자동사의미;
				}
				else if (자동사.StartsWith("may "))
				{
					자동사의미 += 자동사뜻(어절들[1], 형식, ref 현재여부).다_빼기().ㄹ_수_있다();
					return 자동사의미;
				}
				else
				{
					자동사의미 += 자동사뜻(어절들[0] + " " + 어절들[1], 형식, ref 현재여부);

					
					return 자동사의미;
				}
			}
			else if (어절들.Count == 1)
			{
				자동사의미 = 자동사뜻(자동사, 형식, ref 현재여부);

				return 자동사의미;
			}
			else
				return 자동사;
		}

		private string 타동사해석(string 타동사, ref string 을를이가, ref bool 현재여부)
		{
            List<string> 어절들 = new List<string>();
            변환.문자열.어절들로(타동사, ref 어절들);

            string 타동사의미 = "";

            if (어절들.Count == 3)
            {
                타동사 = 타동사.Replace("ⓧ{", "").Replace("}", "");


                if (타동사.StartsWith("should have "))
                {
                    타동사의미 += 타동사뜻(어절들[2], ref 현재여부).다_빼기() + "었어야 했다";
                }
				else if(타동사.StartsWith("did not"))
				{
					타동사의미 += 타동사뜻(어절들[2], ref 현재여부).다_빼기() + "지 않았다";
				}
				else if(HAVE형태(어절들[0]) && 부사(어절들[1]) && PP형(어절들[2]))
				{
					타동사의미 += 부사뜻(어절들[1]) + " " + 타동사뜻(어절들[2], ref 현재여부).해왔다();
				}
				else if (HAD형태(어절들[0]) && 부사(어절들[1]) && PP형(어절들[2]))
				{
					타동사의미 += 부사뜻(어절들[1]) + " " + 타동사뜻(어절들[2], ref 현재여부).해왔다();
				}

				return 타동사의미;
            }
            else if (어절들.Count == 2)
            {
				// 이어동사인 경우
				타동사의미 = 타동사뜻(타동사, ref 현재여부); // 타동사뜻 함수에서는 뜻이 없는 경우, 타동사를 그대로 보여준다.
				if (타동사의미 != 타동사) return 타동사의미;
				else 타동사의미 = "";

				// 조동사가 있는 것들
				타동사 = 타동사.Replace("ⓧ{", "").Replace("}", "");

				if (타동사.StartsWith("can "))
				{
					타동사의미 += 타동사뜻(어절들[1], ref 현재여부).ㄹ_수_있다();
				}
				else if (타동사.StartsWith("should "))
				{
					타동사의미 += 타동사뜻(어절들[1], ref 현재여부).다_빼기() + "어야 한다";

					타동사의미 = 타동사의미.Replace("보어야", "봐야"); // 보다 > 봐야 한다.
				}
				else if (타동사.StartsWith("'ll ") || 타동사.StartsWith("will "))
				{
					타동사의미 += 타동사뜻(어절들[1], ref 현재여부).ㄹ것이다();
				}
				else if (타동사.StartsWith("'d "))
				{
					타동사의미 += 타동사뜻(어절들[1], ref 현재여부).ㄹ텐데(); // 했을텐데, 라는 느낌의 가정법 과거 형태이다.
				}
				else if (타동사.StartsWith("didn't have"))
				{
					을를이가 = "이가";
					타동사의미 = "없었다";
				}
				else if (타동사.StartsWith("didn't "))				{					타동사의미 += 타동사뜻(어절들[1], ref 현재여부).지않았다();				}
				else if (Be동사현재(어절들[0]) && ING형(어절들[1]))	{					타동사의미 += 타동사뜻(어절들[1], ref 현재여부).다_빼기() + "고 있다";	}
				else if (Be동사과거(어절들[0]) && ING형(어절들[1])) { 타동사의미 += 타동사뜻(어절들[1], ref 현재여부).다_빼기() + "고 있었다"; }

				return 타동사의미;
            }
            else if (어절들.Count == 1)
            {
                타동사의미 = 타동사뜻(타동사, ref 현재여부);

                if (타동사의미 == "마음에 들었다") 을를이가 = "이가"; // I loved it.
                return 타동사의미;
            }
            else
                return 타동사;

		}

		private bool PP형(string 단어)
		{
			string 사전검색 = Form1._검색.영한사전(단어);
			if (사전검색.Contains("ed형") || 사전검색.Contains("pp형") )
				return true;
			else
				return false;

		}

		private bool 말하기관련(string 단어)
		{
			if (단어 == "say") return true;
			else if (단어 == "saying") return true;
			else if (단어 == "says") return true;
			else if (단어 == "said") return true;
			else if (단어.EndsWith(" say")) return true; // can say, might say, will say
			else if (단어.EndsWith(" saying")) return true;

			return false;
		}

		private bool HAVE형태(string 어절)
		{
			어절 = 어절.ToLower();

			if (어절 == "has") return true;
			if (어절 == "have") return true;

			return false;
		}

		private bool HAD형태(string 어절)
		{
			어절 = 어절.ToLower();

			if (어절 == "had") return true;

			return false;
		}

		private string 접속어해석(string 단어)
		{
			string 접속어의미 = Form1._검색.접속어의미추출(단어);

			if (접속어의미 != "") return 접속어의미;

			return 단어;
		}

		private string 보어해석(string 보어, ref bool 형용사여부)
		{
			if (보어.Contains(" ") == false)
				return 형용사뜻없으면명사(보어, ref 형용사여부);
			else
			{
				string 보어해석처리결과로그 = "";
				return 명사구해석(보어, ref 보어해석처리결과로그);
			}
		}

        private string 형용사구절해석(string 형용사구절)
        {
            string 해석결과 = "";

            if (형용사구절.Contains("ⓢ") || 형용사구절.Contains("ⓒ") || 형용사구절.StartsWith("to ⓥ{"))
            {
                단문복문해석 형용사절 = new 단문복문해석();
                형용사절._문장구조기호 = 형용사절.문자열분석(형용사구절);


                if(형용사절._문장구조기호 == "SVC(C)")
                {
                    if(형용사절._보어 == "called")
                    {
						string 형용사절보어잔재_처리결과로그 = "";
                        해석결과 += 명사구해석(형용사절._보어잔재, ref 형용사절보어잔재_처리결과로그).라고() + " 불리는";
                    }
                }
                else if(형용사구절.StartsWith("to ⓥ{"))
                {
                    형용사절._내부 = true;
                    해석결과 += 형용사절.단문복문해석하기(형용사구절, _전체문맥).형용사화();
                }
				else if (형용사구절.StartsWith("㉨"))
				{ // ⓢ{The Yosemite Grant Bill} ⓥ{was} ⓒ{based} ⓧ{on the idea ⓧ{㉨{that} ⓢ{wilderness} ⓥ{ⓧ{should} be} ⓒ{preserved} ⓧ{for future generations}}}.
					형용사절._내부 = true;
					해석결과 += 형용사절.단문복문해석하기(형용사구절, _전체문맥).형용사화_문장();
				}
				else if(형용사절._문장구조기호 == "OSVX")
				{
					형용사절._내부 = true;
					해석결과 += 형용사절.단문복문해석하기(형용사구절, _전체문맥);
				}
				else // 문장인 경우
				{
					형용사절._내부 = true;
					해석결과 += 형용사절.단문복문해석하기(형용사구절, _전체문맥).형용사화();
				}

			}
			else
            {
				string 남은수식어구 = 앞의전치사빼고남은수식어구(형용사구절);
				string LOG = "";
                string 명사구해석결과 = 명사구해석(앞의전치사빼고남은수식어구(형용사구절), ref LOG);
                해석결과 = 명사구해석결과;

                if (앞의전치사만(형용사구절) == "as")
                {
                    해석결과 += "로서의";
                }
                else if (앞의전치사만(형용사구절) == "by")
                {
                    해석결과 += "에 의한";
                }
                else if ((앞의전치사만(형용사구절) == "in") && !시간표현(남은수식어구, 명사구해석결과))
                {
                    해석결과 += "의";
                }
                else if ((앞의전치사만(형용사구절) == "in") && 시간표현(남은수식어구, 명사구해석결과))
                {
                    해석결과 += "의";
                }
                else if ((앞의전치사만(형용사구절) == "around"))
                {
                    해석결과 += "경의";
                }
                else if ((앞의전치사만(형용사구절) == "to"))
                {
                    해석결과 += "으로의";
                }
            }

            return 해석결과;

        }



        private string 수식어부사구해석(string 서술어, string 보어, string 전치사구, int 형식, ref string 처리결과로그)
		{
			if (전치사구.StartsWith("ⓧ{") && 전치사구.EndsWith("}"))
				전치사구 = 전치사구.Substring(2, 전치사구.Length - 3);

			if (전치사구 == "") return "";

			string LOG = "";

			string 해석결과 = "";

			#region 한방에 끝내기
            if (전치사구 == "Then") return "그러면"; // 대문자는 쓰면 안된다.

            if(전치사구.ToLower() == "there")
            {
                if (형식 == 1) return "그곳에";
                else return "그곳에서";
            }

			string 부사의미 = 부사뜻(전치사구);			if (부사의미 != 전치사구) { return 부사의미; }

            string 감탄사의미 = 감탄사뜻(전치사구);		if (감탄사의미 != 전치사구){ return 감탄사의미;}

            string 형용사의미 = 형용사뜻(전치사구);		if (형용사의미 != 전치사구){ 처리결과로그 = "형용사구"; return 형용사의미; }

			#endregion
			 List<string> 전치사구_안의_어절들 = new List<string>();

            변환.문자열.어절들로(전치사구, ref 전치사구_안의_어절들);

			string 전치사포함숙어뒷부분 = "";

            #region 숙어표현처리용
            if (전치사구_안의_어절들.Count() >= 6)
            {
                string 검색할어절 = String.Format("{0} {1} {2} {3} {4} {5}", 전치사구_안의_어절들[0], 전치사구_안의_어절들[1], 전치사구_안의_어절들[2], 전치사구_안의_어절들[3], 전치사구_안의_어절들[4], 전치사구_안의_어절들[5]);
				전치사포함숙어뒷부분 = "";
				for (int i = 6; i < 전치사구_안의_어절들.Count(); i++){		전치사포함숙어뒷부분 += 전치사구_안의_어절들[i] + " ";	}

	            string 전치사구뜻 = 부사뜻(검색할어절);

		        if (전치사구뜻 != 검색할어절) { return 명사구해석(전치사포함숙어뒷부분, ref LOG) + " " + 전치사구뜻; }
            }

            if (전치사구_안의_어절들.Count() >= 5)
            {
                string 검색할어절 = String.Format("{0} {1} {2} {3} {4}", 전치사구_안의_어절들[0], 전치사구_안의_어절들[1], 전치사구_안의_어절들[2], 전치사구_안의_어절들[3], 전치사구_안의_어절들[4]);
				전치사포함숙어뒷부분 = "";
				for (int i = 5; i < 전치사구_안의_어절들.Count(); i++){		전치사포함숙어뒷부분 += 전치사구_안의_어절들[i] + " ";	}

                string 전치사구뜻 = 부사뜻(검색할어절);

	            if (전치사구뜻 != 검색할어절) { return 명사구해석(전치사포함숙어뒷부분, ref LOG) + " " + 전치사구뜻; }
            }

            if (전치사구_안의_어절들.Count() >= 4)
            {
                string 검색할어절 = String.Format("{0} {1} {2} {3}", 전치사구_안의_어절들[0], 전치사구_안의_어절들[1], 전치사구_안의_어절들[2], 전치사구_안의_어절들[3]);
				전치사포함숙어뒷부분 = "";
				for (int i = 4; i < 전치사구_안의_어절들.Count(); i++){		전치사포함숙어뒷부분 += 전치사구_안의_어절들[i] + " ";	}

                string 전치사구뜻 = 부사뜻(검색할어절);

	            if (전치사구뜻 != 검색할어절) { return 명사구해석(전치사포함숙어뒷부분, ref LOG) + " " + 전치사구뜻; }
            }

            if (전치사구_안의_어절들.Count() >= 3)
            {
                string 검색할어절 = String.Format("{0} {1} {2}", 전치사구_안의_어절들[0], 전치사구_안의_어절들[1], 전치사구_안의_어절들[2]);
				전치사포함숙어뒷부분 = "";
				for (int i = 3; i < 전치사구_안의_어절들.Count(); i++){		전치사포함숙어뒷부분 += 전치사구_안의_어절들[i] + " ";	}

                string 전치사구뜻 = 부사뜻(검색할어절);

	            if (전치사구뜻 != 검색할어절) { return 명사구해석(전치사포함숙어뒷부분, ref LOG) + " " + 전치사구뜻; }
            }

            if (전치사구_안의_어절들.Count() >= 2)
            {
                string 검색할어절 = String.Format("{0} {1}", 전치사구_안의_어절들[0], 전치사구_안의_어절들[1]);
				전치사포함숙어뒷부분 = "";
				for (int i = 2; i < 전치사구_안의_어절들.Count(); i++){		전치사포함숙어뒷부분 += 전치사구_안의_어절들[i] + " ";	}

	            string 전치사구뜻 = 부사뜻(검색할어절);

	            if (전치사구뜻 != 검색할어절) { return 명사구해석(전치사포함숙어뒷부분, ref LOG) + " " + 전치사구뜻; }
            }
            #endregion



			string 남은수식어구 = 앞의전치사빼고남은수식어구(전치사구);
			string 남은수식어구_소문자 = 남은수식어구.ToLower();
            string 남은수식어구해석결과 = 명사구해석(남은수식어구, ref LOG);
			
			해석결과 = 남은수식어구해석결과;

			if (앞의전치사만(전치사구) == "as")
			{
				해석결과 += "로서";
			}
			else if (앞의전치사만(전치사구) == "by")
			{
				해석결과 += "에 의해";
			}
			else if ((앞의전치사만(전치사구) == "in") && 색상표현(남은수식어구_소문자))
			{
				해석결과 += "으로 된";
			}
			else if ((앞의전치사만(전치사구) == "in") && 시간표현(남은수식어구, 남은수식어구해석결과) // !시간표현보다 항상 앞에 와야 합니다.
			|| (앞의전치사만(전치사구) == "in") && 보어.ToLower() == "interested")
			{
				해석결과 += "에";
			}
			else if ((앞의전치사만(전치사구) == "in") && !시간표현(남은수식어구, 남은수식어구해석결과))
			{
				if (형식 == 1)
					해석결과 += " 안에";
				else
					해석결과 += "에서";
			}
			else if (앞의전치사만(전치사구) == "around")
			{
				해석결과 += "경에";
			}
			else if (앞의전치사만(전치사구) == "from")
			{
				if(서술어.ToLower() == "differed" || 서술어.ToLower() == "differs" || 서술어.ToLower() == "differ")
					해석결과 = 해석결과.와과() + "는";
				else
					해석결과 = 해석결과.로() + "부터";
			}
			else if (앞의전치사만(전치사구) == "to")
			{
				해석결과 += "에";
			}
			else if (앞의전치사만(전치사구) == "about")
			{
				해석결과 += "에 대해서";
			}
			else if (앞의전치사만(전치사구) == "towards")
			{
				해석결과 += "쪽으로";
			}

			else if (앞의전치사만(전치사구) == "during") { 해석결과 += " 동안"; }
			else if (앞의전치사만(전치사구) == "after")
			{
				if (시간표현(남은수식어구, 남은수식어구해석결과))
				{
					해석결과 = 해석결과 + " 뒤에";
				}
				else
				{
					해석결과 = 해석결과 + " 후에"; // ⓧ{After the worst economic crisis of our lifetimes},
				}
			}

			else if (앞의전치사만(전치사구) == "on")
			{
				if (시간표현(남은수식어구, 남은수식어구해석결과))
				{
					해석결과 += "에";
				}
				else if (형식 == 1)
				{
					해석결과 = 해석결과.로();
					해석결과 = 해석결과.Replace("여행으로", "여행을");
				}
				else if (형식 == 2)
					해석결과 += "에서";// 그냥 써 봄
				else if (형식 == 3)
					해석결과 += "에서";
				else
					해석결과 += "에서";
			}
			else if (앞의전치사만(전치사구) == "up")
			{
				해석결과 += "에";
			}
			else if (앞의전치사만(전치사구) == "with")
			{
				if (전치사구_안의_어절들.Count > 1)
				{
					if (Form1._검색.목적격대명사인지확인(전치사구_안의_어절들[1]))
					{
						해석결과 = 해석결과.와과();

						return 해석결과;
					}
				}

				해석결과 = 해석결과.로(); // ⓥ{Greet} ⓞ{guests} ⓧ{immediately} ⓧ{with a smile and positive attitude}.
			}
			else if (앞의전치사만(전치사구) == "for")
			{
				해석결과 = 해석결과.을를("단어") + " 위해서";
			}

			return 해석결과;
		}

		private bool 색상표현(string 남은수식어구)
		{
			if(남은수식어구.Contains(" colors ")) 			return true;
			if(남은수식어구.EndsWith(" colors"))			return true;

			return false;
		}

		private bool 시간표현(string 남은수식어구, string 명사구해석) 
		{
			string  남은수식어구소문자 = 남은수식어구.ToLower();

			if(남은수식어구소문자.StartsWith("monday")) return true;
			if(남은수식어구소문자.StartsWith("tuesday")) return true;
			if(남은수식어구소문자.StartsWith("wednesday")) return true;
			if(남은수식어구소문자.StartsWith("thursday")) return true;
			if(남은수식어구소문자.StartsWith("friday")) return true;
			if(남은수식어구소문자.StartsWith("saturday")) return true;
			if(남은수식어구소문자.StartsWith("sunday")) return true;

			if(남은수식어구소문자.StartsWith("mon ")) return true;
			if(남은수식어구소문자.StartsWith("tue ")) return true;
			if(남은수식어구소문자.StartsWith("wed ")) return true;
			if(남은수식어구소문자.StartsWith("thu ")) return true;
			if(남은수식어구소문자.StartsWith("fri ")) return true;
			if(남은수식어구소문자.StartsWith("sat ")) return true;
			if(남은수식어구소문자.StartsWith("sun ")) return true;

			if(남은수식어구소문자.StartsWith("january")) return true;
			if(남은수식어구소문자.StartsWith("february")) return true;
			if(남은수식어구소문자.StartsWith("march")) return true;
			if(남은수식어구소문자.StartsWith("april")) return true;
			if(남은수식어구소문자.StartsWith("may")) return true;
			if(남은수식어구소문자.StartsWith("june")) return true;
			if(남은수식어구소문자.StartsWith("july")) return true;
			if(남은수식어구소문자.StartsWith("august")) return true;
			if(남은수식어구소문자.StartsWith("september")) return true;
			if(남은수식어구소문자.StartsWith("october")) return true;
			if(남은수식어구소문자.StartsWith("november")) return true;
			if(남은수식어구소문자.StartsWith("december")) return true;

			if(명사구해석.Contains("1"))		return true;
			else if (명사구해석.Contains("2")) return true;
			else if (명사구해석.Contains("3")) return true;
			else if (명사구해석.Contains("4")) return true;
			else if (명사구해석.Contains("5")) return true;
			else if (명사구해석.Contains("6")) return true;
			else if (명사구해석.Contains("7")) return true;
			else if (명사구해석.Contains("8")) return true;
			else if (명사구해석.Contains("9")) return true;
			else if (명사구해석.Contains("0")) return true;
			else if (명사구해석.Contains("기원전")) return true;
			else if (명사구해석.Contains("세기")) return true;

			return false;
		}

		private bool 숫자표현(string 숫자문자열)
		{
			if (숫자문자열.Contains("1")) return true;
			if (숫자문자열.Contains("2")) return true;
			if (숫자문자열.Contains("3")) return true;
			if (숫자문자열.Contains("4")) return true;
			if (숫자문자열.Contains("5")) return true;
			if (숫자문자열.Contains("6")) return true;
			if (숫자문자열.Contains("7")) return true;
			if (숫자문자열.Contains("8")) return true;
			if (숫자문자열.Contains("9")) return true;
			if (숫자문자열.Contains("0")) return true;

			if (숫자문자열.Contains("million")) return true;
			if (숫자문자열.Contains("trillion")) return true;
			if (숫자문자열.Contains("hundred")) return true;
			if (숫자문자열.Contains("thousand")) return true;

			return false;
		}

        private string 단어뿐인_명사구해석(string 명사구)
        {
            // Happy Birthday to You!
            if (명사구.ToLower().StartsWith("happy birthday"))
            {
                string 전처리된명사구 = 명사구.ToLower().불필요제거();

                if (전처리된명사구.StartsWith("happy birthday to "))
                {
                    string 축하받을사람 = 전처리된명사구.Substring(18);

                    return 축하받을사람 + "님의 생일을 축하합니다.";
                }
            }

            string 해석결과 = "";

            List<string> 명사구어절들 = new List<string>();

            변환.문자열.어절들로(명사구, ref 명사구어절들);

			if (명사구어절들.Count() == 0)
				return "";

			if(명사구어절들.Count() == 2)
			{
				if(Form1._검색.영한사전(명사구어절들[0]).Contains("이름") && 한국인의_성씨(명사구어절들[1]))
				{
					return 명사뜻(명사구어절들[1]) + 명사뜻(명사구어절들[0]);
				}

			}
			
			if(명사구어절들.Count() > 2)
			{
				if(명사구어절들[0].ToLower() == "more" && 명사구어절들[1].ToLower() == "than")
				{
					string 숫자표현앞부분 = "";
					string 나머지뒷부분 = "";
					bool 숫자표현시작 = true;

					for (int i = 2; i < 명사구어절들.Count; i++)
					{
						if (숫자표현(명사구어절들[i]) && 숫자표현시작)
						{
							숫자표현앞부분 += " " + 명사구어절들[i];

						}
						else
						{
							숫자표현시작 = false;

							나머지뒷부분 += " " + 명사구어절들[i];
						}
					}

					return 단어뿐인_명사구해석(숫자표현앞부분) + " 개 이상의 " + 단어뿐인_명사구해석(나머지뒷부분);
				}

			}

            bool 형용사여부 = false;

                #region 숙어표현처리용 예)All kinds of : 모든 종류의

                if (명사구어절들.Count() >= 6)
                {
                    string 검색할어절 = 명사구어절들[0] + " " + 명사구어절들[1] + " " + 명사구어절들[2] + " " + 명사구어절들[3] + " " + 명사구어절들[4] + " " + 명사구어절들[5];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절) 
					{
						string 검색할어절뒷부분 = "";
						for (int i = 6; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}
						
						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (명사구어절들.Count() >= 5)
                {
                    string 검색할어절 = 명사구어절들[0] + " " + 명사구어절들[1] + " " + 명사구어절들[2] + " " + 명사구어절들[3] + " " + 명사구어절들[4];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{ 
						string 검색할어절뒷부분 = "";
						for (int i = 5; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (명사구어절들.Count() >= 4)
                {
                    string 검색할어절 = 명사구어절들[0] + " "+ 명사구어절들[1] + " " + 명사구어절들[2] + " " + 명사구어절들[3];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{
						string 검색할어절뒷부분 = "";
						for (int i = 4; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (명사구어절들.Count() >= 3)
                {
                    string 검색할어절 = 명사구어절들[0] + " " + 명사구어절들[1] + " " + 명사구어절들[2];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{
						string 검색할어절뒷부분 = "";
						for (int i = 3; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (명사구어절들.Count() >= 2)
                {
                    string 검색할어절 = 명사구어절들[0] + " " + 명사구어절들[1];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{
						string 검색할어절뒷부분 = "";
						for (int i = 2; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }
                #endregion

                if (_구문분석.관사(명사구어절들[0]))
                {
					string 검색할어절뒷부분 = "";

					for (int i = 1; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}
						
					return 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
                }
                else if (명사구어절들[0].ToLower() == "or")
                {
					string 검색할어절뒷부분 = "";

					for (int i = 1; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

                    return  "혹은 " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
                }
                else if (명사구어절들.Count() > 1)
                {
                    if(명사구어절들[1].ToLower() == "and")
					{
						string 검색결과 = 명사뜻(명사구어절들[0]);

						string 검색할어절뒷부분 = "";

						for (int i = 2; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 검색결과 + 와과(검색결과) + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                    else if(_구문분석.접속어(명사구어절들[1]))
					{
						string 검색결과 = 명사뜻(명사구어절들[0]);
						string 검색할어절뒷부분 = "";

						for (int i = 1; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 검색결과 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                    else if(명사구어절들[0].EndsWith(",") && !_구문분석.형용사가능성(명사구어절들[1]))
					{
						string 검색결과 = 명사뜻(명사구어절들[0]);
						string 검색할어절뒷부분 = "";

						for (int i = 1; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 검색결과 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                    else if(명사구어절들.Count >= 3)
					{
						string 검색결과 = 형용사뜻없으면명사_상황봐서부사(명사구어절들[0], ref 형용사여부);
						string 검색할어절뒷부분 = "";

						for (int i = 1; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 검색결과 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                    else
					{
                        string 검색결과 = 형용사뜻없으면명사(명사구어절들[0], ref 형용사여부);

						string 검색할어절뒷부분 = "";

						for (int i = 1; i < 명사구어절들.Count(); i++){		검색할어절뒷부분 += 명사구어절들[i] + " ";	}

						return 검색결과 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}


                }
                else if (명사구어절들.Count() == 1)
                {
                    return 명사뜻(명사구어절들[0]);
                }



            //해석결과 = 명사구후처리(해석결과.Trim());

            return "";
        }
		private bool 한국인의_성씨(string 성씨)
		{
			성씨 = 성씨.ToLower();

			if(성씨 == "kim") return true;
			if(성씨 == "lee") return true;
			if(성씨 == "park") return true;
			if(성씨 == "choi") return true;
			if(성씨 == "chae") return true;
			if(성씨 == "Jung") return true;

			return false;
		}
        private string be주격보어해석(string 주격보어, ref bool 형용사여부)
        {
			string LOG = "";
			// 수동태 해석 추가해야 함


            if (주격보어.Contains("ⓧ{"))
            {
                string 앞부분 = 주격보어.Substring(0, 주격보어.IndexOf("ⓧ{"));
                string 뒷부분 = 주격보어.Substring(주격보어.IndexOf("ⓧ{"));

                뒷부분 = 뒷부분.Substring(2, 뒷부분.Length - 3);

                형용사여부 = false;
                return 형용사구절해석(뒷부분).Trim() + " " + 단어뿐인_명사구해석(앞부분).Trim();
            }

            if (of로나눌수있는지확인(주격보어))
            {
                string 앞부분 = "";
                string 뒷부분 = "";

                of로나누기(주격보어, ref 앞부분, ref 뒷부분);

				return 명사구해석(뒷부분, ref LOG).Trim() + "의 " + 명사구해석(앞부분, ref LOG).Trim();
            }


            return 단어뿐인_보어해석(주격보어, ref 형용사여부).Trim();

        }


        private string 명사구해석(string 명사구, ref string 명사구해석_처리결과로그)
		{
			bool 명사구를느낌표가둘러쌈 = false;

			if (String.IsNullOrEmpty(명사구)) return "";

			string LOG = "";

			if(명사구.Count() > 2 && 명사구.StartsWith("'") && 명사구.EndsWith("'"))
			{
				명사구 = 명사구.Substring(1, 명사구.Length - 2);
				명사구를느낌표가둘러쌈 = true;
			}

			if (명사구.EndsWith(".")) 명사구 = 명사구.Substring(0, 명사구.Length - 1);
			if (명사구.EndsWith("!")) 명사구 = 명사구.Substring(0, 명사구.Length - 1);
			if (명사구.EndsWith("?")) 명사구 = 명사구.Substring(0, 명사구.Length - 1);
			if (명사구.EndsWith(",")) 명사구 = 명사구.Substring(0, 명사구.Length - 1);


			string 통채검색결과 = 명사뜻(명사구.문법표지제거());

			if (통채검색결과 != 명사구.문법표지제거())
			{
				if (명사구를느낌표가둘러쌈) return "'" + 통채검색결과 + "'";
				else return 통채검색결과;
			}


			// here가 맨 뒤에 붙어 있는 경우가 있다. Many restaurants here -> 이곳의 많은 레스토랑들
			if(명사구.EndsWith(" here"))
			{
				string here앞부분 = 명사구.Substring(0, 명사구.Length - 5);

				if (명사구를느낌표가둘러쌈) return "'이곳의 " + 명사구해석(here앞부분, ref LOG).Trim() + "'";
				else return "이곳의 " + 명사구해석(here앞부분, ref LOG).Trim();
			}

            if (명사구.Contains("ⓧ{") && 명사구.EndsWith("}"))
            {
                string 앞부분 = 명사구.Substring(0, 명사구.IndexOf("ⓧ{"));
                string 뒷부분 = 명사구.Substring(명사구.IndexOf("ⓧ{"));

                뒷부분 = 뒷부분.Substring(2, 뒷부분.Length - 3);

				if (명사구를느낌표가둘러쌈) return "'" + 형용사구절해석(뒷부분) + " " + 단어뿐인_명사구해석(앞부분).Trim() + "'";
				else return 형용사구절해석(뒷부분) + " " + 단어뿐인_명사구해석(앞부분).Trim();
            }

            if(and로나눌수있는지확인(명사구))
            {
                string 앞부분 = "";
                string 뒷부분 = "";

                and로나누기(명사구, ref 앞부분, ref 뒷부분);

				if (명사구를느낌표가둘러쌈) return "'" + 명사구해석(앞부분, ref LOG) + " 그리고 " + 명사구해석(뒷부분, ref LOG).Trim() + "'";
				else return 명사구해석(앞부분, ref LOG) + " 그리고 " + 명사구해석(뒷부분, ref LOG).Trim();
            }

            if (of로나눌수있는지확인(명사구))
            {
                string 앞부분 = "";
                string 뒷부분 = "";

                of로나누기(명사구, ref 앞부분, ref 뒷부분);

				if (명사구를느낌표가둘러쌈) return "'" + 명사구해석(뒷부분, ref LOG) + "의 " + 명사구해석(앞부분, ref LOG).Trim() + "'";
				else return 명사구해석(뒷부분, ref LOG) + "의 " + 명사구해석(앞부분, ref LOG).Trim();
            }

			if (명사구를느낌표가둘러쌈) return "'" + 단어뿐인_명사구해석(명사구).Trim() + "'";
			else return 단어뿐인_명사구해석(명사구).Trim();
		}

        private bool and로나눌수있는지확인(string 명사구)
        {
            명사구 = 명사구.ToLower();

			if (명사구.Contains("more and more")) return false; // more and more는 and로 구분하기보다 '더 많은' 이라는 뜻으로 해석하면 좋다.
            if (!명사구.Contains(" and ")) return false;

            string 앞부분 = 명사구.Substring(0, 명사구.IndexOf(" and "));
            string 뒷부분 = 명사구.Substring(명사구.IndexOf(" and "));
            뒷부분 = 뒷부분.Substring(5);

            string 뒷부분_바로다음어절 = "";

            if(뒷부분.Contains(" "))
            {
                뒷부분_바로다음어절 = 뒷부분.Substring(0, 뒷부분.IndexOf(" "));
                if (_구문분석.관사(뒷부분_바로다음어절))
                    return true;
            }
            else // 뒷부분 자체가 바로 다음어절임
            {
                return false; // 이런 경우 나눌 수는 있지만 나눌 필요가 없다.
            }
            return true;
        }

        private void and로나누기(string 명사구, ref string 앞부분, ref string 뒷부분)
        {
            앞부분 = 명사구.Substring(0, 명사구.IndexOf(" and "));
            뒷부분 = 명사구.Substring(명사구.IndexOf(" and "));
            뒷부분 = 뒷부분.Substring(5);
        }


        private void of로나누기(string 명사구, ref string 앞부분, ref string 뒷부분)
        {
            앞부분 = 명사구.Substring(0, 명사구.IndexOf(" of "));
            뒷부분 = 명사구.Substring(명사구.IndexOf(" of "));
            뒷부분 = 뒷부분.Substring(4);
        }

        private bool of로나눌수있는지확인(string 명사구)
        {
            명사구 = 명사구.ToLower();

            if (!명사구.Contains(" of ")) return false;
            if (명사구.StartsWith("lots of ")) 명사구 = 명사구.Substring(8);
            if (명사구.StartsWith("a lot of ")) 명사구 = 명사구.Substring(9);

            명사구 = 명사구.Replace(" a lot of ", "");

            명사구 = 명사구.Replace(" lots of ", "");
            명사구 = 명사구.Replace(" kinds of ", "");
            명사구 = 명사구.Replace(" kind of ", "");


            if (!명사구.Contains(" of ")) return false;

			// 만약 사전에 뜻이 있으면, 예컨데, 'State of the Union' 처럼, ... 이런 경우에는 of로 나눌 수 없다.
			List<string> 명사구_안의_어절들 = new List<string>();
			변환.문자열.어절들로(명사구, ref 명사구_안의_어절들);



            for (int i = 0; i < 명사구_안의_어절들.Count(); i++)
            {
                #region 숙어표현처리용 예)All kinds of : 모든 종류의

				bool 처리됨 = false;

				bool 형용사여부 = false; // 여기서는 안쓰일듯

				if (명사구_안의_어절들.Count() >= 6 + i)
				{
					string 검색할어절 = String.Format("{0} {1} {2} {3} {4} {5}", 명사구_안의_어절들[i], 명사구_안의_어절들[i + 1], 명사구_안의_어절들[i + 2], 명사구_안의_어절들[i + 3], 명사구_안의_어절들[i + 4], 명사구_안의_어절들[i + 5]);

					if(검색할어절.Contains(" of ") || 검색할어절.EndsWith(" of"))
					{
						string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

						if (형용사나명사구의뜻 != 검색할어절) { return false;}
					}
				}

				if (명사구_안의_어절들.Count() >= 5 + i && 처리됨 == false)
				{
					string 검색할어절 = String.Format("{0} {1} {2} {3} {4}", 명사구_안의_어절들[i], 명사구_안의_어절들[i + 1], 명사구_안의_어절들[i + 2], 명사구_안의_어절들[i + 3], 명사구_안의_어절들[i + 4]);

					if(검색할어절.Contains(" of ") || 검색할어절.EndsWith(" of"))
					{
						string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

						if (형용사나명사구의뜻 != 검색할어절) { return false; }
					}
				}

				if (명사구_안의_어절들.Count() >= 4 + i && 처리됨 == false)
				{
					string 검색할어절 = String.Format("{0} {1} {2} {3}", 명사구_안의_어절들[i], 명사구_안의_어절들[i + 1], 명사구_안의_어절들[i + 2], 명사구_안의_어절들[i + 3]);

					if(검색할어절.Contains(" of ") || 검색할어절.EndsWith(" of"))
					{
						string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

						if (형용사나명사구의뜻 != 검색할어절) { return false; }
					}
				}

				if (명사구_안의_어절들.Count() >= 3 + i && 처리됨 == false)
				{
					string 검색할어절 = String.Format("{0} {1} {2}", 명사구_안의_어절들[i], 명사구_안의_어절들[i + 1], 명사구_안의_어절들[i + 2]);

					if(검색할어절.Contains(" of ") || 검색할어절.EndsWith(" of"))
					{
						string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

						if (형용사나명사구의뜻 != 검색할어절) { return false; }
					}
				}

				if (명사구_안의_어절들.Count() >= 2 + i && 처리됨 == false)
				{
					string 검색할어절 = String.Format("{0} {1}", 명사구_안의_어절들[i], 명사구_안의_어절들[i + 1]);

					if(검색할어절.Contains(" of ") || 검색할어절.EndsWith(" of"))
					{
						string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

						if (형용사나명사구의뜻 != 검색할어절) { return false; }
					}
				}
				#endregion
				}

            return true;
        }


        private string 명사구후처리(string 명사구)
		{
			명사구 = 명사구.Replace("종이로 된 돈", "지폐");

			if(변환.문자열.Right(명사구, 3) ==  "기원전" && !명사구.Contains("세기"))
				명사구 = "기원전 " + 명사구.Substring(0, 명사구.Count() - 3).Trim() + "년";
			else if (변환.문자열.Right(명사구, 3) == "기원전")
				명사구 = "기원전 " + 명사구.Substring(0, 명사구.Count() - 3).Trim();
			else if (변환.문자열.Right(명사구, 3) == "기원후")
				명사구 = 명사구.Substring(0, 명사구.Count() - 3).Trim() + "년";
			else if (변환.문자열.Right(명사구, 3) == "기원후")
				명사구 = 명사구.Substring(0, 명사구.Count() - 3).Trim();

			명사구 = 명사구.Replace("1 ", "1");
			명사구 = 명사구.Replace("2 ", "2");
			명사구 = 명사구.Replace("3 ", "3");
			명사구 = 명사구.Replace("4 ", "4");
			명사구 = 명사구.Replace("5 ", "5");
			명사구 = 명사구.Replace("6 ", "6");
			명사구 = 명사구.Replace("7 ", "7");
			명사구 = 명사구.Replace("8 ", "8");
			명사구 = 명사구.Replace("9 ", "9");
			명사구 = 명사구.Replace("0 ", "0");

			return 명사구;
		}


        private bool 글자종성있는지여부(string 글자)
		{
			int x = (int)글자[0];

			if (x >= 0xAC00 && x <= 0xD7A3)
            {
				//int a,b,c;
				int c;

                c = x - 0xAC00;
                //a = c / (21 * 28);
                c = c % (21 * 28);
                //b = c / 28;
                c = c % 28;

				if( c == 0)
					return false;
				else
					return true;
			}

			return false;
		}
        private string 최적의미추출(string 모든의미)
        {
			List<string> 우리말뜻후보들 = new List<string>();
			List<int> 우리말뜻점수들 = new List<int>();

			if (모든의미.Contains(","))
            {
                string[] 모든의미들 = 모든의미.Split(',');

                if (!모든의미.Contains("<"))
                    return 모든의미들[0];

                for (int i = 0; i < 모든의미들.Count(); i++)
                {
                    string 현재모든의미 = 모든의미들[i];

                    if (현재모든의미.Contains("<"))
                    {
                        string 현재우리말뜻 = 현재모든의미.Substring(0, 현재모든의미.IndexOf("<")).Trim();
                        string 문맥단어모음 = 현재모든의미.Substring(현재모든의미.IndexOf("<") + 1);

						if(문맥단어모음.Contains(">"))
							문맥단어모음 = 문맥단어모음.Substring(0, 문맥단어모음.IndexOf(">"));

                        string[] 문맥단어들 = 문맥단어모음.Split(';');

                        for (int j = 0; j < 문맥단어들.Count(); j++)
                        {
                            if (_전체문장.StartsWith(문맥단어들[j] + " "))		{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 4);}
                            if (_전체문장.EndsWith(" " + 문맥단어들[j]))		{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 4);}
							if (_전체문장.Contains(" " + 문맥단어들[j] + " "))	{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 4);}
                        }

						for (int j = 0; j < 문맥단어들.Count(); j++)
						{
							if (_전체문장_소문자.StartsWith(문맥단어들[j] + " "))		{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 3);}
							if (_전체문장_소문자.EndsWith(" " + 문맥단어들[j]))			{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 3);}
							if (_전체문장_소문자.Contains(" " + 문맥단어들[j] + " "))	{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 3);}
						}

						for (int j = 0; j < 문맥단어들.Count(); j++)
                        {
                            if (_전체문맥.StartsWith(문맥단어들[j] + " "))		{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 2);}
                            if (_전체문맥.EndsWith(" " + 문맥단어들[j]))		{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 2);}
                            if (_전체문맥.Contains(" " + 문맥단어들[j] + " "))	{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length * 2);}
                        }

						for (int j = 0; j < 문맥단어들.Count(); j++)
						{
							if (_전체문맥_소문자.StartsWith(문맥단어들[j] + " "))		{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length);}
							if (_전체문맥_소문자.EndsWith(" " + 문맥단어들[j]))			{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length);}
							if (_전체문맥_소문자.Contains(" " + 문맥단어들[j] + " "))	{ 우리말뜻후보들.Add(현재우리말뜻); 우리말뜻점수들.Add(문맥단어들[j].Length);}
						}
					}
				}

				int 최대점수 = 0;
				int 최대점수인덱스 = -1;

				for (int i = 0; i < 우리말뜻점수들.Count; i++)
				{
					if(최대점수 < 우리말뜻점수들[i])
					{
						최대점수인덱스 = i;
					}
				}
				if (최대점수인덱스 != -1) return 우리말뜻후보들[최대점수인덱스];

                // 이도 저도 아니면
                if(모든의미들[0].Contains("<")) // 그냥 <기호 앞에 있는 것들을 모두 뜻으로 뽑아낸다는 의미입니다.
                    return 모든의미들[0].Substring(0, 모든의미들[0].IndexOf("<")).Trim();
                else
                    return 모든의미들[0].Trim();
            }
            else if(모든의미.Contains("<"))
            {
                return 모든의미.Substring(0, 모든의미.IndexOf("<"));
            }

            return 모든의미.Trim();
        }
        private string 명사뜻(string 단어)
		{
			if (단어.Length == 0) return "";

            bool 쉼표로끝남 = false;

            if (단어.EndsWith(",")) { 쉼표로끝남 = true; 단어 = 단어.Substring(0, 단어.Length - 1); }

            string 명사의미 = Form1._검색.명사의미추출(단어);

			if(단어.맨앞글자만대문자인지())
			{
				if(명사의미.Contains("{사람이름}") && !명사의미.Contains("|{사람이름}"))
				{
					List<string> 뜻어절들 = new List<string>();

					문자열.어절들로(명사의미, ref 뜻어절들);

					for(int i = 0; i < 뜻어절들.Count; i++)
					{
						if(뜻어절들[i].Contains("{사람이름}"))
						{
							string 현재뜻어절 = 뜻어절들[i];

							현재뜻어절 = 현재뜻어절.Replace("{사람이름}", "").Trim();

							if (현재뜻어절 != "")
								return 현재뜻어절;
						}
					}
				}
			}

            string 최적의미 = 최적의미추출(명사의미);

            if (최적의미 != "")
                if (쉼표로끝남) return 최적의미 + ",";
                else return 최적의미;
            if (쉼표로끝남) return 단어 + ",";
            else return 단어;
        }

		private bool 부사(string 단어)
		{
			string 부사의미 = Form1._검색.부사의미추출(단어);

			if (부사의미 != "")
				return true;
			else
				return false;
		}



		private string 부사뜻(string 단어)
		{
			string 원래단어 = 단어;
			단어 = 단어.불필요제거_사전용();

			string 부사의미 = Form1._검색.부사의미추출(단어);

            string 최적의미 = 최적의미추출(부사의미);
            if (최적의미 != "") return 최적의미;

            if (부사의미 != "") return 부사의미;

			return 원래단어;
		}

        private string 감탄사뜻(string 단어)
        {
            string 감탄사의미 = Form1._검색.감탄사의미추출(단어);

            string 최적의미 = 최적의미추출(감탄사의미);
            if (최적의미 != "") return 최적의미;

            if (감탄사의미 != "") return 감탄사의미;

            return 단어;
        }
        private string 자동사뜻(string 단어, int 형식, ref bool 현재여부)
        {
			현재여부 = true;

            string 자동사의미 = Form1._검색.자동사의미추출(단어, 형식, ref 현재여부);
            string 최적의미 = 최적의미추출(자동사의미);

            if (최적의미 != "")
			{
				return 최적의미;
			}

			return 단어;
        }

		private string 서술적용법뜻(string 단어)
        {
            string 서술적용법형용사의미 = Form1._검색.서술적용법형용사의미(단어);
            if (서술적용법형용사의미 != "") return 서술적용법형용사의미;

            서술적용법형용사의미 = 형용사뜻(단어);
            if (서술적용법형용사의미 != "") return 서술적용법형용사의미;

            return 단어;
        }
        private string 타동사뜻(string 단어, ref bool 현재여부)
		{
			string 타동사의미 = Form1._검색.타동사의미추출(단어, ref 현재여부);

            string 최적의미 = 최적의미추출(타동사의미);

			if (최적의미 != "")
			{
				return 최적의미;
			}

			return 단어;
		}

        private string 보어형용사뜻없으면명사(string 단어, ref bool 형용사여부)
        {
			if(단어.맨앞글자만대문자인지()) // 백퍼센트 이름이지 싶다.
			{
				string 명사의미 = Form1._검색.명사의미추출(단어);

				string 최적의미 = 최적의미추출(명사의미);
				if (최적의미 != "")
				{
					형용사여부 = false;
					return 최적의미;
				}

				return 단어;
			}
			else
			{
				string 서술적용법형용사의미 = Form1._검색.서술적용법형용사의미(단어);
				string 형용사의미 = Form1._검색.형용사의미추출(단어);
				string 명사의미 = Form1._검색.명사의미추출(단어);
			

				string 최적의미 = 최적의미추출(서술적용법형용사의미);
				if (최적의미 != "")
				{
					형용사여부 = true;
					return 최적의미;
				}

				최적의미 = 최적의미추출(형용사의미);
				if (최적의미 != "")
				{
					형용사여부 = true;
					return 최적의미;
				}

				최적의미 = 최적의미추출(명사의미);
				if (최적의미 != "")
				{
					형용사여부 = false;
					return 최적의미;
				}

				return 단어;
			}
        }

		private string 형용사뜻없으면명사_상황봐서부사(string 단어, ref bool 형용사여부)
		{
			string 결과 =  형용사뜻없으면명사(단어, ref 형용사여부);

			if(단어 == 결과) 결과 = 부사뜻(단어); // 없으면 부사뜻이라도 내보낸다.

			return 결과;
		}

        private string 형용사뜻없으면명사(string 단어, ref bool 형용사여부)
		{

            bool 쉼표로끝남 = false;

            if (단어.EndsWith(",")) { 쉼표로끝남 = true; 단어 = 단어.Substring(0, 단어.Length - 1); }

			if(단어.EndsWith("'s"))
			{
				string 단어s앞부분 = 단어.Substring(0, 단어.Length - 2);
				string 단어s앞부분의미 = 단어뿐인_명사구해석(단어s앞부분);

				if(!String.IsNullOrEmpty(단어s앞부분의미))
				{
					return 단어s앞부분의미 + "의";
				}
				else if(!단어s앞부분.Contains(" "))
					return 단어s앞부분 + "의";
				else
					return "단어";
			}
			if (단어.EndsWith("s'"))
			{
				string 단어s앞부분 = 단어.Substring(0, 단어.Length - 1);
				string 단어s앞부분의미 = 단어뿐인_명사구해석(단어s앞부분);

				if (!String.IsNullOrEmpty(단어s앞부분의미))
				{
					return 단어s앞부분의미 + "의";
				}
				else if (!단어s앞부분.Contains(" "))
					return 단어s앞부분 + "의";
				else
					return "단어";

			}

			string 형용사의미 = Form1._검색.형용사의미추출(단어);
			string 명사의미 = Form1._검색.명사의미추출(단어);


            string 최적의미 = 최적의미추출(형용사의미);
            if (최적의미 != "")
            {
                형용사여부 = true;
                if(쉼표로끝남) return 최적의미 + ",";
                else return 최적의미;
            }

            최적의미 = 최적의미추출(명사의미);
            if (최적의미 != "")
            {
                형용사여부 = false;
                if (쉼표로끝남) return 최적의미 + ",";
                else return 최적의미;
            }

            if (쉼표로끝남) return 단어 + ",";
            else return 단어;
		}
        private string 형용사뜻(string 단어)
        {
            string 형용사의미 = Form1._검색.형용사의미추출(단어);

            string 최적의미 = 최적의미추출(형용사의미);
            if (최적의미 != "") return 최적의미;

            return 단어;
        }


        public string 문자열분석(string s)
		{
			string 결과 = "", 표지 = ""; /* 현재문법표지 */	int 단계 = 0 /*문법표지 안에 문법 표지가 들어갈 수 있기 때문에 만들어 놓은 설정입니다.*/, 위치 = 0;

			for(int i = 0; i < s.Count(); i++){
				if(s.Substring(i).Left(2)=="ⓢ{")		{	if(단계==0){	표지="주어";		위치=i+2;}단계++;i++;}			else if(s.Substring(i).Left(2)=="ⓒ{")	{if(단계==0){표지="보어";		위치=i+2;}단계++;i++;}
                else if(s.Substring(i).Left(2)=="ⓘ{")	{	if(단계==0){	표지="간접목적어";	위치=i+2;}단계++;i++;}			else if(s.Substring(i).Left(2)=="ⓓ{")	{if(단계==0){표지="직접목적어";	위치=i+2;}단계++;i++;}
                else if(s.Substring(i).Left(4)=="(ⓒ){"){	if(단계==0){	표지="보어잔재";	위치=i+4;}단계++;i+=3;}			else if(s.Substring(i).Left(2)=="ⓞ{")	{if(단계==0){표지="목적어";		위치=i+2;}단계++;i++;}
				else if(s.Substring(i).Left(2)=="㉨{")	{	if(단계==0){	표지="접속어";		위치=i+2;}단계++;i++;}
				else if(s.Substring(i).Left(2)=="ⓥ{")	{	if(단계==0){ if(표지=="미분석"){if(		_미분석0=="")	_미분석0	=s.Substring(위치,i-위치);	else if(_미분석1=="") _미분석1=s.Substring(위치,i-위치); else if(_미분석2=="") _미분석2=s.Substring(위치,i-위치);	결과+="_";}	표지="서술어";		위치=i+2;}단계++;i++;}
				else if(s.Substring(i).Left(2)=="ⓧ{")	{	if(단계==0){ if(표지=="미분석"){if(		_미분석0=="")	_미분석0	=s.Substring(위치,i-위치);	else if(_미분석1=="") _미분석1=s.Substring(위치,i-위치); else if(_미분석2=="") _미분석2=s.Substring(위치,i-위치);	결과+="_";}	표지="수식어";		위치=i;}단계++;i++;}
				else if(s.Substring(i).Left(1)=="}"){단계--;if(단계==0){ if(표지=="주어"){							_주어		=s.Substring(위치,i-위치);	결과+="S";}
																	else if(표지=="서술어"){						_서술어		=s.Substring(위치,i-위치);	결과+="V";}
																	else if(표지=="수식어")
						{		 if (결과.EndsWith("S"))	_수식어S뒤	+= " " + s.Substring(위치, i - 위치 + 1);
							else if (결과.EndsWith("V"))	_수식어V뒤	+= " " + s.Substring(위치, i - 위치 + 1);
							else if (결과.EndsWith("O"))	_수식어O뒤	+= " " + s.Substring(위치, i - 위치 + 1); 
							else if (결과.EndsWith("C"))	_수식어C뒤	+= " " + s.Substring(위치, i - 위치 + 1);
							else							_수식어앞			+= " " + s.Substring(위치, i - 위치 + 1);

							/*결과 += "X";*/ }

						else if (표지 == "목적어") { _목적어 = s.Substring(위치, i - 위치); 결과 += "O"; }
						else if (표지=="간접목적어"){					_간접목적어	=s.Substring(위치,i-위치);	결과+="I";}
																	else if(표지=="직접목적어"){					_직접목적어 =s.Substring(위치,i-위치);	결과+="D";}
																	else if(표지=="보어"){							_보어		=s.Substring(위치,i-위치);	결과+="C";}
																	else if(표지=="보어잔재"){						_보어잔재	=s.Substring(위치,i-위치);  결과+="(C)";}
																	else if(표지=="접속어"){						_접속어		=s.Substring(위치,i-위치);	결과 += "+";}
																	표지 = ""; /*초기화*/}}
				else if(단계 == 0 && 표지 == "" && 변환.문자열.Left(s.Substring(i), 1) != " " && 변환.문자열.Left(s.Substring(i), 1) != "," && 변환.문자열.Left(s.Substring(i), 1) != "." && 변환.문자열.Left(s.Substring(i), 1) != "!" && 변환.문자열.Left(s.Substring(i), 1) != ":" && 변환.문자열.Left(s.Substring(i), 1) != "\""){
																	표지 = "미분석";			위치 = i;}
				else if(i == s.Count() - 1 && 표지 == "미분석"){	if (_미분석0 == "")	_미분석0=s.Substring(위치,i-위치+1);	else if (_미분석1=="") _미분석1=s.Substring(위치,i-위치+1); else if (_미분석2=="") _미분석2 = s.Substring(위치, i - 위치 + 1);
																																							결과 += "_";}}
			_수식어앞 = _수식어앞.Trim();
			_수식어S뒤 = _수식어S뒤.Trim();
			_수식어V뒤 = _수식어V뒤.Trim();
			_수식어O뒤 = _수식어O뒤.Trim();
			_수식어C뒤 = _수식어C뒤.Trim();

			return 결과;
		}
		
		
		// 단어뿐인_명사구해석과 기본적으로는 비슷하다.
        private string 단어뿐인_보어해석(string 보어, ref bool 형용사여부)
        {

            string 해석결과 = "";

            List<string> 보어_안의_어절들 = new List<string>();

            변환.문자열.어절들로(보어, ref 보어_안의_어절들);

			if (보어_안의_어절들.Count() == 0)
				return "";

			if(보어_안의_어절들.Count() == 2)
			{
				if(Form1._검색.영한사전(보어_안의_어절들[0]).Contains("이름") && 한국인의_성씨(보어_안의_어절들[1]))
				{
					return 명사뜻(보어_안의_어절들[1]) + 명사뜻(보어_안의_어절들[0]);
				}

			}


                bool 처리됨 = false;

                if (보어_안의_어절들.Count() == 1)
                {
                    return 보어형용사뜻없으면명사(보어_안의_어절들[0], ref 형용사여부);
                }

                if (보어_안의_어절들.Count() >= 6)
                {
                    string 검색할어절 = 보어_안의_어절들[0] + " " + 보어_안의_어절들[1] + " " + 보어_안의_어절들[2] + " " + 보어_안의_어절들[3] + " " +  보어_안의_어절들[4] + " " + 보어_안의_어절들[5];
					 
                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절) 
					{
						string 검색할어절뒷부분 = "";
						for (int i = 6; i < 보어_안의_어절들.Count(); i++){		검색할어절뒷부분 += 보어_안의_어절들[i] + " ";	}
						
						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (보어_안의_어절들.Count() >= 5)
                {
                    string 검색할어절 = 보어_안의_어절들[0] + " " + 보어_안의_어절들[1] + " " + 보어_안의_어절들[2] + " " + 보어_안의_어절들[3] + " " + 보어_안의_어절들[4];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{ 
						string 검색할어절뒷부분 = "";
						for (int i = 5; i < 보어_안의_어절들.Count(); i++){		검색할어절뒷부분 += 보어_안의_어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (보어_안의_어절들.Count() >= 4)
                {
                    string 검색할어절 = 보어_안의_어절들[0] + " " + 보어_안의_어절들[1] + " " + 보어_안의_어절들[2] + " " + 보어_안의_어절들[3];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{
						string 검색할어절뒷부분 = "";
						for (int i = 4; i < 보어_안의_어절들.Count(); i++){		검색할어절뒷부분 += 보어_안의_어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (보어_안의_어절들.Count() >= 3)
                {
                    string 검색할어절 = 보어_안의_어절들[0] + " " + 보어_안의_어절들[1] + " " + 보어_안의_어절들[2];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{
						string 검색할어절뒷부분 = "";
						for (int i = 3; i < 보어_안의_어절들.Count(); i++){		검색할어절뒷부분 += 보어_안의_어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

                if (보어_안의_어절들.Count() >= 2)
                {
                    string 검색할어절 = 보어_안의_어절들[0] + " " + 보어_안의_어절들[1];

                    string 형용사나명사구의뜻 = 형용사뜻없으면명사(검색할어절, ref 형용사여부);

                    if (형용사나명사구의뜻 != 검색할어절)
					{
						string 검색할어절뒷부분 = "";
						for (int i = 2; i < 보어_안의_어절들.Count(); i++){		검색할어절뒷부분 += 보어_안의_어절들[i] + " ";	}

						return 형용사나명사구의뜻 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
					}
                }

			if (_구문분석.관사(보어_안의_어절들[0]))
			{
				string 검색할어절뒷부분 = "";

				for (int i = 1; i < 보어_안의_어절들.Count(); i++) { 검색할어절뒷부분 += 보어_안의_어절들[i] + " "; }

				return 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
			}
			else if (보어_안의_어절들.Count() > 1)
			{
                string 검색결과 = 형용사뜻없으면명사(보어_안의_어절들[0], ref 형용사여부);

				string 검색할어절뒷부분 = "";

				for (int i = 1; i < 보어_안의_어절들.Count(); i++){		검색할어절뒷부분 += 보어_안의_어절들[i] + " ";	}

				return 검색결과 + " " + 단어뿐인_명사구해석(검색할어절뒷부분.Trim());
			}
			else if (보어_안의_어절들.Count() == 1)
			{
				return 형용사뜻없으면명사(보어_안의_어절들[0], ref 형용사여부);
			}


			return "";         

        }
	}
}



