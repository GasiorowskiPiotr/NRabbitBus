namespace NRabbitBus.Framework.Configuration
{
    public class Exchange
    {
        public Exchange()
        {
        }

        public Exchange(string name, string type, bool durable)
        {
            Name = name;
            Type = type;
            Durable = durable;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
    }
}