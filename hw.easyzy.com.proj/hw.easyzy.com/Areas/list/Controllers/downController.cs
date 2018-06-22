using easyzy.sdk;
using hw.easyzy.bll;
using hw.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list.Controllers
{
    /// <summary>
    /// 已完成作业
    /// </summary>
    [LoginFilterAttribute]
    public class downController : baseController
    {
        public ActionResult Index(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Zy> list = null;
            List<int> ids = B_Answer.GetSubmitedZyIds(UserId, pageIndex, pageSize, out totalCount);
            if (ids != null)
            {
                list = new List<dto_Zy>();
                ids.ForEach(a =>
                {
                    list.Add(B_ZyRedis.GetZy(a));
                });
            }
            if (list != null)
            {
                List<dto_RelateGroup> rgl = null;
                Dictionary<int, int> d = B_Answer.GetZySubmitStudentCount(ids.ToArray());

                foreach (var l in list)
                {
                    //隐藏真实Id
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Zy, l.Id);
                    
                    int c = 0;
                    if (d != null)
                    {
                        d.TryGetValue(l.Id, out c);
                    }
                    rgl = new List<dto_RelateGroup>();
                    rgl.Add(new dto_RelateGroup() { ClassId = 0, ClassName = "试用班", GradeId = 0, GradeName = "", SubmitCount = c, TotalCount = 0 });
                    l.ClassSubmitInfo = rgl;
                    l.Id = 0;
                }
            }

            ViewBag.ZyList = list;
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }
    }
}