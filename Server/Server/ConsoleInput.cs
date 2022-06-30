using System;
using System.Collections.Generic;
using System.Threading;
using Swift;

namespace Server
{
    /// <summary>
    /// 接受控制台输入
    /// </summary>
    public class ConsoleInput : Component, IFrameDrived
    {
        // 游戏服务器
        public GameServer Srv = null;

        Thread thr; // 等待输入的工作线程
        bool running = false;

        // 等待执行的指令
        List<Action> cmdQ = new List<Action>();

        public void Start()
        {
            running = true;
            thr = new Thread(new ThreadStart(WorkingThread));
            thr.Start();

            OnCommand("stop", (ps) => { running = false; Srv.Stop(); });
        }

        void WorkingThread()
        {
            var cmd = "";
            while (running)
            {
                cmd = Console.ReadLine();
                if (cmd == null)
                {
                    Thread.Sleep(100);
                    continue;
                }

                var ins = Console.ReadLine().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                cmd = ins.Length > 0 ? ins[0] : "";
                var ps = ins.Length > 1 ? ins.SubArray(1, ins.Length - 1) : null;
                lock (cmdHandlers)
                {
                    if (!cmdHandlers.ContainsKey(cmd))
                        Console.WriteLine("unknown command: " + cmd);
                    else
                        cmdQ.Add(() => { cmdHandlers[cmd](ps); });
                }
            }
        }

        public void PushCommand(string cmd, params string[] ps)
        {
            cmdQ.Add(() => { cmdHandlers[cmd](ps); });
        }

        public void OnTimeElapsed(int te)
        {
            foreach (var cmd in cmdQ.ToArray())
                cmd();

            cmdQ.Clear();
        }

        Dictionary<string, Action<string[]>> cmdHandlers = new Dictionary<string, Action<string[]>>();
        public void OnCommand(string cmd, Action<string[]> op)
        {
            lock(cmdHandlers)
                cmdHandlers[cmd] = op;
        }
    }
}
