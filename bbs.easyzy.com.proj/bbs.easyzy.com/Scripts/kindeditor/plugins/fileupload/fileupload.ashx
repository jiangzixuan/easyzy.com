<%@ WebHandler Language="C#" Class="fileupload" %>

using System;
using System.IO;
using System.Web;
using System.Globalization;

public class fileupload : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/plain";
        HttpPostedFile file = context.Request.Files["Filedata"];
        if (file != null)
        {
            string strPath = "";
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            string strDateName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
            if (fileExt == ".flv")
            {
                strPath = "/kindeditor/attached/flv/" + ymd + "/";
            }
            else if (fileExt == ".mp4" || fileExt == ".wmv" || fileExt == ".swf")
            {
                strPath = "/kindeditor/attached/flash/" + ymd + "/";
            }
            else
            {
                strPath = "/kindeditor/attached/file/" + ymd + "/";
            }
            string dirPath = context.Server.MapPath(strPath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            file.SaveAs(dirPath + strDateName + fileExt);
            context.Response.Write(strPath + strDateName + fileExt);
        }
        else
        {
            context.Response.Write("0");
        }
    }

    #region // 原始自带

    public bool IsReusable
    {
        get { return true; }
    }

    #endregion
}