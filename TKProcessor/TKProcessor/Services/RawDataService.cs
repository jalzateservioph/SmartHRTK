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

namespace TKProcessor.Services
{
    public class RawDataService : TKService<RawData>, IExportTemplate
    {
        readonly string[] columns;

        public RawDataService()
        {
            var exclude1 = typeof(IEntity).GetProperties().Select(i => i.Name);
            var exclude2 = typeof(IModel).GetProperties().Select(i => i.Name);
            var shift = typeof(RawData).GetProperties().Select(i => i.Name);
            var include = shift.Where(s => !exclude1.Any(e => e == s) && !exclude2.Any(e => e == s));

            columns = include.ToArray();
        }

        public void ExportTemplate(string filename)
        {
            ExcelFileHandler.Export(filename, columns);
        }

        public new void Save(RawData entity)
        {
            var existing = List().FirstOrDefault(i => i.BiometricsId == entity.BiometricsId &&
                                                    i.TransactionType == entity.TransactionType &&
                                                    i.ScheduleDate == entity.ScheduleDate);

            if (existing != default(RawData))
                entity.Id = existing.Id;

            base.Save(entity);
        }

        public void Import(string filename, Action<RawData> iterationCallback)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"{filename} was not found.");

            try
            {
                var data = ExcelFileHandler.Import(filename);

                var cols = data.Columns.Cast<DataColumn>().Select(i => i.ColumnName).ToArray();

                if (cols.Count() != columns.Count())
                    throw new FormatException("Invalid biometrics raw data file format");

                for (int a = 0; a < cols.Count(); a++)
                {
                    if (cols[a] != columns[a])
                        throw new FormatException("Invalid biometrics raw data file format");
                }

                List<RawData> entries = new List<RawData>();

                var validBiometricsId = Context.Employee.Select(i => i.BiometricsId).ToList();

                var dbRecords = Context.RawData.ToList();

                var types = Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                               .Select(i => i.ToString().ToLower()).ToList();

                foreach (DataRow row in data.Rows)
                {
                    if (validBiometricsId.Any(i => i == row[columns[0]].ToString()))
                    {
                        try
                        {
                            var importedRawData = new RawData()
                            {
                                BiometricsId = row[columns[0]].ToString(),

                                TransactionType = row[columns[1]].ToString().ToLower() == "in" ? 1 :
                                                     row[columns[1]].ToString().ToLower() == "out" ? 2 :
                                                        int.Parse(row[columns[1]].ToString()),

                                TransactionDateTime = DateTime.Parse(row[columns[2]].ToString()),

                                ScheduleDate = string.IsNullOrEmpty(row[columns[3]].ToString()) ?
                                                    DateTime.Parse(row[columns[2]].ToString()).Date :
                                                    DateTime.Parse(row[columns[3]].ToString()).Date
                            };

                            Save(importedRawData);
                        }
                        catch
                        {
                            throw new Exception("Invalid format template");
                        }
                    }
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
