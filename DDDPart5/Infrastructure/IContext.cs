using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Defines the context of a given operation and allow storage for that context.
    /// In a web application, this will be implemented with an HttpContext. 
    /// In other runtime, it will probably be implemented by a thread context.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Store or retrieve an item from the context.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[object key] { get; set; }
    }
}
