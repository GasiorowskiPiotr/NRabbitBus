using System.Configuration;

namespace EvilDuck.Framework.Tests.Configuration
{
    public class NameAndValues : ConfigurationSection
    {
        [ConfigurationProperty("data")]
        public KeyValueConfigurationCollection Data
        {
            get { return (KeyValueConfigurationCollection)this["data"]; }
            set { this["data"] = value; }
        }
    }
}