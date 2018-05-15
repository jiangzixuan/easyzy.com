using upload.easyzy.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using upload.easyzy.com.Models;
using easyzy.sdk;

namespace upload.easyzy.com.Controllers
{
    public class ImgController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "getall";
        }
        [HttpGet]
        public string Get(int id)
        {
            return "get_" + id;
        }

        [HttpDelete]
        public void Delete(int id)
        {

        }

        [HttpPut]
        public void Put()
        {

        }

        [HttpPost]
        public ResponseEntity<List<string>> Post()
        {
            LogHelper.Error("asf");
            ResponseEntity<List<string>> result = new ResponseEntity<List<string>>();
            List<string> imgPathList = null;
            string filePath = "";
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                imgPathList = new List<string>();
                int imgFunc = Convert.ToInt32(HttpContext.Current.Request.Form["func"]);
                string msg = "";
                for (int i = 0; i < files.Count; i++)
                {
                    try
                    {
                        HttpPostedFile f = files[i];
                        string FileName = f.FileName;
                        string Pattern = FileName.Substring(FileName.LastIndexOf(".") + 1).ToLower();
                        
                        if (!Const.ImgPattern.Contains(Pattern))
                        {
                            imgPathList.Add("上传文件类型错误");
                            msg = "有文件上传失败！";
                            continue;
                        }
                        //根据word类型决定存放word的相对路径
                        string path = "";

                        path = "\\img\\" + imgFunc;
                        string saveName = UniqueObjectID.GenerateStrNewId();
                        filePath = FileHelper.SaveFile(f, path, saveName);

                        imgPathList.Add(filePath.Replace("\\", "/"));
                    }
                    catch (Exception ex)
                    {
                        imgPathList.Add(ex.Message);
                        msg = "有文件上传失败！";
                    }
                }

                result.Code = (int)ResponseCode.Success;
                result.BussCode = (int)ResponseBussCode.Success;
                result.Data = imgPathList;
                result.Message = msg;
            }
            else
            {
                result.Code = (int)ResponseCode.DataNotExist;
                result.BussCode = (int)ResponseBussCode.Error;
                result.Data = null;
                result.Message = "找不到任何上传的文件";
            }
            return result;
        }
    }
}