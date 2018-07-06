using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw.easyzy.common
{
    public class SolrHelper
    {
        private static readonly string SolrCoreUrl = "http://47.94.100.66:8080/solr/ques/";

        static SolrHelper()
        {
            //主程序调用前先初始化
            Startup.Init<dto_SolrQues>(SolrCoreUrl);
        }

        public static int[] QueryQuesIds(int courseId, int kpointId, int[] cpointIds, int typeId, int diffType, int paperYear, int pageIndex, int pageSize, out int totalCount)
        {
            List<int> list = null;
            ISolrOperations<dto_SolrQues> solr = ServiceLocator.Current.GetInstance<ISolrOperations<dto_SolrQues>>();

            //SolrMultipleCriteriaQuery sq = GetSolrQueryByField(courseId, kpointId, cpointIds, typeId, diffType, paperYear);
            string sq = GetSolrQueryByString(courseId, kpointId, cpointIds, typeId, diffType, paperYear);
            QueryOptions options = GetOptions(pageIndex, pageSize);
            SolrQueryResults<dto_SolrQues> solrResults = solr.Query(sq, options);

            //总数
            totalCount = solrResults.NumFound;

            if (solrResults.Count != 0)
            {
                list = new List<int>();
                foreach (var solrQueryResult in solrResults)
                {
                    list.Add(solrQueryResult.id);
                }
            }
            
            return list == null ? null : list.ToArray();
        }

        public static SolrMultipleCriteriaQuery GetSolrQueryByField(int courseId, int kpointId, int[] cpointIds, int typeId, int diffType, int paperYear)
        {
            IList<ISolrQuery> filter = new List<ISolrQuery>();
            filter.Add(new SolrQueryByField("courseid", courseId.ToString()));

            if (typeId != 0)
            {
                filter.Add(new SolrQueryByField("btypeid", typeId.ToString()));
            }

            if (diffType != 0)
            {
                filter.Add(new SolrQueryByField("difftype", diffType.ToString()));
            }

            if (paperYear > 0)
            {
                filter.Add(new SolrQueryByField("paperyear", paperYear.ToString()));
            }
            else if (paperYear < 0)
            {
                filter.Add(new SolrQueryByRange<int>("paperyear", 0, paperYear * -1));
            }

            if (kpointId != 0)
            {
                filter.Add(new SolrQueryByField("kpoints", string.Concat("*", kpointId, "*")));
            }

            if (cpointIds != null && cpointIds.Length > 0)
            {
                if (cpointIds.Length == 1)
                {
                    filter.Add(new SolrQueryByField("cpoints", string.Concat("*", cpointIds[0], "*")));
                }
                else
                {
                    IList<ISolrQuery> filter2 = new List<ISolrQuery>();
                    foreach (var c in cpointIds)
                    {
                        filter2.Add(new SolrQueryByField("cpoints", string.Concat("*", c, "*")));
                    }
                    SolrMultipleCriteriaQuery t = new SolrMultipleCriteriaQuery(filter2, SolrMultipleCriteriaQuery.Operator.OR);
                    filter.Add(t);
                }
            }

            SolrMultipleCriteriaQuery s = new SolrMultipleCriteriaQuery(filter, SolrMultipleCriteriaQuery.Operator.AND);
            return s;

        }

        public static string GetSolrQueryByString(int courseId, int kpointId, int[] cpointIds, int typeId, int diffType, int paperYear)
        {
            string qs = string.Format("courseid: {0}", courseId);

            if (typeId != 0)
            {
                qs += string.Format(" AND btypeid: {0}", typeId);
            }

            if (diffType != 0)
            {
                qs += string.Format(" AND difftype: {0}", diffType);
            }

            if (paperYear > 0)
            {
                qs += string.Format(" AND paperyear: {0}", paperYear);
            }
            else if (paperYear < 0)
            {
                qs += string.Format(" AND paperyear: [* TO {0}]", paperYear * -1);
            }

            if (kpointId != 0)
            {
                qs += string.Format(" AND kpoints: *{0}*", kpointId);
            }

            if (cpointIds.Length > 0)
            {
                if (cpointIds.Length == 1)
                {
                    qs += string.Format(" AND cpoints: *{0}*", cpointIds[0]);
                }
                else
                {
                    qs += " AND (";
                    string s = "";
                    foreach (var c in cpointIds)
                    {
                        s += string.Format(" OR cpoints: *{0}*", c);
                    }
                    qs += s.Substring(3) + ")";
                }
            }

            return qs;
        }

        public static QueryOptions GetOptions(int pageIndex, int pageSize)
        {
            QueryOptions options = new QueryOptions();
            options.Rows = pageSize;//结果获取个数

            int begin = (pageIndex - 1) * pageSize;
            if (begin < 0) begin = 0;
            
            options.Start = begin;//取结果的开始位置 ，用于分页
            options.OrderBy = new Collection<SortOrder> { new SortOrder("usagetimes", Order.DESC) };

            return options;
        }
    }
}
