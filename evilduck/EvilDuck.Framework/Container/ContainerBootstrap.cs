using Autofac;

namespace EvilDuck.Framework.Container
{
    public class ContainerBootstrap
    {
        public static void Initialize(Module module)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(module);

            var container = containerBuilder.Build();

            ComponentLocator.Initialize(container);
        }

        public static void Close()
        {
            ComponentLocator.Dispose();
        }
    }
}