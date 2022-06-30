using System.Collections.Generic;

namespace SpaceX
{
    /// <summary>
    /// 对战房间
    /// </summary>
    public class Room
    {
        List<string> usrs = new List<string>();
        public Room(string owner)
        {
            usrs.Add(owner);
        }

        // 根据 uid 获取角色编号
        public int GetPlayerByUser(string uid)
        {
            return usrs.IndexOf(uid);
        }

        #region 对战流程

        #endregion
    }
}