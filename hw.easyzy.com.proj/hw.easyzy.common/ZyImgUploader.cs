using easyzy.sdk;
using hw.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace hw.easyzy.common
{
    public class ZyImgUploader
    {
        public static dto_AjaxJsonResult<string> Upload(HttpFileCollection files)
        {
            dto_AjaxJsonResult<string> r = new dto_AjaxJsonResult<string>();
            
            if (files.Count == 0)
            {
                r.code = AjaxResultCodeEnum.Error;
                r.message = "未找到任何上传的图片！";
                r.data = "";
                return r;
            }

            string url = "";
            if (files.Count > 0)
            {
                HttpPostedFile file = files[0];
                string result = FileUploader.UploadToServer(file, Const.ImgFunc.SubmitAnswer);
                var resp = Newtonsoft.Json.JsonConvert.DeserializeObject<dto_UploadResponseEntity<List<string>>>(result);
                if (resp.Code != (int)dto_UploadResponseCode.Success)
                {
                    r.code = AjaxResultCodeEnum.Error;
                    r.message = resp.Message;
                    r.data = "";
                    return r;
                }
                url = resp.Data[0];
            }

            r.code = AjaxResultCodeEnum.Success;
            r.message = "";
            r.data = url;
            return r;
        }
    }
}
