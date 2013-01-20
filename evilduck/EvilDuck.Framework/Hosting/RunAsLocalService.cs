using Topshelf;
using Topshelf.HostConfigurators;

namespace EvilDuck.Framework.Hosting
{
    public class RunAsLocalService : RunAs
    {
        public override void Configure(HostConfigurator conf)
        {
            conf.RunAsLocalService();
        }
    }
}