using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if the given type implements the given generic definition. A generic definition is the type without concrete type. Ex: IDictionary{,}.
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

        /// <summary>
        /// Indicate whether or not this type is a simple one.
        /// A simple object is a well known struct.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsSimpleType(this Type t)
        {
            return t == typeof(byte) ||
                t == typeof(short) ||
                t == typeof(uint) ||
                t == typeof(long) ||
                t == typeof(ulong) ||
                t == typeof(string) ||
                t == typeof(DateTime) ||
                t == typeof(bool) ||
                t == typeof(float) ||
                t == typeof(double) ||
                t == typeof(decimal) ||
                t == typeof(char) ||
                t == typeof(int) ||
                t.IsEnum;
        }

        /// <summary>
        /// Gets the generic interface supporting the given generic definition in the given type.
        /// </summary>
        /// <param name="t">The type to get the generic interface from.</param>
        /// <param name="genericDefinition">The generic definition we are interested in.</param>
        /// <returns>The generic interface. Ex: IDictionary{string, object} for typeof(Dictionary{string, object}).GetGenericInterface(typeof(IDictionary{,}))</returns>
        public static Type GetGenericInterface(this Type t, Type genericDefinition)
        {
            Type resVal = null;

            foreach (Type i in t.GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == genericDefinition)
                {
                    resVal = i;
                    break;
                }
            }

            return resVal;
        }

    }
}
