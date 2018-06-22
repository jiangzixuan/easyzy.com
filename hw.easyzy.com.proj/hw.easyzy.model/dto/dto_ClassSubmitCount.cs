using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_ClassSubmitCount
    {
        public int SchoolId { get; set; }

        public string SchoolName { get; set; }

        public int GradeId { get; set; }

        public string GradeName { get; set; }

        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public int SubmitCount { get; set; }
    }
}
