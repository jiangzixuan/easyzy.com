using m.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.model.dto
{
    public class dto_Zy : T_Zy
    {
        public string SubjectName { get; set; }

        public string TypeName { get; set; }

        public string OpenDateStr { get; set; }

        public string DueDateStr { get; set; }

        public long NewId { get; set; }

        public string UserName { get; set; }
        public string TrueName { get; set; }

        /// <summary>
        /// 是否提交过
        /// </summary>
        public bool Submited { get; set; }
    }
}
