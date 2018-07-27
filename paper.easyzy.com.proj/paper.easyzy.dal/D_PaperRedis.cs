using easyzy.sdk;
using paper.easyzy.model.dto;
using paper.easyzy.model.entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static easyzy.sdk.Const;

namespace paper.easyzy.dal
{
    public class D_PaperRedis
    {
        //缓存有效期(50天）
        private static TimeSpan ts = new TimeSpan(500, 0, 0, 0);

        public static dto_Paper GetPaper(int paperId)
        {
            dto_Paper tempresult = null;
            string key = RedisHelper.GetEasyZyRedisKey(CacheCatalog.Paper, paperId.ToString());
            using (var client = RedisHelper.Instance.GetRedisClient(CacheCatalog.Paper.ToString()))
            {
                if (client != null)
                {
                    tempresult = client.Get<dto_Paper>(key);
                    if (tempresult == null)
                    {
                        tempresult = D_Paper.GetPaper(paperId);
                        if (tempresult != null)
                        {
                            client.Set(key, tempresult, ts);
                        }
                    }
                }
            }

            return tempresult;
        }
    }
}
