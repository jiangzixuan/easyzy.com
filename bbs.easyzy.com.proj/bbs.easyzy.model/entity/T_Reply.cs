using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbs.easyzy.model.entity
{
    public class T_Reply
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int TopicId { get; set; }

        public string ReplyContent { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Deleted { get; set; }

        public bool Blocked { get; set; }

        public string Ip { get; set; }
    }
}
