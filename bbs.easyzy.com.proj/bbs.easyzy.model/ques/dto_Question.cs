﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbs.easyzy.model.ques
{
    public class dto_Question : T_Questions
    {
        public List<dto_CQuestion> Children { get; set; }

        public T_QuesOptions Options { get; set; }

        public long NewId { get; set; }
    }
}
