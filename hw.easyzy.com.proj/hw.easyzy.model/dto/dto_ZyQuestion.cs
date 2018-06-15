using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    /// <summary>
    /// 生成T_ZyQuestions的QuesJson实体类
    /// </summary>
    public class dto_ZyQuestion
    {
        public int PQId { get; set; }

        public int QId { get; set; }

        /// <summary>
        /// 选择题转化为2，3，4，5，6，7之后的题型Id
        /// </summary>
        public int PTypeId { get; set; }

        public int OrderIndex { get; set; }

        public decimal Score { get; set; }
    }
}
