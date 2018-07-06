using hw.easyzy.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.bll
{
    public class B_QuesSolr
    {
        /// <summary>
        /// 根据查询条件返回符合条件的试题Id数组
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="kpointId"></param>
        /// <param name="cpointId"></param>
        /// <param name="typeId"></param>
        /// <param name="diffType"></param>
        /// <param name="paperYear"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static int[] GetQuesIds(int courseId, int kpointId, int cpointId, int typeId, int diffType, int paperYear, int pageIndex, int pageSize, out int totalCount)
        {
            int[] cpointIds = null;
            if (cpointId != 0)
            {
                int[] s = B_QuesBase.GetSimilarCatalogs(courseId, cpointId);
                if (s == null)
                {
                    cpointIds = new int[1] { cpointId };
                }
                else
                {
                    cpointIds = s;
                }
            }

            return SolrHelper.QueryQuesIds(courseId, kpointId, cpointIds, typeId, diffType, paperYear, pageIndex, pageSize, out totalCount);
        }
    }
}
