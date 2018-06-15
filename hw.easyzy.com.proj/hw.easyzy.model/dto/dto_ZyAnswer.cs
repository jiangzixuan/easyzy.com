using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    /// <summary>
    /// 生成T_Answer的AnswerJson实体类
    /// </summary>
    public class dto_ZyAnswer
    {
        public int QId { get; set; }

        public int PTypeId { get; set; }

        public string Answer { get; set; }

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
