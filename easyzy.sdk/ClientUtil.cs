using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace easyzy.sdk
{
    /// <summary>
    /// 跟客户端相关的公共方法
    /// </summary>
    public class ClientUtil
    {
        public static string Ip
        {
            get { return GetUserIP(); }
            
        }

        public static string IMEI
        {
            get { return ""; }
            
        }

        public static string MobileBrand
        {
            get { return ""; }
            
        }

        

        #region 私有方法

        /// <summary>
        /// 获取公网IP
        /// 通过内网访问，获取的是内网地址
        /// 通过外网访问，获取外网地址，无法获取到内网地址
        /// 要想外网访问可以获取内网IP/Mac地址，除非用户使用IE浏览器同时开启ActiveX
        /// </summary>
        /// <returns></returns>
        private static string GetUserIP()
        {
            string User_IP;
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    User_IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    User_IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取用户ID异常：" + ex.Message);
                return "0.0.0.0";
            }
            return User_IP;
        }
        #endregion

    }
}
