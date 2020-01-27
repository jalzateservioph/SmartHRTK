using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

    public class ErrorLogBase
    {
        protected void CreateErrorLog(Exception ex, [CallerMemberName]string source = "")
        {
            using (TKContext Context = new TKContext())
            {
                Context.ErrorLog.Add(new ErrorLog(ex, source));
                Context.SaveChanges();
            }
        }
    }

    public interface IBasicTimekeepingService<T> where T : class
    {
        void CreateAuditLog(T newValue);
        void CreateAuditLog(T newValue, T oldValue);
        void CreateErrorLog(Exception ex, [CallerMemberName] string source = "");
        void CreateErrorLog(string message, [CallerMemberName] string source = "");
    }

    public class BasicTimekeepingService<T> : IBasicTimekeepingService<T> where T : class
    {
        protected string user;

        protected TKContext context;

        public BasicTimekeepingService()
        {
            context = new TKContext();
        }

        public BasicTimekeepingService(Guid userId) : this()
        {
            this.user = GetUserName(userId);
        }

        public BasicTimekeepingService(TKContext context)
        {
            this.context = context;
        }

        public BasicTimekeepingService(TKContext context, Guid userId)
        {
            this.context = context;
            this.user = GetUserName(userId);
        }

        /// <summary>
        /// Validates and retrieves the user Id of the user with the id userId
        /// </summary>
        /// <param name="userId">Id of the application user</param>
        /// <returns></returns>
        protected string GetUserName(Guid userId)
        {
            using (TKContext ctx = new TKContext())
            {
                User user = ctx.User.Find(userId);

                if (user == null)
                    user = ctx.User.First();

                return user.Name;
            }
        }

        public virtual void CreateErrorLog(Exception ex, [CallerMemberName]string source = "")
        {
            using (var ctx = new TKContext())
            {
                ctx.ErrorLog.Add(new ErrorLog(ex, source));
                ctx.SaveChanges();
            }
        }

        public virtual void CreateErrorLog(string message, [CallerMemberName]string source = "")
        {
            using (var ctx = new TKContext())
            {
                ctx.ErrorLog.Add(new ErrorLog(message, source));
                ctx.SaveChanges();
            }
        }

        public void CreateAuditLog(T newValue)
        {
            CreateAuditLog(newValue, null);
        }

        public virtual void CreateAuditLog(T newValue, T oldValue)
        {
            try
            {
                var log = new AuditLog()
                {
                    Target = $"{typeof(T).Name}",
                    Action = newValue == null ? "delete" : oldValue == null ? "Create" : "Update",
                    OldValue = JsonConvert.SerializeObject(oldValue),
                    NewValue = JsonConvert.SerializeObject(newValue),
                    ModifiedBy = user
                };

                using (var ctx = new TKContext())
                {
                    ctx.AuditLog.Add(log);

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class TimekeepingService<T> : IDisposable
        where T : class, IEntity, new()
    {
        protected string user;

        protected TKContext context;

        public TimekeepingService()
        {
            context = new TKContext();
        }

        public TimekeepingService(Guid userId) : this()
        {
            user = GetUserName(userId);
        }

        public TimekeepingService(TKContext context)
        {
            this.context = context;
        }

        public TimekeepingService(TKContext context, Guid userId)
        {
            this.context = context;
            this.user = GetUserName(userId);
        }

        /// <summary>
        /// Validates and retrieves the user Id of the user with the id userId
        /// </summary>
        /// <param name="userId">Id of the application user</param>
        /// <returns></returns>
        protected string GetUserName(Guid userId)
        {
            User user = context.User.Find(userId);

            if (user == null)
                user = context.User.First();

            return user.Name;
        }

        public void CreateErrorLog(Exception ex, [CallerMemberName]string source = "")
        {
            context.ErrorLog.Add(new ErrorLog(ex, source));
            context.SaveChanges();
        }

        public void CreateErrorLog(string message, [CallerMemberName]string source = "")
        {
            context.ErrorLog.Add(new ErrorLog(message, source));
            context.SaveChanges();
        }

        public void CreateAuditLog(T newValue)
        {
            CreateAuditLog(newValue, null);
        }

        public void CreateAuditLog(T newValue, T oldValue)
        {
            try
            {
                var log = new AuditLog()
                {
                    Target = $"{typeof(T).Name}",
                    Action = newValue == null ? "delete" : oldValue == null ? "Create" : "Update",
                    OldValue = JsonConvert.SerializeObject(oldValue, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    NewValue = JsonConvert.SerializeObject(newValue, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                    ModifiedBy = user
                };

                context.AuditLog.Add(log);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual IQueryable<T> List()
        {
            return context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.CreatedBy = entity.LastModifiedBy = string.IsNullOrEmpty(entity.CreatedBy) ? user : entity.CreatedBy;
            entity.CreatedOn = entity.LastModifiedOn = DateTime.Now;

            CreateAuditLog(entity);

            context.Set<T>().Add(entity);
        }

        public virtual void Update(Guid id, T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            T existing = context.Set<T>().Find(id);

            if (existing != null)
            {
                entity.Id = existing.Id;
                entity.LastModifiedBy = string.IsNullOrEmpty(entity.CreatedBy) ? user : entity.CreatedBy;
                entity.LastModifiedOn = DateTime.Now;

                CreateAuditLog(entity, existing);

                context.Entry(existing).CurrentValues.SetValues(entity);
            }
            else
                throw new Exception("Record does not exist");
        }

        public virtual void Save(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Update(entity.Id, entity);
            }
            catch
            {
                Add(entity);
            }
        }

        public virtual void SetState(Guid id, bool isActive)
        {
            T existing = context.Set<T>().Find(id);

            if (existing != null)
            {
                existing.LastModifiedBy = string.IsNullOrEmpty(existing.CreatedBy) ? user : existing.CreatedBy;
                existing.LastModifiedOn = DateTime.Now;

                existing.IsActive = isActive;
            }
            else
                throw new Exception("Record does not exist");
        }

        public virtual void Delete(Guid id)
        {
            T existing = context.Set<T>().Find(id);

            if (existing != null)
                context.Set<T>().Remove(existing);
            else
                throw new Exception("Record does not exist");
        }

        public virtual int SaveChanges()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
    }

    /// <summary>
    /// old. will be replaced
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TKService<T> : IDisposable
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

        public TKService(TKContext context)
        {
            AutoSaveChanges = true;
            UseDefaultUser = true;

            this.Context = context;

            logs = new List<AuditLog>();
        }

        public TKService(Guid userId) : this()
        {
            CurrentUser = Context.User.First(i => i.Id == userId);
        }

        public TKService(Guid userId, TKContext context) : this(context)
        {
            CurrentUser = Context.User.First(i => i.Id == userId);
        }

        public virtual void Delete(T entity)
        {
            var existing = Context.Set<T>().Find(entity.Id);

            if (existing != default(T))
            {
                var tempex = existing;

                existing.IsActive = false;

                CreateAuditLog(tempex, existing);

                if (AutoSaveChanges)
                    SaveChanges();
            }
        }

        public virtual void DeleteHard(T entity)
        {
            var existing = Context.Set<T>().Find(entity.Id);

            CreateAuditLog(null, existing);

            Context.Set<T>().Remove(existing);

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

            CurrentUser = Context.User.Find(CurrentUser.Id);

            if (existing == default(T))
            {
                entity.IsActive = true;
                entity.CreatedBy = CurrentUser.ToString();
                entity.CreatedOn = DateTime.Now;
                entity.LastModifiedBy = CurrentUser.ToString();
                entity.LastModifiedOn = DateTime.Now;

                CreateAuditLog(entity);

                Context.Set<T>().Add(entity);
            }
            else
            {
                entity.Id = existing.Id;
                entity.IsActive = true;
                entity.LastModifiedBy = CurrentUser.ToString();
                entity.LastModifiedOn = DateTime.Now;

                CreateAuditLog(entity, existing);

                Context.Entry(existing).CurrentValues.SetValues(entity);
            }

            if (AutoSaveChanges)
                SaveChanges();
        }

        public virtual void Save(T existing, T entity)
        {
            CurrentUser = Context.User.Find(CurrentUser.Id);

            if (existing == default(T))
            {
                entity.IsActive = true;
                entity.CreatedBy = CurrentUser.ToString();
                entity.CreatedOn = DateTime.Now;
                entity.LastModifiedBy = CurrentUser.ToString();
                entity.LastModifiedOn = DateTime.Now;

                CreateAuditLog(entity);

                Context.Set<T>().Add(entity);
            }
            else
            {
                entity.Id = existing.Id;
                entity.IsActive = true;
                entity.LastModifiedBy = CurrentUser.ToString();
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
            try
            {
                var log = new AuditLog()
                {
                    Target = $"{typeof(T).Name}",
                    Action = newValue == null ? "delete" : oldValue == null ? "Create" : "Update",
                    OldValue = JsonConvert.SerializeObject(oldValue),
                    NewValue = JsonConvert.SerializeObject(newValue),
                    ModifiedBy = CurrentUser?.Name ?? "admin",
                };

                if (AutoSaveChanges)
                {
                    Context.AuditLog.Add(log);

                    Context.SaveChanges();
                }
                else
                {
                    logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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