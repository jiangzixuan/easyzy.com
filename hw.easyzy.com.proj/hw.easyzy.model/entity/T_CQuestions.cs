using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_CQuestions
    {
        public int id { get; set; }

        public int parentid { get; set; }

        public int orderindex { get; set; }

        public int typeid { get; set; }

        public string typename { get; set; }

        public string quesbody { get; set; }

        public string quesanswer { get; set; }

        public string quesparse { get; set; }
        
    }
}
