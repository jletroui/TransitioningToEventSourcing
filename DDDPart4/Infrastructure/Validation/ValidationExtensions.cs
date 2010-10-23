using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using nVentive.Umbrella.Validation;
using nVentive.Umbrella.Extensions;

namespace Infrastructure.Validation
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Checks the given value is not null or don't have only white spaces.
        /// </summary>
        /// <param name="extPoint"></param>
        /// <param name="name"></param>
        public static string NotEmpty(this ValidationExtensionPoint<string> extPoint, string name)
        {
            if (string.IsNullOrWhiteSpace(extPoint.ExtendedValue))
            {
                throw new ArgumentNullException(name, "This parameter can not be null or empty.");
            }

            return extPoint.ExtendedValue;
        }
    }
}
