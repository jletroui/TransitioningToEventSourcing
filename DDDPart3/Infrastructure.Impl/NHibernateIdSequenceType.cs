using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.UserTypes;
using NHibernate.SqlTypes;
using NHibernate;
using System.Data;

namespace Infrastructure.Impl
{
	public class NHibernateIdSequenceType : IUserType
	{
        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (x == null || y == null) return false;

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var obj = NHibernateUtil.Int32.NullSafeGet(rs, names[0]);

            if (obj == null) return null;

            return new IdSequence((int)obj);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDataParameter)cmd.Parameters[index]).Value = ((IdSequence)value).ToId();
            }

        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType
        {
            get { return typeof(IdSequence); }
        }

        public SqlType[] SqlTypes
        {
            get { return new SqlType[]{NHibernateUtil.Int32.SqlType}; }
        }
    }
}
