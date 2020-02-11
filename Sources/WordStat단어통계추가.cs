using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace 편집기의_제왕
{
	public partial class WordStat단어통계추가 : Form
	{
		
		string _바뀌기_전_파일의_한_줄;

		public WordStat단어통계추가()
		{

			InitializeComponent();

		}

		public string _원래찾을단어 = "";
		public string _줄여가며찾을단어 = "";

		public void 찾을단어세팅(string 줄여가며찾을단어, string 찾을단어)
		{
			_원래찾을단어 = 찾을단어;

			_줄여가며찾을단어 = 줄여가며찾을단어;


			찾기textBox.Text = 찾을단어.ToLower();
		}

		private void 찾기버튼_Click(object sender, EventArgs e)
		{
			찾기(찾기textBox.Text, 찾기textBox.Text);
			this.Close();
		}

		bool _변이형인지여부 = false;


		public bool 찾기(string 찾을단어, string 원래찾을단어)
		{
			Form1 폼1 = (Form1)Owner;

			if (폼1 == null) return false;

			string 사전검색결과 = Form1._검색.영한사전(원래찾을단어);

			if (사전검색결과 == "") _변이형인지여부 = true;
			if (사전검색결과.Contains("s형")) _변이형인지여부 = true;
			if (사전검색결과.Contains("ed형")) _변이형인지여부 = true;
			if (사전검색결과.Contains("ing형")) _변이형인지여부 = true;


			if(_변이형인지여부 == true && 사전검색결과 != "")
				추가하기textBox.Text = "0000)" + 원래찾을단어 + "(0)";
			else
				추가하기textBox.Text = "0000)" + 원래찾을단어 + "(0)";

			기존항목textBox.Text = "";

			_바뀌기_전_파일의_한_줄 = "";


			후보listBox.Items.Clear();


			for (int i = 0; i < 폼1._사용자단어파일_문자열들.Count; i++)
			{
				if (폼1._사용자단어파일_문자열들[i].Contains(")" + 찾을단어) || 폼1._사용자단어파일_문자열들[i].Contains(":" + 찾을단어))
				{
					if (후보listBox.Items.Count == 0)
					{
						기존항목textBox.Text = 폼1._사용자단어파일_문자열들[i];
						_바뀌기_전_파일의_한_줄 = 폼1._사용자단어파일_문자열들[i];
					}

					후보listBox.Items.Add(폼1._사용자단어파일_문자열들[i]);
				}
			}

			if (후보listBox.Items.Count == 0)
				추천하는대안textBox.Text = "";
			else if (_변이형인지여부 == true && !_바뀌기_전_파일의_한_줄.Contains("::"))
				추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄 + "::" + 원래찾을단어 + "(0)";
			else if (_변이형인지여부 == true && _바뀌기_전_파일의_한_줄.Contains("::"))
				추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄 + 원래찾을단어 + "(0)";
			else if (_변이형인지여부 == false && !_바뀌기_전_파일의_한_줄.Contains("::"))
				추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄 + 원래찾을단어 + "(0)";
			else if (_변이형인지여부 == false && _바뀌기_전_파일의_한_줄.Contains("::"))
				추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄.Replace("::", 원래찾을단어 + "(0)::");

			if (후보listBox.Items.Count != 0)
				return true;
			else
				return false;
		}

		public void 모르는단어목록세팅(ref List<string> 단어_사용자파일문자열들)
		{

		}

		private void 찾기textBox_TextChanged(object sender, EventArgs e)
		{
			찾기(찾기textBox.Text, 찾기textBox.Text);
		}

		private void 추가하기버튼_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			string[] files = Directory.GetFiles(폼1._학생정보파일들폴더경로);

			foreach(String 현재 in files)
			{
				if(현재.EndsWith(".nf"))
				{
					학생정보파일에새로운내용추가하기(현재, 추가하기textBox.Text);
				}
			}

			학생정보파일에새로운내용추가하기(폼1._학생정보파일들폴더경로 + "\\txt\\WordStat.txt", 추가하기textBox.Text);

			폼1.화면업데이트중지();
			폼1.독심술();
			폼1.동영상용화면만들기();
			폼1.전체화면다시그리기();
			폼1.그림위치변경_최근();
			폼1.화면업데이트재개();

			this.Close();
		}


		private void 학생정보파일에새로운내용추가하기(string 정보파일경로, string 추가할내용)
		{
			bool 학생정보파일여부 = false;
			// 학생정보파일을 하나씩 읽어서, 맨마지막줄의 </word>나오기 전에 추가하고, </word>를 추가하면 된다.
			List<string> 텍스트파일의_문자열들 = new List<string>();
			List<string> 새로운파일의_문자열들 = new List<string>();


			변환.텍스트파일.문자열들로(정보파일경로, ref 텍스트파일의_문자열들);

			for(int i = 0; i < 텍스트파일의_문자열들.Count; i++)
			{
				if (텍스트파일의_문자열들[i] != "</word>")
					새로운파일의_문자열들.Add(텍스트파일의_문자열들[i]);
				else
				{
					새로운파일의_문자열들.Add(추가할내용);
					새로운파일의_문자열들.Add("</word>");
					학생정보파일여부 = true;
				}
			}

			if(학생정보파일여부 == false)
				새로운파일의_문자열들.Add(추가할내용);


			변환.문자열들.UTF8파일로(새로운파일의_문자열들, 정보파일경로);
		}
		private void 학생정보파일에새로운내용수정하기(string 정보파일경로, string 수정할내용)
		{
			// 학생정보파일을 하나씩 읽어서, 바뀌기전 내용이 있는지 검색하고, 있으면 새로운 내용으로 바꿔준 다음 저장하면 끝난다.
			List<string> 텍스트파일의_문자열들 = new List<string>();


			변환.텍스트파일.문자열들로(정보파일경로, ref 텍스트파일의_문자열들);

			for (int i = 0; i < 텍스트파일의_문자열들.Count; i++)
			{

					if(텍스트파일의_문자열들[i].Replace("#", "").Replace("@", "") == _바뀌기_전_파일의_한_줄.Replace("#", "").Replace("@", ""))
					{
						if (추천하는대안textBox.Text != "") { 텍스트파일의_문자열들[i] = 텍스트파일의_문자열들[i].Replace(_바뀌기_전_파일의_한_줄.Replace("#", "").Replace("@", ""), 추천하는대안textBox.Text.Replace("#", "").Replace("@", "")); }	else { 텍스트파일의_문자열들.RemoveAt(i); }
					}
			}

			변환.문자열들.UTF8파일로(텍스트파일의_문자열들, 정보파일경로);
		}


		private void 수정하기버튼_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			string[] files = Directory.GetFiles(폼1._학생정보파일들폴더경로);

			foreach (String 현재 in files)
			{
				if (현재.EndsWith(".nf"))
				{
					학생정보파일에새로운내용수정하기(현재, 추가하기textBox.Text);
				}
			}

			학생정보파일에새로운내용수정하기(폼1._학생정보파일들폴더경로 + "\\txt\\WordStat.txt", 추가하기textBox.Text);

			폼1.화면업데이트중지();
			폼1.독심술();
			폼1.동영상용화면만들기();
			폼1.전체화면다시그리기();
			폼1.그림위치변경_최근();
			폼1.화면업데이트재개();

			this.Close();
		}

		private void WordStat단어통계추가_Shown(object sender, EventArgs e)
		{
			찾기(_줄여가며찾을단어, _원래찾을단어);
		}

		private void 후보listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			기존항목textBox.Text = 후보listBox.Text;
			_바뀌기_전_파일의_한_줄 = 후보listBox.Text;

			if (_변이형인지여부 == true && !_바뀌기_전_파일의_한_줄.Contains("::")) 추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄 + "::" + _원래찾을단어 + "(0)";
			else if (_변이형인지여부 == true && _바뀌기_전_파일의_한_줄.Contains("::")) 추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄 + _원래찾을단어 + "(0)";
			else if (_변이형인지여부 == false && !_바뀌기_전_파일의_한_줄.Contains("::")) 추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄 + _원래찾을단어 + "(0)";
			else if (_변이형인지여부 == false && _바뀌기_전_파일의_한_줄.Contains("::")) 추천하는대안textBox.Text = _바뀌기_전_파일의_한_줄.Replace("::", _원래찾을단어 + "(0)::");
		}
	}
}
