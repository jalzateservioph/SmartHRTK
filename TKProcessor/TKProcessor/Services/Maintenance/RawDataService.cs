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
using TKProcessor.Contexts;

namespace TKProcessor.Services.Maintenance
{
    public class RawDataService : TKService<RawData>, IExportTemplate
    {
        private WorkScheduleService workScheduleService;

        WorkSchedule[] workSchedules = null;

        IEnumerable<string> types;

        public IList<string[]> templateSetup;

        public RawDataService(Guid id, TKContext context) : base(id, context)
        {
            workScheduleService = new WorkScheduleService(id, context);

            Init();
        }

        public RawDataService(Guid id) : base(id)
        {
            workScheduleService = new WorkScheduleService(id);

            Init();
        }

        private void Init()
        {
            var exclude1 = typeof(IEntity).GetProperties().Select(i => i.Name);
            var exclude2 = typeof(IModel).GetProperties().Select(i => i.Name);
            var shift = typeof(RawData).GetProperties().Select(i => i.Name);
            var include = shift.Where(s => !exclude1.Any(e => e == s) && !exclude2.Any(e => e == s));

            workSchedules = workScheduleService.List().ToArray();

            types = Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                                    .Select(i => i.ToString().ToLower()).ToList();

            templateSetup = new List<string[]>
            {
                new string[]
                {
                    "BiometricsId",
                    "ScheduleDate",
                    "In",
                    "Out"
                }
            };

            templateSetup.Add(include.ToArray());
        }

        public void ExportTemplate(string filename)
        {
            var folder = Path.GetDirectoryName(filename);
            var name = Path.GetFileNameWithoutExtension(filename);

            ExcelFileHandler.Export(folder + "\\" + name + "_InOut.xlsx", templateSetup[0]);
            ExcelFileHandler.Export(folder + "\\" + name + ".xlsx", templateSetup[1]);
        }

        public override void Save(RawData entity)
        {
            //Context = new TKContext();

            var existing = List().FirstOrDefault(i => i.BiometricsId == entity.BiometricsId &&
                                                    i.TransactionType == entity.TransactionType &&
                                                    i.ScheduleDate == entity.ScheduleDate);

            if (entity.ScheduleDate.Date != entity.TransactionDateTime.Date)
                entity = SetScheduleDate(entity);

            base.Save(existing, entity);
        }

        public void Import(string filename, Action<RawData> iterationCallback)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"{filename} was not found.");

            try
            {

                var ds = ExcelFileHandler.ImportFile(filename);

                foreach (DataTable data in ds.Tables)
                {
                    string[] columns = null;

                    foreach (var columnSetup in templateSetup)
                    {
                        var cols = data.Columns.Cast<DataColumn>().Select(i => i.ColumnName).ToArray();

                        if (cols.Count() != columnSetup.Count())
                                continue;

                        for (int a = 0; a < cols.Count(); a++)
                        {
                            if (cols[a] != columnSetup[a])
                                continue;
                        }

                        columns = columnSetup;
                        break;
                    }

                    if(columns == null)
                        throw new FormatException("Invalid biometrics raw data file format");

                    var validBiometricsId = Context.Employee.Select(i => i.BiometricsId).ToList();

                    var dbRecords = Context.RawData.ToList();

                    var types = Enum.GetValues(typeof(HolidayType)).Cast<HolidayType>()
                                    .Select(i => i.ToString().ToLower()).ToList();

                    for (int rowCounter = 0; rowCounter < data.Rows.Count; rowCounter++)
                    {
                        DataRow row = data.Rows[rowCounter];

                        if (validBiometricsId.Any(i => i == row[columns[0]].ToString()))
                        {
                            if (templateSetup.IndexOf(columns) == 1)
                                ImportNormal(row, columns, iterationCallback);
                            else
                                ImportInOutFormat(row, columns, iterationCallback);
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

        private void ImportNormal(DataRow row, string[] columns, Action<RawData> iterationCallback)
        {
            try
            {
                var importedRawData = new RawData()
                {
                    BiometricsId = row[columns[0]].ToString(),

                    TransactionType = string.Compare(row[columns[1]].ToString(), "in", true) == 0 ?
                                            1 : string.Compare(row[columns[1]].ToString(), "out", true) == 0 ?
                                                2 : int.Parse(row[columns[1]].ToString()),
                };

                if (DateTime.TryParse(row[columns[2]].ToString(), out DateTime dt))
                {
                    importedRawData.TransactionDateTime = DateTime.Parse(row[columns[2]].ToString());
                }
                else
                {
                    importedRawData.TransactionDateTime = DateTime.FromOADate(double.Parse(row[columns[2]].ToString()));
                }

                if (string.IsNullOrEmpty(row[columns[3]].ToString()))
                {
                    importedRawData.ScheduleDate = importedRawData.TransactionDateTime;
                }
                else if (DateTime.TryParse(row[columns[3]].ToString(), out dt))
                {
                    importedRawData.ScheduleDate = string.IsNullOrEmpty(row[columns[3]].ToString()) ?
                                                    DateTime.Parse(row[columns[2]].ToString()).Date :
                                                    DateTime.Parse(row[columns[3]].ToString()).Date;
                }
                else
                {
                    importedRawData.TransactionDateTime = DateTime.FromOADate(double.Parse(row[columns[3]].ToString()));
                }

                Save(importedRawData);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid format template");
            }
        }

        private void ImportInOutFormat(DataRow row, string[] columns, Action<RawData> iterationCallback)
        {

            try
            {
                var schedDate = DateTimeHelpers.Parse(row[columns[1]]);
                var timein = DateTimeHelpers.Parse(row[columns[2]]);
                var timeout = DateTimeHelpers.Parse(row[columns[3]]);

                if (schedDate != null && timein != null)
                {
                    var rawDataIn = new RawData()
                    {
                        BiometricsId = row[columns[0]].ToString(),
                        TransactionType = 1,
                        ScheduleDate = schedDate.Value,
                    };

                    Save(rawDataIn);
                }

                if (schedDate != null && timeout != null)
                {
                    var rawDataOut = new RawData()
                    {
                        BiometricsId = row[columns[0]].ToString(),
                        TransactionType = 1
                    };

                    if (timein.HasValue && timeout.Value > timein.Value)
                        rawDataOut.TransactionDateTime = DateTimeHelpers.ConstructDate(schedDate.Value.AddDays(1), timeout.Value);
                    else
                        rawDataOut.TransactionDateTime = DateTimeHelpers.ConstructDate(schedDate.Value, timeout.Value);

                    Save(rawDataOut);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid format template");
            }
        }

        public RawData SetScheduleDate(RawData rawData)
        {
            rawData.ScheduleDate = rawData.TransactionDateTime.Date; //by default
            if (rawData.TransactionType == (int)TransactionType.TimeIn)
            {
                WorkSchedule ws = workSchedules.Where(e => e.Employee.BiometricsId == rawData.BiometricsId && e.ScheduleDate == rawData.TransactionDateTime.Date.AddDays(-1)).FirstOrDefault();

                if (ws != null)
                {
                    if (ws.Shift.ScheduleIn.HasValue && ws.Shift.ScheduleOut.HasValue)
                    {
                        if (ws.Shift.ScheduleOut.Value < ws.Shift.ScheduleIn.Value) //splittable
                        {
                            DateTime actualScheduleOut = new DateTime(rawData.TransactionDateTime.Year, rawData.TransactionDateTime.Month, rawData.TransactionDateTime.Day).Add(ws.Shift.ScheduleOut.Value.TimeOfDay).RemoveSeconds();

                            if (actualScheduleOut > rawData.TransactionDateTime)
                            {
                                rawData.ScheduleDate = rawData.TransactionDateTime.Date.AddDays(-1);
                            }
                        }
                    }
                }
            }
            else if (rawData.TransactionType == (int)TransactionType.TimeOut)
            {
                WorkSchedule ws_t = workSchedules.Where(e => e.Employee.BiometricsId == rawData.BiometricsId && e.ScheduleDate == rawData.TransactionDateTime.Date).FirstOrDefault();

                if (ws_t != null)
                {
                    if (ws_t.Shift.ScheduleIn.HasValue && ws_t.Shift.ScheduleOut.HasValue) //Splittable
                    {
                        DateTime actualScheduleIn = new DateTime(rawData.TransactionDateTime.Year, rawData.TransactionDateTime.Month, rawData.TransactionDateTime.Day).Add(ws_t.Shift.ScheduleIn.Value.TimeOfDay).RemoveSeconds();

                        if (actualScheduleIn > rawData.TransactionDateTime)
                        {
                            WorkSchedule ws = workSchedules.Where(e => e.Employee.BiometricsId == rawData.BiometricsId && e.ScheduleDate == rawData.TransactionDateTime.Date.AddDays(-1)).FirstOrDefault();
                            if (ws != null)
                            {
                                if (ws.Shift.ScheduleIn.HasValue && ws.Shift.ScheduleOut.HasValue)
                                {
                                    if (ws.Shift.ScheduleOut.Value < ws.Shift.ScheduleIn.Value) //splittable
                                    {
                                        rawData.ScheduleDate = rawData.TransactionDateTime.Date.AddDays(-1);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return rawData;
        }
    }
}
