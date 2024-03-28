namespace Bars.Gkh.Gis.Reports.Utils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    public static class ReportUtils
    {
        public static DataTable ToDataTable<T>(IList<T> data, string name = "")
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable(name);
            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            var values = new object[props.Count];
            foreach (var item in data)
            {
                for (var i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        public static string ReplaceParams(string sql, Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return sql;
            }

            return parameters.Aggregate(sql, (current, parameter) => current.Replace("{" + parameter.Key + "}", parameter.Value));
        }

        
    }
}
