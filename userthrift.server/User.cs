using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userthrift.itf;

namespace userthrift.server
{
    public class User : UserService.Iface
    {
        public int Add(int userId, string userName)
        {
            return 123;
        }

        public string GetUser(int userId)
        {
            return "name of user:" + userId;
        }

        public bool Login(string username, string psd)
        {
            throw new NotImplementedException();
        }
    }
}
