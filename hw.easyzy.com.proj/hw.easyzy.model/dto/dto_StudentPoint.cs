using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_StudentPoint
    {
        public int ZyId { get; set; }

        public int StudentId { get; set; }

        public DateTime SubmitDate { get; set; }

        public int Score { get; set; }

        public string UserName { get; set; }

        public string TrueName { get; set; }

        public long NewId { get; set; }
    }
}
