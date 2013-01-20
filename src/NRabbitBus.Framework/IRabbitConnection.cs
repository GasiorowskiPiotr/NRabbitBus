using System;
using RabbitMQ.Client;

namespace NRabbitBus.Framework
{
    public interface IRabbitConnection
    {
        IConnection Current { get; }

        event EventHandler<EventArgs> ConnectionMade;
        event EventHandler<EventArgs> ConnectionLost;
    }
}