using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Models.TK
{
    public class Mapping : IModel
    {
        public Mapping()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Target { get; set; }
        public string Source { get; set; }
    }
}
