using System;

namespace TKProcessor.WPF.Models
{
    public partial class ErrorLog
    {
        public string Source { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime DateRaised { get; set; }
    }
}
