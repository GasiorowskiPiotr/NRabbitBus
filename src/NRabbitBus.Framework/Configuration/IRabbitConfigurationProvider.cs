namespace NRabbitBus.Framework.Configuration
{
    public interface IRabbitConfigurationProvider
    {
        RabbitConfiguration Get();
        void Load(RabbitConfiguration configuration);
    }
}