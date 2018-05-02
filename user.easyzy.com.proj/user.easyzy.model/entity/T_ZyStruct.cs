using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.easyzy.model.entity
{
    public class T_ZyStruct
    {
        public int Id { get; set; }

        public int ZyId { get; set; }

        /// <summary>
        /// Big Ques Num
        /// </summary>
        public int BqNum { get; set; }

        /// <summary>
        /// Small Ques Num
        /// </summary>
        public int SqNum { get; set; }

        /// <summary>
        /// 0：客观题；1：主观题
        /// </summary>
        public int QuesType { get; set; }

        /// <summary>
        /// 客观题答案
        /// </summary>
        public string QuesAnswer { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
