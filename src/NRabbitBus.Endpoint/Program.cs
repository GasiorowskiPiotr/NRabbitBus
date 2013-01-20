using EvilDuck.Framework.Hosting;

namespace NRabbitBus.Endpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationHost.Run<RabbitEndpoint>();
        }
    }
}
