using easyzy.bll;
using easyzy.common;
using easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace self.easyzy.com.Controllers
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
                Util.ClearCookies(EasyzyConst.CookieName_User);
            }
            catch
            { }
        }
    }
}