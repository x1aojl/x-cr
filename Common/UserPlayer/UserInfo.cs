using Swift;

namespace SpaceX
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : SerializableData
    {
        protected override void Sync()
        {
            BeginSync();
            EndSync();
        }
    }
}
