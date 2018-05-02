using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace m.easyzy.common
{
    public class LogHelper
    {
        private static ILog log = null;
        static LogHelper()
        {
            //如果Web.config配置文件中的key为log4net的value值为1，就表示log4net的配置文件是作为一个单独文件进行配置的，  
            //if (System.Configuration.ConfigurationManager.AppSettings["log4net"].ToString() == "1")
            //{
            //    //那么我就获取log4net的配置文件，并将它加载到我们的项目中去  
            //    string filePath = System.Configuration.ConfigurationManager.AppSettings["log4netPath"].ToString();
            //    System.IO.FileInfo file = new System.IO.FileInfo(filePath);

            //    //Getlogger()静态方法，用来检索框架里是否存在logger对象，如果不存在就创建一个名字为logger的对象  
            //    XmlConfigurator.Configure(file);
            //}
            //else
            //{
            //    XmlConfigurator.Configure();
            //}
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }
        /// <summary>  
        /// 程序运行的过程中的，一般信息可以调用此方法记录日记  
        /// </summary>  
        /// <param name="info"></param>  
        public static void Info(string info)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(info);
            }
        }
        public static void InfoFormat(string info, params object[] args)
        {
            if (log.IsInfoEnabled)
            {
                log.InfoFormat(info, args);
            }
        }
        /// <summary>  
        /// 程序运行的过程中的，一般信息可以调用此方法记录日记  
        /// </summary>  
        /// <param name="info"></param>  
        /// <param name="ex"></param>  
        public static void Info(string info, Exception ex)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(info, ex);
            }
        }
        /// <summary>  
        /// 程序出现错误的时候调用此方法记录日记（一般用在出现了异常以后）  
        /// </summary>  
        /// <param name="info"></param>  
        public static void Error(string info)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(info);
            }
        }
        public static void ErrorFormat(string info, params object[] args)
        {
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat(info, args);
            }
        }
        /// <summary>  
        ///  程序出现错误的时候调用此方法记录日记（一般用在出现了异常以后）  
        /// </summary>  
        /// <param name="info"></param>  
        /// <param name="ex"></param>  
        public static void Error(string info, Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(info, ex);
            }
        }
        /// <summary>  
        /// 程序员觉得任何有利于程序在调试时更详细的了解系统运行状态的信息，比如变量的值等等，都可以调用此方法记录到日记  
        /// </summary>  
        /// <param name="info"></param>  
        public static void Debug(string info)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(info);
            }
        }
        public static void DebugFormat(string info, params object[] args)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat(info, args);
            }
        }
        /// <summary>  
        /// 程序员觉得任何有利于程序在调试时更详细的了解系统运行状态的信息，比如变量的值等等，都可以调用此方法记录到日记  
        /// </summary>  
        /// <param name="info"></param>  
        /// <param name="ex"></param>  
        public static void Debug(string info, Exception ex)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(info, ex);
            }
        }
        /// <summary>  
        /// 程序出现警告时调用此方法记录日记（程序出现警告不会使程序出现异常，但是可能会影响程序性能）  
        /// </summary>  
        /// <param name="info"></param>  
        public static void Warn(string info)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(info);
            }
        }
        public static void WarnFormat(string info, params object[] args)
        {
            if (log.IsWarnEnabled)
            {
                log.WarnFormat(info, args);
            }
        }
        /// <summary>  
        ///  程序出现警告时调用此方法记录日记（程序出现警告不会使程序出现异常，但是可能会影响程序性能）  
        /// </summary>  
        /// <param name="info"></param>  
        /// <param name="ex"></param>  
        public static void Warn(string info, Exception ex)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(info, ex);
            }
        }
        /// <summary>  
        /// 程序出现特别严重的错误，一般是在应用程序崩溃的时候调用此方法记录日记  
        /// </summary>  
        /// <param name="info"></param>  
        public static void Fatal(string info)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(info);
            }
        }
        public static void FatalFormat(string info, params object[] args)
        {
            if (log.IsFatalEnabled)
            {
                log.FatalFormat(info, args);
            }
        }
        /// <summary>  
        /// 程序出现特别严重的错误，一般是在应用程序崩溃的时候调用此方法记录日记  
        /// </summary>  
        /// <param name="info"></param>  
        /// <param name="ex"></param>  
        public static void Fatal(string info, Exception ex)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(info, ex);
            }
        }
    }
}
