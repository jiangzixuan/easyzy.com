using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using hw.easyzy.model.entity;
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
            return View();
        }

        public ActionResult GetMyZy(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Zy> list = B_Zy.GetMyZy(UserId, pageIndex, pageSize, out totalCount);
            if (list != null)
            {
                List<dto_RelateGroup> rgl = B_User.GetGroupedRelatedUser(UserId);
                
                foreach (var l in list)
                {
                    string subName = "";
                    Const.Subjects.TryGetValue(l.SubjectId, out subName);
                    l.SubjectName = subName;
                    l.TypeName = l.Type == 0 ? "题库" : "自传";
                    if (rgl != null)
                    {
                        rgl.ForEach(b =>
                        {
                            b.SubmitCount = 0;
                        });
                    }
                    Dictionary<int, int> subl = B_Zy.GetZySubmitCountByClass(l.Id);
                    rgl.ForEach(a =>
                    {
                        int c = 0;
                        string s = string.Concat(a.GradeId, a.ClassId.ToString().PadLeft(2, '0'));
                        subl.TryGetValue(int.Parse(s), out c);
                        a.SubmitCount = c;
                    });
                    l.ClassSubmitInfo = rgl;
                }
            }
            ViewBag.ZyList = list;
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
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