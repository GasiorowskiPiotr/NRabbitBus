using System.Linq;
using Autofac;
using Autofac.Core;
using NLog;

namespace NRabbitBus.Framework.Logging
{
    public class LoggingModule : Module
    {
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += registration_Preparing;
        }

        void registration_Preparing(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[]
                                                  {
                                                      new ResolvedParameter(
                                                          (p, i) => p.ParameterType == typeof (Logger),
                                                          (p, i) => LogManager.GetLogger(t.FullName))
                                                  });
        }
    }
}
