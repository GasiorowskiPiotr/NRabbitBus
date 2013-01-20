using Topshelf;
using Topshelf.HostConfigurators;

namespace EvilDuck.Framework.Hosting
{
    public class RunAsLocalSystem : RunAs
    {
        public override void Configure(HostConfigurator conf)
        {
            conf.RunAsLocalSystem();
        }
    }
}