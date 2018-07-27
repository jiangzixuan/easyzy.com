using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paper.easyzy.model.dto
{
    public class dto_UserAnswer
    {
        /// <summary>
        /// 试题Id
        /// </summary>
        public int QId { get; set; }

        /// <summary>
        /// 题型Id，转化为2，3，4，5，6，7后的
        /// </summary>
        public int PTypeId { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 正确答案（客观题有效）
        /// </summary>
        public string CAnswer { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public decimal Point { get; set; }

        /// <summary>
        /// 本题总分值
        /// </summary>
        public decimal Score { get; set; }
    }
}
