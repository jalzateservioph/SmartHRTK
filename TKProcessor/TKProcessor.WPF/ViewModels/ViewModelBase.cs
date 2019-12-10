using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TKProcessor.WPF.Events;
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
        private bool _isCheckedAll;

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

        public virtual void HandleError(Exception ex)
        {
            eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
        }

        public virtual void HandleError(string message)
        {
            eventAggregator.PublishOnUIThread(new NewMessageEvent(message, MessageType.Error));
        }

        public virtual void RaiseMessage(string message, MessageType messageType = MessageType.Information, int duration = 3)
        {
            eventAggregator.PublishOnUIThread(new NewMessageEvent(message, messageType, duration));
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

        public bool IsCheckedAll
        {
            get => _isCheckedAll;
            set
            {
                _isCheckedAll = value;
                NotifyOfPropertyChange();
            }
        }
    }

    public class ViewModel<T> : Conductor<T>.Collection.OneActive
        where T : class, new()
    {
        protected readonly IEventAggregator eventAggregator;
        protected readonly IWindowManager windowManager;

        private bool isFormEnabled;

        public ViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;
        }

        public virtual void HandleError(Exception ex)
        {
            eventAggregator.PublishOnUIThread(new NewMessageEvent(ex.Message, MessageType.Error));
        }

        public virtual void RaiseGlobalMessage(string message, MessageType messageType = MessageType.Information, int duration = 3)
        {
            eventAggregator.PublishOnUIThread(new NewMessageEvent(message, messageType, duration));
        }

        public bool IsFormEnabled
        {
            get => isFormEnabled;
            set
            {
                isFormEnabled = value;
                NotifyOfPropertyChange();
            }
        }
    }

    public class DialogViewModelBase<T> : PropertyChangedBase
    {
        private readonly Dictionary<string, object> modalCollection;

        private bool isModalShown;

        private object activeModal;

        public DialogViewModelBase()
        {
            modalCollection = new Dictionary<string, object>();
        }

        public void AddModal(string key, object modal)
        {
            modalCollection[key] = modal;
        }

        public virtual void ShowModal(string key)
        {
            ActiveModal = modalCollection[key];

            IsModalShown = true;
        }

        public virtual void CloseModal()
        {
            IsModalShown = false;
        }

        public bool IsModalShown
        {
            get => isModalShown;
            set
            {
                isModalShown = value;
                NotifyOfPropertyChange();
            }
        }

        public object ActiveModal
        {
            get => activeModal;
            set
            {
                activeModal = value;
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
