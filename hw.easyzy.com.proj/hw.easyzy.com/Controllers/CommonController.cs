using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.common;
using hw.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Controllers
{
    /// <summary>
    /// 公共功能
    /// </summary>
    public class CommonController : Controller
    {   
        public string SaveSuggest(string content, string name, string phone)
        {
            T_Suggestion t = new T_Suggestion() { Name = name, Phone = phone, Content = content, CreateDate = DateTime.Now, Processed = false };
            int i = B_Common.AddSuggestion(t);
            return i > 0 ? "" : "Error";
        }

        public void Exit()
        {
            try
            {
                Util.ClearCookies(Const.CookieName_User);
            }
            catch
            { }
        }
    }
}