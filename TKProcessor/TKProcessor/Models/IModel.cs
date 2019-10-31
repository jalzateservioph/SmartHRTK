using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKProcessor.Models.TK;

namespace TKProcessor.Models
{
    public interface IModel
    {
        Guid Id { get; set; }
    }

    public interface IEntity : IModel
    {
        User CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        User LastModifiedBy { get; set; }
        DateTime? LastModifiedOn { get; set; }
        bool IsActive { get; set; }
    }

    public class Entity : IEntity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }

        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
