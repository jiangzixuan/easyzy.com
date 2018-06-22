using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list.Controllers
{
    public class homeController : baseController
    {
        public ActionResult Index()
        {
            ViewBag.UserId = UserId;
            return View();
        }

        

        private List<dto_RelateGroup> GetGroupedRelatedUser()
        {
            //查询关注自己的人的各班级数量
            List<dto_RelateGroup> ul = B_User.GetGroupedRelatedUser(UserId);
            if (ul != null)
            {
                string GName = "";
                ul.ForEach(a => {
                    Const.Grades.TryGetValue(a.GradeId, out GName);
                    a.GradeName = GName;
                    a.ClassName = a.ClassId + "班";
                });
            }
            return ul;
        }

        
        
    }
}