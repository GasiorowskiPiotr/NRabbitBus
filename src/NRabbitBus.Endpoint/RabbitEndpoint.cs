using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using EvilDuck.Framework.Components;
using EvilDuck.Framework.Hosting;
using NLog;
using NRabbitBus.Framework;

namespace NRabbitBus.Endpoint
{
    class RabbitEndpoint : BaseComponent, IHostableApplication
    {
        private const string RabbitAssemblyString = "rabbitAssembly";

        public RabbitEndpoint()
            : base(LogManager.GetLogger(typeof(RabbitEndpoint).FullName))
        {
            }

        public void Start()
        {
            var asmStr = ConfigurationManager.AppSettings[RabbitAssemblyString];
            if(Log.IsInfoEnabled)
                Log.Info("Loading assembly: {0}...", asmStr);

            var asm = Assembly.Load(asmStr);

            if (Log.IsInfoEnabled)
                Log.Info("Assembly {0} loaded.", asmStr);

            if (Log.IsInfoEnabled)
                Log.Info("Looking for Endpoint Configuration in Assembly: {0}", asmStr);

            var endpointConfigurationTypes = asm.GetTypes().Where(t => typeof (EndpointConfiguration).IsAssignableFrom(t)).ToList();
            if(endpointConfigurationTypes.Count == 0)
                throw new Exception("No EndpointConfiguration found");

            if(endpointConfigurationTypes.Count > 1)
                throw new Exception("More than one EndpointConfiguration found");

            var endpoitConfiguration = (EndpointConfiguration)Activator.CreateInstance(endpointConfigurationTypes[0]);

            if (Log.IsInfoEnabled)
                Log.Info("Endpoint properly {0} loaded", endpoitConfiguration.GetType());

            if (Log.IsInfoEnabled)
                Log.Info("Initializing Rabbit...");

            var rabbit = Rabbit.Initialize(asm);
            if(endpoitConfiguration.DeclareTheseExchanges != null)
                rabbit.DeclareExchanges(endpoitConfiguration.DeclareTheseExchanges);

            if(endpoitConfiguration.DeclareTheseQueues != null)
                rabbit.DeclareQueues(endpoitConfiguration.DeclareTheseQueues);

            if(endpoitConfiguration.SetupRoutes != null)
                rabbit.SetupRouting(endpoitConfiguration.SetupRoutes);

            if (endpoitConfiguration.SetupHandlerOrder != null)
                rabbit.SetMessageHandlerOrder(endpoitConfiguration.SetupHandlerOrder);

            rabbit.StartListeningOnDeclaredQueues();

            if (Log.IsInfoEnabled)
                Log.Info("Rabbit initialized.");
        }

        public void Stop()
        {
            
        }

        public RunAs RunAs
        {
            get { return RunAs.LocalSystem(); }
        }
        public string Describtion
        {
            get { return "Rabbit Endpoint"; }
        }
        public string ServiceName
        {
            get { return "RabbitEndpoint"; }
        }
        public string DisplayName
        {
            get { return "RabbitEndpoint"; }
        }
    }
}