using System.Collections.Generic;
using Swift;

namespace Server
{
    /// <summary>
    /// 房间管理，转发房间消息
    /// </summary>
    public class RoomManager : Component
    {
        UserPort UP;
        SessionContainer SC;

        // 所有当前房间
        List<Room4Server> rooms = new List<Room4Server>();

        // userid 对应到房间
        StableDictionary<string, Room4Server> usr2room = new StableDictionary<string, Room4Server>();

        void RedirectMessage(string op)
        {
            UP.OnMessage(op, (Session s, IReadableBuffer data) =>
            {
                var uid = s.ID;
                var r = usr2room.ContainsKey(uid) ? usr2room[uid] : null;
                if (r == null)
                    return;

                r.OnMessage(op, uid, data);
            });
        }

        public override void Init()
        {
            UP = GetCom<UserPort>();
            SC = GetCom<SessionContainer>();
        }
    }
}