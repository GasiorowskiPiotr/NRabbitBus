using System;
using EvilDuck.Framework.Components;
using NLog;
using NRabbitBus.Framework.Configuration;
using RabbitMQ.Client;

namespace NRabbitBus.Framework
{
    public class RabbitConnection : BaseComponent, IRabbitConnection
    {
        private readonly IRabbitConfigurationProvider _rabbitConfigurationProvider;
        private readonly ConnectionFactory _connectionFactory;

        private volatile IConnection _currentConnection;

        private readonly object _syncLock = new object();

        public RabbitConnection(IRabbitConfigurationProvider rabbitConfigurationProvider,  ConnectionFactory connectionFactory, Logger logger) : base(logger)
        {
            _rabbitConfigurationProvider = rabbitConfigurationProvider;
            _connectionFactory = connectionFactory;

            if(Log.IsDebugEnabled)
                Log.Debug("Getting RabbitConfiguration...");

            var configuration = _rabbitConfigurationProvider.Get();
            if(configuration != null)
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("Using RabbitConfiguration from Application Configuration file.");

                _connectionFactory.HostName = configuration.Hostname;
                _connectionFactory.Port = configuration.Port;
                _connectionFactory.UserName = configuration.Username;
                _connectionFactory.Password = configuration.Password;
            }
            else
            {
                if (Log.IsWarnEnabled)
                    Log.Warn("Using DEFAULT connection configuration.");
            }
            
        }

        public IConnection Current
        {
            get
            {
                if(_currentConnection == null)
                {
                    lock (_syncLock)
                    {
                        if(_currentConnection == null)
                        {
                            try
                            {
                                if(Log.IsInfoEnabled)
                                    Log.Info("Creating new connection");

                                var newConnection = _connectionFactory.CreateConnection();

                                _currentConnection = newConnection;
                                InvokeConnectionMade();

                                if (newConnection != null)
                                    newConnection.ConnectionShutdown += CurrentConnectionConnectionShutdown;

                                if(Log.IsInfoEnabled)
                                    Log.Info("New connection created.");
                            }
                            catch (Exception ex)
                            {
                                Log.ErrorException("Error creating new Connection", ex);
                            }
                        }
                    }
                }

                if(_currentConnection != null && _currentConnection.CloseReason != null)
                {
                    lock (_syncLock)
                    {
                        if(_currentConnection != null && _currentConnection.CloseReason != null)
                        {
                            try
                            {
                                if(Log.IsInfoEnabled)
                                    Log.Info("Connection has been closed: {0}. Creating another one.", _currentConnection.CloseReason);
                                var newConnection = _connectionFactory.CreateConnection();

                                _currentConnection = newConnection;
                                InvokeConnectionMade();
                                if (newConnection != null)
                                    newConnection.ConnectionShutdown += CurrentConnectionConnectionShutdown;

                                if (Log.IsInfoEnabled)
                                    Log.Info("New connection created.");
                            }
                            catch (Exception ex)
                            {
                                Log.ErrorException("Error creating new Connection", ex);
                            }
                        }
                    }
                }

                return _currentConnection;
            }
        }

        void CurrentConnectionConnectionShutdown(IConnection connection, ShutdownEventArgs reason)
        {
            if (Log.IsWarnEnabled)
                Log.Warn("Connection lost.");

            InvokeConnectionLost();
        }

        public event EventHandler<EventArgs> ConnectionMade;
        public event EventHandler<EventArgs> ConnectionLost;

        private void InvokeConnectionMade()
        {
            if(ConnectionMade != null)
            {
                ConnectionMade(this, EventArgs.Empty);
            }
        }

        private void InvokeConnectionLost()
        {
            if(ConnectionLost != null)
            {
                ConnectionLost(this, EventArgs.Empty);
            }
        }
    }
}