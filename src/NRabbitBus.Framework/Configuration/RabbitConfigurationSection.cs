using System.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    public class RabbitConfigurationSection : ConfigurationSection
    {
        private const string HostnamePropertyName = "hostname";

        [ConfigurationProperty(HostnamePropertyName)]
        public string Hostname
        {
            get { return (string) base[HostnamePropertyName]; }
        }

        private const string PortPropertyName = "port";

        [ConfigurationProperty(PortPropertyName)]
        public int Port
        {
            get { return (int) base[PortPropertyName]; }
        }

        private const string UsernamePropertyName = "username";

        [ConfigurationProperty(UsernamePropertyName)]
        public string Username
        {
            get { return (string) base[UsernamePropertyName]; }
        }

        private const string PasswordPropertyName = "password";

        [ConfigurationProperty(PasswordPropertyName)]
        public string Password
        {
            get { return (string) base[PasswordPropertyName]; }
        }
    }
}
