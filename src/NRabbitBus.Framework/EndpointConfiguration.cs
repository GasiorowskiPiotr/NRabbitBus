using NRabbitBus.Framework.Configuration;

namespace NRabbitBus.Framework
{
    public abstract class EndpointConfiguration
    {
        public ExchangesConfiguration DeclareTheseExchanges { get; protected set; }
        public QueuesConfiguration DeclareTheseQueues { get; protected set; }
        public RoutesConfiguration SetupRoutes { get; protected set; }
        public MessageHandlersOrderConfiguration SetupHandlerOrder { get; protected set; }
    }
}