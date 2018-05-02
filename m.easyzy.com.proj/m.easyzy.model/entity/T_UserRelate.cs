using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.model.entity
{
    public class T_UserRelate
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RUserId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
