using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.model.dto
{
    public class dto_AjaxJsonResult<T>
    {
        public AjaxResultCodeEnum code { get; set; }

        public string message { get; set; }

        public T data { get; set; }
    }

    public enum AjaxResultCodeEnum
    {
        Success,
        Error
    }
}
