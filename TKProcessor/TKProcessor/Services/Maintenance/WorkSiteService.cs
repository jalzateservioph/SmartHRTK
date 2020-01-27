using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.TK;

namespace TKProcessor.Services.Maintenance
{
    public class WorkSiteService : BasicTimekeepingService<WorkSite>
    {
        readonly TKContext context;

        public WorkSiteService()
        {
            context = new TKContext();
        }

        public WorkSiteService(TKContext context)
        {
            this.context = context;
        }

        public IQueryable<WorkSite> Get()
        {
            return context.WorkSite.AsNoTracking();
        }

        public void Add(WorkSite entity)
        {
            try
            {
                context.WorkSite.Add(entity);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Update(Guid id, WorkSite entity)
        {
            try
            {
                var existing = context.WorkSite.Find(id);

                context.Entry(existing).CurrentValues.SetValues(entity);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                var entity = context.WorkSite.Find(id);

                context.WorkSite.Remove(entity);
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }

        public int Save()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                CreateErrorLog(ex);

                throw ex;
            }
        }
    }
}
