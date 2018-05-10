using ques.thrift.itf;
using System;
using System.Collections.Generic;
using Thrift.Protocol;
using Thrift.Transport;

namespace easyzy.sdk
{
    public class QuesHelper
    {
        static QuesService.Client client = null;
        static QuesHelper()
        {
            if (client == null)
            {
                string ServerIP = System.Configuration.ConfigurationManager.AppSettings["ThriftServerIp"];
                int ServerPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThriftServerPort"]);
                //设置服务端端口号和地址
                TTransport transport = new TSocket(ServerIP, ServerPort);

                transport.Open();

                //设置传输协议为二进制传输协议
                TProtocol protocol = new TBinaryProtocol(transport);
                client = new QuesService.Client(protocol);
            }
        }

        //调用服务端的方法
        public static List<Question> QueryQuestions()
        {
            return client.QueryQuestions(1, 1, 1,1,1,1,1,1);
        }
    }
}
