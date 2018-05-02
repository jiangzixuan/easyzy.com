using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.easyzy.model.entity
{
    public class T_Zy
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string BodyWordPath { get; set; }

        public string BodyHtmlPath { get; set; }

        public string AnswerWordPath { get; set; }

        public string AnswerHtmlPath { get; set; }

        public DateTime CreateDate { get; set; }

        public string Ip { get; set; }

        public string IMEI { get; set; }

        public string MobileBrand { get; set; }

        public string SystemType { get; set; }

        public string Browser { get; set; }

        public bool Structed { get; set; }
    }
}
