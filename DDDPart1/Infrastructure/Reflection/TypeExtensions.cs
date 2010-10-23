using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if the given type implements the given generic definition. A generic definition is the type without concrete type. Ex: IDictionary&lt;,&gt;.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericDefinition"></param>
        /// <returns></returns>
        public static bool ImplementsGenericDefinition(this Type type, Type genericDefinition)
        {
            bool resVal = false;

            if (genericDefinition.IsInterface)
            {
                foreach (Type i in type.GetInterfaces())
                {
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == genericDefinition)
                    {
                        resVal = true;
                        break;
                    }
                }
            }
            else
            {
                Type concrete = type;
                while (concrete != null && !resVal)
                {
                    if (concrete.IsGenericType && concrete.GetGenericTypeDefinition() == genericDefinition)
                    {
                        resVal = true;
                        break;
                    }
                    concrete = concrete.BaseType;
                }
            }

            return resVal;

        }
    }
}
