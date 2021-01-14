using System;
using Autofac;

namespace Bfm.Diet.Core.Dependency
{
    public class ScopeResolver : Resolver, IScopeResolver
    {
        private readonly ILifetimeScope _lifetimeScope;

        public ScopeResolver(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }


        public IScopeResolver BeginScope()
        {
            return new ScopeResolver(_lifetimeScope.BeginLifetimeScope());
        }

        public virtual void Dispose()
        {
            _lifetimeScope.Dispose();
        }
    }

    public interface IScopeResolver : IResolver, IDisposable
    {
        IScopeResolver BeginScope();
    }
}