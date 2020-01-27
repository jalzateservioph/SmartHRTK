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

        public static DataTable CreateTable(IEnumerable<string> columns)
        {
            DataTable table = new DataTable();

            foreach (var col in columns)
            {
                table.Columns.Add(col);
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

        public static void RetainColumns(this DataTable data, IEnumerable<string> columns)
        {
            var dataCols = data.Columns.Cast<DataColumn>().Where(col => columns.Count(i => i == col.ColumnName) == 0).ToArray();

            foreach (var col in dataCols)
                data.Columns.Remove(col);
        }

        public static void RemoveColumns(this DataTable data, IEnumerable<string> columns)
        {
            var dataCols = data.Columns.Cast<DataColumn>().Where(col => columns.Count(i => i == col.ColumnName) == 1).ToArray();

            foreach (var col in dataCols)
                data.Columns.Remove(col);
        }

        public static IEnumerable<T> CastAs<T>(this DataTable data)
            where T : class, new()
        {
            foreach(DataRow row in data.Rows)
            {
                T entity = new T();

                entity.LoadValues(data.Columns, row);

                yield return entity;
            }
        }

        public static void LoadValues<T>(this T target, DataColumnCollection columns, DataRow row)
        {
            target.LoadValues(columns.Cast<DataColumn>(), row);
        }

        public static void LoadValues<T>(this T target, IEnumerable<DataColumn> columns, DataRow row)
        {
            foreach (DataColumn col in columns)
            {
                var prop = typeof(T).GetProperties().First(i => i.Name == col.ColumnName);

                var value = row[col].ToString();

                if (string.IsNullOrEmpty(value.ToString()) || string.Compare(value.ToString(), "NULL", true) == 0)
                {
                    prop.SetValue(target, ObjectHelpers.GetDefault(prop.PropertyType));
                }
                else if (Guid.TryParse(value.ToString(), out Guid id))
                {
                    prop.SetValue(target, id);
                }
                else if ((prop.PropertyType == typeof(DateTime)) || (prop.PropertyType == typeof(DateTime?)))
                {
                    if (DateTime.TryParse(value.ToString(), out DateTime result))
                    {
                        prop.SetValue(target, result);
                    }
                    else
                    {
                        prop.SetValue(target, DateTime.FromOADate(Convert.ToDouble(value.ToString())));
                    }
                }
                else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                {
                    prop.SetValue(target, string.Compare(value.ToString(), "1", true) == 0 || string.Compare(value.ToString(), "y", true) == 0 || string.Compare(value.ToString(), "yes", true) == 0);
                }
                else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                {
                    prop.SetValue(target, ObjectHelpers.GetNullableInt(value.ToString()));
                }
                else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    prop.SetValue(target, ObjectHelpers.GetNullableDecimal(value.ToString()));
                }
                else
                {
                    prop.SetValue(target, value);
                }
            }
        }
    }
}
