using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TKProcessor.WPF.Models;

namespace TKProcessor.WPF.ViewModels
{
    public class ViewModelBase<T> : Conductor<T>.Collection.OneActive
        where T : class, new()
    {
        protected readonly IEventAggregator eventAggregator;
        protected readonly IWindowManager windowManager;

        private string _filterString;
        private bool _isEnabled;

        public ViewModelBase()
        {
            View = CollectionViewSource.GetDefaultView(Items);

            Sort();

            IsEnabled = true;
        }

        public ViewModelBase(IEventAggregator eventAggregator, IWindowManager windowManager) : this()
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
        }

        public virtual void StartProcessing()
        {
            IsEnabled = false;
        }

        public virtual void EndProcessing()
        {
            IsEnabled = true;
        }

        public virtual void Sort()
        {

        }

        public virtual bool Filter(object o)
        {
            return true;
        }

        public virtual void InvokeFilter(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                View.Filter += Filter;
                View.Refresh();
            }
        }

        public ICollectionView View { get; }
        public string FilterString
        {
            get => _filterString;
            set
            {
                _filterString = value;

                if (AutoFilter)
                {
                    View.Filter += Filter;
                    View.Refresh();
                }
            }
        }
        public bool AutoFilter { get; set; }
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange();
            }
        }
    }

    public class EditableViewModelBase<T> : ViewModelBase<T>
        where T : class, new()
    {
        private bool _isEditing;

        public EditableViewModelBase(IEventAggregator eventAggregator, IWindowManager windowManager) : base(eventAggregator, windowManager)
        {
            IsEditing = false;
        }

        public virtual void StartEditing()
        {
            IsEditing = true;
        }

        public virtual void EndEditing()
        {
            IsEditing = false;
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
