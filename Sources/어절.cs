using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 편집기의_제왕
{
    public class 어절
    {
	    public string 내용;

	    public float x글자시작좌표;
        public float x글자끝좌표;
        public float x글자앞빈칸중간좌표;
        public float x글자뒷빈칸중간좌표;

        public float y글자시작좌표;
        public float y밑줄좌표;

        public int 몇번째문장인지;
    }
}
