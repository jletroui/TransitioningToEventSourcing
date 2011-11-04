using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// A bus allowing to send commands to the domain.
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Sends the given command to the domain.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        void Send<T>(T cmd) where T : ICommand;
    }
}
