using Topshelf;
using Topshelf.HostConfigurators;

namespace EvilDuck.Framework.Hosting
{
    public class RunAsNetworkService : RunAs
    {
        public override void Configure(HostConfigurator conf)
        {
            conf.RunAsNetworkService();
        }
    }
}