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
    /// 试用作业
    /// </summary>
    public class trailController : baseController
    {
        /// <summary>
        /// 获取试用的作业列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult index(int pageIndex, int pageSize)
        {
            int totalCount = 0;
            List<dto_Zy> list = B_Zy.GetZyList(0, pageIndex, pageSize, out totalCount);
            if (list != null)
            {
                List<dto_RelateGroup> rgl = null;
                Dictionary<int, int> d = B_TrailAnswer.GetZySubmitStudentCount(list.Select(a => a.Id).ToArray());

                foreach (var l in list)
                {
                    //隐藏真实Id
                    l.NewId = IdNamingHelper.Encrypt(IdNamingHelper.IdTypeEnum.Zy, l.Id);

                    string subName = "";
                    Const.Subjects.TryGetValue(l.SubjectId, out subName);
                    l.SubjectName = subName;
                    l.TypeName = l.Type == 0 ? "题库" : "自传";
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