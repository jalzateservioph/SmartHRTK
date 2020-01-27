using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKProcessor.WPF.Models
{
    public interface IBaseModel
    {
        Guid Id { get; set; }
        bool IsActive { get; set; }
        bool IsDirty { get; set; }
        bool IsSelected { get; set; }
        bool IsValid { get; set; }
        string CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedOn { get; set; }
    }

    public class BaseModel : PropertyChangedBase, IBaseModel
    {
        private bool _isDirty;
        private bool _isValid;
        private bool _isSelected;

        public BaseModel()
        {
            Id = Guid.NewGuid();
            IsDirty = false;
            IsActive = true;
            IsSelected = false;
        }

        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
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
