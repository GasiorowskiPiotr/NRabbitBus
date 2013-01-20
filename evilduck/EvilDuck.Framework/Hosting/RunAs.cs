using Topshelf.HostConfigurators;

namespace EvilDuck.Framework.Hosting
{
    public abstract class RunAs
    {
        public abstract void Configure(HostConfigurator conf);

        public static RunAs LocalSystem()
        {
            return new RunAsLocalSystem();
        }

        public static RunAs LocalService()
        {
            return new RunAsLocalService();
        }

        public static RunAs NetworkService()
        {
            return new RunAsNetworkService();
        }

        public static RunAs Custom(string username, string password)
        {
            return new RunAsCustomUser(username, password);
        }
    }
}