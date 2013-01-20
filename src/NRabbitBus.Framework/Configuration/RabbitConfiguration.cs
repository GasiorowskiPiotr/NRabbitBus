using EvilDuck.Framework.Configuration;

namespace NRabbitBus.Framework.Configuration
{
    

    public class RabbitConfiguration
    {
        private readonly string _hostname;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public string Hostname
        {
            get { return _hostname; }
        }

        public int Port
        {
            get { return _port; }
        }

        public string Username
        {
            get { return _username; }
        }

        public string Password
        {
            get { return _password; }
        }

        RabbitConfiguration(string hostname, int port, string username, string password)
        {
            _hostname = hostname;
            _port = port;
            _username = username;
            _password = password;
        }

        public static RabbitConfiguration FromCode(string hostname, int port, string username, string password)
        {
            return new RabbitConfiguration(hostname, port, username, password);
        }

        public static RabbitConfiguration FromConfiguration()
        {
            var configuration = Configuration<RabbitConfigurationSection>.Current;
            if (configuration != null)
                return new RabbitConfiguration(
                    configuration.Hostname,
                    configuration.Port,
                    configuration.Username,
                    configuration.Password);
            return null;
        }
    }
}