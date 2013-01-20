using NRabbitBus.Framework;
using NRabbitBus.Framework.Configuration;

namespace Consumer
{
    public class ConsumerEndpoint : EndpointConfiguration
    {
        public ConsumerEndpoint()
        {
            var queues = QueuesConfiguration.FromCode(new Queue
                                                          {
                                                              Durable = false,
                                                              Name = "3_1_Simple_Queue",
                                                              RequiresAck = false,
                                                              IsRcp = false,
                                                              MaxThreads = 10
                                                          },
                                                      new Queue
                                                          {
                                                              Durable = false,
                                                              Name = "3_1_Routed_Queue1",
                                                              RequiresAck = false,
                                                              IsRcp = false,
                                                              MaxThreads = 10
                                                          },
                                                      new Queue
                                                          {
                                                              Durable = false,
                                                              Name = "3_1_Routed_Queue2",
                                                              RequiresAck = false,
                                                              IsRcp = false,
                                                              MaxThreads = 10
                                                          });
                             

            DeclareTheseQueues = queues;
            DeclareTheseExchanges = ExchangesConfiguration.FromCode(new Exchange
                                                                        {
                                                                            Durable = false,
                                                                            Name = "3_1_RoutedExchange",
                                                                            Type = "direct"
                                                                        });

            SetupRoutes = RoutesConfiguration.FromCode(new Route("3_1_Routed_Queue1", "3_1_RoutedExchange", "MyKey1"),
                                                       new Route("3_1_Routed_Queue2", "3_1_RoutedExchange", "MyKey2"));
                                    

        }
    }
}