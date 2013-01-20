using System.Runtime.Serialization.Formatters;
using EvilDuck.Framework.Components;
using NLog;
using Newtonsoft.Json;

namespace NRabbitBus.Framework.MessageProcess
{
    public class JsonMessageStringifier : BaseComponent, IMessageStringifier
    {
        public JsonMessageStringifier(Logger logger) : base(logger)
        {
        }

        public virtual string Stringify(object obj)
        {
         
            var str = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                                                        {
                                                            TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                                                            TypeNameHandling = TypeNameHandling.All
                                                        });

            if (Log.IsDebugEnabled)
                Log.Debug("Object stringified as: {0}", str);

            return str;
        }

        public virtual object Destringify(string objStringForm)
        {
            if (Log.IsDebugEnabled)
                Log.Debug("Object {0} will be destringified");

            var obj = JsonConvert.DeserializeObject(objStringForm, new JsonSerializerSettings
                                                                    {
                                                                        TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                                                                        TypeNameHandling = TypeNameHandling.All
                                                                    });

            if (Log.IsDebugEnabled)
                Log.Debug("Destringify finished");

            return obj;
        }
    }
}