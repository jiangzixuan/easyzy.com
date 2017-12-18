using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;
using userthrift.itf;

namespace userthrift.client
{
    class Program
    {
        static void Main(string[] args)
        {
            try

            {

                //设置服务端端口号和地址

                TTransport transport = new TSocket("localhost", 8800);

                transport.Open();

                //设置传输协议为二进制传输协议

                TProtocol protocol = new TBinaryProtocol(transport);

                //创建客户端对象

                UserService.Client client = new UserService.Client(protocol);



                //调用服务端的方法

                Console.WriteLine(client.GetUserName(11));

                Console.WriteLine(client.GetUserInfo(11));
                var c = client.GetUserInfo(11);
                Console.WriteLine(c.UserId);
                Console.WriteLine(c.UserName);
                Console.WriteLine(c.TrueName);
                Console.ReadKey();

            }

            catch (TTransportException e)
            {

                Console.WriteLine(e.Message);

            }
        }
    }
}
