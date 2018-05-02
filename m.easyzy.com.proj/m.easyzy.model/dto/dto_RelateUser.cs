using m.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.model.dto
{
    public class dto_RelateUser:T_UserRelate
    {
        /// <summary>
        /// 关注人用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 关注人真实姓名
        /// </summary>
        public string TrueName { get; set; }
        /// <summary>
        /// 被关注人用户名
        /// </summary>
        public string RUserName { get; set; }
        /// <summary>
        /// 被关注人真实姓名
        /// </summary>
        public string RTrueName { get; set; }
    }
}
