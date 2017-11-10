using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace easyzy.common
{
    public class FileHelper
    {
        /// <summary>
        /// 保存文件，返回相对路径
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="path">保存的路径</param>
        /// <param name="uniqName">文件名称</param>
        public static string SaveFile(HttpPostedFile file, string path, string saveName)
        {
            
            string FileName = file.FileName;
            string Pattern = FileName.Substring(FileName.LastIndexOf(".") + 1);
            string shortTime = DateTime.Now.ToShortDateString().Replace("/", "-");
            string filePath = (path.EndsWith("/") ? path : (path + "/")) + shortTime + "/" + saveName + "." + Pattern;
            string fileToUpload = AppDomain.CurrentDomain.BaseDirectory + (path.EndsWith("/")?path:(path+"/")) + shortTime + "/" + saveName + "." + Pattern;
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            file.SaveAs(fileToUpload);
            return filePath;
        }

        /// <summary>
        /// Word转Html，返回Html路径
        /// </summary>
        /// <returns></returns>
        public static bool Word2Html(string wordPath, string htmlPath)
        {
            try
            {
                if (!File.Exists(wordPath))
                {
                    return false;
                }
                string path = htmlPath.Substring(0, htmlPath.LastIndexOf("\\"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Aspose.Words.Document awd = new Aspose.Words.Document(wordPath);
                awd.Save(htmlPath, Aspose.Words.SaveFormat.Html);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.ErrorFormat("Word2Html word文档转化HTML失败，原因是：" + ex.Message);
                
            }
            return false;
        }
    }
}
