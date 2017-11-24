using easyzy.bll;
using easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyzy.com.Controllers
{
    public class CommonController : Controller
    {
        public string SaveSuggest(string content, string name, string phone)
        {
            T_Suggestion t = new T_Suggestion() { Name = name, Phone = phone, Content = content, CreateDate = DateTime.Now, Processed = false };
            int i = B_Common.AddSuggestion(t);
            return i > 0 ? "" : "Error";
        }
    }
}