using bbs.easyzy.model.ques;
using easyzy.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbs.easyzy.bll
{
    public class B_QuesRedis
    {
        public static dto_Question GetQuestion(int courseId, int qId)
        {
            dto_Question tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(Const.CacheCatalog.Ques, qId.ToString());
            using (var client = RedisHelper.Instance.GetRedisClient(Const.CacheCatalog.Ques.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<dto_Question>(key);
                }
            }

            return tempresult;
        }
    }
}
