using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid? DPUserId { get; set; }
    }
}
