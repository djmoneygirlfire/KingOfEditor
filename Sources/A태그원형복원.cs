using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 편집기의_제왕
{
    class A태그원형복원
    {
        public static int _ABC형보기인지;
        public static string _A의앞부분;
        public static string _A의뒷부분;
        public static string _B의앞부분;
        public static string _B의뒷부분;
        public static string _C의앞부분;
        public static string _C의뒷부분;

        static A태그원형복원()
        {
            _ABC형보기인지 = 0;
        }

        public static void T태그내용에서ABC각부분추출(string T태그내용)
        {
            if (!T태그내용.Contains("(A)")) return;
            if (!T태그내용.Contains("(B)")) return;
            if (!T태그내용.Contains("(C)")) return;

            T태그내용 = T태그내용.Replace("<TBAR></TBAR>", ""); // "/"가 들어있어서 혼동될 수 있습니다.

            while (T태그내용.Contains("  "))
            {
                T태그내용 = T태그내용.Replace("  ", " ");

            }

            _A의앞부분 = ""; _A의뒷부분 = ""; _B의앞부분 = ""; _B의뒷부분 = ""; _C의앞부분 = ""; _C의뒷부분 = "";

            if (T태그내용.Contains("[") && T태그내용.Contains("/") && T태그내용.Contains("]"))
            {

                int A위치 = T태그내용.IndexOf("(A)");
                int A격자시작위치 = T태그내용.IndexOf("[", A위치) + 1;
                int A슬래쉬위치 = T태그내용.IndexOf("/", A위치);
                int A격자종료위치 = T태그내용.IndexOf("]", A위치);

                _A의앞부분 = T태그내용.Substring(A격자시작위치, A슬래쉬위치 - A격자시작위치).Trim();
                _A의뒷부분 = T태그내용.Substring(A슬래쉬위치 + 1, A격자종료위치 - A슬래쉬위치 - 1).Trim();


                int B위치 = T태그내용.IndexOf("(B)");
                int B격자시작위치 = T태그내용.IndexOf("[", B위치) + 1;
                int B슬래쉬위치 = T태그내용.IndexOf("/", B위치);
                int B격자종료위치 = T태그내용.IndexOf("]", B위치);

                _B의앞부분 = T태그내용.Substring(B격자시작위치, B슬래쉬위치 - B격자시작위치).Trim();
                _B의뒷부분 = T태그내용.Substring(B슬래쉬위치 + 1, B격자종료위치 - B슬래쉬위치 - 1).Trim();

                int C위치 = T태그내용.IndexOf("(C)");

                int C격자시작위치 = T태그내용.IndexOf("[", C위치) + 1;
                int C슬래쉬위치 = T태그내용.IndexOf("/", C위치);
                int C격자종료위치 = T태그내용.IndexOf("]", C위치);

                _C의앞부분 = T태그내용.Substring(C격자시작위치, C슬래쉬위치 - C격자시작위치).Trim();
                _C의뒷부분 = T태그내용.Substring(C슬래쉬위치 + 1, C격자종료위치 - C슬래쉬위치 - 1).Trim();
            }
            else if(T태그내용.Contains("/"))
            {
                bool 격자시작 = false;

                string 새로운T태그내용 = "";

                List<string> 어절들 = new List<string>();

                변환.문자열.어절들로(T태그내용, ref 어절들);

                for(int i = 0; i< 어절들.Count; i++)
                {
                    if (어절들[i] == "(A)") 새로운T태그내용 += "(A) [";
                    else if (어절들[i] == "(B)") 새로운T태그내용 += "(B) [";
                    else if (어절들[i] == "(C)") 새로운T태그내용 += "(C) [";
                    else if (어절들[i] == "/")
                    {
                        새로운T태그내용 += " / ";
                        격자시작 = true;
                    }
                    else if (격자시작)
                    {
                        격자시작 = false;

                        새로운T태그내용 += 어절들[i] + "] ";
                    }
                    else
                        새로운T태그내용 += 어절들[i] + " ";
                }
                새로운T태그내용 = 새로운T태그내용.Replace("(A) [“", "(A) “[");
                새로운T태그내용 = 새로운T태그내용.Replace("(B) [“", "(B) “[");
                새로운T태그내용 = 새로운T태그내용.Replace("(C) [“", "(C) “[");

                새로운T태그내용 = 새로운T태그내용.Replace("(A) [\"", "(A) \"[");
                새로운T태그내용 = 새로운T태그내용.Replace("(B) [\"", "(B) \"[");
                새로운T태그내용 = 새로운T태그내용.Replace("(C) [\"", "(C) \"[");

                새로운T태그내용 = 새로운T태그내용.Replace("[ ", "[");

                T태그내용 = 새로운T태그내용.Trim();

                int A위치 = T태그내용.IndexOf("(A)");
                int A격자시작위치 = T태그내용.IndexOf("[", A위치) + 1;
                int A슬래쉬위치 = T태그내용.IndexOf("/", A위치);
                int A격자종료위치 = T태그내용.IndexOf("]", A위치);

                if (A격자시작위치 > A슬래쉬위치) return;
                if (A슬래쉬위치 > A격자종료위치) return;

                _A의앞부분 = T태그내용.Substring(A격자시작위치, A슬래쉬위치 - A격자시작위치).Trim();
                _A의뒷부분 = T태그내용.Substring(A슬래쉬위치 + 1, A격자종료위치 - A슬래쉬위치 - 1).Trim();

                int B위치 = T태그내용.IndexOf("(B)");
                int B격자시작위치 = T태그내용.IndexOf("[", B위치) + 1;
                int B슬래쉬위치 = T태그내용.IndexOf("/", B위치);
                int B격자종료위치 = T태그내용.IndexOf("]", B위치);

                if (B격자시작위치 > B슬래쉬위치) return;
                if (B슬래쉬위치 > B격자종료위치) return;

                _B의앞부분 = T태그내용.Substring(B격자시작위치, B슬래쉬위치 - B격자시작위치).Trim();
                _B의뒷부분 = T태그내용.Substring(B슬래쉬위치 + 1, B격자종료위치 - B슬래쉬위치 - 1).Trim();



                int C위치 = T태그내용.IndexOf("(C)");

                int C격자시작위치 = T태그내용.IndexOf("[", C위치) + 1;
                int C슬래쉬위치 = T태그내용.IndexOf("/", C위치);
                int C격자종료위치 = T태그내용.IndexOf("]", C위치);

                if (C격자시작위치 > C슬래쉬위치) return;
                if (C슬래쉬위치 > C격자종료위치) return;

                _C의앞부분 = T태그내용.Substring(C격자시작위치, C슬래쉬위치 - C격자시작위치).Trim();
                _C의뒷부분 = T태그내용.Substring(C슬래쉬위치 + 1, C격자종료위치 - C슬래쉬위치 - 1).Trim();
            }
        }

        public static bool 복원결과(string 원문, string T태그내용, ref string A0, ref string A1, ref string A2, ref string A3, ref string A4, ref string A5)
        {
            _A의앞부분 = ""; _A의뒷부분 = ""; _B의앞부분 = ""; _B의뒷부분 = ""; _C의앞부분 = ""; _C의뒷부분 = ""; 
            
            T태그내용에서ABC각부분추출(T태그내용);

            List <string> 문자열들 = new List<string>();
			원문 = 원문.Replace(" \u2013 ", " …… ");
			원문 = 원문.Replace(" \u2014 ", " …… ");

			원문 = 원문.Replace("(B) – (D) – (C)", "(B) …… (D) …… (C)");
			원문 = 원문.Replace("(B) – (C) – (D)", "(B) …… (C) …… (D)");
			원문 = 원문.Replace("(C) – (B) – (D)", "(C) …… (B) …… (D)");
			원문 = 원문.Replace("(C) – (D) – (B)", "(C) …… (D) …… (B)");
			원문 = 원문.Replace("(D) – (B) – (C)", "(D) …… (B) …… (C)");
			원문 = 원문.Replace("(D) – (C) – (B)", "(D) …… (C) …… (B)");

            string 빈칸없는원문 = 원문.Trim();


            if(빈칸없는원문.Contains("①") && 빈칸없는원문.Contains("②") && 빈칸없는원문.Contains("③") && 빈칸없는원문.Contains("④") && 빈칸없는원문.Contains("⑤"))
            {
                char [] 원형문자들 =  {'①', '②', '③', '④', '⑤'};
                string 현재라인 = "";

                int 맨처음나오는_원형문자위치 = 0;
                int 바로전_원형문자위치 = 0;
                맨처음나오는_원형문자위치 = 빈칸없는원문.IndexOfAny(원형문자들, 맨처음나오는_원형문자위치);

                if(맨처음나오는_원형문자위치 == 0)
                {
                    _ABC형보기인지 = 1;

                    // ① 보기1
                    // ② 보기2
                    // ...
                    맨처음나오는_원형문자위치 = 1;
                    맨처음나오는_원형문자위치 = 빈칸없는원문.IndexOfAny(원형문자들, 맨처음나오는_원형문자위치);

                    현재라인 = 빈칸없는원문.Substring(0, 맨처음나오는_원형문자위치); // 예를 들면 ① 보기1 일 수 있음
                    현재라인 = 현재라인.Replace("\n", " ");

                    문자열들.Add(현재라인.Trim());
                }
                else
                {
                    // (A) (B)형인지, (A) (B) (C)형인지 확인해야 함

                    // (A) 
                    // ① 보기1
                    // ② 보기2

                    현재라인 = 빈칸없는원문.Substring(0, 맨처음나오는_원형문자위치); // 예를 들면 (A)
                    현재라인 = 현재라인.Trim();

                    if(!현재라인.Contains("(C)"))
                    {
                        _ABC형보기인지 = 2;

                        문자열들.Add(AB형하이픈넣기(현재라인));


                    }
                    else
                    {
                        _ABC형보기인지 = 3;

                        문자열들.Add(ABC형하이픈넣기(현재라인));

                    }







                    바로전_원형문자위치 = 맨처음나오는_원형문자위치;

                    맨처음나오는_원형문자위치 = 빈칸없는원문.IndexOfAny(원형문자들, 맨처음나오는_원형문자위치 + 1);
                    현재라인 = 빈칸없는원문.Substring(바로전_원형문자위치, 맨처음나오는_원형문자위치 - 바로전_원형문자위치); // 예를 들면 ① 보기1
                    현재라인 = 현재라인.Replace("\n", " ");

                    if( _ABC형보기인지 == 2)  문자열들.Add(AB형하이픈넣기(현재라인.Trim())); // ①임
                    if( _ABC형보기인지 == 3)  문자열들.Add(ABC형하이픈넣기(현재라인.Trim())); // ①임
                }

                바로전_원형문자위치 = 맨처음나오는_원형문자위치;
                맨처음나오는_원형문자위치 = 빈칸없는원문.IndexOfAny(원형문자들, 맨처음나오는_원형문자위치 + 1);
                현재라인 = 빈칸없는원문.Substring(바로전_원형문자위치, 맨처음나오는_원형문자위치 - 바로전_원형문자위치); // 예를 들면 ② 보기2
                현재라인 = 현재라인.Replace("\n", " ");
                
                if( _ABC형보기인지 == 1)  문자열들.Add(현재라인.Trim());                     // ②임
                if( _ABC형보기인지 == 2)  문자열들.Add(AB형하이픈넣기(현재라인.Trim()));     // ②임
                if( _ABC형보기인지 == 3)  문자열들.Add(ABC형하이픈넣기(현재라인.Trim()));    // ②임

                바로전_원형문자위치 = 맨처음나오는_원형문자위치;
                맨처음나오는_원형문자위치 = 빈칸없는원문.IndexOfAny(원형문자들, 맨처음나오는_원형문자위치 + 1);
                현재라인 = 빈칸없는원문.Substring(바로전_원형문자위치, 맨처음나오는_원형문자위치 - 바로전_원형문자위치); // 예를 들면 ③ 보기3
                현재라인 = 현재라인.Replace("\n", " ");

                if( _ABC형보기인지 == 1)  문자열들.Add(현재라인.Trim());                     // ③임
                if( _ABC형보기인지 == 2)  문자열들.Add(AB형하이픈넣기(현재라인.Trim()));     // ③임
                if( _ABC형보기인지 == 3)  문자열들.Add(ABC형하이픈넣기(현재라인.Trim()));    // ③임

                바로전_원형문자위치 = 맨처음나오는_원형문자위치;
                맨처음나오는_원형문자위치 = 빈칸없는원문.IndexOfAny(원형문자들, 맨처음나오는_원형문자위치 + 1);
                현재라인 = 빈칸없는원문.Substring(바로전_원형문자위치, 맨처음나오는_원형문자위치 - 바로전_원형문자위치); // 예를 들면 ④ 보기4
                현재라인 = 현재라인.Replace("\n", " ");

                if( _ABC형보기인지 == 1)  문자열들.Add(현재라인.Trim());                     // ④임
                if( _ABC형보기인지 == 2)  문자열들.Add(AB형하이픈넣기(현재라인.Trim()));     // ④임
                if( _ABC형보기인지 == 3)  문자열들.Add(ABC형하이픈넣기(현재라인.Trim()));    // ④임

                현재라인 = 빈칸없는원문.Substring(맨처음나오는_원형문자위치); // 예를 들면 ⑤ 보기5
                현재라인 = 현재라인.Replace("\n", " ");

                if( _ABC형보기인지 == 1)  문자열들.Add(현재라인.Trim());                     // ⑤임
                if( _ABC형보기인지 == 2)  문자열들.Add(AB형하이픈넣기(현재라인.Trim()));     // ⑤임
                if( _ABC형보기인지 == 3)  문자열들.Add(ABC형하이픈넣기(현재라인.Trim()));    // ⑤임


                전처리(ref 문자열들);

                if(문자열들.Count == 5)
                {
					A0 = "";
                    A1 = 문자열들[0];
                    A2 = 문자열들[1];
                    A3 = 문자열들[2];
                    A4 = 문자열들[3];
                    A5 = 문자열들[4];

                    return true;
                }
                if(문자열들.Count == 6)
                {
                    A0 = 문자열들[0];
                    A1 = 문자열들[1];
                    A2 = 문자열들[2];
                    A3 = 문자열들[3];
                    A4 = 문자열들[4];
                    A5 = 문자열들[5];

                    return true;
                }

                return false;
            }
                //
            else
            {
                변환.문자열.개행문자로_구분한_문자열들로(빈칸없는원문, ref 문자열들);

                전처리(ref 문자열들);

                if(문자열들.Count == 6)
                {
					A0 = 문자열들[0];
					A1 = 문자열들[1];
					A2 = 문자열들[2];
					A3 = 문자열들[3];
					A4 = 문자열들[4];
					A5 = 문자열들[5];

					return true;
				}
				if (문자열들.Count == 5)
                {
					A0 = "";
					A1 = 문자열들[0];
					A2 = 문자열들[1];
					A3 = 문자열들[2];
					A4 = 문자열들[3];
					A5 = 문자열들[4];

					return true;
				}
				return false;
            }
        }

        private static string ABC형하이픈넣기(string 문자열)
        {
            문자열 = 문자열.Replace(".", "");
            문자열 = 문자열.Replace("…", "");
            문자열 = 문자열.Replace("·", "");
            
            string 처리된결과 = "";

            if(문자열.Contains("(A) (B) (C)"))
            {
                문자열 = 문자열.Replace("(A) (B) (C)", "(A) …… (B) …… (C)");
                return 문자열;
            }
            else if (변환.문자열.Left(문자열.Trim(), 1) == "①")
            {
                문자열 = 문자열.Substring(1).Trim();
                처리된결과 += "① ";
            }
            else if (변환.문자열.Left(문자열.Trim(), 1) == "②")
            {
                문자열 = 문자열.Substring(1).Trim();
                처리된결과 += "② ";
            }
            else if (변환.문자열.Left(문자열.Trim(), 1) == "③")
            {
                문자열 = 문자열.Substring(1).Trim();
                처리된결과 += "③ ";
            }
            else if (변환.문자열.Left(문자열.Trim(), 1) == "④")
            {
                문자열 = 문자열.Substring(1).Trim();
                처리된결과 += "④ ";
            }
            else if (변환.문자열.Left(문자열.Trim(), 1) == "⑤")
            {
                문자열 = 문자열.Substring(1).Trim();
                처리된결과 += "⑤ ";
            }
            

            if(_A의앞부분.Length > _A의뒷부분.Length)
            {
                if(변환.문자열.Left(문자열.Trim(), _A의앞부분.Length) == _A의앞부분)
                {
                    문자열 = 문자열.Substring(_A의앞부분.Length).Trim();
                    처리된결과 += _A의앞부분;
                }
                if(변환.문자열.Left(문자열.Trim(), _A의뒷부분.Length) == _A의뒷부분)
                {
                    문자열 = 문자열.Substring(_A의뒷부분.Length).Trim();
                    처리된결과 += _A의뒷부분;
                }
            }
            else
            {
                if(변환.문자열.Left(문자열.Trim(), _A의뒷부분.Length) == _A의뒷부분)
                {
                    문자열 = 문자열.Substring(_A의뒷부분.Length).Trim();
                    처리된결과 += _A의뒷부분;
                }
                if(변환.문자열.Left(문자열.Trim(), _A의앞부분.Length) == _A의앞부분)
                {
                    문자열 = 문자열.Substring(_A의앞부분.Length).Trim();
                    처리된결과 += _A의앞부분;
                }
            }

            if(_B의앞부분.Length > _B의뒷부분.Length)
            {
                if(변환.문자열.Left(문자열.Trim(), _B의앞부분.Length) == _B의앞부분)
                {
                    문자열 = 문자열.Substring(_B의앞부분.Length).Trim();
                    처리된결과 += " …… " + _B의앞부분;
                }
                if(변환.문자열.Left(문자열.Trim(), _B의뒷부분.Length) == _B의뒷부분)
                {
                    문자열 = 문자열.Substring(_B의뒷부분.Length).Trim();
                    처리된결과 += " …… " + _B의뒷부분;
                }
            }
            else
            {
                if(변환.문자열.Left(문자열.Trim(), _B의뒷부분.Length) == _B의뒷부분)
                {
                    문자열 = 문자열.Substring(_B의뒷부분.Length).Trim();
                    처리된결과 += " …… " + _B의뒷부분;
                }
                if(변환.문자열.Left(문자열.Trim(), _B의앞부분.Length) == _B의앞부분)
                {
                    문자열 = 문자열.Substring(_B의앞부분.Length).Trim();
                    처리된결과 += " …… " + _B의앞부분;
                }
            }

            if(_C의앞부분.Length > _C의뒷부분.Length)
            {
                if(변환.문자열.Left(문자열.Trim(), _C의앞부분.Length) == _C의앞부분)
                {
                    문자열 = 문자열.Substring(_C의앞부분.Length).Trim();
                    처리된결과 += " …… " + _C의앞부분;
                }
                if(변환.문자열.Left(문자열.Trim(), _C의뒷부분.Length) == _C의뒷부분)
                {
                    문자열 = 문자열.Substring(_C의뒷부분.Length).Trim();
                    처리된결과 += " …… " + _C의뒷부분;
                }
            }
            else
            {
                if(변환.문자열.Left(문자열.Trim(), _C의뒷부분.Length) == _C의뒷부분)
                {
                    문자열 = 문자열.Substring(_C의뒷부분.Length).Trim();
                    처리된결과 += " …… " + _C의뒷부분;
                }
                if(변환.문자열.Left(문자열.Trim(), _C의앞부분.Length) == _C의앞부분)
                {
                    문자열 = 문자열.Substring(_C의앞부분.Length).Trim();
                    처리된결과 += " …… " + _C의앞부분;
                }
            }

            return 처리된결과;
        }

        private static string AB형하이픈넣기(string 문자열)
        {
            문자열 = 문자열.Replace("(A) (B) (A) (B)", "(A) …… (B)");

            문자열 = 문자열.Replace("(A) (B)", "(A) …… (B)");

            문자열 = 문자열.Replace(" ······ ", " …… ");

            return 문자열;
        }


        private static void 전처리(ref List<string> 문자열들)
        {
            for(int i = 0 ; i < 문자열들.Count ; i++ )
            {
                while(문자열들[i].Contains("  "))
                    문자열들[i] = 문자열들[i].Replace("  "," ");

                문자열들[i] = 문자열들[i].Replace(" ...... "," …… ");
                문자열들[i] = 문자열들[i].Replace(" ..... ", " …… ");

                문자열들[i] = 문자열들[i].Replace("①","");
                문자열들[i] = 문자열들[i].Replace("②","");
                문자열들[i] = 문자열들[i].Replace("③","");
                문자열들[i] = 문자열들[i].Replace("④","");
                문자열들[i] = 문자열들[i].Replace("⑤","");

                문자열들[i] = 문자열들[i].Replace("(B)-(C)-(D)","(B) …… (C) …… (D)");
                문자열들[i] = 문자열들[i].Replace("(B)-(D)-(C)","(B) …… (D) …… (C)");
                문자열들[i] = 문자열들[i].Replace("(C)-(B)-(D)","(C) …… (B) …… (D)");
                문자열들[i] = 문자열들[i].Replace("(C)-(D)-(B)","(C) …… (D) …… (B)");
                문자열들[i] = 문자열들[i].Replace("(D)-(B)-(C)","(D) …… (B) …… (C)");
                문자열들[i] = 문자열들[i].Replace("(D)-(C)-(B)","(D) …… (C) …… (B)");

                문자열들[i] = 문자열들[i].Trim();

                // 맨 앞에 ⓒ가 나오면 인식 에러일 가능성이 높다. 제거한다.
                if(변환.문자열.Left(문자열들[i], 1) == "ⓒ") 
                {
                    문자열들[i] = 문자열들[i].Substring(1);
                    문자열들[i] = 문자열들[i].Trim();
                }
				if (변환.문자열.Substring강력(문자열들[i], 0, 2) == "1 ")	문자열들[i] = 문자열들[i].Substring(2);
				if (변환.문자열.Substring강력(문자열들[i], 0, 2) == "2 ") 문자열들[i] = 문자열들[i].Substring(2);
				if (변환.문자열.Substring강력(문자열들[i], 0, 2) == "3 ") 문자열들[i] = 문자열들[i].Substring(2);
				if (변환.문자열.Substring강력(문자열들[i], 0, 2) == "4 ") 문자열들[i] = 문자열들[i].Substring(2);
				if (변환.문자열.Substring강력(문자열들[i], 0, 2) == "5 ") 문자열들[i] = 문자열들[i].Substring(2);

				if (문자열들[i] == "") { 문자열들.RemoveAt(i); i--; };
					
            }
        }
    }
}
