using System;
using Swift;
using SpaceX;

namespace Server
{
    /// <summary>
    /// 对战房间，服务器端使用
    /// </summary>
    public class Room4Server : Room
    {
        public Room4Server(string owner)
            : base(owner) { }

        // 映射消息处理
        StableDictionary<string, Action<int, IReadableBuffer>> msgHandlers = null;

        public void OnMessage(string op, string uid, IReadableBuffer data)
        {
            MakeSureMsgHandlers();
            var p = GetPlayerByUser(uid);
            msgHandlers[op](p, data);
        }

        // 建立消息映射表
        void MakeSureMsgHandlers()
        {
            if (msgHandlers != null)
                return;

            msgHandlers = new StableDictionary<string, Action<int, IReadableBuffer>>();
        }
    }
}