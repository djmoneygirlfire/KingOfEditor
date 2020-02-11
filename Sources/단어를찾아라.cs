using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using 검색_진화하는;

namespace 편집기의_제왕
{
    class 단어를찾아라
    {
        public static string _단어장루트폴더 = "C:/Users/Administrator/Google 드라이브/편집기의제왕단어장/";

        public static List<string> _단어들;
        public static Bitmap _비트맵;
        public static Graphics _그래픽;

        public static Font _영문폰트;

        public static Font _한글폰트;

        public static SolidBrush _브러쉬;

        public static RectangleF[] _단어위치;
        public static RectangleF[] _해설위치;




        public static StringFormat _글자형식;
        public static StringFormat _해설형식;

        public static 검색 _검색;

        static 단어를찾아라()
        {
            _단어들 = new List<string>();
            _비트맵 = new Bitmap(2480, 3508);
            _그래픽 = Graphics.FromImage(_비트맵);

            _영문폰트 = new Font("Segoe UI", 32);
            _한글폰트 = new Font("맑은 고딕", 20);
            _브러쉬 = new SolidBrush(Color.Black);

            _단어위치 = new RectangleF[] { 
                new RectangleF(180.0f, 190.6f, 400.0f, 200.0f),
                new RectangleF(552.0f, 351.0f, 400.0f, 200.0f), 
                new RectangleF(190.0f, 567.0f, 400.0f, 200.0f), 
                new RectangleF(795.0f, 674.0f, 400.0f, 200.0f), 
                new RectangleF(249.0f, 914.0f, 400.0f, 200.0f), 
                new RectangleF(687.0f, 972.0f, 400.0f, 200.0f), 
                new RectangleF(1200.0f, 1008.0f, 400.0f, 200.0f),
                new RectangleF(408.0f, 1206.0f, 400.0f, 200.0f),
                new RectangleF(249.0f, 1524.0f, 400.0f, 200.0f), 
                new RectangleF(741.0f, 1398.0f, 400.0f, 200.0f), 
                new RectangleF(1359.0f, 1308.0f, 400.0f, 200.0f), 
                new RectangleF(135.0f, 1830.0f, 400.0f, 200.0f), 
                new RectangleF(639.0f, 1737.0f, 400.0f, 200.0f), 
                new RectangleF(1185.0f, 1626.0f, 400.0f, 200.0f), 
                new RectangleF(1632.0f, 1581.0f, 400.0f, 200.0f), 
                new RectangleF(414.0f, 2171.0f, 400.0f, 200.0f), 
                new RectangleF(876.0f, 2006.0f, 400.0f, 200.0f), 
                new RectangleF(1296.0f, 1919.0f, 400.0f, 200.0f), 
                new RectangleF(1848.0f, 1838.0f, 400.0f, 200.0f), 
                new RectangleF(132.0f, 2444.0f, 400.0f, 200.0f), 
                new RectangleF(801.0f, 2345.0f, 400.0f, 200.0f), 
                new RectangleF(1248.0f, 2225.0f, 400.0f, 200.0f), 
                new RectangleF(1695.0f, 2141.0f, 400.0f, 200.0f), 
                new RectangleF(522.0f, 2651.0f, 400.0f, 200.0f), 
                new RectangleF(1083.0f, 2666.0f, 400.0f, 200.0f), 
                new RectangleF(1509.0f, 2504.0f, 400.0f, 200.0f), 
                new RectangleF(2016.0f, 2435.0f, 400.0f, 200.0f), 
                new RectangleF(246.0f, 2945.0f, 400.0f, 200.0f), 
                new RectangleF(804.0f, 2993.0f, 400.0f, 200.0f), 
                new RectangleF(1431.0f, 2942.0f, 400.0f, 200.0f), 
                new RectangleF(1851.0f, 2801.0f, 400.0f, 200.0f), 
                new RectangleF(1851.0f, 3086.0f, 400.0f, 200.0f), 

            };

            _해설위치 = new RectangleF[] { 
                new RectangleF(180.0f, 290.6f, 400.0f, 200.0f),
                new RectangleF(552.0f, 451.0f, 400.0f, 200.0f), 
                new RectangleF(190.0f, 667.0f, 400.0f, 200.0f), 
                new RectangleF(795.0f, 774.0f, 400.0f, 200.0f), 
                new RectangleF(249.0f, 1014.0f, 400.0f, 200.0f), 
                new RectangleF(687.0f, 1072.0f, 400.0f, 200.0f), 
                new RectangleF(1200.0f, 1108.0f, 400.0f, 200.0f), 
                new RectangleF(408.0f, 1306.0f, 400.0f, 200.0f),
                new RectangleF(249.0f, 1624.0f, 400.0f, 200.0f), 
                new RectangleF(741.0f, 1498.0f, 400.0f, 200.0f), 
                new RectangleF(1359.0f, 1408.0f, 400.0f, 200.0f), 
                new RectangleF(135.0f, 1930.0f, 400.0f, 200.0f), 
                new RectangleF(639.0f, 1837.0f, 400.0f, 200.0f), 
                new RectangleF(1185.0f, 1726.0f, 400.0f, 200.0f), 
                new RectangleF(1632.0f, 1681.0f, 400.0f, 200.0f), 
                new RectangleF(414.0f, 2271.0f, 400.0f, 200.0f), 
                new RectangleF(876.0f, 2106.0f, 400.0f, 200.0f), 
                new RectangleF(1296.0f, 2019.0f, 400.0f, 200.0f), 
                new RectangleF(1848.0f, 1938.0f, 400.0f, 200.0f), 
                new RectangleF(132.0f, 2544.0f, 400.0f, 200.0f), 
                new RectangleF(801.0f, 2445.0f, 400.0f, 200.0f), 
                new RectangleF(1248.0f, 2325.0f, 400.0f, 200.0f), 
                new RectangleF(1695.0f, 2241.0f, 400.0f, 200.0f), 
                new RectangleF(522.0f, 2751.0f, 400.0f, 200.0f), 
                new RectangleF(1083.0f, 2766.0f, 400.0f, 200.0f), 
                new RectangleF(1509.0f, 2604.0f, 400.0f, 200.0f), 
                new RectangleF(2016.0f, 2535.0f, 400.0f, 200.0f), 
                new RectangleF(246.0f, 3045.0f, 400.0f, 200.0f), 
                new RectangleF(804.0f, 3093.0f, 400.0f, 200.0f), 
                new RectangleF(1431.0f, 3042.0f, 400.0f, 200.0f), 
                new RectangleF(1851.0f, 2901.0f, 400.0f, 200.0f), 
                new RectangleF(1851.0f, 3186.0f, 400.0f, 200.0f), 

            };

            _글자형식 = new StringFormat();
            _글자형식.Alignment = StringAlignment.Center;

            _해설형식 = new StringFormat();
            _해설형식.Alignment = StringAlignment.Center;

            _검색 = new 검색();
        }

        ~단어를찾아라()
        {
            _비트맵.Dispose();
            _그래픽.Dispose();
        }

        public static string 심플한버전(string 선택된_문자열)
        {
            string 결과물 = "";

            변환.문자열.문자열들로n(선택된_문자열, ref _단어들);
            for (int i = 0; i < _단어들.Count; i++)
            {
                결과물 += _단어들[i];
                결과물 += " : ";

                for (int j = 0; j < 20 - _단어들[i].Length; j++)
                {
                    결과물 += " ";
                }

                결과물 += _검색.영한사전(_단어들[i]);
                결과물 += "\n";
            }

            return 결과물;
        }


        public static void 만들기(string 선택된_문자열)
        {
            #region 폴더 초기화 부분

            if (Directory.Exists(_단어장루트폴더))
            {
                DirectoryInfo dir = new DirectoryInfo(_단어장루트폴더);
                System.IO.FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (System.IO.FileInfo file in files)
                    file.Delete();
            }
            else
                Directory.CreateDirectory(_단어장루트폴더);
            #endregion

            #region JPG 옵션 설정부분
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            #endregion

            //변환.문자열.문자열들로n(선택된_문자열, ref _단어들);
            변환.문자열.영어만남긴문자열들로n(선택된_문자열, ref _단어들);

            int 단어장페이지 = 1;
            string 단어장경로;

            #region 단어순서에 따른 처리
            for (int i = 0; i < _단어들.Count; i++)
            {
                if (i % 32 == 0)
                {
                    if (i != 0)
                    {
                        단어장경로 = String.Format("{0}{1:D3}.jpg", _단어장루트폴더, 단어장페이지);
                        _비트맵.SetResolution(300, 300);
                        _비트맵.Save(단어장경로, jpgEncoder, myEncoderParameters);
                        단어장페이지++;
                    }

                    _그래픽.Clear(Color.White);
                    Image 단어장배경 = Image.FromFile(String.Format("{0}필요이미지/모험의 세계 단어장 배경 2.jpg", _단어장루트폴더));
                    _그래픽.DrawImage(단어장배경, new Rectangle(0, 0, 2480, 3508));
                }

                _그래픽.DrawString(_단어들[i], _영문폰트, _브러쉬, _단어위치[i % 32], _글자형식);
                _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(_단어들[i], 17), _한글폰트, _브러쉬, _해설위치[i % 32], _해설형식);


            }

            단어장경로 = String.Format("{0}{1:D3}.jpg", _단어장루트폴더, 단어장페이지);
            #endregion

            _비트맵.SetResolution(300, 300);
            _비트맵.Save(단어장경로, jpgEncoder, myEncoderParameters);

            _단어들.Clear();

        }

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
    }
}
