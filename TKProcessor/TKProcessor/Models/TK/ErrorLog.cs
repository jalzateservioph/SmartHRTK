using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TKProcessor.Models.TK
{
    public partial class ErrorLog : IEntity
    {
        public ErrorLog()
        {
            Id = Guid.NewGuid();
            DateRaised = DateTime.Now;
        }

        public ErrorLog(string source, Exception ex) : this()
        {
            Source = source;
            StackTrace = ex.StackTrace;
            Message = ex.Message;
        }

        public ErrorLog(Exception ex, [CallerMemberName]string source = "") : this()
        {
            Source = source;
            StackTrace = ex.StackTrace;
            Message = ex.Message;
        }

        public ErrorLog(string message, [CallerMemberName]string source = "") : this()
        {
            Source = source;
            Message = message;
        }

        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }


        public string Source { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateRaised { get; set; }
    }
}
