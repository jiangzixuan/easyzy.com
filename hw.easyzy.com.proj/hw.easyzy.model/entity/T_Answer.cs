﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.entity
{
    public class T_Answer
    {
        public int Id { get; set; }

        public int ZyId { get; set; }

        public int ZyType { get; set; }

        public int StudentId { get; set; }
        
        public string AnswerJson { get; set; }

        public string AnswerImg { get; set; }

        public bool Submited { get; set; }

        public DateTime CreateDate { get; set; }

        public string Ip { get; set; }

        public string IMEI { get; set; }

        public string MobileBrand { get; set; }

        public string SystemType { get; set; }

        public string Browser { get; set; }
    }
}
