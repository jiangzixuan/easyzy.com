using ques.thrift.itf;
using System;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace ques.thrift.server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int ServerPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThriftServerPort"]);
                //设置服务端口
                TServerSocket serverTransport = new TServerSocket(ServerPort);
                
                //设置传输协议工厂
                TBinaryProtocol.Factory factory = new TBinaryProtocol.Factory();
                
                //关联处理器与服务的实现
                TProcessor processor = new QuesService.Processor(new Question());
                
                //创建服务端对象
                TServer server = new TThreadPoolServer(processor, serverTransport, new TTransportFactory(), factory);

                Console.WriteLine("试题服务端正在监听" + ServerPort + "端口");
                server.Serve();

                Console.ReadLine();
            }
            catch (TTransportException ex)
            {
                //打印异常信息
                Console.WriteLine(ex.Message);
            }
        }
    }
}
