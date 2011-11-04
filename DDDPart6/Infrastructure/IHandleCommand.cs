using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Represents a handler of a given command type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHandleCommand<T> where T : ICommand
    {
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="cmd"></param>
        void Handle(T cmd);
    }
}
