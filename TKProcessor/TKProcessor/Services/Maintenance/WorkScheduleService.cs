using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

namespace TKProcessor.Services.Maintenance
{
    public class WorkScheduleService : TimekeepingService<WorkSchedule>, IExportTemplate
    {
        readonly List<List<string>> headerDef;

        Action<string> iterationCallback;

        public WorkScheduleService(Guid userId) : base(userId)
        {
            headerDef = new List<List<string>>()
            {
                new List<string>()
                {
                    "Employee Code",
                    "Schedule Date",
                    "Shift Code"
                },
                new List<string>()
                {
                    "Employee Code",
                    "From",
                    "To",
                    "M", "T", "W", "Th", "F", "Sa", "Su"
                },
                new List<string>()
                {
                    "Employee Code",
                    "{datetime}"
                }
            };
        }

        public WorkScheduleService(Guid userId, TKContext context) : base(context, userId)
        {
            headerDef = new List<List<string>>()
            {
                new List<string>()
                {
                    "Employee Code",
                    "Schedule Date",
                    "Shift Code"
                },
                new List<string>()
                {
                    "Employee Code",
                    "From",
                    "To",
                    "M", "T", "W", "Th", "F", "Sa", "Su"
                },
                new List<string>()
                {
                    "Employee Code",
                    "{datetime}"
                }
            };
        }

        public override void Save(WorkSchedule entity)
        {
            var existing = context.WorkSchedule.Find(entity.Id);

            if (existing == null)
            {
                existing = context.WorkSchedule.FirstOrDefault(i => i.Employee.Id == entity.Employee.Id &&
                                                                    i.ScheduleDate == entity.ScheduleDate);
            }

            if (existing == null)
            {
                entity.IsActive = true;
                entity.CreatedBy = user;
                entity.CreatedOn = DateTime.Now;
                entity.LastModifiedBy = user;
                entity.LastModifiedOn = DateTime.Now;

                entity.Employee = context.Employee.First(i => i.Id == entity.Employee.Id);

                entity.Shift = context.Shift.First(i => i.Id == entity.Shift.Id);

                CreateAuditLog(entity);

                context.WorkSchedule.Add(entity);
            }
            else
            {
                existing.LastModifiedBy = user;
                existing.LastModifiedOn = DateTime.Now;

                existing.Employee = context.Employee.First(i => i.Id == entity.Employee.Id);

                existing.ScheduleDate = entity.ScheduleDate;

                existing.Shift = context.Shift.First(i => i.Id == entity.Shift.Id);

                CreateAuditLog(entity, existing);
            }
        }

        public void Import(string filename, Action<string> iterationCallback)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException($"{filename} was not found.");

            this.iterationCallback = iterationCallback;

            try
            {
                var data = ExcelFileHandler.Import(filename);

                if (TryGetDefinition(data, out int defIndex))
                {
                    if (defIndex == 0) ParseFlat(data);
                    else if (defIndex == 1) ParseSummary(data);
                    else if (defIndex == 2) ParsePlot(data);
                }
                else
                {
                    throw new FormatException("Invalid work schedule file format");
                }
            }
            catch (Exception ex)
            {
                context.ErrorLog.Add(new ErrorLog(nameof(WorkScheduleService.Import), ex));
                context.SaveChanges();

                throw ex;
            }
        }

        public override IQueryable<WorkSchedule> List()
        {
            return context.WorkSchedule
                            .Include(entity => entity.Employee)
                            .Include(entity => entity.Shift);
        }

        private void ParsePlot(DataTable data)
        {
            try
            {
                WorkSchedule ws = null;
                Employee currentEmp = null;

                foreach (DataRow row in data.Rows)
                {
                    bool isFirstCol = true;

                    foreach (var column in data.Columns.Cast<DataColumn>())
                    {
                        var value = row[column].ToString();

                        if (isFirstCol)
                        {
                            currentEmp = context.Employee.FirstOrDefault(i => i.EmployeeCode == value);

                            if (currentEmp == default(Employee))
                                break;

                            isFirstCol = false;
                        }
                        else
                        {
                            var scheduleDate = DateTime.Parse(column.ColumnName);
                            var shift = context.Shift.FirstOrDefault(i => i.ShiftCode == value);

                            if (shift == default(Shift))
                            {
                                iterationCallback?.Invoke($"No shift found for {currentEmp.EmployeeCode} - {scheduleDate.ToShortDateString()}");

                                continue;
                            }

                            ws = new WorkSchedule()
                            {
                                Employee = currentEmp,
                                ScheduleDate = scheduleDate,
                                Shift = shift
                            };

                            Save(ws);

                            iterationCallback?.Invoke($"{ws.Employee.EmployeeCode} - {ws.ScheduleDate.ToShortDateString()} - {ws.Shift.ShiftCode} ");
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

        private void ParseFlat(DataTable data)
        {
            try
            {
                foreach (DataRow row in data.Rows)
                {
                    var currentEmp = context.Employee.FirstOrDefault(i => i.EmployeeCode == row[headerDef[0][0]].ToString());

                    if (currentEmp == default(Employee))
                    {
                        iterationCallback?.Invoke($"Employee {row[headerDef[0][0]].ToString()} does not exist");

                        continue;
                    }

                    var scheduleDate = DateTime.Parse(row[headerDef[0][1]].ToString());

                    var shift = context.Shift.FirstOrDefault(i => i.ShiftCode == row[headerDef[0][2]].ToString());

                    if (shift == default(Shift))
                    {
                        iterationCallback?.Invoke($"No shift found for {currentEmp.EmployeeCode} - {scheduleDate.ToShortDateString()}");

                        continue;
                    }

                    var ws = new WorkSchedule()
                    {
                        Employee = currentEmp,
                        ScheduleDate = scheduleDate,
                        Shift = shift
                    };

                    Save(ws);

                    iterationCallback?.Invoke($"{ws.Employee.EmployeeCode} - {ws.ScheduleDate.ToShortDateString()} - {ws.Shift.ShiftCode} ");
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        private void ParseSummary(DataTable data)
        {
            try
            {
                // import
                foreach (DataRow row in data.Rows)
                {
                    var currentEmp = context.Employee.FirstOrDefault(i => i.EmployeeCode == row["Employee Code"].ToString());

                    if (currentEmp == default(Employee))
                    {
                        iterationCallback?.Invoke($"Employee {row["Employee Code"].ToString()} does not exist");

                        continue;
                    }

                    DateTime startDate = DateTime.Parse(row["From"].ToString());
                    DateTime endDate = DateTime.Parse(row["To"].ToString());

                    while (startDate <= endDate)
                    {
                        var index = (int)startDate.DayOfWeek;

                        index += index == 0 ? 9 : 2;

                        var shift = context.Shift.FirstOrDefault(i => i.ShiftCode == row[index].ToString());

                        if (shift == default(Shift))
                        {
                            iterationCallback?.Invoke($"No shift found for {currentEmp.EmployeeCode} - {startDate.ToShortDateString()}");

                            startDate = startDate.AddDays(1);

                            continue;
                        }

                        WorkSchedule ws = new WorkSchedule()
                        {
                            Employee = currentEmp,
                            ScheduleDate = startDate,
                            Shift = shift
                        };

                        Save(ws);

                        iterationCallback?.Invoke($"{ws.Employee.EmployeeCode} - {ws.ScheduleDate.ToShortDateString()} - {ws.Shift.ShiftCode} ");

                        startDate = startDate.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        private bool TryGetDefinition(DataTable data, out int defIndex)
        {
            try
            {
                bool isValid = true;

                List<string> cols = data.Columns.Cast<DataColumn>().Select(i => i.ColumnName).ToList();

                for (int i = 0; i < headerDef.Count; i++)
                {
                    List<string> def = headerDef[i];

                    if (def.Last() == "{datetime}")
                    {
                        while (def.Count < cols.Count)
                            def.Add("{datetime}");
                    }

                    if (def.Count != cols.Count)
                        continue;

                    for (int defIdx = 0; defIdx < def.Count; defIdx++)
                    {
                        if (def[defIdx] == "{datetime}")
                        {
                            if (!DateTime.TryParse(cols[defIdx], out DateTime d))
                            {
                                isValid = false;
                                break;
                            }
                        }
                        else
                        {
                            if (def[defIdx] != cols[defIdx])
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }

                    if (isValid)
                    {
                        defIndex = i;
                        return true;
                    }
                }

                defIndex = -1;

                return false;
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void ExportTemplate(string filename)
        {
            //ExcelFileHander.Export(filename, headerDef[templateType]);
        }

        public List<List<string>> Columns => headerDef;
    }
}
