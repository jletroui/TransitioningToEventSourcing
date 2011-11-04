using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Releasers;
using Castle.Windsor;
using Castle.MicroKernel;
using Castle.Core;
using Castle.MicroKernel.Registration;


namespace Infrastructure.Impl
{
    /// <summary>
    /// Castle Windsor implementaton of IContainer.
    /// </summary>
    public class WindsorObjectBuilder : IContainer
    {
        public IWindsorContainer Container { get; private set; }

        public WindsorObjectBuilder()
        {
            Container = new WindsorContainer();
            Container.Kernel.ReleasePolicy = new NoTrackingReleasePolicy();
        }

        public void Configure<T>(ComponentInstanciationPolicy callModel)
        {
            ((IContainer)this).Configure(typeof(T), callModel);
        }

        public void Configure(Type t, ComponentInstanciationPolicy callModel)
        {
            var handler = GetHandlerForType(t);
            if (handler == null)
            {
                var reg = Component.For(GetAllServiceTypesFor(t)).ImplementedBy(t);
                reg.LifeStyle.Is(GetLifestyleTypeFrom(callModel));

                Container.Kernel.Register(reg);
            }
        }

        public void RegisterSingleton<T>(T instance)
        {
            foreach (Type service in GetAllServiceTypesFor(typeof(T)))
            {
                Container.Kernel.AddComponentInstance(Guid.NewGuid().ToString(), service, instance);
            }
        }

        public T Build<T>()
        {
            return Container.Resolve<T>();
        }

        public T Build<T>(Type type)
        {
            return (T)Container.Resolve(type);
        }

        public IEnumerable<T> BuildAll<T>()
        {
            return Container.ResolveAll<T>();
        }

        public IEnumerable<T> BuildAll<T>(Type type)
        {
            return Container.ResolveAll(type).Cast<T>();
        }

        private static LifestyleType GetLifestyleTypeFrom(ComponentInstanciationPolicy callModel)
        {
            switch (callModel)
            {
                case ComponentInstanciationPolicy.NewInstance: return LifestyleType.Transient;
                case ComponentInstanciationPolicy.Singleton: return LifestyleType.Singleton;
            }

            return LifestyleType.Undefined;
        }

        private static IEnumerable<Type> GetAllServiceTypesFor(Type t)
        {
            return new List<Type>(t.GetInterfaces()) { t };
        }

        private IHandler GetHandlerForType(Type t)
        {
            return Container.Kernel.GetAssignableHandlers(typeof(object))
                .Where(h => h.ComponentModel.Implementation == t)
                .FirstOrDefault();
        }
    }
}
