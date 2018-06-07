using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace easyzy.sdk
{
    public class Util
    {
        public static string GetConnectString(string Name)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[Name].ToString();
        }

        public static string GetAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        public static string ToChinese(int num)
        {
            if (num > 20) return "";
            string[] array = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十" };
            return array[num - 1];
        }

        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetTotalPageCount(int totalCount, int pageSize)
        {
            return (totalCount + pageSize - 1) / pageSize;
        }

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string MD5(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strText);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0');
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime LongDateTimeToDateTime(long timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(timestamp);
        }

        /// <summary>
        ///当前时间转换为Unix时间戳格式
        /// </summary>
        /// <returns></returns>
        public static Int64 GetTimeStamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(DateTime.UtcNow - startTime).TotalSeconds;
        }

        #region Cookie Get/Set/Clear

        /// <summary>
        /// 添加Cookies
        /// </summary>
        /// <param name="valuename"></param>
        /// <param name="value"></param>
        /// <param name="expiry">如果不想设置过期时间，可传递DateTime.MinValue</param>
        public static void SetCookie(string cookieName, string valueName, string value, DateTime expiry)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Values[valueName] = value;
                if (expiry != DateTime.MinValue)
                {
                    cookie.Expires = expiry;
                }
                cookie.Path = "/";
                cookie.Domain = "easyzy.com";
                HttpContext.Current.Response.AppendCookie(cookie);
            }
            else
            {
                cookie = new HttpCookie(cookieName);
                cookie.Values.Add(valueName, value);
                if (expiry != DateTime.MinValue)
                {
                    cookie.Expires = expiry;
                }
                cookie.Path = "/";
                cookie.Domain = "easyzy.com";
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void ClearCookie(string cookieName, string valueName)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Add(valueName, "");
            cookie.Expires = DateTime.Now.AddMonths(-1);
            cookie.Path = "/";
            cookie.Domain = "easyzy.com";
            HttpContext.Current.Response.Cookies.Add(cookie);

        }

        public static void ClearCookies(string cookieName)
        {
            HttpCookie Cookie = new HttpCookie(cookieName);
            Cookie.Expires = DateTime.Now.AddDays(-1);
            Cookie.Path = "/";
            Cookie.Domain = "easyzy.com";
            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        /// <summary>
        /// 返回cook的值
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="valuename"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string valueName)
        {
            string value = "";

            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                value = HttpContext.Current.Request.Cookies[cookieName].Values[valueName].ToString();
            }
            return value;
        }

        #endregion

        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="s"></param>
        /// <param name="Qty"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Stream ImageCompress(Stream s, int Qty, ref int Width, ref int Height)
        {
            MemoryStream ms = new MemoryStream();
            Image image = null;
            Bitmap bmp = null;
            try
            {
                image = Image.FromStream(s);
                bmp = new Bitmap(image);
                bmp.Save(ms, ImageFormat.Jpeg);
                ms.Position = 0;
                Width = image.Width;
                Height = image.Height;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ImageCompress 图片压缩错误，原因是：" + ex.Message);
            }
            finally
            {
                //p.Dispose();
                //ps.Dispose();
                image.Dispose();
                bmp.Dispose();
            }
            return ms;
        }

        /// <summary>
        /// 获取文件的Hash
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileHash(Stream file)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
