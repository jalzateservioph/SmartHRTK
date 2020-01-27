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
    public class ShiftService : TimekeepingService<Shift>, IExportTemplate
    {
        public string[] Columns;

        public ShiftService(Guid userId) : base(userId)
        {
            var exclude1 = typeof(IEntity).GetProperties().Select(i => i.Name);
            var exclude2 = typeof(IModel).GetProperties().Select(i => i.Name);
            var shift = typeof(Shift).GetProperties().Select(i => i.Name);
            var include = shift.Where(s => !exclude1.Any(e => e == s) && !exclude2.Any(e => e == s));

            Columns = include.ToArray();
        }

        public override void Save(Shift entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                var existing = context.Shift.FirstOrDefault(i => i.IsActive && (i.Id == entity.Id || i.ShiftCode == entity.ShiftCode));

                if (existing == null)
                {
                    Add(entity);
                }
                else 
                {
                    Update(existing.Id, entity);
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
            ExcelFileHandler.Export(filename, Columns);
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

            try
            {
                foreach (var item  in data.CastAs<Shift>())
                {
                    Save(item);
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
