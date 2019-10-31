using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Models.TK;

namespace TKProcessor.Services
{
    public class ErrorLogService : TKService<ErrorLog>
    {
        public void Log(Exception ex, [CallerMemberName]string source = "")
        {
            Save(new ErrorLog(ex, source));
        }
    }
}
