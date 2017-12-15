using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;
using userthrift.itf;
using userthrift.itf.model;

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
            //userthrift.itf.model.User user = new userthrift.itf.model.User();
            //user.UserId = 100;
            //user.UserName = "test";
            //user.TrueName = "哈哈哈";

            //// 序列化  
            //ByteArrayOutputStream out = new ByteArrayOutputStream();
            //TTransport transport = new (out);
            //TBinaryProtocol tp = new TBinaryProtocol(transport);//二进制编码格式进行数据传输  
            //                                                    // TCompactProtocol tp = new TCompactProtocol (transport); 

            //try
            //{
            //    user.Write(tp);
            //}
            //catch (TException e)
            //{

            //}
            //byte[] buf = out.toByteArray();
            //return buf.ToString();
            return "name of user:" + userId;
        }

        public bool Login(string username, string psd)
        {
            throw new NotImplementedException();
        }
    }
}
