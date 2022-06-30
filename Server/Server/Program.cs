using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = 9001;
            var noConsole = false;
            foreach (var p in args)
            {
                if (p == "-noconsole")
                    noConsole = true;
                else if (p.StartsWith("-p"))
                    port = int.Parse(p.Substring(2));
                else
                    Console.WriteLine("unknown parameter: " + p);
            }

            var srv = new GameServer();
            var start = ServerBuilder.BuildLibServer(srv, port);
            if (!noConsole)
                srv.Get<ConsoleInput>().Start(); // 启用控制台输入

            // 启动服务器
            start();
        }
    }
}
