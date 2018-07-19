using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paper.easyzy.model.entity
{
    public class T_Paper
    {
        public int PaperId { get; set; }

        public int CourseId { get; set; }

        public int GradeId { get; set; }

        public int TypeId { get; set; }

        public int PaperYear { get; set; }

        /// <summary>
        /// ProvinceId
        /// </summary>
        public int AreaId { get; set; }

        public string Title { get; set; }

        public string QuestionIds { get; set; }
    }
}
