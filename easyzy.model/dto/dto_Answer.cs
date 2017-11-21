using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.model.dto
{
    public class dto_Answer
    {
        public int StructId { get; set; }

        public int BqNum { get; set; }

        public int SqNum { get; set; }

        public string Answer { get; set; }
    }

    public class dto_Answer2 : dto_Answer
    {
        /// <summary>
        /// 正确答案
        /// </summary>
        public string QuesAnswer { get; set; }
    }
}
