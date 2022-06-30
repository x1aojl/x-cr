using System.Collections.Generic;
using Swift;

namespace Server
{
    /// <summary>
    /// 匹配管理
    /// </summary>
    public class MatchBoard : Component
    {
        UserPort UP;
        SessionContainer SC;
        RoomManager RM;

        public override void Init()
        {
            UP = GetCom<UserPort>();
            SC = GetCom<SessionContainer>();
            RM = GetCom<RoomManager>();

            UP.OnMessage("InMatch", OnInMatch);
            UP.OnMessage("OutMatch", OnOutMatch);
            UP.OnMessage("MathRobot", OnMatchRobot);

            GetCom<LoginManager>().OnUserDisconnecting += OnUserDisconnecting;
        }

        const int MinNum4Battle = 2; // 开局最少 2 人 
        List<Session> waitingLst = new List<Session>();
        void OnUserDisconnecting(Session s)
        {
            if (waitingLst.Contains(s))
                waitingLst.Remove(s);
        }

        void OnInMatch(Session s, IReadableBuffer data)
        {
            if (waitingLst.Contains(s))
                return;

            waitingLst.Add(s);
            if (waitingLst.Count >= MinNum4Battle)
            {
                CreateRoom(waitingLst[0]);
                waitingLst.Clear();
            }
        }

        void OnOutMatch(Session s, IReadableBuffer data)
        {
            if (waitingLst.Contains(s))
                waitingLst.Remove(s);
        }

        void OnMatchRobot(Session s, IReadableBuffer data)
        {
            if (waitingLst.Contains(s))
                waitingLst.Remove(s);

            var r = CreateRoom(s);
        }

        // 创建房间
        Room4Server CreateRoom(Session owner)
        {
            var r = new Room4Server(owner.ID);
            return r;
        }
    }
}