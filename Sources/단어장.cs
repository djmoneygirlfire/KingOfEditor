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
    class 단어장
    {
        public static string _단어장루트폴더 = "C:/Users/Administrator/Google 드라이브/편집기의제왕단어장/";

        public static List<string>  _단어들;
        public static Bitmap        _비트맵;
        public static Graphics      _그래픽;

        public static Font          _영문폰트;

        public static Font          _한글폰트;

        public static SolidBrush _브러쉬;

        public static RectangleF    _단어위치0101, _단어위치0102, _단어위치0103, _단어위치0104,
                                    _단어위치0201, _단어위치0202, _단어위치0203, _단어위치0204,
                                    _단어위치0301, _단어위치0302, _단어위치0303, _단어위치0304,
                                    _단어위치0401, _단어위치0402, _단어위치0403, _단어위치0404;

        public static RectangleF    _해설위치0101, _해설위치0102, _해설위치0103, _해설위치0104,
                                    _해설위치0201, _해설위치0202, _해설위치0203, _해설위치0204,
                                    _해설위치0301, _해설위치0302, _해설위치0303, _해설위치0304,
                                    _해설위치0401, _해설위치0402, _해설위치0403, _해설위치0404;



        public static StringFormat  _글자형식;

        public static StringFormat  _해설형식_좌;
        public static StringFormat  _해설형식_우;

        public static 검색 _검색;

        static 단어장()
        {
            _단어들 = new List<string>();
            _비트맵 = new Bitmap(2480, 3508);
            _그래픽 = Graphics.FromImage(_비트맵);

            _영문폰트 = new Font("Segoe UI", 32);
            _한글폰트 = new Font("맑은 고딕", 20);
            _브러쉬 = new SolidBrush(Color.Black);

            _단어위치0101 = new RectangleF(267.0f,  526.0f, 411.0f, 300.0f);
            _단어위치0102 = new RectangleF(678.0f,  526.0f, 411.0f, 300.0f);
            _단어위치0103 = new RectangleF(1388.0f, 526.0f, 411.0f, 300.0f);
            _단어위치0104 = new RectangleF(1800.0f, 526.0f, 411.0f, 300.0f);

            _단어위치0201 = new RectangleF(267.0f, 1346.0f, 411.0f, 300.0f);
            _단어위치0202 = new RectangleF(678.0f, 1346.0f, 411.0f, 300.0f);
            _단어위치0203 = new RectangleF(1388.0f, 1346.0f, 411.0f, 300.0f);
            _단어위치0204 = new RectangleF(1800.0f, 1346.0f, 411.0f, 300.0f);

            _단어위치0301 = new RectangleF(267.0f, 2164.0f, 411.0f, 300.0f);
            _단어위치0302 = new RectangleF(678.0f, 2164.0f, 411.0f, 300.0f);
            _단어위치0303 = new RectangleF(1388.0f, 2164.0f, 411.0f, 300.0f);
            _단어위치0304 = new RectangleF(1800.0f, 2164.0f, 411.0f, 300.0f);

            _단어위치0401 = new RectangleF(267.0f, 2984.0f, 411.0f, 300.0f);
            _단어위치0402 = new RectangleF(678.0f, 2984.0f, 411.0f, 300.0f);
            _단어위치0403 = new RectangleF(1388.0f, 2984.0f, 411.0f, 300.0f);
            _단어위치0404 = new RectangleF(1800.0f, 2984.0f, 411.0f, 300.0f);

            _해설위치0101 = new RectangleF(526.0f, -678.0f, 390.0f, 300.0f);
            _해설위치0102 = new RectangleF(-914.0f, 678.0f, 390.0f, 300.0f);
            _해설위치0103 = new RectangleF(526.0f, -1800.0f, 390.0f, 300.0f);
            _해설위치0104 = new RectangleF(-914.0f, 1800.0f, 390.0f, 300.0f);

            _해설위치0201 = new RectangleF(1346.0f, -678.0f, 390.0f, 300.0f);
            _해설위치0202 = new RectangleF(-1734.0f, 678.0f, 390.0f, 300.0f);
            _해설위치0203 = new RectangleF(1346.0f, -1800.0f, 390.0f, 300.0f);
            _해설위치0204 = new RectangleF(-1734.0f, 1800.0f, 390.0f, 300.0f);

            _해설위치0301 = new RectangleF(2166.0f, -678.0f, 390.0f, 300.0f);
            _해설위치0302 = new RectangleF(-2554.0f, 678.0f, 390.0f, 300.0f);
            _해설위치0303 = new RectangleF(2166.0f, -1800.0f, 390.0f, 300.0f);
            _해설위치0304 = new RectangleF(-2554.0f, 1800.0f, 390.0f, 300.0f);


            _해설위치0401 = new RectangleF(2981.0f, -678.0f, 390.0f, 300.0f);
            _해설위치0402 = new RectangleF(-3372.0f, 678.0f, 390.0f, 300.0f);
            _해설위치0403 = new RectangleF(2981.0f, -1800.0f, 390.0f, 300.0f);
            _해설위치0404 = new RectangleF(-3372.0f, 1800.0f, 390.0f, 300.0f);

            _글자형식 = new StringFormat();
            _글자형식.Alignment = StringAlignment.Center;


            _해설형식_좌 = new StringFormat();
            _해설형식_좌.Alignment = StringAlignment.Far;

            _해설형식_우 = new StringFormat();
            _해설형식_우.Alignment = StringAlignment.Near;

            _검색 = new 검색();
        }

        ~단어장()
        {
            _비트맵.Dispose();
            _그래픽.Dispose();
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

            변환.문자열.문자열들로n(선택된_문자열, ref _단어들);

            int 단어순서 = 0;
            int 단어장페이지 = 1;
            string 단어장경로;

            #region 단어순서에 따른 처리
            foreach (string 현재단어 in _단어들)
            {
                if (단어순서 % 16 == 0)
                {
                    _그래픽.Clear(Color.White);
                    Image 단어장배경 = Image.FromFile(String.Format("{0}필요이미지/origami3D_wordbook.jpg", _단어장루트폴더));
                    _그래픽.DrawImage(단어장배경, new Rectangle(0, 0, 2480, 3508));

                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0101, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0101, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);
                }
                else if (단어순서 % 16 == 1)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0102, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0102, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }
                else if (단어순서 % 16 == 2)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0103, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0103, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);
                }
                else if (단어순서 % 16 == 3)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0104, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0104, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }
                else if (단어순서 % 16 == 4)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0201, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0201, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);
                }
                else if (단어순서 % 16 == 5)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0202, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0202, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }
                else if (단어순서 % 16 == 6)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0203, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0203, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);
                }
                else if (단어순서 % 16 == 7)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0204, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0204, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }
                else if (단어순서 % 16 == 8)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0301, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0301, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);
                }
                else if (단어순서 % 16 == 9)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0302, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0302, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }
                else if (단어순서 % 16 == 10)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0303, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0303, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);

                }
                else if (단어순서 % 16 == 11)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0304, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0304, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }
                else if (단어순서 % 16 == 12)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0401, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0401, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);
                }
                else if (단어순서 % 16 == 13)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0402, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0402, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }
                else if (단어순서 % 16 == 14)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0403, _글자형식);

                    _그래픽.RotateTransform(90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0403, _해설형식_좌);
                    _그래픽.RotateTransform(-90.0f);
                }
                else if (단어순서 % 16 == 15)
                {
                    _그래픽.DrawString(현재단어, _영문폰트, _브러쉬, _단어위치0404, _글자형식);

                    _그래픽.RotateTransform(-90.0f);
                    _그래픽.DrawString(_검색.적정길이에서_줄바꿈한_영영사전(현재단어), _한글폰트, _브러쉬, _해설위치0404, _해설형식_우);
                    _그래픽.RotateTransform(90.0f);
                }

                단어순서++;

                if(단어순서 == 16)
                {
                    단어장경로 = String.Format("{0}{1:D3}.jpg",_단어장루트폴더, 단어장페이지);
                    _비트맵.SetResolution(300, 300);
                    _비트맵.Save(단어장경로, jpgEncoder, myEncoderParameters);

                    단어장페이지++;
                    단어순서 = 0;
                }
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
