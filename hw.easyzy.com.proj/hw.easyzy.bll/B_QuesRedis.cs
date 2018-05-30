using easyzy.sdk;
using hw.easyzy.model.entity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static easyzy.sdk.Const;

namespace hw.easyzy.bll
{
    public class B_QuesRedis
    {
        //缓存有效期(500天）
        private static TimeSpan ts = new TimeSpan(500, 0, 0, 0);
        
    }
}
