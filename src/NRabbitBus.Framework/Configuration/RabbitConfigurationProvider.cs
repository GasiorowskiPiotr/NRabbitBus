using EvilDuck.Framework.Components;
using NLog;

namespace NRabbitBus.Framework.Configuration
{
    public class RabbitConfigurationProvider : BaseComponent, IRabbitConfigurationProvider
    {
        private volatile RabbitConfiguration _codeConfiguration;

        private volatile RabbitConfiguration _appConfigConfiguration;

        private RabbitConfiguration _currentConfiguration;

        private readonly object _syncLock = new object();

        public RabbitConfigurationProvider(Logger logger) : base(logger)
        {
        }

        public RabbitConfiguration Get()
        {
            if(Log.IsInfoEnabled)
                Log.Info("Trying to get RabbitConfiguration...");

            if (_currentConfiguration != null)
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Configuration already exists.");
                return _currentConfiguration;
            }
                

            if(_appConfigConfiguration == null)
            {
                lock (_syncLock)
                {
                    if(_appConfigConfiguration == null)
                    {
                        if (Log.IsInfoEnabled)
                            Log.Info("Loading configuration from Application Configuration file...");

                        _appConfigConfiguration = RabbitConfiguration.FromConfiguration();

                        if (Log.IsInfoEnabled)
                            Log.Info("Configuration from Application Configuration file loaded.");
                    }
                }
            }

            if (_appConfigConfiguration != null)
            {
                if (Log.IsInfoEnabled)
                    Log.Info("Application Configuration file configuration will be used.");

                _currentConfiguration = _appConfigConfiguration;
                return _appConfigConfiguration;
            }

            if (_codeConfiguration != null)
            {
                if (Log.IsInfoEnabled)
                    Log.Info("Code configuration will be used.");

                _currentConfiguration = _codeConfiguration;
                return _codeConfiguration;
            }

            if (Log.IsInfoEnabled)
                Log.Info("No configuration found.");
            _currentConfiguration = null;
            return null;
        }

        public void Load(RabbitConfiguration configuration)
        {
            if (Log.IsInfoEnabled)
                Log.Info("Loading configuration from code");

            _codeConfiguration = configuration;
        }
    }
}