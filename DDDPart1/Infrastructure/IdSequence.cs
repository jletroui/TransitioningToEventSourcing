using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// A value object which purpose is to generate ids.
    /// </summary>
    public struct IdSequence
    {
        private int sequence;

        public IdSequence(int sequence)
        {
            this.sequence = sequence;
        }

        /// <summary>
        /// Generate the next id in the sequence.
        /// </summary>
        /// <returns></returns>
        public IdSequence Next()
        {
            return new IdSequence(sequence + 1);
        }

        /// <summary>
        /// Gets the current id. Use it for creating a new entity.
        /// </summary>
        /// <returns></returns>
        public int ToId()
        {
            return sequence;
        }
    }
}
