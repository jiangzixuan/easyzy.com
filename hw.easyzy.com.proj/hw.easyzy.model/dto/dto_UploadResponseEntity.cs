using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hw.easyzy.model.dto
{
    public class dto_UploadResponseEntity<T>
    {

        public int Code { get; set; }

        public int BussCode { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public dto_UploadResponseEntity()
        {
            BussCode = (int)dto_UploadResponseBussCode.Default;
        }
    }
    
}