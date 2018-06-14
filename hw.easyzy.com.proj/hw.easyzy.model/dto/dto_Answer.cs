using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    /// <summary>
    /// 老的自传作业的
    /// </summary>
    public class dto_Answer
    {
        public int StructId { get; set; }

        public int BqNum { get; set; }

        public int SqNum { get; set; }

        public string Answer { get; set; }
    }

    /// <summary>
    /// 获取学生答题卡，需要有试题正确答案
    /// </summary>
    public class dto_Answer2 : dto_Answer
    {
        /// <summary>
        /// 正确答案
        /// </summary>
        public string QuesAnswer { get; set; }
    }

    /// <summary>
    /// 作业查询，返回每个作业的客观题正确率
    /// </summary>
    public class dto_Answer3 : T_Answer
    {
        /// <summary>
        /// 客观题数
        /// </summary>
        public int ObjectiveQuesCount { get; set; }

        /// <summary>
        /// 客观题正确数
        /// </summary>
        public int ObjectiveQuesCorrectCount { get; set; }
    }
}
