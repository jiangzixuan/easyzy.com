using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.easyzy.model.entity
{
    public class T_ModifyRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int FromSchoolId { get; set; }

        public int ToSchoolId { get; set; }

        public string Reason { get; set; }

        /// <summary>
        /// 0：未处理 1：取消 2：已处理
        /// </summary>
        public int Status { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
