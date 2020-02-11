using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace 편집기의_제왕
{
    public partial class 오른쪽클릭메뉴 : Form
    {
        public 오른쪽클릭메뉴()
        {
            InitializeComponent();
        }

        private void 오른쪽클릭메뉴_MouseLeave(object sender, EventArgs e)
        {
        }




        private void 오른쪽클릭메뉴_Deactivate(object sender, EventArgs e)
        {
       //     this.Close();
            //thisnd.DestroyHandle();
            //this.Dispose();
        }

        private void 사전편집창열기_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

			

            폼1.사전편집창띄우기();
        }

        private void 명사_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_명사();
        }

        private void 명사구_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_명사구();
        }

        private void 형용사_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_형용사();
        }

        private void 동사_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_동사();
        }

        private void 동사ed_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_동사ed();
        }

        private void 동사es_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_동사es();
        }

        private void 명령주제_Click(object sender, EventArgs e)
        {

        }

        private void 속담_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_속담빈칸();
        }

        private void 서술어로시작하는문구_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우_서술어시작문구();
        }

        private void 서술어s로시작하는문구_Click(object sender, EventArgs e)
        {
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_서술어s시작문구();
        }

        private void 명사_바로적용_Click(object sender, EventArgs e)
        {

        }

        private void 명사구_바로적용_Click(object sender, EventArgs e)
        {

        }

        private void 형용사_바로적용_Click(object sender, EventArgs e)
        {

        }

        private void 명령주제_바로적용_Click(object sender, EventArgs e)
        {

        }

        private void 속담_바로적용_Click(object sender, EventArgs e)
        {

        }

        private void 서술어로시작하는문구_바로적용_Click(object sender, EventArgs e)
        {

        }

        private void 서술어s로시작하는문구_바로적용_Click(object sender, EventArgs e)
        {

        }

		private void 문장_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_문장빈칸();

		}

        private void pixabay검색_Click(object sender, EventArgs e)
        {
            Form1 폼1 = (Form1)Owner;

            this.Hide();

            폼1.우클릭_선택영역_PIXABAY열기();
        }

		private void 동의어검색_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우클릭_선택영역_동의어찾기();
		}

		private void 위키사전검색_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우클릭_선택영역_위키사전();
		}

		private void 위키백과검색_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우클릭_선택영역_위키백과();
		}

		private void 영작_고교_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_영작고교();
		}

		private void 영작_중3_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_영작중3();
		}

		private void 영작_중2_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_영작중2();
		}

		private void 영작_중1_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_영작중1();
		}

		private void 빈칸_고교_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_빈칸고교();
		}

		private void 빈칸_중3_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_빈칸중3();
		}

		private void 빈칸_중2_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_빈칸중2();
		}

		private void 빈칸_중1_Click(object sender, EventArgs e)
		{
			Form1 폼1 = (Form1)Owner;

			this.Hide();

			폼1.우_빈칸중1();
		}
	}
}
