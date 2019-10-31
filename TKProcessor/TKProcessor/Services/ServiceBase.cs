using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public interface IExportTemplate
    {
        void ExportTemplate(string filename);
    }

    public interface IService<T>
        where T : class, new()
    {
        IEnumerable<T> List();
        void Save(T entity);
        void Delete(T entity);
        void DeleteHard(T entity);
        void SaveChanges();
    }

    public class TKService<T> : IService<T>, IDisposable
        where T : class, IEntity, new()
    {
        private List<AuditLog> logs;

        public TKService()
        {
            AutoSaveChanges = true;
            UseDefaultUser = true;

            Context = new TKContext();

            logs = new List<AuditLog>();
        }

        public TKService(Guid userId) : this()
        {
            CurrentUser = Context.User.First(i => i.Id == userId);
        }

        public virtual void Delete(T entity)
        {
            entity.IsActive = false;

            Save(entity);
        }

        public virtual void DeleteHard(T entity)
        {
            Context.Set<T>().Remove(entity);

            if (AutoSaveChanges)
                SaveChanges();
        }

        public virtual IEnumerable<T> List()
        {
            return Context.Set<T>().ToList();
        }

        public virtual void Save(T entity)
        {
            var existing = Context.Set<T>().Find(entity.Id);

            if (existing == default(T))
            {
                entity.IsActive = true;
                entity.CreatedBy = CurrentUser;
                entity.CreatedOn = DateTime.Now;
                entity.LastModifiedBy = CurrentUser;
                entity.LastModifiedOn = DateTime.Now;

                CreateAuditLog(entity);

                Context.Set<T>().Add(entity);
            }
            else
            {
                entity.Id = existing.Id;
                entity.IsActive = true;
                entity.LastModifiedBy = CurrentUser;
                entity.LastModifiedOn = DateTime.Now;

                CreateAuditLog(entity, existing);

                Context.Entry(existing).CurrentValues.SetValues(entity);
            }

            if (AutoSaveChanges)
                SaveChanges();
        }

        public virtual void SaveChanges()
        {
            foreach (var item in logs)
                Context.AuditLog.Add(item);

            logs.Clear();

            Context.SaveChanges();
        }

        protected void CreateErrorLog(Exception ex, [CallerMemberName]string source = "")
        {
            Context.ErrorLog.Add(new ErrorLog(ex, source));
            Context.SaveChanges();
        }

        protected void CreateAuditLog(T newValue)
        {
            CreateAuditLog(newValue, null);
        }

        protected void CreateAuditLog(T newValue, T oldValue)
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                var oldVal = oldValue == null ? null : prop.GetValue(oldValue)?.ToString();
                var newVal = prop.GetValue(newValue)?.ToString();

                if (oldValue == null || oldVal != newVal)

                    logs.Add(new AuditLog()
                    {
                        Target = $"{typeof(T).Name}.{prop.Name}",
                        Action = oldValue == null ? "Create" : "Update",
                        OldValue = oldVal,
                        NewValue = newVal,
                        ModifiedBy = CurrentUser?.Name ?? "admin"
                    });
            }
        }

        public void Dispose()
        {
            Context.Dispose();

            logs.Clear();
            logs = null;
        }

        protected TKContext Context { get; set; }
        protected User CurrentUser { get; set; }
        public bool AutoSaveChanges { get; set; }
        public bool UseDefaultUser { get; set; }
    }

    public interface IReportService<T>
    {
        IEnumerable<T> GetData();
    }
}