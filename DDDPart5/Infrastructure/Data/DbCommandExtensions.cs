using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Infrastructure.Data
{
    public static class DbCommandExtensions
    {
        public static void AddParameter(this IDbCommand cmd, string name, object value)
        {
            var par = cmd.CreateParameter();
            par.ParameterName = name;
            par.Value = value;
            cmd.Parameters.Add(par);
        }
    }
}
