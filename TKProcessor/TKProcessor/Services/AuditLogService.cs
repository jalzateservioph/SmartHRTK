using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Contexts;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public class AuditLogService
    {
        readonly TKContext ctx;

        public AuditLogService()
        {
            ctx = new TKContext();
        }

        public IEnumerable<AuditLog> Get()
        {
            return ctx.AuditLog;
        }
    }
}
