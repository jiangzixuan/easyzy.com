using upload.easyzy.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using upload.easyzy.com.Models;

namespace upload.easyzy.com.Controllers
{
    public class WordController : ApiController
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
        public ResponseEntity<dtoWordUploadPath> Post()
        {
            ResponseEntity<dtoWordUploadPath> result = new ResponseEntity<dtoWordUploadPath>();
            dtoWordUploadPath dto = new dtoWordUploadPath();
            string filePath = "";
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                try
                {
                    HttpPostedFile file = files[0];
                    string FileName = file.FileName;
                    string Pattern = FileName.Substring(FileName.LastIndexOf(".") + 1);
                    if (Pattern != "doc" && Pattern != "docx")
                    {
                        result.Code = (int)ResponseCode.ParamsError;
                        result.BussCode = (int)ResponseBussCode.Error;
                        result.Data = null;
                        result.Message = "上传文件类型必须是doc/docx";
                        return result;
                    }
                    //根据word类型决定存放word的相对路径
                    string path = "";
                    int wordFunc = Convert.ToInt32(HttpContext.Current.Request.Form["func"]);
                    int wordCategory = Convert.ToInt32(HttpContext.Current.Request.Form["category"]);
                    path = "\\word\\" + wordFunc + "\\" + wordCategory;
                    string saveName = UniqueObjectID.GenerateStrNewId();
                    filePath = FileHelper.SaveFile(file, path, saveName);
                    if (wordFunc == (int)EasyzyConst.WordFunc.CreateZY)
                    {
                        //Word转Html
                        string htmlPath = filePath.Replace("word", "html").Replace("docx", "html").Replace("doc", "html");
                        bool b = FileHelper.Word2Html(filePath, htmlPath);
                        if (b)
                        {
                            dto.WordPath = filePath.Replace("\\", "/");
                            dto.HtmlPath = htmlPath.Replace("\\", "/");
                            result.Code = (int)ResponseCode.Success;
                            result.BussCode = (int)ResponseBussCode.Success;
                            result.Data = dto;
                            result.Message = "";
                        }
                        else
                        {
                            dto.WordPath = filePath.Replace("\\", "/");
                            dto.HtmlPath = "";
                            result.Code = (int)ResponseCode.Error;
                            result.BussCode = (int)ResponseBussCode.Error;
                            result.Data = dto;
                            result.Message = "word转html失败！";
                        }
                    }
                    else
                    {
                        dto.WordPath = filePath.Replace("\\", "/");
                        dto.HtmlPath = "";
                        result.Code = (int)ResponseCode.Success;
                        result.BussCode = (int)ResponseBussCode.Success;
                        result.Data = dto;
                        result.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    result.Code = (int)ResponseCode.Error;
                    result.BussCode = (int)ResponseBussCode.Error;
                    result.Data = null;
                    result.Message = ex.Message;
                    LogHelper.Error("上传Word报错，原因是：" + ex.Message);
                }
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
