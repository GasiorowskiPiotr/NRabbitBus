namespace NRabbitBus.Framework.Configuration
{
    public class Queue
    {
        public Queue()
        {
        }

        public Queue(string name, bool requiresAck, bool durable, bool isRcp, short maxThreads)
        {
            Name = name;
            RequiresAck = requiresAck;
            Durable = durable;
            IsRcp = isRcp;
            MaxThreads = maxThreads;
        }

        public string Name { get; set; }
        public bool RequiresAck { get; set; }
        public bool Durable { get; set; }
        public bool IsRcp { get; set; }
        public short MaxThreads { get; set; }
    }
}