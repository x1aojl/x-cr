using System;
using Swift;
using SpaceX;

namespace Server
{
    /// <summary>
    /// 登录管理器
    /// </summary>
    public class LoginManager : Component
    {
        SessionContainer SC;
        UserPort UP;
        UserContainer UC;

        // 用户登录
        public Action<Session, bool> BeforeUserLogin = null;
        public Action<Session, bool> OnUserLogin = null;

        // 用户连接断开
        public Action<Session> OnUserDisconnecting = null;

        // 初始化
        public override void Init()
        {
            SC = GetCom<SessionContainer>();
            UP = GetCom<UserPort>();
            UC = GetCom<UserContainer>();
            var nc = GetCom<NetCore>();
            nc.OnDisconnected += OnDisconnected;

            UP.OnRequest("Login", OnUserLoginMsg);
        }

        // 连接断开
        void OnDisconnected(Connection conn, string reason)
        {
            var s = SC.GetByConn(conn);
            if (s == null)
                return;

            KickOut(s.ID);
        }

        // 用户登录请求
        void OnUserLoginMsg(Connection conn, IReadableBuffer data, IWriteableBuffer buff, Action end)
        {
            var uid = data.ReadString();
            var pwd = data.ReadString();

            if (SC[uid] != null)
                KickOut(uid);

            UC.Retrieve(uid, (usr) =>
            {
                var isNew = usr == null;
                if (isNew) // 用户不存在就创建新的
                {
                    usr = new User();
                    usr.ID = uid;
                    usr.Pwd = pwd;
                    usr.Info = new UserInfo();
                    UC.AddNew(usr);
                }
                else if (pwd != usr.Pwd)
                {
                    buff.Write(false);
                    end();
                    return;
                }

                // 创建会话
                var s = new Session();
                s.Usr = usr;
                s.Conn = conn;
                SC[uid] = s;

                BeforeUserLogin.SC(s, isNew);

                // 通知登录成功
                buff.Write(true);
                buff.Write(usr.Info);
                end();

                OnUserLogin.SC(s, isNew);
            });
        }

        // 踢掉用户，断开连接
        void KickOut(string uid)
        {
            var s = SC[uid];
            if (s == null)
                return;

            OnUserDisconnecting.SC(s);

            SC.Remove(uid);

            var conn = s.Conn;
            if (conn == null)
                return;

            conn.Close();
        }
    }
}
