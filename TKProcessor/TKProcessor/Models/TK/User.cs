using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.Models.TK
{
    public class User : IEntity
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Name;
        }

        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid? DPUserId { get; set; }
    }
}
