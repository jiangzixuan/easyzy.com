using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_UserZy
    {
        public int ZyId { get; set; }

        public int UserId { get; set; }

        public string ZyNum { get; set; }

        public DateTime CreateDate { get; set; }

        public string BodyHtmlPath { get; set; }

        public string AnswerHtmlPath { get; set; }

        public bool Structed { get; set; }
    }
}
