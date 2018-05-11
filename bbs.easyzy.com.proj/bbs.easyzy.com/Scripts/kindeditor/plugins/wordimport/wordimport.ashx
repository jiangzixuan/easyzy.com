<%@ WebHandler Language="C#" Class="wordimport" %>

using System;
using LitJson;
using System.IO;
using System.Web;
using System.Text;
using System.Collections;
using System.Globalization;

public class wordimport : IHttpHandler
{
    #region // Word上传转化及处理

    public void ProcessRequest(HttpContext context)
    {
        string postType = context.Request.Params["postType"] != null ? context.Request.Params["postType"] : "";
        if (postType == "WordImport")
        {
            HttpPostedFile wordFile = context.Request.Files["wordFile"];
            if (wordFile != null)
            {
                // 文件保存目录路径
                string savePath = "/kindeditor/attached/wordimport/";
                // 定义允许上传的文件扩展名
                Hashtable extTable = new Hashtable();
                extTable.Add("wordimport", "doc,docx");

                // 最大文件大小[10MB]
                int maxSize = 10485760;

                string fileName = wordFile.FileName;
                string fileExt = Path.GetExtension(fileName).ToLower();

                if (wordFile.InputStream == null || wordFile.InputStream.Length > maxSize)
                {
                    showError(context, "上传文件大小超过限制。");
                }

                string dirName = "wordimport";
                if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(((string)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    showError(context, "上传文件扩展名是不允许的扩展名。\n只允许" + ((string)extTable[dirName]) + "格式。");
                }

                string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                string strDateName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
                string dirPath = context.Server.MapPath(savePath) + ymd + "/" + strDateName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string WordFilePath = dirPath + strDateName + fileExt;
                string SaveHtmlPath = dirPath + strDateName + ".html";
                wordFile.SaveAs(WordFilePath);
                Aspose.Words.Document doc = new Aspose.Words.Document(WordFilePath);
                doc.Save(SaveHtmlPath, Aspose.Words.SaveFormat.Html);
                string strResult = "";
                System.IO.StreamReader reader = null;
                try
                {
                    // 打开文件
                    reader = new System.IO.StreamReader(dirPath + strDateName + ".html", Encoding.GetEncoding("gb2312"));
                    // 读取流
                    string sHtml = reader.ReadToEnd();
                    // 关闭流
                    reader.Close();
                    string strReplace = "/kindeditor/attached/wordimport/" + ymd + "/" + strDateName + "/" + strDateName;
                    strResult = sHtml.Replace(strDateName, strReplace);
                }
                catch
                {
                    reader.Close();
                }

                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(strResult);
                context.Response.End();
            }
            else
            {
                showError(context, "请选择文件。");
            }
        }
    }

    #endregion

    #region // 反回错误提示

    private void showError(HttpContext context, string message)
    {
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(message);
        context.Response.End();
    }

    #endregion

    #region // 原始自带

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

    #endregion
}