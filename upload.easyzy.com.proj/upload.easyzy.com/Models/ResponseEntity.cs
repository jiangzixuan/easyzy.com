using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace upload.easyzy.com.Models
{
    public class ResponseEntity<T>
    {
        /// <summary>执行结果状态码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 业务状态码
        /// </summary>
        public int BussCode { get; set; }
        /// <summary>执行结果信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>执行结果内容
        /// </summary>
        public T Data { get; set; }

        public ResponseEntity()
        {
            BussCode = (int)ResponseBussCode.Default;
        }
    }
    
}