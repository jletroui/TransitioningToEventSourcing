using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Represents various model for component instanciation in an ioc container.
    /// </summary>
    public enum ComponentInstanciationPolicy
    {
        /// <summary>
        /// Accept the default call model of the underlying technology.
        /// </summary>
        None,
        /// <summary>
        /// Only one instance of the component will ever be called.
        /// </summary>
        Singleton,
        /// <summary>
        /// Each call on the component will be performed on a new instance.
        /// </summary>
        NewInstance
    }
}
