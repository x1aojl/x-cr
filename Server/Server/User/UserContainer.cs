using Swift;

namespace Server
{
    class UserContainer : DataContainer<User, string>
    {
        public UserContainer(MySqlDbPersistence<User, string> persistence) : base(persistence)
        {
        }
    }
}