using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Common
{
    public static class DataTableHelpers
    {
        public static DataTable CreateTable(Type type)
        {
            DataTable table = new DataTable();

            foreach (var propInfo in type.GetProperties())
            {
                table.Columns.Add(new DataColumn(propInfo.Name, propInfo.PropertyType));
            }

            return table;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
            where T : class, new()
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            DataTable table = new DataTable();

            foreach (var propInfo in typeof(T).GetProperties())
            {
                table.Columns.Add(new DataColumn(propInfo.Name, propInfo.PropertyType));
            }

            foreach (var item in data)
            {
                var row = table.NewRow();

                foreach (var propInfo in typeof(T).GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable ToStringDataTable<T>(IEnumerable<T> data)
            where T : class, new()
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            DataTable table = new DataTable();

            foreach (var propInfo in typeof(T).GetProperties())
            {
                table.Columns.Add(new DataColumn(propInfo.Name, typeof(string)));
            }

            foreach (var item in data)
            {
                var row = table.NewRow();

                foreach (var propInfo in typeof(T).GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item) ?? "";
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
