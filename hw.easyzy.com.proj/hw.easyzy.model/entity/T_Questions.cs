using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_Questions
    {
        public int id { get; set; }

        public int courseid { get; set; }

        public int typeid { get; set; }

        public string typename { get; set; }

        public int difftype { get; set; }

        public float diff { get; set; }

        public int paperid { get; set; }

        public int paperyear { get; set; }

        public int papertypeid { get; set; }

        public bool haschildren { get; set; }

        public string quesbody { get; set; }

        public string quesanswer { get; set; }

        public string quesparse { get; set; }

        public string kpoints { get; set; }

        public string cpoints { get; set; }
    }
}
