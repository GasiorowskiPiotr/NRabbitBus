using System.Configuration;

namespace EvilDuck.Framework.Tests.Configuration
{
    public class NestedElement : ConfigurationElement
    {
        [ConfigurationProperty("float")]
        public float Float
        {
            get { return (float)this["float"]; }
            set { this["float"] = value; }
        }
    }
}