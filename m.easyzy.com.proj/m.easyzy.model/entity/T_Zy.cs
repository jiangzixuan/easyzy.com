using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.model.entity
{
    public class T_Zy
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ZyName { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime DueDate { get; set; }

        /// <summary>
        /// 0: Qdb 1: Self
        /// </summary>
        public int Type { get; set; }

        public DateTime CreateDate { get; set; }

        public string Ip { get; set; }

        public string IMEI { get; set; }

        public string MobileBrand { get; set; }

        public string SystemType { get; set; }

        public string Browser { get; set; }

        public int CourseId { get; set; }

        public int SubjectId { get; set; }

        /// <summary>
        /// 0: 正常 1:已关闭（能打开不能提交） 2:已删除（不能打开）
        /// </summary>
        public int Status { get; set; }
    }
}
