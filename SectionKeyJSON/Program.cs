using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONSAMPLE
{
    class Program
    {
        static void Main(string[] args)
        {
            SectionKeyJSON Sample = new SectionKeyJSON();

            Sample.WriteData("인사", "GoodMorning", "早上好");
            Sample.WriteData("질문", "How", "怎么样");
            Sample.WriteData("나이", "몇살", "几岁");
            Sample.Write("test.json");

            SectionKeyJSON Sample1 = new SectionKeyJSON();
            Sample1.Read("test.json");
            var str1 = Sample1.ReadData("인사", "GoodMorning");
            var str2 = Sample1.ReadData("질문", "How");
            var str3 = Sample1.ReadData("나이", "몇살");
        }
    }
}
