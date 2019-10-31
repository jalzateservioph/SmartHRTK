using System;
using System.Collections.Generic;

namespace TKProcessor.Models.TK
{
    public partial class GlobalSetting :Entity
    {
        public DateTime DefaultNDStart { get; set; }
        public DateTime DefaultNDEnd { get; set; }
        public bool CreateDTRForNoWorkDays { get; set; }

        public ICollection<Mapping> PayrollCodeMappings { get; set; }
        public ICollection<Mapping> PayPackageMappings { get; set; }
    }
}
