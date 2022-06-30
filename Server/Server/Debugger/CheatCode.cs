using Swift;

namespace Server
{
    public class CheatCode : Component
    {
        ConsoleInput ci;
        public override void Init()
        {
            ci = GetCom<ConsoleInput>();
        }
    }
}