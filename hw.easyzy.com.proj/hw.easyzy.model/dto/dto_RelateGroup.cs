using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_RelateGroup
    {
        public int GradeId { get; set; }

        public string GradeName { get; set; }

        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public int SubmitCount { get; set; }

        public int TotalCount { get; set; }
    }
}
