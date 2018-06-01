using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_QuesType
    {
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// 题型ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 题型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否客观题
        /// </summary>
        public bool Objective { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Ordinal { get; set; }
        /// <summary>
        /// 父题型ID
        /// </summary>
        public int ParentId { get; set; }
    }
}
