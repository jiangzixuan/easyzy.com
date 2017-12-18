using easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyzy.model.dto
{
    public class dto_RelateUser:T_UserRelate
    {
        public string RUserName { get; set; }

        public string RTrueName { get; set; }
    }
}
