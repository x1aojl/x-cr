using System;
using System.Threading;
using Swift;

namespace Server
{
    /// <summary>
    /// 游戏服务器
    /// </summary>
    public class GameServer : Core
    {
        // 服务器逻辑帧间隔（毫秒）
        public int Interval = 50;

        public GameServer()
        {
            // 加入默认功能组件
            Add("CoroutineManager", new CoroutineManager());
        }

        public void Start()
        {
            running = true;
            long t = TimeUtils.Now;

            while (running)
            {
                long now = TimeUtils.Now;
                int dt = (int)(now - t);

                RunOneFrame(dt);
                t = now;

                // sleep according to interval
                int sleepTime = Interval > dt ? Interval - dt : 0;
                Thread.Sleep(sleepTime);
            }

            Console.WriteLine("GameServer stopped.");
        }

        // 停止服务器
        public void Stop()
        {
            Close();
            running = false;
        }

        #region 保护部分

        // 运行中标记
        bool running = false;

        #endregion
    }
}
