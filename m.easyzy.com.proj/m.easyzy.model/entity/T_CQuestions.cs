using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.model.entity
{
    public class T_CQuestions
    {
        public int id { get; set; }

        public int pid { get; set; }

        /// <summary>
        /// 客观题转化为2，3，4，5，6，7后的题型Id
        /// </summary>
        public int ptypeid { get; set; }

        /// <summary>
        /// 题型Id
        /// </summary>
        public int typeid { get; set; }

        public string typename { get; set; }

        public string quesbody { get; set; }

        public string quesanswer { get; set; }

        public string quesparse { get; set; }
        
    }
}
