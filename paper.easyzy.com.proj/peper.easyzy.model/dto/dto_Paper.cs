using paper.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paper.easyzy.model.dto
{
    public class dto_Paper : T_Paper
    {
        public long NewId { get; set; }
        public string GradeName { get; set; }
    }
}
