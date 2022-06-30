using System;

namespace SpaceX
{
    public abstract class Logger
    {
        public static Logger Instance { get; set; }

        public abstract void Info(string msg);
        public abstract void Err(string msg);
    }

    public class SimpleLogger : Logger
    {
        Action<string> infoImpl = null;
        Action<string> errImpl = null;

        public SimpleLogger(Action<string> info, Action<string> err)
        {
            infoImpl = info;
            errImpl = err;
        }

        public override void Info(string msg)
        {
            infoImpl(msg);
        }

        public override void Err(string msg)
        {
            errImpl(msg);
        }
    }
}
