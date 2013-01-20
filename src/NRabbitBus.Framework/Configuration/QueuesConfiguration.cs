using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NRabbitBus.Framework.Configuration
{
    public class QueuesConfiguration
    {
        private readonly List<Queue> _queues = new List<Queue>();
        public IEnumerable<Queue> Queues
        {
            get { return _queues; }
        }

        public static QueuesConfiguration FromCode(params Queue[] queues)
        {
            var conf = new QueuesConfiguration();
            if (queues != null && queues.Length > 0)
                conf._queues.AddRange(queues);

            return conf;
        }

        private QueuesConfiguration()
        {
        }

        public static QueuesConfiguration FromConfiguration()
        {
            var queuesConfigurations = new QueuesConfiguration();

            var queueConfigurationSection = ConfigurationManager.GetSection("QueueConfigurationSection") as QueueConfigurationSection;
            if(queueConfigurationSection != null)
            {
                queuesConfigurations._queues.AddRange(from QueueElement queue in queueConfigurationSection.Queues
                                select new Queue
                                           {
                                               Name = queue.Name, 
                                               RequiresAck = queue.AckRequired,
                                               Durable = queue.Durable,
                                               MaxThreads = queue.MaxThreads
                                           });
            }

            return queuesConfigurations;
        }
    }
}