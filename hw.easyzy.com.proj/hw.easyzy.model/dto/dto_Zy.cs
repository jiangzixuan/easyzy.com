using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_Zy : T_Zy
    {
        public string SubjectName { get; set; }

        public string TypeName { get; set; }

        public List<dto_RelateGroup> ClassSubmitInfo { get; set; }

        public string OpenDateStr { get; set; }

        public string DueDateStr { get; set; }

        public long NewId { get; set; }
    }
}
