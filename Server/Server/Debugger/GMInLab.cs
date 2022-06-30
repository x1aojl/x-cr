using Swift;

namespace Server
{
    public class GMInLab : Component
    {
        ConsoleInput ci;
        public override void Init()
        {
            ci = GetCom<ConsoleInput>();

            GetCom<LoginManager>().BeforeUserLogin += (Session s, bool isNew) =>
            {
                ////---- 这里加个 trick，方便内部测试
                var usrInfo = s.Usr.Info;
            };
        }
    }
}