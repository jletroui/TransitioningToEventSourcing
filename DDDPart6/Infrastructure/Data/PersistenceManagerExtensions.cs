using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Reflection;
using System.Data;

namespace Infrastructure.Data
{
    public static class PersistenceManagerExtensions
    {
        public static int ExecuteNonQuery(this IPersistenceManager pm, string commandText, object commandParameters)
        {
            using (var cmd = pm.CreateCommand())
            {
                cmd.CommandText = commandText;

                foreach (var pair in commandParameters.ToDictionary())
                {
                    cmd.AddParameter("@" + pair.Key, pair.Value);
                }

                return cmd.ExecuteNonQuery();
            }
        }

        public static IDataReader ExecuteQuery(this IPersistenceManager pm, string commandText, object commandParameters)
        {
            using (var cmd = pm.CreateCommand())
            {
                cmd.CommandText = commandText;

                foreach (var pair in commandParameters.ToDictionary())
                {
                    cmd.AddParameter("@" + pair.Key, pair.Value);
                }

                return cmd.ExecuteReader();
            }
        }
    }
}
