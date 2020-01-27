using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TKProcessor.Services;
using TKProcessor.Services.Maintenance;
using TKProcessor.WPF.Common;
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

        private BasicTimekeepingService<T> basicService;

        public ViewModelBase()
        {
            basicService = new BasicTimekeepingService<T>();

            View = CollectionViewSource.GetDefaultView(Items);

            Sort();

            IsEnabled = true;
        }

        public ViewModelBase(IEventAggregator eventAggregator, IWindowManager windowManager) : this()
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;

            basicService = new BasicTimekeepingService<T>();
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

        public void InitializeFilter(IEnumerable<string> columnsToFilter = null)
        {
            //Dictionary<string, object> columnValues = new Dictionary<string, string>();
            Dictionary<string, string> filterValues = new Dictionary<string, string>();

            View.Filter += (o) =>
            {
                if (string.IsNullOrEmpty(FilterString))
                    return true;

                T entity = (T)o;

                //foreach (var i in columnsToFilter)
                //{
                //    System.Reflection.PropertyInfo propInfo = typeof(T).GetProperty(i);

                //    if (propInfo.PropertyType == typeof(DateTime))
                //        columnValues.Add(i, "$date$$" + ((DateTime)propInfo.GetValue(entity)).ToString("MM/dd/yyyy h:mm tt - dddd"));
                //    else
                //        columnValues.Add(i, propInfo.GetValue(entity).ToString());
                //}

                //foreach (var i in FilterString.Split(';'))
                //{
                //    if (i.IndexOf('=') > 0)
                //    {
                //        filterValues.Add("ALL", i);
                //    }
                //    else
                //    {
                //        var values = i.Split('=');
                //        filterValues.Add(values[0], values[1]);
                //    }
                //}

                //foreach (var filterValue in filterValues)
                //{
                //    if (filterValue.Key == "ALL")
                //    {
                //        var filters = filterValue.Value.Split(',');

                //        foreach (var filter in filters)
                //        {
                //            if (filter.IndexOf('-') > 0)
                //            {
                //                var filterParts = filter.Split('-');

                //                if (DateTime.TryParse(filterParts[0].Trim(), out DateTime part1) && DateTime.TryParse(filterParts[1].Trim(), out DateTime part2))
                //                {
                //                    foreach(var propInfo in typeof(T).GetProperties().Where(propInfo => propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?)))
                //                    {

                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                bool output = false;

                foreach (var orGroup in FilterString.Split(';'))
                {
                    if (string.IsNullOrEmpty(orGroup))
                        continue;

                    bool? andGroupOut = null;

                    foreach (var andCriteria in orGroup.Split(','))
                    {
                        var criteriaFlag = false;

                        foreach (var propInfo in typeof(T).GetProperties())
                        {
                            var propValue = propInfo.GetValue(entity);
                            var propName = propInfo.Name;
                            var propValueOut = "";

                            if (propInfo.PropertyType == typeof(DateTime) || (propInfo.PropertyType == typeof(DateTime?) && propValue != null))
                            {
                                var datevalue = (DateTime)propInfo.GetValue(entity);

                                if (andCriteria.IndexOf('-') > -1)
                                {
                                    var criteria = andCriteria.Split('-');

                                    if (criteria.Length > 0)
                                    {
                                        if ((DateTime.TryParseExact(criteria[0].Trim(), "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out DateTime from) &&
                                             DateTime.TryParseExact(criteria[1].Trim(), "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out DateTime to)) ||
                                            (DateTime.TryParseExact(criteria[0].Trim(), "HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out from) &&
                                             DateTime.TryParseExact(criteria[1].Trim(), "HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out to)))
                                        {
                                            criteriaFlag = criteriaFlag || datevalue.TimeOfDay >= from.TimeOfDay && (datevalue.TimeOfDay <= to.TimeOfDay || to.TimeOfDay == new TimeSpan(0));
                                        }
                                        else if ((DateTime.TryParseExact(criteria[0].Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out from) &&
                                                  DateTime.TryParseExact(criteria[1].Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out to)) ||
                                                 (DateTime.TryParseExact(criteria[0].Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out from) &&
                                                  DateTime.TryParseExact(criteria[1].Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out to)))
                                        {
                                            criteriaFlag = criteriaFlag || (((DateTime)propInfo.GetValue(entity)) >= from && ((DateTime)propInfo.GetValue(entity)) <= to);
                                        }

                                        //if (DateTime.TryParseExact(criteria[0].Trim(), out DateTime from) && DateTime.TryParseExact(criteria[1].Trim(), out DateTime to))
                                        //{
                                        //    if (from.Date == DateTime.MinValue && to.Date == DateTime.MinValue)
                                        //    {
                                        //        criteriaFlag = criteriaFlag || (((DateTime)propInfo.GetValue(entity)).TimeOfDay >= from.TimeOfDay && ((DateTime)propInfo.GetValue(entity)).TimeOfDay <= to.TimeOfDay);
                                        //    }
                                        //    else
                                        //    {
                                        //        criteriaFlag = criteriaFlag || (((DateTime)propInfo.GetValue(entity)) >= from && ((DateTime)propInfo.GetValue(entity)) <= to);
                                        //    }
                                        //}
                                    }
                                }
                                else
                                {
                                    propValueOut = datevalue.ToString(Constants.DateFormatForFilter);

                                    criteriaFlag = criteriaFlag || propValueOut.Trim().ToUpper().Contains(andCriteria.Trim().ToUpper());
                                }
                            }
                            else
                            {
                                if (propInfo.PropertyType == typeof(bool) || (propInfo.PropertyType == typeof(bool?) && propValue != null))
                                {
                                    propValueOut = ((bool)propValue).ToString() + " || " + (((bool)propValue) ? "YES" : "NO") + " || " + (((bool)propValue) ? "Y" : "N") + " || " + (((bool)propValue) ? "TRUE" : "FALSE");
                                }
                                else if (propValue != null)
                                {
                                    propValueOut = propValue.ToString();
                                }

                                criteriaFlag = criteriaFlag || propValueOut.Trim().ToUpper().Contains(andCriteria.Trim().ToUpper());
                            }
                        }

                        if (criteriaFlag == false)
                        {
                            andGroupOut = false;
                            break;
                        }

                        andGroupOut = true;
                    }

                    output = output || (andGroupOut ?? false);
                }

                return output;
            };
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

        public virtual void HandleError(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("message", nameof(message));
            }

            basicService?.CreateErrorLog(message);

            eventAggregator.PublishOnUIThread(new NewMessageEvent((message.Length > 100) ? "An error has occured. Please see error logs for details" : message, MessageType.Error));
        }

        public virtual void HandleError(Exception ex)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            if (ex.Message.Contains("See the inner exception for details") && ex.InnerException != null)
                ex = ex.InnerException;

            basicService?.CreateErrorLog(ex);

            eventAggregator.PublishOnUIThread(new NewMessageEvent((ex.Message.Length > 100) ? "An error has occured. Please see error logs for details" : ex.Message, MessageType.Error));
        }

        public virtual void HandleError(Exception ex, string classname, [CallerMemberName]string methodname = "")
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            if (ex.Message.Contains("See the inner exception for details") && ex.InnerException != null)
                ex = ex.InnerException;

            basicService?.CreateErrorLog(ex, classname + "." + methodname);

            eventAggregator.PublishOnUIThread(new NewMessageEvent((ex.Message.Length > 100) ? "An error has occured. Please see error logs for details" : ex.Message, MessageType.Error));
        }

        public virtual void RaiseMessage(string message, int duration = 3)
        {
            eventAggregator.PublishOnUIThread(new NewMessageEvent(message, MessageType.Information, duration));
        }

        public virtual void RaiseMessage(string message, MessageType messageType, int duration = 3)
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
