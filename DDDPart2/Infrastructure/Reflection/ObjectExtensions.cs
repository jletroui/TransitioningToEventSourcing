using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Infrastructure.Reflection
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts the given anonymous object to a dictionary.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object o)
        {
            Dictionary<string, object> resVal = new Dictionary<string, object>();

            if (o != null)
            {
                foreach (System.ComponentModel.PropertyDescriptor prop in TypeDescriptor.GetProperties(o))
                {
                    resVal.Add(prop.Name, prop.GetValue(o));
                }
            }

            return resVal;
        }

    }
}
