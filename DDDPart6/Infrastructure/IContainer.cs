using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Abstraction of a container.
    /// Inspired from NServiceBus's abstraction.
    /// </summary>
    public interface IContainer
    {
        // Lookup
        T Build<T>();
        T Build<T>(Type type);
        IEnumerable<T> BuildAll<T>();
        IEnumerable<T> BuildAll<T>(Type type);

        // Config
        void Configure<T>(ComponentInstanciationPolicy instanciationPolicy);
        void Configure(Type t, ComponentInstanciationPolicy instanciationPolicy);
        void RegisterSingleton<T>(T instance);

    }
}
