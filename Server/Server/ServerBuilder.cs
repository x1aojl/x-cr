using System;
using Swift;
using SpaceX;

namespace Server
{
    /// <summary>
    /// 构建服务对象
    /// </summary>
    public static class ServerBuilder
    {
        // 创建实验室用的服务器，返回启动操作
        public static Action BuildLibServer(GameServer gs, int port)
        {
            srv = gs;

            var ci = BC<ConsoleInput>();
            ci.Srv = gs;

            BuildBaseComponents(); // 基础模块
            BuildLogicComponents(gs); // 逻辑功能
            BuildGMComponent(); // GM 和调试功能

            // 初始化所有模块
            srv.Initialize();

            return () =>
            {
                // 启动服务器
                srv.Get<NetCore>().StartListening("0.0.0.0", port);
                Console.WriteLine("GameServer started at: " + port);
                srv.Start();
            };
        }

        // 创建中的服务器对象
        static GameServer srv = null;
        
        // 默认方式创建给定模块
        static T BC<T>() where T : Component, new()
        {
            var c = new T();
            srv.Add(typeof(T).Name, c);
            return c;
        }

        // 通用基础模块
        public static void BuildBaseComponents()
        {
            BC<NetCore>(); // 网络
            BC<UserPort>(); // 消息端口
            UserConnectionExt.ClientMessageHandler = "ServerPort";
        }

        // 游戏逻辑
        public static void BuildLogicComponents(GameServer gs)
        {
            BC<SessionContainer>(); // 会话容器
            BC<LoginManager>(); // 登录

            var uc = new UserContainer(new MySqlDbPersistence<User, string>(
                "mj", "127.0.0.1", "root", "123456",
                @"Users", "CREATE TABLE Users(ID VARCHAR(100) BINARY, Data MediumBlob, PRIMARY KEY(ID ASC));", 
                null, (usr) =>
                {
                    var buff = new WriteBuffer();
                    usr.Serialize(buff);
                    return buff.Data;
                }, (data) =>
                {
                    var rb = new RingBuffer(data);
                    var usr = new User();
                    usr.Deserialize(rb);
                    return usr;
                }, null));
            srv.Add("UserContainer", uc);

            MountAIAPIs(gs);

            Logger.Instance = new SimpleLogger(
                (msg) => { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine(msg); },
                (msg) => { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(msg); }
            );
        }

        static void MountAIAPIs(GameServer gs)
        {
        }

        public static void BuildGMComponent()
        {
            BC<GMInLab>().Init();
            BC<CheatCode>().Init();
        }
    }
}
