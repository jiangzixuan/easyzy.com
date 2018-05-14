using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbs.easyzy.model.entity
{
    public class T_Topic
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Invites { get; set; }

        public string Title { get; set; }

        public string TopicContent { get; set; }

        public string TopicText { get; set; }

        public DateTime CreateDate { get; set; }

        public int Good { get; set; }

        public int Hit { get; set; }

        public int ReplyCount { get; set; }

        public int GradeId { get; set; }

        public int SubjectId { get; set; }

        public bool Deleted { get; set; }

        public bool Blocked { get; set; }
    }
}
