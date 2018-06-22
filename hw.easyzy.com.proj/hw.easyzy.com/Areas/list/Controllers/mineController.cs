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
    /// 我建的作业
    /// </summary>
    public class mineController : baseController
    {
        /// <summary>
        /// 获取我新建的作业列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult index(int pageIndex, int pageSize)
        {

            int totalCount = 0;
            List<dto_Zy> list = null;
            if (UserId != 0)
            {
                list = B_Zy.GetZyList(UserId, pageIndex, pageSize, out totalCount);
                if (list != null)
                {
                    List<dto_RelateGroup> rgl = B_User.GetGroupedRelatedUser(UserId);

                    foreach (var l in list)
                    {
                        string subName = "";
                        Const.Subjects.TryGetValue(l.SubjectId, out subName);
                        l.SubjectName = subName;
                        l.TypeName = l.Type == 0 ? "题库" : "自传";

                        List<int> subl = B_Answer.GetZySubmitStudents(l.Id);
                        subl.ForEach(a =>
                        {
                            dto_User u = B_UserRedis.GetUser(a);
                            var r = rgl.FirstOrDefault(b => b.GradeId == u.GradeId && b.ClassId == u.ClassId);
                            if (r != null) r.SubmitCount += 1;
                        });

                        l.ClassSubmitInfo = rgl;
                    }
                }
            }
            ViewBag.ZyList = list;
            ViewBag.PageCount = Util.GetTotalPageCount(totalCount, pageSize);
            return PartialView();
        }
    }
}