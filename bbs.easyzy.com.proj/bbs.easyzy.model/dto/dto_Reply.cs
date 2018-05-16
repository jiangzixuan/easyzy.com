using bbs.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbs.easyzy.model.dto
{
    public class dto_Reply:T_Reply
    {
        public string UserName { get; set; }

        public string TrueName { get; set; }
    }
}
