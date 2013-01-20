
using NLog;

namespace EvilDuck.Framework.Components
{
    public class BaseComponent
    {
        private readonly Logger _logger;
        protected Logger Log
        {
            get
            {
                return _logger ?? LogManager.GetLogger(GetType().FullName);
            }
        }

        public BaseComponent(Logger logger)
        {
            _logger = logger;
        }
    }
}
