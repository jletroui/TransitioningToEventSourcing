using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Linq.Expressions;
using Infrastructure.Reflection;
using nVentive.Umbrella.Reflection;
using nVentive.Umbrella.Extensions;
using System.Collections;
using Iesi.Collections;

namespace Infrastructure.Impl
{
    /// <summary>
    /// Maps data to a DTO class.
    /// DTOs must have an empty constuctor and properties with public getters, and private setters.
    /// Collections must be concrete type (List{T} rather than IList{T} for example).
    /// Supported collections are IList{T}, ISet{T}, IDictionary{T}.
    /// </summary>
    public class DTOMapper
    {
        /// <summary>
        /// Maps a list of DTOs. Collections are not supported.
        /// </summary>
        /// <typeparam name="T">The type to map to.</typeparam>
        /// <param name="reader">The reader from which to read values.</param>
        /// <returns>An enumerable of DTOs.</returns>
        public IEnumerable<T> MapList<T>(IDataReader reader) where T : new()
        {
            List<T> resVal = new List<T>();
            while (reader.Read())
            {
                T item = new T();
                ProcessSimpleDTO(item, reader);
                resVal.Add(item);
            }
            return resVal;
        }

        /// <summary>
        /// Maps a reader to a DTO including collections.
        /// </summary>
        /// <typeparam name="T">>The type to map to.</typeparam>
        /// <param name="reader">The reader from which to read values.</param>
        /// <param name="resultSetMapping">A list of expressions representing the collections to map the extra result sets to.</param>
        /// <returns>The mapped DTO.</returns>
        public T Map<T>(IDataReader reader, params Expression<Func<T, object>>[] resultSetMapping) where T : new()
        {
            T resVal = default(T);
            bool initialized = false;

            if (reader.Read())
            {
                // Process the first result set at the DTO level.
                resVal = new T();
                initialized = true;
                ProcessSimpleDTO(resVal, reader);
            }

            // The process the other result sets for the collections.
            int setIndex = 0;

            while (reader.NextResult() &&
                   setIndex < resultSetMapping.Length)
            {
                if (!initialized)
                {
                    resVal = new T();
                }
                ProcessCollection(resVal, reader, resultSetMapping[setIndex++]);
            }


            return resVal;
        }

        private void ProcessCollection<T>(T dto, IDataReader reader, Expression<Func<T, object>> property)
        {
            var member = property.Body as MemberExpression;

            if (!member.Type.ImplementsGenericDefinition(typeof(IDictionary<,>)) &&
                !member.Type.ImplementsGenericDefinition(typeof(IList<>)) &&
                !member.Type.ImplementsGenericDefinition(typeof(ISet<>)))
            {
                throw new DTOMappingException(string.Format(
                    "Error on property '{0}': The mapper only supports IList<T>, ISet<T>, and IDictionary<K,V> collections.",
                    member.Member.Name));
            }

            if (member.Type.GetConstructor(new Type[] { }) == null)
            {
                throw new DTOMappingException(string.Format(
                    "Error on property '{0}': The mapper only supports collections that have an empty constructor.",
                    member.Member.Name));
            }

            var collection = Activator.CreateInstance(member.Type);
            dto.Reflection().Set(member.Member.Name, collection);

            // Do we have a dictionary?
            if (member.Type.ImplementsGenericDefinition(typeof(IDictionary<,>)))
            {
                Type keyType = member.Type.GetGenericInterface(typeof(IDictionary<,>)).GetGenericArguments()[0];
                if (!keyType.IsSimpleType())
                {
                    throw new DTOMappingException(string.Format(
                        "Error on property '{0}': '{1}' is not a simple type. Only simple types are allowed for dictionary keys.",
                        member.Member.Name,
                        keyType.Name));
                }
                Type itemType = member.Type.GetGenericInterface(typeof(IDictionary<,>)).GetGenericArguments()[1];
                ProcessDictionary((IDictionary)collection, reader, itemType);
            } // Or a list?
            else if (member.Type.ImplementsGenericDefinition(typeof(IList<>)))
            {
                Type itemType = member.Type.GetGenericInterface(typeof(IList<>)).GetGenericArguments()[0];
                ProcessCollection(collection, (col, item) => ((IList)col).Add(item), reader, itemType);
            } // Or a set?
            else if (member.Type.ImplementsGenericDefinition(typeof(ISet<>)))
            {
                Type itemType = member.Type.GetGenericInterface(typeof(ISet<>)).GetGenericArguments()[0];
                ProcessCollection(collection, (col, item) => ((ISet)col).Add(item), reader, itemType);
            }
        }

        private void ProcessCollection(object collection,
            Action<object, object> addAction,
            IDataReader reader,
            Type itemType)
        {
            while (reader.Read())
            {
                object value;
                if (itemType.IsSimpleType())
                {
                    value = reader.GetValue(0);
                }
                else
                {
                    value = Activator.CreateInstance(itemType);
                    ProcessSimpleDTO(value, reader);
                }

                addAction(collection, value);
            }
        }

        private void ProcessDictionary(IDictionary dic, IDataReader reader, Type itemType)
        {
            while (reader.Read())
            {
                object value;
                if (itemType.IsSimpleType())
                {
                    value = reader.GetValue(1);
                }
                else
                {
                    value = Activator.CreateInstance(itemType);
                    ProcessSimpleDTO(value, reader, 1);
                }

                dic.Add(reader.GetValue(0), value);
            }
        }

        private void ProcessSimpleDTO(object instance, IDataReader reader)
        {
            ProcessSimpleDTO(instance, reader, 0);
        }

        private void ProcessSimpleDTO(object instance, IDataReader reader, int startField)
        {
            for (int i = startField; i < reader.FieldCount; i++)
            {
                ProcessDTOProperty(reader.GetName(i).Split('.'), instance, reader.GetValue(i));
            }
        }

        private void ProcessDTOProperty(string[] path, object instance, object value)
        {
            if (instance.Reflection().FindDescriptor(path[0]) != null)
            {
                if (value == DBNull.Value)
                {
                    value = null;
                }

                if (path.Length == 1)
                {
                    instance.Reflection().Set(path[0], value);
                }
                else
                {
                    var subInstance = instance.Reflection().Get(path[0]);

                    if (subInstance == null)
                    {
                        subInstance = Activator.CreateInstance(instance.Reflection().FindDescriptor(path[0]).Type);
                        instance.Reflection().Set(path[0], subInstance);
                    }

                    string[] subPath = new string[path.Length - 1];
                    Array.Copy(path, 1, subPath, 0, path.Length - 1);
                    ProcessDTOProperty(subPath, subInstance, value);
                }
            }
        }


    }

    /// <summary>
    /// Occurs when the mapping of a DTO can not be performed.
    /// </summary>
    public class DTOMappingException : Exception
    {
        /// <summary>
        /// Creates a <see cref="DTOMappingException"/> with the given message.
        /// </summary>
        /// <param name="message"></param>
        public DTOMappingException(string message)
            : base(message)
        {
        }
    }
}
