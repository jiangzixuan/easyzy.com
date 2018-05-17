using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbs.easyzy.common
{
    /// <summary>
    /// 活跃度权重常量
    /// </summary>
    public class ActivityWeightConst
    {
        /// <summary>
        /// 计算活跃度的功能枚举(Controller_Action)
        /// </summary>
        public enum ActivityFuncEnum
        {
            topic_detail,
            topic_addtopic,
            topic_reply
        }

        /// <summary>
        /// 活跃度权重
        /// </summary>
        public static Dictionary<string, int> ActivityWeights = new Dictionary<string, int>()
        {
            { "topic_detail", 1},
            { "topic_addtopic", 5},
            { "topic_reply", 2},
        };
    }
}
