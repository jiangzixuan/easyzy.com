using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_CQuestion : T_CQuestions
    {
        public T_QuesOptions Options { get; set; }
    }
}
