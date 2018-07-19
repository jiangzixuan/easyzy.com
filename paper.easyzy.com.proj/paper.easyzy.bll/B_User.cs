using paper.easyzy.dal;
using paper.easyzy.model.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paper.easyzy.bll
{
    public class B_User
    {
        private int _uid = 0;

        public B_User(int userId)
        {
            _uid = userId;
        }
        public dto_User GetUser()
        {
            return D_UserRedis.GetUser(_uid);
        }
    }
}
