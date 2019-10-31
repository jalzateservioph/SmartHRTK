using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public class BaseModel : PropertyChangedBase
    {
        private bool _isDirty;
        private bool _isValid;
        private bool _isSelected;

        public BaseModel()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public User LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public virtual bool IsDirty
        {
            get => _isDirty;
            set
            {
                _isDirty = value;
                NotifyOfPropertyChange();
            }
        }
        public virtual bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                NotifyOfPropertyChange();
            }
        }
        public virtual bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
