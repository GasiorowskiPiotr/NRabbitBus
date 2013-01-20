using System.Configuration;

namespace EvilDuck.Framework.Tests.Configuration
{
    public class SampleSection : ConfigurationSection
    {
        [ConfigurationProperty("data")]
        public string Data
        {
            get { return (string)this["data"]; }
            set { this["data"] = value; }
        }

        [ConfigurationProperty("defaultVal", DefaultValue = 666)]
        public int DefaultVal
        {
            get { return (int)this["defaultVal"]; }
            set { this["defaultVal"] = value; }
        }

        [ConfigurationProperty("nested")]
        public NestedElement Nested
        {
            get { return (NestedElement)this["nested"]; }
            set { this["nested"] = value; }
        }
    }
}