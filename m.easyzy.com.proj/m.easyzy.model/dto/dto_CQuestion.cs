using m.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.model.dto
{
    public class dto_CQuestion : T_CQuestions
    {
        public T_QuesOptions Options { get; set; }

        public long NewId { get; set; }

        /// <summary>
        /// 学生答案，兼容查看已完成作业
        /// </summary>
        public string SAnswer { get; set; }
    }
}
