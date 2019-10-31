using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Models.TK
{
    public class ImportLog : IModel
    {
        public ImportLog()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Target { get; set; }
        public string Record { get; set; }
        public string Remarks { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
