using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;
using userthrift.itf;

namespace userthrift.server
{
    public class User : UserService.Iface
    {
        public int Add(int userId, string userName)
        {
            return 123;
        }
        

        public itf.User GetUserInfo(int userId)
        {
            itf.User user = new itf.User();
            user.UserId = 100;
            user.UserName = "test";
            user.TrueName = "哈哈哈";
            return user;
        }

        public string GetUserName(int userId)
        {
            return "name of user:" + userId;
        }

        public bool Login(string username, string psd)
        {
            throw new NotImplementedException();
        }
    }
}
