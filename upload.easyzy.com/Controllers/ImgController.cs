using easyzy.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using upload.easyzy.com.Models;

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
            ResponseEntity<List<string>> result = new ResponseEntity<List<string>>();
            List<string> imgPathList = null;
            string filePath = "";
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                imgPathList = new List<string>();
                int imgFunc = Convert.ToInt32(HttpContext.Current.Request.Form["func"]);
                foreach (HttpPostedFile file in files)
                {
                    try
                    {
                        string FileName = file.FileName;
                        string Pattern = FileName.Substring(FileName.LastIndexOf(".") + 1);
                        if (!EasyzyConst.ImgPattern.Contains(Pattern))
                        {
                            imgPathList.Add("上传文件类型错误");
                            continue;
                        }
                        //根据word类型决定存放word的相对路径
                        string path = "";
                        
                        path = "\\word\\" + imgFunc;
                        string saveName = UniqueObjectID.GenerateStrNewId();
                        filePath = FileHelper.SaveFile(file, path, saveName);

                        imgPathList.Add(filePath.Replace("\\", "/"));
                    }
                    catch (Exception ex)
                    {
                        imgPathList.Add(ex.Message);
                    }
                }

                result.Code = (int)ResponseCode.Success;
                result.BussCode = (int)ResponseBussCode.Success;
                result.Data = imgPathList;
                result.Message = "";
            }
            else
            {
                result.Code = (int)ResponseCode.DataNotExist;
                result.BussCode = (int)ResponseBussCode.Error;
                result.Data = null;
                result.Message = "找不到上传文件";
            }
            return result;
        }
    }
}