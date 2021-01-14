using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace Bfm.Diet.Core.Dependency
{
    public class Resolver : IResolver
    {
        private readonly IComponentContext _componentContext;


        public Resolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }


        public T Resolve<T>()
        {
            return _componentContext.Resolve<T>();
        }


        public object Resolve(Type serviceType)
        {
            return _componentContext.Resolve(serviceType);
        }


        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return ((IEnumerable) _componentContext.Resolve(enumerableType)).OfType<object>().ToList();
        }


        public IEnumerable<Type> GetRegisteredServices()
        {
            return _componentContext.ComponentRegistry.Registrations
                .SelectMany(x => x.Services)
                .OfType<TypedService>()
                .Select(x => x.ServiceType)
                .ToList();
        }


        public bool IsRegistered<T>()
            where T : class
        {
            return IsRegistered(typeof(T));
        }


        public bool IsRegistered(Type type)
        {
            return _componentContext.IsRegistered(type);
        }

        public T Resolve<T>(object argumentAsAnonymousType)
        {
            return _componentContext.Resolve<T>(argumentAsAnonymousType.GetTypedResolvingParameters());
        }


        public object Resolve(Type type, object argumentAsAnonymousType)
        {
            return _componentContext.Resolve(type, argumentAsAnonymousType.GetTypedResolvingParameters());
        }


        public T Resolve<T>(params Parameter[] parameters)
        {
            return _componentContext.Resolve<T>(parameters);
        }


        public object Resolve(Type serviceType, params Parameter[] parameters)
        {
            return _componentContext.Resolve(serviceType, parameters);
        }
    }


    public interface IResolver
    {
        T Resolve<T>();


        T Resolve<T>(object argumentAsAnonymousType);


        object Resolve(Type type, object argumentAsAnonymousType);


        object Resolve(Type serviceType);


        IEnumerable<object> ResolveAll(Type serviceType);


        IEnumerable<Type> GetRegisteredServices();


        bool IsRegistered<T>() where T : class;

        bool IsRegistered(Type type);
    }
}