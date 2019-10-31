using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Common;
using TKProcessor.Contexts;
using TKProcessor.Models;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public class HolidayService : TKService<Holiday>, IExportTemplate
    {
        readonly string[] columns;

        public HolidayService() : base()
        {
            var exclude1 = typeof(IEntity).GetProperties().Select(i => i.Name);
            var exclude2 = typeof(IModel).GetProperties().Select(i => i.Name);
            var entity = typeof(Holiday).GetProperties().Select(i => i.Name);
            var include = entity.Where(s => !exclude1.Any(e => e == s) && !exclude2.Any(e => e == s));

            columns = include.ToArray();
        }

        public IEnumerable<Holiday> GetHolidays(DateTime date)
        {
            var holidays = Context.Holiday.Where(i => i.Date == date).Select(i => i);

            return holidays.AsEnumerable();
        }

        public override void Save(Holiday entity)
        {
            try
            {
                var existing = Context.Holiday.FirstOrDefault(i => i.Name == entity.Name &&
                                                                   i.Type == entity.Type &&
                                                                   i.Date == entity.Date);

                if (existing != default(Holiday))
                    entity.Id = existing.Id;

                base.Save(entity);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public override void DeleteHard(Holiday entity)
        {
            try
            {
                using (TKContext context = new TKContext())
                {
                    context.Holiday.Remove(entity);

                    if (AutoSaveChanges)
                        context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Import(string filename, Action<WorkSchedule> iterationCallback)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"{filename} was not found.");

            try
            {
                var data = ExcelFileHander.Import(filename);

                foreach (DataColumn col in data.Columns)
                {
                    if (!columns.Any(i => i == col.ColumnName))
                        throw new FormatException("Invalid work schedule file format");
                }

                var types = Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                                .Select(i => i.ToString().ToLower()).ToList();

                foreach (DataRow row in data.Rows)
                {
                    foreach (var typeToken in row[1].ToString().Split('/'))
                    {
                        int type = 0;

                        if (int.TryParse(typeToken, out int outType))
                        {
                            type = outType;
                        }
                        else
                        {
                            type = types.IndexOf(typeToken.ToLower());
                        }

                        Save(new Holiday()
                        {
                            Name = row[0].ToString(),
                            Type = type,
                            Date = DateTime.Parse(row[2].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void ExportTemplate(string filename)
        {
            try
            {
                ExcelFileHander.Export(filename, columns);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }
    }
}
