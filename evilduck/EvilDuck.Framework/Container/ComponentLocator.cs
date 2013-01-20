using System;
using System.Collections.Generic;
using Autofac;

namespace EvilDuck.Framework.Container
{
    public class ComponentLocator
    {
        private volatile IContainer _container;

        protected ComponentLocator(IContainer container)
        {
            _container = container;
        }

        public static ComponentLocator Current { get; private set; }

        public static void Initialize(IContainer container)
        {
            if (Current == null)
            {
                lock (typeof(ComponentLocator))
                {
                    if (Current == null)
                    {
                        Current = new ComponentLocator(container);
                    }
                }
            }
        }

        public ILifetimeScope StartChildScope()
        {
            return _container.BeginLifetimeScope();
        }

        public TType Get<TType>()
        {
            TType result;

            _container.TryResolve(out result);

            return result;
        }

        public object Get(Type type)
        {
            object o;
            _container.TryResolve(type, out o);

            return o;
        }

        public IEnumerable<TType> GetAll<TType>()
        {
            IEnumerable<TType> result;
            _container.TryResolve(out result);

            return result;
        }

        public IEnumerable<object> GetAll(Type type)
        {
            var ttype = typeof (IEnumerable<>).MakeGenericType(type);

            return _container.Resolve(ttype) as IEnumerable<object>;
        }

        public static void Dispose()
        {
            if (Current != null)
            {
                if (Current._container != null)
                {
                    Current._container.Dispose();
                    Current._container = null;
                }
                Current = null;
            }
        }
    }
}