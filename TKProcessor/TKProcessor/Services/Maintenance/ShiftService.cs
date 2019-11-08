using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Models;
using TKProcessor.Models.TK;

namespace TKProcessor.Services.Maintenance
{
    public class ShiftService : TKService<Shift>, IExportTemplate
    {
        private readonly string[] columns;

        public ShiftService(Guid userId) : base(userId)
        {
            var exclude1 = typeof(IEntity).GetProperties().Select(i => i.Name);
            var exclude2 = typeof(IModel).GetProperties().Select(i => i.Name);
            var shift = typeof(Shift).GetProperties().Select(i => i.Name);
            var include = shift.Where(s => !exclude1.Any(e => e == s) && !exclude2.Any(e => e == s));

           columns = include.ToArray(); 
        }

        public override void Save(Shift entity)
        {
            try
            {
                var existing = Context.Shift.FirstOrDefault(i => i.Id == entity.Id || i.ShiftCode == entity.ShiftCode);

                if (existing != default(Shift))
                    entity.Id = existing.Id;

                base.Save(entity);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }


        public void ExportTemplate(string filename)
        {
            ExcelFileHandler.Export(filename, columns);
        }

        public void Import(string filename, Action<WorkSchedule> iterationCallback)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"{filename} was not found.");

            var data = ExcelFileHandler.Import(filename);

            var shiftProperties = typeof(Shift).GetProperties();

            foreach (DataColumn col in data.Columns)
            {
                if (!shiftProperties.Any(i => i.Name == col.ColumnName))
                    throw new FormatException("Invalid employee shift file format");
            }

            Shift tempShift = null;

            try
            {
                foreach (DataRow row in data.Rows)
                {
                    tempShift = new Shift();

                    foreach (DataColumn col in data.Columns)
                    {
                        var prop = shiftProperties.First(i => i.Name == col.ColumnName);

                        var value = row[col].ToString();

                        if (value.ToString() == "" || value == "NULL")
                        {
                            prop.SetValue(tempShift, ObjectHelpers.GetDefault(prop.PropertyType));
                        }
                        else if ((prop.PropertyType == typeof(DateTime)) || (prop.PropertyType == typeof(DateTime?)))
                        {
                            if (DateTime.TryParse(value.ToString(), out DateTime result))
                                prop.SetValue(tempShift, result);
                            else
                                prop.SetValue(tempShift, DateTime.FromOADate(Convert.ToDouble(value.ToString())));
                        }
                        else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
                        {
                            prop.SetValue(tempShift, value.ToString() == "1" || value.ToString() == "y" || value.ToString() == "yes");
                        }
                        else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                        {
                            prop.SetValue(tempShift, ObjectHelpers.GetNullableInt(value.ToString()));
                        }
                        else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                        {
                            prop.SetValue(tempShift, ObjectHelpers.GetNullableDecimal(value.ToString()));
                        }
                        else
                        {
                            prop.SetValue(tempShift, value);
                        }
                    }

                    Save(tempShift);
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }
    }
}
