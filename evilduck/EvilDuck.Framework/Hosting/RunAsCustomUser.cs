using Topshelf;
using Topshelf.HostConfigurators;

namespace EvilDuck.Framework.Hosting
{
    public class RunAsCustomUser : RunAs
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public RunAsCustomUser(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public override void Configure(HostConfigurator conf)
        {
            conf.RunAs(Username, Password);
        }
    }
}